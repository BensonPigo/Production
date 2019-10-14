using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci;
using Sci.Data;
using Ict;
using Ict.Win;
using Sci.Win;
using System.IO;
using Word = Microsoft.Office.Interop.Word;
using System.Linq;
using System.Runtime.InteropServices;


namespace Sci.Production.Warehouse
{
    public partial class P01_TrimCardPrint : Sci.Win.Tems.PrintForm
    {
        DataTable dtPrint_Content, dtPrint_LeftColumn, dtColor, dtColor2;
        DataRow rowColor;
        string sql, temp, orderID, StyleID, SeasonID, FactoryID, BrandID, POID;
        List<string> ListColor = new List<string>();


        public P01_TrimCardPrint(string _orderID, string _StyleID, string _SeasonID, string _FactoryID, string _BrandID, string _POID)
        {
            InitializeComponent();
            orderID = _orderID;
            StyleID = _StyleID;
            SeasonID = _SeasonID;
            FactoryID = _FactoryID;
            BrandID = _BrandID;
            POID = _POID;
        }

        //欄位檢核
        protected override bool ValidateInput()  
        {
            if (!radioFabric.Checked && !radioAccessory.Checked && !radioOther.Checked && !radioThread.Checked)
            {
                ShowErr("Please select an item !!");
                return false;
            }
            return true;            
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult result = Result.True;
            if (dtPrint_Content != null) dtPrint_Content.Rows.Clear();

            #region SQL
            if (radioFabric.Checked)
            {
                #region FABRIC
                sql = string.Format(@"

select  A.PatternPanel 
		, A.FabricCode 
		, B.Refno 
		, C.Description 
		, A.FabricPanelCode
from Orders o WITH (NOLOCK) 
inner join Orders allOrder With (NoLock) on o.Poid = allOrder.Poid
inner join Order_FabricCode A WITH (NOLOCK) on allOrder.ID = a.ID
left join Order_BOF B WITH (NOLOCK) on B.Id=A.Id and B.FabricCode=A.FabricCode
left join Fabric C WITH (NOLOCK) on C.SCIRefno=B.SCIRefno
where o.ID = '{0}'
order by FabricCode", orderID);
                result = DBProxy.Current.Select(null, sql, out dtPrint_Content);
                if (!result) return result;

                sql = string.Format(@"
select distinct Article 
from Orders o WITH (NOLOCK) 
inner join Orders allOrder With (NoLock) on o.Poid = allOrder.Poid
inner join Order_ColorCombo occ WITH (NOLOCK) on allOrder.id = occ.ID
where o.Id='{0}' 
	  and FabricPanelCode in (select a.FabricPanelCode 
	  						  from Orders o WITH (NOLOCK) 
                              inner join Orders allOrder With (NoLock) on o.Poid = allOrder.Poid
                              inner join Order_FabricCode A WITH (NOLOCK) on allOrder.ID = a.ID
	  						  where o.ID = '{0}')", orderID);
                result = DBProxy.Current.Select(null, sql, out dtPrint_LeftColumn);
                if (!result) return result;

                sql = string.Format(@"
select a.ColorID 
	   , B.Name 
	   , a.FabricPanelCode 
	   , a.Article
from Orders o WITH (NOLOCK) 
inner join Orders allOrder With (NoLock) on o.Poid = allOrder.Poid
inner join Order_ColorCombo A WITH (NOLOCK) on allOrder.ID = A.ID
left join Color B WITH (NOLOCK) on B.BrandId = '{0}' 
								   and B.ID = A.ColorID
where o.Id='{1}'
	  and FabricPanelCode in (select A.FabricPanelCode 
							  from Orders o WITH (NOLOCK) 
                              inner join Orders allOrder With (NoLock) on o.Poid = allOrder.Poid
                              inner join Order_FabricCode A WITH (NOLOCK) on allOrder.ID = a.ID
							  left join Order_BOF B WITH (NOLOCK) on B.Id = A.Id 
							  										 and B.FabricCode=A.FabricCode
							  left join Fabric C WITH (NOLOCK) on C.SCIRefno = B.SCIRefno
							  where o.ID = '{1}')"
                                , BrandID, orderID);
                result = DBProxy.Current.Select(null, sql, out dtColor);
                if (!result) return result;

                #endregion
            }
            else if (radioAccessory.Checked)
            {
                #region ACCESSORY
                sql = string.Format(@"
select A.Refno, B.Description 
from Order_BOA A WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on a.id = o.poid 
inner join Orders allOrder WITH (NOLOCK) on o.poid = allOrder.poid 
inner join Order_Article oa With (NoLock) on allOrder.id = oa.id
inner join Order_ColorCombo occ With(NoLock) on a.id = occ.Id
												and a.FabricPanelCode = occ.FabricPanelCode
                                                and oa.Article = occ.Article
left join Fabric B WITH (NOLOCK) on B.SCIRefno=A.SCIRefno
where o.id = '{0}' 
	  and oa.Article <> '' 
	  and occ.ColorId<>''
	  and b.MtlTypeID in (select id from MtlType WITH (NOLOCK) where IsTrimcardOther=0)
group by A.Refno, B.Description ", orderID);
                result = DBProxy.Current.Select(null, sql, out dtPrint_Content);
                if (!result) return result;

                sql = string.Format(@"
select distinct oa.article 
from Order_BOA A WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on a.id = o.poid 
inner join Orders allOrder WITH (NOLOCK) on o.poid = allOrder.poid 
inner join Order_Article oa With (NoLock) on allOrder.id = oa.id
inner join Order_ColorCombo occ With(NoLock) on a.id = occ.Id
												and a.FabricPanelCode = occ.FabricPanelCode
                                                and oa.Article = occ.Article
left join Fabric B WITH (NOLOCK) on B.SCIRefno=A.SCIRefno
where o.id = '{0}' 
	  and oa.Article <> '' 
	  and occ.ColorId<>''
order by oa.Article", orderID);
                result = DBProxy.Current.Select(null, sql, out dtPrint_LeftColumn);
                if (!result) return result;

                sql = string.Format(@"
select distinct A.Refno
	   , oa.article 
	   , ColorId = dbo.GetColorMultipleID('{1}', occ.ColorID)
	   , Name = stuff((select '/' + x.Name
					   from (
					        select mc.Name, m.Seqno
					        from dbo.Color as c
					        inner join dbo.Color_multiple as m on m.ID = c.ID 
				  				  						     and m.BrandID = c.BrandId
					        inner join dbo.Color mc on m.ColorID = mc.ID 
                                                       and m.BrandID = mc.BrandId
					        where c.BrandId = '{1}'
						          and c.ID = occ.ColorID 
					   ) x			   
					   order by Seqno
					   for xml path('')) 
			          , 1, 1, '')
from Order_BOA A WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on a.id = o.poid 
inner join Orders allOrder WITH (NOLOCK) on o.poid = allOrder.poid 
inner join Order_Article oa With (NoLock) on allOrder.id = oa.id
inner join Order_ColorCombo occ With(NoLock) on a.id = occ.Id
												and a.FabricPanelCode = occ.FabricPanelCode
                                                and oa.Article = occ.Article
left join Fabric B WITH (NOLOCK) on B.SCIRefno=A.SCIRefno
where o.id = '{0}' 
	  and oa.Article <> '' 
	  and ColorId <> ''"
                                    , orderID, BrandID);
                result = DBProxy.Current.Select(null, sql, out dtColor);
                if (!result) return result;

                sql = string.Format(@"select ID , Name from Color WITH (NOLOCK) where BrandId='{0}' and JUNK=0", BrandID);
                result = DBProxy.Current.Select(null, sql, out dtColor2);
                if (!result) return result;
                #endregion
            }
            else if (radioOther.Checked)
            {
                #region OTHER
                //架構要調，先HOLD住
                sql = string.Format(@"
select distinct ob.Refno
	   , B.DescDetail
from Orders o WITH (NOLOCK) 
inner join Orders allOrder With (NoLock) on o.Poid = allOrder.Poid
inner join Order_BOA ob WITH (NOLOCK) on allOrder.ID = ob.ID
left join Fabric B WITH (NOLOCK) on B.SCIRefno = ob.SCIRefno
where o.Id='{0}'
	  and b.MtlTypeID in (select id from MtlType WITH (NOLOCK) where IsTrimcardOther=1)
	  and not ob.SuppID = 'fty' 
	  and not ob.SuppID = 'fty-c'", orderID);
                result = DBProxy.Current.Select(null, sql, out dtPrint_Content);
                if (!result) return result;

                sql = string.Format(@"select distinct orders.ID 
                                    from orders WITH (NOLOCK) 
                                    where orders.POID='{0}'", POID);
                result = DBProxy.Current.Select(null, sql, out dtPrint_LeftColumn);
                if (!result) return result;
                #endregion
            }
            else if (radioThread.Checked)
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
where o.iD = '{POID}' and article<>''  
";

                result = DBProxy.Current.Select(null, sqlCmd_1, out dt_CheckOld);
                if (!result) return result;


                // 2.區分新舊單，Article來源不同
                if (dt_CheckOld.Rows.Count==0)
                {
                    isNewOrder = true;
                    // 新單
                    //Article要從Style_ThreadColorCombo抓取

                    #region 取得新單的Article

                    sql = $@"

--1.從訂單串回物料
select distinct StyleID,BrandID,POID,FtyGroup 
into #tmpOrder
from orders where id like '{orderID}'


Select distinct [ID] = PO.POID
, std.SuppId --Mapping PO_Supp
, std.SCIRefNo, std.ColorID, std.Article --Mapping PO_Supp_Detail
into #ArticleForThread_Detail
From #tmpOrder PO
Inner Join dbo.Orders as o On o.ID = po.POID 
Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
Inner Join dbo.Style_ThreadColorCombo as st On st.StyleUkey = s.Ukey
Inner Join dbo.Style_ThreadColorCombo_Detail as std On std.Style_ThreadColorComboUkey = st.Ukey



select distinct
a.ID,SuppId,SCIRefNo,ColorID,
[Article] = Stuff((select distinct concat( ',',Article)   
								from #ArticleForThread_Detail 
								where	ID		   = a.ID		and
										SuppId	   = a.SuppId	and
										SCIRefNo   = a.SCIRefNo	and
										ColorID	   = a.ColorID	
								FOR XML PATH('')),1,1,'') 
into #ArticleForThread
from #ArticleForThread_Detail a


--2.得到跟WH P03 一樣的對應方式
SELECT DISTINCT [OrderIdList] = stuff((select concat('/',tmp.OrderID) 
		                                    from (
			                                    select orderID from po_supp_Detail_orderList e
			                                    where e.ID = a.ID and e.SEQ1 =a.SEQ1 and e.SEQ2 = a.SEQ2 AND orderID = '{orderID}'
		                                    ) tmp for xml path(''))
                                    ,1,1,'')
									,[Article] = aft.Article
INTO #tmpOrder_Article
from #tmpOrder as orders WITH (NOLOCK) 
inner join PO_Supp_Detail a WITH (NOLOCK) on a.id = orders.poid
left join dbo.MDivisionPoDetail m WITH (NOLOCK) on  m.POID = a.ID and m.seq1 = a.SEQ1 and m.Seq2 = a.Seq2
left join po_supp b WITH (NOLOCK) on a.id = b.id and a.SEQ1 = b.SEQ1
left join #ArticleForThread aft on	aft.ID = m.POID		and
									aft.SuppId	   = b.SuppId	and
									aft.SCIRefNo   = a.SCIRefNo	and
									aft.ColorID	   = a.ColorID	and
									a.SEQ1 like 'T%' 
WHERE  stuff((select concat('/',tmp.OrderID) 
		                                    from (
			                                    select orderID from po_supp_Detail_orderList e
			                                    where e.ID = a.ID and e.SEQ1 =a.SEQ1 and e.SEQ2 = a.SEQ2 AND orderID = '{orderID}'
		                                    ) tmp for xml path(''))
                                    ,1,1,'') 
IS NOT NULL 
AND aft.Article IS NOT NULL

--3.再回去Style_ThreadColorCombo串出需要的資料
 select DISTINCT 
 B.Article
 from Orders o WITH (NOLOCK) 
 inner join Orders allOrder With (NoLock) on o.Poid = allOrder.Poid
 inner join Style_ThreadColorCombo A WITH (NOLOCK) on allOrder.StyleUkey = a.StyleUkey
 left join Style_ThreadColorCombo_Detail B WITH (NOLOCK) on B.Style_ThreadColorComboUkey = A.Ukey
 where o.ID = '{POID}'
 AND (
     EXISTS (SELECT 1 FROM #tmpOrder_Article WHERE [OrderIdList] = allOrder.ID AND Article = B.Article) 
     OR
     (SELECT COUNT(1) FROM #tmpOrder_Article) = 0
 )
--Order List如果是空，代表所有訂單都有用到這個物料



DROP TABLE #tmpOrder,#ArticleForThread_Detail,#ArticleForThread , #tmpOrder_Article
";

                    #endregion
                    result = DBProxy.Current.Select(null, sql, out dtPrint_LeftColumn);
                    if (!result) return result;
                }

                // 3.找出對應的Color，新單的話，由於是透過Order的StyleUkey去串出Article，母單子單都是相同StyleUkey
                // 因此參考 WH P03 OrderList欄位，找出這個繡線被用在哪一些訂單
                if (isNewOrder)
                {
                    #region 取得Article和對應的Color，只挑選出order List有的Article

                    sql = $@"

--1.從訂單串回物料
select distinct StyleID,BrandID,POID,FtyGroup 
into #tmpOrder
from orders where id like '{orderID}'


Select distinct [ID] = PO.POID
, std.SuppId --Mapping PO_Supp
, std.SCIRefNo, std.ColorID, std.Article --Mapping PO_Supp_Detail
into #ArticleForThread_Detail
From #tmpOrder PO
Inner Join dbo.Orders as o On o.ID = po.POID 
Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
Inner Join dbo.Style_ThreadColorCombo as st On st.StyleUkey = s.Ukey
Inner Join dbo.Style_ThreadColorCombo_Detail as std On std.Style_ThreadColorComboUkey = st.Ukey



select distinct
a.ID,SuppId,SCIRefNo,ColorID,
[Article] = Stuff((select distinct concat( ',',Article)   
								from #ArticleForThread_Detail 
								where	ID		   = a.ID		and
										SuppId	   = a.SuppId	and
										SCIRefNo   = a.SCIRefNo	and
										ColorID	   = a.ColorID	
								FOR XML PATH('')),1,1,'') 
into #ArticleForThread
from #ArticleForThread_Detail a


--2.得到跟WH P03 一樣的對應方式
SELECT DISTINCT [OrderIdList] = stuff((select concat('/',tmp.OrderID) 
		                                    from (
			                                    select orderID from po_supp_Detail_orderList e
			                                    where e.ID = a.ID and e.SEQ1 =a.SEQ1 and e.SEQ2 = a.SEQ2 AND orderID = '{orderID}'
		                                    ) tmp for xml path(''))
                                    ,1,1,'')
									,[Article] = aft.Article
INTO #tmpOrder_Article
from #tmpOrder as orders WITH (NOLOCK) 
inner join PO_Supp_Detail a WITH (NOLOCK) on a.id = orders.poid
left join dbo.MDivisionPoDetail m WITH (NOLOCK) on  m.POID = a.ID and m.seq1 = a.SEQ1 and m.Seq2 = a.Seq2
left join po_supp b WITH (NOLOCK) on a.id = b.id and a.SEQ1 = b.SEQ1
left join #ArticleForThread aft on	aft.ID = m.POID		and
									aft.SuppId	   = b.SuppId	and
									aft.SCIRefNo   = a.SCIRefNo	and
									aft.ColorID	   = a.ColorID	and
									a.SEQ1 like 'T%' 
WHERE  stuff((select concat('/',tmp.OrderID) 
		                                    from (
			                                    select orderID from po_supp_Detail_orderList e
			                                    where e.ID = a.ID and e.SEQ1 =a.SEQ1 and e.SEQ2 = a.SEQ2 AND orderID = '{orderID}'
		                                    ) tmp for xml path(''))
                                    ,1,1,'') 
IS NOT NULL 
AND aft.Article IS NOT NULL

--3.再回去Style_ThreadColorCombo串出需要的資料
 select DISTINCT 
 B.Article
 , B.ColorID AS ThreadColorID
 from Orders o WITH (NOLOCK) 
 inner join Orders allOrder With (NoLock) on o.Poid = allOrder.Poid
 inner join Style_ThreadColorCombo A WITH (NOLOCK) on allOrder.StyleUkey = a.StyleUkey
 left join Style_ThreadColorCombo_Detail B WITH (NOLOCK) on B.Style_ThreadColorComboUkey = A.Ukey
 where o.ID = '{POID}'
 AND (
     EXISTS (SELECT 1 FROM #tmpOrder_Article WHERE [OrderIdList] = allOrder.ID AND Article = B.Article) 
     OR
     (SELECT COUNT(1) FROM #tmpOrder_Article) = 0
 )
--Order List如果是空，代表所有訂單都有用到這個物料



DROP TABLE #tmpOrder,#ArticleForThread_Detail,#ArticleForThread , #tmpOrder_Article
";

                    #endregion
                    result = DBProxy.Current.Select(null, sql, out dtPrint_Content);
                    if (!result) return result;
                }
                else
                {

                    #region 重新找一次Article。Style_ThreadColorCombo的也要納入
                    sql = $@"

SELECT Article FROM
(
	select DISTINCT
	  B.Article
	, A.ThreadColorID
	from Orders o WITH (NOLOCK) 
	inner join Orders allOrder With (NoLock) on o.Poid = allOrder.Poid
	inner join ThreadRequisition_Detail A WITH (NOLOCK) on allOrder.id = a.OrderID
	left join ThreadRequisition_Detail_Cons B WITH (NOLOCK) on B.ThreadRequisition_DetailUkey = A.Ukey
	where o.ID = '{POID}' and article<>'' AND allOrder.ID='{orderID}'

	UNION  --加上當地採購
	SELECT DISTINCT 
	[Artuicle] = IIF(BOA_Article.Article IS NULL 
					,Order_Article.Article
					,BOA_Article.Article 
					)				

	,[ThreadColorID]=PSD.SuppColor 
	FROM PO_Supp_Detail PSD
	INNER join Fabric f on f.SCIRefno = PSD.SCIRefno
	LEFT JOIN PO_Supp_Detail_OrderList pd ON PSD.ID = pd.ID AND PSD.SEQ1= pd.SEQ1 AND PSD.SEQ2= pd.SEQ2
	LEFT JOIN Order_BOA boa ON boa.id =psd.ID and boa.SCIRefno = psd.SCIRefno and boa.seq=psd.SEQ1
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
	WHERE PSD.ID ='{POID}' and f.MtltypeId like '%thread%' AND PSD.Junk=0 AND boa.Ukey IS NOT NULL AND pd.OrderID='{orderID}'
)A
WHERE ThreadColorID <> '' AND ThreadColorID IS NOT NULL

UNION

 select DISTINCT 
 B.Article
 from Orders o WITH (NOLOCK) 
 inner join Orders allOrder With (NoLock) on o.Poid = allOrder.Poid
 inner join Style_ThreadColorCombo A WITH (NOLOCK) on allOrder.StyleUkey = a.StyleUkey
 left join Style_ThreadColorCombo_Detail B WITH (NOLOCK) on B.Style_ThreadColorComboUkey = A.Ukey
 where o.ID = '{POID}' 
";

                    result = DBProxy.Current.Select(null, sql, out dtPrint_LeftColumn);

                    #endregion

                    #region 找出對應的Article、Color，由於舊單的繡線物料不會直接顯示在Seq，因此無法按照新單的做法

                    sql = $@"

SELECT * FROM
(
	select 
	  B.Article
	, A.ThreadColorID
	from Orders o WITH (NOLOCK) 
	inner join Orders allOrder With (NoLock) on o.Poid = allOrder.Poid
	inner join ThreadRequisition_Detail A WITH (NOLOCK) on allOrder.id = a.OrderID
	left join ThreadRequisition_Detail_Cons B WITH (NOLOCK) on B.ThreadRequisition_DetailUkey = A.Ukey
	where o.ID = '{POID}' and article<>'' AND allOrder.ID= '{orderID}'
	group by article, threadcolorid

	UNION  --加上當地採購
	SELECT DISTINCT 
	[Artuicle] = IIF(BOA_Article.Article IS NULL 
					,Order_Article.Article
					,BOA_Article.Article 
					)				

	,[ThreadColorID]=PSD.SuppColor 
	FROM PO_Supp_Detail PSD
	INNER join Fabric f on f.SCIRefno = PSD.SCIRefno
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
	WHERE PSD.ID ='{POID}' and f.MtltypeId like '%thread%' AND PSD.Junk=0 AND boa.Ukey IS NOT NULL  AND pd.OrderID='{orderID}'
)A
WHERE ThreadColorID <> '' AND ThreadColorID IS NOT NULL


UNION 

 select --o.ID,
 B.Article
 , B.ColorID AS ThreadColorID
 from Orders o WITH (NOLOCK) 
 inner join Orders allOrder With (NoLock) on o.Poid = allOrder.Poid
 inner join Style_ThreadColorCombo A WITH (NOLOCK) on allOrder.StyleUkey = a.StyleUkey
 left join Style_ThreadColorCombo_Detail B WITH (NOLOCK) on B.Style_ThreadColorComboUkey = A.Ukey
 where o.ID = '{POID}' --and article<>''
 group by B.Article, B.ColorID-- ,o.ID


ORDER BY ThreadColorID ASC
";
                    #endregion
                    result = DBProxy.Current.Select(null, sql, out dtPrint_Content);
                    if (!result) return result;
                }
                #endregion
            }
            #endregion
            return Result.True;            
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (dtPrint_Content.Rows.Count == 0)
            {
                MessageBox.Show("Data not found!!");
                return false;
            }
            SetCount(dtPrint_Content.Rows.Count);
            DualResult result = Result.True;
            //decimal intRowsCount = dtPrint.Rows.Count;
            //int page = Convert.ToInt16(Math.Ceiling(intRowsCount / 4));
            Object temfile; ;

            if (radioOther.Checked)
                temfile = Sci.Env.Cfg.XltPathDir + "\\Warehouse-P01.TrimCardPrint_B.dotx";
            else
                temfile = Sci.Env.Cfg.XltPathDir + "\\Warehouse-P01.TrimCardPrint_A.dotx";
            this.ShowWaitMessage(temfile.ToString());
            Microsoft.Office.Interop.Word._Application winword = new Microsoft.Office.Interop.Word.Application();
            winword.FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip;
            //Set status for word application is to be visible or not.
            winword.Visible = false;

            //Create a new document
            Microsoft.Office.Interop.Word._Document document = winword.Documents.Add(ref temfile);

            Word.Table tables = null;

            try
            {
                int intThreadMaxLength = 0;
                document.Activate();
                Word.Tables table = document.Tables;

                //retreive the first table of the document
                tables = table[1];

                #region ROW1
                 if (radioOther.Checked)
                    if(dtPrint_LeftColumn.Rows.Count > 1)
                        tables.Cell(1, 1).Range.Text = string.Format("LABELLING & PACKAGING (SP# {0} - {1})", orderID, dtPrint_LeftColumn.Rows[dtPrint_LeftColumn.Rows.Count - 1]["id"].ToString().Substring(8));
                    else
                        tables.Cell(1, 1).Range.Text = string.Format("LABELLING & PACKAGING (SP# {0})", orderID);
                else
                    tables.Cell(1, 1).Range.Text = string.Format("SP#{0}     ST:{1}     SEASON:{2}     FTY:{3}", orderID, StyleID, SeasonID, FactoryID);
                #endregion

                #region ROW2
                 string Row2Type = "";
                 //if (radioFabric.Checked)
                 //{
                 //    for (int i = 2; i <= tables.Columns.Count; i++) tables.Cell(2, i).Range.Text = "FABRIC";
                 //}
                 //else if (radioAccessory.Checked)
                 //{
                 //    for (int i = 2; i <= tables.Columns.Count; i++) tables.Cell(2, i).Range.Text = "ACCESSORY";
                 //}
                 //else if (radioOther.Checked)
                 //{
                 //    for (int i = 2; i <= tables.Columns.Count; i++) tables.Cell(2, i).Range.Text = "OTHER";
                 //}
                 //else if (radioThread.Checked)
                 //{
                 //    for (int i = 2; i <= tables.Columns.Count; i++) tables.Cell(2, i).Range.Text = "Thread";
                 //}
                 if (radioFabric.Checked)
                 {
                     Row2Type = "FABRIC";
                 }
                 else if (radioAccessory.Checked)
                 {
                     Row2Type = "ACCESSORY";
                 }
                 else if (radioOther.Checked)
                 {
                     Row2Type = "OTHER";
                 }
                 else if (radioThread.Checked)
                 {
                     Row2Type = "Thread";
                 }
                #endregion

                #region 計算共要幾頁
                int pagecount;
                int CC, rC;
                if (radioOther.Checked)
                {
                    CC = (dtPrint_Content.Rows.Count - 1) / 6 + 1;
                    rC = (dtPrint_LeftColumn.Rows.Count - 1) / 7 + 1;
                    pagecount = CC * rC;
                }
                else if (radioThread.Checked)
                {
                    DataTable dtMaxLength;
                    string strDtPrintMaxLengthSQL = @"
select Max(ColorCount)
from (
    select ColorCount = count(1)
    from #tmp
    group by Article
) tmp";
                    DualResult resultMaxLength = MyUtility.Tool.ProcessWithDatatable(dtPrint_Content, null, strDtPrintMaxLengthSQL, out dtMaxLength);
                    if (resultMaxLength == false)
                    {
                        MyUtility.Msg.WarningBox(resultMaxLength.Description);
                        return false;
                    }
                    else
                    {
                        intThreadMaxLength = Convert.ToInt32(dtMaxLength.Rows[0][0]);
                        CC = (intThreadMaxLength - 1) / 6 + 1;
                        rC = (dtPrint_LeftColumn.Rows.Count - 1) / 4 + 1;
                        pagecount = CC * rC;
                    }
                }
                else
                {
                    CC = (dtPrint_Content.Rows.Count - 1) / 6 + 1;
                    rC = (dtPrint_LeftColumn.Rows.Count - 1) / 4 + 1;
                    pagecount = CC * rC;
                }
                #endregion

                #region 複製第1頁的格式到後面幾頁
                winword.Selection.Tables[1].Select();
                winword.Selection.Copy();
                for (int i = 1; i < pagecount; i++)
                {
                    winword.Selection.MoveDown();
                    if (pagecount > 1)
                        winword.Selection.InsertBreak();
                    winword.Selection.Paste();
                }
                #endregion

                #region ROW4開始 左側抬頭
                int nextPage = 1;
                tables = table[nextPage];
                if (radioFabric.Checked || radioAccessory.Checked || radioThread.Checked)
                {
                    for (int j = 0; j < CC; j++)
                    {
                        for (int i = 0; i < dtPrint_LeftColumn.Rows.Count; i++)
                        {
                            //根據 DataRow 數量選取 Table, Dot DataRow = 4
                            tables = table[nextPage + (i / 4)];
                            tables.Cell(4 + (i % 4), 1).Range.Text = dtPrint_LeftColumn.Rows[i]["Article"].ToString().Trim();
                            //tables.Cell((i + 4 + 3 * (i / 4)) + rC * j * 7, 1).Range.Text = dtPrint2.Rows[i]["Article"].ToString().Trim();
                        }
                        nextPage += rC;
                        if (!(nextPage > pagecount))
                            tables = table[nextPage];
                    }
                }
                else if (radioOther.Checked)
                {
                    for (int j = 0; j < CC; j++)
                    {
                        for (int i = 0; i < dtPrint_LeftColumn.Rows.Count; i++)
                        {
                            //根據 DataRow 數量選取 Table, Dot DataRow = 7
                            tables = table[nextPage + (i / 7)];
                            tables.Cell(4 + (i % 7), 1).Range.Text = dtPrint_LeftColumn.Rows[i]["ID"].ToString().Trim().Substring(8); ;
                            //tables.Cell(i + 4 + (3 * (i / 7)) + rC * j * 10, 1).Range.Text = dtPrint2.Rows[i]["ID"].ToString().Trim().Substring(8);
                        }
                        nextPage += rC;
                        if (!(nextPage > pagecount))
                            tables = table[nextPage];
                    }
                }
                #endregion

                #region [ROW3]欄位名,[ROW4]~[ROW7]對應資料
                nextPage = 1;
                tables = table[nextPage];

                if (radioFabric.Checked)
                {
                    for (int i = 0; i < dtPrint_Content.Rows.Count; i++)
                    {
                        #region 準備欄位名稱
                        temp = "Pattern Panel:" + dtPrint_Content.Rows[i]["FabricPanelCode"].ToString() + Environment.NewLine
                             + "Fabric Code:" + dtPrint_Content.Rows[i]["FabricCode"].ToString().Trim() + Environment.NewLine
                             + dtPrint_Content.Rows[i]["Refno"].ToString().Trim() + Environment.NewLine
                             + dtPrint_Content.Rows[i]["Description"].ToString().Trim();
                        //填入欄位名稱,從第一欄開始填入需要的頁數
                        for (int j = 0; j < rC; j++)
                        {
                            //根據 DataColumn 選取 Table => 首頁 + (DataColumnIndex / 6 * 每個 FabricPanelCode 會占用的 Table 數) + 目前是編輯第 j 個 Table
                            //其中 6 代表, 每個 Table 可以存的 FabricPanelCode 數量
                            tables = table[nextPage + (i / 6 * rC) + j];

                            //有資料時才顯示Type
                            tables.Cell(2, (2 + (i % 6))).Range.Text = Row2Type;
                            tables.Cell(3, (2 + (i % 6))).Range.Text = temp;
                        }
                        #endregion

                        #region 填入Datas
                        for (int k = 0; k < dtPrint_LeftColumn.Rows.Count; k++)
                        {
                            //準備filter字串
                            sql = string.Format(@"FabricPanelCode='{0}' and Article='{1}'"
                                , dtPrint_Content.Rows[i]["FabricPanelCode"].ToString().Trim(), dtPrint_LeftColumn.Rows[k]["Article"].ToString().Trim());
                            if (dtColor.Select(sql).Length > 0)
                            {//找出對應的Datas組起來
                                rowColor = dtColor.Select(sql)[0];
                                temp = rowColor["Name"].ToString().Trim() + Environment.NewLine + rowColor["ColorID"].ToString().Trim();
                            }
                            else
                            {
                                temp = "";
                            }
                            //根據 DataColumn & DataRow 選取 Table => 首頁 + (DataColumnIndex / 6 * 每個 FabricPanelCode 會占用的 Table 數) + k / Table 可存的 Article 數量
                            //其中 K 代表, 目前編輯到 FabricPanelCode 的第幾個 Article
                            tables = table[nextPage + (i / 6 * rC) + (k / 4)];

                            //填入字串
                            tables.Cell((4 + (k % 4)), (2 + (i % 6))).Range.Text = temp;
                        }
                        #endregion
                        //調整頁數
                    }
                }
                else if (radioAccessory.Checked) 
                {
                    for (int i = 0; i < dtPrint_Content.Rows.Count; i++)
                    {
                        #region 準備欄位名稱
                        temp = dtPrint_Content.Rows[i]["Refno"].ToString() + Environment.NewLine
                             + dtPrint_Content.Rows[i]["Description"].ToString().Trim();
                        //填入欄位名稱,從第一欄開始填入需要的頁數
                        for (int j = 0; j < rC; j++)
                        {
                            //根據 DataColumn 選取 Table => 首頁 + (DataColumnIndex / 6 * 每個 FabricPanelCode 會占用的 Table 數) + 目前是編輯第 j 個 Table
                            //其中 6 代表, 每個 Table 可以存的 FabricPanelCode 數量
                            tables = table[nextPage + (i / 6 * rC) + j];

                            //有資料時才顯示Type
                            tables.Cell(2, (2 + (i % 6))).Range.Text = Row2Type;
                            tables.Cell(3, (2 + (i % 6))).Range.Text = temp;
                        }
                        #endregion
                        #region 填入Datas
                        for (int k = 0; k < dtPrint_LeftColumn.Rows.Count; k++)
                        {
                            //準備filter字串
                            sql = string.Format(@"Refno='{0}' and Article='{1}'"
                                , dtPrint_Content.Rows[i]["Refno"].ToString().Trim(), dtPrint_LeftColumn.Rows[k]["Article"].ToString().Trim());
                            if (dtColor.Select(sql).Length > 0)
                            {
                                rowColor = dtColor.Select(sql)[0];
                                temp = rowColor["Name"].ToString().Trim() + Environment.NewLine + rowColor["ColorID"].ToString().Trim();
                            }
                            else
                            {
                                temp = "";
                            }

                            //根據 DataColumn & DataRow 選取 Table => 首頁 + (DataColumnIndex / 6 * 每個 FabricPanelCode 會占用的 Table 數) + k / Table 可存的 Article 數量
                            //其中 K 代表, 目前編輯到 FabricPanelCode 的第幾個 Article
                            tables = table[nextPage + (i / 6 * rC) + (k / 4)];

                            //填入字串
                            tables.Cell((4 + (k % 4)), (2 + (i % 6))).Range.Text = temp;
                        }
                        #endregion
                    }
                }
                else if (radioOther.Checked)
                {
                    for (int i = 0; i < dtPrint_Content.Rows.Count; i++)
                    {
                        #region 準備欄位名稱
                        temp = dtPrint_Content.Rows[i]["Refno"].ToString() + Environment.NewLine
                             + dtPrint_Content.Rows[i]["DescDetail"].ToString().Trim();
                        //填入欄位名稱,從第一欄開始填入需要的頁數
                        for (int j = 0; j < rC; j++)
                        {
                            //根據 DataColumn 選取 Table => 首頁 + (DataColumnIndex / 6 * 每個 FabricPanelCode 會占用的 Table 數) + 目前是編輯第 j 個 Table
                            //其中 6 代表, 每個 Table 可以存的 FabricPanelCode 數量
                            tables = table[nextPage + (i / 6 * rC) + j];

                            //有資料時才顯示Type
                            tables.Cell(2, (2 + (i % 6))).Range.Text = Row2Type;
                            tables.Cell(3, (2 + (i % 6))).Range.Text = temp;
                        }                       
                        #endregion
                    }
                }               
                else if (radioThread.Checked)
                {
                    for (int i = 0; i < intThreadMaxLength; i++)
                    {
                        for (int j = 0; j < rC; j++)
                        {
                            //根據 DataColumn 選取 Table => 首頁 + (DataColumnIndex / 6 * 每個 FabricPanelCode 會占用的 Table 數) + 目前是編輯第 j 個 Table
                            //其中 6 代表, 每個 Table 可以存的 FabricPanelCode 數量
                            tables = table[nextPage + (i / 6 * rC) + j];

                            //有資料時才顯示Type
                            tables.Cell(2, (2 + (i % 6))).Range.Text = Row2Type;
                        }
                    }
                    #region 填入Datas
                    for (int k = 0; k < dtPrint_LeftColumn.Rows.Count; k++)
                    {
                        //準備filter字串
                        sql = string.Format(@"Article='{0}'"
                            ,dtPrint_LeftColumn.Rows[k]["Article"].ToString().Trim());
                        if (dtPrint_Content.Select(sql).Length > 0)
                        {
                            DataRow[] rowColorA = dtPrint_Content.Select(sql);
                            for (int l = 0; l < rowColorA.Count(); l++)
                            {
                                //填入字串
                                temp = rowColorA[l]["ThreadColorID"].ToString().Trim();

                                //根據 DataColumn & DataRow 選取 Table => 首頁 + (DataColumnIndex / 6 * 每個 FabricPanelCode 會占用的 Table 數) + k / Table 可存的 Article 數量
                                //其中 K 代表, 目前編輯到 FabricPanelCode 的第幾個 Article
                                tables = table[nextPage + (l / 6 * rC) + (k / 4)];


                                tables.Cell((4 + (k % 4)), (2 + (l % 6))).Range.Text = temp;
                            }
                        }
                    }
                    #endregion
                }
                #endregion
                winword.Visible = true;
               // winword.Quit(ref missing, ref missing, ref missing);     //close word application                
                return Result.True;
            }
            catch (Exception ex)
            {
                if (null != winword)
                    winword.Quit();
                MyUtility.Msg.WarningBox(ex.ToString(), "Export word error.");  
                return false;
            }
            finally
            {
                Marshal.ReleaseComObject(winword);
                GC.Collect();
                GC.WaitForPendingFinalizers();

                this.HideWaitMessage();
                //Marshal.FinalReleaseComObject(winword);
            }
        }
    }
}
