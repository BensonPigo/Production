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

namespace Sci.Production.Quality
{
    public partial class P31 : Sci.Win.Tems.Input6
    {
        public string _Type = string.Empty;
        private List<string> _Articles = new List<string>();

        // 每一Article底下的Size數量
        private Dictionary<string, int> Size_per_Article = new Dictionary<string, int>();

        public P31(ToolStripMenuItem menuitem,string type)
            : base(menuitem)
        {
            InitializeComponent();
            this.Text = type == "1" ? "P31. CFA Master List" : "P311. CFA Master List(History)";
            this._Type = type;

            this.DefaultWhere = this._Type == "1" ? $"(SELECT MDivisionID FROM Orders WHERE ID = Order_QtyShip.ID) = '{Sci.Env.User.Keyword}' AND (SELECT Finished FROM Orders WHERE ID = Order_QtyShip.ID) = 0 " : $"(SELECT MDivisionID FROM Orders WHERE ID = Order_QtyShip.ID) = '{Sci.Env.User.Keyword}' AND (SELECT Finished FROM Orders WHERE ID = Order_QtyShip.ID) = 1";
        }


        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.Size_per_Article.Clear();
            this._Articles.Clear();
            this.gridQtyBreakdown.Columns.Clear();

            #region 表頭欄位帶入
            this.txtSpSeq.TextBoxSP.IsSupportEditMode = false;
            this.txtSpSeq.TextBoxSeq.IsSupportEditMode = false;

            this.txtSpSeq.TextBoxSP.ReadOnly = true;
            this.txtSpSeq.TextBoxSeq.ReadOnly = true;

