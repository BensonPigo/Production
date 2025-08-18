using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P33 : Win.Tems.Input8
    {
        private StringBuilder sbSizecode;
        private DataTable dtSizeCode = null;
        private DataTable dtIssueBreakDown = null;
        private bool Ismatrix_Reload = true; // 是否需要重新抓取資料庫資料
        private string poid = string.Empty;

        /// <inheritdoc/>
        public P33(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.gridicon.Location = new Point(this.btnBreakDown.Location.X + 20, 95); // 此gridcon位置會跑掉，需強制設定gridcon位置
            this.DefaultFilter = $"MDivisionID='{Env.User.Keyword}' AND Type='E' ";

            this.WorkAlias = "Issue";                        // PK: ID
            this.GridAlias = "Issue_Summary";           // PK: ID+UKey
            this.SubGridAlias = "Issue_Detail";          // PK: ID+Issue_SummaryUkey+FtyInventoryUkey

            this.KeyField1 = "ID"; // Issue PK
            this.KeyField2 = "ID"; // Summary FK

            // SubKeyField1 = "Ukey";    // 將第2層的PK欄位傳給第3層的FK。
            this.SubKeyField1 = "ID";    // 將第2層的PK欄位傳給第3層的FK。
            this.SubKeyField2 = "Ukey";  // 將第2層的PK欄位傳給第3層的FK。

            this.SubDetailKeyField1 = "id,Ukey";    // second PK
            this.SubDetailKeyField2 = "id,Issue_SummaryUkey"; // third FK

            this.DoSubForm = new P33_Detail();
        }

        /// <inheritdoc/>
        public P33(ToolStripMenuItem menuitem, string transID)
            : this(menuitem)
        {
            this.DefaultFilter = string.Format("Type = 'E' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;

            this.WorkAlias = "Issue";                        // PK: ID
            this.GridAlias = "Issue_summary";           // PK: ID+UKey
            this.SubGridAlias = "Issue_detail";          // PK: ID+Issue_SummaryUkey+FtyInventoryUkey

            this.KeyField1 = "ID"; // Issue PK
            this.KeyField2 = "ID"; // Summary FK

            // SubKeyField1 = "Ukey";    // 將第2層的PK欄位傳給第3層的FK。
            this.SubKeyField1 = "ID";    // 將第2層的PK欄位傳給第3層的FK。
            this.SubKeyField2 = "Ukey";  // 將第2層的PK欄位傳給第3層的FK。

            this.SubDetailKeyField1 = "id,Ukey";    // second PK
            this.SubDetailKeyField2 = "id,Issue_SummaryUkey"; // third FK

            this.DoSubForm = new P33_Detail();
        }

        #region Form事件

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.dgToPlace.SetDefalutIndex();
            bool isAutomationEnable = Automation.UtilityAutomation.IsAutomationEnable;
            this.dgToPlace.Visible = isAutomationEnable;
            this.lblToPlace.Visible = isAutomationEnable;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            bool isAutomationEnable = Automation.UtilityAutomation.IsAutomationEnable;
            this.dgToPlace.Visible = isAutomationEnable;
            this.lblToPlace.Visible = isAutomationEnable;

            // this.labelConfirmed.Visible = MyUtility.Check.Empty(this.CurrentMaintain["ID"]) ? false : true;
            this.labelConfirmed.Text = this.CurrentMaintain["status"].ToString();

            if (!MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.txtOrderID.IsSupportEditMode = false;
                this.txtOrderID.ReadOnly = true;
            }
            else
            {
                this.txtOrderID.IsSupportEditMode = true;
                this.txtOrderID.ReadOnly = false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                this.CurrentMaintain["IssueDate"] = DateTime.Now;
            }

            string orderID = this.CurrentMaintain["OrderID"].ToString();

            this.RefreshOrderField(orderID);

            this.displayLineNo.Text = MyUtility.GetValue.Lookup($@"
SELECT t.sewline + ',' 
FROM(SELECT DISTINCT o.sewline FROM dbo.issue_detail a WITH (nolock) 
INNER JOIN dbo.orders o WITH (nolock) ON a.poid = o.poid  
WHERE o.id = '{orderID}' AND o.sewline != '') t FOR xml path('')
");

            #region -- matrix breakdown
            this.RenewData();
            DualResult result;
            if (!(result = this.Matrix_Reload()))
            {
                this.ShowErr(result);
            }
            #endregion

            // System.Automation=1 和confirmed 且 有P99 Use 權限的人才可以看到此按紐
            if (UtilityAutomation.IsAutomationEnable && (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED") &&
                MyUtility.Check.Seek($@"
select * from Pass1
where (FKPass0 in (select distinct FKPass0 from Pass2 where BarPrompt = 'P99. Send to WMS command Status' and Used = 'Y') or IsMIS = 1 or IsAdmin = 1)
and ID = '{Sci.Env.User.UserID}'"))
            {
                this.btnCallP99.Visible = true;
            }
            else
            {
                this.btnCallP99.Visible = false;
            }
        }

        private void RefreshOrderField(string orderID)
        {
            DataRow drOrder;
            string sqlGetOrder = $"SELECT POID,StyleID FROm Orders WHERE ID='{orderID}' ";
            if (MyUtility.Check.Seek(sqlGetOrder, out drOrder))
            {
                this.displayPOID.Text = drOrder["POID"].ToString();
                this.displayStyle.Text = drOrder["StyleID"].ToString();
            }
            else
            {
                this.displayPOID.Text = string.Empty;
                this.displayStyle.Text = string.Empty;
            }

            this.poid = this.displayPOID.Text;
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            string orderID = (e.Master == null) ? string.Empty : e.Master["OrderID"].ToString();

            this.Ismatrix_Reload = true;
            this.DetailSelectCommand = $@"

----------------Quiting用量計算--------------------------
WITH tmpQT as (
    select P.ID,OQ.Article,OQ.SizeCode,OCC.FabricPanelCode,OCC.FabricCode
    ,OE.ConsPC
    ,[Number of Needle for QT] =  ceiling(sqh.Width/sqh.HSize/sqh.NeedleDistance)
    from Orders O
    Inner join Order_Qty OQ on O.ID=OQ.ID
    Inner join PO P on O.POID=P.ID
    inner join Order_ColorCombo OCC on O.POID=OCC.Id and OQ.Article=OCC.Article and OCC.FabricCode is not null and OCC.FabricCode !=''
    inner join Order_EachCons oe WITH (NOLOCK) on oe.id = occ.id and oe.FabricCombo = occ.PatternPanel and oe.CuttingPiece = 0 
    inner join Order_EachCons_SizeQty OEZ on OE.Ukey=OEZ.Order_EachConsUkey and OQ.SizeCode=OEZ.SizeCode 
    Inner join Style_QTThreadColorCombo_History SQH on O.styleUkey=SQH.styleUkey and SQH.FabricCode=OE.FabricCode and SQH.FabricPanelCode=OE.FabricPanelCode and P.ThreadVersion=SQH.Version
    where o.ID = '{orderID}'

)
, tmpQTFinal as (
    select O.ID,OQ.Article,OQ.SizeCode,OCC.FabricPanelCode,OCC.FabricCode
    ,SQHD.Seq,SQHD.SCIRefno,SQHD.ColorID,QTt.Val
    from Orders O
    Inner join Order_Qty OQ on O.ID=OQ.ID
    Inner join PO P on O.POID=P.ID
    inner join Order_ColorCombo OCC on O.POID=OCC.Id and OQ.Article=OCC.Article and OCC.FabricCode is not null and OCC.FabricCode !=''
    Inner join Style_QTThreadColorCombo_History SQH on O.styleUkey=SQH.styleUkey and SQH.FabricCode=OCC.FabricCode and SQH.FabricPanelCode=OCC.FabricPanelCode and P.ThreadVersion=SQH.Version
    Inner join Style_QTThreadColorCombo_History_Detail SQHD on SQH.Ukey=SQHD.Style_QTThreadColorCombo_HistoryUkey and OQ.Article=SQHD.Article
    OUTER APPLY(
	    SELECT Val=[Number of Needle for QT]* SQHD.Ratio * 0.9144 * qt.ConsPC * s.Qty
	    FROM tmpQT qt
	    LEFT JOIN (
		    SELECT Article, SizeCode,Qty=SUM(Qty)
		    FROM Issue_Breakdown
		    WHERE ID='{masterID}' AND OrderID='{orderID}'
		    GROUP BY Article, SizeCode
	    )
	        s ON qt.Article = s.Article AND qt.SizeCode = s.SizeCode
	    WHERE qt.ID=o.ID AND qt.Article=oq.Article AND qt.SizeCode=oq.SizeCode
	    AND qt.FabricPanelCode=OCC.FabricPanelCode AND qt.FabricCode =OCC.FabricCode
    )QTt
    where o.ID = '{orderID}'
)
----------------Quiting用量計算--------------------------

--------------------------------------------------------

----  先By Article撈取資料再加總
, BreakdownByArticle as (

    SELECT   DISTINCT
              iis.SCIRefno
            , [Refno]=Refno.Refno
            , iis.ColorID
		    , iis.SuppColor
		    , f.DescDetail
		    , [@Qty]= ISNULL(ThreadUsedQtyByBOT.Val,0) + ISNULL(QT.Val,0)
		    , [AccuIssued] = (
					    select isnull(sum([IS].qty),0)
					    from dbo.issue I2 WITH (NOLOCK) 
					    inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I2.id = [IS].Id 
					    where I2.type = 'E' and I2.Status = 'Confirmed' 
					    and [IS].Poid=iis.POID AND [IS].SCIRefno=iis.SCIRefno AND [IS].ColorID=iis.ColorID and i2.[EditDate]<I.AddDate AND i2.ID <> i.ID
				    )
		    , [IssueQty]=iis.Qty
		    , [Use Qty By Stock Unit] = CEILING( ISNULL(ThreadUsedQtyByBOT.Qty,0) * (ISNULL(ThreadUsedQtyByBOT.Val,0) + ISNULL(QT.Val,0)) / 100 * ISNULL(UnitRate.RateValue,1))
		    , [Stock Unit]=StockUnit.StockUnit

		    , [Use Unit]='CM'
		    , [Use Qty By Use Unit]=(ThreadUsedQtyByBOT.Qty * (ISNULL(ThreadUsedQtyByBOT.Val,0) + ISNULL(QT.Val,0)) )

		    , [Stock Unit Desc.]=StockUnit.Description
		    , [OutputQty] = ISNULL(ThreadUsedQtyByBOT.Qty,0)
		    , [Balance(Stock Unit)]= 0
		    , [Location]=''
            , [POID]=iis.POID
            , i.MDivisionID
            , i.ID
            , iis.Ukey
    FROM Issue i 
    INNER JOIN Issue_Summary iis ON i.ID= iis.Id
    LEFT JOIN Issue_Detail isd ON isd.Issue_SummaryUkey=iis.Ukey
    LEFT JOIN Fabric f ON f.SCIRefno = iis.SCIRefno
    OUTER APPLY(
	    SELECT DISTINCT Refno
	    FROM PO_Supp_Detail psd
        left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
	    WHERE psd.ID = iis.POID AND psd.SCIRefno = iis.SCIRefno 
	    AND isnull(psdsC.SpecValue, '')=iis.ColorID
    )Refno
    OUTER APPLY(
	    SELECT SCIRefNo
		    ,ColorID
		    ,[Val]=sum (g.OpThreadQty)
		    ,[Qty] = (	
			    SELECt [Qty]=SUM(b.Qty)
			    FROM (
					    Select distinct o.ID,tcd.SCIRefNo, tcd.ColorID ,tcd.Article 
					    From dbo.Orders as o
					    Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
                        INNER JOIN PO WITH (NOLOCK) ON po.StyleUkey = o.StyleUkey
					    Inner Join dbo.Style_ThreadColorCombo_History as tc On tc.StyleUkey = s.Ukey AND po.ThreadVersion = tc.Version
					    Inner Join dbo.Style_ThreadColorCombo_History_Detail as tcd On tcd.Style_ThreadColorCombo_HistoryUkey = tc.Ukey
					    WHERE O.ID=iis.POID AND tcd.Article IN ( SELECT Article FROM Issue_Breakdown WHERE ID=i.ID)
					    ) a
			    INNER JOIN (		
							    SELECt Article,[Qty]=SUM(Qty) 
							    FROM Issue_Breakdown
							    WHERE ID=i.ID
							    GROUP BY Article
						    ) b ON a.Article = b.Article
			    WHERE SCIRefNo=iis.SCIRefNo AND  ColorID= iis.ColorID AND a.Article=g.Article
			    GROUP BY a.Article
		    )
	    FROM DBO.GetThreadUsedQtyByBOT(iis.POID,default) g
	    WHERE SCIRefNo= iis.SCIRefNo AND ColorID = iis.ColorID  
	    AND Article IN (		
            SELECt Article
            FROM Issue_Breakdown
            WHERE ID=i.ID
	    )
	    GROUP BY SCIRefNo,ColorID , Article
    )ThreadUsedQtyByBOT
    OUTER APPLY(
	    SELECT Val = SUM(t.Val)
	    FROM tmpQTFinal　ｔ
	    WHERE t.SCIRefNo=iis.SCIRefno AND t.ColorID=iis.ColorID 
    )QT
    OUTER APPLY(
	    SELECT TOP 1 psd2.StockUnit ,u.Description
	    FROM PO_Supp_Detail psd2
        left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd2.id and psdsC.seq1 = psd2.seq1 and psdsC.seq2 = psd2.seq2 and psdsC.SpecColumnID = 'Color'
	    LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	    WHERE psd2.ID = i.OrderId 
	    AND psd2.SCIRefno = iis.SCIRefno 
	    AND isnull(psdsC.SpecValue, '') = iis.ColorID
    )StockUnit
    OUTER APPLY(
	    SELECT RateValue = IIF(Denominator = 0,0, Numerator / Denominator)
	    FROM Unit_Rate
	    WHERE UnitFrom='M' and  UnitTo = StockUnit.StockUnit
    )UnitRate
    WHERE i.ID='{masterID}' 
    AND iis.ColorID <> ''
)

, Final as(

	SELECt SCIRefno
		, Refno
        , ColorID
		, SuppColor
		, DescDetail
		, [@Qty] = SUM([@Qty])
		, [AccuIssued]
		, [IssueQty]
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
		, [IssueQty]
		, [Stock Unit]
		, [Use Unit]
		, [Stock Unit Desc.]
		, [Balance(Stock Unit)]
		, [Location]
		, [POID]
		, MDivisionID
		, ID
		, Ukey
)

SELECt SCIRefno
		, Refno
        , ColorID
		, SuppColor
		, DescDetail
		, [@Qty]
		, [AccuIssued]
		, [IssueQty]
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
	SELECT [Qty]=ISNULL(( SUM(Fty.InQty - Fty.OutQty + Fty.AdjustQty - Fty.ReturnQty)) ,0)
	FROM PO_Supp_Detail psd 
	LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
    left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
	WHERE psd.SCIRefno=t.SCIRefno AND isnull(psdsC.SpecValue, '')=t.ColorID AND psd.ID='{this.poid}'
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
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override DualResult ConvertSubDetailDatasFromDoSubForm(SubDetailConvertFromEventArgs e)
        {
            Sum_subDetail(e.Detail, e.SubDetails);

            DataTable dt;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (this.GetSubDetailDatas(dr, out dt))
                {
                    Sum_subDetail(dr, dt);
                }
            }

            return base.ConvertSubDetailDatasFromDoSubForm(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region Refno事件
            DataGridViewGeneratorTextColumnSettings refnoSet = new DataGridViewGeneratorTextColumnSettings();
            refnoSet.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    if (this.Is_IssueBreakDownEmpty())
                    {
                        MyUtility.Msg.InfoBox("Issue Breakdown can't be empty !!");
                        return;
                    }

                    DataTable bulkItems;
                    string colorID = MyUtility.Convert.GetString(this.CurrentDetailData["ColorID"]);
                    string refno = MyUtility.Convert.GetString(this.CurrentDetailData["Refno"]);
                    string sCIRefno = MyUtility.Convert.GetString(this.CurrentDetailData["SCIRefno"]);

                    #region 選單SQL
                    string sqlcmd = $@"
 SELECT   DISTINCT [Refno]= psd.Refno
		 , [ColorID]=isnull(psdsC.SpecValue, '')
		 , SuppColor = SuppCol.Val
		 , [MtlType]=fc.MtlTypeID
		 , [Desc]=fc.DescDetail
		 , [Stock Unit]=StockUnit.Val
		 , [UnitDesc]=StockUnit.Description
		 , [BulkQty]=BulkQty.Val
		 , [InventoryQty]=InventoryQty.Val
		 , psd.SCIRefno
INTO #tmp
FROM PO_Supp_Detail psd
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
LEFT JOIN Fabric fc ON fc.SCIRefno = psd.SCIRefno
LEFT JOIN MtlType m on m.id= fc.MtlTypeID
 OUTER APPLY(
	 SELECT TOP 1 [Val] = psd2.StockUnit  ,u.Description
	 FROM PO_Supp_Detail psd2 
	 LEFT JOIN Unit u ON u.ID = psd2.StockUnit
     left join PO_Supp_Detail_Spec psdsC2 WITH (NOLOCK) on psdsC2.ID = psd2.id and psdsC2.seq1 = psd2.seq1 and psdsC2.seq2 = psd2.seq2 and psdsC2.SpecColumnID = 'Color'
	 WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND isnull(psdsC2.SpecValue, '')= isnull(psdsC.SpecValue, '')
 )StockUnit
 OUTER APPLY(
	SELECT [Val]=(f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty) 
	FROM PO_Supp_Detail psd2
	INNER JOIN FtyInventory F ON F.POID=psd2.ID AND F.SEQ1= psd2.SEQ1 AND F.SEQ2 = psd2.SEQ2
     left join PO_Supp_Detail_Spec psdsC2 WITH (NOLOCK) on psdsC2.ID = psd2.id and psdsC2.seq1 = psd2.seq1 and psdsC2.seq2 = psd2.seq2 and psdsC2.SpecColumnID = 'Color'
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND isnull(psdsC2.SpecValue, '')=isnull(psdsC.SpecValue, '') AND F.StockType='B' {(MyUtility.Check.Empty(colorID) ? "AND isnull(psdsC2.SpecValue, '') <> ''" : $"AND isnull(psdsC2.SpecValue, '')='{colorID}'")}
 )BulkQty
 OUTER APPLY(
	SELECT [Val]=(f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty) 
	FROM PO_Supp_Detail psd2
	INNER JOIN FtyInventory F ON F.POID=psd2.ID AND F.SEQ1= psd2.SEQ1 AND F.SEQ2 = psd2.SEQ2
     left join PO_Supp_Detail_Spec psdsC2 WITH (NOLOCK) on psdsC2.ID = psd2.id and psdsC2.seq1 = psd2.seq1 and psdsC2.seq2 = psd2.seq2 and psdsC2.SpecColumnID = 'Color'
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND isnull(psdsC2.SpecValue, '')=isnull(psdsC.SpecValue, '') AND F.StockType='I' {(MyUtility.Check.Empty(colorID) ? "AND isnull(psdsC2.SpecValue, '') <> ''" : $"AND isnull(psdsC2.SpecValue, '')='{colorID}'")}
 )InventoryQty
OUTER APPLY(
	----列出所有Seq1 Seq2對應到的SuppColor
	SELECT [Val]=STUFF((
		SELECT  DISTINCT ',' + SuppColor
		FROM PO_Supp_Detail y
        left join PO_Supp_Detail_Spec psdsCy WITH (NOLOCK) on psdsCy.ID = y.id and psdsCy.seq1 = y.seq1 and psdsCy.seq2 = y.seq2 and psdsCy.SpecColumnID = 'Color'
		WHERE EXISTS( 
			SELECT 1 
			FROM PO_Supp_Detail t 
			LEFT JOIN FtyInventory Fty ON  Fty.poid = t.ID AND Fty.seq1 = t.seq1 AND Fty.seq2 = t.seq2 AND fty.StockType='B'
            left join PO_Supp_Detail_Spec psdsCt WITH (NOLOCK) on psdsCt.ID = t.id and psdsCt.seq1 = t.seq1 and psdsCt.seq2 = t.seq2 and psdsCt.SpecColumnID = 'Color'
			WHERE psd.SCIRefno=t.SCIRefno AND isnull(psdsC.SpecValue, '') = isnull(psdsCt.SpecValue, '') AND t.ID = '{this.poid}'
			AND t.SCIRefno = y.SCIRefno AND isnull(psdsCt.SpecValue, '') = isnull(psdsCy.SpecValue, '') AND t.ID = y.ID
			AND t.SEQ1 = y.SEQ1 AND t.SEQ2 = y.SEQ2
		)
		FOR XML PATH('')
	),1,1,'')
)SuppCol
WHERE psd.ID='{this.poid}' 
AND m.IsThread=1 
AND psd.FabricType ='A'
";
                    if (!MyUtility.Check.Empty(sCIRefno))
                    {
                        sqlcmd += $"AND psd.SCIRefno='{sCIRefno}' ";
                    }
                    else
                    {
                        sqlcmd += $"AND psd.SCIRefno <> '' ";
                    }

                    if (!MyUtility.Check.Empty(refno))
                    {
                        sqlcmd += $"AND psd.Refno='{refno}' ";
                    }
                    else
                    {
                        sqlcmd += $"AND psd.Refno <> '' ";
                    }

                    if (!MyUtility.Check.Empty(colorID))
                    {
                        sqlcmd += $"AND isnull(psdsC.SpecValue, '')='{colorID}' ";
                    }
                    else
                    {
                        sqlcmd += $"AND isnull(psdsC.SpecValue, '') <> '' ";
                    }

                    sqlcmd += $@"
SELECT [Refno]
		 , [ColorID]
         , [SuppColor]
		 , [MtlType]
		 , [Desc]
		 , [Stock Unit]
		 , [UnitDesc]
		 , [BulkQty]=SUM(BulkQty)
		 , [InventoryQty]=SUM(InventoryQty)
         , [SCIRefno]
FROM #tmp
GROUP BY  [Refno]
		 , [ColorID]
         , [SuppColor]
		 , [MtlType]
		 , [Desc]
		 , [Stock Unit]
		 , [UnitDesc]
         , [SCIRefno]

DROP TABLE #tmp

";
                    #endregion

                    IList<DataRow> selectedDatas;
                    DualResult result2;
                    if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out bulkItems)))
                    {
                        this.ShowErr(sqlcmd, result2);
                        return;
                    }

                    Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(
                        bulkItems,
                        "SCIRefno,Refno,ColorID,SuppColor,MtlType,Desc,Stock Unit,UnitDesc,BulkQty,InventoryQty",
                        "25,15,5,10,45,5,15,10,10",
                        this.CurrentDetailData["Refno"].ToString(),
                        "SCIRefno,Refno,ColorID,SuppColor,MtlType,Desc,Stock Unit,UnitDesc,BulkQty,InventoryQty");
                    selepoitem.Width = 1250;
                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    selectedDatas = selepoitem.GetSelecteds();

                    this.CurrentDetailData["SCIRefno"] = selectedDatas[0]["SCIRefno"];
                    this.CurrentDetailData["Refno"] = selectedDatas[0]["Refno"];
                    this.CurrentDetailData["ColorID"] = selectedDatas[0]["ColorID"];
                    this.CurrentDetailData["SuppColor"] = selectedDatas[0]["SuppColor"];

                    sCIRefno = this.CurrentDetailData["SCIRefno"].ToString();
                    refno = this.CurrentDetailData["Refno"].ToString();
                    colorID = this.CurrentDetailData["ColorID"].ToString();
                    this.CurrentDetailData["POID"] = this.poid;

                    if (this.dtIssueBreakDown == null)
                    {
                        return;
                    }

                    // 將IssueBreakDown整理成Datatable
                    DataTable unPivotBrkQty = this.UnPivotBrkQty();
                    this.RefnoCellValidating_QT(MyUtility.Convert.GetString(this.CurrentDetailData["Refno"]), unPivotBrkQty, null);
                }
            };
            refnoSet.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (string.Compare(e.FormattedValue.ToString().Trim(), this.CurrentDetailData["Refno"].ToString().Trim()) != 0)
                {
                    if (this.Is_IssueBreakDownEmpty())
                    {
                        MyUtility.Msg.InfoBox("Issue Breakdown can't be empty !!");
                        return;
                    }

                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.CurrentDetailData["Refno"] = string.Empty;
                        this.CurrentDetailData["SCIRefno"] = string.Empty;

                        // CurrentDetailData["SuppColor"] = "";
                        this.CurrentDetailData["POID"] = string.Empty;
                        this.CurrentDetailData["DescDetail"] = string.Empty;
                        this.CurrentDetailData["@Qty"] = DBNull.Value;
                        this.CurrentDetailData["Use Unit"] = string.Empty;
                        this.CurrentDetailData["AccuIssued"] = DBNull.Value;

                        this.CurrentDetailData["IssueQty"] = DBNull.Value;
                        this.CurrentDetailData["Use Qty By Stock Unit"] = DBNull.Value;
                        this.CurrentDetailData["Stock Unit"] = string.Empty;
                        this.CurrentDetailData["Use Qty By Use Unit"] = DBNull.Value;
                        this.CurrentDetailData["Use Unit"] = string.Empty;
                        this.CurrentDetailData["Stock Unit Desc."] = string.Empty;
                        this.CurrentDetailData["OutputQty"] = DBNull.Value;
                        this.CurrentDetailData["Balance(Stock Unit)"] = DBNull.Value;
                        this.CurrentDetailData["Location"] = string.Empty;
                        this.CurrentDetailData["MDivisionID"] = string.Empty;
                    }
                    else
                    {
                        if (this.dtIssueBreakDown == null)
                        {
                            return;
                        }

                        // 將IssueBreakDown整理成Datatable
                        DataTable unPivotBrkQty = this.UnPivotBrkQty();
                        this.RefnoCellValidating_QT(MyUtility.Convert.GetString(e.FormattedValue), unPivotBrkQty, e);
                    }
                }
            };
            #endregion

            #region Color事件
            DataGridViewGeneratorTextColumnSettings colorSet = new DataGridViewGeneratorTextColumnSettings();
            colorSet.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    if (this.Is_IssueBreakDownEmpty())
                    {
                        MyUtility.Msg.InfoBox("Issue Breakdown can't be empty !!");
                        return;
                    }

                    DataTable bulkItems;
                    string sCIRefno = MyUtility.Convert.GetString(this.CurrentDetailData["SCIRefno"]);
                    string refno = MyUtility.Convert.GetString(this.CurrentDetailData["Refno"]);
                    string colorID = MyUtility.Convert.GetString(this.CurrentDetailData["ColorID"]);

                    #region 取得選單SQL
                    string sqlcmd = $@"
 SELECT  DISTINCT  [Refno]= psd.Refno
		 , [ColorID]=isnull(psdsC.SpecValue, '')
		 , SuppColor = SuppCol.Val
		 , [MtlType]=fc.MtlTypeID
		 , [Desc]=fc.DescDetail
		 , [Stock Unit]=StockUnit.Val
		 , [UnitDesc]=StockUnit.Description
		 , [BulkQty]=BulkQty.Val
		 , [InventoryQty]=InventoryQty.Val
         , psd.SCIRefno
