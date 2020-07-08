using System;
using System.Data;
using System.Drawing;
using System.Text;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_Qty
    /// </summary>
    public partial class P01_Qty : Win.Subs.Base
    {
        private string orderID;
        private string poID;
        private string poCombo;
        private DataTable grid1Data;
        private DataTable grid2Data;
        private DataTable grid3Data;
        private DataTable grid4Data;  // 存Qty資料
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
        public P01_Qty(string orderID, string pOID, string pOCombo)
        {
            this.InitializeComponent();
            this.orderID = orderID;
            this.poID = pOID;
            this.poCombo = pOCombo;
            this.Text = this.Text + " (" + this.orderID + ")";
            this.displaySPPOCombination.Value = this.poCombo;
            this.displayColorwayPOCombination.Value = this.poCombo;
            this.displayDeliveryPOCombination.Value = this.poCombo;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // 撈出所有的Size
            string sqlCmd = string.Format("select * from Order_SizeCode WITH (NOLOCK) where ID = '{0}' order by Seq", this.poID);
            DataTable headerData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out headerData);
            StringBuilder pivot = new StringBuilder();

            // 設定Grid1的顯示欄位
            this.gridQtyBDown.IsEditingReadOnly = true;
            this.gridQtyBDown.DataSource = this.listControlBindingSource1;
            var gen = this.Helper.Controls.Grid.Generator(this.gridQtyBDown);
            this.CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            this.CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    this.CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                    pivot.Append(string.Format("[{0}],", MyUtility.Convert.GetString(dr["SizeCode"])));
                }
            }

            // 設定Grid2的顯示欄位
            this.gridCombBySPNo.IsEditingReadOnly = true;
            this.gridCombBySPNo.DataSource = this.listControlBindingSource2;
            gen = this.Helper.Controls.Grid.Generator(this.gridCombBySPNo);
            this.CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            this.CreateGrid(gen, "string", "ID", "SP#", Widths.AnsiChars(15));
            this.CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            this.CreateGrid(gen, "string", "ColorID", "Color", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    this.CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                }
            }

            this.CreateGrid(gen, "string", "BuyerDelivery", "Buyer Delivery", Widths.AnsiChars(8));

            // 設定Grid3的顯示欄位
            this.gridColorway.IsEditingReadOnly = true;
            this.gridColorway.DataSource = this.listControlBindingSource3;
            gen = this.Helper.Controls.Grid.Generator(this.gridColorway);
            this.CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            this.CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
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
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    this.CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                }
            }

            // 凍結欄位
            this.gridQtyBDown.Columns[1].Frozen = true;
            this.gridCombBySPNo.Columns[2].Frozen = true;
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
             , oq.SizeCode
             , oq.Qty
             , sb.RowNo
      from Order_Qty oq WITH (NOLOCK) 
      inner join SortBy sb on oq.Article = sb.Article
      where oq.ID = '{0}'
),
SubTotal as (
      select 'TTL' as Article
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
             , oq.SizeCode
             , oq.OriQty
             , sb.RowNo
      from Order_Qty oq WITH (NOLOCK) 
      inner join SortBy sb on oq.Article = sb.Article
      where oq.ID = '{0}'
),
SubTotal as (
      select 'TTL' as Article
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
             , Order_ColorCombo.ColorID
             , iif(o.junk = 1 , '' ,oq.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oq.Qty) as Qty 
             , DENSE_RANK() OVER (ORDER BY o.ID) as rnk
             , BuyerDelivery = o.BuyerDelivery
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
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
             , ColorID = ''
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
             , Order_ColorCombo.ColorID
             , iif(o.junk = 1 , '' ,oq.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oq.OriQty) as OriQty
             , DENSE_RANK() OVER (ORDER BY o.ID) as rnk
             , BuyerDelivery = o.BuyerDelivery
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
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
             , ColorID = ''
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
             , iif(o.junk = 1 , '' ,oq.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oq.Qty) as Qty 
             , sb.RowNo
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
      inner join SortBy sb on oq.Article = sb.Article
      where o.POID = '{0}' 
),
SubTotal as (
      select 'TTL' as Article,SizeCode,SUM(Qty) as Qty
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
             , iif(o.junk = 1 , '' ,oq.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oq.OriQty) as OriQty
             , sb.RowNo
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID      
      inner join SortBy sb on oq.Article = sb.Article
      where o.POID = '{0}' 
),
SubTotal as (
      select 'TTL' as Article
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
             , iif(o.junk = 1 , '' ,oqd.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oqd.Qty) as Qty
             , DENSE_RANK() OVER (ORDER BY oq.BuyerDelivery) as rnk
             , sb.RowNo
      from Orders o WITH (NOLOCK) 
      inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
      inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID and oq.Seq = oqd.Seq
      inner join SortBy sb on oqd.Article = sb.Article
      where o.POID = '{0}' 
),
SubTotal as (
      select null as BuyerDelivery
             , 'TTL' as Article
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
             , iif(o.junk = 1 , '' ,oqd.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oqd.OriQty) as OriQty
             , DENSE_RANK() OVER (ORDER BY oq.BuyerDelivery) as rnk
             , sb.RowNo
      from Orders o WITH (NOLOCK) 
      inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
      inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID and oq.Seq = oqd.Seq
      inner join SortBy sb on oqd.Article = sb.Article
      where o.POID = '{0}'  
),
SubTotal as (
      select null as BuyerDelivery
             , 'TTL' as Article
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
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(SizeCode),N',' + QUOTENAME(SizeCode))
FROM (
      select distinct oq.SizeCode
      FROM Order_Qty oq WITH (NOLOCK) 
      where oq.ID = @ID
)a
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

