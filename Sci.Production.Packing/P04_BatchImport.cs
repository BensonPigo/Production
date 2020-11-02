using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P04_BatchImport
    /// </summary>
    public partial class P04_BatchImport : Win.Subs.Base
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataRow packingListData;
        private DataTable selectDataTable;
        private DataTable detailData;

        /// <summary>
        /// P04_BatchImport
        /// </summary>
        /// <param name="packingListData">packingListData</param>
        /// <param name="detailData">detailData</param>
        public P04_BatchImport(DataRow packingListData, DataTable detailData)
        {
            this.InitializeComponent();
            this.packingListData = packingListData;
            this.detailData = detailData;
            this.displayBrand.Value = packingListData["BrandID"].ToString();
            this.displayShipMode.Value = packingListData["ShipModeID"].ToString();
            this.txtcountryDestination.TextBox1.Text = packingListData["Dest"].ToString();
            this.txtcustcd.Text = packingListData["CustCDID"].ToString();
            this.txtcountryDestination.TextBox1.IsSupportEditMode = false;
            this.txtcountryDestination.TextBox1.ReadOnly = true;
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
                .Text("CustCDID", header: "CustCD", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("ShipModeID", header: "Ship Mode", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("OrderTypeID", header: "Order Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O.#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Article", header: "Color Way", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("ShipQty", header: "Qty", iseditingreadonly: true);
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(@"with OrderData
as
(select Orders.ID as OrderID,StyleID,SeasonID,CustCDID,OrderTypeID,CustPONo,POID
 from Orders WITH (NOLOCK) 
inner join Factory WITH (NOLOCK) on Factory.id = Orders.Factoryid
 where Category = 'S'
 and Factory.IsProduceFty = 1
 and BrandID = @brand");
            if (!MyUtility.Check.Empty(this.txtcustcd.Text))
            {
                sqlCmd.Append("\r\n and CustCDID = @custcd");
            }

            if (!MyUtility.Check.Empty(this.txtOrderType.Text))
            {
                sqlCmd.Append("\r\n and OrderTypeID = @orderType");
            }

            if (!MyUtility.Check.Empty(this.txtseason.Text))
            {
                sqlCmd.Append("\r\n and SeasonID = @season");
            }

            if (!MyUtility.Check.Empty(this.txtdropdownlistBuyMonth.SelectedValue))
            {
                sqlCmd.Append("\r\n and BuyMonth = @buyMonth");
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                sqlCmd.Append("\r\n and BuyerDelivery >= @buyerDelivery1");
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value2))
            {
                sqlCmd.Append("\r\n and BuyerDelivery <= @buyerDelivery2");
            }

            sqlCmd.Append(@"),
OrderQty
as
(select o.*, oqsd.Seq as OrderShipmodeSeq, oqs.ShipmodeID, oqsd.Article, oqsd.SizeCode, oqsd.Qty as ShipQty
from OrderData o, Order_QtyShip oqs WITH (NOLOCK) , Order_QtyShip_Detail oqsd WITH (NOLOCK) 
where oqs.Id = o.OrderID
and oqs.Id = oqsd.Id
and oqs.Seq = oqsd.Seq");
            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                sqlCmd.Append("\r\n and oqs.BuyerDelivery >= @buyerDelivery1");
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value2))
            {
                sqlCmd.Append("\r\n and oqs.BuyerDelivery <= @buyerDelivery2");
            }

            sqlCmd.Append(@"),
PackData
as
(select b.* 
 from (select oq.OrderID,oq.StyleID,oq.SeasonID,oq.CustCDID,oq.OrderTypeID,oq.CustPONo,oq.POID,oq.OrderShipmodeSeq,oq.ShipmodeID,oq.Article,oq.SizeCode,(oq.ShipQty-isnull(a.PulloutQty,0)) as ShipQty
       from OrderQty oq
	   left Join (select oq1.OrderID,oq1.OrderShipmodeSeq,pdd.Article,pdd.SizeCode,sum(pdd.ShipQty) as PulloutQty
                  from OrderQty oq1, Pullout_Detail pd WITH (NOLOCK) , Pullout_Detail_Detail pdd WITH (NOLOCK) 
				  where oq1.OrderID = pd.OrderID
				  and oq1.OrderShipmodeSeq = pd.OrderShipmodeSeq
				  and pdd.Pullout_DetailUKey = pd.UKey
				  and pdd.Article = oq1.Article
				  and pdd.SizeCode = oq1.SizeCode
				  group by oq1.OrderID,oq1.OrderShipmodeSeq,pdd.Article,pdd.SizeCode) a on a.OrderID = oq.OrderID and a.OrderShipmodeSeq = oq.OrderShipmodeSeq and a.Article = oq.Article and a.SizeCode = oq.SizeCode
		) b
 where b.ShipQty > 0
)
select 0 as Selected,pd.OrderID,pd.StyleID,pd.SeasonID,pd.CustCDID,pd.SeasonID,pd.OrderTypeID,pd.CustPONo,pd.POID,pd.OrderShipmodeSeq,pd.ShipmodeID,pd.Article,pd.SizeCode,pd.ShipQty, isnull(voc.ColorID,'') as Color
from PackData pd
left join View_OrderFAColor voc on voc.ID = pd.OrderID and voc.Article = pd.Article");
            #region 準備sql參數資料
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@brand", this.displayBrand.Value);
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@custcd", this.txtcustcd.Text);
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter("@orderType", this.txtOrderType.Text);
            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter("@season", this.txtseason.Text);
            System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@buyMonth",
                Value = !MyUtility.Check.Empty(this.txtdropdownlistBuyMonth.SelectedValue) ?
                this.txtdropdownlistBuyMonth.SelectedValue : string.Empty,
            };
            System.Data.SqlClient.SqlParameter sp6 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp7 = new System.Data.SqlClient.SqlParameter();
            sp6.ParameterName = "@buyerDelivery1";
            sp6.Value = !MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) ? this.dateBuyerDelivery.Value1 : DateTime.Now;
            sp7.ParameterName = "@buyerDelivery2";
            sp7.Value = !MyUtility.Check.Empty(this.dateBuyerDelivery.Value2) ? this.dateBuyerDelivery.Value2 : DateTime.Now;

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);
            cmds.Add(sp4);
            cmds.Add(sp5);
            cmds.Add(sp6);
            cmds.Add(sp7);
            #endregion
            DualResult selectResult;
            if (selectResult = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.selectDataTable))
            {
                if (this.selectDataTable.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox(selectResult.ToString());
                return;
            }

            this.listControlBindingSource1.DataSource = this.selectDataTable;
        }

        // Order Type Right Click
        private void TxtOrderType_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlWhere = string.Format("select ID from OrderType WITH (NOLOCK) where BrandID = '{0}' order by ID", this.packingListData["BrandID"].ToString());
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlWhere, "20", this.Text, false, string.Empty);

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtOrderType.Text = item.GetSelectedString();
        }

        // Find
        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.listControlBindingSource1.DataSource))
            {
                return;
            }

            int index = this.listControlBindingSource1.Find("OrderId", this.txtLocateSPNo.Text.ToString());
            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.listControlBindingSource1.Position = index;
            }
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.gridDetail.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable gridData = (DataTable)this.listControlBindingSource1.DataSource;

            if (gridData.Rows.Count == 0)
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
                        findrow[0]["ShipQty"] = MyUtility.Convert.GetInt(currentRow["ShipQty"]);
                    }
                }
            }

            // 系統會自動有回傳值
            this.DialogResult = DialogResult.OK;
        }
    }
}
