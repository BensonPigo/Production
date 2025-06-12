using System;
using System.Data;
using System.Drawing;
using System.Text;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_Qty
    /// </summary>
    public partial class P01_Qty : Win.Subs.Base
    {
        private bool isPPIC_P01_Class;
        private bool isTabPag5;
        private bool isTabPag6;
        private string orderID;
        private string poID;
        private string poCombo;
        private DataTable grid1Data;
        private DataTable grid2Data;
        private DataTable grid3Data;
        private DataTable grid4Data;  // 存Qty資料
        private DataTable grid5Data;
        private DataTable grid6Data;

        private DataTable grid1Data_OriQty;
        private DataTable grid2Data_OriQty;
        private DataTable grid3Data_OriQty;
        private DataTable grid4Data_OriQty;  // 存OriQty資料

        /// <summary>
        /// P01_Qty
        /// </summary>
        /// <param name="orderID">string OrderID</param>
        /// <param name="pOID">string POID</param>
        /// <param name="pOCombo">string POCombo</param>
        /// <param name="isPPIC_P01">isPPIC_P01</param>
        /// <param name="isfromWH">isfromWH</param>
        public P01_Qty(string orderID, string pOID, string pOCombo, bool isPPIC_P01 = false, bool isfromWH = false)
        {
            this.InitializeComponent();
            this.orderID = orderID;
            this.poID = pOID;
            this.poCombo = pOCombo;
            this.Text = this.Text + " (" + this.orderID + ")";
            this.displaySPPOCombination.Value = this.poCombo;
            this.displayColorwayPOCombination.Value = this.poCombo;
            this.displayDeliveryPOCombination.Value = this.poCombo;
            this.isPPIC_P01_Class = isPPIC_P01;

            // 預設TabPage 隱藏
            this.tabPage5.Parent = null;
            this.tabPage6.Parent = null;

            this.btnPrint.Visible = isfromWH;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // 撈出所有的Size
            string sqlCmd = string.Format("select * from Order_SizeCode WITH (NOLOCK) where ID = '{0}' order by iif(SizeGroup = 'N', Null, SizeGroup),seq", this.poID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out DataTable headerData);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            StringBuilder pivot = new StringBuilder();

            // 設定Grid1的顯示欄位
            this.gridQtyBDown.IsEditingReadOnly = true;
            this.gridQtyBDown.DataSource = this.listControlBindingSource1;
            var gen = this.Helper.Controls.Grid.Generator(this.gridQtyBDown);
            this.CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            this.CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(9));
            this.CreateGrid(gen, "string", "ArticleName", "Colorway Name", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    this.CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                    pivot.Append(string.Format("[{0}],", MyUtility.Convert.GetString(dr["SizeCode"])));
                }
            }

            DataTable btop = new DataTable();
            btop.Columns.Add("TotalQty");
            btop.Columns.Add("Article");
            this.Helper.Controls.Grid.Generator(this.gridqtybdownTop)
                .Text("total", header: string.Empty, width: Widths.AnsiChars(6))
                .Text("Article", header: string.Empty, width: Widths.AnsiChars(9));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    string sizeCode = MyUtility.Convert.GetString(dr["SizeCode"]);
                    btop.Columns.Add(sizeCode);
                    this.Helper.Controls.Grid.Generator(this.gridqtybdownTop)
                        .Text(sizeCode, header: sizeCode, width: Widths.AnsiChars(8));
                }
            }

            DataRow btopdr = btop.NewRow();
            btopdr["Article"] = "SizeGroup";
            foreach (DataRow dr in headerData.Rows)
            {
                string sizeCode = MyUtility.Convert.GetString(dr["SizeCode"]);
                btopdr[sizeCode] = dr["SizeGroup"];
            }

            btop.Rows.Add(btopdr);
            this.listControlBindingSource5.DataSource = btop;

            // 設定Grid3的顯示欄位
            this.gridColorway.IsEditingReadOnly = true;
            this.gridColorway.DataSource = this.listControlBindingSource3;
            gen = this.Helper.Controls.Grid.Generator(this.gridColorway);
            this.CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            this.CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            this.CreateGrid(gen, "string", "ArticleName", "Colorway Name", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    this.CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                }
            }

            // 設定Grid4的顯示欄位
            this.gridDelivery.IsEditingReadOnly = true;
            this.gridDelivery.DataSource = this.listControlBindingSource4;
            gen = this.Helper.Controls.Grid.Generator(this.gridDelivery);
            this.CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            this.CreateGrid(gen, "date", "BuyerDelivery", "Buyer Delivery", Widths.AnsiChars(10));
            this.CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            this.CreateGrid(gen, "string", "ArticleName", "Colorway Name", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    this.CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                }
            }

            // 凍結欄位
            this.gridQtyBDown.Columns[1].Frozen = true;
            this.gridqtybdownTop.Columns[1].Frozen = true;
            this.gridColorway.Columns[1].Frozen = true;
            this.gridDelivery.Columns[2].Frozen = true;

            #region 撈Grid1資料
            sqlCmd = string.Format(
                @"
with SortBy as (
      select oq.Article  
             , RowNo = ROW_NUMBER() over (order by oq.Article) 
      from Order_Qty oq WITH (NOLOCK) 
      where oq.ID = '{0}'
      group by oq.Article
),
tmpData as (
      select oq.Article
             , sa.ArticleName
             , oq.SizeCode
             , oq.Qty
             , sb.RowNo
      from Order_Qty oq WITH (NOLOCK) 
      inner join SortBy sb on oq.Article = sb.Article
      left join orders o WITH (NOLOCK) on oq.id = o.id
      left join Style_Article sa WITH (NOLOCK) on sa.StyleUkey = o.StyleUkey and sa.Article = oq.Article
      where oq.ID = '{0}'
),
SubTotal as (
      select 'TTL' as Article
             , '' as ArticleName
             , SizeCode
             , SUM(Qty) as Qty
             , '9999' as RowNo
      from tmpData
      group by SizeCode
),
UnionData as (
      select * from tmpData
      union all
      select * from SubTotal
),
pivotData as (
      select *
      from UnionData
      pivot( 
            sum(Qty) for SizeCode in ({1})
      ) a
)
select *
       , (select sum(Qty) from UnionData where Article = p.Article) as TotalQty
from pivotData p
order by RowNo",
                this.orderID,
                MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));

            result = DBProxy.Current.Select(null, sqlCmd, out this.grid1Data);

            sqlCmd = string.Format(
                @"
with SortBy as (
      select oq.Article
             , RowNo = ROW_NUMBER() over (order by oq.Article)
      from Order_Qty oq WITH (NOLOCK) 
      where oq.ID = '{0}'
      group by oq.Article
),
tmpData as (
      select oq.Article
             , sa.ArticleName
             , oq.SizeCode
             , oq.OriQty
             , sb.RowNo
      from Order_Qty oq WITH (NOLOCK) 
      inner join SortBy sb on oq.Article = sb.Article
      left join orders o WITH (NOLOCK) on oq.id = o.id
      left join Style_Article sa WITH (NOLOCK) on sa.StyleUkey = o.StyleUkey and sa.Article = oq.Article
      where oq.ID = '{0}'
),
SubTotal as (
      select 'TTL' as Article
             , '' as ArticleName
             , SizeCode
             , SUM(OriQty) as Qty
             , '9999' as RowNo
      from tmpData
      group by SizeCode
),
UnionData as (
      select * from tmpData
      union all
      select * from SubTotal
),
pivotData as (
      select *
      from UnionData
      pivot( 
            sum(OriQty) for SizeCode in ({1})
      ) a
)
select *
       , (select sum(OriQty) from UnionData where Article = p.Article) as TotalQty
from pivotData p
order by RowNo",
                this.orderID,
                MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));

            result = DBProxy.Current.Select(null, sqlCmd, out this.grid1Data_OriQty);
            #endregion

            #region 撈Grid2資料
            sqlCmd = string.Format(
                @"
with tmpData as (
      select o.ID
             , oq.Article
             , sa.ArticleName
             , Order_ColorCombo.ColorID
			 , c.Alias
			 , o.CustCDID
			 , CustCD.Kit
             ,o.CustPONo
			 ,[SpecialField] = o.Customize1
             , iif(o.junk = 1 , '' ,oq.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oq.Qty) as Qty 
             , DENSE_RANK() OVER (ORDER BY o.ID) as rnk
             , BuyerDelivery = o.BuyerDelivery
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
      left join Style_Article sa WITH (NOLOCK) on sa.StyleUkey = o.StyleUkey and sa.Article = oq.Article
	  left join Country c  WITH (NOLOCK) on c.ID = o.Dest
	  left join CustCD WITH (NOLOCK) on CustCD.BrandID  = o.BrandID  and CustCD.ID = o.CustCDID 
      outer apply (
			select top 1 ColorID
			from Order_ColorCombo occ WITH (NOLOCK)
			where occ.ID = o.Poid and occ.Article = oq.Article and occ.Patternpanel = 'FA'
		) Order_ColorCombo	
      where o.POID = '{0}'
), 
SubTotal as (
      select ID = ''
             , Article = 'TTL'
             , ArticleName = ''
             , ColorID = ''
			 , Alias = ''
			 , CustCDID = ''
			 , Kit = ''
             , CustPONo = ''
			 ,[SpecialField] = ''
             , SizeCode
             , Qty = SUM(Qty)
             , rnk = 99999
             , BuyerDelivery = null
      from tmpData
      group by SizeCode
),
UnionData as (
      select * from tmpData
      union all
      select * from SubTotal
),
pivotData as (
      select *
      from UnionData
      pivot( 
            sum(Qty) for SizeCode in ({1})
      ) a
)
select *
       , (select sum(Qty) from UnionData where ID = p.ID and Article = p.Article) as TotalQty
from pivotData p
order by rnk",
                this.poID,
                MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));

            result = DBProxy.Current.Select(null, sqlCmd, out this.grid2Data);

            sqlCmd = string.Format(
                @"
with tmpData as (
      select o.ID
             , oq.Article
             , sa.ArticleName
             , Order_ColorCombo.ColorID
			 , c.Alias
			 , o.CustCDID
			 , CustCD.Kit
             ,o.CustPONo
			 ,[SpecialField] = o.Customize1
			 ,[TT_SpecialField] = b.Customize1
             , iif(o.junk = 1 , '' ,oq.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oq.OriQty) as OriQty
             , DENSE_RANK() OVER (ORDER BY o.ID) as rnk
             , BuyerDelivery = o.BuyerDelivery
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
      left join Style_Article sa WITH (NOLOCK) on sa.StyleUkey = o.StyleUkey and sa.Article = oq.Article
	  left join Country c  WITH (NOLOCK) on c.ID = o.Dest
	  left join CustCD WITH (NOLOCK) on CustCD.BrandID  = o.BrandID  and CustCD.ID = o.CustCDID 
      left join Brand b with(nolock) on o.BrandID = b.ID
      outer apply (
			select top 1 ColorID
			from Order_ColorCombo occ WITH (NOLOCK)
			where occ.ID = o.Poid and occ.Article = oq.Article and occ.Patternpanel = 'FA'
		) Order_ColorCombo	
      where o.POID = '{0}' 
),
SubTotal as (
      select ID = ''
             , Article = 'TTL'
             , ArticleName = ''
             , ColorID = ''
			 , Alias = ''
			 , CustCDID = ''
			 , Kit = ''
             ,CustPONo = ''
			 ,[SpecialField] = ''
			 ,[TT_SpecialField] = ''
             , SizeCode
             , Qty = SUM(OriQty)
             , rnk = 99999 
             , BuyerDelivery = null
      from tmpData
      group by SizeCode
),
UnionData as (
      select * from tmpData
      union all
      select * from SubTotal
),
pivotData as (
      select *
      from UnionData
      pivot( 
            sum(OriQty) for SizeCode in ({1})
      ) a
)
select *
      , (select sum(OriQty) from UnionData where ID = p.ID and Article = p.Article) as TotalQty
from pivotData p
order by rnk",
                this.poID,
                MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));

            result = DBProxy.Current.Select(null, sqlCmd, out this.grid2Data_OriQty);

            // 設定Grid2的顯示欄位
            this.gridCombBySPNo.IsEditingReadOnly = true;
            this.gridCombBySPNo.DataSource = this.listControlBindingSource2;
            gen = this.Helper.Controls.Grid.Generator(this.gridCombBySPNo);
            this.CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            this.CreateGrid(gen, "string", "ID", "SP#", Widths.AnsiChars(15));
            this.CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            this.CreateGrid(gen, "string", "ArticleName", "Colorway Name", Widths.AnsiChars(8));
            this.CreateGrid(gen, "string", "ColorID", "Color", Widths.AnsiChars(8));
            this.CreateGrid(gen, "string", "Alias", "Destination", Widths.AnsiChars(15));
            this.CreateGrid(gen, "string", "CustCDID", "CustCD", Widths.AnsiChars(12));
            this.CreateGrid(gen, "string", "Kit", "KIT#", Widths.AnsiChars(10));

            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    this.CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                }
            }

            this.CreateGrid(gen, "string", "BuyerDelivery", "Buyer Delivery", Widths.AnsiChars(8));
            this.CreateGrid(gen, "string", "CustPONo", "PO No.", Widths.AnsiChars(15));

            string sqlcmd = $@"
                select 
                b.Customize1
                from orders o with(nolock)
                left join brand b with(nolock) on o.BrandID = b.ID
                where o.id = '{this.orderID}'";
            string columnName = MyUtility.GetValue.Lookup(sqlcmd);
            if (columnName != string.Empty)
            {
                this.CreateGrid(gen, "string", "SpecialField", columnName, Widths.AnsiChars(15));
            }

            // 凍結欄位
            this.gridCombBySPNo.Columns[2].Frozen = true;
            #endregion

            #region 撈Grid3資料
            sqlCmd = string.Format(
                @"
with SortBy as (
      select oq.Article
             , RowNo = ROW_NUMBER() over (order by oq.Article)
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
      where o.POID = '{0}'
      group by oq.Article
),
tmpData as (
      select oq.Article
             , sa.ArticleName
             , iif(o.junk = 1 , '' ,oq.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oq.Qty) as Qty 
             , sb.RowNo
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
      inner join SortBy sb on oq.Article = sb.Article
      left join Style_Article sa WITH (NOLOCK) on sa.StyleUkey = o.StyleUkey and sa.Article = oq.Article
      where o.POID = '{0}' 
),
SubTotal as (
      select 'TTL' as Article
             , '' as ArticleName
             , SizeCode,SUM(Qty) as Qty
             , '9999' as RowNo
      from tmpData
      group by SizeCode
), 
UnionData as (
      select * from tmpData
      
      union all
      select * from SubTotal
),
pivotData as (
      select *
      from UnionData
      pivot( 
            sum(Qty) for SizeCode in ({1})
      ) a
)
select *
       , (select sum(Qty) from UnionData where Article = p.Article) as TotalQty
from pivotData p
order by RowNo",
                this.poID,
                MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));

            result = DBProxy.Current.Select(null, sqlCmd, out this.grid3Data);

            sqlCmd = string.Format(
                @"
with SortBy as (
      select oq.Article
             , RowNo = ROW_NUMBER() over (order by oq.Article)
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
      where o.POID = '{0}'
      group by oq.Article
),
tmpData as (
      select oq.Article
             , sa.ArticleName
             , iif(o.junk = 1 , '' ,oq.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oq.OriQty) as OriQty
             , sb.RowNo
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID      
      inner join SortBy sb on oq.Article = sb.Article
      left join Style_Article sa WITH (NOLOCK) on sa.StyleUkey = o.StyleUkey and sa.Article = oq.Article
      where o.POID = '{0}' 
),
SubTotal as (
      select 'TTL' as Article
             , '' as ArticleName
             , SizeCode
             , SUM(OriQty) as Qty
             , '9999' as RowNo
      from tmpData
      group by SizeCode
),
UnionData as (
      select * from tmpData
      union all
      select * from SubTotal
),
pivotData as (
      select *
      from UnionData
      pivot( 
            sum(OriQty) for SizeCode in ({1})
      ) a
)
select *
       , (select sum(OriQty) from UnionData where Article = p.Article) as TotalQty
from pivotData p
order by RowNo",
                this.poID,
                MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));

            result = DBProxy.Current.Select(null, sqlCmd, out this.grid3Data_OriQty);
            #endregion

            #region 撈Grid4資料
            sqlCmd = string.Format(
                @"
with SortBy as (
      select oqd.Article
             , RowNo = Row_Number() over (order by oqd.Article)
      from Orders o WITH (NOLOCK) 
      inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
      inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID and oq.Seq = oqd.Seq
      where o.POID = '{0}'
      group by oqd.Article
),
tmpData as (
      select oq.BuyerDelivery
             , oqd.Article
             , sa.ArticleName
             , iif(o.junk = 1 , '' ,oqd.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oqd.Qty) as Qty
             , DENSE_RANK() OVER (ORDER BY oq.BuyerDelivery) as rnk
             , sb.RowNo
      from Orders o WITH (NOLOCK) 
      inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
      inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID and oq.Seq = oqd.Seq
      inner join SortBy sb WITH (NOLOCK) on oqd.Article = sb.Article
      left join Style_Article sa WITH (NOLOCK) on sa.StyleUkey = o.StyleUkey and oqd.Article = sa.Article
      where o.POID = '{0}' 
),
SubTotal as (
      select null as BuyerDelivery
             , 'TTL' as Article
             , '' as ArticleName
             , SizeCode
             , SUM(Qty) as Qty
             , 99999 as rnk
             , 99999 as RowNo
      from tmpData
      group by SizeCode
),
UnionData as (
      select * from tmpData
      union all
      select * from SubTotal
),
pivotData as (
      select *
      from UnionData
      pivot( 
            sum(Qty) for SizeCode in ({1})
      ) a
)
select *
       , (select sum(isnull(Qty,0)) from UnionData where rnk = p.rnk and Article = p.Article) as TotalQty
from pivotData p
order by rnk, RowNo",
                this.poID,
                MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));

            result = DBProxy.Current.Select(null, sqlCmd, out this.grid4Data);

            sqlCmd = string.Format(
                @"
with SortBy as (
      select oqd.Article
             , RowNo = Row_Number() over (order by oqd.Article)
      from Orders o WITH (NOLOCK) 
      inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
      inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID and oq.Seq = oqd.Seq
      where o.POID = '{0}'
      group by oqd.Article
), 
tmpData as (
      select oq.BuyerDelivery
             , oqd.Article
             , sa.ArticleName
             , iif(o.junk = 1 , '' ,oqd.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oqd.OriQty) as OriQty
             , DENSE_RANK() OVER (ORDER BY oq.BuyerDelivery) as rnk
             , sb.RowNo
      from Orders o WITH (NOLOCK) 
      inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
      inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID and oq.Seq = oqd.Seq
      inner join SortBy sb on oqd.Article = sb.Article
      left join Style_Article sa on sa.StyleUkey = o.StyleUkey and oqd.Article = sa.Article
      where o.POID = '{0}'  
),
SubTotal as (
      select null as BuyerDelivery
             , 'TTL' as Article
             , '' as ArticleName
             , SizeCode
             , SUM(OriQty) as Qty
             , 99999 as rnk
             , 99999 as RowNo
      from tmpData
      group by SizeCode
),
UnionData as (
      select * from tmpData
      union all
      select * from SubTotal
),
pivotData as (
      select *
      from UnionData
      pivot( 
            sum(OriQty) for SizeCode in ({1})
      ) a
)
select *
       , (select sum(isnull(OriQty,0)) from UnionData where rnk = p.rnk and Article = p.Article) as TotalQty
from pivotData p
order by rnk, RowNo",
                this.poID,
                MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));

            result = DBProxy.Current.Select(null, sqlCmd, out this.grid4Data_OriQty);
            #endregion

            this.listControlBindingSource1.DataSource = this.grid1Data;
            this.listControlBindingSource2.DataSource = this.grid2Data;
            this.listControlBindingSource3.DataSource = this.grid3Data;
            this.listControlBindingSource4.DataSource = this.grid4Data;

            #region TabPage 5 Grid 設定
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = this.listControlBindingSource6;
            gen = this.Helper.Controls.Grid.Generator(this.grid1);
            this.CreateGrid(gen, "int", "Total", "Total", Widths.AnsiChars(6));
            this.CreateGrid(gen, "string", "SP#", "SP#", Widths.AnsiChars(13));
            this.CreateGrid(gen, "string", "Garment SP#", "Garment SP#", Widths.AnsiChars(13));
            this.CreateGrid(gen, "string", "Colorway", "Colorway", Widths.AnsiChars(8));

            // SizeCode 查詢
            string sqlSizeCode_5 = $@"select 
                                    os.SizeCode
                                    from Order_SizeCode os 
                                    where 
                                    EXISTS (
	                                    select 1
	                                    from Order_Qty_Garment oqg with(nolock) 
	                                    inner join Orders o with(nolock) on o.ID = oqg.id
	                                    where oqg.OrderIDFrom = '{this.orderID}'  and oqg.Junk = 0
	                                    and o.poid = os.id and os.SizeCode = oqg.SizeCode
                                    ) 
                                    order by os.Seq";
            string strSizeCode_5 = string.Empty;
            DBProxy.Current.Select(null, sqlSizeCode_5, out DataTable dt_5);
            StringBuilder pivot1 = new StringBuilder();
            if (dt_5 != null && dt_5.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_5.Rows)
                {
                    this.CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                    pivot1.Append(string.Format("[{0}],", MyUtility.Convert.GetString(dr["SizeCode"])));
                    strSizeCode_5 += $@" ,[{MyUtility.Convert.GetString(dr["SizeCode"])}] =p.[{MyUtility.Convert.GetString(dr["SizeCode"])}]" + Environment.NewLine;
                }
            }

            this.CreateGrid(gen, "date", "Buyer Delivery", "Buyer Delivery", Widths.AnsiChars(10));
            #endregion

            #region TabPage 6 Grid 設定
            this.grid2.IsEditingReadOnly = true;
            this.grid2.DataSource = this.listControlBindingSource7;
            gen = this.Helper.Controls.Grid.Generator(this.grid2);
            this.CreateGrid(gen, "int", "Total", "Total", Widths.AnsiChars(6));
            this.CreateGrid(gen, "string", "SP#", "SP#", Widths.AnsiChars(13));
            this.CreateGrid(gen, "string", "From SP#", "From SP#", Widths.AnsiChars(13));
            this.CreateGrid(gen, "string", "Colorway", "Colorway", Widths.AnsiChars(8));

            string sqlSizeCode_6 = $@"select 
                                    os.SizeCode
                                    from Order_SizeCode os 
                                    where 
                                    EXISTS (
	                                    select 1
                                        from Order_Qty_Garment oqg with(nolock) 
                                        inner join Orders o with(nolock) on o.ID = oqg.id
                                        where oqg.ID  = '{this.orderID}' and oqg.Junk = 0
	                                    and os.id = o.POID and os.SizeCode = oqg.SizeCode
                                    ) 
                                    order by os.Seq";

            string strSizeCode_6 = string.Empty;
            DBProxy.Current.Select(null, sqlSizeCode_6, out DataTable dt_6);
            StringBuilder pivot2 = new StringBuilder();
            if (dt_6 != null && dt_6.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_6.Rows)
                {
                    this.CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                    pivot2.Append(string.Format("[{0}],", MyUtility.Convert.GetString(dr["SizeCode"])));
                    strSizeCode_6 += $@" ,[{MyUtility.Convert.GetString(dr["SizeCode"])}] = p.[{MyUtility.Convert.GetString(dr["SizeCode"])}]" + Environment.NewLine;
                }
            }

            this.CreateGrid(gen, "date", "Buyer Delivery", "Buyer Delivery", Widths.AnsiChars(10));
            #endregion

            if (this.isPPIC_P01_Class)
            {
                string sQLcmd_TabPag5 = $@"select 1
                            from Orders
                            where 
                            Category ='B' 
                            and OrderTypeID +BrandID in (select ID+BrandID from OrderType where IsGMTMaster = 1)
                            and ID = '{this.orderID}'";
                this.isTabPag5 = MyUtility.GetValue.Lookup(sQLcmd_TabPag5) == "1" ? true : false;
                if (this.isTabPag5)
                {
                    string strSize = MyUtility.Check.Empty(pivot1.ToString()) ? "[ ]" : pivot1.ToString().Substring(0, pivot1.ToString().Length - 1);
                    this.tabPage5.Parent = this.tabControl1; // 顯示TabPage
                    string sqlTable5 = $@"
                    with 
                    tmpData as (
	                    select 
	                    [SP#] = oqg.OrderIDFrom
                        ,[Garment SP#] = oqg.ID
                        ,[Colorway] = Article
                        ,SizeCode
                        ,oqg.Qty
                        ,[RowNo] = ''
                        from Order_Qty_Garment oqg with(nolock) 
                        inner join Orders o with(nolock) on o.ID = oqg.id
                        where oqg.OrderIDFrom = '{this.orderID}'  and oqg.Junk = 0
                    ),
                    SubTotal as (
                          select 
	                    SP# =''
	                    ,[Garment SP#] = ''
	                    ,'TTL' as Article
	                    , SizeCode
	                    , SUM(Qty) as Qty
                        ,[RowNo] = '99999'
                          from tmpData a
                          group by SizeCode
                    ),
                    UnionData as (
                          select * from tmpData
                          union all
                          select * from SubTotal
                    ),
                    pivotData as
                    (
                            select *
                            from UnionData
                            pivot( 
                                sum(Qty) for SizeCode in ({strSize})
                            ) a
                    )
                    select
	                [Total] = (select sum(isnull(Qty,0)) from UnionData where [SP#] = p.[SP#] and [Garment SP#] = p.[Garment SP#] and [Colorway] = p.[Colorway])
			        ,p.[SP#]
					,p.[Garment SP#]
					,p.[Colorway]
                    {strSizeCode_5}
                    ,[Buyer Delivery] = convert(date,(select BuyerDelivery from Orders where  ID = p.[SP#]))
                    from pivotData p
                    order by RowNo";
                    result = DBProxy.Current.Select(null, sqlTable5, out this.grid5Data);
                    this.listControlBindingSource6.DataSource = this.grid5Data;
                }

                string sQLcmd_TabPage6 = $@"select 1 from Orders where Category ='G' and id = '{this.orderID}'";
                this.isTabPag6 = MyUtility.GetValue.Lookup(sQLcmd_TabPage6) == "1" ? true : false;
                if (this.isTabPag6)
                {
                    this.tabPage6.Parent = this.tabControl1; // 顯示TabPage
                    string strSize = MyUtility.Check.Empty(pivot2.ToString()) ? "[ ]" : pivot2.ToString().Substring(0, pivot2.ToString().Length - 1);
                    string sqlTable6 = $@"
                    with 
                    tmpData as (
	                    select 
	                    [SP#] = o.ID
                        ,[From SP#] = oqg.OrderIDFrom
	                    ,[Colorway] = Article
                        ,SizeCode
                        ,oqg.Qty
                        ,[RowNo] = ''
                        from Order_Qty_Garment oqg with(nolock) 
                        inner join Orders o with(nolock) on o.ID = oqg.id
                        where oqg.ID  = '{this.orderID}' and oqg.Junk = 0
                    ),
                    SubTotal as (
                          select 
	                    SP# =''
	                    ,[From SP#] = ''
	                    ,'TTL' as Article
	                    , SizeCode
	                    , SUM(Qty) as Qty
                        ,[RowNo] = '999999'
                          from tmpData a
                          group by SizeCode
                    ),
                    UnionData as (
                          select * from tmpData
                          union all
                          select * from SubTotal
                    ),
                    pivotData as
                    (
                            select *
                            from UnionData
                            pivot( 
                                sum(Qty) for SizeCode in ({strSize})
                            ) a
                    )
                    select
                    [Total] = (select sum(isnull(Qty,0)) from UnionData where [SP#] = p.[SP#] and [From SP#] = p.[From SP#] and [Colorway] = p.[Colorway])
                    ,p.[SP#]
					,p.[From SP#]
					,p.[Colorway]
	                {strSizeCode_6}
	                ,[Buyer Delivery] = convert(date,(select BuyerDelivery from Orders where  ID = p.[SP#]))
                    from pivotData p
                    order by RowNo";
                    result = DBProxy.Current.Select(null, sqlTable6, out this.grid6Data);
                    this.listControlBindingSource7.DataSource = this.grid6Data;
                }
            }
        }

        /// <summary>
        /// CreateGrid
        /// </summary>
        /// <param name="gen">IDataGridViewGenerator gen</param>
        /// <param name="datatype">string datatype</param>
        /// <param name="propname">string propname</param>
        /// <param name="header">string header</param>
        /// <param name="width">IWidth width</param>
        public void CreateGrid(IDataGridViewGenerator gen, string datatype, string propname, string header, IWidth width)
        {
            this.CreateGridCol(
                gen,
                datatype,
                propname: propname,
                header: header,
                width: width);
        }

        private void CreateGridCol(IDataGridViewGenerator gen, string datatype, string propname = null, string header = null, IWidth width = null, bool? iseditingreadonly = null, int index = -1)
        {
            switch (datatype)
            {
                case "int":
                    gen.Numeric(propname, header: header, width: width, iseditingreadonly: iseditingreadonly);
                    break;
                case "date":
                    gen.Date(propname, header: header, width: width, iseditingreadonly: iseditingreadonly);
                    break;
                case "string":
                default:
                    gen.Text(propname, header: header, width: width, iseditingreadonly: iseditingreadonly);
                    break;
            }
        }

        private void RadioOriQty_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioQty.Checked)
            {
                this.listControlBindingSource1.DataSource = this.grid1Data;
                this.listControlBindingSource2.DataSource = this.grid2Data;
                this.listControlBindingSource3.DataSource = this.grid3Data;
                this.listControlBindingSource4.DataSource = this.grid4Data;
            }
            else if (this.radioOriQty.Checked)
            {
                this.listControlBindingSource1.DataSource = this.grid1Data_OriQty;
                this.listControlBindingSource2.DataSource = this.grid2Data_OriQty;
                this.listControlBindingSource3.DataSource = this.grid3Data_OriQty;
                this.listControlBindingSource4.DataSource = this.grid4Data_OriQty;
            }
        }

        private void ToExcel_Click(object sender, EventArgs e)
        {
            DataTable ptb1, ptb2, ptb3, ptb4;
            if (this.radioQty.Checked)
            {
                #region
                string sqlcmd1 = string.Format(
                    @"
DECLARE @ID nvarchar(20) = '{0}'
DECLARE @POID nvarchar(20) = '{1}'
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(SizeCode),N',' + QUOTENAME(SizeCode))
FROM (
    select distinct osc.SizeCode,osc.Seq,SizeGroup = iif(osc.SizeGroup = 'N', '', osc.SizeGroup)
	from Orders o WITH (NOLOCK) 
	inner join Order_SizeCode osc WITH (NOLOCK) on osc.Id = o.ID 
    where o.ID = @POID 
)a
order by SizeGroup,seq

print @cols

DECLARE @sql NVARCHAR(MAX)
SET @sql = N'
;with SortBy as (
      select oq.Article
             , RowNo = Row_Number() over (order by oq.Article)
      from Order_Qty oq WITH (NOLOCK) 
      where oq.ID = '''+@ID+'''
      group by oq.Article
),
tmpData as (
      select oq.Article
             , oq.SizeCode
             , oq.Qty
             , sb.RowNo
      from Order_Qty oq WITH (NOLOCK) 
      inner join SortBy sb on oq.Article = sb.Article
      where oq.ID = '''+@ID+'''
),
SubTotal as (
      select ''TTL'' as Article
             , SizeCode
             , SUM(Qty) as Qty
             , 9999 as RowNo
      from tmpData
      group by SizeCode
),
UnionData as (
      select * from tmpData
      union all
      select * from SubTotal
),
pivotData as (
      select *
      from UnionData
      pivot( 
            sum(Qty) for SizeCode in ('+@cols+')
      ) a
)
select (select sum(Qty) from UnionData where Article = p.Article) as TotalQty
       , [Colorway] = p.Article
       , '+@cols+'
from pivotData p
order by RowNO'

EXEC sp_executesql @sql", this.orderID, this.poID);

                DBProxy.Current.Select(null, sqlcmd1, out ptb1);

                string sqlcmd2 = string.Format(
                    @"
DECLARE @ID nvarchar(20) = '{0}'
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(SizeCode),N',' + QUOTENAME(SizeCode))
FROM (
      	select distinct osc.SizeCode,osc.Seq,SizeGroup = iif(osc.SizeGroup = 'N', '', osc.SizeGroup)
	    from Orders o WITH (NOLOCK) 
	    inner join Order_SizeCode osc WITH (NOLOCK) on osc.Id = o.ID 
        where o.POID = @ID
)s
order by SizeGroup,seq

DECLARE @sql NVARCHAR(MAX)
SET @sql = N'
;with SortBy as (
      select oq.Article
             , RowNo = Row_Number() over (order by oq.Article)
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
      where o.POID = '''+@ID+'''
      group by oq.Article
),
tmpData as (
      select o.ID
             , oq.Article
             , Order_ColorCombo.ColorID
			 , c.Alias
			 , o.CustCDID
			 , CustCD.Kit
			 ,o.CustPONo
			 ,[SpecialField] = o.Customize1
             , iif(o.junk = 1 , '''' ,oq.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oq.Qty) as Qty
             , DENSE_RANK() OVER (ORDER BY o.ID) as rnk
             , sb.RowNo
             , BuyerDelivery = '''''''' + Convert (varchar(10), o.BuyerDelivery)			
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
      inner join SortBy sb on oq.Article = sb.Article
	  left join Country c WITH (NOLOCK) on c.ID = o.Dest
	  left join CustCD WITH (NOLOCK) on CustCD.BrandID  = o.BrandID  and CustCD.ID = o.CustCDID 
      outer apply (
			select top 1 ColorID
			from Order_ColorCombo occ WITH (NOLOCK)
			where occ.ID = o.Poid and occ.Article = oq.Article and occ.Patternpanel = ''FA''
		) Order_ColorCombo	
      where o.POID = '''+@ID+'''  
),
SubTotal as (
      select '''' as ID
             , ''TTL'' as Article
             , '''' as ColorID
			 , Alias = ''''
			 , CustCDID = ''''
			 , Kit = ''''
			　,CustPONo = ''''
			 ,[SpecialField] = ''''
             , SizeCode
             , SUM(Qty) as Qty             
             , 99999 as rnk
             , 99999 as RowNo
             , null as BuyerDelivery
      from tmpData
      group by SizeCode
),
UnionData as (
      select * from tmpData
      union all
      select * from SubTotal
),
pivotData as (
      select *
      from UnionData
      pivot( 
            sum(Qty)   for SizeCode in ('+@cols+')
      ) a
)
select TotalQty = (select sum(Qty) from UnionData where ID = p.ID and Article = p.Article)
       , [Sp#] = ID
       , [Colorway] = p.Article
       , Color = p.ColorID
       , Destination = Alias
	   , CustCD = CustCDID
	   , Kit# = Kit

       , '+@cols+'
       , [Buyer Delivery] = p.BuyerDelivery
	   ,[PO No.] = p.CustPONo
	   ,[SpecialField] = p.SpecialField
from pivotData p
order by rnk, RowNo'

EXEC sp_executesql @sql", this.poID);
                DBProxy.Current.Select(null, sqlcmd2, out ptb2);

                string sqlcmd3 = string.Format(
                    @"
DECLARE @ID nvarchar(20) = '{0}'
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(SizeCode),N',' + QUOTENAME(SizeCode))
FROM (
      	select distinct osc.SizeCode,osc.Seq,SizeGroup = iif(osc.SizeGroup = 'N', '', osc.SizeGroup)
	    from Orders o WITH (NOLOCK) 
	    inner join Order_SizeCode osc WITH (NOLOCK) on osc.Id = o.ID 
        where o.POID = @ID
)s
order by SizeGroup,seq
  

DECLARE @sql NVARCHAR(MAX)
SET @sql = N'
;with SortBy as (
      select oq.Article
             , RowNo = Row_Number() over (order by oq.Article)
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
      where o.POID = '''+@ID+'''      
	  group by oq.Article
),
tmpData as (
      select oq.Article
             , iif(o.junk = 1 , '''' ,oq.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oq.Qty) as Qty
             , sb.RowNo
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
      inner join SortBy sb on oq.Article = sb.Article
      where o.POID = '''+@ID+'''  
),
SubTotal as (
    select ''TTL'' as Article
           , SizeCode
           , SUM(Qty) as Qty
           , 9999 as RowNo
    from tmpData
    group by SizeCode
),
UnionData as (
      select * from tmpData
      union all
      select * from SubTotal
),
pivotData as (
      select *
      from UnionData
      pivot( 
        sum(Qty)   for SizeCode in ('+@cols+')
      ) a
)
select (select sum(Qty) from UnionData where Article = p.Article) as TotalQty
       , [Colorway] = p.Article
       , '+@cols+'
from pivotData p
order by RowNo'

EXEC sp_executesql @sql", this.poID);
                DBProxy.Current.Select(null, sqlcmd3, out ptb3);

                string sqlcmd4 = string.Format(
                    @"
DECLARE @ID nvarchar(20) = '{0}'
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(SizeCode),N',' + QUOTENAME(SizeCode))
FROM (
   select distinct oqd.SizeCode,osc.Seq,SizeGroup = iif(osc.SizeGroup = 'N', '', osc.SizeGroup)
   from Orders o WITH (NOLOCK) 
    inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
    inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID and oq.Seq = oqd.Seq
    inner join Order_SizeCode osc WITH (NOLOCK) on osc.Id = oq.ID and osc.SizeCode = oqd.SizeCode
  where o.POID = @ID
)s
order by SizeGroup,seq

DECLARE @sql NVARCHAR(MAX)
SET @sql = N'
;with SortBy as (
    select oqd.Article
           , RowNo = Row_Number() over (order by oqd.Article)
    from Orders o WITH (NOLOCK) 
    inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
    inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID 
                                                         and oq.Seq = oqd.Seq
    where o.POID = '''+@ID+'''
    group by oqd.Article
),
tmpData as (
    select oq.BuyerDelivery
           , oqd.Article
           , iif(o.junk = 1 , '''' ,oqd.SizeCode) as SizeCode
           , iif(o.junk = 1 , 0 ,oqd.Qty) as Qty
           , DENSE_RANK() OVER (ORDER BY oq.BuyerDelivery) as rnk
           , sb.RowNo
    from Orders o WITH (NOLOCK) 
    inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
    inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID 
                                                         and oq.Seq = oqd.Seq
    inner join SortBy sb on oqd.Article = sb.Article
    where o.POID = '''+@ID+''' 
),
SubTotal as (
    select null as BuyerDelivery
           , ''TTL'' as Article
           , SizeCode
           , SUM(Qty) as Qty
           , 99999 as rnk
           , 99999 as RowNo
    from tmpData
    group by SizeCode
),
UnionData as (
    select * from tmpData
    union all
    select * from SubTotal
),
pivotData as (
    select *
    from UnionData
    pivot( 
      sum(Qty) for SizeCode in ('+@cols+')
    ) a
)
select (select sum(isnull(Qty,0)) from UnionData where rnk = p.rnk and Article = p.Article) as TotalQty
       , [Buyer Delivery] = P.BuyerDelivery
       , [Colorway] = p.Article
       , '+@cols+'
from pivotData p
order by rnk, RowNo'

EXEC sp_executesql @sql", this.poID);
                DBProxy.Current.Select(null, sqlcmd4, out ptb4);
                #endregion
            }
            else
            {
                #region
                string sqlcmd1 = string.Format(
                    @"
DECLARE @ID nvarchar(20) = '{0}'
DECLARE @POID nvarchar(20) = '{1}'
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(SizeCode),N',' + QUOTENAME(SizeCode))
FROM (
      	select distinct osc.SizeCode,osc.Seq,SizeGroup = iif(osc.SizeGroup = 'N', '', osc.SizeGroup)
	    from Orders o WITH (NOLOCK) 
	    inner join Order_SizeCode osc WITH (NOLOCK) on osc.Id = o.ID 
        where o.ID = @POID 
)a
order by SizeGroup,seq

print @cols

DECLARE @sql NVARCHAR(MAX)
SET @sql = N'
;with SortBy as (
    select oq.Article
           , RowNo = Row_Number() over (order by oq.Article)
    from Order_Qty oq WITH (NOLOCK) 
    where oq.ID = '''+@ID+'''
    group by oq.Article
), 
tmpData as (
    select oq.Article
           , oq.SizeCode
           , oq.OriQty
           , sb.RowNo
    from Order_Qty oq WITH (NOLOCK) 
    inner join SortBy sb on oq.Article = sb.Article
    where oq.ID = '''+@ID+'''

),
SubTotal as (
    select ''TTL'' as Article
           , SizeCode
           , SUM(OriQty) as Qty
           , 9999 as RowNo
    from tmpData
    group by SizeCode
),UnionData as (
    select * from tmpData
    union all
    select * from SubTotal
),pivotData as (
    select *
      from UnionData
      pivot( 
        sum(OriQty) for SizeCode in ('+@cols+')
    ) a
)
select (select sum(OriQty) from UnionData where Article = p.Article) as TotalQty
       , [Colorway] = p.Article
       , '+@cols+'
from pivotData p
order by RowNo'

EXEC sp_executesql @sql", this.orderID, this.poID);
                DBProxy.Current.Select(null, sqlcmd1, out ptb1);

                string sqlcmd2 = string.Format(
                    @"
DECLARE @ID nvarchar(20) = '{0}'
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(SizeCode),N',' + QUOTENAME(SizeCode))
FROM (
  	select distinct osc.SizeCode,osc.Seq,SizeGroup = iif(osc.SizeGroup = 'N', '', osc.SizeGroup)
	from Orders o WITH (NOLOCK) 
	inner join Order_SizeCode osc WITH (NOLOCK) on osc.Id = o.ID 
    where o.POID = @ID
)s
order by SizeGroup,seq

DECLARE @sql NVARCHAR(MAX)
SET @sql = N'
;with SortBy as (
    select oq.Article
           , RowNo = Row_Number() over (order by oq.Article)
    from Orders o WITH (NOLOCK) 
    inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
    where o.POID = '''+@ID+'''
    group by oq.Article
),
tmpData as (
    select o.ID
           , oq.Article
           , Order_ColorCombo.ColorID
           , c.Alias
		   , o.CustCDID
		   , CustCD.Kit
		   ,o.CustPONo
		   ,[SpecialField] = o.Customize1
           , iif(o.junk = 1 , '''' ,oq.SizeCode) as SizeCode
           , iif(o.junk = 1 , 0 ,oq.OriQty) as OriQty 
           , DENSE_RANK() OVER (ORDER BY o.ID) as rnk
           , sb.RowNo
           , BuyerDelivery = '''''''' + Convert (varchar(10), o.BuyerDelivery)
    from Orders o WITH (NOLOCK) 
    inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
    inner join SortBy sb on oq.Article = sb.Article
    left join Country c WITH (NOLOCK) on c.ID = o.Dest
	left join CustCD WITH (NOLOCK) on CustCD.BrandID  = o.BrandID  and CustCD.ID = o.CustCDID
    outer apply (
			select top 1 ColorID
			from Order_ColorCombo occ WITH (NOLOCK)
			where occ.ID = o.Poid and occ.Article = oq.Article and occ.Patternpanel = ''FA''
		) Order_ColorCombo	
    where o.POID = '''+@ID+'''  
),
SubTotal as (
    select '''' as ID
           , ''TTL'' as Article
           , '''' as ColorID
           , Alias = ''''
		   , CustCDID = ''''
		   , Kit = ''''
		   ,CustPONo = ''''
		   ,[SpecialField] = ''''
           , SizeCode
           , SUM(OriQty) as Qty
           , 99999 as rnk
           , 99999 as RowNo
           , null as BuyerDelivery
    from tmpData
    group by SizeCode
),UnionData as (
    select * from tmpData
    union all
    select * from SubTotal
),pivotData as (
    select *
    from UnionData
    pivot( 
      sum(OriQty)  for SizeCode in ('+@cols+')
    ) a
)
select (select sum(OriQty) from UnionData where ID = p.ID and Article = p.Article) as TotalQty
       , [Sp#] = ID
       , [Colorway] = p.Article
       , Color = p.ColorID
       , Destination = Alias
	   , CustCD = CustCDID
	   , Kit# = Kit
       , '+@cols+'
       , [Buyer Delivery] = p.BuyerDelivery
	   ,[PO No.] = p.CustPONo
	   ,[SpecialField] = p.SpecialField
from pivotData p
order by rnk, RowNo'

EXEC sp_executesql @sql", this.poID);
                DBProxy.Current.Select(null, sqlcmd2, out ptb2);

                string sqlcmd3 = string.Format(
                    @"
DECLARE @ID nvarchar(20) = '{0}'
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(SizeCode),N',' + QUOTENAME(SizeCode))
FROM (
  	select distinct osc.SizeCode,osc.Seq,SizeGroup = iif(osc.SizeGroup = 'N', '', osc.SizeGroup)
	from Orders o WITH (NOLOCK) 
	inner join Order_SizeCode osc WITH (NOLOCK) on osc.Id = o.ID 
    where o.POID = @ID
)s
order by SizeGroup,seq

DECLARE @sql NVARCHAR(MAX)
SET @sql = N'
;with SortBy as (
    select oq.Article
           , RowNo = Row_Number() over (order by oq.Article)
    from Orders o WITH (NOLOCK) 
    inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
    where o.POID = '''+@ID+'''
    group by oq.Article
),
tmpData as (
    select oq.Article
           , iif(o.junk = 1 , '''' ,oq.SizeCode) as SizeCode
           , iif(o.junk = 1 , 0 ,oq.OriQty) as OriQty
           , sb.RowNo
    from Orders o WITH (NOLOCK) 
    inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
    inner join SortBy sb on oq.Article = sb.Article
    where o.POID = '''+@ID+'''   
),
SubTotal as (
    select ''TTL'' as Article
           , SizeCode
           , SUM(OriQty) as Qty
           , 9999 as RowNo
    from tmpData
    group by SizeCode
),UnionData as (
    select * from tmpData
    union all
    select * from SubTotal
),pivotData as (
    select *
    from UnionData
    pivot( 
      sum(OriQty)  for SizeCode in ('+@cols+')
    ) a
)
select (select sum(OriQty) from UnionData where Article = p.Article) as TotalQty
       , [Colorway] = p.Article
       , '+@cols+'
from pivotData p
order by RowNo'

EXEC sp_executesql @sql", this.poID);
                DBProxy.Current.Select(null, sqlcmd3, out ptb3);

                string sqlcmd4 = string.Format(
                    @"
DECLARE @ID nvarchar(20) = '{0}'
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(SizeCode),N',' + QUOTENAME(SizeCode))
FROM (
    select distinct oqd.SizeCode,osc.Seq,SizeGroup = iif(osc.SizeGroup = 'N', '', osc.SizeGroup)
    from Orders o WITH (NOLOCK) 
    inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
    inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID and oq.Seq = oqd.Seq
    inner join Order_SizeCode osc WITH (NOLOCK) on osc.Id = oq.ID and osc.SizeCode = oqd.SizeCode
  where o.POID = @ID
)s
order by SizeGroup,seq

DECLARE @sql NVARCHAR(MAX)
SET @sql = N'
;with SortBy as (
    select oqd.Article
           , RowNo = Row_Number() over (order by oqd.Article)
    from Orders o WITH (NOLOCK) 
    inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
    inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID 
                                                        and oq.Seq = oqd.Seq
    where o.POID = '''+@ID+'''
    group by oqd.Article
),
tmpData as (
    select oq.BuyerDelivery
           , oqd.Article
           , iif(o.junk = 1 , '''' ,oqd.SizeCode) as SizeCode
           , iif(o.junk = 1 , 0 ,oqd.OriQty) as OriQty
           , DENSE_RANK() OVER (ORDER BY oq.BuyerDelivery) as rnk
           , sb.RowNo
    from Orders o WITH (NOLOCK) 
    inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
    inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID 
                                                         and oq.Seq = oqd.Seq
    inner join SortBy sb on oqd.Article = sb.Article
    where o.POID = '''+@ID+'''  
),
SubTotal as (
    select null as BuyerDelivery
           , ''TTL'' as Article
           , SizeCode
           , SUM(OriQty) as Qty
           , 99999 as rnk           
           , 99999 as RowNo
    from tmpData
    group by SizeCode
),
UnionData as (
    select * from tmpData
    union all
    select * from SubTotal
),
pivotData as (
    select *
    from UnionData
    pivot( 
      sum(OriQty)  for SizeCode in ('+@cols+')
    ) a
)
select (select sum(isnull(OriQty,0)) from UnionData where rnk = p.rnk and Article = p.Article) as TotalQty
       , [Buyer Delivery] = P.BuyerDelivery
       , [Colorway] = p.Article
       , '+@cols+'
from pivotData p
order by rnk, RowNo'

EXEC sp_executesql @sql", this.poID);
                DBProxy.Current.Select(null, sqlcmd4, out ptb4);
                #endregion
            }

            string sqlcmd = $@"
            select 
            b.Customize1
            from orders o with(nolock)
            left join brand b with(nolock) on o.BrandID = b.ID
            where o.id = '{this.orderID}'";
            string columnName = MyUtility.GetValue.Lookup(sqlcmd);
            if (ptb2 != null)
            {
                if (columnName == string.Empty)
                {
                    ptb2.Columns.Remove("SpecialField");
                }
                else
                {
                    ptb2.Columns["SpecialField"].ColumnName = columnName;
                }
            }

            int columns1 = 0, columns2 = 0, columns3 = 0, columns4 = 0;
            if (ptb1 != null)
            {
                columns1 = ptb1.Columns.Count;
            }

            if (ptb2 != null)
            {
                columns2 = ptb2.Columns.Count;
            }

            if (ptb3 != null)
            {
                columns3 = ptb3.Columns.Count;
            }

            if (ptb4 != null)
            {
                columns4 = ptb4.Columns.Count;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\PPIC_P01_Qtybreakdown.xltx"); // 預先開啟excel app

            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];
            if (ptb1 != null && ptb1.Rows.Count > 0)
            {
                for (int i = 0; i < columns1; i++)
                {
                    objSheets.Cells[2, i + 1] = ptb1.Columns[i].ColumnName;

                    // 欄位Format：文字
                    if (ptb1.Columns[i].ColumnName == "Colorway")
                    {
                        objSheets.Columns[i + 1].NumberFormat = "@";
                    }
                }

                string r1 = MyUtility.Excel.ConvertNumericToExcelColumn(columns1);
                objSheets.get_Range("A1", r1 + "1").Merge(false);
                MyUtility.Excel.CopyToXls(ptb1, string.Empty, "PPIC_P01_Qtybreakdown.xltx", 2, false, null, objApp, wSheet: objSheets);
                objSheets.Cells[1, 1] = "Qty breakdown (" + this.orderID + ")";
                objSheets.get_Range("A1", r1 + "2").Interior.Color = Color.LightGreen;
                objSheets.get_Range("A2", r1 + "2").AutoFilter(1);
            }

            if (ptb2 != null && ptb2.Rows.Count > 0)
            {
                objSheets = objApp.ActiveWorkbook.Worksheets[2];
                for (int i = 0; i < columns2; i++)
                {
                    objSheets.Cells[3, i + 1] = ptb2.Columns[i].ColumnName;

                    // 欄位Format：文字
                    if (ptb2.Columns[i].ColumnName == "Sp#" ||
                        ptb2.Columns[i].ColumnName == "Colorway" ||
                        ptb2.Columns[i].ColumnName == "Color" ||
                        ptb2.Columns[i].ColumnName == "Destination" ||
                        ptb2.Columns[i].ColumnName == "CustCD" ||
                        ptb2.Columns[i].ColumnName == "Kit#")
                    {
                        objSheets.Columns[i + 1].NumberFormat = "@";
                    }

                    // 顯示日期
                    if (ptb2.Columns[i].ColumnName == "Buyer Delivery")
                    {
                        objSheets.Columns[i + 1].NumberFormat = "yyyy/MM/dd";
                    }
                }

                string r2 = MyUtility.Excel.ConvertNumericToExcelColumn(columns2);
                objSheets.get_Range("A1", r2 + "1").Merge(false);
                objSheets.get_Range("A2", r2 + "2").Merge(false);
                MyUtility.Excel.CopyToXls(ptb2, string.Empty, "PPIC_P01_Qtybreakdown.xltx", 3, false, null, objApp, wSheet: objSheets);
                objSheets.Cells[1, 1] = "Qty breakdown (" + this.poID + ")";
                objSheets.Cells[2, 1] = "PO Combination :" + this.displaySPPOCombination.Text;
                objSheets.get_Range("A1", r2 + "3").Interior.Color = Color.LightGreen;
                objSheets.get_Range("A3", r2 + "3").AutoFilter(1);
            }

            if (ptb3 != null && ptb3.Rows.Count > 0)
            {
                objSheets = objApp.ActiveWorkbook.Worksheets[3];
                for (int i = 0; i < columns3; i++)
                {
                    objSheets.Cells[3, i + 1] = ptb3.Columns[i].ColumnName;

                    // 欄位Format：文字
                    if (ptb3.Columns[i].ColumnName == "Colorway")
                    {
                        objSheets.Columns[i + 1].NumberFormat = "@";
                    }
                }

                string r3 = MyUtility.Excel.ConvertNumericToExcelColumn(columns3);
                objSheets.get_Range("A1", r3 + "1").Merge(false);
                objSheets.get_Range("A2", r3 + "2").Merge(false);
                MyUtility.Excel.CopyToXls(ptb3, string.Empty, "PPIC_P01_Qtybreakdown.xltx", 3, false, null, objApp, wSheet: objSheets);
                objSheets.Cells[1, 1] = "Qty breakdown (" + this.poID + ")";
                objSheets.Cells[2, 1] = "PO Combination :" + this.displayColorwayPOCombination.Text;
                objSheets.get_Range("A1", r3 + "3").Interior.Color = Color.LightGreen;
                objSheets.get_Range("A3", r3 + "3").AutoFilter(1);
            }

            if (ptb4 != null && ptb4.Rows.Count > 0)
            {
                objSheets = objApp.ActiveWorkbook.Worksheets[4];
                for (int i = 0; i < columns4; i++)
                {
                    objSheets.Cells[3, i + 1] = ptb4.Columns[i].ColumnName;

                    // 顯示日期
                    if (ptb4.Columns[i].ColumnName == "Buyer Delivery")
                    {
                        objSheets.Columns[i + 1].NumberFormat = "yyyy/MM/dd";
                    }

                    // 欄位Format：文字
                    if (ptb4.Columns[i].ColumnName == "Colorway")
                    {
                        objSheets.Columns[i + 1].NumberFormat = "@";
                    }
                }

                string r4 = MyUtility.Excel.ConvertNumericToExcelColumn(columns4);
                objSheets.get_Range("A1", r4 + "1").Merge(false);
                objSheets.get_Range("A2", r4 + "2").Merge(false);
                MyUtility.Excel.CopyToXls(ptb4, string.Empty, "PPIC_P01_Qtybreakdown.xltx", 3, false, null, objApp, wSheet: objSheets);
                objSheets.Cells[1, 1] = "Qty breakdown (" + this.poID + ")";
                objSheets.Cells[2, 1] = "PO Combination :" + this.displayDeliveryPOCombination.Text;
                objSheets.get_Range("A1", r4 + "3").Interior.Color = Color.LightGreen;
                objSheets.get_Range("A3", r4 + "3").AutoFilter(1);
            }

            if (this.grid5Data != null && this.grid5Data.Rows.Count > 0)
            {
                objSheets = objApp.ActiveWorkbook.Worksheets[5];
                for (int i = 0; i < this.grid5Data.Columns.Count; i++)
                {
                    objSheets.Cells[2, i + 1] = this.grid5Data.Columns[i].ColumnName;

                    // 顯示日期
                    if (this.grid5Data.Columns[i].ColumnName == "Buyer Delivery")
                    {
                        objSheets.Columns[i + 1].NumberFormat = "yyyy/MM/dd";
                    }

                    // 欄位Format：文字
                    if (this.grid5Data.Columns[i].ColumnName == "Colorway" ||
                        this.grid5Data.Columns[i].ColumnName == "SP#" ||
                        this.grid5Data.Columns[i].ColumnName == "Garment SP#")
                    {
                        objSheets.Columns[i + 1].NumberFormat = "@";
                    }
                }

                string r5 = MyUtility.Excel.ConvertNumericToExcelColumn(this.grid5Data.Columns.Count);
                objSheets.get_Range("A1", r5 + "1").Merge(false);
                MyUtility.Excel.CopyToXls(this.grid5Data, string.Empty, "PPIC_P01_Qtybreakdown.xltx", 2, false, null, objApp, wSheet: objSheets);
                objSheets.Cells[1, 1] = "Qty breakdown (" + this.poID + ")";
                objSheets.Cells[1, 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                objSheets.Cells[1, 1].VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                objSheets.get_Range("A1", r5 + "2").Interior.Color = Color.LightGreen;
                objSheets.get_Range("A2", r5 + "2").AutoFilter(1);
            }

            if (this.grid6Data != null && this.grid6Data.Rows.Count > 0)
            {
                objSheets = objApp.ActiveWorkbook.Worksheets[6];
                for (int i = 0; i < this.grid6Data.Columns.Count; i++)
                {
                    objSheets.Cells[2, i + 1] = this.grid6Data.Columns[i].ColumnName;

                    // 顯示日期
                    if (this.grid6Data.Columns[i].ColumnName == "Buyer Delivery")
                    {
                        objSheets.Columns[i + 1].NumberFormat = "yyyy/MM/dd";
                    }

                    // 欄位Format：文字
                    if (this.grid6Data.Columns[i].ColumnName == "Colorway" ||
                        this.grid6Data.Columns[i].ColumnName == "SP#" ||
                        this.grid6Data.Columns[i].ColumnName == "From SP#")
                    {
                        objSheets.Columns[i + 1].NumberFormat = "@";
                    }
                }

                string r6 = MyUtility.Excel.ConvertNumericToExcelColumn(this.grid6Data.Columns.Count);
                objSheets.get_Range("A1", r6 + "1").Merge(false);
                MyUtility.Excel.CopyToXls(this.grid6Data, string.Empty, "PPIC_P01_Qtybreakdown.xltx", 2, false, null, objApp, wSheet: objSheets);
                objSheets.Cells[1, 1] = "Qty breakdown (" + this.poID + ")";
                objSheets.Cells[1, 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                objSheets.Cells[1, 1].VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                objSheets.get_Range("A1", r6 + "2").Interior.Color = Color.LightGreen;
                objSheets.get_Range("A2", r6 + "2").AutoFilter(1);
            }

            // 隱藏第五分頁
            if (!this.isTabPag5)
            {
                objSheets = objApp.ActiveWorkbook.Worksheets[5];
                objSheets.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
            }

            // 隱藏第六分頁
            if (!this.isTabPag6)
            {
                objSheets = objApp.ActiveWorkbook.Worksheets[6];
                objSheets.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;
            }

            for (int i = 1; i <= 4; i++)
            {
                objSheets = objApp.ActiveWorkbook.Worksheets[i];
                objSheets.Columns.AutoFit();
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("PPIC_P01_Qtybreakdown");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
        }

        private void GridQtyBDown_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            this.gridqtybdownTop.HorizontalScrollingOffset = e.NewValue;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            BindingSource bs = this.gridCombBySPNo.DataSource as BindingSource;
            DataTable dtCombBySPNo = bs?.DataSource as DataTable;
            P01_PrintFabricSticker frm = new P01_PrintFabricSticker(dtCombBySPNo);
            frm.ShowDialog();
        }
    }
}
