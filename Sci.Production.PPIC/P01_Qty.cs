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
            displayBox1.Value = poCombo;
            displayBox2.Value = poCombo;
            displayBox3.Value = poCombo;
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
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            var gen = Helper.Controls.Grid.Generator(this.grid1);
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
            this.grid2.IsEditingReadOnly = true;
            this.grid2.DataSource = listControlBindingSource2;
            gen = Helper.Controls.Grid.Generator(this.grid2);
            CreateGrid(gen, "int", "TotalQty", "Total", Widths.AnsiChars(6));
            CreateGrid(gen, "string", "ID", "SP#", Widths.AnsiChars(15));
            CreateGrid(gen, "string", "Article", "Colorway", Widths.AnsiChars(8));
            if (headerData != null && headerData.Rows.Count > 0)
            {
                foreach (DataRow dr in headerData.Rows)
                {
                    CreateGrid(gen, "int", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["SizeCode"]), Widths.AnsiChars(8));
                }
            }
            //設定Grid3的顯示欄位
            this.grid3.IsEditingReadOnly = true;
            this.grid3.DataSource = listControlBindingSource3;
            gen = Helper.Controls.Grid.Generator(this.grid3);
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
            this.grid4.IsEditingReadOnly = true;
            this.grid4.DataSource = listControlBindingSource4;
            gen = Helper.Controls.Grid.Generator(this.grid4);
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
            grid1.Columns[1].Frozen = true;
            grid2.Columns[2].Frozen = true;
            grid3.Columns[1].Frozen = true;
            grid4.Columns[2].Frozen = true;

            #region 撈Grid1資料
            sqlCmd = string.Format(@"with tmpData
            as (
            select oq.Article,oq.SizeCode,oq.Qty,oa.Seq
            from Order_Qty oq WITH (NOLOCK) 
            left join Order_Article oa WITH (NOLOCK) on oa.ID = oq.ID and oa.Article = oq.Article
            where oq.ID = '{0}'
            ),
            SubTotal
            as (
            select 'TTL' as Article,SizeCode,SUM(Qty) as Qty, '9999' as Seq
            from tmpData
            group by SizeCode
            ),
            UnionData
            as (
            select * from tmpData
            union all
            select * from SubTotal
            ),
            pivotData
            as (
            select *
            from UnionData
            pivot( sum(Qty)
            for SizeCode in ({1})
            ) a
            )
            select *,(select sum(Qty) from UnionData where Article = p.Article) as TotalQty
            from pivotData p
            order by Seq", orderID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid1Data);

            sqlCmd = string.Format(@"with tmpData
            as (
            select oq.Article,oq.SizeCode,oq.OriQty,oa.Seq
            from Order_Qty oq WITH (NOLOCK) 
            left join Order_Article oa WITH (NOLOCK) on oa.ID = oq.ID and oa.Article = oq.Article
            where oq.ID = '{0}'
            ),
            SubTotal
            as (
            select 'TTL' as Article,SizeCode,SUM(OriQty) as Qty, '9999' as Seq
            from tmpData
            group by SizeCode
            ),
            UnionData
            as (
            select * from tmpData
            union all
            select * from SubTotal
            ),
            pivotData
            as (
            select *
            from UnionData
            pivot( sum(OriQty)
            for SizeCode in ({1})
            ) a
            )
            select *,(select sum(OriQty) from UnionData where Article = p.Article) as TotalQty
            from pivotData p
            order by Seq", orderID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid1Data_OriQty);
            #endregion  

            #region 撈Grid2資料
            sqlCmd = string.Format(@"with tmpData
            as (
            select o.ID,oq.Article,oq.SizeCode,oq.Qty,oa.Seq,DENSE_RANK() OVER (ORDER BY o.ID) as rnk
            from Orders o WITH (NOLOCK) 
            inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
            left join Order_Article oa WITH (NOLOCK) on oa.ID = oq.ID and oa.Article = oq.Article
            where o.POID = '{0}'
            ),
            SubTotal
            as (
            select '' as ID,'TTL' as Article,SizeCode,SUM(Qty) as Qty, '9999' as Seq,99999 as rnk
            from tmpData
            group by SizeCode
            ),
            UnionData
            as (
            select * from tmpData
            union all
            select * from SubTotal
            ),
            pivotData
            as (
            select *
            from UnionData
            pivot( sum(Qty)
            for SizeCode in ({1})
            ) a
            )
            select *,(select sum(Qty) from UnionData where ID = p.ID and Article = p.Article) as TotalQty
            from pivotData p
            order by rnk,Seq", poID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid2Data);

            sqlCmd = string.Format(@"with tmpData
            as (
            select o.ID,oq.Article,oq.SizeCode,oq.OriQty,oa.Seq,DENSE_RANK() OVER (ORDER BY o.ID) as rnk
            from Orders o WITH (NOLOCK) 
            inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
            left join Order_Article oa WITH (NOLOCK) on oa.ID = oq.ID and oa.Article = oq.Article
            where o.POID = '{0}'
            ),
            SubTotal
            as (
            select '' as ID,'TTL' as Article,SizeCode,SUM(OriQty) as Qty, '9999' as Seq,99999 as rnk
            from tmpData
            group by SizeCode
            ),
            UnionData
            as (
            select * from tmpData
            union all
            select * from SubTotal
            ),
            pivotData
            as (
            select *
            from UnionData
            pivot( sum(OriQty)
            for SizeCode in ({1})
            ) a
            )
            select *,(select sum(OriQty) from UnionData where ID = p.ID and Article = p.Article) as TotalQty
            from pivotData p
            order by rnk,Seq", poID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid2Data_OriQty);
            #endregion

            #region 撈Grid3資料
            sqlCmd = string.Format(@"with tmpData
            as (
            select oq.Article,oq.SizeCode,oq.Qty,oa.Seq
            from Orders o WITH (NOLOCK) 
            inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
            left join Order_Article oa WITH (NOLOCK) on oa.ID = oq.ID and oa.Article = oq.Article
            where o.POID = '{0}'
            ),
            SubTotal
            as (
            select 'TTL' as Article,SizeCode,SUM(Qty) as Qty, '9999' as Seq
            from tmpData
            group by SizeCode
            ),
            UnionData
            as (
            select * from tmpData
            union all
            select * from SubTotal
            ),
            pivotData
            as (
            select *
            from UnionData
            pivot( sum(Qty)
            for SizeCode in ({1})
            ) a
            )
            select *,(select sum(Qty) from UnionData where Article = p.Article) as TotalQty
            from pivotData p
            order by Seq", poID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid3Data);

            sqlCmd = string.Format(@"with tmpData
            as (
            select oq.Article,oq.SizeCode,oq.OriQty,oa.Seq
            from Orders o WITH (NOLOCK) 
            inner join Order_Qty oq WITH (NOLOCK) on o.ID = oq.ID
            left join Order_Article oa WITH (NOLOCK) on oa.ID = oq.ID and oa.Article = oq.Article
            where o.POID = '{0}'
            ),
            SubTotal
            as (
            select 'TTL' as Article,SizeCode,SUM(OriQty) as Qty, '9999' as Seq
            from tmpData
            group by SizeCode
            ),
            UnionData
            as (
            select * from tmpData
            union all
            select * from SubTotal
            ),
            pivotData
            as (
            select *
            from UnionData
            pivot( sum(OriQty)
            for SizeCode in ({1})
            ) a
            )
            select *,(select sum(OriQty) from UnionData where Article = p.Article) as TotalQty
            from pivotData p
            order by Seq", poID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid3Data_OriQty);
            #endregion

            #region 撈Grid4資料
            sqlCmd = string.Format(@"with tmpData
            as (
            select oq.BuyerDelivery,oqd.Article,oqd.SizeCode,oqd.Qty,oa.Seq,DENSE_RANK() OVER (ORDER BY oq.BuyerDelivery) as rnk
            from Orders o WITH (NOLOCK) 
            inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
            inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID and oq.Seq = oqd.Seq
            left join Order_Article oa WITH (NOLOCK) on oa.ID = oqd.ID and oa.Article = oqd.Article
            where o.POID = '{0}'
            ),
            SubTotal
            as (
            select null as BuyerDelivery,'TTL' as Article,SizeCode,SUM(Qty) as Qty, '9999' as Seq,99999 as rnk
            from tmpData
            group by SizeCode
            ),
            UnionData
            as (
            select * from tmpData
            union all
            select * from SubTotal
            ),
            pivotData
            as (
            select *
            from UnionData
            pivot( sum(Qty)
            for SizeCode in ({1})
            ) a
            )
            select *,(select sum(isnull(Qty,0)) from UnionData where rnk = p.rnk and Article = p.Article) as TotalQty
            from pivotData p
            order by rnk,Seq", poID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid4Data);

            sqlCmd = string.Format(@"with tmpData
            as (
            select oq.BuyerDelivery,oqd.Article,oqd.SizeCode,oqd.OriQty,oa.Seq,DENSE_RANK() OVER (ORDER BY oq.BuyerDelivery) as rnk
            from Orders o WITH (NOLOCK) 
            inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.ID
            inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID and oq.Seq = oqd.Seq
            left join Order_Article oa WITH (NOLOCK) on oa.ID = oqd.ID and oa.Article = oqd.Article
            where o.POID = '{0}'
            ),
            SubTotal
            as (
            select null as BuyerDelivery,'TTL' as Article,SizeCode,SUM(OriQty) as Qty, '9999' as Seq,99999 as rnk
            from tmpData
            group by SizeCode
            ),
            UnionData
            as (
            select * from tmpData
            union all
            select * from SubTotal
            ),
            pivotData
            as (
            select *
            from UnionData
            pivot( sum(OriQty)
            for SizeCode in ({1})
            ) a
            )
            select *,(select sum(isnull(OriQty,0)) from UnionData where rnk = p.rnk and Article = p.Article) as TotalQty
            from pivotData p
            order by rnk,Seq", poID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
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

        private void rarioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (rbQty.Checked)
            {
                listControlBindingSource1.DataSource = grid1Data;
                listControlBindingSource2.DataSource = grid2Data;
                listControlBindingSource3.DataSource = grid3Data;
                listControlBindingSource4.DataSource = grid4Data;
            }
            else if (rbOriQty.Checked)
            {
                listControlBindingSource1.DataSource = grid1Data_OriQty;
                listControlBindingSource2.DataSource = grid2Data_OriQty;
                listControlBindingSource3.DataSource = grid3Data_OriQty;
                listControlBindingSource4.DataSource = grid4Data_OriQty;
            }
        }


    }
}
