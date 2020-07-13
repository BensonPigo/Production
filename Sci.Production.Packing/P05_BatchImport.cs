using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P05_BatchImport
    /// </summary>
    public partial class P05_BatchImport : Win.Subs.Base
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataRow packingListData;
        private DataTable selectDataTable;
        private DataTable detailData;
        private DualResult result;

        /// <summary>
        /// P05_BatchImport
        /// </summary>
        /// <param name="packingListData">packingListData</param>
        /// <param name="detailData">detailData</param>
        public P05_BatchImport(DataRow packingListData, DataTable detailData)
        {
            this.InitializeComponent();
            this.packingListData = packingListData;
            this.detailData = detailData;
            this.displayBrand.Value = packingListData["BrandID"].ToString();
            this.displayM.Value = packingListData["MDivisionID"].ToString();
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridDetail.IsEditingReadOnly = false;
            this.gridDetail.DataSource = this.listControlBindingSource1;

            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("OrderId", header: "SP No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderShipmodeSeq", header: "Seq", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("StyleID", header: "Style No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", iseditingreadonly: true)
                .Text("CustPONo", header: "P.O.#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Article", header: "Color Way", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("PulloutQty", header: "Accu. Ship Qty", iseditingreadonly: true)
                .Numeric("ShipQty", header: "Qty")
                .Text("CustCDID", header: "Cust CD", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Dest", header: "Dest", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("OrderTypeID", header: "Order Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                ;

            this.gridDetail.Columns[10].DefaultCellStyle.BackColor = Color.Pink;
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
with OrderData as ( 
    select  o.ID
            , oq.Seq
            , o.StyleID
            , oq.BuyerDelivery
            , o.CustPONo
            , oqd.Article
            , oqd.SizeCode
            , oqd.Qty
            , o.POID
            , o.SeasonID
            , Factory = o.FtyGroup
            , o.CustCDID
            , o.Dest
            , o.OrderTypeID
    from    Orders o WITH (NOLOCK) 
    inner join Order_QtyShip oq WITH (NOLOCK) on oq.Id = o.ID
    inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oqd.Id = oq.Id
													     and oqd.Seq = oq.Seq
    inner join Factory f With (NoLock) on o.FactoryID = f.ID
    where   o.BrandID = @brand
            and o.MDivisionID = @mdivisionid
            and o.IsForecast = 0
            and o.PulloutComplete = 0
            and o.LocalOrder = 0
            and o.Junk = 0
            and f.IsProduceFty = 1
            and o.category not in ('B','G')
");
            if (!MyUtility.Check.Empty(this.txtSPNoStart.Text))
            {
                sqlCmd.Append("\r\n and o.ID >= @orderID1");
            }

            if (!MyUtility.Check.Empty(this.txtSPNoEnd.Text))
            {
                sqlCmd.Append("\r\n and o.ID <= @orderID2");
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                sqlCmd.Append("\r\n and oq.BuyerDelivery >= @buyerDelivery1");
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value2))
            {
                sqlCmd.Append("\r\n and oq.BuyerDelivery <= @buyerDelivery2");
            }

            if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
            {
                sqlCmd.Append("\r\n and o.SciDelivery >= @sciDelivery1");
            }

            if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value2))
            {
                sqlCmd.Append("\r\n and o.SciDelivery <= @sciDelivery2");
            }

            sqlCmd.Append(@"
),
DistOrderData as (
    select  distinct ID
            , Article
            , SizeCode
    from OrderData
),
FOCData as (
    select  a.*
    from (
        select  dod.ID
                , dod.Article
                , dod.SizeCode
                , isnull(ou2.POPrice,isnull(ou1.POPrice,-1)) as Price
        from DistOrderData dod
        left join Order_UnitPrice ou1 WITH (NOLOCK) on ou1.Id = dod.ID and ou1.Article = '----' and ou1.SizeCode = '----' and ou1.POPrice = 0
        left join Order_UnitPrice ou2 WITH (NOLOCK) on ou2.Id = dod.ID and ou2.Article = dod.Article and ou2.SizeCode = dod.SizeCode and ou2.POPrice >= 0) a
    where a.Price = 0
),
PulloutData as (
    select  pdd.OrderID
            , pd.OrderShipmodeSeq
            , pdd.Article
            , pdd.SizeCode
            , sum(pdd.ShipQty) as PulloutQty
    from    FOCData fd
            , Pullout_Detail pd WITH (NOLOCK) 
            , Pullout_Detail_Detail pdd WITH (NOLOCK) 
    where   pdd.OrderID = fd.ID
            and pd.UKey = pdd.Pullout_DetailUKey
            and fd.Article = pdd.Article
		    and fd.SizeCode = pdd.SizeCode
    group by pdd.OrderID, pd.OrderShipmodeSeq, pdd.Article, pdd.SizeCode
)
select  0 as Selected
        , od.ID as OrderID
        , od.Seq as OrderShipmodeSeq
        , od.StyleID
        , od.BuyerDelivery
        , od.CustPONo
        , od.Article
        , od.SizeCode
        , (od.Qty-isnull(pd.PulloutQty,0)) as ShipQty
        , isnull(voc.ColorID,'') as Color
        , isnull(pd.PulloutQty,0) as PulloutQty
        , isnull(fd.Price,-1) as Price
        , od.SeasonID
        , od.Factory
        , od.CustCDID
        , od.Dest
        , od.OrderTypeID
into #tmp
from OrderData od
left join FOCData fd on od.ID = fd.ID and od.Article = fd.Article and od.SizeCode = fd.SizeCode
left join PulloutData pd on pd.OrderID = od.ID and pd.OrderShipmodeSeq = od.Seq and pd.Article = od.Article and pd.SizeCode = od.SizeCode
left join View_OrderFAColor voc on voc.ID = od.ID and voc.Article = od.Article

select * from #tmp where Price != 0 or ShipQty <= 0

select * from #tmp where ShipQty > 0 and Price = 0

drop table #tmp;
");
            #region 準備sql參數資料
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@brand", this.displayBrand.Value);
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@mdivisionid", this.displayM.Value);
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter("@orderID1", this.txtSPNoStart.Text);
            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter("@orderID2", this.txtSPNoEnd.Text);
            System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter();
            sp5.ParameterName = "@buyerDelivery1";
            sp5.Value = !MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) ? this.dateBuyerDelivery.Value1 : DateTime.Now;

            System.Data.SqlClient.SqlParameter sp6 = new System.Data.SqlClient.SqlParameter();
            sp6.ParameterName = "@buyerDelivery2";
            sp6.Value = !MyUtility.Check.Empty(this.dateBuyerDelivery.Value2) ? this.dateBuyerDelivery.Value2 : DateTime.Now;

            System.Data.SqlClient.SqlParameter sp7 = new System.Data.SqlClient.SqlParameter();
            sp7.ParameterName = "@sciDelivery1";
            sp7.Value = !MyUtility.Check.Empty(this.dateSCIDelivery.Value1) ? this.dateSCIDelivery.Value1 : DateTime.Now;

            System.Data.SqlClient.SqlParameter sp8 = new System.Data.SqlClient.SqlParameter();
            sp8.ParameterName = "@sciDelivery2";
            sp8.Value = !MyUtility.Check.Empty(this.dateSCIDelivery.Value2) ? this.dateSCIDelivery.Value2 : DateTime.Now;

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);
            cmds.Add(sp4);
            cmds.Add(sp5);
            cmds.Add(sp6);
            cmds.Add(sp7);
            cmds.Add(sp8);
            #endregion

            DataTable[] dts;

            if (this.result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out dts))
            {
                StringBuilder msg = new StringBuilder();
                this.selectDataTable = dts[1];
                if (!MyUtility.Check.Empty(dts[0]) && dts[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dts[0].Rows)
                    {
                        msg.Append($@"SP: {dr["OrderID"]}, Colorway: {dr["Article"]}, Size: {dr["SizeCode"]}" + Environment.NewLine);
                    }

                    MyUtility.Msg.WarningBox("Below records are in packing FOC already, please check again" + Environment.NewLine + msg.ToString());
                }

                if (this.selectDataTable.Rows.Count == 0 && MyUtility.Check.Empty(msg.ToString()))
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox(this.result.ToString());
                return;
            }

            this.listControlBindingSource1.DataSource = this.selectDataTable;
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.gridDetail.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable gridData = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(gridData) || gridData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No data!");
                return;
            }

            DataRow[] dr = gridData.Select("Selected = 1");
            if (dr.Length > 0)
            {
                foreach (DataRow currentRow in dr)
                {
                    DataRow[] findrow = this.detailData.Select(string.Format("OrderID = '{0}' and OrderShipmodeSeq = '{1}' and Article = '{2}' and SizeCode = '{3}'", currentRow["OrderID"].ToString(), currentRow["OrderShipmodeSeq"].ToString(), currentRow["Article"].ToString(), currentRow["SizeCode"].ToString()));
                    if (findrow.Length == 0)
                    {
                        currentRow.AcceptChanges();
                        currentRow.SetAdded();
                        this.detailData.ImportRow(currentRow);
                    }
                    else
                    {
                        findrow[0]["Color"] = currentRow["Color"].ToString();
                        findrow[0]["ShipQty"] = Convert.ToInt32(currentRow["ShipQty"]);
                    }
                }
            }

            // 系統會自動有回傳值
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
