using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class P01_Artwork : Sci.Win.Subs.Base
    {
        private string Order_ArtworkId;
        public P01_Artwork(string ID)
        {
            this.InitializeComponent();
            this.Order_ArtworkId = ID;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region Artwork Tab

            string sqlCmd = $@" 
SELECT oa.ID,oa.AddDate,oa.AddName,oa.ArtworkID,oa.ArtworkName,oa.ArtworkTypeID,oa.Cost,oa.EditDate,oa.EditName,oa.PatternCode,oa.PatternDesc,oa.Price,oa.Qty,oa.Remark,oa.TMS,oa.Ukey 
	,''AS CreateBy,''AS EditBy,''AS UnitID, a.ArtworkUnit,article=iif(oa.article='----', art.article,oa.article)
FROM Order_Artwork oa 
LEFT JOIN ArtworkType a WITH (NOLOCK) on oa.ArtworkTypeID = a.ID
outer apply(
	select article = stuff((
		select distinct concat(',',oaa.Article)
		from Order_Article oaa with(nolock)
		inner join orders o with(nolock) on oaa.id = o.id
		where o.id = (select poid from orders o2 with(nolock) where o2.id = oa.id)
		for xml path('')
	),1,1,'')
)art
WHERE oa.ID = '{this.Order_ArtworkId}'
ORDER BY ArtworkTypeID,Article";
            DataTable artworkUnit;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out artworkUnit);

            this.Helper.Controls.Grid.Generator(this.ArtworkGrid)
               .Text("ArtworkTypeID", header: "Artwork Type", width: Widths.AnsiChars(20))
               .Text("Article", header: "Article", width: Widths.AnsiChars(8))
               .Text("PatternCode", header: "Cut Part", width: Widths.AnsiChars(10))
               .Text("PatternDesc", header: "Description", width: Widths.AnsiChars(20))
               .Text("ArtworkID", header: "Pattern#", width: Widths.AnsiChars(15))
               .Text("ArtworkName", header: "Pattern Description", width: Widths.AnsiChars(30))
               .Numeric("Qty", header: string.Empty, width: Widths.AnsiChars(5))
               .Text("UnitID", header: "Unit", width: Widths.AnsiChars(8))
               .Numeric("TMS", header: "TMS", width: Widths.AnsiChars(5))
               .Numeric("Price", header: "Price", width: Widths.AnsiChars(8), decimal_places: 4)
               .Numeric("Cost", header: "Cost", width: Widths.AnsiChars(8), decimal_places: 4)
               .EditText("Remark", header: "Remark", width: Widths.AnsiChars(30))
               .Text("CreateBy", header: "Create By", width: Widths.AnsiChars(30))
               .Text("EditBy", header: "Edit By", width: Widths.AnsiChars(30));

            foreach (DataRow gridData in artworkUnit.Rows)
            {
                gridData["CreateBy"] = gridData["AddName"].ToString() + "   " + ((DateTime)gridData["AddDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                if (gridData["EditDate"] != System.DBNull.Value)
                {
                    gridData["EditBy"] = gridData["EditName"].ToString() + "   " + ((DateTime)gridData["EditDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                }

                DataRow[] findrow = artworkUnit.Select(string.Format("ArtworkTypeID = '{0}'", gridData["ArtworkTypeID"].ToString()));
                if (findrow.Length > 0)
                {
                    gridData["UnitID"] = findrow[0]["ArtworkUnit"].ToString();
                }

                gridData.AcceptChanges();
            }

            this.ArtworkSource.DataSource = artworkUnit;
            this.ArtworkGrid.IsEditingReadOnly = true;
            this.ArtworkGrid.DataSource = this.ArtworkSource;
            #endregion

            #region Comb by SP#

            //表格的head是動態的，ArtworkTypeID + ArtworkID = 一組key
            sqlCmd = string.Format(
                            $@"  select  DISTINCT
                                oa.ArtworkTypeID
                                ,oa.ArtworkID

                                from Orders o
                                inner join Order_Artwork oa on o.ID = oa.ID
                                left join Order_Qty oq on o.ID = oq.ID and oq.Article = oa.Article
                                where o.ID in (select  id from Orders o1 where 
                                o1.POID = o.POID 
                                AND o1.POID=(SELECT TOP 1 POID FROM Orders WHERE ID='{0}') )
                                group by oa.PatternCode,oa.Article,oa.ID,oa.ArtworkTypeID,oa.ArtworkID
                                order by oa.ArtworkTypeID", this.Order_ArtworkId);
            DataTable head;
            result = DBProxy.Current.Select(null, sqlCmd, out head);

            //左半邊的單號
            sqlCmd = $@"
select DISTINCT
    oa.ID
    ,ax.article
    ,CASE WHEN o.Junk = 1  and o.NeedProduction=0 THEN 0 WHEN sum(oq.Qty) IS NULL THEN 0 ELSE sum(oq.Qty)END as OrderQty  
from Orders o
inner join Order_Artwork oa on o.ID = oa.ID
left join Order_Article oaa on oaa.id = o.id
outer apply(select article=iif(oa.article='----', oaa.article,oa.article)) ax
left join Order_Qty oq on o.ID = oq.ID and oq.Article = ax.Article
where o.ID in (select  id from Orders o1 where 
o1.POID = o.POID 
AND o1.POID=(SELECT TOP 1 POID FROM Orders WHERE ID='{this.Order_ArtworkId}') )
group by oa.PatternCode,ax.Article,oa.ID,oa.ArtworkTypeID,oa.ArtworkID,o.Junk,o.NeedProduction
order by oa.ID";
            DataTable Left;
            result = DBProxy.Current.Select(null, sqlCmd, out Left);

            //一組key對應的只會有一組 PatternCode
            sqlCmd = $@"
select DISTINCT
    oa.ID                               
    ,oa.ArtworkTypeID
    ,oa.ArtworkID
    ,ax.Article
    ,CASE WHEN o.Junk = 1 and o.NeedProduction = 0 THEN 0 WHEN sum(oq.Qty) IS NULL THEN 0 ELSE sum(oq.Qty)END as OrderQty  
    ,oa.PatternCode
from Orders o
inner join Order_Artwork oa on o.ID = oa.ID
left join Order_Article oaa on oaa.id = o.id
outer apply(select article=iif(oa.article='----', oaa.article,oa.article))ax
left join Order_Qty oq on o.ID = oq.ID and oq.Article = ax.Article
where o.ID in (select  id from Orders o1 where 
o1.POID = o.POID 
AND o1.POID=(SELECT TOP 1 POID FROM Orders WHERE ID='{this.Order_ArtworkId}') )
group by oa.PatternCode,ax.Article,oa.ID,oa.ArtworkTypeID,oa.ArtworkID,o.Junk,o.NeedProduction
order by oa.ArtworkTypeID";
            DataTable value;
            result = DBProxy.Current.Select(null, sqlCmd, out value);

            //開始畫表格

            //前三欄是固定的
            DataTable dt = new DataTable();
            dt.Columns.Add("SP#", typeof(string));
            dt.Columns.Add("Article", typeof(string));
            dt.Columns.Add("Order Qty", typeof(string));


            this.Helper.Controls.Grid.Generator(this.CombBySPgrid)
               .Text("SP#", header: "SP#", width: Widths.AnsiChars(18))
               .Text("Article", header: "Article", width: Widths.AnsiChars(10))
               .Text("Order Qty", header: "Order Qty", width: Widths.AnsiChars(10));

            //設定"有幾個"head
            if (head.Rows.Count>0)
            {

                foreach (DataRow item in head.Rows)
                {
                    //標投的文字內容，拿Key值來填
                    dt.Columns.Add(item["ArtworkTypeID"].ToString() + item["ArtworkID"].ToString(), typeof(string));
                    this.Helper.Controls.Grid.Generator(this.CombBySPgrid)
                       .Text(item["ArtworkTypeID"].ToString() + item["ArtworkID"].ToString(), header: item["ArtworkTypeID"].ToString() + Environment.NewLine + item["ArtworkID"].ToString(), width: Widths.AnsiChars(10));
                }
            }

            //依據id開始填row
            foreach (DataRow leftitem in Left.Rows)
            {
                DataRow row;
                row = dt.NewRow();

                //前面是固定的
                row["SP#"] = leftitem["ID"].ToString();
                row["Article"] = leftitem["Article"].ToString();
                row["Order Qty"] = leftitem["OrderQty"].ToString();

                if (head.Rows.Count > 0)
                {
                    foreach (DataRow valueitem in value.Rows)
                    {
                        //開始填入PatternCode
                        //必須對應ID、Article、OrderQty
                        if (valueitem["ID"].ToString()== leftitem["ID"].ToString() && 
                            valueitem["Article"].ToString() == leftitem["Article"].ToString() &&
                            valueitem["OrderQty"].ToString() == leftitem["OrderQty"].ToString()
                            )
                        {
                            row[valueitem["ArtworkTypeID"].ToString() + valueitem["ArtworkID"].ToString()] = valueitem["PatternCode"].ToString();
                        }
                    }
                }
                dt.Rows.Add(row);
            }

            this.CombBySPSource.DataSource = dt;
            this.CombBySPgrid.IsEditingReadOnly = true;
            this.CombBySPgrid.DataSource = this.CombBySPSource;
            //凍結前三欄
            this.CombBySPgrid.Columns[0].Frozen = true;
            this.CombBySPgrid.Columns[1].Frozen = true;
            this.CombBySPgrid.Columns[2].Frozen = true;
            #endregion

            #region Comb By Artwork

            // 左邊
            sqlCmd = string.Format(
                            @"  
                                SELECT DISTINCT 
                                oa.ArtworkTypeID ,oa.ArtworkID,oa.PatternCode
                                FROM Orders o
                                INNER JOIN Order_Artwork oa ON o.ID = oa.ID
                                LEFT JOIN Order_Qty oq ON o.ID = oq.ID AND oq.Article = oa.Article
                                WHERE Exists (select 1 FROM Orders o1 WHERE
                                o1.POID = o.POID 
                                AND o1.POID=(SELECT TOP 1 POID FROM Orders WHERE ID='{0}') )
                                GROUP BY o.POID ,oa.ID,oa.ArtworkTypeID,oa.ArtworkID,oa.PatternCode,oa.Article,o.Junk
                                ORDER BY  oa.ArtworkTypeID, oa.ArtworkID, oa.PatternCode

                                ", this.Order_ArtworkId);

            DataTable Left_2;
            result = DBProxy.Current.Select(null, sqlCmd, out Left_2);

            sqlCmd = $@"
;with LeftCol as (
	SELECT DISTINCT oa.ArtworkTypeID ,oa.ArtworkID,oa.PatternCode
	FROM Orders o
	INNER JOIN Order_Artwork oa ON o.ID = oa.ID
	LEFT JOIN Order_Qty oq ON o.ID = oq.ID AND oq.Article = oa.Article
	WHERE Exists (select 1 FROM Orders o1 WHERE
	o1.POID = o.POID 
	AND o1.POID=(SELECT TOP 1 POID FROM Orders WHERE ID='{this.Order_ArtworkId}') )
	GROUP BY o.POID ,oa.ID,oa.ArtworkTypeID,oa.ArtworkID,oa.PatternCode,oa.Article,o.Junk
)
SELECT 
DISTINCT 
lc.ArtworkTypeID,
lc.ArtworkID,
lc.PatternCode ,
ax.Article
,CASE WHEN o.Junk = 1 and o.NeedProduction=0 THEN 0 WHEN sum(oq.Qty) IS NULL THEN 0 ELSE sum(oq.Qty)END as OrderQty  
FROM Orders o
INNER JOIN Order_Artwork oa ON o.ID = oa.ID
LEFT JOIN LeftCol lc ON  lc.ArtworkTypeID=oa.ArtworkTypeID AND lc.ArtworkID=oa.ArtworkID AND lc.PatternCode=oa.PatternCode
outer apply(
	select article = stuff((
		select distinct concat(',', oaa.Article)
		from Order_Article oaa with(nolock)
		where oaa.id = o.poid
		for xml path('')
	),1,1,'')
)art
outer apply(select article=iif(oa.article='----', art.article,oa.article)) ax
LEFT JOIN Order_Qty oq ON o.ID = oq.ID AND oq.Article = ax.Article
WHERE Exists (select 1 FROM Orders o1 WHERE 
o1.POID = o.POID 
AND o1.POID=(SELECT TOP 1 POID FROM Orders WHERE ID='{this.Order_ArtworkId}') )
GROUP BY lc.ArtworkTypeID,lc.ArtworkID,lc.PatternCode,ax.Article,o.Junk,o.NeedProduction
ORDER BY  lc.ArtworkTypeID,lc.ArtworkID,lc.PatternCode,Article
";
            DataTable Content;
            result = DBProxy.Current.Select(null, sqlCmd, out Content);

            DataTable OrderArtworks = new DataTable();

            OrderArtworks.Columns.Add("ArtworkTypeID", typeof(string));
            OrderArtworks.Columns.Add("ArtworkID", typeof(string));
            OrderArtworks.Columns.Add("PatternCode", typeof(string));
            OrderArtworks.Columns.Add("Article", typeof(string));
            OrderArtworks.Columns.Add("OrderQty", typeof(int));

            this.Helper.Controls.Grid.Generator(this.CombByArtworkGrid)
               .Text("ArtworkTypeID", header: "Artwork", width: Widths.AnsiChars(20))
               .Text("ArtworkID", header: "Pattern#", width: Widths.AnsiChars(20))
               .Text("PatternCode", header: "Cut Part", width: Widths.AnsiChars(20))
               .Text("Article", header: "Article", width: Widths.AnsiChars(20))
               .Text("OrderQty", header: "Order Qty", width: Widths.AnsiChars(20));

            if (Left_2.Rows.Count>0)
            {
                foreach (DataRow Left_item in Left_2.Rows)
                {
                    DataRow row;
                    row = OrderArtworks.NewRow();

                    // 前面是固定的
                    row["ArtworkTypeID"] = Left_item["ArtworkTypeID"].ToString();
                    row["ArtworkID"] = Left_item["ArtworkID"].ToString();
                    row["PatternCode"] = Left_item["PatternCode"].ToString();

                    List<string> Article = new List<string>();
                    int sum = 0;

                    // Article
                    foreach (DataRow Content_item in Content.Rows)
                    {
                        if (Left_item["ArtworkTypeID"].ToString() == Content_item["ArtworkTypeID"].ToString() &&
                            Left_item["ArtworkID"].ToString() == Content_item["ArtworkID"].ToString() &&
                            Left_item["PatternCode"].ToString() == Content_item["PatternCode"].ToString() 
                            )
                        {
                            Article.Add(Content_item["Article"].ToString());
                            sum += Convert.ToInt32(Content_item["OrderQty"]);
                        }
                    }

                    row["Article"] = Article.JoinToString(",");
                    row["OrderQty"] = sum;

                    OrderArtworks.Rows.Add(row);
                }
            }

            this.CombByArtworkTypeSource.DataSource = OrderArtworks;
            this.CombByArtworkGrid.IsEditingReadOnly = true;
            this.CombByArtworkGrid.DataSource = this.CombByArtworkTypeSource;
            #endregion
        }
    }
}