            this.txtSpSeq.TextBoxSP.Text = this.CurrentMaintain["ID"].ToString();
            this.txtSpSeq.TextBoxSeq.Text = this.CurrentMaintain["Seq"].ToString();

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
            this.disDest.Value = MyUtility.GetValue.Lookup($@"
SELECT  Dest
FROM Orders 
WHERE ID = '{this.CurrentMaintain["ID"].ToString()}'
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
and P.SEQ='{this.CurrentMaintain["Seq"].ToString()}'
");

            this.disClogQty.Value = MyUtility.GetValue.Lookup($@"
SELECT ISNULL( SUM(IIF
            ( CFAReceiveDate IS NOT NULL OR ReceiveDate IS NOT NULL
	        ,ShipQty
	        ,0)
        ),0)
FROM PackingList_Detail
WHERE OrderID='{this.CurrentMaintain["ID"].ToString()}' 
and SEQ='{this.CurrentMaintain["Seq"].ToString()}'
");

            this.dateLastCarton.Value = MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup($@"
            select ReceiveDate = max(pd.ReceiveDate) 
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
and pd.SEQ='{this.CurrentMaintain["Seq"].ToString()}'
"));


            this.disVasShas.Value = MyUtility.GetValue.Lookup($@"
SElECT IIF(VasShas=1 ,'Y' ,'N')
FROM Orders
WHERE ID='{this.CurrentMaintain["ID"].ToString()}' 
");

            this.disGarmentTest.Value = MyUtility.GetValue.Lookup($@"
SELECT  IIF(COUNT(ID) > 0 ,'Fail','Pass')
FROM GarmentTest
WHERE OrderID = '{this.CurrentMaintain["ID"].ToString()}'
AND (Result = 'F' OR  Result <> 'P')
");
            #endregion

            // 雙行Column Header的做法
            // 搭配I:\MIS\Personal\Benson\QA P31\CFA.xlxs 第一個Sheet的畫面看比較好懂

            // 開始取得表身兩個Tab的資料
            DataTable dtQtybreakdown;
            DataTable dtCtnSummary;


            DataTable GridQtybreakdown=new DataTable();
            DataTable GridCtnSummary = new DataTable();

            string OrderID = this.CurrentMaintain["ID"].ToString();
            string Seq = this.CurrentMaintain["Seq"].ToString();

            #region Qtybreakdown分頁SQL

            string cmd = $@"
----By Qty Breakdown分頁
SELECT oqd.ID,oqd.Seq,oqd.Article,oqd.SizeCode,oqd.Qty,oqd.OriQty
        ,[CMPOutput]= ISNULL((SELECT dbo.getMinCompleteSewQty( oqd.ID, oqd.Article , oqd.SizeCode ) ) ,0)
        ,[CMP%] = IIF(oqd.Qty = 0 OR (SELECT COUNT(Seq) FROM Order_QtyShip oq WHERE oq.ID = oqd.ID) > 1
				        ,'N/A'
				        , CAST( CAST( ROUND(((ISNULL((SELECT dbo.getMinCompleteSewQty( oqd.ID, oqd.Article , oqd.SizeCode ) ),0) * 1.0  / oqd.Qty ) * 100) ,0) as int )as varchar) + '%'
			         )
        ,[OriCMP%]=IIF(oqd.Qty = 0 OR (SELECT COUNT(Seq) FROM Order_QtyShip oq WHERE oq.ID = oqd.ID) > 1
				        ,0
				        , (ISNULL((SELECT dbo.getMinCompleteSewQty( oqd.ID, oqd.Article , oqd.SizeCode ) ),0) * 1.0 / oqd.Qty ) * 100 
			         )
        ,[CFA staggered Qty] = ISNULL(Staggered.Qty,0)
        ,[Staggered%]=IIF(oqd.Qty = 0
					        , 'N/A'
					        , CAST( CAST(ROUND((ISNULL(Staggered.Qty,0) * 1.0 /oqd.Qty)* 100,0) as int ) as varchar) + '%'
				        )
        ,[OriStaggered%]=IIF(oqd.Qty = 0 
					        ,0
					        , (ISNULL(Staggered.Qty,0) * 1.0 /oqd.Qty)* 100
				         )
        ,[CLOG Qty]=ISNULL(Clog.Qty ,0)
        ,[CLOG%]=IIF(oqd.Qty = 0
					        , 'N/A'
					        , CAST( CAST(ROUND((ISNULL(Clog.Qty,0)/oqd.Qty) * 1.0 * 100 ,0) as int ) as varchar) + '%'
				        )
        ,[OriCLOG%]=IIF(oqd.Qty = 0 
					        ,0
					        , (ISNULL(CLOG.Qty,0)/oqd.Qty) * 1.0 * 100
				         )
        ,[OrderKey] = ROW_NUMBER() OVER(ORDER BY oqd.Article ASC) 
INTO #tmp
FROM Order_QtyShip_Detail oqd
OUTER APPLY(
	SELECT [Qty]=Sum(p.ShipQty)
	From PackingList_Detail P 
	Inner join CFAInspectionRecord CFA on P.StaggeredCFAInspectionRecordID=CFA.ID
	Where CFA.Status='Confirmed' and CFA.Stage='Staggered' and p.Article=oqd.Article and P.SizeCode=oqd.SizeCode and P.OrderID=oqd.ID and P.SEQ=oqd.Seq
)Staggered
OUTER APPLY(
	SELECT [Qty]=SUM(IIF( pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL,pd.ShipQty,0))
	FROM PackingList_Detail pd
	where pd.OrderID=oqd.ID and pd.SEQ = oqd.SEQ and pd.Article= oqd.Article and pd.SizeCode= oqd.SizeCode
)Clog
WHERE oqd.ID='{OrderID}'
AND oqd.Seq='{Seq}'

SELECT --[ID]
	--,[Seq]
	[Article]
	,[SizeCode]
	,[Order Qty]=[Qty]
	,[CMP output]=[CMPOutput]
	,[CMP %]=[CMP%]
	,[CFA staggered output]=[CFA staggered Qty]
	,[Staggered %]=[Staggered%]
	,[CLOG output]=[CLOG Qty]
	,[CLOG %]=[CLOG%]  
	,[OrderKey]
FROM #tmp
UNION 
SELECT --[ID]='{OrderID}'  ----外部帶入寫死
	--,[Seq]='{Seq}'
	[Article]='TTL'
	,[SizeCode]='TTL'
	,[Order Qty]= (SELECT SUM(Qty) FROM #tmp)
	,[CMP output]=(SELECT SUM(CMPOutput) FROM #tmp)
	,[CMP %] = Cast( CAST( ROUND( (SELECT SUM([OriCMP%]) / COUNT([OriCMP%]) FROM #tmp),3) as INT ) as Varchar ) + '%'
	,[CFA staggered output] = (SELECT SUM([CFA staggered Qty]) FROM #tmp)
	,[Staggered %]= Cast(CAST( ROUND( (SELECT SUM([CFA staggered Qty]) / COUNT([CFA staggered Qty]) FROM #tmp),3) as INT)   as Varchar ) + '%'
	,[CLOG output]=(SELECT SUM([CLOG Qty]) FROM #tmp)
	,[CLOG %]=Cast(CAST( ROUND( (SELECT SUM([CLOG Qty]) / COUNT([CLOG Qty]) FROM #tmp),3) as INT)   as Varchar  ) + '%'
	,[OrderKey] = (SELECT MAX(OrderKey) + 1 FROM #tmp)
ORDER BY [OrderKey]

DROP TABLE #tmp
";
            #endregion

            DBProxy.Current.Select(null, cmd, out dtQtybreakdown);

            var Qtybreakdown = dtQtybreakdown.AsEnumerable().ToList();

            List<string> Articles = new List<string>();

            #region 塞第一欄
            // 第一欄是固定的
            Articles.Add("Category/Size");
            Articles.AddRange(Qtybreakdown.Select(o => o["Article"].ToString()).Distinct().ToList());
            
            this._Articles = Articles;
            this.Size_per_Article.Add("Category/Size", 1);

            DataGridViewColumn Firstcol = new DataGridViewColumn()
            {
                Name = "Category/Size",
                HeaderText = "Category/Size"
            };

            this.gridQtyBreakdown.Columns.Add(Firstcol);
            #endregion 

            // 先產生第二行Header (Size) 
            foreach (var Article in Articles)
            {
                List<string> SizeCodes = Qtybreakdown.Where(o => o["Article"].ToString() == Article).Select(o => o["SizeCode"].ToString()).ToList();

                if (!this.Size_per_Article.Keys.Contains(Article))
                {
                    // 紀錄每個Article有多少個Size，用於後面產生第一行Header
                    this.Size_per_Article.Add(Article, SizeCodes.Count());

                    foreach (var SizeCode in SizeCodes)
                    {
                        DataGridViewColumn col = new DataGridViewColumn()
                        {
                            Name = SizeCode,
                            HeaderText = SizeCode/*,
                            Width = 20*/
                        };
                        this.gridQtyBreakdown.Columns.Add(col);
                    }
                }

            }

            this.gridQtyBreakdown.AllowUserToAddRows = false;
            this.gridQtyBreakdown.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridQtyBreakdown.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.gridQtyBreakdown.ColumnHeadersHeight = 46;// this.gridQtyBreakdown.ColumnHeadersHeight * 2;
            this.gridQtyBreakdown.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

            // 動態畫出第一行Header
            this.gridQtyBreakdown.Paint += gridQtyBreakdown_Paint;

            this.gridQtyBreakdown.Scroll += gridQtyBreakdown_Scroll;
            this.gridQtyBreakdown.ColumnWidthChanged += gridQtyBreakdown_ColumnWidthChanged;
            this.gridQtyBreakdown.Resize += gridQtyBreakdown_Resize;

            #region Carton Summary分頁SQL
            cmd = $@"
----Carton Summary分頁
----記錄哪些箱號有混尺碼
SELECT ID,OrderID,OrderShipmodeSeq,CTNStartNo
		,[ArticleCount]=COUNT(DISTINCT Article)
		,[SizeCodeCount]=COUNT(DISTINCT SizeCode)
INTO #MixCTNStartNo
FROM PackingList_Detail pd
WHERE OrderID LIKE '{OrderID}' 
AND OrderShipmodeSeq ='{Seq}'
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
WHERE OrderID LIKE '{OrderID}'
AND  OrderShipmodeSeq ='{Seq}'
AND CTNStartNo NOT IN (SELECT CTNStartNo FROM #MixCTNStartNo)

----記錄哪些箱號 是 混尺碼
SELECT ID, OrderID, OrderShipmodeSeq, CTNStartNo
        , Article
        , SizeCode
        , StaggeredCFAInspectionRecordID
        , CFAReceiveDate
        , ReceiveDate
INTO #Is_MixCTNStartNo
FROM PackingList_Detail pd
WHERE OrderID LIKE '{OrderID}'
AND OrderShipmodeSeq = '{Seq}'
AND CTNStartNo IN(SELECT CTNStartNo FROM #MixCTNStartNo)

----先計算 不是 混尺碼
SELECT  oqd.ID, oqd.Seq, oqd.Article, oqd.SizeCode
        ,[OrderCTN] = OrderCTN.Val
        ,[CFA_StaggeredCTN] = ISNULL(CFA_StaggeredCTN.Val, 0)
        ,[Staggered %] = IIF(ISNULL(OrderCTN.Val, 0) = 0
                                , 'N/A'
                                , CAST(CAST(ROUND((ISNULL(CFA_StaggeredCTN.Val, 0) * 1.0 / ISNULL(OrderCTN.Val, 0)) * 100, 0) AS Int) as varchar) + '%'
                            )
        ,[OriStaggered %] = IIF(ISNULL(OrderCTN.Val, 0) = 0
                            , 0
                            , (ISNULL(CFA_StaggeredCTN.Val, 0) / ISNULL(OrderCTN.Val, 0)) * 100
                         )
        ,[ClogCTN] = ClogCTN.Val
        ,[Clog %] = IIF(ISNULL(OrderCTN.Val, 0) = 0
                                , 'N/A'
                                , CAST(CAST(ROUND((ISNULL(ClogCTN.Val, 0) * 1.0 / ISNULL(OrderCTN.Val, 0)) * 100, 0) AS Int) as varchar) + '%'
                            )
        ,[OriClog %] = IIF(ISNULL(OrderCTN.Val, 0) = 0
                            , 0
                            , (ISNULL(ClogCTN.Val, 0) / ISNULL(OrderCTN.Val, 0)) * 100
                         )
INTO #Not_Mix_Final
FROM Order_QtyShip_Detail oqd
OUTER APPLY(
    SELECT t.Article, t.SizeCode,[Val] = COUNT(DISTINCT t.CTNStartNo)

    FROM #Not_MixCTNStartNo t
	WHERE  t.OrderID = oqd.Id

        AND t.OrderShipmodeSeq = oqd.Seq

        AND t.Article = oqd.Article

        AND t.SizeCode = oqd.SizeCode

    GROUP BY t.Article, t.SizeCode
)OrderCTN
OUTER APPLY(
    SELECT t.Article, t.SizeCode,[Val] = COUNT(DISTINCT t.CTNStartNo)

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
    SELECT[Val] = SUM(IIF(t.CFAReceiveDate IS NOT NULL OR t.ReceiveDate IS NOT NULL, 1, 0))

    FROM #Not_MixCTNStartNo t
	WHERE t.OrderID = oqd.Id

        AND t.OrderShipmodeSeq = oqd.Seq

        AND t.Article = oqd.Article

        AND t.SizeCode = oqd.SizeCode
)ClogCTN
WHERE oqd.ID = '{OrderID}' AND oqd.Seq = '{Seq}'

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
        ,[OriStaggered %] = IIF(ISNULL(OrderCTN.Val, 0) = 0
                            , 0
                            , (ISNULL(CFA_StaggeredCTN.Val, 0) / ISNULL(OrderCTN.Val, 0)) * 100
                         )
        ,[ClogCTN] = ClogCTN.Val
        ,[Clog %] = IIF(ISNULL(OrderCTN.Val, 0) = 0
                                , 'N/A'
                                , CAST(CAST(ROUND((ISNULL(ClogCTN.Val, 0) * 1.0 / ISNULL(OrderCTN.Val, 0)) * 100, 0) AS Int) as varchar) + '%'
                            )
        ,[OriClog %] = IIF(ISNULL(OrderCTN.Val, 0) = 0
                            , 0
                            , (ISNULL(ClogCTN.Val, 0) / ISNULL(OrderCTN.Val, 0)) * 100
                         )
INTO #Is_Mix_Final
FROM #Is_MixCTNStartNo t
OUTER APPLY(
    SELECT[Val] = COUNT(DISTINCT CTNStartNo)

    FROM #Is_MixCTNStartNo
)OrderCTN
OUTER APPLY(
    SELECT[Val] = COUNT(DISTINCT CTNStartNo)

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
        SELECT[Ctn] = (IIF(MAX(CFAReceiveDate) IS NOT NULL OR MAX(ReceiveDate) IS NOT NULL, 1, 0))

        FROM #Is_MixCTNStartNo t2
		WHERE t2.OrderID = t.OrderID AND t2.OrderShipmodeSeq = t.OrderShipmodeSeq
    )x
)ClogCTN

----彙整
SELECT[ID]
    ,[Seq]
    ,[Article]
    ,[SizeCode]
    ,[OrderCTN]
    ,[CFA_StaggeredCTN]
    ,[Staggered %]
    ,[OriStaggered %]
    ,[ClogCTN]
    ,[Clog %]
    ,[OriClog %]
INTO #Without_Ttl
FROM #Not_Mix_Final
UNION
SELECT[ID]
    ,[Seq]
    ,[Article]
    ,[SizeCode]
    ,[OrderCTN]
    ,[CFA_StaggeredCTN]
    ,[Staggered %]
    ,[OriStaggered %]
    ,[ClogCTN]
    ,[Clog %]
    ,[OriClog %]
FROM #Is_Mix_Final

SELECT  --[ID]
	--,[Seq]
	[Article]
	,[SizeCode]
	,[Order CTN]=[OrderCTN]
	,[CFA staggered CTN]=[CFA_StaggeredCTN]
	,[Staggered %]=[Staggered%] 
	,[CLOG output]=[ClogCTN] 
	,[CLOG %]=[Clog%]
FROM #Without_Ttl
UNION 
SELECT --[ID]='{OrderID}'  ----外部帶入寫死
	--,[Seq]='{Seq}' ----外部帶入寫死
	[Article]='Total' ----外部帶入寫死
	,[SizeCode]='Total' ----外部帶入寫死
	,[Order CTN]=(SELECT SUM(OrderCTN) FROM #Without_Ttl)	
	,[CFA staggered CTN] = (SELECT SUM(CFA_StaggeredCTN) FROM #Without_Ttl)
	,[Staggered %] = Cast( CAST( ROUND( (SELECT SUM([OriStaggered%]) / COUNT([OriStaggered%]) FROM #Without_Ttl),3) as INT ) as Varchar ) + '%'
	,[CLOG output]=(SELECT SUM(ClogCTN) FROM #Without_Ttl)
	,[CLOG %] = Cast( CAST( ROUND( (SELECT SUM([OriClog%]) / COUNT([OriClog%]) FROM #Without_Ttl),3) as INT ) as Varchar ) + '%'

DROP TABLE #MixCTNStartNo ,#Is_MixCTNStartNo ,#Not_MixCTNStartNo ,#Not_Mix_Final ,#Is_Mix_Final,#Without_Ttl

";
            #endregion

            DBProxy.Current.Select(null, cmd, out dtCtnSummary);



            //this.dataSourceCtnSummary.DataSource = GridCtnSummary;
        }


        private void gridQtyBreakdown_Paint(object sender, PaintEventArgs e)
        {
            int col = 0;

            // 一個Article畫一次
            foreach (string Article in this._Articles)
            {
                // 宣告要放在第一行的矩形物件
                Rectangle r1 = this.gridQtyBreakdown.GetCellDisplayRectangle(col, -1, true);

                // 取得第二行有幾格
                int SizeCount = this.Size_per_Article[Article];

                for (int ctn = 0; ctn < SizeCount; ctn++)
                {
                    Rectangle r2 = this.gridQtyBreakdown.GetCellDisplayRectangle(col + ctn, -1, true);

                    if (r1.Width == 0)
                    {
                        r1 = r2;
                    }
                    else
                    {
                        // 如果超過兩個，則寬度累計上去
                        if(SizeCount > 1)
                        {
                            r1.Width += r2.Width;
                        }
                    }
                }

                // 微調新的矩形位置
                r1.X += -1;
                //r1.Y += 1;
                r1.Height = r1.Height / 2 - 2;
                //r1.Width -= 1;

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
                    e.Graphics.DrawString(Article, this.gridQtyBreakdown.ColumnHeadersDefaultCellStyle.Font, fore, r1, format);
                }

                col += SizeCount; // 這個Article畫完，移動到下一個Article
            }
        }

        private void gridQtyBreakdown_Scroll(object sender, ScrollEventArgs e)
        {
            this.InvalidateHeader();
        }

        private void gridQtyBreakdown_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            this.InvalidateHeader();
        }

        private void gridQtyBreakdown_Resize(object sender, EventArgs e)
        {
            this.InvalidateHeader();
        }

        private void InvalidateHeader()
        {
            Rectangle rtHeader = this.gridQtyBreakdown.DisplayRectangle;
            rtHeader.Height = this.gridQtyBreakdown.ColumnHeadersHeight / 2;
            this.gridQtyBreakdown.Invalidate(rtHeader);
        }


        private void chkForThird_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CurrentMaintain !=  null)
            {
                if (!this.chkForThird.Checked)
                {
                    string InspectionCtn = MyUtility.GetValue.Lookup($@"
SELECT COUNT(ID)
FROM CFAInspectionRecord
WHERE ID = '{this.CurrentMaintain["ID"].ToString()}' 
AND Seq = '{this.CurrentMaintain["Seq"].ToString()}'
AND Stage='3rd party'
");
                    if (MyUtility.Convert.GetInt(InspectionCtn) > 0)
                    {
                        MyUtility.Msg.WarningBox("There is 3rd party inspection record, can't change.");
                        this.chkForThird.Checked = true;
                        this.CurrentMaintain["CFAIs3rdInspect"] = true;
                    }
                }

            }
        }
    }
}
