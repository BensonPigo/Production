using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class P01_Artwork : Win.Subs.Base
    {
        private string Order_ArtworkId;

        /// <summary>
        /// Initializes a new instance of the <see cref="P01_Artwork"/> class.
        /// </summary>
        /// <param name="iD">Artwork ID</param>
        public P01_Artwork(string iD)
        {
            this.InitializeComponent();
            this.Order_ArtworkId = iD;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region Artwork Tab

            string sqlCmd = $@" 
SELECT oa.ID,oa.AddDate,oa.AddName,oa.ArtworkID,oa.ArtworkName,oa.ArtworkTypeID,oa.Cost,oa.EditDate,oa.EditName,oa.PatternCode,oa.PatternDesc,oa.Price,oa.Qty,oa.Remark,oa.TMS,oa.Ukey 
	,''AS CreateBy,''AS EditBy,''AS UnitID, a.ArtworkUnit,article=iif(oa.article='----', art.article,oa.article)
FROM  Order_Artwork oa 
LEFT JOIN ArtworkType a WITH (NOLOCK) on oa.ArtworkTypeID = a.ID
outer apply(
	select article = stuff((
		select distinct concat(',',oaa.Article)
		from Order_Article oaa with(nolock)
		inner join orders o with(nolock) on oaa.id = o.id
		where o.poid = (select poid from orders o2 with(nolock) where o2.id = oa.id)
		for xml path('')
	),1,1,'')
)art
WHERE oa.ID = '{this.Order_ArtworkId}'
ORDER BY ArtworkTypeID,Article";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out DataTable artworkUnit);

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
                if (gridData["AddDate"] != DBNull.Value)
                {
                    gridData["CreateBy"] = gridData["AddName"].ToString() + "   " + ((DateTime)gridData["AddDate"]).ToString(string.Format("{0}", Env.Cfg.DateTimeStringFormat));
                }
                else
                {
                    gridData["CreateBy"] = gridData["AddName"].ToString();
                }

                if (gridData["EditDate"] != DBNull.Value)
                {
                    gridData["EditBy"] = gridData["EditName"].ToString() + "   " + ((DateTime)gridData["EditDate"]).ToString(string.Format("{0}", Env.Cfg.DateTimeStringFormat));
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

            // 表格的head是動態的，ArtworkTypeID + ArtworkID = 一組key
            sqlCmd = $@"  select  DISTINCT
                                oa.ArtworkTypeID
                                ,oa.ArtworkID
                                from Orders o
                                inner join Order_Artwork oa on o.ID = oa.ID
                                where o.POID = (SELECT POID FROM Orders WHERE ID='{this.Order_ArtworkId}') 
                                group by oa.PatternCode,oa.Article,oa.ID,oa.ArtworkTypeID,oa.ArtworkID
                                order by oa.ArtworkTypeID";
            result = DBProxy.Current.Select(null, sqlCmd, out DataTable head);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            // 左半邊的單號
            sqlCmd = $@"
--因為Order_Artwork有可能同一個OrderID下Article同時有----和正常的Article
--所以要先distinct 去除Order_Artwork重複的部分
SELECT 
distinct
oa.ID,
oa.ArtworkTypeID,
oa.ArtworkID,
oa.PatternCode ,
oq.Article,
oq.SizeCode,
[Qty] = CASE    WHEN    o.Junk = 1  and o.NeedProduction=0 THEN 0 
                else    oq.Qty  end
into #baseArtworkOrderQty
FROM Orders o
INNER JOIN Order_Artwork oa ON o.ID = oa.ID
LEFT JOIN Order_Qty oq ON o.ID = oq.ID AND (oq.Article = oa.Article or oa.article='----')
WHERE   o.POID = (SELECT POID FROM Orders WHERE ID='{this.Order_ArtworkId}')

select  bao.ID,
        bao.ArtworkTypeID,
        bao.ArtworkID,
        bao.PatternCode,
        [Article] = art.article,
        [OrderQty] = sum(isnull(bao.Qty, 0))
into    #comboBySP
from    #baseArtworkOrderQty bao
outer apply(
	select article = stuff((
		select distinct concat(',', baoo.Article)
		from #baseArtworkOrderQty baoo with(nolock)
		where	bao.ID = baoo.ID and
                bao.ArtworkTypeID = baoo.ArtworkTypeID and
				bao.ArtworkID = baoo.ArtworkID and
				bao.PatternCode = baoo.PatternCode 
		for xml path('')
	),1,1,'')
)art
group by bao.ID,
        bao.ArtworkTypeID,
        bao.ArtworkID,
        bao.PatternCode,
        art.article
order by bao.ID

-- combo by SP Right Value
select  cbs.ID,
        cbs.Article,
        cbs.ArtworkTypeID,
        cbs.ArtworkID,
        [PatternCode] = ptCode.PatternCode
from    (select  distinct
            		ID,
                    ArtworkTypeID,
                    ArtworkID,
                    Article
            from    #comboBySP) cbs
outer apply(
	select PatternCode = stuff((
		select distinct concat(',', cbss.PatternCode)
		from #comboBySP cbss with(nolock)
		where	cbs.ID = cbss.ID and
                cbs.ArtworkTypeID = cbss.ArtworkTypeID and
				cbs.ArtworkID = cbss.ArtworkID and
				cbs.Article = cbss.Article 
		for xml path('')
	),1,1,'')
) ptCode

-- combo by SP Left
select  a.ID,
		a.Article,
		[OrderQty] = sum(OrderQty)
from ( select  distinct
				ID,
				Article,
				OrderQty
				from    #comboBySP) a
group by ID, Article
order by ID

-- Combo by Artwork Type

select	bao.ArtworkTypeID,
		bao.ArtworkID,
		bao.PatternCode,
		art.Article,
		[OrderQty] = sum(isnull(bao.Qty, 0))
from #baseArtworkOrderQty bao
outer apply(
	select article = stuff((
		select distinct concat(',', baoo.Article)
		from #baseArtworkOrderQty baoo with(nolock)
		where	bao.ArtworkTypeID = baoo.ArtworkTypeID and
				bao.ArtworkID = baoo.ArtworkID and
				bao.PatternCode = baoo.PatternCode 
		for xml path('')
	),1,1,'')
)art
group by bao.ArtworkTypeID,
		 bao.ArtworkID,
		 bao.PatternCode,
         art.Article

drop table #baseArtworkOrderQty,#comboBySP
";
            result = DBProxy.Current.Select(null, sqlCmd, out DataTable[] dtComboResults);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            DataTable left = dtComboResults[1];

            // 一組key對應的只會有一組 PatternCode
            DataTable value = dtComboResults[0];

            // 開始畫表格

            // 前三欄是固定的
            DataTable dt = new DataTable();
            dt.Columns.Add("SP#", typeof(string));
            dt.Columns.Add("Article", typeof(string));
            dt.Columns.Add("Order Qty", typeof(string));

            this.Helper.Controls.Grid.Generator(this.CombBySPgrid)
               .Text("SP#", header: "SP#", width: Widths.AnsiChars(18))
               .Text("Article", header: "Article", width: Widths.AnsiChars(10))
               .Text("Order Qty", header: "Order Qty", width: Widths.AnsiChars(10));

            // 設定"有幾個"head
            if (head.Rows.Count > 0)
            {
                foreach (DataRow item in head.Rows)
                {
                    // 標投的文字內容，拿Key值來填
                    dt.Columns.Add(item["ArtworkTypeID"].ToString() + item["ArtworkID"].ToString(), typeof(string));
                    this.Helper.Controls.Grid.Generator(this.CombBySPgrid)
                       .Text(item["ArtworkTypeID"].ToString() + item["ArtworkID"].ToString(), header: item["ArtworkTypeID"].ToString() + Environment.NewLine + item["ArtworkID"].ToString(), width: Widths.AnsiChars(10));
                }
            }

            // 依據id開始填row
            foreach (DataRow leftitem in left.Rows)
            {
                DataRow row;
                row = dt.NewRow();

                // 前面是固定的
                row["SP#"] = leftitem["ID"].ToString();
                row["Article"] = leftitem["Article"].ToString();
                row["Order Qty"] = leftitem["OrderQty"].ToString();

                if (head.Rows.Count > 0)
                {
                    foreach (DataRow valueitem in value.Rows)
                    {
                        // 開始填入PatternCode
                        // 必須對應ID、Article、OrderQty
                        if (valueitem["ID"].ToString() == leftitem["ID"].ToString() &&
                            valueitem["Article"].ToString() == leftitem["Article"].ToString())
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

            // 凍結前三欄
            this.CombBySPgrid.Columns[0].Frozen = true;
            this.CombBySPgrid.Columns[1].Frozen = true;
            this.CombBySPgrid.Columns[2].Frozen = true;
            #endregion

            #region Comb By Artwork

            DataTable orderArtworks = dtComboResults[2];

            this.Helper.Controls.Grid.Generator(this.CombByArtworkGrid)
               .Text("ArtworkTypeID", header: "Artwork", width: Widths.AnsiChars(20))
               .Text("ArtworkID", header: "Pattern#", width: Widths.AnsiChars(20))
               .Text("PatternCode", header: "Cut Part", width: Widths.AnsiChars(20))
               .Text("Article", header: "Article", width: Widths.AnsiChars(20))
               .Text("OrderQty", header: "Order Qty", width: Widths.AnsiChars(20));

            this.CombByArtworkTypeSource.DataSource = orderArtworks;
            this.CombByArtworkGrid.IsEditingReadOnly = true;
            this.CombByArtworkGrid.DataSource = this.CombByArtworkTypeSource;
            #endregion
        }
    }
}