EXEC sp_executesql @sql", this.orderID);
                DBProxy.Current.Select(null, sqlcmd1, out ptb1);

                string sqlcmd2 = string.Format(
                    @"
DECLARE @ID nvarchar(20) = '{0}'
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(SizeCode),N',' + QUOTENAME(SizeCode))
FROM (
      select distinct oq.SizeCode
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
      where o.POID = @ID
)s

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
             , iif(o.junk = 1 , '''' ,oq.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oq.Qty) as Qty
             , DENSE_RANK() OVER (ORDER BY o.ID) as rnk
             , sb.RowNo
             , BuyerDelivery = '''''''' + Convert (varchar(10), o.BuyerDelivery)
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
      inner join SortBy sb on oq.Article = sb.Article
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
       , '+@cols+'
       , [Buyer Delivery] = p.BuyerDelivery
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
      select distinct oq.SizeCode
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
      where o.POID = @ID
)s

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
  select distinct oqd.SizeCode
   from Orders o WITH (NOLOCK) 
    inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
    inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID and oq.Seq = oqd.Seq
  where o.POID = @ID
)s

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
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(SizeCode),N',' + QUOTENAME(SizeCode))
FROM (
  select distinct oq.SizeCode
  FROM Order_Qty oq WITH (NOLOCK) 
  where oq.ID = @ID
)a
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

EXEC sp_executesql @sql", this.orderID);
                DBProxy.Current.Select(null, sqlcmd1, out ptb1);

                string sqlcmd2 = string.Format(
                    @"
DECLARE @ID nvarchar(20) = '{0}'
DECLARE @cols NVARCHAR(MAX)= N''
SELECT @cols = @cols + iif(@cols = N'',QUOTENAME(SizeCode),N',' + QUOTENAME(SizeCode))
FROM (
  select distinct oq.SizeCode
  from Orders o WITH (NOLOCK) 
  inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
  where o.POID = @ID
)s

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
           , iif(o.junk = 1 , '''' ,oq.SizeCode) as SizeCode
           , iif(o.junk = 1 , 0 ,oq.OriQty) as OriQty 
           , DENSE_RANK() OVER (ORDER BY o.ID) as rnk
           , sb.RowNo
           , BuyerDelivery = '''''''' + Convert (varchar(10), o.BuyerDelivery)
    from Orders o WITH (NOLOCK) 
    inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
    inner join SortBy sb on oq.Article = sb.Article
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
       , '+@cols+'
       , [Buyer Delivery] = p.BuyerDelivery
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
  select distinct oq.SizeCode
  from Orders o WITH (NOLOCK) 
  inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
  where o.POID = @ID
)s

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
  select distinct oqd.SizeCode
   from Orders o WITH (NOLOCK) 
    inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
    inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID and oq.Seq = oqd.Seq
  where o.POID = @ID
)s

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

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\PPIC_P01_Qtybreakdown.xltx"); // 預先開啟excel app

            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];
            if (ptb1 != null && ptb1.Rows.Count > 0)
            {
                for (int i = 0; i < columns1; i++)
                {
                    objSheets.Cells[2, i + 1] = ptb1.Columns[i].ColumnName;
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

            for (int i = 1; i <= 4; i++)
            {
                objSheets = objApp.ActiveWorkbook.Worksheets[i];
                objSheets.Columns.AutoFit();
            }

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_P01_Qtybreakdown");
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
    }
}
