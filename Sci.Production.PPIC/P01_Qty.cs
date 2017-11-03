using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    public partial class P01_Qty : Sci.Win.Subs.Base
    {
        string orderID, poID,poCombo;
        DataTable grid1Data, grid2Data, grid3Data, grid4Data;  //存Qty資料
        DataTable grid1Data_OriQty, grid2Data_OriQty, grid3Data_OriQty, grid4Data_OriQty;  //存OriQty資料


        public P01_Qty(string OrderID, string POID, string POCombo)
        {
            InitializeComponent();
            orderID = OrderID;
            poID = POID;
            poCombo = POCombo;
            Text = Text + " (" + orderID + ")";
            displaySPPOCombination.Value = poCombo;
            displayColorwayPOCombination.Value = poCombo;
            displayDeliveryPOCombination.Value = poCombo;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            //撈出所有的Size
            string sqlCmd = string.Format("select * from Order_SizeCode WITH (NOLOCK) where ID = '{0}' order by Seq", poID);
            DataTable headerData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out headerData);
            StringBuilder pivot = new StringBuilder();

            //設定Grid1的顯示欄位
            this.gridQtyBDown.IsEditingReadOnly = true;
            this.gridQtyBDown.DataSource = listControlBindingSource1;
            var gen = Helper.Controls.Grid.Generator(this.gridQtyBDown);
            CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                    pivot.Append(string.Format("[{0}],", MyUtility.Convert.GetString(dr["SizeCode"])));
                }
            }
            //設定Grid2的顯示欄位
            this.gridCombBySPNo.IsEditingReadOnly = true;
            this.gridCombBySPNo.DataSource = listControlBindingSource2;
            gen = Helper.Controls.Grid.Generator(this.gridCombBySPNo);
            CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            CreateGrid(gen, "string", "ID", "SP#", Widths.AnsiChars(15));
            CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            CreateGrid(gen, "string", "ColorID", "Color", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                }
            }
            CreateGrid(gen, "string", "BuyerDelivery", "Buyer Delivery", Widths.AnsiChars(8));
            //設定Grid3的顯示欄位
            this.gridColorway.IsEditingReadOnly = true;
            this.gridColorway.DataSource = listControlBindingSource3;
            gen = Helper.Controls.Grid.Generator(this.gridColorway);
            CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                }
            }
            //設定Grid4的顯示欄位
            this.gridDelivery.IsEditingReadOnly = true;
            this.gridDelivery.DataSource = listControlBindingSource4;
            gen = Helper.Controls.Grid.Generator(this.gridDelivery);
            CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            CreateGrid(gen, "date", "BuyerDelivery", "Buyer Delivery", Widths.AnsiChars(10));
            CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                }
            }

            //凍結欄位
            gridQtyBDown.Columns[1].Frozen = true;
            gridCombBySPNo.Columns[2].Frozen = true;
            gridColorway.Columns[1].Frozen = true;
            gridDelivery.Columns[2].Frozen = true;

            #region 撈Grid1資料
            sqlCmd = string.Format(@"
with SortBy as (
      select oq.Article  
             , RowNo = ROW_NUMBER() over (order by oq.Article) 
      from Order_Qty oq WITH (NOLOCK) 
      where oq.ID = '{0}'
      group by oq.Article
),
tmpData as (
      select oq.Article
             , iif(o.junk = 1 , '' ,oq.SizeCode) as SizeCode 
             , iif(o.junk = 1 , 0 ,oq.Qty) as Qty  
             , sb.RowNo
      from Order_Qty oq WITH (NOLOCK) 
      inner join orders o WITH (NOLOCK) on o.ID = oq.ID
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
order by RowNo", orderID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid1Data);

            sqlCmd = string.Format(@"
with SortBy as (
      select oq.Article
             , RowNo = ROW_NUMBER() over (order by oq.Article)
      from Order_Qty oq WITH (NOLOCK) 
      where oq.ID = '{0}'
      group by oq.Article
),
tmpData as (
      select oq.Article
             , iif(o.junk = 1 , '' ,oq.SizeCode) as SizeCode 
             , iif(o.junk = 1 , 0 ,oq.OriQty) as OriQty
             , sb.RowNo
      from Order_Qty oq WITH (NOLOCK) 
      inner join orders o WITH (NOLOCK) on o.ID = oq.ID 
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
order by RowNo", orderID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid1Data_OriQty);
            #endregion  

            #region 撈Grid2資料
            sqlCmd = string.Format(@"
with tmpData as (
      select o.ID
             , oq.Article
             , occ.ColorID
             , iif(o.junk = 1 , '' ,oq.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oq.Qty) as Qty 
             , DENSE_RANK() OVER (ORDER BY o.ID) as rnk
             , BuyerDelivery = o.BuyerDelivery
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
      left join Order_ColorCombo occ With (NoLock) on occ.ID = o.Poid
                                                      and occ.Article = oq.Article
                                                      and occ.Patternpanel = 'FA'
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
order by rnk", poID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid2Data);

            sqlCmd = string.Format(@"
with tmpData as (
      select o.ID
             , oq.Article
             , occ.ColorID
             , iif(o.junk = 1 , '' ,oq.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oq.OriQty) as OriQty
             , DENSE_RANK() OVER (ORDER BY o.ID) as rnk
             , BuyerDelivery = o.BuyerDelivery
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
      left join Order_ColorCombo occ With (NoLock) on occ.ID = o.Poid
                                                      and occ.Article = oq.Article
                                                      and occ.Patternpanel = 'FA'
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
order by rnk", poID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid2Data_OriQty);
            #endregion

            #region 撈Grid3資料
            sqlCmd = string.Format(@"
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
order by RowNo", poID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid3Data);

            sqlCmd = string.Format(@"
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
order by RowNo", poID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid3Data_OriQty);
            #endregion

            #region 撈Grid4資料
            sqlCmd = string.Format(@"
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
order by rnk, RowNo", poID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid4Data);

            sqlCmd = string.Format(@"
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
order by rnk, RowNo", poID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid4Data_OriQty);
            #endregion

            listControlBindingSource1.DataSource = grid1Data;
            listControlBindingSource2.DataSource = grid2Data;
            listControlBindingSource3.DataSource = grid3Data;
            listControlBindingSource4.DataSource = grid4Data;
        }

        public void CreateGrid(IDataGridViewGenerator gen, string datatype, string propname, string header, IWidth width)
        {
            CreateGridCol(gen, datatype
                , propname: propname
                , header: header
                , width: width
            );
        }

        private void CreateGridCol(IDataGridViewGenerator gen, string datatype
            , string propname = null, string header = null, IWidth width = null, bool? iseditingreadonly = null
            , int index = -1)
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

        private void radioOriQty_CheckedChanged(object sender, EventArgs e)
        {
            if (radioQty.Checked)
            {
                listControlBindingSource1.DataSource = grid1Data;
                listControlBindingSource2.DataSource = grid2Data;
                listControlBindingSource3.DataSource = grid3Data;
                listControlBindingSource4.DataSource = grid4Data;
            }
            else if (radioOriQty.Checked)
            {
                listControlBindingSource1.DataSource = grid1Data_OriQty;
                listControlBindingSource2.DataSource = grid2Data_OriQty;
                listControlBindingSource3.DataSource = grid3Data_OriQty;
                listControlBindingSource4.DataSource = grid4Data_OriQty;
            }
        }

        private void ToExcel_Click(object sender, EventArgs e)
        {
            DataTable ptb1, ptb2, ptb3, ptb4;
            if (radioQty.Checked)
            {
                #region
                string sqlcmd1 = string.Format(@"
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
             , iif(o.junk = 1 , '''' ,oq.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oq.Qty) as Qty
             , sb.RowNo
      from Order_Qty oq WITH (NOLOCK) 
      inner join orders o WITH (NOLOCK) on o.ID = oq.ID
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

EXEC sp_executesql @sql", orderID);
                DBProxy.Current.Select(null, sqlcmd1, out ptb1);

                string sqlcmd2 = string.Format(@"
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
             , occ.ColorID
             , iif(o.junk = 1 , '''' ,oq.SizeCode) as SizeCode
             , iif(o.junk = 1 , 0 ,oq.Qty) as Qty
             , DENSE_RANK() OVER (ORDER BY o.ID) as rnk
             , sb.RowNo
             , BuyerDelivery = '''''''' + Convert (varchar(10), o.BuyerDelivery)
      from Orders o WITH (NOLOCK) 
      inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
      inner join SortBy sb on oq.Article = sb.Article
      left join Order_ColorCombo occ With (NoLock) on occ.ID = o.Poid
                                                      and occ.Article = oq.Article
                                                      and occ.Patternpanel = ''FA''    
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

EXEC sp_executesql @sql", poID);
                DBProxy.Current.Select(null, sqlcmd2, out ptb2);

                string sqlcmd3 = string.Format(@"
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

EXEC sp_executesql @sql", poID);
                DBProxy.Current.Select(null, sqlcmd3, out ptb3);

                string sqlcmd4 = string.Format(@"
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

EXEC sp_executesql @sql", poID);
                DBProxy.Current.Select(null, sqlcmd4, out ptb4);
                #endregion
            }
            else
            {
                #region
                string sqlcmd1 = string.Format(@"
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
           , iif(o.junk = 1 , '''' ,oq.SizeCode) as SizeCode
           , iif(o.junk = 1 , 0 ,oq.OriQty) as OriQty
           , sb.RowNo
    from Order_Qty oq WITH (NOLOCK) 
    inner join orders o WITH (NOLOCK) on o.ID = oq.ID
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

EXEC sp_executesql @sql", orderID);
                DBProxy.Current.Select(null, sqlcmd1, out ptb1);

                string sqlcmd2 = string.Format(@"
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
           , occ.ColorID
           , iif(o.junk = 1 , '''' ,oq.SizeCode) as SizeCode
           , iif(o.junk = 1 , 0 ,oq.OriQty) as OriQty 
           , DENSE_RANK() OVER (ORDER BY o.ID) as rnk
           , sb.RowNo
           , BuyerDelivery = '''''''' + Convert (varchar(10), o.BuyerDelivery)
    from Orders o WITH (NOLOCK) 
    inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
    inner join SortBy sb on oq.Article = sb.Article
    left join Order_ColorCombo occ With (NoLock) on occ.ID = o.Poid
                                                    and occ.Article = oq.Article
                                                    and occ.Patternpanel = ''FA''   
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

EXEC sp_executesql @sql", poID);
                DBProxy.Current.Select(null, sqlcmd2, out ptb2);

                string sqlcmd3 = string.Format(@"
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

EXEC sp_executesql @sql", poID);
                DBProxy.Current.Select(null, sqlcmd3, out ptb3);

                string sqlcmd4 = string.Format(@"
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

EXEC sp_executesql @sql", poID);
                DBProxy.Current.Select(null, sqlcmd4, out ptb4);
                #endregion
            }
            int columns1 = 0,columns2 = 0,columns3 = 0,columns4 = 0;
            if (ptb1 != null) columns1 = ptb1.Columns.Count;
            if (ptb2 != null) columns2 = ptb2.Columns.Count;
            if (ptb3 != null) columns3 = ptb3.Columns.Count;
            if (ptb4 != null) columns4 = ptb4.Columns.Count;
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\PPIC_P01_Qtybreakdown.xltx"); //預先開啟excel app

            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];
            if (ptb1 != null && ptb1.Rows.Count > 0)
            {
                for (int i = 0; i < columns1; i++)
                {
                    objSheets.Cells[2, i + 1] = ptb1.Columns[i].ColumnName;
                }
                string r1 = MyUtility.Excel.ConvertNumericToExcelColumn(columns1);
                objSheets.get_Range("A1", r1 + "1").Merge(false);
                MyUtility.Excel.CopyToXls(ptb1, "", "PPIC_P01_Qtybreakdown.xltx", 2, false, null, objApp, wSheet: objSheets);
                objSheets.Cells[1, 1] = "Qty breakdown (" + orderID + ")";
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
                MyUtility.Excel.CopyToXls(ptb2, "", "PPIC_P01_Qtybreakdown.xltx", 3, false, null, objApp, wSheet: objSheets);
                objSheets.Cells[1, 1] = "Qty breakdown (" + poID + ")";
                objSheets.Cells[2, 1] = "PO Combination :" + displaySPPOCombination.Text;
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
                MyUtility.Excel.CopyToXls(ptb3, "", "PPIC_P01_Qtybreakdown.xltx", 3, false, null, objApp, wSheet: objSheets);
                objSheets.Cells[1, 1] = "Qty breakdown (" + poID + ")";
                objSheets.Cells[2, 1] = "PO Combination :" + displayColorwayPOCombination.Text;
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
                MyUtility.Excel.CopyToXls(ptb4, "", "PPIC_P01_Qtybreakdown.xltx", 3, false, null, objApp, wSheet: objSheets);
                objSheets.Cells[1, 1] = "Qty breakdown (" + poID + ")";
                objSheets.Cells[2, 1] = "PO Combination :" + displayDeliveryPOCombination.Text;
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
