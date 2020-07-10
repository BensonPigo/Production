using System;
using System.Data;
using System.Text;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_QtyShip
    /// </summary>
    public partial class P01_QtyShip : Win.Subs.Base
    {
        private DataTable grid1Data;
        private DataTable grid2Data;  // 存Qty資料
        private DataTable grid1Data_OriQty;
        private DataTable grid2Data_OriQty;  // 存OriQty資料
        private string orderID;
        private string poID;

        /// <summary>
        /// P01_QtyShip
        /// </summary>
        /// <param name="orderID">string orderID</param>
        /// <param name="poid">string pOID</param>
        public P01_QtyShip(string orderID, string poid)
        {
            this.InitializeComponent();
            this.orderID = orderID;
            this.poID = poid;
            this.Text = this.Text + " (" + this.orderID + ")";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // 設定Grid1的顯示欄位
            this.gridQtyBreakDownByShipmode.IsEditingReadOnly = true;
            this.gridQtyBreakDownByShipmode.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridQtyBreakDownByShipmode)
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(2))
                .Text("ShipmodeID", header: "Ship Mode", width: Widths.AnsiChars(10))
                .Date("BuyerDelivery", header: "Delivery", width: Widths.AnsiChars(10))
                .Date("FtyKPI", header: "FtyKPI", width: Widths.AnsiChars(10))
                .Numeric("Qty", header: "Total Q'ty", width: Widths.AnsiChars(6))
                .Text("AddName", header: "Create by", width: Widths.AnsiChars(10))
                .DateTime("AddDate", header: "Create at", width: Widths.AnsiChars(18))
                .Text("EditName", header: "Edit by", width: Widths.AnsiChars(10))
                .DateTime("EditDate", header: "Edit at", width: Widths.AnsiChars(18));

            string sqlCmd = string.Format(
                @"select Seq,ShipmodeID,BuyerDelivery,FtyKPI,Qty,AddName,AddDate,EditName,EditDate from Order_QtyShip WITH (NOLOCK) 
            where ID = '{0}'
            order by Seq", this.orderID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.grid1Data);
            sqlCmd = string.Format(
                @"select Seq,ShipmodeID,BuyerDelivery,FtyKPI,OriQty as Qty,AddName,AddDate,EditName,EditDate from Order_QtyShip WITH (NOLOCK) 
            where ID = '{0}'
            order by Seq", this.orderID);
            result = DBProxy.Current.Select(null, sqlCmd, out this.grid1Data_OriQty);

            sqlCmd = string.Format("select * from Order_SizeCode WITH (NOLOCK) where ID = '{0}' order by Seq", this.poID);
            DataTable headerData;
            result = DBProxy.Current.Select(null, sqlCmd, out headerData);
            StringBuilder pivot = new StringBuilder();

            // 設定Grid2的顯示欄位
            this.gridQtyBreakDownbyArticleSizeDetail.IsEditingReadOnly = true;
            this.gridQtyBreakDownbyArticleSizeDetail.DataSource = this.listControlBindingSource2;
            var gen = this.Helper.Controls.Grid.Generator(this.gridQtyBreakDownbyArticleSizeDetail);
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

            // 凍結欄位
            this.gridQtyBreakDownbyArticleSizeDetail.Columns[1].Frozen = true;

            // 撈Grid2資料
            sqlCmd = string.Format(
                @"with tmpData
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
            order by ASeq",
                this.orderID,
                MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out this.grid2Data);

            sqlCmd = string.Format(
                @"with tmpData
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
            order by ASeq",
                this.orderID,
                MyUtility.Check.Empty(pivot.ToString()) ? "[ ]" : pivot.ToString().Substring(0, pivot.ToString().Length - 1));
            result = DBProxy.Current.Select(null, sqlCmd, out this.grid2Data_OriQty);

            // 設定兩個Grid的關聯
            if (this.grid2Data != null)
            {
                this.gridQtyBreakDownByShipmode.SelectionChanged += (s, e) =>
                {
                    this.gridQtyBreakDownByShipmode.ValidateControl();
                    DataRow dr = this.gridQtyBreakDownByShipmode.GetDataRow<DataRow>(this.gridQtyBreakDownByShipmode.GetSelectedRowIndex());
                    if (dr != null)
                    {
                        string filter = string.Format("Seq = '{0}'", MyUtility.Convert.GetString(dr["Seq"]));
                        this.grid2Data.DefaultView.RowFilter = filter;
                    }
                };
            }

            this.listControlBindingSource1.DataSource = this.grid1Data;
            this.listControlBindingSource2.DataSource = this.grid2Data;
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
            this.CreateGridCol(gen, datatype, propname: propname, header: header, width: width);
        }

        private void CreateGridCol(
            IDataGridViewGenerator gen,
            string datatype,
            string propname = null,
            string header = null,
            IWidth width = null,
            bool? iseditingreadonly = null,
            int index = -1)
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

        private void RadioOriQty_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioQty.Checked)
            {
                this.listControlBindingSource1.DataSource = this.grid1Data;
                this.listControlBindingSource2.DataSource = this.grid2Data;
            }
            else if (this.radioOriQty.Checked)
            {
                this.listControlBindingSource1.DataSource = this.grid1Data_OriQty;
                this.listControlBindingSource2.DataSource = this.grid2Data_OriQty;
            }
        }
    }
}
