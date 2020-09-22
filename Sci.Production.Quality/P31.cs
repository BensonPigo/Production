using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using System.Data.SqlClient;
using Ict.Win.UI;
using System.Linq;
using Sci.Production.PublicPrg;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P31 : Win.Tems.Input6
    {
        public string type = string.Empty;
        private List<string> _Articles = new List<string>();
        private List<string> _Articles_c = new List<string>();

        // 每一Article底下的Size數量
        private readonly Dictionary<string, int> Size_per_Article = new Dictionary<string, int>();

        /// <inheritdoc/>
        public P31(ToolStripMenuItem menuitem, string type)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.Text = type == "1" ? "P31. CFA Master List" : "P311. CFA Master List(History)";
            this.type = type;

            string isfinished = type == "1" ? "0" : "1";
            string defaultFilter = $@" 
EXISTS (
    SELECT 1 
    FROM Orders o WITH (NOLOCK) 
    WHERE o.Ftygroup = '{Sci.Env.User.Factory}'  --MDivisionID='{Sci.Env.User.Keyword}' 
    AND Finished = {isfinished} 
	AND o.ID = Order_QtyShip.ID
    AND o.Category IN ('B','S','G')
)";
            this.DefaultFilter = defaultFilter;

            if (type != "1")
            {
                this.IsSupportEdit = false;
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            this.Size_per_Article.Clear();
            this._Articles.Clear();
            this._Articles_c.Clear();
            this.gridQtyBreakdown.Columns.Clear();
            this.gridCartonSummary.Columns.Clear();

            bool isSample = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($@"SELECT  IIF(Category='S','True','False') FROM Orders WHERE ID = '{this.CurrentMaintain["ID"].ToString()}' "));

            if (isSample)
            {
                this.ByCarton.Visible = false;

                // this.tabControl.TabPages.Remove(this.tab_CartonSummary);
                this.tab_CartonSummary.Parent = null;
            }
            else
            {
                this.ByCarton.Visible = true;
                this.tab_CartonSummary.Parent = this.tabControl;
            }

            #region 表頭欄位帶入
            this.txtSpSeq.TextBoxSP.IsSupportEditMode = false;
            this.txtSpSeq.TextBoxSeq.IsSupportEditMode = false;

            this.txtSpSeq.TextBoxSP.ReadOnly = true;
            this.txtSpSeq.TextBoxSeq.ReadOnly = true;

            this.disFinalCtn.Value = MyUtility.GetValue.Lookup($@"
select COUNT(1)
from CFAInspectionRecord
where Status='Confirmed'
AND Stage='Final'
AND OrderID='{this.CurrentMaintain["ID"].ToString()}'
AND Seq='{this.CurrentMaintain["Seq"].ToString()}'
");

            this.dis3rdPartyCtn.Value = MyUtility.GetValue.Lookup($@"
select COUNT(1)
from CFAInspectionRecord
where Status='Confirmed'
AND Stage='3rd party'
AND OrderID='{this.CurrentMaintain["ID"].ToString()}'
AND Seq='{this.CurrentMaintain["Seq"].ToString()}'
");

            this.disPO.Value = MyUtility.GetValue.Lookup($@"
SELECT  CustPoNo
FROM Orders 
WHERE ID = '{this.CurrentMaintain["ID"].ToString()}'
");
            this.disBrand.Value = MyUtility.GetValue.Lookup($@"
SELECT  BrandID
FROM Orders 
WHERE ID = '{this.CurrentMaintain["ID"].ToString()}'
");
            this.disSeason.Value = MyUtility.GetValue.Lookup($@"
SELECT SeasonID
FROM Orders 
WHERE ID = '{this.CurrentMaintain["ID"].ToString()}'
");
            this.disDest.Value = MyUtility.GetValue.Lookup($@"
SELECT c.Alias
FROM Orders o
INNER JOIN Country c ON o.Dest = c.ID
WHERE o.ID = '{this.CurrentMaintain["ID"].ToString()}'
");
            this.disArticle.Value = MyUtility.GetValue.Lookup($@"
SELECT STUFF(
    (SELECT DISTINCT ','+Article 
    FROM Order_QtyShip_Detail 
    WHERE ID = '{this.CurrentMaintain["ID"].ToString()}' AND Seq = '{this.CurrentMaintain["Seq"].ToString()}'
    FOR XML PATH('')
    )
,1,1,'')
");
            this.disSewingOutput.Value = MyUtility.GetValue.Lookup($@"
SELECT  dbo.getMinCompleteSewQty('{this.CurrentMaintain["ID"].ToString()}',NULL,NULL)
");
            this.disCFAStaggeredQty.Value = MyUtility.GetValue.Lookup($@"
SELECT ISNULL(Sum(ISNULL(ShipQty,0)),0)
From PackingList_Detail P
Inner join CFAInspectionRecord CFA on P.StaggeredCFAInspectionRecordID=P.ID
Where CFA.Status='Confirmed' 
and CFA.Stage='Staggered' 
and P.OrderID='{this.CurrentMaintain["ID"].ToString()}' 
and P.OrderShipmodeSeq='{this.CurrentMaintain["Seq"].ToString()}'
");

            this.disClogQty.Value = MyUtility.GetValue.Lookup($@"
SELECT ISNULL( SUM(IIF
            ( CFAReceiveDate IS NOT NULL OR ReceiveDate IS NOT NULL
	        ,ShipQty
	        ,0)
        ),0)
FROM PackingList_Detail
WHERE OrderID='{this.CurrentMaintain["ID"].ToString()}' 
and OrderShipmodeSeq='{this.CurrentMaintain["Seq"].ToString()}'
");

            this.dateLastCarton.Value = MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup($@"
            select ReceiveDate = MAX(pd.ReceiveDate) 
            	from Production.dbo.PackingList_Detail pd
            	where	pd.OrderID = '{this.CurrentMaintain["ID"].ToString()}'
            			and pd.OrderShipmodeSeq = '{this.CurrentMaintain["Seq"].ToString()}'
            			and not exists (
            				-- 每個紙箱必須放在 Clog（ReceiveDate 有日期）
            				select 1 
            				from Production.dbo.PackingList_Detail pdCheck
            				where pd.OrderID = pdCheck.OrderID 
            					  and pd.OrderShipmodeSeq = pdCheck.OrderShipmodeSeq
            					  and pdCheck.ReceiveDate is null)
            	group by pd.OrderID, pd.ordershipmodeseq
            "));

            this.datePullOut.Value = MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup($@"

SElECT Top 1 PulloutDate
FROM  PackingList p
INNER JOIN PackingList_Detail PD on P.ID=PD.ID
where pd.OrderID ='{this.CurrentMaintain["ID"].ToString()}' 
and pd.OrderShipmodeSeq='{this.CurrentMaintain["Seq"].ToString()}'
"));

            this.disVasShas.Value = MyUtility.GetValue.Lookup($@"
SElECT IIF(VasShas=1 ,'Y' ,'N')
FROM Orders
WHERE ID='{this.CurrentMaintain["ID"].ToString()}' 
");

            this.disGarmentTest.Value = MyUtility.GetValue.Lookup($@"
SELECT IIF(
(
	(SELECT  COUNT(ID) FROM GarmentTest WHERE OrderID = '{this.CurrentMaintain["ID"].ToString()}'AND Result='P')
	-
	(SELECT  COUNT(ID) FROM GarmentTest WHERE OrderID = '{this.CurrentMaintain["ID"].ToString()}' AND Result='F')
	=
	(SELECT  COUNT(ID) FROM GarmentTest WHERE OrderID = '{this.CurrentMaintain["ID"].ToString()}')
)
AND (SELECT  COUNT(ID) FROM GarmentTest WHERE OrderID = '{this.CurrentMaintain["ID"].ToString()}') > 0
,'Pass'
,IIF((SELECT  COUNT(ID) FROM GarmentTest WHERE OrderID = '{this.CurrentMaintain["ID"].ToString()}') > 0 , 'Fail','')
)
");
            #endregion

            if (this.EditMode)
            {
                this.btnCreateInsRecord.Enabled = false;
            }
            else
            {
                bool canNew = Prgs.GetAuthority(Sci.Env.User.UserID, "P32. CFA Inspection Record ", "CanNew");

                this.btnCreateInsRecord.Enabled = canNew;
            }

            // 雙行Column Header的做法
            // 搭配I:\MIS\Personal\Benson\QA P31\CFA.xlxs 第一個Sheet的畫面看比較好懂

            // 開始取得表身兩個Tab的資料
            DataTable dtQtybreakdown;
            DataTable dtCtnSummary;

            DataTable gridQtybreakdown = new DataTable();
            DataTable gridCtnSummary = new DataTable();

            string orderID = this.CurrentMaintain["ID"].ToString();
            string seq = this.CurrentMaintain["Seq"].ToString();

            #region Qtybreakdown分頁

            #region SQL

            string cmd = $@"
----By Qty Breakdown分頁
SELECT oqd.ID,oqd.Seq,oqd.Article,oqd.SizeCode,oqd.Qty
		,[IsCategory]=(SELECT IIF(Category='S',1,0) FROM Orders o WHERE ID = oqd.Id)
        ,[CMPOutput]= IIF(oqd.Qty = 0 OR (SELECT COUNT(Seq) FROM Order_QtyShip oq WHERE oq.ID = oqd.ID) > 1
							,'N/A'
							,Cast( ISNULL((SELECT dbo.getMinCompleteSewQty( oqd.ID, oqd.Article , oqd.SizeCode ) ) ,0) as varchar)
						)
        ,[CMP%] = IIF(oqd.Qty = 0 OR (SELECT COUNT(Seq) FROM Order_QtyShip oq WHERE oq.ID = oqd.ID) > 1
				        ,'N/A'
				        , CAST( CAST( ROUND(((ISNULL((SELECT dbo.getMinCompleteSewQty( oqd.ID, oqd.Article , oqd.SizeCode ) ),0) * 1.0  / oqd.Qty ) * 100) ,0) as int )as varchar) + '%'
			         )
        ,[CFA staggered Qty] = ISNULL(Staggered.Qty,0)
        ,[Staggered%]=IIF(oqd.Qty = 0
					        , 'N/A'
					        , CAST( CAST(ROUND((ISNULL(Staggered.Qty,0) * 1.0 /oqd.Qty)* 100,0) as int ) as varchar) + '%'
				        )
        ,[CLOG Qty]=ISNULL(Clog.Qty ,0)
        ,[CLOG%]=IIF(        oqd.Qty = 0
					        , 'N/A'
					        , CAST( CAST(ROUND((ISNULL(Clog.Qty,0) * 1.0 /oqd.Qty) * 100 ,0) as int ) as varchar) + '%'
				        )
        ,[OrderKey] = ROW_NUMBER() OVER(ORDER BY oqd.Article ASC) 
INTO #tmp
FROM Order_QtyShip_Detail oqd
OUTER APPLY(
	SELECT [Qty]=Sum(p.ShipQty)
	From PackingList_Detail P 
	Inner join CFAInspectionRecord CFA on P.StaggeredCFAInspectionRecordID=CFA.ID
	Where CFA.Status='Confirmed' and CFA.Stage='Staggered' and p.Article=oqd.Article and P.SizeCode=oqd.SizeCode and P.OrderID=oqd.ID and P.OrderShipmodeSeq=oqd.Seq
)Staggered
OUTER APPLY(
	SELECT [Qty]=SUM(IIF( pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL,pd.ShipQty,0))
	FROM PackingList_Detail pd
	where pd.OrderID=oqd.ID and pd.OrderShipmodeSeq = oqd.SEQ and pd.Article= oqd.Article and pd.SizeCode= oqd.SizeCode
)Clog
WHERE oqd.ID='{orderID}'
AND oqd.Seq='{seq}'

SELECT --[ID]
	--,[Seq]
	[Article]
	,[SizeCode]
	,[Order Qty]=[Qty]
	,[CMP output]=[CMPOutput]
	,[CMP %]=[CMP%]
	,[CFA staggered output]=IIF(IsCategory=1,'N/A',Cast([CFA staggered Qty]as varchar))
	,[Staggered %]=IIF(IsCategory=1,'N/A',[Staggered%])
	,[CLOG output]=IIF(IsCategory=1,'N/A',Cast([CLOG Qty] as varchar))
	,[CLOG %]=IIF(IsCategory=1,'N/A',[CLOG%]  )
	,[OrderKey]
FROM #tmp
UNION 
SELECT 
	 [Article]='TTL'  -- 注意，這邊TTL要跟Grid對上，修改的話要同步改
	,[SizeCode]='TTL'
	,[Order Qty]= (SELECT SUM(Qty) FROM #tmp)
	,[CMP output]=IIF((SELECT TOP 1 CMPOutput FROM #tmp) <> 'N/A', Cast( (SELECT SUM(Cast(CMPOutput as int)) FROM #tmp) as Varchar) , 'N/A' )
	,[CMP %] = IIF((SELECT TOP 1 CMPOutput FROM #tmp) <> 'N/A' AND (SELECT SUM(Qty) FROM #tmp) <> 0
					,Cast( CAST( ROUND( (SELECT SUM(Cast(CMPOutput as int)) * 1.0 / SUM(Qty)  FROM #tmp),3)  * 100 as INT ) as Varchar ) + '%'
					,'N/A' 
					)
	,[CFA staggered output] = IIF((SELECT TOP 1 IsCategory FROM #tmp)=1,'N/A',Cast( (SELECT SUM([CFA staggered Qty]) FROM #tmp) as varchar))
	,[Staggered %]= IIF((SELECT SUM(Qty) FROM #tmp)=0 OR (SELECT TOP 1 IsCategory FROM #tmp)=1
						,'N/A'
						,Cast(CAST( ROUND( (SELECT SUM([CFA staggered Qty]) *1.0 / SUM(Qty) FROM #tmp),3) * 100 as INT)   as Varchar ) + '%'
						)
	,[CLOG output]= IIF((SELECT TOP 1 IsCategory FROM #tmp)=1,'N/A',Cast( (SELECT SUM([CLOG Qty]) FROM #tmp) as varchar))
	,[CLOG %]=IIF((SELECT SUM(Qty) FROM #tmp)=0 OR (SELECT TOP 1 IsCategory FROM #tmp)=1
					,'N/A'
					,Cast(CAST( ROUND( (SELECT SUM([CLOG Qty]) *1.0 / SUM(Qty)FROM #tmp) * 100,3) as INT)   as Varchar  ) + '%'
				)
	,[OrderKey] = (SELECT MAX(OrderKey) + 1 FROM #tmp) 

ORDER BY [OrderKey]

DROP TABLE #tmp
";
            #endregion

            // 取得基礎資料
            DualResult r = DBProxy.Current.Select(null, cmd, out dtQtybreakdown);

            if (!r)
            {
                this.ShowErr(r);
                return;
            }

            var qtybreakdown = dtQtybreakdown.AsEnumerable().ToList();

            List<string> articles = new List<string>();

            #region Gird 客製 : 第一行Column Header的第一欄是固定的，可以先塞

            // 第一欄是固定的
            articles.Add("Article");
            articles.AddRange(qtybreakdown.Select(o => o["Article"].ToString()).Distinct().ToList());

            this._Articles = articles;
            this.Size_per_Article.Add("Article", 1);

            System.Windows.Forms.DataGridViewTextBoxColumn firstcol = new System.Windows.Forms.DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Category/Size",
                Name = "Category/Size",
                HeaderText = "Category/Size",
                Width = 150,
            };

            this.gridQtyBreakdown.Columns.Add(firstcol);
            #endregion

            #region  Gird 客製 : 先產生第二行Column Header (Size)

            foreach (var article in articles)
            {
                List<string> sizeCodes = qtybreakdown.Where(o => o["Article"].ToString() == article).Select(o => o["SizeCode"].ToString()).ToList();

                if (!this.Size_per_Article.Keys.Contains(article))
                {
                    // 紀錄每個Article有多少個Size，用於後面產生第一行Header
                    this.Size_per_Article.Add(article, sizeCodes.Count());

                    foreach (var sizeCode in sizeCodes)
                    {
                        System.Windows.Forms.DataGridViewTextBoxColumn col = new System.Windows.Forms.DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = article + "_" + sizeCode,
                            Name = article + "_" + sizeCode,
                            HeaderText = sizeCode, /*,
                            Width = 20*/
                        };
                        this.gridQtyBreakdown.Columns.Add(col);
                    }
                }
            }
            #endregion

            #region 製作Grid的DataSource

            DataTable source_QtyBreakdown = new DataTable();

            // 設定欄位名稱
            foreach (System.Windows.Forms.DataGridViewTextBoxColumn columm in this.gridQtyBreakdown.Columns)
            {
                source_QtyBreakdown.ColumnsStringAdd(columm.Name);
            }

            // 注意！ 這邊要對應到SQL語法的欄位名稱
            List<string> vHeaders = new List<string>();
            vHeaders.Add("Order Qty");
            vHeaders.Add("CMP output");
            vHeaders.Add("CMP %");
            vHeaders.Add("CFA staggered output");
            vHeaders.Add("Staggered %");
            vHeaders.Add("CLOG output");
            vHeaders.Add("CLOG %");

            foreach (var vHeader in vHeaders)
            {
                DataRow newRow = source_QtyBreakdown.NewRow();
                string dtColumnName = vHeader;

                foreach (System.Windows.Forms.DataGridViewTextBoxColumn columm in this.gridQtyBreakdown.Columns)
                {
                    string article_SizeCode = columm.Name;
                    if (article_SizeCode == "Category/Size")
                    {
                        newRow[article_SizeCode] = dtColumnName;
                    }
                    else
                    {
                        string article = article_SizeCode.Split('_')[0];
                        string sizeCode = article_SizeCode.Split('_')[1];

                        newRow[article_SizeCode] = qtybreakdown.Where(o => o["Article"].ToString() == article && o["SizeCode"].ToString() == sizeCode).FirstOrDefault()[dtColumnName];
                    }
                }

                source_QtyBreakdown.Rows.Add(newRow);
            }

            source_QtyBreakdown.AcceptChanges();
            #endregion

            this.gridQtyBreakdown.DataSource = source_QtyBreakdown;
            #endregion

            this.Size_per_Article.Clear();

            #region Carton Summary

            #region SQL
            cmd = $@"
----Carton Summary分頁
----記錄哪些箱號有混尺碼
SELECT ID,OrderID,OrderShipmodeSeq,CTNStartNo
		,[ArticleCount]=COUNT(DISTINCT Article)
		,[SizeCodeCount]=COUNT(DISTINCT SizeCode)
INTO #MixCTNStartNo
FROM PackingList_Detail pd
WHERE OrderID LIKE '{orderID}' 
AND OrderShipmodeSeq ='{seq}'
GROUP BY ID,OrderID,OrderShipmodeSeq,CTNStartNo
HAVING COUNT(DISTINCT Article) > 1 OR COUNT(DISTINCT SizeCode) > 1

----記錄哪些箱號沒有混尺碼
SELECT ID,OrderID,OrderShipmodeSeq,CTNStartNo
		,Article
		,SizeCode
		,StaggeredCFAInspectionRecordID
		,CFAReceiveDate
		,ReceiveDate
INTO #Not_MixCTNStartNo
FROM PackingList_Detail pd
WHERE OrderID LIKE '{orderID}'
AND  OrderShipmodeSeq ='{seq}'
AND CTNStartNo NOT IN (SELECT CTNStartNo FROM #MixCTNStartNo)

----記錄 混尺碼箱號 包含的Article SizeCode
SELECT ID, OrderID, OrderShipmodeSeq, CTNStartNo
        , Article
        , SizeCode
        , StaggeredCFAInspectionRecordID
        , CFAReceiveDate
        , ReceiveDate
INTO #Is_MixCTNStartNo
FROM PackingList_Detail pd
WHERE OrderID LIKE '{orderID}'
AND OrderShipmodeSeq = '{seq}'
AND CTNStartNo IN(SELECT CTNStartNo FROM #MixCTNStartNo)

----先計算 不是 混尺碼
SELECT  oqd.ID, oqd.Seq, oqd.Article, oqd.SizeCode
        ,[OrderCTN] = ISNULL(OrderCTN.Val,0) 
        ,[CFA_StaggeredCTN] = ISNULL(CFA_StaggeredCTN.Val, 0)
        ,[Staggered %] = IIF(ISNULL(OrderCTN.Val, 0) = 0
                                , 'N/A'
                                , CAST(CAST(ROUND((ISNULL(CFA_StaggeredCTN.Val, 0) * 1.0 / ISNULL(OrderCTN.Val, 0)) * 100, 0) AS Int) as varchar) + '%'
                            )
        ,[ClogCTN] = ISNULL(ClogCTN.Val,0)
        ,[Clog %] = IIF(ISNULL(OrderCTN.Val, 0) = 0
                                , 'N/A'
                                , CAST(CAST(ROUND((ISNULL(ClogCTN.Val, 0) * 1.0 / ISNULL(OrderCTN.Val, 0)) * 100, 0) AS Int) as varchar) + '%'
                            )
INTO #Not_Mix_Final
FROM Order_QtyShip_Detail oqd
OUTER APPLY(
    SELECT t.Article, t.SizeCode,[Val] = COUNT(DISTINCT t.CTNStartNo)  --不同PackingListID，但相同CTNStartNo，現階段視作相同
    FROM #Not_MixCTNStartNo t
	WHERE  t.OrderID = oqd.Id
        AND t.OrderShipmodeSeq = oqd.Seq
        AND t.Article = oqd.Article
        AND t.SizeCode = oqd.SizeCode
    GROUP BY t.Article, t.SizeCode
)OrderCTN
OUTER APPLY(
    SELECT t.Article, t.SizeCode,[Val] = COUNT(DISTINCT t.CTNStartNo) --不同PackingListID，但相同CTNStartNo，現階段視作相同
    FROM #Not_MixCTNStartNo t
	INNER JOIN CFAInspectionRecord CFA on t.StaggeredCFAInspectionRecordID = CFA.ID
    WHERE  t.OrderID = oqd.Id
        AND t.OrderShipmodeSeq = oqd.Seq
        AND t.Article = oqd.Article
        AND t.SizeCode = oqd.SizeCode
        AND CFA.Status = 'Confirmed'
        AND CFA.Stage = 'Staggered'
    GROUP BY t.Article, t.SizeCode
)CFA_StaggeredCTN
OUTER APPLY(
    SELECT [Val] = COUNT(DISTINCT t.CTNStartNo)--,[Val] = SUM(IIF(t.CFAReceiveDate IS NOT NULL OR t.ReceiveDate IS NOT NULL, 1, 0)) --不同PackingListID，但相同CTNStartNo，現階段視作相同
    FROM #Not_MixCTNStartNo t
	WHERE t.OrderID = oqd.Id
        AND t.OrderShipmodeSeq = oqd.Seq
        AND t.Article = oqd.Article
        AND t.SizeCode = oqd.SizeCode
		AND (t.CFAReceiveDate IS NOT NULL OR t.ReceiveDate IS NOT NULL)
)ClogCTN
WHERE oqd.ID = '{orderID}' AND oqd.Seq = '{seq}'

----計算 是 混尺碼
SELECT DISTINCT
         [ID] = t.OrderID
        ,[Seq] = t.OrderShipmodeSeq
        ,[Article] = 'Mix Article'
        ,[SizeCode] = 'Mix Size'
        ,[OrderCTN] = OrderCTN.Val
        ,[CFA_StaggeredCTN] = CFA_StaggeredCTN.Val
        ,[Staggered %] = IIF(ISNULL(OrderCTN.Val, 0) = 0
                        , 'N/A'
                        , CAST(CAST(ROUND((ISNULL(CFA_StaggeredCTN.Val, 0) * 1.0 / ISNULL(OrderCTN.Val, 0)) * 100, 0) AS Int) as varchar) + '%'
                    )
        ,[ClogCTN] = ClogCTN.Val
        ,[Clog %] = IIF(ISNULL(OrderCTN.Val, 0) = 0
                                , 'N/A'
                                , CAST(CAST(ROUND((ISNULL(ClogCTN.Val, 0) * 1.0 / ISNULL(OrderCTN.Val, 0)) * 100, 0) AS Int) as varchar) + '%'
                            )
INTO #Is_Mix_Final
FROM #Is_MixCTNStartNo t
OUTER APPLY(
    SELECT[Val] = COUNT(DISTINCT CTNStartNo) --不同PackingListID，但相同CTNStartNo，現階段視作相同
    FROM #Is_MixCTNStartNo
)OrderCTN
OUTER APPLY(
    SELECT[Val] = COUNT(DISTINCT CTNStartNo) --不同PackingListID，但相同CTNStartNo，現階段視作相同
    FROM #Is_MixCTNStartNo t2
	INNER JOIN CFAInspectionRecord CFA on t2.StaggeredCFAInspectionRecordID = CFA.ID
    WHERE  t2.OrderID = t.OrderID
        AND t2.OrderShipmodeSeq = t.OrderShipmodeSeq
        AND CFA.Status = 'Confirmed'
        AND CFA.Stage = 'Staggered'
)CFA_StaggeredCTN
OUTER APPLY(
    SELECT[Val] = SUM(Ctn)
    FROM(
        SELECT [Ctn]= COUNT(DISTINCT t2.CTNStartNo) --- [Ctn] = (IIF(MAX(CFAReceiveDate) IS NOT NULL OR MAX(ReceiveDate) IS NOT NULL, 1, 0)) --不同PackingListID，但相同CTNStartNo，現階段視作相同
        FROM #Is_MixCTNStartNo t2
		WHERE t2.OrderID = t.OrderID AND t2.OrderShipmodeSeq = t.OrderShipmodeSeq
		HAVING MAX(CFAReceiveDate) IS NOT NULL OR MAX(ReceiveDate) IS NOT NULL
    )x
)ClogCTN

----彙整
SELECT   [Article]
		,[SizeCode]
		,[OrderCTN]
		,[CFA_StaggeredCTN]
		,[Staggered %]
		,[ClogCTN]
		,[Clog %]
        ,[OrderKey] = ROW_NUMBER() OVER(ORDER BY Article ASC) 
INTO #Without_Ttl
FROM #Not_Mix_Final
UNION
SELECT   [Article]
		,[SizeCode]
		,[OrderCTN]
		,[CFA_StaggeredCTN]
		,[Staggered %]
		,[ClogCTN]
		,[Clog %]
        ,[OrderKey] = (SELECT COUNT(Article)+1 FROM #Not_Mix_Final)
FROM #Is_Mix_Final

SELECT  [Article]
	,[SizeCode]
	,[Order CTN]=[OrderCTN]
	,[CFA staggered CTN]=[CFA_StaggeredCTN]
	,[Staggered %]
	,[CLOG output]=[ClogCTN] 
	,[CLOG %]
	,[OrderKey]
FROM #Without_Ttl
UNION 
SELECT [Article]='TTL' ----寫死
	,[SizeCode]='TTL' ----寫死
	,[Order CTN]=(SELECT SUM(OrderCTN) FROM #Without_Ttl)	
	,[CFA staggered CTN] = (SELECT SUM(CFA_StaggeredCTN) FROM #Without_Ttl)
	,[Staggered %] = IIF((SELECT SUM(OrderCTN) FROM #Without_Ttl)	 = 0 ,'N/A' , Cast( CAST( ROUND( (SELECT  SUM(CFA_StaggeredCTN) * 1.0 / SUM(OrderCTN) FROM #Without_Ttl),3) * 100 as INT ) as Varchar ) + '%')
	,[CLOG output]=(SELECT SUM(ClogCTN) FROM #Without_Ttl)
	,[CLOG %] = IIF((SELECT SUM(OrderCTN) FROM #Without_Ttl) = 0 ,'N/A' ,Cast( CAST( ROUND( (SELECT SUM(ClogCTN) * 1.0 / SUM(OrderCTN) FROM #Without_Ttl),3) * 100 as INT ) as Varchar ) + '%')
	,[OrderKey] = (SELECT MAX(OrderKey)+1 FROM #Without_Ttl)
ORDER BY OrderKey

DROP TABLE #MixCTNStartNo ,#Is_MixCTNStartNo ,#Not_MixCTNStartNo ,#Not_Mix_Final ,#Is_Mix_Final,#Without_Ttl

";
            #endregion

            // 取得基礎資料
            r = DBProxy.Current.Select(null, cmd, out dtCtnSummary);

            if (!r)
            {
                this.ShowErr(r);
                return;
            }

            var cartonSummary = dtCtnSummary.AsEnumerable().ToList();

            List<string> articles_c = new List<string>();

            #region Gird 客製 : 第一行Column Header的第一欄是固定的，可以先塞

            // 第一欄是固定的
            articles_c.Add("Article");
            articles_c.AddRange(cartonSummary.Select(o => o["Article"].ToString()).Distinct().ToList());

            this._Articles_c = articles_c;
            this.Size_per_Article.Add("Article", 1);

            System.Windows.Forms.DataGridViewTextBoxColumn firstcol_c = new System.Windows.Forms.DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Category/Size",
                Name = "Category/Size",
                HeaderText = "Category/Size",
                Width = 150,
            };

            this.gridCartonSummary.Columns.Add(firstcol_c);
            #endregion

            #region  Gird 客製 : 先產生第二行Column Header (Size)

            foreach (var article in articles_c)
            {
                List<string> sizeCodes = cartonSummary.Where(o => o["Article"].ToString() == article).Select(o => o["SizeCode"].ToString()).ToList();

                if (!this.Size_per_Article.Keys.Contains(article))
                {
                    // 紀錄每個Article有多少個Size，用於後面產生第一行Header
                    this.Size_per_Article.Add(article, sizeCodes.Count());

                    foreach (var sizeCode in sizeCodes)
                    {
                        System.Windows.Forms.DataGridViewTextBoxColumn col = new System.Windows.Forms.DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = article + "_" + sizeCode,
                            Name = article + "_" + sizeCode,
                            HeaderText = sizeCode, /*,
                            Width = 20*/
                        };
                        this.gridCartonSummary.Columns.Add(col);
                    }
                }
            }
            #endregion

            #region 製作Grid的DataSource

            DataTable source_CartonSummary = new DataTable();

            // 設定欄位名稱
            foreach (System.Windows.Forms.DataGridViewTextBoxColumn columm in this.gridCartonSummary.Columns)
            {
                source_CartonSummary.ColumnsStringAdd(columm.Name);
            }

            // 注意！ 這邊要對應到SQL語法的欄位名稱
            List<string> cHeaders = new List<string>();
            cHeaders.Add("Order CTN");
            cHeaders.Add("CFA staggered CTN");
            cHeaders.Add("Staggered %");
            cHeaders.Add("CLOG output");
            cHeaders.Add("CLOG %");

            foreach (var cHeader in cHeaders)
            {
                DataRow newRow = source_CartonSummary.NewRow();
                string dtColumnName = cHeader;

                foreach (System.Windows.Forms.DataGridViewTextBoxColumn columm in this.gridCartonSummary.Columns)
                {
                    string article_SizeCode = columm.Name;
                    if (article_SizeCode == "Category/Size")
                    {
                        newRow[article_SizeCode] = dtColumnName;
                    }
                    else
                    {
                        string article = article_SizeCode.Split('_')[0];
                        string sizeCode = article_SizeCode.Split('_')[1];

                        newRow[article_SizeCode] = cartonSummary.Where(o => o["Article"].ToString() == article && o["SizeCode"].ToString() == sizeCode).FirstOrDefault()[dtColumnName];
                    }
                }

                source_CartonSummary.Rows.Add(newRow);
            }

            source_CartonSummary.AcceptChanges();
            #endregion

            this.gridCartonSummary.DataSource = source_CartonSummary;
            #endregion

            // 動態產生第一行Column Header在這裡面的Paint事件
            this.GridSetting();

            base.OnDetailEntered();
        }

        private void GridQtyBreakdown_Paint(object sender, PaintEventArgs e)
        {
            int col = 0;

            // 一個Article畫一次
            foreach (string article in this._Articles)
            {
                // 宣告要放在第一行的矩形物件
                Rectangle r1 = this.gridQtyBreakdown.GetCellDisplayRectangle(col, -1, true);

                // 取得第二行有幾格
                int sizeCount = this.Size_per_Article[article];

                for (int ctn = 0; ctn < sizeCount; ctn++)
                {
                    Rectangle r2 = this.gridQtyBreakdown.GetCellDisplayRectangle(col + ctn, -1, true);

                    if (r1.Width == 0)
                    {
                        r1 = r2;
                    }
                    else
                    {
                        // 如果超過兩個，則寬度累計上去
                        if (sizeCount > 1)
                        {
                            r1.Width += r2.Width;
                        }
                    }
                }

                // 微調新的矩形位置
                r1.X += -1;

                // r1.Y += 1;
                r1.Height = (r1.Height / 2) - 2;

                // r1.Width -= 1;

                // 重點 !!!  開始畫第一行Header
                using (Brush back = new SolidBrush(this.gridQtyBreakdown.ColumnHeadersDefaultCellStyle.BackColor))
                using (Brush fore = new SolidBrush(this.gridQtyBreakdown.ColumnHeadersDefaultCellStyle.ForeColor))
                using (Pen p = new Pen(this.gridQtyBreakdown.GridColor))
                using (StringFormat format = new StringFormat())
                {
                    // 對齊設定
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Far;

                    // 沿用Grid的字型和樣式
                    e.Graphics.FillRectangle(back, r1);
                    e.Graphics.DrawRectangle(p, r1);

                    // 畫上第一行Header
                    e.Graphics.DrawString(article, this.gridQtyBreakdown.ColumnHeadersDefaultCellStyle.Font, fore, r1, format);
                }

                col += sizeCount; // 這個Article畫完，移動到下一個Article
            }
        }

        private void GridQtyBreakdown_Scroll(object sender, ScrollEventArgs e)
        {
            this.InvalidateHeader();
        }

        private void GridQtyBreakdown_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            this.InvalidateHeader();
        }

        private void GridQtyBreakdown_Resize(object sender, EventArgs e)
        {
            this.InvalidateHeader();
        }

        private void InvalidateHeader()
        {
            Rectangle rtHeader = this.gridQtyBreakdown.DisplayRectangle;
            rtHeader.Height = this.gridQtyBreakdown.ColumnHeadersHeight / 2;
            this.gridQtyBreakdown.Invalidate(rtHeader);

            Rectangle rtHeader_c = this.gridCartonSummary.DisplayRectangle;
            rtHeader_c.Height = this.gridCartonSummary.ColumnHeadersHeight / 2;
            this.gridCartonSummary.Invalidate(rtHeader_c);
        }

        private void GridCartonSummary_Paint(object sender, PaintEventArgs e)
        {
            int col = 0;

            // 一個Article畫一次
            foreach (string article in this._Articles_c)
            {
                // 宣告要放在第一行的矩形物件
                Rectangle r1 = this.gridCartonSummary.GetCellDisplayRectangle(col, -1, true);

                // 取得第二行有幾格
                int sizeCount = this.Size_per_Article[article];

                for (int ctn = 0; ctn < sizeCount; ctn++)
                {
                    Rectangle r2 = this.gridCartonSummary.GetCellDisplayRectangle(col + ctn, -1, true);

                    if (r1.Width == 0)
                    {
                        r1 = r2;
                    }
                    else
                    {
                        // 如果超過兩個，則寬度累計上去
                        if (sizeCount > 1)
                        {
                            r1.Width += r2.Width;
                        }
                    }
                }

                // 微調新的矩形位置
                r1.X += -1;

                // r1.Y += 1;
                r1.Height = (r1.Height / 2) - 2;

                // r1.Width -= 1;

                // 重點 !!!  開始畫第一行Header
                using (Brush back = new SolidBrush(this.gridCartonSummary.ColumnHeadersDefaultCellStyle.BackColor))
                using (Brush fore = new SolidBrush(this.gridCartonSummary.ColumnHeadersDefaultCellStyle.ForeColor))
                using (Pen p = new Pen(this.gridCartonSummary.GridColor))
                using (StringFormat format = new StringFormat())
                {
                    // 對齊設定
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Far;

                    // 沿用Grid的字型和樣式
                    e.Graphics.FillRectangle(back, r1);
                    e.Graphics.DrawRectangle(p, r1);

                    // 畫上第一行Header
                    e.Graphics.DrawString(article, this.gridCartonSummary.ColumnHeadersDefaultCellStyle.Font, fore, r1, format);
                }

                col += sizeCount; // 這個Article畫完，移動到下一個Article
            }
        }

        private void GridCartonSummary_Scroll(object sender, ScrollEventArgs e)
        {
            this.InvalidateHeader();
        }

        private void GridCartonSummary_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            this.InvalidateHeader();
        }

        private void GridCartonSummary_Resize(object sender, EventArgs e)
        {
            this.InvalidateHeader();
        }

        private void GridSetting()
        {
            this.gridQtyBreakdown.AllowUserToAddRows = false;
            this.gridQtyBreakdown.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridQtyBreakdown.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.gridQtyBreakdown.ColumnHeadersHeight = 46; // this.gridQtyBreakdown.ColumnHeadersHeight * 2;
            this.gridQtyBreakdown.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            this.gridQtyBreakdown.RowHeadersVisible = false;

            this.gridQtyBreakdown.IsEditingReadOnly = true;

            // 動態畫出第一行Header
            this.gridQtyBreakdown.Paint += this.GridQtyBreakdown_Paint;
            this.gridQtyBreakdown.Scroll += this.GridQtyBreakdown_Scroll;
            this.gridQtyBreakdown.ColumnWidthChanged += this.GridQtyBreakdown_ColumnWidthChanged;
            this.gridQtyBreakdown.Resize += this.GridQtyBreakdown_Resize;

            /*--------------------我是分隔線--------------------*/

            this.gridCartonSummary.AllowUserToAddRows = false;
            this.gridCartonSummary.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridCartonSummary.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.gridCartonSummary.ColumnHeadersHeight = 46; // this.gridCartonSummary.ColumnHeadersHeight * 2;
            this.gridCartonSummary.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            this.gridCartonSummary.RowHeadersVisible = false;

            this.gridCartonSummary.IsEditingReadOnly = true;

            // 動態畫出第一行Header
            this.gridCartonSummary.Paint += this.GridCartonSummary_Paint;
            this.gridCartonSummary.Scroll += this.GridCartonSummary_Scroll;
            this.gridCartonSummary.ColumnWidthChanged += this.GridCartonSummary_ColumnWidthChanged;
            this.gridCartonSummary.Resize += this.GridCartonSummary_Resize;
        }

        private void ChkForThird_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CurrentMaintain != null)
            {
                if (!this.chkForThird.Checked)
                {
                    string inspectionCtn = MyUtility.GetValue.Lookup($@"
SELECT COUNT(ID)
FROM CFAInspectionRecord
WHERE ID = '{this.CurrentMaintain["ID"].ToString()}' 
AND Seq = '{this.CurrentMaintain["Seq"].ToString()}'
AND Stage='3rd party'
");
                    if (MyUtility.Convert.GetInt(inspectionCtn) > 0)
                    {
                        MyUtility.Msg.WarningBox("There is 3rd party inspection record, can't change.");
                        this.chkForThird.Checked = true;
                        this.CurrentMaintain["CFAIs3rdInspect"] = true;
                    }
                }
            }
        }

        private void BtnH_Click(object sender, EventArgs e)
        {
            string content = MyUtility.GetValue.Lookup($@"
SELECT Packing2
FROM Orders 
WHERE ID='{this.CurrentMaintain["ID"]}'
");
            EditMemo callNextForm = new EditMemo(content, "VAS/SHAS Instruction", false, null);
            callNextForm.ShowDialog(this);
        }

        private void ByCarton_Click(object sender, EventArgs e)
        {
            P31_ByCarton form = new P31_ByCarton(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain["Seq"].ToString());
            form.ShowDialog();
        }

        private void BtnByRecord_Click(object sender, EventArgs e)
        {
            P31_ByRecord form = new P31_ByRecord(this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain["Seq"].ToString());
            form.ShowDialog();
        }

        private void BtnCreateInsRecord_Click(object sender, EventArgs e)
        {
            string cmdSeason = $@"
SELECT  SeasonID
FROM Orders 
WHERE ID = '{this.CurrentMaintain["ID"].ToString()}'
";

            string cmdM = $@"
SELECT  MDivisionid
FROM Orders 
WHERE ID = '{this.CurrentMaintain["ID"].ToString()}'
";

            string cmdDest = $@"
SELECT c.Alias
FROM Orders o
INNER JOIN Country c ON o.Dest = c.ID
WHERE o.ID = '{this.CurrentMaintain["ID"].ToString()}'
";

            string cmdArticle = $@"
SELECT STUFF(
    (SELECT DISTINCT ','+Article 
    FROM Order_QtyShip_Detail 
    WHERE ID = '{this.CurrentMaintain["ID"].ToString()}' AND Seq = '{this.CurrentMaintain["Seq"].ToString()}'
    FOR XML PATH('')
    )
,1,1,'')
";
            P32Header obj = new P32Header()
            {
                OrderID = this.CurrentMaintain["ID"].ToString(),
                Seq = this.CurrentMaintain["Seq"].ToString(),
                PO = this.disPO.Value.ToString(),
                Style = this.CurrentMaintain["StyleID"].ToString(),
                Brand = this.disBrand.Value.ToString(),
                Season = MyUtility.GetValue.Lookup(cmdSeason),
                M = MyUtility.GetValue.Lookup(cmdM),
                Factory = this.CurrentMaintain["FactoryID"].ToString(),
                BuyerDev = this.CurrentMaintain["BuyerDelivery"].ToString(),
                OrderQty = this.CurrentMaintain["Qty"].ToString(),
                Dest = MyUtility.GetValue.Lookup(cmdDest),
                Article = MyUtility.GetValue.Lookup(cmdArticle),
            };

            P32 p32 = new P32(new ToolStripMenuItem(), "1", sourceHeader: obj);
            p32.ShowDialog(this);
        }
    }
}