INTO #tmp
 FROM PO_Supp_Detail psd
 LEFT JOIN Fabric fc ON fc.SCIRefno = psd.SCIRefno
LEFT JOIN MtlType m on m.id= fc.MtlTypeID
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
 OUTER APPLY(
	 SELECT TOP 1 [Val] = psd2.StockUnit  ,u.Description
	 FROM PO_Supp_Detail psd2 
	 LEFT JOIN Unit u ON u.ID = psd2.StockUnit
     left join PO_Supp_Detail_Spec psdsC2 WITH (NOLOCK) on psdsC2.ID = psd2.id and psdsC2.seq1 = psd2.seq1 and psdsC2.seq2 = psd2.seq2 and psdsC2.SpecColumnID = 'Color'
	 WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND isnull(psdsC2.SpecValue, '')= isnull(psdsC.SpecValue, '')
 )StockUnit
 OUTER APPLY(
	SELECT [Val]=(f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty) 
	FROM PO_Supp_Detail psd2
	INNER JOIN FtyInventory F ON F.POID=psd2.ID AND F.SEQ1= psd2.SEQ1 AND F.SEQ2 = psd2.SEQ2
     left join PO_Supp_Detail_Spec psdsC2 WITH (NOLOCK) on psdsC2.ID = psd2.id and psdsC2.seq1 = psd2.seq1 and psdsC2.seq2 = psd2.seq2 and psdsC2.SpecColumnID = 'Color'
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND isnull(psdsC2.SpecValue, '')= isnull(psdsC.SpecValue, '') AND F.StockType='B' {(MyUtility.Check.Empty(refno) ? "AND psd2.Refno <> ''" : $"AND psd2.Refno='{refno}'")}
 )BulkQty
 OUTER APPLY(
	SELECT [Val]=(f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty) 
	FROM PO_Supp_Detail psd2
	INNER JOIN FtyInventory F ON F.POID=psd2.ID AND F.SEQ1= psd2.SEQ1 AND F.SEQ2 = psd2.SEQ2
     left join PO_Supp_Detail_Spec psdsC2 WITH (NOLOCK) on psdsC2.ID = psd2.id and psdsC2.seq1 = psd2.seq1 and psdsC2.seq2 = psd2.seq2 and psdsC2.SpecColumnID = 'Color'
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND isnull(psdsC2.SpecValue, '')= isnull(psdsC.SpecValue, '') AND F.StockType='I' {(MyUtility.Check.Empty(refno) ? "AND psd2.Refno <> ''" : $"AND psd2.Refno='{refno}'")}
 )InventoryQty
OUTER APPLY(
	----列出所有Seq1 Seq2對應到的SuppColor
	SELECT [Val]=STUFF((
		SELECT  DISTINCT ',' + SuppColor
		FROM PO_Supp_Detail y
        left join PO_Supp_Detail_Spec psdsCy WITH (NOLOCK) on psdsCy.ID = y.id and psdsCy.seq1 = y.seq1 and psdsCy.seq2 = y.seq2 and psdsCy.SpecColumnID = 'Color'
		WHERE EXISTS( 
			SELECT 1 
			FROM PO_Supp_Detail t 
			LEFT JOIN FtyInventory Fty ON  Fty.poid = t.ID AND Fty.seq1 = t.seq1 AND Fty.seq2 = t.seq2 AND fty.StockType='B'
            left join PO_Supp_Detail_Spec psdsCt WITH (NOLOCK) on psdsCt.ID = t.id and psdsCt.seq1 = t.seq1 and psdsCt.seq2 = t.seq2 and psdsCt.SpecColumnID = 'Color'
			WHERE psd.SCIRefno=t.SCIRefno AND isnull(psdsC.SpecValue, '') = isnull(psdsCt.SpecValue, '') AND t.ID = '{this.poid}'
			AND t.SCIRefno = y.SCIRefno AND isnull(psdsCt.SpecValue, '') = isnull(psdsCy.SpecValue, '') AND t.ID = y.ID
			AND t.SEQ1 = y.SEQ1 AND t.SEQ2 = y.SEQ2
		)
		FOR XML PATH('')
	),1,1,'')
)SuppCol
 WHERE psd.ID='{this.poid}'
 AND m.IsThread=1 
AND psd.FabricType ='A'
AND isnull(psdsC.SpecValue, '') <> ''
";
                    if (!MyUtility.Check.Empty(sCIRefno))
                    {
                        sqlcmd += $"AND psd.SCIRefno='{sCIRefno}' ";
                    }
                    else
                    {
                        sqlcmd += $"AND psd.SCIRefno <> '' ";
                    }

                    if (!MyUtility.Check.Empty(refno))
                    {
                        sqlcmd += $"AND psd.Refno='{refno}' ";
                    }
                    else
                    {
                        sqlcmd += $"AND psd.Refno <> '' ";
                    }

                    if (!MyUtility.Check.Empty(colorID))
                    {
                        sqlcmd += $"AND isnull(psdsC.SpecValue, '') ='{colorID}' ";
                    }
                    else
                    {
                        sqlcmd += $"AND isnull(psdsC.SpecValue, '') <> '' ";
                    }

                    sqlcmd += $@"
SELECT [Refno]
		 , [ColorID]
		 , [SuppColor]
		 , [MtlType]
		 , [Desc]
		 , [Stock Unit]
		 , [UnitDesc]
		 , [BulkQty]=SUM(BulkQty)
		 , [InventoryQty]=SUM(InventoryQty)
         , [SCIRefno]
FROM #tmp
GROUP BY  [Refno]
		 , [ColorID]
		 , [SuppColor]
		 , [MtlType]
		 , [Desc]
		 , [Stock Unit]
		 , [UnitDesc]
         , [SCIRefno]

DROP TABLE #tmp

";

                    #endregion

                    IList<DataRow> selectedDatas;
                    DualResult result2;
                    if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out bulkItems)))
                    {
                        this.ShowErr(sqlcmd, result2);
                        return;
                    }

                    Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(
                        bulkItems,
                        "SCIRefno,Refno,ColorID,SuppColor,MtlType,Desc,Stock Unit,UnitDesc,BulkQty,InventoryQty",
                        "25,15,5,10,45,5,15,10,10",
                        this.CurrentDetailData["Refno"].ToString(),
                        "SCIRefno,Refno,ColorID,SuppColor,MtlType,Desc,Stock Unit,UnitDesc,BulkQty,InventoryQty");
                    selepoitem.Width = 1250;
                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    selectedDatas = selepoitem.GetSelecteds();

                    this.CurrentDetailData["SCIRefno"] = selectedDatas[0]["SCIRefno"];
                    this.CurrentDetailData["Refno"] = selectedDatas[0]["Refno"];
                    this.CurrentDetailData["ColorID"] = selectedDatas[0]["ColorID"];
                    this.CurrentDetailData["SuppColor"] = selectedDatas[0]["SuppColor"];

                    sCIRefno = this.CurrentDetailData["SCIRefno"].ToString();
                    refno = this.CurrentDetailData["Refno"].ToString();
                    colorID = this.CurrentDetailData["ColorID"].ToString();
                    this.CurrentDetailData["POID"] = this.poid;

                    if (this.dtIssueBreakDown == null)
                    {
                        return;
                    }

                    // 將IssueBreakDown整理成Datatable
                    DataTable unPivotBrkQty = this.UnPivotBrkQty();

                    // 取得預設帶入
                    #region 取得預設帶入
                    sqlcmd = $@"
SELECt Article,[Qty]=SUM(Qty) 
INTO #tmp_sumQty
FROm #tmp
GROUP BY Article

