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
    public partial class P01_QtyShip : Sci.Win.Subs.Base
    {
        DataTable grid1Data, grid2Data;  //存Qty資料
        DataTable grid1Data_OriQty, grid2Data_OriQty;  //存OriQty資料
        string orderID, poID;

        public P01_QtyShip(string OrderID, string POID)
        {
            InitializeComponent();
            orderID = OrderID;
            poID = POID;
            Text = Text + " (" + orderID + ")";
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            //設定Grid1的顯示欄位
            this.gridQtyBreakDownByShipmode.IsEditingReadOnly = true;
            this.gridQtyBreakDownByShipmode.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridQtyBreakDownByShipmode)
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(2))
                .Text("ShipmodeID", header: "Ship Mode", width: Widths.AnsiChars(10))
                .Date("BuyerDelivery", header: "Delivery", width: Widths.AnsiChars(10))
                .Date("FtyKPI", header: "FtyKPI", width: Widths.AnsiChars(10))
                .Numeric("Qty", header: "Total Q'ty", width: Widths.AnsiChars(6))
                .Text("AddName", header: "Create by", width: Widths.AnsiChars(10))
                .DateTime("AddDate", header: "Create at", width: Widths.AnsiChars(18))
                .Text("EditName", header: "Edit by", width: Widths.AnsiChars(10))
                .DateTime("EditDate", header: "Edit at", width: Widths.AnsiChars(18));

            string sqlCmd = string.Format(@"select Seq,ShipmodeID,BuyerDelivery,FtyKPI,Qty,AddName,AddDate,EditName,EditDate from Order_QtyShip WITH (NOLOCK) 
            where ID = '{0}'
            order by Seq", orderID);
            DualResult result = DBProxy.Current.Select(null,sqlCmd,out grid1Data);
            sqlCmd = string.Format(@"select Seq,ShipmodeID,BuyerDelivery,FtyKPI,OriQty as Qty,AddName,AddDate,EditName,EditDate from Order_QtyShip WITH (NOLOCK) 
            where ID = '{0}'
            order by Seq", orderID);
            result = DBProxy.Current.Select(null, sqlCmd, out grid1Data_OriQty);

            sqlCmd = string.Format("select * from Order_SizeCode WITH (NOLOCK) where ID = '{0}' order by Seq", poID);
            DataTable headerData;
            result = DBProxy.Current.Select(null, sqlCmd, out headerData);
            StringBuilder pivot = new StringBuilder();

            //設定Grid2的顯示欄位
            this.gridQtyBreakDownbyArticleSizeDetail.IsEditingReadOnly = true;
            this.gridQtyBreakDownbyArticleSizeDetail.DataSource = listControlBindingSource2;
            var gen = Helper.Controls.Grid.Generator(this.gridQtyBreakDownbyArticleSizeDetail);
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
            //凍結欄位
            gridQtyBreakDownbyArticleSizeDetail.Columns[1].Frozen = true;

            //撈Grid2資料
            sqlCmd = string.Format(@"with tmpData
            as (
            select oqd.Seq,oqd.Article,oqd.SizeCode,oqd.Qty,oa.Seq as ASeq
            from Order_QtyShip_Detail oqd WITH (NOLOCK) 
            left join Order_Article oa WITH (NOLOCK) on oa.ID = oqd.ID and oa.Article = oqd.Article
            where oqd.ID = '{0}'
            ),
            SubTotal
            as (
            select Seq,'TTL' as Article,SizeCode,SUM(Qty) as Qty, '9999' as ASeq
            from tmpData
            group by Seq,SizeCode
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
            select *,(select sum(Qty) from UnionData where Seq = p.Seq and Article = p.Article) as TotalQty
            from pivotData p
            order by ASeq", orderID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid2Data);

            sqlCmd = string.Format(@"with tmpData
            as (
            select oqd.Seq,oqd.Article,oqd.SizeCode,oqd.OriQty,oa.Seq as ASeq
            from Order_QtyShip_Detail oqd WITH (NOLOCK) 
            left join Order_Article oa WITH (NOLOCK) on oa.ID = oqd.ID and oa.Article = oqd.Article
            where oqd.ID = '{0}'
            ),
            SubTotal
            as (
            select Seq,'TTL' as Article,SizeCode,SUM(OriQty) as Qty, '9999' as ASeq
            from tmpData
            group by Seq,SizeCode
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
            select *,(select sum(OriQty) from UnionData where Seq = p.Seq and Article = p.Article) as TotalQty
            from pivotData p
            order by ASeq", orderID, MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out grid2Data_OriQty);
            

            //設定兩個Grid的關聯
            if (grid2Data != null)
            {
                gridQtyBreakDownByShipmode.SelectionChanged += (s, e) =>
                {
                    gridQtyBreakDownByShipmode.ValidateControl();
                    DataRow dr = this.gridQtyBreakDownByShipmode.GetDataRow<DataRow>(gridQtyBreakDownByShipmode.GetSelectedRowIndex());
                    if (dr != null)
                    {
                        string filter = string.Format("Seq = '{0}'", MyUtility.Convert.GetString(dr["Seq"]));
                        grid2Data.DefaultView.RowFilter = filter;
                    }
                };
            }

            listControlBindingSource1.DataSource = grid1Data;
            listControlBindingSource2.DataSource = grid2Data;
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

            }
            else if (radioOriQty.Checked)
            {
                listControlBindingSource1.DataSource = grid1Data_OriQty;
                listControlBindingSource2.DataSource = grid2Data_OriQty;

            }
        }


    }
}
