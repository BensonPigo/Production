using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Sci.Data;
using Ict;
using Word = Microsoft.Office.Interop.Word;
using System.Linq;
using System.Runtime.InteropServices;
using Sci.Production.Class;
using System.Runtime.DesignerServices;
using System.IO;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Drawing;
using Sci.Production.PublicPrg;
using System.Threading;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P01_TrimCardPrint : Win.Tems.PrintForm
    {
        private DataTable dtPrint_Content;
        private DataTable dtPrint_LeftColumn;
        private DataTable dtColor;
        private DataTable dtMutiColor;
        private DataRow rowColor;
        private string sql;
        private string temp;
        private string orderID;
        private string StyleID;
        private string SeasonID;
        private string FactoryID;
        private string BrandID;
        private string POID;
        private List<string> ListColor = new List<string>();
        private string FabricPath;
        private string ColorPath;

        /// <inheritdoc/>
        public P01_TrimCardPrint(string orderID, string styleID, string seasonID, string factoryID, string brandID, string pOID)
        {
            this.InitializeComponent();
            this.orderID = orderID;
            this.StyleID = styleID;
            this.SeasonID = seasonID;
            this.FactoryID = factoryID;
            this.BrandID = brandID;
            this.POID = pOID;

            // 取得color & Fabric 圖片檔路徑
            if (MyUtility.Check.Seek("select FabricPath,ColorPath from System",out DataRow dr))
            {
                this.FabricPath = dr["FabricPath"].ToString();
                this.ColorPath = dr["ColorPath"].ToString();
            }
        }

        // 欄位檢核

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.radioFabric.Checked && !this.radioAccessory.Checked && !this.radioOther.Checked && !this.radioThread.Checked)
            {
                this.ShowErr("Please select an item !!");
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult result = Ict.Result.True;
            if (this.dtPrint_Content != null)
            {
                this.dtPrint_Content.Rows.Clear();
            }

            #region SQL
            if (this.radioFabric.Checked)
            {
                #region FABRIC
                this.sql = $@"
select  A.PatternPanel 
		, A.FabricCode 
		, B.Refno 
		, f.Description 
		, A.FabricPanelCode
        , f.Picture
        , f.BrandID
		, occ.Article
		, occ.ColorID
		, [ColorName] = c.Name
        , [ColorPicture] = c.Picture
into #tmpFabricInfo
from Orders o WITH (NOLOCK) 
inner join Order_ColorCombo occ WITH (NOLOCK) on o.id = occ.ID
inner join Color c with(nolock) on c.ID = occ.ColorID
                                    and c.BrandID = o.BrandID
inner join Order_FabricCode A WITH (NOLOCK) on a.ID = occ.ID and a.FabricPanelCode = occ.FabricPanelCode
left join Order_BOF B WITH (NOLOCK) on B.Id=A.Id and B.FabricCode=A.FabricCode
left join Fabric f WITH (NOLOCK) on f.SCIRefno = B.SCIRefno
where o.POID = '{this.POID}'
order by FabricCode

SELECT	[ColorID] = t.ColorID,
		[ChildColor] = m.ColorID,
		[Picture] = c.Picture,
		[ChildColorName] = c.Name
into #tmpMutiColor
from (select distinct ColorID, BrandID from #tmpFabricInfo) t
inner join dbo.Color_multiple m on m.ID = t.ColorID and m.BrandID = t.BrandID 
inner join dbo.Color c on c.BrandId = m.BrandID and c.ID = m.ColorID
where m.ColorID <> t.ColorID
order by t.ColorID, m.Seqno

select distinct Article from #tmpFabricInfo

SELECT	distinct
		FabricPanelCode,
		FabricCode,
		Refno,
		Description,
		[MainPicture] = Picture,
        [ColorSer] = ROW_NUMBER() OVER (PARTITION BY FabricPanelCode, FabricCode, Refno, Article ORDER BY ColorID)
from #tmpFabricInfo
order by FabricCode

SELECT	tf.FabricPanelCode,
        tf.FabricCode,
        tf.Article,
		tf.ColorID,
		tf.Refno,
		[ColorDesc] = ColorName.val + CHAR(13) + CHAR(10) + ColorID.val,
        [Picture] = tf.ColorPicture,
        [IsWritedExcel] = cast(0 as bit)
from #tmpFabricInfo tf
outer apply(select val = isnull(stuff((select concat('/', t.ChildColorName)
							 from #tmpMutiColor t
							 where t.ColorID = tf.ColorID
							 for xml path(''))
							, 1, 1, ''), tf.ColorName) ) ColorName
outer apply(select val = isnull(stuff((select concat('/', t.ChildColor)
							 from #tmpMutiColor t
							 where t.ColorID = tf.ColorID
							 for xml path(''))
							, 1, 1, ''), tf.ColorID) ) ColorID
order by count(*) OVER (PARTITION BY tf.FabricPanelCode, tf.FabricCode, tf.Refno, tf.ColorID) desc

select * from #tmpMutiColor

drop table #tmpFabricInfo, #tmpMutiColor
";
                DataTable[] listThreadResult;
                result = DBProxy.Current.Select(null, this.sql, out listThreadResult);
                if (!result)
                {
                    return result;
                }

                this.dtPrint_LeftColumn = listThreadResult[0];
                this.dtPrint_Content = listThreadResult[1];
                this.dtColor = listThreadResult[2];
                this.dtMutiColor = listThreadResult[3];
                #endregion
            }
            else if (this.radioAccessory.Checked)
            {
                #region ACCESSORY
                this.sql = $@"
select	distinct
        A.Refno,
		occ.Article,
		occ.ColorID,
		B.Description,
		B.Picture,
		B.BrandID,
		[ColorName] = c.Name,
		[ColorPicture] = c.Picture
into #tmpAccessoryInfo
from Order_BOA A WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on a.id = o.poid 
inner join Order_Article oa With (NoLock) on o.id = oa.id
inner join Order_ColorCombo occ With(NoLock) on a.id = occ.Id
												and a.FabricPanelCode = occ.FabricPanelCode
                                                and oa.Article = occ.Article
inner join Color c with(nolock) on c.ID = occ.ColorID
                                    and c.BrandID = o.BrandID
left join Fabric B WITH (NOLOCK) on B.SCIRefno=A.SCIRefno
where o.POID = '{this.POID}' 
	  and oa.Article <> '' 
	  and occ.ColorId<>''
	  and b.MtlTypeID in (select id from MtlType WITH (NOLOCK) where IsTrimcardOther=0)


SELECT	[ColorID] = t.ColorID,
		[ChildColor] = m.ColorID,
		[Picture] = c.Picture,
		[ChildColorName] = c.Name
into #tmpMutiColor
from (select distinct ColorID, BrandID from #tmpAccessoryInfo) t
inner join dbo.Color_multiple m on m.ID = t.ColorID and m.BrandID = t.BrandID 
inner join dbo.Color c on c.BrandId = m.BrandID and c.ID = m.ColorID
where m.ColorID <> t.ColorID
order by t.ColorID, m.Seqno

select distinct Article from #tmpAccessoryInfo

SELECT	distinct
		Refno,
		Description,
		[MainPicture] = Picture,
        [ColorSer] = ROW_NUMBER() OVER (PARTITION BY Refno, Article ORDER BY ColorID)
from #tmpAccessoryInfo

SELECT	tf.Article,
		tf.ColorID,
		tf.Refno,
		[ColorDesc] = ColorName.val + CHAR(13) + CHAR(10) + ColorID.val,
        [Picture] = tf.ColorPicture,
        [IsWritedExcel] = cast(0 as bit),
		count(*) OVER (PARTITION BY tf.Refno, tf.ColorID)
from #tmpAccessoryInfo tf
outer apply(select val = isnull(stuff((select concat('/', t.ChildColorName)
							 from #tmpMutiColor t
							 where t.ColorID = tf.ColorID
							 for xml path(''))
							, 1, 1, ''), tf.ColorName) ) ColorName
outer apply(select val = isnull(stuff((select concat('/', t.ChildColor)
							 from #tmpMutiColor t
							 where t.ColorID = tf.ColorID
							 for xml path(''))
							, 1, 1, ''), tf.ColorID) ) ColorID
order by count(*) OVER (PARTITION BY tf.Refno, tf.ColorID) desc

select * from #tmpMutiColor

drop table #tmpAccessoryInfo, #tmpMutiColor
";
                DataTable[] listThreadResult;
                result = DBProxy.Current.Select(null, this.sql, out listThreadResult);
                if (!result)
                {
                    return result;
                }

                this.dtPrint_LeftColumn = listThreadResult[0];
                this.dtPrint_Content = listThreadResult[1];
                this.dtColor = listThreadResult[2];
                this.dtMutiColor = listThreadResult[3];
                #endregion
            }
            else if (this.radioOther.Checked)
            {
                #region OTHER
                // 架構要調，先HOLD住
                this.sql = string.Format(
                    @"
select distinct ob.Refno
	    , B.DescDetail
        , B.Picture
        , o.BrandID
from Orders o WITH (NOLOCK) 
inner join Orders allOrder With (NoLock) on o.Poid = allOrder.Poid
inner join Order_BOA ob WITH (NOLOCK) on allOrder.ID = ob.ID
left join Fabric B WITH (NOLOCK) on B.SCIRefno = ob.SCIRefno
where o.Id = '{0}'
	  and b.MtlTypeID in (select id from MtlType WITH (NOLOCK) where IsTrimcardOther = 1)
	  and not ob.SuppID = 'fty' 
	  and not ob.SuppID = 'fty-c'", this.orderID);
                result = DBProxy.Current.Select(null, this.sql, out this.dtPrint_Content);
                if (!result)
                {
                    return result;
                }

                this.sql = string.Format(
                    @"select distinct orders.ID 
                                    from orders WITH (NOLOCK) 
                                    where orders.POID='{0}'", this.POID);
                result = DBProxy.Current.Select(null, this.sql, out this.dtPrint_LeftColumn);
                if (!result)
                {
                    return result;
                }
                #endregion
            }
            else if (this.radioThread.Checked)
            {
                #region Thread

                // 1.判斷新舊單，判斷Article要從哪裡來
                bool isNewOrder = false;
                DataTable dt_CheckOld;
                string sqlCmd_1 = $@"
select distinct B.Article
from Orders o WITH (NOLOCK) 
inner join Orders allOrder With (NoLock) on o.Poid = allOrder.Poid
inner join ThreadRequisition_Detail A WITH (NOLOCK) on allOrder.ID = a.orderid
left join ThreadRequisition_Detail_Cons B WITH (NOLOCK) on B.ThreadRequisition_DetailUkey = A.Ukey
where o.iD = '{this.POID}' and article<>''  
";

                result = DBProxy.Current.Select(null, sqlCmd_1, out dt_CheckOld);
                if (!result)
                {
                    return result;
                }

                // 2.區分新舊單，Article來源不同
                if (dt_CheckOld.Rows.Count == 0)
                {
                    isNewOrder = true;
                }

                // 3.找出對應的Color，新單的話，由於是透過Order的StyleUkey去串出Article，母單子單都是相同StyleUkey
                // 因此參考 WH P03 OrderList欄位，找出這個繡線被用在哪一些訂單
                if (isNewOrder)
                {
                    #region 取得Article和對應的Color，只挑選出order List有的Article

                    this.sql = $@"

--1.從訂單串回物料
select distinct StyleID,BrandID,POID,FtyGroup 
into #tmpOrder
from orders where id = '{this.orderID}'


Select distinct [ID] = PO.POID
, std.SuppId --Mapping PO_Supp
, std.SCIRefNo, std.ColorID, std.Article --Mapping PO_Supp_Detail
into #ArticleForThread_Detail
From #tmpOrder PO
Inner Join dbo.Orders as o On o.ID = po.POID 
Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
Inner Join dbo.Style_ThreadColorCombo as st On st.StyleUkey = s.Ukey
Inner Join dbo.Style_ThreadColorCombo_Detail as std On std.Style_ThreadColorComboUkey = st.Ukey


--2.得到跟WH P03 一樣的對應方式
SELECT DISTINCT orders.POID
				,[Article] = aft.Article
INTO #tmpOrder_Article
from #tmpOrder as orders WITH (NOLOCK) 
inner join PO_Supp_Detail a WITH (NOLOCK) on a.id = orders.poid
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = a.id and psdsC.seq1 = a.seq1 and psdsC.seq2 = a.seq2 and psdsC.SpecColumnID = 'Color'
left join dbo.MDivisionPoDetail m WITH (NOLOCK) on  m.POID = a.ID and m.seq1 = a.SEQ1 and m.Seq2 = a.Seq2
left join po_supp b WITH (NOLOCK) on a.id = b.id and a.SEQ1 = b.SEQ1
left join #ArticleForThread_Detail aft on	aft.ID = m.POID		and
											aft.SuppId	   = b.SuppId	and
											aft.SCIRefNo   = a.SCIRefNo	and
											aft.ColorID	   = psdsC.SpecValue	and
											a.SEQ1 like 'T%' 
WHERE	aft.Article IS NOT NULL AND
		exists(select 1 from po_supp_Detail_orderList e
			            where e.ID = a.ID and e.SEQ1 =a.SEQ1 and e.SEQ2 = a.SEQ2 AND e.orderID = e.ID)

--3.再回去Style_ThreadColorCombo串出需要的資料
 select DISTINCT 
    B.Article
    , [ThreadColorID] = B.ColorID
    , c.Picture
    , [Refno] = isnull(f.Refno, '')
    , [Description] = isnull(f.Description, '')
    , [ThreadPicture] = isnull(f.Picture, '')
    , o.BrandID
    , [ColorName] = c.Name
into #tmpThreadInfo
 from Orders o WITH (NOLOCK) 
 inner join Style_ThreadColorCombo A WITH (NOLOCK) on o.StyleUkey = a.StyleUkey
 inner join Style_ThreadColorCombo_Detail B WITH (NOLOCK) on B.Style_ThreadColorComboUkey = A.Ukey
 inner join Color c with(nolock) on c.ID = b.ColorID
                                    and c.BrandID = o.BrandID
 inner join Fabric f with (nolock) on f.SCIRefno = b.SCIRefNo
 where o.ID = '{this.POID}'
 AND (
     EXISTS (SELECT 1 FROM #tmpOrder_Article WHERE POID = o.ID AND Article = B.Article) 
     OR
     (SELECT COUNT(1) FROM #tmpOrder_Article) = 0
 )

SELECT	[ColorID] = t.ThreadColorID,
		[ChildColor] = m.ColorID,
		[Picture] = c.Picture,
		[ChildColorName] = c.Name
into #tmpMutiColor
from (select distinct ThreadColorID, BrandID from #tmpThreadInfo) t
inner join dbo.Color_multiple m on m.ID = t.ThreadColorID and m.BrandID = t.BrandID 
inner join dbo.Color c on c.BrandId = m.BrandID and c.ID = m.ColorID
where m.ColorID <> t.ThreadColorID
order by t.ThreadColorID, m.Seqno

select  distinct Article
from    #tmpThreadInfo

select  distinct    Refno,
                    Description,
                    [MainPicture] = ThreadPicture,
                    [ColorSer] = ROW_NUMBER() OVER (PARTITION BY refno, article ORDER BY ThreadColorID)
from    #tmpThreadInfo

select	tThread.Article,
		[ColorID] = tThread.ThreadColorID,
		tThread.Refno,
		[ColorDesc] = ColorName.val + CHAR(13) + CHAR(10) + ColorID.val,
        tThread.Picture,
        [IsWritedExcel] = cast(0 as bit)
from #tmpThreadInfo tThread
outer apply(select val = isnull(stuff((select concat('/', t.ChildColorName)
							 from #tmpMutiColor t
							 where t.ColorID = tThread.ThreadColorID
							 for xml path(''))
							, 1, 1, ''), tThread.ColorName) ) ColorName
outer apply(select val = isnull(stuff((select concat('/', t.ChildColor)
							 from #tmpMutiColor t
							 where t.ColorID = tThread.ThreadColorID
							 for xml path(''))
							, 1, 1, ''), tThread.ThreadColorID) ) ColorID
order by count(*) OVER (PARTITION BY tThread.refno, tThread.ThreadColorID) desc

select * from #tmpMutiColor

DROP TABLE #tmpOrder, #ArticleForThread_Detail, #tmpOrder_Article, #tmpThreadInfo, #tmpMutiColor
";

                    #endregion
                    DataTable[] listThreadResult;
                    result = DBProxy.Current.Select(null, this.sql, out listThreadResult);
                    if (!result)
                    {
                        return result;
                    }

                    this.dtPrint_LeftColumn = listThreadResult[0];
                    this.dtPrint_Content = listThreadResult[1];
                    this.dtColor = listThreadResult[2];
                    this.dtMutiColor = listThreadResult[3];
                }
                else
                {
                    #region 找出對應的Article、Color，由於舊單的繡線物料不會直接顯示在Seq，因此無法按照新單的做法

                    this.sql = $@"

SELECT * into #tmpThreadInfo  FROM
(
	select 
	  B.Article
	, A.ThreadColorID
    , [Picture] = ''
    , [Refno] = ''
    , [Description] = ''
    , [ThreadPicture] = ''
	from Orders o WITH (NOLOCK) 
	inner join Orders allOrder With (NoLock) on o.Poid = allOrder.Poid
	inner join ThreadRequisition_Detail A WITH (NOLOCK) on allOrder.id = a.OrderID
	left join ThreadRequisition_Detail_Cons B WITH (NOLOCK) on B.ThreadRequisition_DetailUkey = A.Ukey
	where o.ID = '{this.POID}' and article<>'' AND allOrder.ID= '{this.orderID}'
	group by article, threadcolorid

	UNION  --加上當地採購
	SELECT DISTINCT 
	[Artuicle] = IIF(BOA_Article.Article IS NULL 
					,Order_Article.Article
					,BOA_Article.Article 
					)				

	, [ThreadColorID] = IIF(PSD.SuppColor = '',dbo.GetColorMultipleID('{this.BrandID}', psdsC.SpecValue),PSD.SuppColor)
    , [Picture] = ''
    , [Refno] = ''
    , [Description] = ''
    , [ThreadPicture] = ''
	FROM PO_Supp_Detail PSD
	INNER join Fabric f on f.SCIRefno = PSD.SCIRefno
    left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = PSD.id and psdsC.seq1 = PSD.seq1 and psdsC.seq2 = PSD.seq2 and psdsC.SpecColumnID = 'Color'	
    LEFT JOIN Order_BOA boa ON boa.id =psd.ID and boa.SCIRefno = psd.SCIRefno and boa.seq=psd.SEQ1
	LEFT JOIN PO_Supp_Detail_OrderList pd ON PSD.ID = pd.ID AND PSD.SEQ1= pd.SEQ1 AND PSD.SEQ2= pd.SEQ2
	OUTER APPLY(
		SELECT oba.Article FROM Order_BOA ob
		LEFT join Order_BOA_Article oba on oba.Order_BoAUkey = ob.Ukey
		WHERE ob.id =psd.ID and ob.SCIRefno = psd.SCIRefno and ob.seq=psd.SEQ1
	)BOA_Article
	OUTER APPLY(
		SELECT Article 
		FROM Order_Article 
		WHERE id =psd.ID
	)Order_Article
	WHERE PSD.ID ='{this.POID}' and f.MtltypeId like '%thread%' AND PSD.Junk=0 AND boa.Ukey IS NOT NULL  AND pd.OrderID='{this.orderID}'
)A
WHERE ThreadColorID <> '' AND ThreadColorID IS NOT NULL


UNION 

 select --o.ID,
 B.Article
 , B.ColorID AS ThreadColorID
 , c.Picture
 , [Refno] = isnull(f.Refno, '')
 , [Description] = isnull(f.Description, '')
 , [ThreadPicture] = isnull(f.Picture, '')
 from Orders o WITH (NOLOCK) 
 inner join Orders allOrder With (NoLock) on o.Poid = allOrder.Poid
 inner join Style_ThreadColorCombo A WITH (NOLOCK) on allOrder.StyleUkey = a.StyleUkey
 left join Style_ThreadColorCombo_Detail B WITH (NOLOCK) on B.Style_ThreadColorComboUkey = A.Ukey
 inner join Color c with(nolock) on c.ID = b.ColorID
                                    and c.BrandID = o.BrandID
 inner join Fabric f with (nolock) on f.SCIRefno = b.SCIRefNo
 where o.ID = '{this.POID}' --and article<>''
 group by B.Article, B.ColorID, isnull(f.Refno, ''), isnull(f.Description, ''), isnull(f.Picture, ''), c.Picture


ORDER BY ThreadColorID ASC

select  distinct Article
from    #tmpThreadInfo

select  distinct    Refno, Description, [MainPicture] = ThreadPicture, [ColorID] = ThreadColorID
from    #tmpThreadInfo

select  tThread.Article,
		[ColorID] = tThread.ThreadColorID,
		tThread.Refno,
		[ColorDesc] = '',
        tThread.Picture
from #tmpThreadInfo tThread

select  [ColorID] = '', [ChildColor] = '', [Picture] = '', [ChildColorName] = ''

drop table #tmpThreadInfo
";
                    #endregion
                    DataTable[] listThreadResult;
                    result = DBProxy.Current.Select(null, this.sql, out listThreadResult);
                    if (!result)
                    {
                        return result;
                    }

                    this.dtPrint_LeftColumn = listThreadResult[0];
                    this.dtPrint_Content = listThreadResult[1];
                    this.dtColor = listThreadResult[2];
                }
                #endregion
            }
            #endregion
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.dtPrint_Content.Rows.Count == 0)
            {
                MessageBox.Show("Data not found!!");
                return false;
            }

            this.SetCount(this.dtPrint_Content.Rows.Count);
            DualResult result = Ict.Result.True;

            // decimal intRowsCount = dtPrint.Rows.Count;
            // int page = Convert.ToInt16(Math.Ceiling(intRowsCount / 4));
            object temfile;

            if (this.radioOther.Checked)
            {
                temfile = Env.Cfg.XltPathDir + "\\Warehouse-P01.TrimCardPrint_B.dotx";
            }
            else
            {
                temfile = Env.Cfg.XltPathDir + "\\Warehouse-P01.TrimCardPrint_A.dotx";
            }

            this.ShowWaitMessage(temfile.ToString());
            Word._Application winword = new Word.Application();
            winword.FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip;

            // Set status for word application is to be visible or not.
            winword.Visible = false;

            // Create a new document
            Word._Document document = winword.Documents.Add(ref temfile);

            Word.Table tables = null;

            try
            {
                int intThreadMaxLength = 0;
                document.Activate();
                Word.Tables table = document.Tables;

                // retreive the first table of the document
                tables = table[1];

                #region ROW1
                if (this.radioOther.Checked)
                {
                    if (this.dtPrint_LeftColumn.Rows.Count > 1)
                    {
                        tables.Cell(1, 1).Range.Text = string.Format("LABELLING & PACKAGING (SP# {0} - {1})", this.orderID, this.dtPrint_LeftColumn.Rows[this.dtPrint_LeftColumn.Rows.Count - 1]["id"].ToString().Substring(8));
                    }
                    else
                    {
                        tables.Cell(1, 1).Range.Text = string.Format("LABELLING & PACKAGING (SP# {0})", this.orderID);
                    }
                }
                else
                {
                    tables.Cell(1, 1).Range.Text = string.Format("SP#{0}     ST:{1}     SEASON:{2}     FTY:{3}", this.orderID, this.StyleID, this.SeasonID, this.FactoryID);
                }
                #endregion

                #region ROW2
                string row2Type = string.Empty;

                if (this.radioFabric.Checked)
                 {
                     row2Type = "FABRIC";
                 }
                 else if (this.radioAccessory.Checked)
                 {
                     row2Type = "ACCESSORY";
                 }
                 else if (this.radioOther.Checked)
                 {
                     row2Type = "OTHER";
                 }
                 else if (this.radioThread.Checked)
                 {
                     row2Type = "Thread";
                 }
                #endregion

                #region 計算共要幾頁
                int pagecount;
                int cC, rC;
                if (this.radioOther.Checked)
                {
                    cC = ((this.dtPrint_Content.Rows.Count - 1) / 6) + 1;
                    rC = 1;
                    pagecount = cC * rC;
                }
                else
                {
                    cC = ((this.dtPrint_Content.Rows.Count - 1) / 6) + 1;
                    rC = ((this.dtPrint_LeftColumn.Rows.Count - 1) / 4) + 1;
                    pagecount = cC * rC;
                }
                #endregion

                #region 複製第1頁的格式到後面幾頁
                winword.Selection.Tables[1].Select();
                winword.Selection.Copy();
                for (int i = 1; i < pagecount; i++)
                {
                    winword.Selection.MoveDown();
                    if (pagecount > 1)
                    {
                        winword.Selection.InsertBreak();
                    }

                    winword.Selection.Paste();
                }
                #endregion

                #region ROW5開始 左側抬頭
                int nextPage = 1;
                tables = table[nextPage];
                if (this.radioFabric.Checked || this.radioAccessory.Checked || this.radioThread.Checked)
                {
                    for (int j = 0; j < cC; j++)
                    {
                        for (int i = 0; i < this.dtPrint_LeftColumn.Rows.Count; i++)
                        {
                            // 根據 DataRow 數量選取 Table, Dot DataRow = 5
                            tables = table[nextPage + (i / 4)];
                            tables.Cell(5 + ((i % 4) * 2), 1).Range.Text = this.dtPrint_LeftColumn.Rows[i]["Article"].ToString().Trim();
                        }

                        nextPage += rC;
                        if (!(nextPage > pagecount))
                        {
                            tables = table[nextPage];
                        }
                    }
                }
                else if (this.radioOther.Checked)
                {
                    string otherTitleSP = this.dtPrint_LeftColumn.AsEnumerable().Select(s => s["ID"].ToString().Trim().Substring(8)).JoinToString(Environment.NewLine);

                    foreach (Word.Table itemTable in table)
                    {
                        itemTable.Cell(3, 1).Range.Text = otherTitleSP;
                    }
                }
                #endregion

                #region [ROW3]欄位名,[ROW4]~[ROW7]對應資料
                nextPage = 1;
                tables = table[nextPage];

                // 抓取當下.exe執行位置路徑 同抓取Excle範本檔路徑
                string path = string.Empty;

                if (this.radioOther.Checked)
                {
                    for (int i = 0; i < this.dtPrint_Content.Rows.Count; i++)
                    {
                        #region 準備欄位名稱
                        this.temp = this.dtPrint_Content.Rows[i]["Refno"].ToString() + Environment.NewLine
                             + this.dtPrint_Content.Rows[i]["DescDetail"].ToString().Trim();

                        // 填入欄位名稱,從第一欄開始填入需要的頁數
                        for (int j = 0; j < rC; j++)
                        {
                            // 根據 DataColumn 選取 Table => 首頁 + (DataColumnIndex / 6 * 每個 FabricPanelCode 會占用的 Table 數) + 目前是編輯第 j 個 Table
                            // 其中 6 代表, 每個 Table 可以存的 FabricPanelCode 數量
                            tables = table[nextPage + (i / 6 * rC) + j];

                            // 有資料時才顯示Type
                            tables.Cell(2, 2 + (i % 6)).Range.Text = row2Type;
                            tables.Cell(3, 2 + (i % 6)).Range.Text = this.temp;

                            // 第一Row塞入圖片
                            Microsoft.Office.Interop.Word.Range rng = tables.Cell(4, 2 + (i % 6)).Range;
                            path = System.IO.Path.Combine(this.FabricPath, this.dtPrint_Content.Rows[i]["BrandID"].ToString().Trim(), this.dtPrint_Content.Rows[i]["Picture"].ToString().Trim());
                            if (this.FileExists(path))
                            {
                                rng = tables.Cell(4, 2 + (i % 6)).Range;
                                rng.InlineShapes.AddPicture(path).ConvertToShape().WrapFormat.Type = Word.WdWrapType.wdWrapInline;
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    for (int i = 0; i < this.dtPrint_Content.Rows.Count; i++)
                    {
                        #region 準備欄位名稱
                        this.temp = this.GetRefNoTitle(this.dtPrint_Content.Rows[i]);

                        // 填入欄位名稱,從第一欄開始填入需要的頁數
                        for (int j = 0; j < rC; j++)
                        {
                            // 根據 DataColumn 選取 Table => 首頁 + (DataColumnIndex / 6 * 每個 FabricPanelCode 會占用的 Table 數) + 目前是編輯第 j 個 Table
                            // 其中 6 代表, 每個 Table 可以存的 FabricPanelCode 數量
                            tables = table[nextPage + (i / 6 * rC) + j];

                            // 有資料時才顯示Type
                            tables.Cell(2, 2 + (i % 6)).Range.Text = row2Type;
                            tables.Cell(3, 2 + (i % 6)).Range.Text = this.temp;

                            // 第一Row塞入圖片
                            Microsoft.Office.Interop.Word.Range rng = tables.Cell(4, 2 + (i % 6)).Range;
                            path = System.IO.Path.Combine(this.FabricPath, this.BrandID, this.dtPrint_Content.Rows[i]["MainPicture"].ToString().Trim());
                            if (this.FileExists(path))
                            {
                                rng.InlineShapes.AddPicture(path).ConvertToShape().WrapFormat.Type = Word.WdWrapType.wdWrapInline;
                            }
                        }
                        #endregion
                        #region 填入Datas
                        for (int k = 0; k < this.dtPrint_LeftColumn.Rows.Count; k++)
                        {
                            // 準備filter字串
                            string whereDetail = string.Empty;

                            if (this.radioFabric.Checked)
                            {
                                whereDetail = $@"   FabricPanelCode = '{this.dtPrint_Content.Rows[i]["FabricPanelCode"]}' and
                                                    FabricCode = '{this.dtPrint_Content.Rows[i]["FabricCode"]}' and
                                                    Refno = '{this.dtPrint_Content.Rows[i]["Refno"]}' and
                                                    Article = '{this.dtPrint_LeftColumn.Rows[k]["Article"]}' and
                                                    IsWritedExcel = 0";
                            }
                            else
                            {
                                whereDetail = $@"   Refno = '{this.dtPrint_Content.Rows[i]["Refno"]}' and
                                                    Article = '{this.dtPrint_LeftColumn.Rows[k]["Article"]}' and
                                                    IsWritedExcel = 0";
                            }

                            DataRow[] colorDetails = this.dtColor.Select(whereDetail);

                            string colorName = string.Empty;

                            if (colorDetails.Length > 0)
                            {
                                DataRow colorInfo = colorDetails[0];
                                colorInfo["IsWritedExcel"] = true;
                                colorName = colorInfo["ColorDesc"].ToString();

                                // 塞入圖片 6 8 10 12
                                int rowPic = 6;
                                rowPic += (k % 4) * 2;
                                Word.Cell cell = tables.Cell(rowPic, 2 + (i % 6));
                                Microsoft.Office.Interop.Word.Range rng = cell.Range;

                                List<string> colorPicPaths = this.GetColorPicPath(colorInfo);
                                List<Bitmap> mergedImages = new List<Bitmap>();
                                if (colorPicPaths.Count > 1)
                                {
                                    mergedImages = Prgs.MergeImages(colorPicPaths);
                                    Bitmap mergedImageVertically = Prgs.MergeBitmapsVertically(mergedImages);

                                    if (mergedImageVertically != null)
                                    {
                                        // 將合併後的圖片插入到 Word 表格中
                                        Clipboard.SetImage(mergedImageVertically);
                                        Thread.Sleep(300);

                                        // 直接將剪貼簿中的圖片插入到 Word 表格中
                                        rng.Paste();

                                        // 調整剪貼簿中的圖片大小以符合儲存格寬度
                                        Word.InlineShape inlineShape = rng.Paragraphs[1].Range.InlineShapes[1]; // 假設圖片是第一個 InlineShape

                                        if (inlineShape.Width > cell.Width)
                                        {
                                            inlineShape.Width = cell.Width - 10;
                                        }
                                    }
                                }
                                else if (File.Exists(colorPicPaths[0]))
                                {
                                    if (this.FileExists(colorPicPaths[0]))
                                    {
                                        rng.InlineShapes.AddPicture(colorPicPaths[0]).ConvertToShape().WrapFormat.Type = Word.WdWrapType.wdWrapInline;
                                    }
                                }
                            }

                            // 根據 DataColumn & DataRow 選取 Table => 首頁 + (DataColumnIndex / 6 * 每個 FabricPanelCode 會占用的 Table 數) + k / Table 可存的 Article 數量
                            // 其中 K 代表, 目前編輯到 FabricPanelCode 的第幾個 Article
                            tables = table[nextPage + (i / 6 * rC) + (k / 4)];

                            // 塞入文字 5 7 9 11
                            int rowStr = 5;
                            rowStr += (k % 4) * 2;
                            tables.Cell(rowStr, 2 + (i % 6)).Range.Text = colorName;
                        }
                        #endregion
                    }
                }
                #endregion

                #region 整理欄位格式
                if (!this.radioOther.Checked)
                {
                    for (int p = 1; p <= pagecount; p++)
                    {
                        tables = table[p];
                        for (int r = 1; r <= tables.Rows.Count; r++)
                        {
                            for (int c = 1; c <= tables.Columns.Count; c++)
                            {
                                // Article合併 5,7,9,11
                                if (r >= 3 && (r % 2) == 1 && c == 1)
                                {
                                    // 合併儲存格
                                    tables.Cell(r, 1).Merge(tables.Cell(r + 1, 1));

                                    // 上下 水平置中
                                    tables.Cell(r, 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                                    tables.Cell(r, 1).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                }

                                // 去除圖片和文字中間分隔線
                                if (r >= 4 && (r % 2) == 0 && c > 1)
                                {
                                    tables.Cell(r, c).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderTop].Visible = false;
                                }
                            }
                        }
                    }
                }
                #endregion

                winword.Visible = true;

               // winword.Quit(ref missing, ref missing, ref missing);     //close word application
                return Ict.Result.True;
            }
            catch (Exception ex)
            {
                if (winword != null)
                {
                    winword.Quit();
                }

                MyUtility.Msg.WarningBox(ex.ToString(), "Export word error.");
                return false;
            }
            finally
            {
                Marshal.ReleaseComObject(winword);
                GC.Collect();
                GC.WaitForPendingFinalizers();

                this.HideWaitMessage();

                // Marshal.FinalReleaseComObject(winword);
            }
        }

        private string GetRefNoTitle(DataRow drContent)
        {
            string resultTitle = string.Empty;
            if (this.radioFabric.Checked)
            {
                resultTitle = "Pattern Panel:" + drContent["FabricPanelCode"].ToString() + Environment.NewLine
                             + "Fabric Code:" + drContent["FabricCode"].ToString().Trim() + Environment.NewLine
                             + drContent["Refno"].ToString().Trim() + Environment.NewLine
                             + drContent["Description"].ToString().Trim();
            }
            else
            {
                resultTitle = drContent["Refno"].ToString() + Environment.NewLine + drContent["Description"].ToString().Trim();
            }

            return resultTitle;
        }

        private List<string> GetColorPicPath(DataRow drContent)
        {
            var resultMutiColor = this.dtMutiColor.AsEnumerable().Where(s => s["ColorID"].ToString() == drContent["ColorID"].ToString());

            if (!resultMutiColor.Any())
            {
                return new List<string>() { Path.Combine(this.ColorPath, drContent["Picture"].ToString()) };
            }

            return resultMutiColor.Select(s => Path.Combine(this.ColorPath, s["Picture"].ToString().Trim())).ToList();
        }

        private bool FileExists(string path)
        {
            // 檔案是否存在
            if (!System.IO.File.Exists(path))
            {
                return false;
            }

            return true;
        }
    }
}