Select distinct o.ID,tcd.SCIRefNo, tcd.ColorID ,tcd.Article 
INTO #step1
From dbo.Orders as o
INNER JOIN PO WITH (NOLOCK) ON po.StyleUkey = o.StyleUkey
Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
Inner Join dbo.Style_ThreadColorCombo_History as tc On tc.StyleUkey = s.Ukey AND po.ThreadVersion = tc.Version
Inner Join dbo.Style_ThreadColorCombo_History_Detail as tcd On tcd.Style_ThreadColorCombo_HistoryUkey  = tc.Ukey
WHERE O.ID='{this.poid}' AND tcd.Article IN ( SELECT Article FROM #tmp )

----------------Quiting用量計算--------------------------

select P.ID,OQ.Article,OQ.SizeCode,OCC.FabricPanelCode,OCC.FabricCode
,OE.ConsPC
,[Number of Needle for QT] =  ceiling(sqh.Width/sqh.HSize/sqh.NeedleDistance)
INTO #tmpQT
from Orders O
Inner join Order_Qty OQ on O.ID=OQ.ID
Inner join PO P on O.POID=P.ID
inner join Order_ColorCombo OCC on O.POID=OCC.Id and OQ.Article=OCC.Article and OCC.FabricCode is not null and OCC.FabricCode !=''
inner join Order_EachCons oe WITH (NOLOCK) on oe.id = occ.id and oe.FabricCombo = occ.PatternPanel and oe.CuttingPiece = 0 
inner join Order_EachCons_SizeQty OEZ on OE.Ukey=OEZ.Order_EachConsUkey and OQ.SizeCode=OEZ.SizeCode 
Inner join Style_QTThreadColorCombo_History SQH on O.styleUkey=SQH.styleUkey and SQH.FabricCode=OE.FabricCode and SQH.FabricPanelCode=OE.FabricPanelCode and P.ThreadVersion=SQH.Version
where o.ID = '{this.poid}'

select O.ID,OQ.Article,OQ.SizeCode,OCC.FabricPanelCode,OCC.FabricCode
,SQHD.Seq,SQHD.SCIRefno,SQHD.ColorID,QTt.Val
INTO #tmpQTFinal
from Orders O
Inner join Order_Qty OQ on O.ID=OQ.ID
Inner join PO P on O.POID=P.ID
inner join Order_ColorCombo OCC on O.POID=OCC.Id and OQ.Article=OCC.Article and OCC.FabricCode is not null and OCC.FabricCode !=''
Inner join Style_QTThreadColorCombo_History SQH on O.styleUkey=SQH.styleUkey and SQH.FabricCode=OCC.FabricCode and SQH.FabricPanelCode=OCC.FabricPanelCode and P.ThreadVersion=SQH.Version
Inner join Style_QTThreadColorCombo_History_Detail SQHD on SQH.Ukey=SQHD.Style_QTThreadColorCombo_HistoryUkey and OQ.Article=SQHD.Article
OUTER APPLY(
	SELECT Val=[Number of Needle for QT]* SQHD.Ratio * 0.9144 * qt.ConsPC * s.Qty
	FROM #tmpQT qt
	LEFT JOIN #tmp s ON qt.Article = s.Article AND qt.SizeCode = s.SizeCode
	WHERE qt.ID=o.ID AND qt.Article=oq.Article AND qt.SizeCode=oq.SizeCode
	AND qt.FabricPanelCode=OCC.FabricPanelCode AND qt.FabricCode =OCC.FabricCode
)QTt
where o.ID = '{this.poid}'

----------------Quiting用量計算--------------------------

SELECT  DISTINCT
  psd.SCIRefno
, psd.Refno
, ColorID = isnull(psdsC.SpecValue, '')
, f.DescDetail
, [@Qty] = ISNULL(ThreadUsedQtyByBOT.Val,0) + ISNULL(QT.Val,0)
, [AccuIssued] = (
					select isnull(sum([IS].qty),0)
					from dbo.issue I WITH (NOLOCK) 
					inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I.id = [IS].Id 
					where I.type = 'E' and I.Status = 'Confirmed' 
					and [IS].Poid=psd.id AND [IS].SCIRefno=PSD.SCIRefno AND [IS].ColorID=isnull(psdsC.SpecValue, '') and i.[EditDate]<GETDATE()
				)
, [IssueQty]=0.00
, [Use Qty By Stock Unit] = CEILING (ISNULL(ThreadUsedQtyByBOT.Qty,0) * ISNULL(ThreadUsedQtyByBOT.Val,0) + ISNULL(QT.Val,0)/ 100 * ISNULL(UnitRate.RateValue,1) )
, [Stock Unit]=StockUnit.StockUnit
, [Use Qty By Use Unit] = (ThreadUsedQtyByBOT.Qty *  ISNULL(ThreadUsedQtyByBOT.Val,0) + ISNULL(QT.Val,0) )
, [Use Unit]='CM'
, [Stock Unit Desc.]=StockUnit.Description
, [OutputQty] = ISNULL(ThreadUsedQtyByBOT.Qty,0)
, [Balance(Stock Unit)]= 0.00
, [Location] = ''
, [POID]=psd.ID 
, o.MDivisionID
INTO #final
FROM PO_Supp_Detail psd
INNER JOIN Fabric f ON f.SCIRefno = psd.SCIRefno
INNER JOIN MtlType m ON m.id= f.MtlTypeID
INNER JOIN Orders o ON psd.ID = o.ID
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
OUTER APPLY(
	SELECT TOP 1 PSD2.StockUnit ,u.Description
	FROM PO_Supp_Detail PSD2 
	LEFT JOIN Unit u ON u.ID = psd2.StockUnit
    left join PO_Supp_Detail_Spec psdsC2 WITH (NOLOCK) on psdsC2.ID = PSD2.id and psdsC2.seq1 = PSD2.seq1 and psdsC2.seq2 = PSD2.seq2 and psdsC2.SpecColumnID = 'Color'
	WHERE PSD2.ID = psd.id
	AND PSD2.SCIRefno=psd.SCIRefno
	AND isnull(psdsC2.SpecValue, '') = isnull(psdsC.SpecValue, '')
)StockUnit
OUTER APPLY(
	SELECT SCIRefNo
		,ColorID
		,[Val]=sum (g.OpThreadQty)
		,[Qty] = (	
			SELECt [Qty]=SUM(b.Qty)
			FROM #step1 a
			INNER JOIN #tmp_sumQty b ON a.Article = b.Article
			WHERE SCIRefNo=psd.SCIRefNo AND  ColorID= isnull(psdsC.SpecValue, '') AND a.Article=g.Article
			GROUP BY a.Article
		)
	FROM DBO.GetThreadUsedQtyByBOT(psd.ID,default) g
	WHERE SCIRefNo= psd.SCIRefNo AND ColorID = isnull(psdsC.SpecValue, '')  
	AND Article IN (
		SELECt Article FROM #step1 WHERE SCIRefNo = psd.SCIRefNo  AND ColorID = isnull(psdsC.SpecValue, '') 
	)
	GROUP BY SCIRefNo,ColorID , Article
)ThreadUsedQtyByBOT
OUTER APPLY(
	SELECT Val = SUM(t.Val)
	FROM #tmpQTFinal　ｔ
	WHERE t.SCIRefNo=psd.SCIRefno AND t.ColorID=isnull(psdsC.SpecValue, '')
)QT
OUTER APPLY(
	SELECT RateValue = IIF(Denominator = 0,0, Numerator / Denominator)
	FROM Unit_Rate
	WHERE UnitFrom='M' and  UnitTo = StockUnit.StockUnit
)UnitRate

WHERE psd.id ='{this.poid}' 
AND m.IsThread=1 
AND psd.FabricType ='A'
and isnull(psdsC.SpecValue, '') <> ''
AND psd.Refno='{refno}'
AND isnull(psdsC.SpecValue, '')='{colorID}'
AND psd.SCIRefno='{sCIRefno}'

SELECT    SCIRefno 
        , Refno
		, ColorID
		, DescDetail
		, [@Qty] = SUM([@Qty])
        , [AccuIssued]
        , [IssueQty]
        , [Use Qty By Stock Unit] = CEILING (SUM([Use Qty By Stock Unit] ))
        , [Stock Unit]
        , [Use Qty By Use Unit] = SUM([Use Qty By Use Unit] )
        , [Use Unit]
        , [Stock Unit Desc.]
        , [OutputQty] = SUM([OutputQty])
        , [Balance(Stock Unit)] = 0
        , [Location] 
        , [POID]
        , MDivisionID
INTO #final2
FROM #final
GROUP BY SCIRefno 
        , Refno
		, ColorID
		, DescDetail
        , [AccuIssued]
        , [IssueQty]
        , [Stock Unit]
        , [Use Unit]
        , [Stock Unit Desc.]
        , [Balance(Stock Unit)]
        , [Location] 
        , [POID]
        , MDivisionID


SELECT    SCIRefno 
        , Refno
		, ColorID
		, SuppColor = RealSuppCol.Val
		, DescDetail
		, [@Qty] 
        , [AccuIssued]
        , [IssueQty]
        , [Use Qty By Stock Unit]
        , [Stock Unit]
        , [Use Qty By Use Unit] 
        , [Use Unit]
        , [Stock Unit Desc.]
        , [OutputQty]
        , [Balance(Stock Unit)] = Balance.Qty
        , [Location] 
        , [POID]
        , MDivisionID
FROM #final2 t
OUTER APPLY(
	SELECT [Qty]=ISNULL(( SUM(Fty.InQty - Fty.OutQty + Fty.AdjustQty - Fty.ReturnQty)) ,0)
	FROM PO_Supp_Detail psd 
	LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
    left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
	WHERE psd.SCIRefno=t.SCIRefno AND isnull(psdsC.SpecValue, '')=t.ColorID AND psd.ID='{this.poid}'
)Balance 
OUTER APPLY(
	----僅列出Balance 有計算到數量的Seq1 Seq2
	SELECT [Val]=STUFF((
		SELECT  DISTINCT ',' + SuppColor
		FROM PO_Supp_Detail y
        inner join PO_Supp_Detail_Spec psdsCy WITH (NOLOCK) on psdsCy.ID = y.id and psdsCy.seq1 = y.seq1 and psdsCy.seq2 = y.seq2 and psdsCy.SpecColumnID = 'Color'
		WHERE EXISTS( 
			SELECT 1 
			FROM PO_Supp_Detail psd 
            inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
			LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
			WHERE psd.SCIRefno=t.SCIRefno AND isnull(psdsC.SpecValue, '') = t.ColorID AND psd.ID = '{this.poid}'
			AND psd.SCIRefno = y.SCIRefno AND isnull(psdsC.SpecValue, '') = isnull(psdsCy.SpecValue, '') AND psd.ID = y.ID
			AND psd.SEQ1 = y.SEQ1 AND psd.SEQ2 = y.SEQ2
			GROUP BY psd.seq1,psd.seq2
			HAVING ISNULL(( SUM(Fty.InQty - Fty.OutQty + Fty.AdjustQty - Fty.ReturnQty)) ,0) > 0
		)
		FOR XML PATH('')
	),1,1,'')
)RealSuppCol

DROP TABLE #tmp_sumQty,#step1,#tmp,#final,#final2
";
                    DataRow row;
                    DataTable rtn = null;
                    MyUtility.Tool.ProcessWithDatatable(unPivotBrkQty, string.Empty, sqlcmd, out rtn, "#tmp");

                    if (rtn == null || rtn.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("Data not found!", "ColorID");
                        return;
                    }

                    row = rtn.Rows[0];

                    this.CurrentDetailData["SCIRefno"] = row["SCIRefno"];
                    this.CurrentDetailData["Refno"] = row["Refno"];
                    this.CurrentDetailData["ColorID"] = row["ColorID"];
                    this.CurrentDetailData["SuppColor"] = row["SuppColor"];
                    this.CurrentDetailData["POID"] = row["POID"];
                    this.CurrentDetailData["DescDetail"] = row["DescDetail"];
                    this.CurrentDetailData["@Qty"] = row["@Qty"];
                    this.CurrentDetailData["Use Unit"] = row["Use Unit"];
                    this.CurrentDetailData["AccuIssued"] = row["AccuIssued"];
                    this.CurrentDetailData["IssueQty"] = row["IssueQty"];
                    this.CurrentDetailData["Use Qty By Stock Unit"] = row["Use Qty By Stock Unit"];
                    this.CurrentDetailData["Stock Unit"] = row["Stock Unit"];
                    this.CurrentDetailData["Use Qty By Use Unit"] = row["Use Qty By Use Unit"];
                    this.CurrentDetailData["Use Unit"] = row["Use Unit"];
                    this.CurrentDetailData["Stock Unit Desc."] = row["Stock Unit Desc."];
                    this.CurrentDetailData["OutputQty"] = row["OutputQty"];
                    this.CurrentDetailData["Balance(Stock Unit)"] = row["Balance(Stock Unit)"];
                    this.CurrentDetailData["Location"] = row["Location"];
                    this.CurrentDetailData["MDivisionID"] = row["MDivisionID"];

                    #endregion

                    this.CurrentDetailData.EndEdit();
                    this.GetSubDetailDatas(this.CurrentDetailData, out DataTable finalSubDt);
                    finalSubDt.Clear();
                }
            };
            colorSet.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (string.Compare(e.FormattedValue.ToString().Trim(), this.CurrentDetailData["ColorID"].ToString().Trim()) != 0)
                {
                    if (this.Is_IssueBreakDownEmpty())
                    {
                        MyUtility.Msg.InfoBox("Issue Breakdown can't be empty !!");
                        return;
                    }

                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        // CurrentDetailData["Refno"] = "";
                        this.CurrentDetailData["SCIRefno"] = string.Empty;
                        this.CurrentDetailData["ColorID"] = string.Empty;
                        this.CurrentDetailData["SuppColor"] = string.Empty;
                        this.CurrentDetailData["POID"] = string.Empty;
                        this.CurrentDetailData["DescDetail"] = string.Empty;
                        this.CurrentDetailData["@Qty"] = DBNull.Value;
                        this.CurrentDetailData["Use Unit"] = string.Empty;
                        this.CurrentDetailData["AccuIssued"] = DBNull.Value;
                        this.CurrentDetailData["IssueQty"] = DBNull.Value;
                        this.CurrentDetailData["Use Qty By Stock Unit"] = DBNull.Value;
                        this.CurrentDetailData["Stock Unit"] = string.Empty;
                        this.CurrentDetailData["Use Qty By Use Unit"] = DBNull.Value;
                        this.CurrentDetailData["Use Unit"] = string.Empty;
                        this.CurrentDetailData["Stock Unit Desc."] = string.Empty;
                        this.CurrentDetailData["OutputQty"] = DBNull.Value;
                        this.CurrentDetailData["Balance(Stock Unit)"] = DBNull.Value;
                        this.CurrentDetailData["Location"] = string.Empty;
                        this.CurrentDetailData["MDivisionID"] = string.Empty;
                    }
                    else
                    {
                        string refno = MyUtility.Convert.GetString(this.CurrentDetailData["Refno"]);

                        #region 將IssueBreakDown整理成Datatable
                        if (this.dtIssueBreakDown == null)
                        {
                            return;
                        }

                        List<IssueQtyBreakdown> modelList = new List<IssueQtyBreakdown>();
                        foreach (DataRow tempRow in this.dtIssueBreakDown.Rows)
                        {
                            int totalQty = 0;
                            foreach (DataColumn col in this.dtIssueBreakDown.Columns)
                            {
                                IssueQtyBreakdown m = new IssueQtyBreakdown()
                                {
                                    OrderID = tempRow["OrderID"].ToString(),
                                    Article = tempRow["Article"].ToString(),
                                };

                                if (tempRow[col].GetType().Name == "Decimal")
                                {
                                    totalQty += Convert.ToInt32(tempRow[col]);
                                }

                                if (col.ColumnName != "OrderID" && col.ColumnName != "Article")
                                {
                                    m.SizeCode = col.ColumnName;

                                    m.Qty = totalQty;
                                    modelList.Add(m);
                                    totalQty = 0;
                                }
                            }
                        }

                        DataTable t = new DataTable();

                        // IssueBreakDown_Dt.Columns.Add(new DataColumn() { ColumnName = "OrderID", DataType = typeof(string) });
                        t.Columns.Add(new DataColumn() { ColumnName = "Article", DataType = typeof(string) });
                        t.Columns.Add(new DataColumn() { ColumnName = "SizeCode", DataType = typeof(string) });
                        t.Columns.Add(new DataColumn() { ColumnName = "Qty", DataType = typeof(int) });

                        var groupByData = modelList.GroupBy(o => new { o.Article, o.SizeCode }).Select(o => new
                        {
                            o.Key.Article,
                            o.Key.SizeCode,
                            Qty = o.Sum(x => x.Qty),
                        }).ToList();

                        foreach (var model in groupByData)
                        {
                            if (model.Qty > 0)
                            {
                                DataRow newDr = t.NewRow();

                                // newDr["OrderID"] = model.OrderID;
                                newDr["Article"] = model.Article;
                                newDr["SizeCode"] = model.SizeCode;
                                newDr["Qty"] = model.Qty;

                                t.Rows.Add(newDr);
                            }
                        }

                        #endregion

                        #region SQL
                        string sqlcmd = $@"

SELECt Article,[Qty]=SUM(Qty) 
INTO #tmp_sumQty
FROm #tmp
GROUP BY Article

Select distinct o.ID,tcd.SCIRefNo, tcd.ColorID ,tcd.Article 
INTO #step1
From dbo.Orders as o
INNER JOIN PO WITH (NOLOCK) ON po.StyleUkey = o.StyleUkey
Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
Inner Join dbo.Style_ThreadColorCombo_History as tc On tc.StyleUkey = s.Ukey AND po.ThreadVersion = tc.Version
Inner Join dbo.Style_ThreadColorCombo_History_Detail as tcd On tcd.Style_ThreadColorCombo_HistoryUkey  = tc.Ukey
WHERE O.ID='{this.poid}' AND tcd.Article IN ( SELECT Article FROM #tmp )

----------------Quiting用量計算--------------------------

select P.ID,OQ.Article,OQ.SizeCode,OCC.FabricPanelCode,OCC.FabricCode
,OE.ConsPC
,[Number of Needle for QT] =  ceiling(sqh.Width/sqh.HSize/sqh.NeedleDistance)
INTO #tmpQT
from Orders O
Inner join Order_Qty OQ on O.ID=OQ.ID
Inner join PO P on O.POID=P.ID
inner join Order_ColorCombo OCC on O.POID=OCC.Id and OQ.Article=OCC.Article and OCC.FabricCode is not null and OCC.FabricCode !=''
inner join Order_EachCons oe WITH (NOLOCK) on oe.id = occ.id and oe.FabricCombo = occ.PatternPanel and oe.CuttingPiece = 0 
inner join Order_EachCons_SizeQty OEZ on OE.Ukey=OEZ.Order_EachConsUkey and OQ.SizeCode=OEZ.SizeCode 
Inner join Style_QTThreadColorCombo_History SQH on O.styleUkey=SQH.styleUkey and SQH.FabricCode=OE.FabricCode and SQH.FabricPanelCode=OE.FabricPanelCode and P.ThreadVersion=SQH.Version
where o.ID = '{this.poid}'

select O.ID,OQ.Article,OQ.SizeCode,OCC.FabricPanelCode,OCC.FabricCode
,SQHD.Seq,SQHD.SCIRefno,SQHD.ColorID,QTt.Val
INTO #tmpQTFinal
from Orders O
Inner join Order_Qty OQ on O.ID=OQ.ID
Inner join PO P on O.POID=P.ID
inner join Order_ColorCombo OCC on O.POID=OCC.Id and OQ.Article=OCC.Article and OCC.FabricCode is not null and OCC.FabricCode !=''
Inner join Style_QTThreadColorCombo_History SQH on O.styleUkey=SQH.styleUkey and SQH.FabricCode=OCC.FabricCode and SQH.FabricPanelCode=OCC.FabricPanelCode and P.ThreadVersion=SQH.Version
Inner join Style_QTThreadColorCombo_History_Detail SQHD on SQH.Ukey=SQHD.Style_QTThreadColorCombo_HistoryUkey and OQ.Article=SQHD.Article
OUTER APPLY(
	SELECT Val=[Number of Needle for QT]* SQHD.Ratio * 0.9144 * qt.ConsPC * s.Qty
	FROM #tmpQT qt
	LEFT JOIN #tmp s ON qt.Article = s.Article AND qt.SizeCode = s.SizeCode
	WHERE qt.ID=o.ID AND qt.Article=oq.Article AND qt.SizeCode=oq.SizeCode
	AND qt.FabricPanelCode=OCC.FabricPanelCode AND qt.FabricCode =OCC.FabricCode
)QTt
where o.ID = '{this.poid}'

----------------Quiting用量計算--------------------------

SELECT  DISTINCT
  psd.SCIRefno
, psd.Refno
, ColorID = isnull(psdsC.SpecValue, '')
, f.DescDetail
, [@Qty] = ISNULL(ThreadUsedQtyByBOT.Val,0) + ISNULL(QT.Val,0)
, [AccuIssued] = (
					select isnull(sum([IS].qty),0)
					from dbo.issue I WITH (NOLOCK) 
					inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I.id = [IS].Id 
					where I.type = 'E' and I.Status = 'Confirmed' 
					and [IS].Poid=psd.id AND [IS].SCIRefno=PSD.SCIRefno AND [IS].ColorID=isnull(psdsC.SpecValue, '') and i.[EditDate]<GETDATE()
				)
, [IssueQty]=0.00
, [Use Qty By Stock Unit] = CEILING( ISNULL(ThreadUsedQtyByBOT.Qty,0) *  ISNULL(ThreadUsedQtyByBOT.Val,0) + ISNULL(QT.Val,0)/ 100 * ISNULL(UnitRate.RateValue,1) )
, [Stock Unit]=StockUnit.StockUnit
, [Use Qty By Use Unit] = (ThreadUsedQtyByBOT.Qty *  ISNULL(ThreadUsedQtyByBOT.Val,0) + ISNULL(QT.Val,0) )
, [Use Unit]='CM'
, [Stock Unit Desc.]=StockUnit.Description
, [OutputQty] = ISNULL(ThreadUsedQtyByBOT.Qty,0)
, [Balance(Stock Unit)]= 0.00
, [Location] = ''
, [POID]=psd.ID 
, o.MDivisionID
INTO #final
FROM PO_Supp_Detail psd
INNER JOIN Fabric f ON f.SCIRefno = psd.SCIRefno
INNER JOIN MtlType m ON m.id= f.MtlTypeID
INNER JOIN Orders o ON psd.ID = o.ID
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
OUTER APPLY(
	SELECT TOP 1 PSD2.StockUnit ,u.Description
	FROM PO_Supp_Detail PSD2 
	LEFT JOIN Unit u ON u.ID = psd2.StockUnit
    left join PO_Supp_Detail_Spec psdsC2 WITH (NOLOCK) on psdsC2.ID = PSD2.id and psdsC2.seq1 = PSD2.seq1 and psdsC2.seq2 = PSD2.seq2 and psdsC2.SpecColumnID = 'Color'
	WHERE PSD2.ID = psd.id
	AND PSD2.SCIRefno=psd.SCIRefno
	AND isnull(psdsC2.SpecValue, '') = isnull(psdsC.SpecValue, '')
)StockUnit
OUTER APPLY(
	SELECT SCIRefNo
		,ColorID
		,[Val]=sum (g.OpThreadQty)
		,[Qty] = (	
			SELECt [Qty]=SUM(b.Qty)
			FROM #step1 a
			INNER JOIN #tmp_sumQty b ON a.Article = b.Article
			WHERE SCIRefNo=psd.SCIRefNo AND  ColorID= isnull(psdsC.SpecValue, '') AND a.Article=g.Article
			GROUP BY a.Article
		)
	FROM DBO.GetThreadUsedQtyByBOT(psd.ID,default) g
	WHERE SCIRefNo= psd.SCIRefNo AND ColorID = isnull(psdsC.SpecValue, '')  
	AND Article IN (
		SELECt Article FROM #step1 WHERE SCIRefNo = psd.SCIRefNo  AND ColorID = isnull(psdsC.SpecValue, '') 
	)
	GROUP BY SCIRefNo,ColorID , Article
)ThreadUsedQtyByBOT
OUTER APPLY(
	SELECT Val = SUM(t.Val)
	FROM #tmpQTFinal　ｔ
	WHERE t.SCIRefNo=psd.SCIRefno AND t.ColorID=isnull(psdsC.SpecValue, '')
)QT
OUTER APPLY(
	SELECT RateValue = IIF(Denominator = 0,0, Numerator / Denominator)
	FROM Unit_Rate
	WHERE UnitFrom='M' and  UnitTo = StockUnit.StockUnit
)UnitRate
WHERE psd.id ='{this.poid}' 
AND m.IsThread=1 
AND psd.FabricType ='A'
and psd.Refno <> ''
AND isnull(psdsC.SpecValue, '')='{e.FormattedValue}'

";

                        if (!MyUtility.Check.Empty(refno))
                        {
                            sqlcmd += $"AND psd.Refno='{refno}' ";
                        }
                        else
                        {
                            sqlcmd += $"AND psd.Refno <> '' ";
                        }

                        sqlcmd += $@"
SELECT    SCIRefno 
        , Refno
        , ColorID
		, DescDetail
		, [@Qty] = SUM([@Qty])
        , [AccuIssued]
        , [IssueQty]
        , [Use Qty By Stock Unit] = CEILING (SUM([Use Qty By Stock Unit] ))
        , [Stock Unit]
        , [Use Qty By Use Unit] = SUM([Use Qty By Use Unit] )
        , [Use Unit]
        , [Stock Unit Desc.]
        , [OutputQty] = SUM([OutputQty])
        , [Balance(Stock Unit)] = 0
        , [Location] 
        , [POID]
        , MDivisionID
INTO #final2
FROM #final
GROUP BY SCIRefno 
        , Refno
        , ColorID
		, DescDetail
        , [AccuIssued]
        , [IssueQty]
        , [Stock Unit]
        , [Use Unit]
        , [Stock Unit Desc.]
        , [Balance(Stock Unit)]
        , [Location] 
        , [POID]
        , MDivisionID


SELECT    SCIRefno 
        , Refno
        , ColorID
		, SuppColor = RealSuppCol.Val
		, DescDetail
		, [@Qty] 
        , [AccuIssued]
        , [IssueQty]
        , [Use Qty By Stock Unit]
        , [Stock Unit]
        , [Use Qty By Use Unit] 
        , [Use Unit]
        , [Stock Unit Desc.]
        , [OutputQty]
        , [Balance(Stock Unit)] = Balance.Qty
        , [Location] 
        , [POID]
        , MDivisionID
FROM #final2 t
OUTER APPLY(
	SELECT [Qty]=ISNULL(( SUM(Fty.InQty - Fty.OutQty + Fty.AdjustQty - Fty.ReturnQty)) ,0)
	FROM PO_Supp_Detail psd 
	LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
    left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
	WHERE psd.SCIRefno=t.SCIRefno AND isnull(psdsC.SpecValue, '')=t.ColorID AND psd.ID='{this.poid}'
)Balance 
OUTER APPLY(
	----僅列出Balance 有計算到數量的Seq1 Seq2
	SELECT [Val]=STUFF((
		SELECT  DISTINCT ',' + SuppColor
		FROM PO_Supp_Detail y
        inner join PO_Supp_Detail_Spec psdsCy WITH (NOLOCK) on psdsCy.ID = y.id and psdsCy.seq1 = y.seq1 and psdsCy.seq2 = y.seq2 and psdsCy.SpecColumnID = 'Color'
		WHERE EXISTS( 
			SELECT 1 
			FROM PO_Supp_Detail psd 
            inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
			LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
			WHERE psd.SCIRefno=t.SCIRefno AND isnull(psdsC.SpecValue, '') = t.ColorID AND psd.ID = '{this.poid}'
			AND psd.SCIRefno = y.SCIRefno AND isnull(psdsC.SpecValue, '') = isnull(psdsCy.SpecValue, '') AND psd.ID = y.ID
			AND psd.SEQ1 = y.SEQ1 AND psd.SEQ2 = y.SEQ2
			GROUP BY psd.seq1,psd.seq2
			HAVING ISNULL(( SUM(Fty.InQty - Fty.OutQty + Fty.AdjustQty - Fty.ReturnQty)) ,0) > 0
		)
		FOR XML PATH('')
	),1,1,'')
)RealSuppCol
DROP TABLE #tmp_sumQty,#step1,#tmp,#final,#final2
";

                        #endregion

                        DataRow row;
                        DataTable rtn = null;
                        MyUtility.Tool.ProcessWithDatatable(t, string.Empty, sqlcmd, out rtn, "#tmp");

                        if (rtn == null || rtn.Rows.Count == 0)
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "ColorID");
                            return;
                        }
                        else
                        {
                            row = rtn.Rows[0];

                            // 當兩筆資料以上時，增加判斷SCIRefno、Refno、Color
                            if (rtn.Rows.Count > 1)
                            {
                                var queryRow = rtn.AsEnumerable().Where(x => x.Field<string>("SCIRefno").EqualString(this.CurrentDetailData["SCIRefno"]) &&
                                                              x.Field<string>("Refno").EqualString(this.CurrentDetailData["Refno"]) &&
                                                              x.Field<string>("ColorID").EqualString(this.CurrentDetailData["ColorID"]));
                                if (queryRow.Any())
                                {
                                    row = queryRow.FirstOrDefault();
                                }
                            }

                            if (MyUtility.Check.Empty(refno))
                            {
                                // CurrentDetailData["SCIRefno"] = row["SCIRefno"];
                                this.CurrentDetailData["ColorID"] = e.FormattedValue;

                                // CurrentDetailData["DescDetail"] = row["DescDetail"];
                                this.CurrentDetailData["POID"] = this.poid;
                            }
                            else
                            {
                                this.CurrentDetailData["SCIRefno"] = row["SCIRefno"];
                                this.CurrentDetailData["Refno"] = row["Refno"];
                                this.CurrentDetailData["ColorID"] = row["ColorID"];
                                this.CurrentDetailData["SuppColor"] = row["SuppColor"];
                                this.CurrentDetailData["POID"] = row["POID"];
                                this.CurrentDetailData["DescDetail"] = row["DescDetail"];
                                this.CurrentDetailData["@Qty"] = row["@Qty"];
                                this.CurrentDetailData["Use Unit"] = row["Use Unit"];
                                this.CurrentDetailData["AccuIssued"] = row["AccuIssued"];
                                this.CurrentDetailData["IssueQty"] = row["IssueQty"];
                                this.CurrentDetailData["Use Qty By Stock Unit"] = row["Use Qty By Stock Unit"];
                                this.CurrentDetailData["Stock Unit"] = row["Stock Unit"];
                                this.CurrentDetailData["Use Qty By Use Unit"] = row["Use Qty By Use Unit"];
                                this.CurrentDetailData["Use Unit"] = row["Use Unit"];
                                this.CurrentDetailData["Stock Unit Desc."] = row["Stock Unit Desc."];
                                this.CurrentDetailData["OutputQty"] = row["OutputQty"];
                                this.CurrentDetailData["Balance(Stock Unit)"] = row["Balance(Stock Unit)"];
                                this.CurrentDetailData["Location"] = row["Location"];
                                this.CurrentDetailData["MDivisionID"] = row["MDivisionID"];
                            }

                            this.CurrentDetailData.EndEdit();

                            this.GetSubDetailDatas(this.CurrentDetailData, out DataTable finalSubDt);
                            finalSubDt.Clear();
                        }
                    }
                }
            };
            #endregion

            #region 單件用量欄位事件
            DataGridViewGeneratorNumericColumnSettings qty = new DataGridViewGeneratorNumericColumnSettings();

            qty.CellMouseDoubleClick += (s, e) =>
            {
                string sCIRefNo = this.CurrentDetailData["SCIRefNo"].ToString();
                string colorID = this.CurrentDetailData["ColorID"].ToString();

                if (this.dtIssueBreakDown == null)
                {
                    return;
                }

                List<IssueQtyBreakdown> modelList = new List<IssueQtyBreakdown>();

                // 檢查是否有勾選Combo，處理傳入AutoPick資料篩選
                if (!this.checkByCombo.Checked && this.dtIssueBreakDown != null)
                {
                    foreach (DataRow tempRow in this.dtIssueBreakDown.Rows)
                    {
                        if (tempRow["OrderID"].ToString() != this.txtOrderID.Text.ToString())
                        {
                            foreach (DataColumn tempColumn in this.dtIssueBreakDown.Columns)
                            {
                                if (tempRow[tempColumn].GetType().Name == "Decimal")
                                {
                                    tempRow[tempColumn] = 0;
                                }
                            }
                        }
                    }
                }

                foreach (DataRow tempRow in this.dtIssueBreakDown.Rows)
                {
                    IssueQtyBreakdown m = new IssueQtyBreakdown()
                    {
                        OrderID = tempRow["OrderID"].ToString(),
                        Article = tempRow["Article"].ToString(),
                    };

                    int totalQty = 0;
                    foreach (DataColumn col in this.dtIssueBreakDown.Columns)
                    {
                        if (tempRow[col].GetType().Name == "Decimal")
                        {
                            totalQty += Convert.ToInt32(tempRow[col]);
                        }
                    }

                    m.Qty = totalQty;
                    modelList.Add(m);
                }

                List<string> articles = modelList.Where(o => o.Qty > 0).Select(o => o.Article).Distinct().ToList();
                string cmd = $@"

SELECT Article, [Qty]=sum (OpThreadQty)
FROM dbo.GetThreadUsedQtyByBOT('{this.poid}',default)
WHERE SCIRefNo='{sCIRefNo}' 
AND ColorID='{colorID}'
AND Article IN ('{articles.JoinToString("','")}')
GROUP BY Article
";
                DataTable dt;
                DualResult dualResult = DBProxy.Current.Select(null, cmd, out dt);
                if (!dualResult)
                {
                    this.ShowErr(dualResult);
                    return;
                }

                MyUtility.Msg.ShowMsgGrid_LockScreen(dt, caption: $"@Qty by Article");
            };
            #endregion

            #region issue Qty 開窗
            DataGridViewGeneratorNumericColumnSettings issueQty = new DataGridViewGeneratorNumericColumnSettings();
            issueQty.CellMouseDoubleClick += (s, e) =>
            {
                if (this.dtIssueBreakDown == null)
                {
                    MyUtility.Msg.WarningBox("IssueBreakdown data no data!", "Warning");
                    return;
                }

                this.DoSubForm.IsSupportUpdate = false;
                this.OpenSubDetailPage();

                DataTable finalSubDt;
                this.GetSubDetailDatas(out finalSubDt);

                DataTable detail = (DataTable)this.detailgridbs.DataSource;
                foreach (DataRow dr in detail.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        this.GetSubDetailDatas(dr, out finalSubDt);

                        dr["Qty"] = Math.Round(
                            finalSubDt.AsEnumerable()
                                                    .Where(row => row.RowState != DataRowState.Deleted && !MyUtility.Check.Empty(row["Qty"]))
                                                    .Sum(row => Convert.ToDouble(row["Qty"].ToString())),
                            2);
                        dr["IssueQty"] = Math.Round(
                            finalSubDt.AsEnumerable()
                                                    .Where(row => row.RowState != DataRowState.Deleted && !MyUtility.Check.Empty(row["Qty"]))
                                                    .Sum(row => Convert.ToDouble(row["Qty"].ToString())),
                            2);
                    }
                }
            };
            #endregion

            #region -- 欄位設定 --
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("SCIRefno", header: "SCIRefno", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Refno", header: "Refno", width: Widths.AnsiChars(15), settings: refnoSet)
            .Text("ColorID", header: "Color", width: Widths.AnsiChars(7), settings: colorSet)
            .Text("SuppColor", header: "SuppColor", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .EditText("DescDetail", header: "Desc.", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Numeric("@Qty", header: "@Qty", width: Widths.AnsiChars(15), decimal_places: 2, integer_places: 10, iseditingreadonly: true, settings: qty)
            .Numeric("AccuIssued", header: "Accu. Issued" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("IssueQty", header: "Issue Qty" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(6), decimal_places: 2, settings: issueQty, iseditingreadonly: true)
            .Numeric("Use Qty By Stock Unit", header: "Use Qty" + Environment.NewLine + "By Stock Unit", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
            .Text("Stock Unit", header: "Stock Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("Use Qty By Use Unit", header: "Use Qty" + Environment.NewLine + "By Use Unit", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
            .Text("Use Unit", header: "Use Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Stock Unit Desc.", header: "Stock Unit Desc.", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Numeric("OutputQty", header: "Output Qty" + Environment.NewLine + "(Garment)", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
            .Numeric("Balance(Stock Unit)", header: "Balance" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
            .Text("Location", header: "Location", width: Widths.AnsiChars(10), iseditingreadonly: true);
            #endregion 欄位設定

            #region 可編輯欄位變色
            this.detailgrid.Columns["Refno"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["ColorID"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["IssueQty"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion 可編輯欄位變色
        }

        /// <inheritdoc/>
        protected override void OpenSubDetailPage()
        {
            base.OpenSubDetailPage();
        }

        #endregion

        #region ToolBar事件

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();

            string tempId = MyUtility.GetValue.GetID(Env.User.Keyword + "IT", "Issue", DateTime.Now);

            if (MyUtility.Check.Empty(tempId))
            {
                MyUtility.Msg.WarningBox("Get document ID fail!!");
                return;
            }

            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "E";
            this.CurrentMaintain["issuedate"] = DateTime.Now;
            this.CurrentMaintain["ToPlace"] = this.dgToPlace.SelectedValue;
            this.CurrentMaintain["combo"] = 0;
            this.dtIssueBreakDown = null;
            this.gridIssueBreakDown.DataSource = null;
            this.txtOrderID.IsSupportEditMode = true;

            // txtRequest.IsSupportEditMode = true;
            // txtOrderID.ReadOnly = false;
            // txtRequest.ReadOnly = false;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            string status = this.CurrentMaintain["Status"].ToString();
            if (status != "New")
            {
                MyUtility.Msg.InfoBox("The record status is not new, can't modify !!");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            DataTable result = null;

            if (((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(o => o.RowState != DataRowState.Deleted).Count() == 0)
            {
                MyUtility.Msg.InfoBox("Detail can't be empty !!");
                return false;
            }

            #region 檢查不可為空
            if (MyUtility.Check.Empty(this.CurrentMaintain["OrderID"]) || MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.InfoBox("[SP#] , [Issue Date] can't be empty !!");
                return false;
            }

            // 將Issue_Detail的數量更新Issue_Summary
            DataTable subDetail;
            DataTable detail = (DataTable)this.detailgridbs.DataSource;
            foreach (DataRow detailRow in detail.Rows)
            {
                if (detailRow.RowState == DataRowState.Deleted)
                {
                    continue;
                }

                this.GetSubDetailDatas(detailRow, out subDetail);
                if (subDetail.Rows.Count == 0)
                {
                    detailRow["Qty"] = 0;
                    detailRow["IssueQty"] = 0;
                }
                else
                {
                    decimal detailQty = subDetail.AsEnumerable().Sum(s => s.RowState != DataRowState.Deleted ? (decimal)s["Qty"] : 0);
                    detailRow["IssueQty"] = detailQty;
                    detailRow["Qty"] = detailQty;
                }
            }

            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["IssueQty"]) || MyUtility.Check.Empty(dr["SCIRefNo"]) || MyUtility.Check.Empty(dr["ColorID"]))
                {
                    MyUtility.Msg.InfoBox("[RefNo] , [Color] , [Issue Qty] can't be empty !!");
                    return false;
                }
            }
            #endregion

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "IC", "Issue", (DateTime)this.CurrentMaintain["IssueDate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;

                // assign 給detail table ID
                DataTable tmp = (DataTable)this.detailgridbs.DataSource;

                foreach (DataRow row in tmp.Rows)
                {
                    row.SetField("ID", tmpId);
                }
            }

            if (this.Is_IssueBreakDownEmpty())
            {
                MyUtility.Msg.InfoBox("Issue Breakdown can't be empty !!");
                return false;
            }

            if (this.dtSizeCode != null && this.dtSizeCode.Rows.Count != 0)
            {
                if (this.checkByCombo.Checked == false)
                {
                    foreach (DataRow data in this.dtIssueBreakDown.ToList())
                    {
                        if (data.ItemArray[0].ToString() != this.txtOrderID.Text)
                        {
                            this.dtIssueBreakDown.Rows.Remove(data);
                        }
                    }
                }

                string sqlcmd;
                sqlcmd = string.Format(
                    @";
delete from dbo.issue_breakdown where id='{0}'
;WITH UNPIVOT_1
AS
(
SELECT * FROM #tmp
UNPIVOT
(
QTY
FOR SIZECODE IN ({1})
)
AS PVT
)
MERGE INTO DBO.ISSUE_BREAKDOWN T
USING UnPivot_1 S
ON T.ID = '{0}' AND T.ORDERID= S.OrderID AND T.ARTICLE = S.ARTICLE AND T.SIZECODE = S.SIZECODE
WHEN MATCHED THEN
UPDATE
SET QTY = S.QTY
WHEN NOT MATCHED THEN
INSERT (ID,ORDERID,ARTICLE,SIZECODE,QTY)
VALUES ('{0}',S.OrderID,S.ARTICLE,S.SIZECODE,S.QTY)
;delete from dbo.issue_breakdown where id='{0}' and qty = 0; ",
                    this.CurrentMaintain["id"],
                    this.sbSizecode.ToString().Substring(0, this.sbSizecode.ToString().Length - 1));

                string aaa = this.sbSizecode.ToString().Substring(0, this.sbSizecode.ToString().Length - 1).Replace("[", string.Empty).Replace("]", string.Empty);

                ProcessWithDatatable2(this.dtIssueBreakDown, "OrderID,Article," + aaa, sqlcmd, out result, "#tmp");
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickDeleteAfter()
        {
            base.ClickDeleteAfter();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickConfirm();
            if (this.CurrentMaintain == null)
            {
                return;
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return;
            }

            // 第3層才是 issue_detail
            DualResult result = DBProxy.Current.Select(null, $"select * from issue_detail WITH (NOLOCK) where id = '{this.CurrentMaintain["ID"]}'", out DataTable dtIssue_Detail);
            if (!result)
            {
                this.ShowErr(result);
            }

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            result = Prgs.GetFtyInventoryData(dtIssue_Detail, this.Name, out DataTable dtOriFtyInventory);
            DataTable datacheck;

            // 檢查 Barcode不可為空
            if (!Prgs.CheckBarCode(dtOriFtyInventory, this.Name))
            {
                return;
            }

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), "P33"))
            {
                return;
            }
            #endregion

            #region 檢查庫存項lock
            string sqlcmd = $@"
SELECT   psd.Refno
		,ColorID = isnull(psdsC.SpecValue, '')
		,d.Seq1
		,d.seq2
FROM Issue i
INNER JOIN Issue_Summary s ON i.ID = s.ID 
INNER JOIN Issue_Detail d ON s.id=d.id AND s.Ukey = d.Issue_SummaryUkey
INNER JOIN FtyInventory f ON f.POID=s.Poid AND f.Seq1=d.Seq1 AND f.Seq2=d.Seq2
INNER JOIN PO_Supp_Detail psd ON psd.ID = s.Poid AND psd.SCIRefno = s.SCIRefno AND psd.SCIRefno = s.SCIRefno AND psd.SEQ1=d.Seq1 AND psd.Seq2=d.Seq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
WHERE i.Id = '{this.CurrentMaintain["id"]}' AND  f.lock = 1 
";
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    var m = MyUtility.Msg.ShowMsgGrid(datacheck, "The following Thread has been Locked. can't confirm!!", "Material Locked");

                    m.Width = 850;
                    m.grid1.Columns[0].Width = 300;
                    m.grid1.Columns[1].Width = 100;
                    m.TopMost = true;
                    return;
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "Issue_Summary"))
            {
                return;
            }
            #endregion

            #region 檢查Location是否為空值
            if (Prgs.ChkLocation(this.CurrentMaintain["ID"].ToString(), "Issue_Detail") == false)
            {
                return;
            }
            #endregion

            #region 檢查負數庫存
            sqlcmd = $@"
SELECT   psd.Refno
		,ColorID = isnull(psdsC.SpecValue, '')
		,d.Seq1
		,d.seq2
		,[BulkQty] = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)
		,[IssueQty] = ISNULL(d.Qty ,0)
		,[Balance] = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) - d.Qty
FROM Issue i
INNER JOIN Issue_Summary s ON i.ID = s.ID 
INNER JOIN Issue_Detail d ON s.id=d.id AND s.Ukey = d.Issue_SummaryUkey
INNER JOIN FtyInventory f ON f.POID=s.Poid AND f.Seq1=d.Seq1 AND f.Seq2=d.Seq2
INNER JOIN PO_Supp_Detail psd ON psd.ID = s.Poid AND psd.SCIRefno = s.SCIRefno AND psd.SCIRefno = s.SCIRefno AND psd.SEQ1=d.Seq1 AND psd.Seq2=d.Seq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
WHERE i.Id = '{this.CurrentMaintain["id"]}'
AND(isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - isnull(f.ReturnQty,0) - ISNULL(d.Qty, 0)) < 0
AND f.StockType = 'B'
";

            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    var m = MyUtility.Msg.ShowMsgGrid(datacheck, "The following bulk stock is insufficient, can't confirm!!", "Balacne Qty is not enough");

                    m.Width = 850;
                    m.grid1.Columns[0].Width = 300;
                    m.grid1.Columns[1].Width = 100;
                    m.TopMost = true;
                    return;
                }
            }
            #endregion 檢查負數庫存

            #region 更新庫存數量  ftyinventory-- 更新mdivisionpodetail B倉數 --
            var bs1 = (from b in dtIssue_Detail.AsEnumerable()
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
                           Qty = m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            StringBuilder sqlupd2_B = new StringBuilder();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, true));
            string sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            #endregion

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dtIssue_Detail, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update Issue set status = 'Confirmed', ApvName = '{Env.User.UserID}', ApvDate  = GETDATE(), editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                this.ShowErr(errMsg);
                return;
            }

            // AutoWHFabric WebAPI
            Prgs_WMS.WMSprocess(false, dtIssue_Detail, this.Name, EnumStatus.New, EnumStatus.Confirm, dtOriFtyInventory);
            MyUtility.Msg.InfoBox("Confirmed successful");
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickUnconfirm();
            if (this.CurrentMaintain == null ||
                MyUtility.Msg.QuestionBox("Do you want to unconfirme it?") == DialogResult.No)
            {
                return;
            }

            // 第3層才是 issue_detail
            DualResult result = DBProxy.Current.Select(null, $"select * from issue_detail WITH (NOLOCK) where id = '{this.CurrentMaintain["ID"]}'", out DataTable dtIssue_Detail);
            if (!result)
            {
                this.ShowErr(result);
            }

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            result = Prgs.GetFtyInventoryData(dtIssue_Detail, this.Name, out DataTable dtOriFtyInventory);

            #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            if (!Prgs.ChkWMSCompleteTime(dtIssue_Detail, "Issue_Detail"))
            {
                return;
            }
            #endregion

            #region 更新庫存數量  ftyinventory
            var bsfio = (from m in dtIssue_Detail.AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = -m.Field<decimal>("qty"),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();

            var bs1 = (from b in dtIssue_Detail.AsEnumerable()
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
                           Qty = -m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            StringBuilder sqlupd2_B = new StringBuilder();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, false));
            string sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            #endregion

            #region UnConfirmed 廠商能上鎖→PMS更新→廠商更新

            // 先確認 WMS 能否上鎖, 不能直接 return
            if (!Prgs_WMS.WMSLock(dtIssue_Detail, dtOriFtyInventory, this.Name, EnumStatus.Unconfirm))
            {
                return;
            }

            // PMS 的資料更新
            Exception errMsg = null;
            List<AutoRecord> autoRecordList = new List<AutoRecord>();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update Issue set status = 'New', ApvName = '' , ApvDate = NULL, editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    // transactionscope 內, 準備 WMS 資料 & 將資料寫入 AutomationCreateRecord (Delete, Unconfirm)
                    Prgs_WMS.WMSprocess(false, dtIssue_Detail, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 1, autoRecord: autoRecordList);
                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                Prgs_WMS.WMSUnLock(false, dtIssue_Detail, this.Name, EnumStatus.UnLock, EnumStatus.Unconfirm, dtOriFtyInventory);
                this.ShowErr(errMsg);
                return;
            }

            // PMS 更新之後,才執行WMS
            Prgs_WMS.WMSprocess(false, dtIssue_Detail, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 2, autoRecord: autoRecordList);
            MyUtility.Msg.InfoBox("UnConfirmed successful");
            #endregion
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            this.labelConfirmed.Text = this.CurrentMaintain["status"].ToString();
            if (this.labelConfirmed.Text.ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }

            //--------------------------------------------------------------------------------------------//
            DataRow issue = this.CurrentMaintain;

            // 上方欄位
            string iD = issue["ID"].ToString();
            string issueDate = Convert.ToDateTime(issue["IssueDate"]).ToString("yyyy/MM/dd");
            string confirmTime = MyUtility.Convert.GetDate(issue["EditDate"]).Value.ToString("yyyy/MM/dd HH:mm:ss");
            string orderID = this.txtOrderID.Text;
            string line = this.displayLineNo.Text;
            string remark = issue["Remark"].ToString();
            string poID = this.displayPOID.Text;
            string style = this.displayStyle.Text;

            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", iD));
            pars.Add(new SqlParameter("@MDivision", Env.User.Keyword));
            pars.Add(new SqlParameter("@OrderID", orderID));

            #region Title
            DataTable dt;
            string cmdText = @"
select NameEN 
from MDivision 
where id = @MDivision";
            DBProxy.Current.Select(string.Empty, cmdText, pars, out dt);
            string rptTitle = dt.Rows[0]["NameEN"].ToString();

            #endregion

            #region Body

            DataTable dtBody;
            DualResult result;

            string cmd = $@"
SELECT f.Refno
		,iis.SCIRefno 
        ,iis.ColorID
		,[SuppColor]=SuppCol.SuppColor
		,[Seq]=iid.Seq1 +'-'+iid.Seq2
		,[Desc]=dbo.getmtldesc(iid.POID,iid.seq1,iid.seq2,2,0) 
		,[Issue_Detail_Qty]=Cast( iid.Qty as int)
		,[Issue_Summary_Qty]=Cast( iis.Qty as int)
		,[Unit]=Unit.StockUnit
		,[UnitDesc]=Unit.Description
		,[Location]= Stuff((SELECT Concat(',', MtlLocationID) 
						FROM (
							SELECT DISTINCT MtlLocationID 
							FROM FtyInventory_Detail 
							WITH(NOLOCK) WHERE Ukey= iid.FtyInventoryUkey and isnull(MtlLocationID,'') <> '' 
					 ) fty FOR xml path('')), 1, 1, '')
        ,[OutputQty] = ThreadUsedQtyByBOT.Qty
INTO #tmp
FROM Issue_Summary iis WITH(NOLOCK)
INNER JOIN Issue_Detail iid WITH(NOLOCK) ON iis.Id = iid.Id AND iis.Ukey = iid.Issue_SummaryUkey
INNER JOIN Fabric f WITH(NOLOCK) ON f.SCIRefno = iis.SCIRefno
OUTER APPLY(
	SELECT TOP 1 PSD.StockUnit  ,u.Description
	FROM PO_Supp_Detail PSD 
	INNER JOIN Unit u ON u.ID = PSD.StockUnit
    left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
	WHERE PSD.ID ='{poID}' AND PSD.SCIRefno=iis.SCIRefno AND isnull(psdsC.SpecValue, '') = iis.ColorID
)Unit
OUTER APPLY(
	 SELECt [Qty]=SUM(b.Qty)
	 FROM (
		    Select distinct o.ID,tcd.SCIRefNo, tcd.ColorID ,tcd.Article
		    From dbo.Orders as o
            INNER JOIN PO WITH (NOLOCK) ON po.StyleUkey = o.StyleUkey
		    Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
            Inner Join dbo.Style_ThreadColorCombo_History as tc On tc.StyleUkey = s.Ukey AND po.ThreadVersion = tc.Version
            Inner Join dbo.Style_ThreadColorCombo_History_Detail as tcd On tcd.Style_ThreadColorCombo_HistoryUkey  = tc.Ukey
		    WHERE O.ID=iis.POID AND tcd.Article IN ( SELECT Article FROM Issue_Breakdown WHERE ID = iis.Id)
		    ) a
	 INNER JOIN (		
				    SELECt Article,[Qty]=SUM(Qty) 
				    FROM Issue_Breakdown
				    WHERE ID = iis.Id
				    GROUP BY Article
			    ) b ON a.Article = b.Article
	 WHERE SCIRefNo=iis.SCIRefNo AND  ColorID= iis.ColorID
)ThreadUsedQtyByBOT
OUTER APPLY(
	SELECT psd.SuppColor
	FROM PO_Supp_Detail psd WITH(NOLOCK)
    left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
	WHERE psd.ID ='{poID}' AND psd.SCIRefno = iis.SCIRefno AND isnull(psdsC.SpecValue, '') = iis.Colorid
	AND psd.SEQ1 =iid.Seq1 AND psd.SEQ2 = iid.Seq2
)SuppCol
WHERE iis.ID='{iD}'


SELECT [Refno]
/*
[Refno]=IIF( Seq <>
				(
					SELECT TOP 1 Seq
					FROM #tmp
					WHERE Refno=t.Refno AND ColorID=t.ColorID
					ORDER BY Seq
				), '' ,Refno )*/
        ,ColorID
		,SuppColor
		,Seq
		,[Desc]=IIF( Seq <>
				(
					SELECT TOP 1 Seq
					FROM #tmp
					WHERE Refno=t.Refno AND ColorID=t.ColorID
					ORDER BY Seq
				), '' ,t.[Desc] )
		,Issue_Detail_Qty
		,[Issue_Summary_Qty]=IIF( Seq <>
				(
					SELECT TOP 1 Seq
					FROM #tmp
					WHERE Refno=t.Refno AND ColorID=t.ColorID
					ORDER BY Seq
				) OR Issue_Detail_Qty = Issue_Summary_Qty, '' ,'= '+Cast(t.Issue_Summary_Qty as char))
		
		,[Unit]=IIF( Seq <>
				(
					SELECT TOP 1 Seq
					FROM #tmp
					WHERE Refno=t.Refno AND ColorID=t.ColorID
					ORDER BY Seq
				), '' , t.Unit)
		
		,[UnitDesc]=IIF( Seq <>
				(
					SELECT TOP 1 Seq
					FROM #tmp
					WHERE Refno=t.Refno AND ColorID=t.ColorID
					ORDER BY Seq
				), '' , t.UnitDesc)
		,[Location]=t.Location /*IIF( Seq <>
				(
					SELECT TOP 1 Seq
					FROM #tmp
					WHERE Refno=t.Refno AND ColorID=t.ColorID
					ORDER BY Seq
				), '' , t.Location)*/
        , t.OutputQty
FROM #tmp t

DROP TABLE #tmp
";
            result = DBProxy.Current.Select(string.Empty, cmd, out dtBody);
            #endregion

            #region RDLC
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", iD));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Issuetime", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("OrderID", orderID));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Style", style));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Line", line));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("confirmTime", confirmTime));

            // report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("poID", poID));
            List<P33_PrintData> printDatas = dtBody.AsEnumerable().Select(o => new P33_PrintData()
            {
                RefNo = o["RefNo"].ToString().Trim(),
                Color = o["ColorID"].ToString().Trim(),
                SuppColor = o["SuppColor"].ToString().Trim(),
                Seq = o["Seq"].ToString().Trim(),
                Desc = o["Desc"].ToString().Trim(),
                Issue_Detail_Qty = o["Issue_Detail_Qty"].ToString().Trim(),
                Issue_Summary_Qty = o["Issue_Summary_Qty"].ToString().Trim(),
                Unit = o["Unit"].ToString().Trim(),
                UnitDesc = o["UnitDesc"].ToString().Trim(),
                Location = o["Location"].ToString().Trim(),
                OutputQty = o["OutputQty"].ToString().Trim(),
            }).ToList();

            report.ReportDataSource = printDatas;

            Type reportResourceNamespace = typeof(P33_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P33_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
            {
                // this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report);
            frm.MdiParent = this.MdiParent;
            frm.Show();
            #endregion

            return base.ClickPrint();
        }
        #endregion

        #region 控制項事件

        private void TxtOrderID_Validating(object sender, CancelEventArgs e)
        {
            string currentOrderID = this.txtOrderID.Text;

            if (MyUtility.Check.Empty(currentOrderID))
            {
                this.displayPOID.Text = string.Empty;
                this.displayStyle.Text = string.Empty;
                this.poid = string.Empty;
                return;
            }

            #region 防呆

            if (!MyUtility.Check.Seek($"SELECT 1 FROM Orders WHERE ID ='{currentOrderID}' "))
            {
                MyUtility.Msg.InfoBox($"<{currentOrderID}> not found !!");
                this.txtOrderID.Focus();
                this.txtOrderID.Text = string.Empty;
                return;
            }

            string currentMDivisionID = MyUtility.GetValue.Lookup($"SELECT MDivisionID FROM Orders WHERE  ID ='{currentOrderID}' ");

            if (currentMDivisionID != Env.User.Keyword)
            {
                MyUtility.Msg.InfoBox($"<{currentOrderID}> M is {currentMDivisionID}. not the same login M. can't release !!");
                this.txtOrderID.Focus();
                return;
            }
            #endregion

            // 取得POID
            this.RefreshOrderField(currentOrderID);
        }

        private void TxtOrderID_Validated(object sender, EventArgs e)
        {
            string currentOrderID = this.txtOrderID.Text;

            // 取得POID
            string pOID = MyUtility.GetValue.Lookup($"SELECT POID FROM Orders WHERE ID ='{currentOrderID}' ");

            if (MyUtility.Check.Empty(pOID))
            {
                this.dtIssueBreakDown = null;
                this.gridIssueBreakDown.DataSource = null;
                foreach (DataRow dr in this.DetailDatas)
                {
                    // 刪除SubDetail資料
                    ((DataTable)this.detailgridbs.DataSource).Rows.Remove(dr);
                    dr.Delete();
                }

                this.displayLineNo.Text = string.Empty;
                return;
            }

            // 根據SP#，帶出這張訂單會用到的線材資訊(線的種類以及顏色)
            DualResult result = this.Detail_Reload();

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return;
            }

            this.displayLineNo.Text = MyUtility.GetValue.Lookup($@"
SELECT t.sewline + ',' 
FROM(SELECT DISTINCT o.sewline FROM dbo.issue_detail a WITH (nolock) 
INNER JOIN dbo.orders o WITH (nolock) ON a.poid = o.poid  
WHERE o.id = '{currentOrderID}'  AND o.sewline != '') t FOR xml path('')
");

            this.Ismatrix_Reload = true;
            this.IssueBreakDown_Reload();
            this.detailgridbs.Position = 0;
            this.detailgrid.Focus();
            this.detailgrid.CurrentCell = this.detailgrid[0, 0];
            this.detailgrid.BeginEdit(true);
        }

        private void BtnBreakDown_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["OrderId"]))
            {
                MyUtility.Msg.WarningBox("Please key-in Order ID first!!");
                return;
            }

            var frm = new P33_IssueBreakDown(this.CurrentMaintain, this.dtIssueBreakDown, this.dtSizeCode);
            frm.ShowDialog(this);
            this.OnDetailEntered();
        }

        private void CheckByCombo_CheckedChanged(object sender, EventArgs e)
        {
            if (this.dtIssueBreakDown == null)
            {
                return;
            }

            if (this.checkByCombo.Checked)
            {
                this.dtIssueBreakDown.DefaultView.RowFilter = string.Format(string.Empty);
            }
            else
            {
                this.dtIssueBreakDown.DefaultView.RowFilter = string.Format("OrderID='{0}'", this.txtOrderID.Text);
            }

            this.HideNullColumn(this.gridIssueBreakDown);
        }

        private void BtnAutoPick_Click(object sender, EventArgs e)
        {
            if (this.dtIssueBreakDown == null)
            {
                return;
            }

            List<IssueQtyBreakdown> modelList = new List<IssueQtyBreakdown>();

            // 檢查是否有勾選Combo，處理傳入AutoPick資料篩選
            if (!this.checkByCombo.Checked && this.dtIssueBreakDown != null)
            {
                foreach (DataRow tempRow in this.dtIssueBreakDown.Rows)
                {
                    if (tempRow["OrderID"].ToString() != this.txtOrderID.Text.ToString())
                    {
                        foreach (DataColumn tempColumn in this.dtIssueBreakDown.Columns)
                        {
                            if (tempRow[tempColumn].GetType().Name == "Decimal")
                            {
                                tempRow[tempColumn] = 0;
                            }
                        }
                    }
                }
            }

            foreach (DataRow tempRow in this.dtIssueBreakDown.Rows)
            {
                int totalQty = 0;
                foreach (DataColumn col in this.dtIssueBreakDown.Columns)
                {
                    IssueQtyBreakdown m = new IssueQtyBreakdown()
                    {
                        OrderID = tempRow["OrderID"].ToString(),
                        Article = tempRow["Article"].ToString(),
                    };

                    if (tempRow[col].GetType().Name == "Decimal")
                    {
                        totalQty += Convert.ToInt32(tempRow[col]);
                    }

                    if (col.ColumnName != "OrderID" && col.ColumnName != "Article")
                    {
                        m.SizeCode = col.ColumnName;

                        m.Qty = totalQty;
                        modelList.Add(m);
                        totalQty = 0;
                    }
                }
            }

            var frm = new P33_AutoPick(this.CurrentMaintain["id"].ToString(), this.poid, this.txtOrderID.Text.ToString(), this.dtIssueBreakDown, this.sbSizecode, this.checkByCombo.Checked, modelList);
            DialogResult result = frm.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                DataTable detail, subDetail;
                detail = (DataTable)this.detailgridbs.DataSource;

                // 刪除表身重新匯入
                foreach (DataRow del in this.DetailDatas)
                {
                    del.Delete();
                }

                foreach (DataRow key in frm.importRows)
                {
                    string pOID = key["POID"].ToString();
                    string sCIRefno = key["SCIRefno"].ToString();
                    string colorID = key["ColorID"].ToString();
                    string suppColor1 = key["SuppColor"].ToString();
                    string refno = key["Refno"].ToString();
                    string descDetail = key["DescDetail"].ToString();
                    string qty = MyUtility.Check.Empty(key["@Qty"].ToString()) ? "0" : key["@Qty"].ToString();
                    decimal useQtyByStockUnit = MyUtility.Convert.GetDecimal(key["Use Qty By Stock Unit"]);
                    string stockUnit = key["Stock Unit"].ToString();
                    decimal useQtyByUseUnit = MyUtility.Convert.GetDecimal(key["Use Qty By Use Unit"]);
                    string useUnit = key["Use Unit"].ToString();
                    string stockUnitDesc = key["Stock Unit Desc."].ToString();
                    string outputQty = key["Output Qty(Garment)"].ToString();
                    decimal balance = (decimal)key["Bulk Balance(Stock Unit)"];

                    // string FtyInventoryUkey = key["FtyInventoryUkey"].ToString();
                    string accuIssued = key["AccuIssued"].ToString();

                    DataRow nRow = detail.NewRow();
                    nRow["ID"] = this.CurrentMaintain["ID"];

                    nRow["POID"] = pOID;
                    nRow["SCIRefno"] = sCIRefno;
                    nRow["Refno"] = refno;
                    nRow["ColorID"] = colorID;
                    nRow["SuppColor"] = suppColor1;
                    nRow["DescDetail"] = descDetail;
                    nRow["@Qty"] = Convert.ToDecimal(qty);
                    nRow["Use Qty By Stock Unit"] = useQtyByStockUnit;
                    nRow["Stock Unit"] = stockUnit;
                    nRow["Use Qty By Use Unit"] = useQtyByUseUnit;
                    nRow["Use Unit"] = useUnit;
                    nRow["Stock Unit Desc."] = stockUnitDesc;
                    nRow["OutputQty"] = outputQty;
                    nRow["Balance(Stock Unit)"] = balance;
                    nRow["AccuIssued"] = accuIssued;

                    if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
                    {
                        // nRow["AccuIssued"] = 0.00;
                    }
                    else
                    {
                        accuIssued = MyUtility.GetValue.Lookup($@"
select isnull(sum([IS].qty),0)
from dbo.issue I WITH (NOLOCK) 
inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I.id = [IS].Id 
where I.type = 'E' and I.Status = 'Confirmed' 
and [IS].Poid='{pOID}' AND [IS].SCIRefno='{sCIRefno}' AND [IS].ColorID='{colorID}' and i.[EditDate]<'{Convert.ToDateTime(this.CurrentMaintain["AddDate"]).ToShortDateString()}'
");

                        nRow["AccuIssued"] = Convert.ToDecimal(accuIssued);
                    }

                    detail.Rows.Add(nRow);
                    decimal totalQty = 0;
                    if (this.GetSubDetailDatas(detail.Rows[detail.Rows.Count - 1], out subDetail))
                    {
                        List<DataRow> issuedList = Prgs.Thread_AutoPick(key, Convert.ToDecimal(accuIssued));
                        List<string> allSuppColor = new List<string>();
                        foreach (var issued in issuedList)
                        {
                            string suppColor = issued["SuppColor"].ToString();

                            // 重複就不加進去了
                            if (!allSuppColor.Contains(suppColor))
                            {
                                allSuppColor.Add(suppColor);
                            }

                            if (MyUtility.Convert.GetDecimal(issued["Qty"]) != 0)
                            {
                                totalQty += (decimal)issued["Qty"];

                                issued.AcceptChanges();
                                issued.SetAdded();
                                subDetail.ImportRow(issued);
                            }
                        }

                        if (allSuppColor.JoinToString(",").Length > 0)
                        {
                            detail.Rows[detail.Rows.Count - 1]["SuppColor"] = allSuppColor.JoinToString(",");
                        }

                        Sum_subDetail(detail.Rows[detail.Rows.Count - 1], subDetail);
                    }

                    // _subDetail.AcceptChanges();
                    detail.Rows[detail.Rows.Count - 1]["IssueQty"] = totalQty;
                    detail.Rows[detail.Rows.Count - 1]["Qty"] = totalQty;
                }

                this.detailgrid.SelectRowToNext();
                this.detailgrid.SelectRowToPrev();
            }
        }

        /// <inheritdoc/>
        protected override DualResult ClickSaveSubDetial(SubDetailSaveEventArgs e)
        {
            return base.ClickSaveSubDetial(e);
        }

        #endregion

        #region 自訂事件

        private DualResult Detail_Reload()
        {
            foreach (DataRow dr in this.DetailDatas)
            {
                // 刪除SubDetail資料
                ((DataTable)this.detailgridbs.DataSource).Rows.Remove(dr);
                dr.Delete();
            }

            DataTable subData;
            DualResult result;

            string pOID = this.poid;

            // 判斷訂單會用到那些物料
            // 勾選By Combo與否，會造成Article不一樣，因此在這個時候就必須找出：這些OrderID，用到哪些Article，這些Article用到哪些SCIRefno + ColorID 的物料
            // 下行註解, 先帶出此 POID 下全部子單的 Article, 按鈕 AutoPick 會排除不是此子單的 Article, 之後要改再說
            // string whereByCombo = this.checkByCombo.Checked ? string.Empty : "AND a.id = b.id";
            string articleWhere = $@"
	Select 1
	From dbo.Orders as b
    INNER JOIN PO WITH (NOLOCK) ON po.StyleUkey = b.StyleUkey
	INNER JOIN dbo.order_qty a WITH (NOLOCK)  on b.id = a.id
	Inner Join dbo.Style as s On s.Ukey = b.StyleUkey
	Inner Join dbo.Style_ThreadColorCombo_History  as tc On tc.StyleUkey = s.Ukey AND po.ThreadVersion = tc.Version
	Inner Join dbo.Style_ThreadColorCombo_History_Detail as tcd On tcd.Style_ThreadColorCombo_HistoryUkey = tc.Ukey
	WHERE b.ID = '{pOID}'
	AND exists (
		select 1
		from dbo.order_qty a WITH (NOLOCK)
		inner join dbo.orders b WITH (NOLOCK) on b.id = a.id
		where b.poid = '{pOID}' -- 找存在母單底下所有的 Article
        and a.Article = tcd.Article
		-----whereByCombo-----
	)
    
";
            string articleWhere1 = articleWhere + $@"
    AND tcd.SCIRefNo = psd.SCIRefno
    AND tcd.SuppColor = psd.SuppColor
";
            string articleWhere2 = articleWhere + $@"
	AND tcd.SCIRefNo = TR.FromSCIRefno
    AND tcd.SuppColor = TR.FromSuppColor
";

            string articleWhere1_outer = articleWhere1 + "\r\nAND tcd.Article = g.Article";
            string articleWhere2_outer = articleWhere2 + "\r\nAND tcd.Article = g.Article";

            // 回採購單找資料
            string sql = $@"

-----由於這時候觸發的SQL，還沒有Issue Breakdown，因此不用加入QUILTING數量
SELECT  DISTINCT
  psd.SCIRefno
, psd.Refno
, ColorID=isnull(psdsC.SpecValue, '')
, f.DescDetail
, [@Qty] = ISNULL(ThreadUsedQtyByBOT.Val,0)
, [AccuIssued] = (
					select isnull(sum([IS].qty),0)
					from dbo.issue I WITH (NOLOCK) 
					inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I.id = [IS].Id 
					where I.type = 'E' and I.Status = 'Confirmed' 
					and [IS].Poid=psd.id AND [IS].SCIRefno=PSD.SCIRefno AND [IS].ColorID=isnull(psdsC.SpecValue, '') and i.[EditDate]<GETDATE()
				)
, [IssueQty]=0.00
, [Use Qty By Stock Unit]=0.00
, [Stock Unit]=StockUnit.StockUnit
, [Use Qty By Use Unit]=0.00
, [Use Unit]='CM'
, [Stock Unit Desc.]=StockUnit.Description
, [OutputQty]=0.00
, [Balance(Stock Unit)]= 0.00
, [Location] = ''
, [POID]=psd.ID 
, o.MDivisionID
INTO #tmp
FROM PO_Supp_Detail psd
INNER JOIN Fabric f ON f.SCIRefno = psd.SCIRefno
INNER JOIN MtlType m ON m.id= f.MtlTypeID
INNER JOIN Orders o ON psd.ID = o.ID
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
OUTER APPLY(
	SELECT TOP 1 PSD2.StockUnit ,u.Description
	FROM PO_Supp_Detail PSD2 
	LEFT JOIN Unit u ON u.ID = psd2.StockUnit
    left join PO_Supp_Detail_Spec psdsC2 WITH (NOLOCK) on psdsC2.ID = psd2.id and psdsC2.seq1 = psd2.seq1 and psdsC2.seq2 = psd2.seq2 and psdsC2.SpecColumnID = 'Color'
	WHERE PSD2.ID = psd.id
	AND PSD2.SCIRefno=psd.SCIRefno
	AND isnull(psdsC2.SpecValue, '') = isnull(psdsC.SpecValue, '')
)StockUnit
OUTER APPLY(
	SELECT top 1 TR.FromSCIRefno,TR.FromSuppColor -- 理應是唯一
	FROM PO_Supp PS 
	INNER JOIN Thread_Replace_Detail_Detail TRDD ON PSD.SCIRefNo=TRDD.ToSCIRefno AND PSD.SuppColor=TRDD.ToBrandSuppColor AND PS.SuppID=TRDD.SuppID 
	INNER JOIN Thread_Replace_Detail TRD ON TRDD.Thread_Replace_DetailUkey = TRD.Ukey
	INNER JOIN Thread_Replace TR ON TRD.Thread_ReplaceUkey = TR.Ukey
	WHERE PS.ID = PSD.ID AND PSD.SEQ1=PS.SEQ1	
)TR1
OUTER APPLY(
	SELECT top 1 TR.FromSCIRefno,TR.FromSuppColor -- 理應是唯一
	FROM PO_Supp PS 
	INNER JOIN Thread_Replace_Detail_Detail TRDD ON PSD.SCIRefNo=TRDD.ToSCIRefno AND PSD.SuppColor=TRDD.ToBrandSuppColor --AND PS.SuppID=TRDD.SuppID 
	INNER JOIN Thread_Replace_Detail TRD ON TRDD.Thread_Replace_DetailUkey = TRD.Ukey
	INNER JOIN Thread_Replace TR ON TRD.Thread_ReplaceUkey = TR.Ukey
	WHERE PS.ID = PSD.ID AND PSD.SEQ1=PS.SEQ1	
)TR2
OUTER APPLY(
	SELECT
		FromSCIRefno = iif(TR1.FromSCIRefno is not null, TR1.FromSCIRefno, TR2.FromSCIRefno),
		FromSuppColor = iif(TR1.FromSuppColor is not null, TR1.FromSuppColor, TR2.FromSuppColor)
)TR
OUTER APPLY(
	SELECT Val=sum (g.OpThreadQty)
	FROM dbo.GetThreadUsedQtyByBOT(psd.ID,default) g
	WHERE g.SCIRefNo = psd.SCIRefno AND g.SuppColor = psd.SuppColor
    AND exists (
        {articleWhere1_outer}
	)
)ThreadUsedQtyByBOT1
OUTER APPLY(
	SELECT Val=sum (g.OpThreadQty)
	FROM dbo.GetThreadUsedQtyByBOT(psd.ID,default) g
	WHERE g.SCIRefNo= TR.FromSCIRefno AND g.SuppColor = TR.FromSuppColor  
    AND exists (
        {articleWhere2_outer}
	)
)ThreadUsedQtyByBOT2
OUTER APPLY(
    select
        Val = iif(psd.IsForOtherBrand = 1, ThreadUsedQtyByBOT2.Val, ThreadUsedQtyByBOT1.Val)
)ThreadUsedQtyByBOT
WHERE psd.id ='{pOID}' 
AND m.IsThread=1 
AND psd.FabricType ='A'
and isnull(psdsC.SpecValue, '') <> ''
and ((psd.IsForOtherBrand = 1 and exists(
    {articleWhere2}
))
or
(psd.IsForOtherBrand = 0 and exists(
    {articleWhere1}
)))


SELECT  
	  SCIRefno
	, Refno
	, ColorID
	, [SuppColor]=SuppCol.Val
	, DescDetail
	, [@Qty]  = sum([@Qty] )
	, [AccuIssued] = SUM(AccuIssued)
	, [IssueQty]
	, [Use Qty By Stock Unit]
	, [Stock Unit]
	, [Use Qty By Use Unit]
	, [Use Unit]
	, [Stock Unit Desc.]
	, [OutputQty]
	, [Balance(Stock Unit)]
	, [Location] 
	, [POID]
	, MDivisionID
FROM #tmp t
OUTER APPLY(
	----列出所有Seq1 Seq2對應到的SuppColor
	SELECT [Val]=STUFF((
		SELECT  DISTINCT ',' + SuppColor
		FROM PO_Supp_Detail y
        left join PO_Supp_Detail_Spec psdsCy WITH (NOLOCK) on psdsCy.ID = y.id and psdsCy.seq1 = y.seq1 and psdsCy.seq2 = y.seq2 and psdsCy.SpecColumnID = 'Color'
		WHERE EXISTS( 
			SELECT 1 
			FROM PO_Supp_Detail psd
			LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
            left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
			WHERE psd.SCIRefno=t.SCIRefno AND t.ColorID = isnull(psdsC.SpecValue, '') AND t.POID = psd.ID
			AND psd.SCIRefno = y.SCIRefno AND isnull(psdsC.SpecValue, '') = isnull(psdsCy.SpecValue, '') AND t.POID = y.ID
			AND psd.SEQ1 = y.SEQ1 AND psd.SEQ2 = y.SEQ2
		)
		FOR XML PATH('')
	),1,1,'')
)SuppCol
GROUP BY 
	  SCIRefno
	, Refno
	, ColorID
	, SuppCol.Val
	, DescDetail
	, [IssueQty]
	, [Use Qty By Stock Unit]
	, [Stock Unit]
	, [Use Qty By Use Unit]
	, [Use Unit]
	, [Stock Unit Desc.]
	, [OutputQty]
	, [Balance(Stock Unit)]
	, [Location] 
	, [POID]
	, MDivisionID
ORDER BY SCIRefno,ColorID

DROP TABLE #tmp

";
            result = DBProxy.Current.Select(null, sql, out subData);
            if (!result)
            {
                return result;
            }

            if (subData.Rows.Count == 0)
            {
                this.txtOrderID.Text = string.Empty;
                return Ict.Result.F("No Issue Thread Data !");
            }

            foreach (DataRow dr in subData.Rows)
            {
                DataTable detailDt = (DataTable)this.detailgridbs.DataSource;
                if (detailDt != null)
                {
                    DataRow ndr = detailDt.NewRow();

                    ndr["SCIRefno"] = dr["SCIRefno"];
                    ndr["Refno"] = dr["Refno"];
                    ndr["ColorID"] = dr["ColorID"];
                    ndr["SuppColor"] = dr["SuppColor"];
                    ndr["POID"] = dr["POID"];
                    ndr["DescDetail"] = dr["DescDetail"];
                    ndr["@Qty"] = dr["@Qty"];
                    ndr["Use Unit"] = dr["Use Unit"];
                    ndr["AccuIssued"] = dr["AccuIssued"];
                    ndr["IssueQty"] = dr["IssueQty"];
                    ndr["Use Qty By Stock Unit"] = dr["Use Qty By Stock Unit"];
                    ndr["Stock Unit"] = dr["Stock Unit"];
                    ndr["Use Qty By Use Unit"] = dr["Use Qty By Use Unit"];
                    ndr["Use Unit"] = dr["Use Unit"];
                    ndr["Stock Unit Desc."] = dr["Stock Unit Desc."];
                    ndr["OutputQty"] = dr["OutputQty"];
                    ndr["Balance(Stock Unit)"] = dr["Balance(Stock Unit)"];
                    ndr["Location"] = dr["Location"];
                    ndr["MDivisionID"] = dr["MDivisionID"];
                    detailDt.Rows.Add(ndr);
                }
            }

            return Ict.Result.True;
        }

        private DualResult Matrix_Reload()
        {
            if (this.EditMode == true && this.Ismatrix_Reload == false)
            {
                return Ict.Result.True;
            }

            this.Ismatrix_Reload = false;
            string sqlcmd;
            StringBuilder sbIssueBreakDown;
            DualResult result;

            string orderID = this.txtOrderID.Text;

            sqlcmd = string.Format(
                @"select sizecode from dbo.order_sizecode WITH (NOLOCK) 
where id = (select poid from dbo.orders WITH (NOLOCK) where id='{0}') order by seq", orderID);

            if (!(result = DBProxy.Current.Select(null, sqlcmd, out this.dtSizeCode)))
            {
                this.ShowErr(sqlcmd, result);
                return Ict.Result.True;
            }

            if (this.dtSizeCode.Rows.Count == 0)
            {
                // MyUtility.Msg.WarningBox(string.Format("Becuase there no sizecode data belong this OrderID {0} , can't show data!!", CurrentDataRow["orderid"]));
                this.dtIssueBreakDown = null;
                this.gridIssueBreakDown.DataSource = null;
                return Ict.Result.True;
            }

            this.sbSizecode = new StringBuilder();
            this.sbSizecode.Clear();
            for (int i = 0; i < this.dtSizeCode.Rows.Count; i++)
            {
                this.sbSizecode.Append(string.Format(@"[{0}],", this.dtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()));
            }

            sbIssueBreakDown = new StringBuilder();
            sbIssueBreakDown.Append(string.Format(
                @";with Bdown as 
            (select a.ID [orderid],a.Article,a.SizeCode,a.Qty from dbo.order_qty a WITH (NOLOCK) 
            inner join dbo.orders b WITH (NOLOCK) on b.id = a.id
            where b.POID=(select poid from dbo.orders WITH (NOLOCK) where id = '{0}')
            )
            ,Issue_Bdown as
            (
            	select isnull(Bdown.orderid,ib.OrderID) [OrderID],isnull(Bdown.Article,ib.Article) Article,isnull(Bdown.SizeCode,ib.sizecode) sizecode,isnull(ib.Qty,0) qty
            	from Bdown full outer join (select * from dbo.Issue_Breakdown WITH (NOLOCK) where id='{1}') ib
            	on Bdown.orderid = ib.OrderID and Bdown.Article = ib.Article and Bdown.SizeCode = ib.SizeCode
            )
            select * from Issue_Bdown
            pivot
            (
            	sum(qty)
            	for sizecode in ({2})
            )as pvt
            order by [OrderID],[Article]",
                orderID,
                this.CurrentMaintain["id"],
                this.sbSizecode.ToString().Substring(0, this.sbSizecode.ToString().Length - 1)));
            if (!(result = DBProxy.Current.Select(null, sbIssueBreakDown.ToString(), out this.dtIssueBreakDown)))
            {
                this.ShowErr(sqlcmd, result);
                return Ict.Result.True;
            }

            this.gridIssueBreakDown.AutoGenerateColumns = true;
            this.gridIssueBreakDownBS.DataSource = this.dtIssueBreakDown;
            this.gridIssueBreakDown.DataSource = this.gridIssueBreakDownBS;
            this.gridIssueBreakDown.IsEditingReadOnly = true;
            this.gridIssueBreakDown.ReadOnly = true;

            this.CheckByCombo_CheckedChanged(null, null);

            return Ict.Result.True;
        }

        private DualResult IssueBreakDown_Reload()
        {
            if (this.EditMode == true && this.Ismatrix_Reload == false)
            {
                return Ict.Result.True;
            }

            this.Ismatrix_Reload = false;
            string sqlcmd;
            StringBuilder sbIssueBreakDown;
            DualResult result;

            string orderID = this.txtOrderID.Text;

            // 取得該訂單Sizecode
            sqlcmd = $@"select sizecode from dbo.order_sizecode WITH (NOLOCK) 
where id = (select poid from dbo.orders WITH (NOLOCK) where id='{orderID}') order by seq";

            if (!(result = DBProxy.Current.Select(null, sqlcmd, out this.dtSizeCode)))
            {
                this.ShowErr(sqlcmd, result);
                return Ict.Result.True;
            }

            if (this.dtSizeCode.Rows.Count == 0)
            {
                this.dtIssueBreakDown = null;
                this.gridIssueBreakDown.DataSource = null;
                return Ict.Result.True;
            }

            this.sbSizecode = new StringBuilder();
            this.sbSizecode.Clear();
            for (int i = 0; i < this.dtSizeCode.Rows.Count; i++)
            {
                this.sbSizecode.Append($@"[{this.dtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()}],");
            }

            sbIssueBreakDown = new StringBuilder();

            sbIssueBreakDown.Append($@"
;with Bdown as 
(
    select a.ID [orderid],a.Article,a.SizeCode,a.Qty from dbo.order_qty a WITH (NOLOCK) 
    inner join dbo.orders b WITH (NOLOCK) on b.id = a.id
    where b.POID=( select poid from dbo.orders WITH (NOLOCK) where id = '{orderID}' )
)
,Issue_Bdown as
(
    select isnull(Bdown.orderid,ib.OrderID) [OrderID],isnull(Bdown.Article,ib.Article) Article,isnull(Bdown.SizeCode,ib.sizecode) sizecode,isnull(ib.Qty,0) qty
    from Bdown full 
    outer join (select * from dbo.Issue_Breakdown WITH (NOLOCK) 
    where id='{this.CurrentMaintain["id"]}') ib
    on Bdown.orderid = ib.OrderID and Bdown.Article = ib.Article and Bdown.SizeCode = ib.SizeCode
)
select * from Issue_Bdown
pivot
(
    sum(qty)
    for sizecode in ({this.sbSizecode.ToString().Substring(0, this.sbSizecode.ToString().Length - 1)})
)as pvt
order by [OrderID],[Article]
");
            if (!(result = DBProxy.Current.Select(null, sbIssueBreakDown.ToString(), out this.dtIssueBreakDown)))
            {
                this.ShowErr(sqlcmd, result);
                return Ict.Result.True;
            }

            this.gridIssueBreakDown.AutoGenerateColumns = true;
            this.gridIssueBreakDownBS.DataSource = this.dtIssueBreakDown;
            this.gridIssueBreakDown.DataSource = this.gridIssueBreakDownBS;
            this.gridIssueBreakDown.IsEditingReadOnly = true;
            this.gridIssueBreakDown.ReadOnly = true;

            this.CheckByCombo_CheckedChanged(null, null);

            return Ict.Result.True;
        }

        private void HideNullColumn(Win.UI.Grid grid)
        {
            List<string> nullCol = new List<string>();
            foreach (DataGridViewColumn column in grid.Columns)
            {
                column.Visible = true;
                int rowCount = 0;
                int nullCount = 0;
                string columnName = column.Name;
                if (columnName != "Selected" && columnName != "Article" && columnName != "OrderID")
                {
                    foreach (DataGridViewRow row in grid.Rows)
                    {
                        string val = row.Cells[columnName].Value.ToString();
                        if (MyUtility.Check.Empty(val))
                        {
                            nullCount++;
                        }

                        rowCount++;
                    }

                    if (rowCount == nullCount)
                    {
                        nullCol.Add(columnName);
                    }
                }
            }

            foreach (var col in nullCol)
            {
                grid.Columns[col].Visible = false;
            }
        }

        /// <inheritdoc/>
        public static void ProcessWithDatatable2(DataTable source, string tmp_columns, string sqlcmd, out DataTable result, string temptablename = "#tmp")
        {
            result = null;
            StringBuilder sb = new StringBuilder();
            if (temptablename.TrimStart().StartsWith("#"))
            {
                sb.Append(string.Format("create table {0} (", temptablename));
            }
            else
            {
                sb.Append(string.Format("create table #{0} (", temptablename));
            }

            string[] cols = tmp_columns.Split(',');
            for (int i = 0; i < cols.Length; i++)
            {
                if (MyUtility.Check.Empty(cols[i]))
                {
                    continue;
                }

                switch (Type.GetTypeCode(source.Columns[cols[i]].DataType))
                {
                    case TypeCode.Boolean:
                        sb.Append(string.Format("[{0}] bit", cols[i]));
                        break;

                    case TypeCode.Char:
                        sb.Append(string.Format("[{0}] varchar(1)", cols[i]));
                        break;

                    case TypeCode.DateTime:
                        sb.Append(string.Format("[{0}] datetime", cols[i]));
                        break;

                    case TypeCode.Decimal:
                        sb.Append(string.Format("[{0}] numeric(24,8)", cols[i]));
                        break;

                    case TypeCode.Int32:
                        sb.Append(string.Format("[{0}] int", cols[i]));
                        break;

                    case TypeCode.String:
                        sb.Append(string.Format("[{0}] varchar(max)", cols[i]));
                        break;

                    case TypeCode.Int64:
                        sb.Append(string.Format("[{0}] bigint", cols[i]));
                        break;
                    default:
                        break;
                }

                if (i < cols.Length - 1)
                {
                    sb.Append(",");
                }
            }

            sb.Append(")");

            SqlConnection conn;
            DBProxy.Current.OpenConnection(null, out conn);

            try
            {
                DualResult result2 = DBProxy.Current.ExecuteByConn(conn, sb.ToString());
                if (!result2)
                {
                    MyUtility.Msg.ShowException(null, result2);
                    return;
                }

                using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                {
                    bulkcopy.BulkCopyTimeout = 60;
                    if (temptablename.TrimStart().StartsWith("#"))
                    {
                        bulkcopy.DestinationTableName = temptablename.Trim();
                    }
                    else
                    {
                        bulkcopy.DestinationTableName = string.Format("#{0}", temptablename.Trim());
                    }

                    for (int i = 0; i < cols.Length; i++)
                    {
                        bulkcopy.ColumnMappings.Add(cols[i], cols[i]);
                    }

                    bulkcopy.WriteToServer(source);
                    bulkcopy.Close();
                }

                result2 = DBProxy.Current.SelectByConn(conn, sqlcmd, out result);
                if (!result2)
                {
                    MyUtility.Msg.ShowException(null, result2);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        private static void Sum_subDetail(DataRow target, DataTable source)
        {
            target["Qty"] = (source.Rows.Count == 0) ? 0m : source.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted)
                .Sum(r => r.Field<decimal>("Qty"));
        }

        // 將IssueBreakDown整理成Datatable
        private DataTable Convert_IssueBreakDown_ToDataTable()
        {
            DataTable issueBreakDown_Dt = new DataTable();
            issueBreakDown_Dt.Columns.Add(new DataColumn() { ColumnName = "OrderID", DataType = typeof(string) });
            issueBreakDown_Dt.Columns.Add(new DataColumn() { ColumnName = "Article", DataType = typeof(string) });
            issueBreakDown_Dt.Columns.Add(new DataColumn() { ColumnName = "Qty", DataType = typeof(int) });

            List<IssueQtyBreakdown> modelList = new List<IssueQtyBreakdown>();

            // 檢查是否有勾選Combo，處理傳入AutoPick資料篩選
            if (!this.checkByCombo.Checked && this.dtIssueBreakDown != null)
            {
                foreach (DataRow tempRow in this.dtIssueBreakDown.Rows)
                {
                    if (tempRow["OrderID"].ToString() != this.txtOrderID.Text.ToString())
                    {
                        foreach (DataColumn tempColumn in this.dtIssueBreakDown.Columns)
                        {
                            if (tempRow[tempColumn].GetType().Name == "Decimal")
                            {
                                tempRow[tempColumn] = 0;
                            }
                        }
                    }
                }
            }

            foreach (DataRow tempRow in this.dtIssueBreakDown.Rows)
            {
                IssueQtyBreakdown m = new IssueQtyBreakdown()
                {
                    OrderID = tempRow["OrderID"].ToString(),
                    Article = tempRow["Article"].ToString(),
                };

                int totalQty = 0;
                foreach (DataColumn col in this.dtIssueBreakDown.Columns)
                {
                    if (tempRow[col].GetType().Name == "Decimal")
                    {
                        totalQty += Convert.ToInt32(tempRow[col]);
                    }
                }

                m.Qty = totalQty;
                modelList.Add(m);
            }

            foreach (var model in modelList)
            {
                if (model.Qty > 0)
                {
                    DataRow newDr = issueBreakDown_Dt.NewRow();
                    newDr["OrderID"] = model.OrderID;
                    newDr["Article"] = model.Article;
                    newDr["Qty"] = model.Qty;

                    issueBreakDown_Dt.Rows.Add(newDr);
                }
            }

            return issueBreakDown_Dt;
        }

        private bool Is_IssueBreakDownEmpty()
        {
            if (this.dtIssueBreakDown == null)
            {
                return true;
            }

            DataTable issueBreakDown_Dt = this.Convert_IssueBreakDown_ToDataTable();

            if (issueBreakDown_Dt.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private DataTable UnPivotBrkQty()
        {
            List<IssueQtyBreakdown> modelList = new List<IssueQtyBreakdown>();
            foreach (DataRow tempRow in this.dtIssueBreakDown.Rows)
            {
                int totalQty = 0;
                foreach (DataColumn col in this.dtIssueBreakDown.Columns)
                {
                    IssueQtyBreakdown m = new IssueQtyBreakdown()
                    {
                        OrderID = tempRow["OrderID"].ToString(),
                        Article = tempRow["Article"].ToString(),
                    };

                    if (tempRow[col].GetType().Name == "Decimal")
                    {
                        totalQty += Convert.ToInt32(tempRow[col]);
                    }

                    if (col.ColumnName != "OrderID" && col.ColumnName != "Article")
                    {
                        m.SizeCode = col.ColumnName;

                        m.Qty = totalQty;
                        modelList.Add(m);
                        totalQty = 0;
                    }
                }
            }

            DataTable t = new DataTable();
            t.Columns.Add(new DataColumn() { ColumnName = "Article", DataType = typeof(string) });
            t.Columns.Add(new DataColumn() { ColumnName = "SizeCode", DataType = typeof(string) });
            t.Columns.Add(new DataColumn() { ColumnName = "Qty", DataType = typeof(int) });

            var groupByData = modelList.GroupBy(o => new { o.Article, o.SizeCode }).Select(o => new
            {
                o.Key.Article,
                o.Key.SizeCode,
                Qty = o.Sum(x => x.Qty),
            }).ToList();

            foreach (var model in groupByData)
            {
                if (model.Qty > 0)
                {
                    DataRow newDr = t.NewRow();
                    newDr["Article"] = model.Article;
                    newDr["SizeCode"] = model.SizeCode;
                    newDr["Qty"] = model.Qty;

                    t.Rows.Add(newDr);
                }
            }

            return t;
        }

        private void RefnoCellValidating_QT(string refno, DataTable t, Ict.Win.UI.DataGridViewCellValidatingEventArgs e)
        {
            string colorID = MyUtility.Convert.GetString(this.CurrentDetailData["ColorID"]);

            #region SQL
            string sqlcmd = $@"
SELECt Article,[Qty]=SUM(Qty) 
INTO #tmp_sumQty
FROm #tmp
GROUP BY Article

Select distinct o.ID,tcd.SCIRefNo, tcd.ColorID ,tcd.Article 
INTO #step1
From dbo.Orders as o
INNER JOIN PO WITH (NOLOCK) ON po.StyleUkey = o.StyleUkey
Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
Inner Join dbo.Style_ThreadColorCombo_History as tc On tc.StyleUkey = s.Ukey AND po.ThreadVersion = tc.Version
Inner Join dbo.Style_ThreadColorCombo_History_Detail as tcd On tcd.Style_ThreadColorCombo_HistoryUkey  = tc.Ukey
WHERE O.ID='{this.poid}' AND tcd.Article IN ( SELECT Article FROM #tmp )

----------------Quiting用量計算--------------------------

select P.ID,OQ.Article,OQ.SizeCode,OCC.FabricPanelCode,OCC.FabricCode
,OE.ConsPC
,[Number of Needle for QT] =  ceiling(sqh.Width/sqh.HSize/sqh.NeedleDistance)
INTO #tmpQT
from Orders O
Inner join Order_Qty OQ on O.ID=OQ.ID
Inner join PO P on O.POID=P.ID
inner join Order_ColorCombo OCC on O.POID=OCC.Id and OQ.Article=OCC.Article and OCC.FabricCode is not null and OCC.FabricCode !=''
inner join Order_EachCons oe WITH (NOLOCK) on oe.id = occ.id and oe.FabricCombo = occ.PatternPanel and oe.CuttingPiece = 0 
inner join Order_EachCons_SizeQty OEZ on OE.Ukey=OEZ.Order_EachConsUkey and OQ.SizeCode=OEZ.SizeCode 
Inner join Style_QTThreadColorCombo_History SQH on O.styleUkey=SQH.styleUkey and SQH.FabricCode=OE.FabricCode and SQH.FabricPanelCode=OE.FabricPanelCode and P.ThreadVersion=SQH.Version
where o.ID = '{this.poid}'

select O.ID,OQ.Article,OQ.SizeCode,OCC.FabricPanelCode,OCC.FabricCode
,SQHD.Seq,SQHD.SCIRefno,SQHD.ColorID,QTt.Val
INTO #tmpQTFinal
from Orders O
Inner join Order_Qty OQ on O.ID=OQ.ID
Inner join PO P on O.POID=P.ID
inner join Order_ColorCombo OCC on O.POID=OCC.Id and OQ.Article=OCC.Article and OCC.FabricCode is not null and OCC.FabricCode !=''
Inner join Style_QTThreadColorCombo_History SQH on O.styleUkey=SQH.styleUkey and SQH.FabricCode=OCC.FabricCode and SQH.FabricPanelCode=OCC.FabricPanelCode and P.ThreadVersion=SQH.Version
Inner join Style_QTThreadColorCombo_History_Detail SQHD on SQH.Ukey=SQHD.Style_QTThreadColorCombo_HistoryUkey and OQ.Article=SQHD.Article
OUTER APPLY(
	SELECT Val=[Number of Needle for QT]* SQHD.Ratio * 0.9144 * qt.ConsPC * s.Qty
	FROM #tmpQT qt
	LEFT JOIN #tmp s ON qt.Article = s.Article AND qt.SizeCode = s.SizeCode
	WHERE qt.ID=o.ID AND qt.Article=oq.Article AND qt.SizeCode=oq.SizeCode
	AND qt.FabricPanelCode=OCC.FabricPanelCode AND qt.FabricCode =OCC.FabricCode
)QTt
where o.ID = '{this.poid}'

----------------Quiting用量計算--------------------------

SELECT  DISTINCT
  psd.SCIRefno
, psd.Refno
, ColorID = isnull(psdsC.SpecValue, '')
, f.DescDetail
, [@Qty] = ISNULL(ThreadUsedQtyByBOT.Val,0) + ISNULL(QT.Val,0)
, [AccuIssued] = (
					select isnull(sum([IS].qty),0)
					from dbo.issue I WITH (NOLOCK) 
					inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I.id = [IS].Id 
					where I.type = 'E' and I.Status = 'Confirmed' 
					and [IS].Poid=psd.id AND [IS].SCIRefno=PSD.SCIRefno AND [IS].ColorID=isnull(psdsC.SpecValue, '') and i.[EditDate]<GETDATE()
				)
, [IssueQty]=0.00
, [Use Qty By Stock Unit] = CEILING( ISNULL(ThreadUsedQtyByBOT.Qty,0) * (ISNULL(ThreadUsedQtyByBOT.Val,0) + ISNULL(QT.Val,0)) / 100 * ISNULL(UnitRate.RateValue,1) )
, [Stock Unit]=StockUnit.StockUnit
, [Use Qty By Use Unit] = (ThreadUsedQtyByBOT.Qty * (ISNULL(ThreadUsedQtyByBOT.Val,0) + ISNULL(QT.Val,0)) )
, [Use Unit]='CM'
, [Stock Unit Desc.]=StockUnit.Description
, [OutputQty] = ISNULL(ThreadUsedQtyByBOT.Qty,0)
, [Balance(Stock Unit)]= 0.00
, [Location] = ''
, [POID]=psd.ID 
, o.MDivisionID
INTO #final
FROM PO_Supp_Detail psd
INNER JOIN Fabric f ON f.SCIRefno = psd.SCIRefno
INNER JOIN MtlType m ON m.id= f.MtlTypeID
INNER JOIN Orders o ON psd.ID = o.ID
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
OUTER APPLY(
	SELECT TOP 1 PSD2.StockUnit ,u.Description
	FROM PO_Supp_Detail PSD2 
	LEFT JOIN Unit u ON u.ID = psd2.StockUnit
    inner join PO_Supp_Detail_Spec psdsC2 WITH (NOLOCK) on psdsC2.ID = PSD2.id and psdsC2.seq1 = PSD2.seq1 and psdsC2.seq2 = PSD2.seq2 and psdsC2.SpecColumnID = 'Color'
	WHERE PSD2.ID = psd.id
	AND PSD2.SCIRefno=psd.SCIRefno
	AND isnull(psdsC2.SpecValue, '')=isnull(psdsC.SpecValue, '')
)StockUnit
OUTER APPLY(
	SELECT SCIRefNo
		,ColorID
		,[Val]=sum (g.OpThreadQty)
		,[Qty] = (	
			SELECt [Qty]=SUM(b.Qty)
			FROM #step1 a
			INNER JOIN #tmp_sumQty b ON a.Article = b.Article
			WHERE SCIRefNo=psd.SCIRefNo AND  ColorID= isnull(psdsC.SpecValue, '') AND a.Article=g.Article
			GROUP BY a.Article
		)
	FROM DBO.GetThreadUsedQtyByBOT(psd.ID,default) g
	WHERE SCIRefNo= psd.SCIRefNo AND ColorID = isnull(psdsC.SpecValue, '')  
	AND Article IN (
		SELECt Article FROM #step1 WHERE SCIRefNo = psd.SCIRefNo  AND ColorID = isnull(psdsC.SpecValue, '')
	)
	GROUP BY SCIRefNo,ColorID , Article
)ThreadUsedQtyByBOT
OUTER APPLY(
	SELECT Val = SUM(t.Val)
	FROM #tmpQTFinal　ｔ
	WHERE t.SCIRefNo=psd.SCIRefno AND t.ColorID=isnull(psdsC.SpecValue, '')
)QT
OUTER APPLY(
	SELECT RateValue = IIF(Denominator = 0,0, Numerator / Denominator)
	FROM Unit_Rate
	WHERE UnitFrom='M' and  UnitTo = StockUnit.StockUnit
)UnitRate
WHERE psd.id ='{this.poid}' 
AND m.IsThread=1 
AND psd.FabricType ='A'
and isnull(psdsC.SpecValue, '') <> ''

AND psd.Refno='{refno}'

";

            if (!MyUtility.Check.Empty(colorID))
            {
                sqlcmd += $"AND isnull(psdsC.SpecValue, '')='{colorID}' ";
            }
            else
            {
                sqlcmd += $"AND isnull(psdsC.SpecValue, '') <> '' ";
            }

            sqlcmd += $@"
SELECT    SCIRefno 
        , Refno
        , ColorID
		, DescDetail
		, [@Qty] = SUM([@Qty])
        , [AccuIssued]
        , [IssueQty]
        , [Use Qty By Stock Unit] = CEILING (SUM([Use Qty By Stock Unit] ))
        , [Stock Unit]
        , [Use Qty By Use Unit] = SUM([Use Qty By Use Unit] )
        , [Use Unit]
        , [Stock Unit Desc.]
        , [OutputQty] = SUM([OutputQty])
        , [Balance(Stock Unit)] = 0
        , [Location] 
        , [POID]
        , MDivisionID
INTO #final2
FROM #final
GROUP BY SCIRefno 
        , Refno
        , ColorID
		, DescDetail
        , [AccuIssued]
        , [IssueQty]
        , [Stock Unit]
        , [Use Unit]
        , [Stock Unit Desc.]
        , [Balance(Stock Unit)]
        , [Location] 
        , [POID]
        , MDivisionID


SELECT    SCIRefno 
        , Refno
        , ColorID
		, SuppColor = RealSuppCol.Val
		, DescDetail
		, [@Qty] 
        , [AccuIssued]
        , [IssueQty]
        , [Use Qty By Stock Unit]
        , [Stock Unit]
        , [Use Qty By Use Unit] 
        , [Use Unit]
        , [Stock Unit Desc.]
        , [OutputQty]
        , [Balance(Stock Unit)] = Balance.Qty
        , [Location] 
        , [POID]
        , MDivisionID
FROM #final2 t
OUTER APPLY(
	SELECT [Qty]=ISNULL(( SUM(Fty.InQty - Fty.OutQty + Fty.AdjustQty - Fty.ReturnQty)) ,0)
	FROM PO_Supp_Detail psd 
    inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
	LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
	WHERE psd.SCIRefno=t.SCIRefno AND isnull(psdsC.SpecValue, '') =t.ColorID AND psd.ID='{this.poid}'
)Balance 
OUTER APPLY(
	----僅列出Balance 有計算到數量的Seq1 Seq2
	SELECT [Val]=STUFF((
		SELECT  DISTINCT ',' + SuppColor
		FROM PO_Supp_Detail y
        left join PO_Supp_Detail_Spec psdsCy WITH (NOLOCK) on psdsCy.ID = y.id and psdsCy.seq1 = y.seq1 and psdsCy.seq2 = y.seq2 and psdsCy.SpecColumnID = 'Color'
		WHERE EXISTS( 
			SELECT 1 
			FROM PO_Supp_Detail psd 
            inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
			LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
			WHERE psd.SCIRefno=t.SCIRefno AND isnull(psdsC.SpecValue, '') = t.ColorID AND psd.ID = '{this.poid}'
			AND psd.SCIRefno = y.SCIRefno AND isnull(psdsC.SpecValue, '') = isnull(psdsCy.SpecValue, '') AND psd.ID = y.ID
			AND psd.SEQ1 = y.SEQ1 AND psd.SEQ2 = y.SEQ2
			GROUP BY psd.seq1,psd.seq2
			HAVING ISNULL(( SUM(Fty.InQty - Fty.OutQty + Fty.AdjustQty - Fty.ReturnQty)) ,0) > 0
		)
		FOR XML PATH('')
	),1,1,'')
)RealSuppCol

DROP TABLE #tmp_sumQty,#step1,#tmp,#final,#final2
";
            #endregion

            DataRow row;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(t, string.Empty, sqlcmd, out DataTable rtn);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (rtn.Rows.Count == 0)
            {
                if (e != null)
                {
                    e.Cancel = true;
                }

                MyUtility.Msg.WarningBox("Data not found!", "Refno");
                return;
            }

            row = rtn.Rows[0];

            // 當兩筆資料以上時，增加判斷SCIRefno、Refno、Color
            if (rtn.Rows.Count > 1)
            {
                var queryRow = rtn.AsEnumerable().Where(x => x.Field<string>("SCIRefno").EqualString(this.CurrentDetailData["SCIRefno"]) &&
                                              x.Field<string>("Refno").EqualString(this.CurrentDetailData["Refno"]) &&
                                              x.Field<string>("ColorID").EqualString(this.CurrentDetailData["ColorID"]));
                if (queryRow.Any())
                {
                    row = queryRow.FirstOrDefault();
                }
            }

            if (MyUtility.Check.Empty(colorID))
            {
                this.CurrentDetailData["Refno"] = refno;
                this.CurrentDetailData["POID"] = this.poid;
            }
            else
            {
                this.CurrentDetailData["SCIRefno"] = row["SCIRefno"];
                this.CurrentDetailData["Refno"] = row["Refno"];
                this.CurrentDetailData["ColorID"] = row["ColorID"];
                this.CurrentDetailData["SuppColor"] = row["SuppColor"];
                this.CurrentDetailData["POID"] = row["POID"];
                this.CurrentDetailData["DescDetail"] = row["DescDetail"];
                this.CurrentDetailData["@Qty"] = row["@Qty"];
                this.CurrentDetailData["Use Unit"] = row["Use Unit"];
                this.CurrentDetailData["AccuIssued"] = row["AccuIssued"];

                this.CurrentDetailData["IssueQty"] = row["IssueQty"];
                this.CurrentDetailData["Use Qty By Stock Unit"] = row["Use Qty By Stock Unit"];
                this.CurrentDetailData["Stock Unit"] = row["Stock Unit"];
                this.CurrentDetailData["Use Qty By Use Unit"] = row["Use Qty By Use Unit"];
                this.CurrentDetailData["Use Unit"] = row["Use Unit"];
                this.CurrentDetailData["Stock Unit Desc."] = row["Stock Unit Desc."];
                this.CurrentDetailData["OutputQty"] = row["OutputQty"];
                this.CurrentDetailData["Balance(Stock Unit)"] = row["Balance(Stock Unit)"];
                this.CurrentDetailData["Location"] = row["Location"];
                this.CurrentDetailData["MDivisionID"] = row["MDivisionID"];
            }

            this.CurrentDetailData.EndEdit();
            this.GetSubDetailDatas(this.CurrentDetailData, out DataTable finalSubDt);
            finalSubDt.Clear();
        }

        #endregion

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), "P33", this);
        }

        private void BtnThreadColorComb_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.poid))
            {
                P33_ThreadColorCombination form = new P33_ThreadColorCombination(this.poid);
                form.ShowDialog();
            }
        }
    }
}
