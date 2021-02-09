using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;
using System.Transactions;
using Sci.Production.PublicPrg;
using Sci.Win;
using System.Data.SqlClient;
using System.Reflection;
using Sci.Production.Automation;
using System.Threading.Tasks;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P33 : Win.Tems.Input8
    {
        private StringBuilder sbSizecode;
        private StringBuilder sbSizecode2;
        private StringBuilder strsbIssueBreakDown;
        private DataTable dtSizeCode = null;
        private DataTable dtIssueBreakDown = null;
        private bool Ismatrix_Reload = true; // 是否需要重新抓取資料庫資料
        private string poid = string.Empty;

        private P33_Detail subform = new P33_Detail();

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
            this.DefaultFilter = string.Format("Type='B' and id='{0}'", transID);
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
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

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

            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable && (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED"))
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

            this.Ismatrix_Reload = true;
            this.DetailSelectCommand = $@"


----  先By Article撈取資料再加總
WITH BreakdownByArticle as (

    SELECT   DISTINCT
              iis.SCIRefno
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
					    and [IS].Poid=iis.POID AND [IS].SCIRefno=iis.SCIRefno AND [IS].ColorID=iis.ColorID and i2.[EditDate]<I.AddDate AND i2.ID <> i.ID
				    )
		    , [IssueQty]=iis.Qty
		    , [Use Qty By Stock Unit] = CEILING( ISNULL(ThreadUsedQtyByBOT.Qty,0) *  ISNULL(ThreadUsedQtyByBOT.Val,0)/ 100 * ISNULL(UnitRate.RateValue,1))
		    , [Stock Unit]=StockUnit.StockUnit

		    , [Use Unit]='CM'
		    , [Use Qty By Use Unit]=(ThreadUsedQtyByBOT.Qty * ISNULL(ThreadUsedQtyByBOT.Val,0) )

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
	    FROM DBO.GetThreadUsedQtyByBOT(iis.POID) g
	    WHERE SCIRefNo= iis.SCIRefNo AND ColorID = iis.ColorID  
	    AND Article IN (		
            SELECt Article
            FROM Issue_Breakdown
            WHERE ID=i.ID
	    )
	    GROUP BY SCIRefNo,ColorID , Article
    )ThreadUsedQtyByBOT
    OUTER APPLY(
	    SELECT TOP 1 psd2.StockUnit ,u.Description
	    FROM PO_Supp_Detail psd2
	    LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	    WHERE psd2.ID = i.OrderId 
	    AND psd2.SCIRefno = iis.SCIRefno 
	    AND psd2.ColorID = iis.ColorID
    )StockUnit
    OUTER APPLY(
	    SELECT RateValue
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
	SELECT [Qty]=ISNULL(( SUM(Fty.InQty-Fty.OutQty + Fty.AdjustQty )) ,0)
	FROM PO_Supp_Detail psd 
	LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
	WHERE psd.SCIRefno=t.SCIRefno AND psd.ColorID=t.ColorID AND psd.ID='{this.poid}'
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
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
		 , [ColorID]=psd.ColorID
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
 OUTER APPLY(
	 SELECT TOP 1 [Val] = psd2.StockUnit  ,u.Description
	 FROM PO_Supp_Detail psd2 
	 LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	 WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.ColorID= psd.ColorID
 )StockUnit
 OUTER APPLY(
	SELECT [Val]=(f.InQty-f.OutQty+f.AdjustQty) 
	FROM PO_Supp_Detail psd2
	INNER JOIN FtyInventory F ON F.POID=psd2.ID AND F.SEQ1= psd2.SEQ1 AND F.SEQ2 = psd2.SEQ2
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.ColorID=psd.ColorID AND F.StockType='B' {(MyUtility.Check.Empty(colorID) ? "AND psd2.ColorID <> ''" : $"AND psd2.ColorID='{colorID}'")}
 )BulkQty
 OUTER APPLY(
	SELECT [Val]=(f.InQty-f.OutQty+f.AdjustQty) 
	FROM PO_Supp_Detail psd2
	INNER JOIN FtyInventory F ON F.POID=psd2.ID AND F.SEQ1= psd2.SEQ1 AND F.SEQ2 = psd2.SEQ2
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.ColorID=psd.ColorID AND F.StockType='I' {(MyUtility.Check.Empty(colorID) ? "AND psd2.ColorID <> ''" : $"AND psd2.ColorID='{colorID}'")}
 )InventoryQty
OUTER APPLY(
	----列出所有Seq1 Seq2對應到的SuppColor
	SELECT [Val]=STUFF((
		SELECT  DISTINCT ',' + SuppColor
		FROM PO_Supp_Detail y
		WHERE EXISTS( 
			SELECT 1 
			FROM PO_Supp_Detail t 
			LEFT JOIN FtyInventory Fty ON  Fty.poid = t.ID AND Fty.seq1 = t.seq1 AND Fty.seq2 = t.seq2 AND fty.StockType='B'
			WHERE psd.SCIRefno=t.SCIRefno AND psd.ColorID = t.ColorID AND t.ID = '{this.poid}'
			AND t.SCIRefno = y.SCIRefno AND t.ColorID = y.ColorID AND t.ID = y.ID
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
                        sqlcmd += $"AND psd.ColorID='{colorID}' ";
                    }
                    else
                    {
                        sqlcmd += $"AND psd.ColorID <> '' ";
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
                        "25,15,5,10,45,5,15,10,10", this.CurrentDetailData["Refno"].ToString(),
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

                    #region 將IssueBreakDown整理成Datatable
                    if (this.dtIssueBreakDown == null)
                    {
                        return;
                    }

                    DataTable issueBreakDown_Dt = this.Convert_IssueBreakDown_ToDataTable();
                    #endregion

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
Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
Inner Join dbo.Style_ThreadColorCombo as tc On tc.StyleUkey = s.Ukey
Inner Join dbo.Style_ThreadColorCombo_Detail as tcd On tcd.Style_ThreadColorComboUkey = tc.Ukey
WHERE O.ID='{this.poid}' AND tcd.Article IN ( SELECT Article FROM #tmp )

SELECT  DISTINCT
  psd.SCIRefno
, psd.Refno
, psd.ColorID
, f.DescDetail
, [@Qty] = ISNULL(ThreadUsedQtyByBOT.Val,0)
, [AccuIssued] = (
					select isnull(sum([IS].qty),0)
					from dbo.issue I WITH (NOLOCK) 
					inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I.id = [IS].Id 
					where I.type = 'E' and I.Status = 'Confirmed' 
					and [IS].Poid=psd.id AND [IS].SCIRefno=PSD.SCIRefno AND [IS].ColorID=PSD.ColorID and i.[EditDate]<GETDATE()
				)
, [IssueQty]=0.00
, [Use Qty By Stock Unit] = CEILING (ISNULL(ThreadUsedQtyByBOT.Qty,0) *  ISNULL(ThreadUsedQtyByBOT.Val,0)/ 100 * ISNULL(UnitRate.RateValue,1) )
, [Stock Unit]=StockUnit.StockUnit
, [Use Qty By Use Unit] = (ThreadUsedQtyByBOT.Qty * ISNULL(ThreadUsedQtyByBOT.Val,0) )
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
OUTER APPLY(
	SELECT TOP 1 PSD2.StockUnit ,u.Description
	FROM PO_Supp_Detail PSD2 
	LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	WHERE PSD2.ID = psd.id
	AND PSD2.SCIRefno=psd.SCIRefno
	AND PSD2.ColorID=psd.ColorID
)StockUnit
OUTER APPLY(
	SELECT SCIRefNo
		,ColorID
		,[Val]=SUM(((SeamLength  * Frequency * UseRatio ) + (Allowance*Segment))) 
		,[Qty] = (	
			SELECt [Qty]=SUM(b.Qty)
			FROM #step1 a
			INNER JOIN #tmp_sumQty b ON a.Article = b.Article
			WHERE SCIRefNo=psd.SCIRefNo AND  ColorID= psd.ColorID AND a.Article=g.Article
			GROUP BY a.Article
		)
	FROM DBO.GetThreadUsedQtyByBOT(psd.ID) g
	WHERE SCIRefNo= psd.SCIRefNo AND ColorID = psd.ColorID  
	AND Article IN (
		SELECt Article FROM #step1 WHERE SCIRefNo = psd.SCIRefNo  AND ColorID = psd.ColorID 
	)
	GROUP BY SCIRefNo,ColorID , Article
)ThreadUsedQtyByBOT
OUTER APPLY(
	SELECT RateValue
	FROM Unit_Rate
	WHERE UnitFrom='M' and  UnitTo = StockUnit.StockUnit
)UnitRate

WHERE psd.id ='{this.poid}' 
AND m.IsThread=1 
AND psd.FabricType ='A'
and psd.ColorID <> ''
AND psd.Refno='{refno}'
AND psd.ColorID='{colorID}'
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
	SELECT [Qty]=ISNULL(( SUM(Fty.InQty-Fty.OutQty + Fty.AdjustQty )) ,0)
	FROM PO_Supp_Detail psd 
	LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
	WHERE psd.SCIRefno=t.SCIRefno AND psd.ColorID=t.ColorID AND psd.ID='{this.poid}'
)Balance 
OUTER APPLY(
	----僅列出Balance 有計算到數量的Seq1 Seq2
	SELECT [Val]=STUFF((
		SELECT  DISTINCT ',' + SuppColor
		FROM PO_Supp_Detail y
		WHERE EXISTS( 
			SELECT 1 
			FROM PO_Supp_Detail psd 
			LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
			WHERE psd.SCIRefno=t.SCIRefno AND psd.ColorID = t.ColorID AND psd.ID = '{this.poid}'
			AND psd.SCIRefno = y.SCIRefno AND psd.ColorID = y.ColorID AND psd.ID = y.ID
			AND psd.SEQ1 = y.SEQ1 AND psd.SEQ2 = y.SEQ2
			GROUP BY psd.seq1,psd.seq2
			HAVING ISNULL(( SUM(Fty.InQty-Fty.OutQty + Fty.AdjustQty )) ,0) > 0
		)
		FOR XML PATH('')
	),1,1,'')
)RealSuppCol

DROP TABLE #tmp_sumQty,#step1,#tmp,#final,#final2
";

                    DataRow row;
                    DataTable rtn = null;
                    MyUtility.Tool.ProcessWithDatatable(issueBreakDown_Dt, string.Empty, sqlcmd, out rtn, "#tmp");

                    if (rtn == null || rtn.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("Data not found!", "Refno");
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
                        #region 將IssueBreakDown整理成Datatable
                        if (this.dtIssueBreakDown == null)
                        {
                            return;
                        }

                        DataTable issueBreakDown_Dt = this.Convert_IssueBreakDown_ToDataTable();

                        #endregion

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
Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
Inner Join dbo.Style_ThreadColorCombo as tc On tc.StyleUkey = s.Ukey
Inner Join dbo.Style_ThreadColorCombo_Detail as tcd On tcd.Style_ThreadColorComboUkey = tc.Ukey
WHERE O.ID='{this.poid}' AND tcd.Article IN ( SELECT Article FROM #tmp )


SELECT  DISTINCT
  psd.SCIRefno
, psd.Refno
, psd.ColorID
, f.DescDetail
, [@Qty] = ISNULL(ThreadUsedQtyByBOT.Val,0)
, [AccuIssued] = (
					select isnull(sum([IS].qty),0)
					from dbo.issue I WITH (NOLOCK) 
					inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I.id = [IS].Id 
					where I.type = 'E' and I.Status = 'Confirmed' 
					and [IS].Poid=psd.id AND [IS].SCIRefno=PSD.SCIRefno AND [IS].ColorID=PSD.ColorID and i.[EditDate]<GETDATE()
				)
, [IssueQty]=0.00
, [Use Qty By Stock Unit] = CEILING( ISNULL(ThreadUsedQtyByBOT.Qty,0) *  ISNULL(ThreadUsedQtyByBOT.Val,0)/ 100 * ISNULL(UnitRate.RateValue,1) )
, [Stock Unit]=StockUnit.StockUnit
, [Use Qty By Use Unit] = (ThreadUsedQtyByBOT.Qty * ISNULL(ThreadUsedQtyByBOT.Val,0) )
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
OUTER APPLY(
	SELECT TOP 1 PSD2.StockUnit ,u.Description
	FROM PO_Supp_Detail PSD2 
	LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	WHERE PSD2.ID = psd.id
	AND PSD2.SCIRefno=psd.SCIRefno
	AND PSD2.ColorID=psd.ColorID
)StockUnit
OUTER APPLY(
	SELECT SCIRefNo
		,ColorID
		,[Val]=SUM(((SeamLength  * Frequency * UseRatio ) + (Allowance*Segment))) 
		,[Qty] = (	
			SELECt [Qty]=SUM(b.Qty)
			FROM #step1 a
			INNER JOIN #tmp_sumQty b ON a.Article = b.Article
			WHERE SCIRefNo=psd.SCIRefNo AND  ColorID= psd.ColorID AND a.Article=g.Article
			GROUP BY a.Article
		)
	FROM DBO.GetThreadUsedQtyByBOT(psd.ID) g
	WHERE SCIRefNo= psd.SCIRefNo AND ColorID = psd.ColorID  
	AND Article IN (
		SELECt Article FROM #step1 WHERE SCIRefNo = psd.SCIRefNo  AND ColorID = psd.ColorID 
	)
	GROUP BY SCIRefNo,ColorID , Article
)ThreadUsedQtyByBOT
OUTER APPLY(
	SELECT RateValue
	FROM Unit_Rate
	WHERE UnitFrom='M' and  UnitTo = StockUnit.StockUnit
)UnitRate
WHERE psd.id ='{this.poid}' 
AND m.IsThread=1 
AND psd.FabricType ='A'
and psd.ColorID <> ''

AND psd.Refno='{e.FormattedValue}'

";

                        if (!MyUtility.Check.Empty(colorID))
                        {
                            sqlcmd += $"AND psd.ColorID='{colorID}' ";
                        }
                        else
                        {
                            sqlcmd += $"AND psd.ColorID <> '' ";
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
	SELECT [Qty]=ISNULL(( SUM(Fty.InQty-Fty.OutQty + Fty.AdjustQty )) ,0)
	FROM PO_Supp_Detail psd 
	LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
	WHERE psd.SCIRefno=t.SCIRefno AND psd.ColorID=t.ColorID AND psd.ID='{this.poid}'
)Balance 
OUTER APPLY(
	----僅列出Balance 有計算到數量的Seq1 Seq2
	SELECT [Val]=STUFF((
		SELECT  DISTINCT ',' + SuppColor
		FROM PO_Supp_Detail y
		WHERE EXISTS( 
			SELECT 1 
			FROM PO_Supp_Detail psd 
			LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
			WHERE psd.SCIRefno=t.SCIRefno AND psd.ColorID = t.ColorID AND psd.ID = '{this.poid}'
			AND psd.SCIRefno = y.SCIRefno AND psd.ColorID = y.ColorID AND psd.ID = y.ID
			AND psd.SEQ1 = y.SEQ1 AND psd.SEQ2 = y.SEQ2
			GROUP BY psd.seq1,psd.seq2
			HAVING ISNULL(( SUM(Fty.InQty-Fty.OutQty + Fty.AdjustQty )) ,0) > 0
		)
		FOR XML PATH('')
	),1,1,'')
)RealSuppCol

DROP TABLE #tmp_sumQty,#step1,#tmp,#final,#final2
";

                        #endregion

                        DataRow row;
                        DataTable rtn = null;
                        MyUtility.Tool.ProcessWithDatatable(issueBreakDown_Dt, string.Empty, sqlcmd, out rtn, "#tmp");
                        if (rtn == null || rtn.Rows.Count == 0)
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Refno");
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

                            if (MyUtility.Check.Empty(colorID))
                            {
                                // CurrentDetailData["SCIRefno"] = row["SCIRefno"];
                                this.CurrentDetailData["Refno"] = e.FormattedValue;

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
		 , [ColorID]=psd.ColorID
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
 OUTER APPLY(
	 SELECT TOP 1 [Val] = psd2.StockUnit  ,u.Description
	 FROM PO_Supp_Detail psd2 
	 LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	 WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.ColorID= psd.ColorID
 )StockUnit
 OUTER APPLY(
	SELECT [Val]=(f.InQty-f.OutQty+f.AdjustQty) 
	FROM PO_Supp_Detail psd2
	INNER JOIN FtyInventory F ON F.POID=psd2.ID AND F.SEQ1= psd2.SEQ1 AND F.SEQ2 = psd2.SEQ2
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.ColorID=psd.ColorID AND F.StockType='B' {(MyUtility.Check.Empty(refno) ? "AND psd2.Refno <> ''" : $"AND psd2.Refno='{refno}'")}
 )BulkQty
 OUTER APPLY(
	SELECT [Val]=(f.InQty-f.OutQty+f.AdjustQty) 
	FROM PO_Supp_Detail psd2
	INNER JOIN FtyInventory F ON F.POID=psd2.ID AND F.SEQ1= psd2.SEQ1 AND F.SEQ2 = psd2.SEQ2
	WHERE psd2.ID ='{this.poid}' AND psd2.SCIRefno=psd.SCIRefno AND psd2.ColorID=psd.ColorID AND F.StockType='I' {(MyUtility.Check.Empty(refno) ? "AND psd2.Refno <> ''" : $"AND psd2.Refno='{refno}'")}
 )InventoryQty
OUTER APPLY(
	----列出所有Seq1 Seq2對應到的SuppColor
	SELECT [Val]=STUFF((
		SELECT  DISTINCT ',' + SuppColor
		FROM PO_Supp_Detail y
		WHERE EXISTS( 
			SELECT 1 
			FROM PO_Supp_Detail t 
			LEFT JOIN FtyInventory Fty ON  Fty.poid = t.ID AND Fty.seq1 = t.seq1 AND Fty.seq2 = t.seq2 AND fty.StockType='B'
			WHERE psd.SCIRefno=t.SCIRefno AND psd.ColorID = t.ColorID AND t.ID = '{this.poid}'
			AND t.SCIRefno = y.SCIRefno AND t.ColorID = y.ColorID AND t.ID = y.ID
			AND t.SEQ1 = y.SEQ1 AND t.SEQ2 = y.SEQ2
		)
		FOR XML PATH('')
	),1,1,'')
)SuppCol
 WHERE psd.ID='{this.poid}'
 AND m.IsThread=1 
AND psd.FabricType ='A'
AND psd.ColorID <> ''
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
                        sqlcmd += $"AND psd.ColorID='{colorID}' ";
                    }
                    else
                    {
                        sqlcmd += $"AND psd.ColorID <> '' ";
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

                    #region 將IssueBreakDown整理成Datatable
                    if (this.dtIssueBreakDown == null)
                    {
                        return;
                    }

                    DataTable issueBreakDown_Dt = this.Convert_IssueBreakDown_ToDataTable();

                    #endregion

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
Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
Inner Join dbo.Style_ThreadColorCombo as tc On tc.StyleUkey = s.Ukey
Inner Join dbo.Style_ThreadColorCombo_Detail as tcd On tcd.Style_ThreadColorComboUkey = tc.Ukey
WHERE O.ID='{this.poid}' AND tcd.Article IN ( SELECT Article FROM #tmp )

SELECT  DISTINCT
  psd.SCIRefno
, psd.Refno
, psd.ColorID
, f.DescDetail
, [@Qty] = ISNULL(ThreadUsedQtyByBOT.Val,0)
, [AccuIssued] = (
					select isnull(sum([IS].qty),0)
					from dbo.issue I WITH (NOLOCK) 
					inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I.id = [IS].Id 
					where I.type = 'E' and I.Status = 'Confirmed' 
					and [IS].Poid=psd.id AND [IS].SCIRefno=PSD.SCIRefno AND [IS].ColorID=PSD.ColorID and i.[EditDate]<GETDATE()
				)
, [IssueQty]=0.00
, [Use Qty By Stock Unit] = CEILING (ISNULL(ThreadUsedQtyByBOT.Qty,0) * ISNULL(ThreadUsedQtyByBOT.Val,0)/ 100 * ISNULL(UnitRate.RateValue,1) )
, [Stock Unit]=StockUnit.StockUnit
, [Use Qty By Use Unit] = (ThreadUsedQtyByBOT.Qty *  ISNULL(ThreadUsedQtyByBOT.Val,0) )
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
OUTER APPLY(
	SELECT TOP 1 PSD2.StockUnit ,u.Description
	FROM PO_Supp_Detail PSD2 
	LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	WHERE PSD2.ID = psd.id
	AND PSD2.SCIRefno=psd.SCIRefno
	AND PSD2.ColorID=psd.ColorID
)StockUnit
OUTER APPLY(
	SELECT SCIRefNo
		,ColorID
		,[Val]=SUM(((SeamLength  * Frequency * UseRatio ) + (Allowance*Segment))) 
		,[Qty] = (	
			SELECt [Qty]=SUM(b.Qty)
			FROM #step1 a
			INNER JOIN #tmp_sumQty b ON a.Article = b.Article
			WHERE SCIRefNo=psd.SCIRefNo AND  ColorID= psd.ColorID AND a.Article=g.Article
			GROUP BY a.Article
		)
	FROM DBO.GetThreadUsedQtyByBOT(psd.ID) g
	WHERE SCIRefNo= psd.SCIRefNo AND ColorID = psd.ColorID  
	AND Article IN (
		SELECt Article FROM #step1 WHERE SCIRefNo = psd.SCIRefNo  AND ColorID = psd.ColorID 
	)
	GROUP BY SCIRefNo,ColorID , Article
)ThreadUsedQtyByBOT
OUTER APPLY(
	SELECT RateValue
	FROM Unit_Rate
	WHERE UnitFrom='M' and  UnitTo = StockUnit.StockUnit
)UnitRate

WHERE psd.id ='{this.poid}' 
AND m.IsThread=1 
AND psd.FabricType ='A'
and psd.ColorID <> ''
AND psd.Refno='{refno}'
AND psd.ColorID='{colorID}'
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
	SELECT [Qty]=ISNULL(( SUM(Fty.InQty-Fty.OutQty + Fty.AdjustQty )) ,0)
	FROM PO_Supp_Detail psd 
	LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
	WHERE psd.SCIRefno=t.SCIRefno AND psd.ColorID=t.ColorID AND psd.ID='{this.poid}'
)Balance 
OUTER APPLY(
	----僅列出Balance 有計算到數量的Seq1 Seq2
	SELECT [Val]=STUFF((
		SELECT  DISTINCT ',' + SuppColor
		FROM PO_Supp_Detail y
		WHERE EXISTS( 
			SELECT 1 
			FROM PO_Supp_Detail psd 
			LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
			WHERE psd.SCIRefno=t.SCIRefno AND psd.ColorID = t.ColorID AND psd.ID = '{this.poid}'
			AND psd.SCIRefno = y.SCIRefno AND psd.ColorID = y.ColorID AND psd.ID = y.ID
			AND psd.SEQ1 = y.SEQ1 AND psd.SEQ2 = y.SEQ2
			GROUP BY psd.seq1,psd.seq2
			HAVING ISNULL(( SUM(Fty.InQty-Fty.OutQty + Fty.AdjustQty )) ,0) > 0
		)
		FOR XML PATH('')
	),1,1,'')
)RealSuppCol

DROP TABLE #tmp_sumQty,#step1,#tmp,#final,#final2
";
                    DataRow row;
                    DataTable rtn = null;
                    MyUtility.Tool.ProcessWithDatatable(issueBreakDown_Dt, string.Empty, sqlcmd, out rtn, "#tmp");

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

                        DataTable issueBreakDown_Dt = this.Convert_IssueBreakDown_ToDataTable();

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
Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
Inner Join dbo.Style_ThreadColorCombo as tc On tc.StyleUkey = s.Ukey
Inner Join dbo.Style_ThreadColorCombo_Detail as tcd On tcd.Style_ThreadColorComboUkey = tc.Ukey
WHERE O.ID='{this.poid}' AND tcd.Article IN ( SELECT Article FROM #tmp )


SELECT  DISTINCT
  psd.SCIRefno
, psd.Refno
, psd.ColorID
, f.DescDetail
, [@Qty] = ISNULL(ThreadUsedQtyByBOT.Val,0)
, [AccuIssued] = (
					select isnull(sum([IS].qty),0)
					from dbo.issue I WITH (NOLOCK) 
					inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I.id = [IS].Id 
					where I.type = 'E' and I.Status = 'Confirmed' 
					and [IS].Poid=psd.id AND [IS].SCIRefno=PSD.SCIRefno AND [IS].ColorID=PSD.ColorID and i.[EditDate]<GETDATE()
				)
, [IssueQty]=0.00
, [Use Qty By Stock Unit] = CEILING( ISNULL(ThreadUsedQtyByBOT.Qty,0) *  ISNULL(ThreadUsedQtyByBOT.Val,0)/ 100 * ISNULL(UnitRate.RateValue,1) )
, [Stock Unit]=StockUnit.StockUnit
, [Use Qty By Use Unit] = (ThreadUsedQtyByBOT.Qty *  ISNULL(ThreadUsedQtyByBOT.Val,0) )
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
OUTER APPLY(
	SELECT TOP 1 PSD2.StockUnit ,u.Description
	FROM PO_Supp_Detail PSD2 
	LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	WHERE PSD2.ID = psd.id
	AND PSD2.SCIRefno=psd.SCIRefno
	AND PSD2.ColorID=psd.ColorID
)StockUnit
OUTER APPLY(
	SELECT SCIRefNo
		,ColorID
		,[Val]=SUM(((SeamLength  * Frequency * UseRatio ) + (Allowance*Segment))) 
		,[Qty] = (	
			SELECt [Qty]=SUM(b.Qty)
			FROM #step1 a
			INNER JOIN #tmp_sumQty b ON a.Article = b.Article
			WHERE SCIRefNo=psd.SCIRefNo AND  ColorID= psd.ColorID AND a.Article=g.Article
			GROUP BY a.Article
		)
	FROM DBO.GetThreadUsedQtyByBOT(psd.ID) g
	WHERE SCIRefNo= psd.SCIRefNo AND ColorID = psd.ColorID  
	AND Article IN (
		SELECt Article FROM #step1 WHERE SCIRefNo = psd.SCIRefNo  AND ColorID = psd.ColorID 
	)
	GROUP BY SCIRefNo,ColorID , Article
)ThreadUsedQtyByBOT
OUTER APPLY(
	SELECT RateValue
	FROM Unit_Rate
	WHERE UnitFrom='M' and  UnitTo = StockUnit.StockUnit
)UnitRate
WHERE psd.id ='{this.poid}' 
AND m.IsThread=1 
AND psd.FabricType ='A'
and psd.Refno <> ''
AND psd.ColorID='{e.FormattedValue}'

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
	SELECT [Qty]=ISNULL(( SUM(Fty.InQty-Fty.OutQty + Fty.AdjustQty )) ,0)
	FROM PO_Supp_Detail psd 
	LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
	WHERE psd.SCIRefno=t.SCIRefno AND psd.ColorID=t.ColorID AND psd.ID='{this.poid}'
)Balance 
OUTER APPLY(
	----僅列出Balance 有計算到數量的Seq1 Seq2
	SELECT [Val]=STUFF((
		SELECT  DISTINCT ',' + SuppColor
		FROM PO_Supp_Detail y
		WHERE EXISTS( 
			SELECT 1 
			FROM PO_Supp_Detail psd 
			LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
			WHERE psd.SCIRefno=t.SCIRefno AND psd.ColorID = t.ColorID AND psd.ID = '{this.poid}'
			AND psd.SCIRefno = y.SCIRefno AND psd.ColorID = y.ColorID AND psd.ID = y.ID
			AND psd.SEQ1 = y.SEQ1 AND psd.SEQ2 = y.SEQ2
			GROUP BY psd.seq1,psd.seq2
			HAVING ISNULL(( SUM(Fty.InQty-Fty.OutQty + Fty.AdjustQty )) ,0) > 0
		)
		FOR XML PATH('')
	),1,1,'')
)RealSuppCol
DROP TABLE #tmp_sumQty,#step1,#tmp,#final,#final2
";

                        #endregion

                        DataRow row;
                        DataTable rtn = null;
                        MyUtility.Tool.ProcessWithDatatable(issueBreakDown_Dt, string.Empty, sqlcmd, out rtn, "#tmp");

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

SELECT Article, [Qty]=SUM(((SeamLength  * Frequency * UseRatio ) + (Allowance * Segment))) 
FROM dbo.GetThreadUsedQtyByBOT('{this.poid}')
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

        // 新增時預設資料

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
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
                    @";delete from dbo.issue_breakdown where id='{0}'
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
;delete from dbo.issue_breakdown where id='{0}' and qty = 0; ", this.CurrentMaintain["id"], this.sbSizecode.ToString().Substring(0, this.sbSizecode.ToString().Length - 1));

                string aaa = this.sbSizecode.ToString().Substring(0, this.sbSizecode.ToString().Length - 1).Replace("[", string.Empty).Replace("]", string.Empty);

                ProcessWithDatatable2(this.dtIssueBreakDown, "OrderID,Article," + aaa,
                    sqlcmd, out result, "#tmp");
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

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickDeleteAfter()
        {
            base.ClickDeleteAfter();
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return;
            }

            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty;
            DualResult result, result2;
            DataTable datacheck;
            string sqlupd2_FIO = string.Empty;
            StringBuilder sqlupd2_B = new StringBuilder();

            #region 檢查庫存項lock
            sqlcmd = $@"

SELECT   psd.Refno
		,psd.ColorID
		,d.Seq1
		,d.seq2
FROM Issue i
INNER JOIN Issue_Summary s ON i.ID = s.ID 
INNER JOIN Issue_Detail d ON s.id=d.id AND s.Ukey = d.Issue_SummaryUkey
INNER JOIN FtyInventory f ON f.POID=s.Poid AND f.Seq1=d.Seq1 AND f.Seq2=d.Seq2
INNER JOIN PO_Supp_Detail psd ON psd.ID = s.Poid AND psd.SCIRefno = s.SCIRefno AND psd.SCIRefno = s.SCIRefno AND psd.SEQ1=d.Seq1 AND psd.Seq2=d.Seq2
WHERE i.Id = '{this.CurrentMaintain["id"]}' AND  f.lock = 1 
";

            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    // foreach (DataRow tmp in datacheck.Rows)
                    // {
                    //    ids += $@"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} is locked!!" + Environment.NewLine;
                    // }
                    // MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
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

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"


SELECT   psd.Refno
		,psd.ColorID
		,d.Seq1
		,d.seq2
		,[BulkQty]=isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) 
		,[IssueQty]=ISNULL(d.Qty ,0)
		,[Balance]=isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty
FROM Issue i
INNER JOIN Issue_Summary s ON i.ID = s.ID 
INNER JOIN Issue_Detail d ON s.id=d.id AND s.Ukey = d.Issue_SummaryUkey
INNER JOIN FtyInventory f ON f.POID=s.Poid AND f.Seq1=d.Seq1 AND f.Seq2=d.Seq2
INNER JOIN PO_Supp_Detail psd ON psd.ID = s.Poid AND psd.SCIRefno = s.SCIRefno AND psd.SCIRefno = s.SCIRefno AND psd.SEQ1=d.Seq1 AND psd.Seq2=d.Seq2
WHERE i.Id = '{0}'
AND(isnull(f.InQty, 0) - isnull(f.OutQty, 0) + isnull(f.AdjustQty, 0) - ISNULL(d.Qty, 0)) < 0
AND f.StockType = 'B'
", this.CurrentMaintain["id"]);

            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    // foreach (DataRow tmp in datacheck.Rows)
                    // {
                    //    ids += $@"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} is less than issue Qty: {tmp["qty"]}" + Environment.NewLine;
                    // }
                    // MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    var m = MyUtility.Msg.ShowMsgGrid(datacheck, "The following bulk stock is insufficient, can't confirm!!", "Balacne Qty is not enough");

                    m.Width = 850;
                    m.grid1.Columns[0].Width = 300;
                    m.grid1.Columns[1].Width = 100;
                    m.TopMost = true;
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(
                @"
update Issue 
set status = 'Confirmed'
    , ApvName = '{0}' 
    , ApvDate  = GETDATE()
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            sqlcmd = string.Format(@"select * from issue_detail WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            #region -- 更新mdivisionpodetail B倉數 --
            var bs1 = (from b in datacheck.AsEnumerable()
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
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, true));
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            #endregion
            #endregion

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithDatatable(
                        datacheck, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Confirmed successful");
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

            // AutoWHACC WebAPI for Vstrong
            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
            {
                DataTable dtDetail = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
                Task.Run(() => new Vstrong_AutoWHAccessory().SentIssue_Detail_New(dtDetail, "P33", "New"))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult == DialogResult.No)
            {
                return;
            }

            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty;
            DualResult result, result2;
            string sqlupd2_FIO = string.Empty;
            StringBuilder sqlupd2_B = new StringBuilder();

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(
                @"update Issue set status='New', ApvName = '' , ApvDate  = NULL, editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            sqlcmd = string.Format(@"select * from issue_detail WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }

            var bsfio = (from m in datacheck.AsEnumerable()
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

            var bs1 = (from b in datacheck.AsEnumerable()
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
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, false));
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            #endregion

            #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            if (!Prgs.ChkWMSCompleteTime(this.CurrentMaintain["id"].ToString(), "Issue_Detail"))
            {
                return;
            }
            #endregion

            #region UnConfirmed 先檢查WMS是否傳送成功
            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
            {
                DataTable dtDetail = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
                if (!Vstrong_AutoWHAccessory.SentIssue_Detail_delete(dtDetail, "P33", "UnConfirmed"))
                {
                    return;
                }
            }
            #endregion

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(
                        bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
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
		,[Desc]=f.DescDetail
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
	WHERE PSD.ID ='{poID}' AND PSD.SCIRefno=iis.SCIRefno AND PSD.ColorID = iis.ColorID
)Unit
OUTER APPLY(
	 SELECt [Qty]=SUM(b.Qty)
	 FROM (
		    Select distinct o.ID,tcd.SCIRefNo, tcd.ColorID ,tcd.Article
		    From dbo.Orders as o
		    Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
		    Inner Join dbo.Style_ThreadColorCombo as tc On tc.StyleUkey = s.Ukey
		    Inner Join dbo.Style_ThreadColorCombo_Detail as tcd On tcd.Style_ThreadColorComboUkey = tc.Ukey
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
	WHERE psd.ID ='{poID}' AND psd.SCIRefno = iis.SCIRefno AND psd.ColorID = iis.Colorid
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
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("IssueDate", issueDate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("OrderID", orderID));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Style", style));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Line", line));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", remark));

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
                            if (MyUtility.Convert.GetDecimal(issued["Qty"]) != 0)
                            {
                                totalQty += (decimal)issued["Qty"];

                                string suppColor = issued["SuppColor"].ToString();

                                // 重複就不加進去了
                                if (!allSuppColor.Contains(suppColor))
                                {
                                    allSuppColor.Add(suppColor);
                                }

                                issued.AcceptChanges();
                                issued.SetAdded();
                                subDetail.ImportRow(issued);
                            }
                        }

                        detail.Rows[detail.Rows.Count - 1]["SuppColor"] = allSuppColor.JoinToString(",");
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
            string articleWhere = string.Empty;

            // 判斷訂單會用到那些物料
            // 勾選By Combo與否，會造成Article不一樣，因此在這個時候就必須找出：這些OrderID，用到哪些Article，這些Article用到哪些SCIRefno + ColorID 的物料
            if (this.checkByCombo.Checked)
            {
                articleWhere = $@"

	SELECT Article FROM(
		Select  distinct tcd.SCIRefNo, tcd.ColorID ,tcd.Article 
		From dbo.Orders as b
		INNER JOIN dbo.order_qty a WITH (NOLOCK)  on b.id = a.id
		Inner Join dbo.Style as s On s.Ukey = b.StyleUkey
		Inner Join dbo.Style_ThreadColorCombo as tc On tc.StyleUkey = s.Ukey
		Inner Join dbo.Style_ThreadColorCombo_Detail as tcd On tcd.Style_ThreadColorComboUkey = tc.Ukey
		WHERE b.ID='{pOID}'
		AND tcd.Article IN (
			select DISTINCT a.Article
			from dbo.order_qty a WITH (NOLOCK) 
			inner join dbo.orders b WITH (NOLOCK) on b.id = a.id
			where b.POID=( select poid from dbo.orders WITH (NOLOCK) where id = '{pOID}' )
			--AND a.id = b.poid
		)
	)Q
	WHERE q.SCIRefNo=psd.SCIRefno AND q.ColorID = psd.ColorID

";
            }
            else
            {
                articleWhere = $@"

	SELECT Article FROM(
		Select  distinct tcd.SCIRefNo, tcd.ColorID ,tcd.Article 
		From dbo.Orders as b
		INNER JOIN dbo.order_qty a WITH (NOLOCK)  on b.id = a.id
		Inner Join dbo.Style as s On s.Ukey = b.StyleUkey
		Inner Join dbo.Style_ThreadColorCombo as tc On tc.StyleUkey = s.Ukey
		Inner Join dbo.Style_ThreadColorCombo_Detail as tcd On tcd.Style_ThreadColorComboUkey = tc.Ukey
		WHERE b.ID='{pOID}'
		AND tcd.Article IN (
			select DISTINCT a.Article
			from dbo.order_qty a WITH (NOLOCK) 
			inner join dbo.orders b WITH (NOLOCK) on b.id = a.id
			where b.POID=( select poid from dbo.orders WITH (NOLOCK) where id = '{pOID}' )
			AND a.id = b.poid
		)
	)Q
	WHERE q.SCIRefNo=psd.SCIRefno AND q.ColorID = psd.ColorID

";
            }

            // 回採購單找資料
            string sql = $@"

SELECT  DISTINCT
  psd.SCIRefno
, psd.Refno
, psd.ColorID
, f.DescDetail
, [@Qty] = ISNULL(ThreadUsedQtyByBOT.Val,0)
, [AccuIssued] = (
					select isnull(sum([IS].qty),0)
					from dbo.issue I WITH (NOLOCK) 
					inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I.id = [IS].Id 
					where I.type = 'E' and I.Status = 'Confirmed' 
					and [IS].Poid=psd.id AND [IS].SCIRefno=PSD.SCIRefno AND [IS].ColorID=PSD.ColorID and i.[EditDate]<GETDATE()
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
OUTER APPLY(
	SELECT TOP 1 PSD2.StockUnit ,u.Description
	FROM PO_Supp_Detail PSD2 
	LEFT JOIN Unit u ON u.ID = psd2.StockUnit
	WHERE PSD2.ID = psd.id
	AND PSD2.SCIRefno=psd.SCIRefno
	AND PSD2.ColorID=psd.ColorID
)StockUnit
OUTER APPLY(
	SELECT Val=SUM((SeamLength * Frequency * UseRatio) +  (Allowance * Segment) )
	FROM dbo.GetThreadUsedQtyByBOT(psd.ID)
	WHERE SCIRefNo = psd.SCIRefno AND ColorID = psd.ColorID AND Article IN (
        {articleWhere}
	)
)ThreadUsedQtyByBOT
WHERE psd.id ='{pOID}' 
AND m.IsThread=1 
AND psd.FabricType ='A'
and psd.ColorID <> ''


SELECT  
	  SCIRefno
	, Refno
	, ColorID
	, [SuppColor]=SuppCol.Val
	, DescDetail
	, [@Qty] 
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
		WHERE EXISTS( 
			SELECT 1 
			FROM PO_Supp_Detail psd
			LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
			WHERE psd.SCIRefno=t.SCIRefno AND t.ColorID = psd.ColorID AND t.POID = psd.ID
			AND psd.SCIRefno = y.SCIRefno AND psd.ColorID = y.ColorID AND t.POID = y.ID
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
	, [@Qty] 
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
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
            this.sbSizecode2 = new StringBuilder();
            this.sbSizecode.Clear();
            this.sbSizecode2.Clear();
            for (int i = 0; i < this.dtSizeCode.Rows.Count; i++)
            {
                this.sbSizecode.Append(string.Format(@"[{0}],", this.dtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()));
                this.sbSizecode2.Append(string.Format(@"{0},", this.dtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()));
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
            order by [OrderID],[Article]", orderID, this.CurrentMaintain["id"], this.sbSizecode.ToString().Substring(0, this.sbSizecode.ToString().Length - 1))); // .Replace("[", "[_")
            this.strsbIssueBreakDown = sbIssueBreakDown; // 多加一個變數來接 不改變欄位
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
            this.sbSizecode2 = new StringBuilder();
            this.sbSizecode.Clear();
            this.sbSizecode2.Clear();
            for (int i = 0; i < this.dtSizeCode.Rows.Count; i++)
            {
                this.sbSizecode.Append($@"[{this.dtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()}],");
                this.sbSizecode2.Append($@"{this.dtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()},");
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
            this.strsbIssueBreakDown = sbIssueBreakDown; // 多加一個變數來接 不改變欄位
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

        #endregion

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), "P33", this);
        }
    }

    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    public class IssueQtyBreakdown
    {
        /// <inheritdoc/>
        public string OrderID { get; set; }

        /// <inheritdoc/>
        public string Article { get; set; }

        /// <inheritdoc/>
        public int Qty { get; set; }
    }
}
