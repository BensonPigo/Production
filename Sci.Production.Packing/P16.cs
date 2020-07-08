using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Drawing;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P14
    /// </summary>
    public partial class P16 : Sci.Win.Tems.QueryForm
    {
        /// <summary>
        /// P16
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        private DataTable gridData;
        private string selectDataTable_DefaultView_Sort = string.Empty;

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // Grid設定
            this.gridDetail.IsEditingReadOnly = false;
            this.gridDetail.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("CustPONo", header: "PO No.", width: Widths.AnsiChars(19), iseditable: false)
                .Text("ID", header: "SP#", width: Widths.AnsiChars(13), iseditable: false)
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(2), iseditable: false)
                .Text("ShipmodeID", header: "Shipmode", width: Widths.AnsiChars(10), iseditable: false)
                .Date("BuyerDelivery", header: "Buyer Delivery", iseditable: false)
                .Text("CustCDID", header: "CustCD", width: Widths.AnsiChars(16), iseditable: false)
                .Text("Alias", header: "Dest", width: Widths.AnsiChars(18), iseditable: false)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(15), iseditable: false);
        }

        private string cmd;

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            // 至少要輸入一個條件才可以查詢
            if (MyUtility.Check.Empty(this.dateRangeBuyerDelivery.Value1) &&
                MyUtility.Check.Empty(this.dateRangeBuyerDelivery.Value2) &&
                MyUtility.Check.Empty(this.txtFromPoNo.Text) &&
                MyUtility.Check.Empty(this.txtToPoNo.Text))
            {
                MyUtility.Msg.WarningBox("PO No. and BuyerDelivery can not both be empty");
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
select *,DENSE_RANK() OVER (PARTITION BY muti_flag ORDER BY CustPONo,BuyerDelivery,ShipModeID,CustCDID) as changeColorRank from (
select o.CustPONo,o.ID,oq.Seq,oq.ShipmodeID,oq.BuyerDelivery,o.CustCDID,co.Alias,oq.Qty ,
iif(count(*) over (PARTITION BY o.CustPONo,oq.BuyerDelivery,oq.ShipModeID,o.CustCDID )>1,2,1)  as muti_flag
from Orders o  WITH (NOLOCK) 
inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.Id 
inner join CustCD c  WITH (NOLOCK)  on c.BrandID=o.BrandID  and c.id = o.CustCDID
left join Country co WITH (NOLOCK)  on o.Dest=co.ID
where o.BrandID = 'U.ARMOUR' 
and o.ProjectID = 'VMI' 
and exists (select 1 from OrderType where IsGMTMaster = 0 and BrandID = 'U.ARMOUR' and ID = o.OrderTypeID) ");

            if (!MyUtility.Check.Empty(this.dateRangeBuyerDelivery.Value1))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery >= '{0}'", this.dateRangeBuyerDelivery.TextBox1.Text));
            }

            if (!MyUtility.Check.Empty(this.dateRangeBuyerDelivery.Value2))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery <= '{0}'", this.dateRangeBuyerDelivery.TextBox2.Text));
            }

            if (!MyUtility.Check.Empty(this.txtFromPoNo.Text))
            {
                sqlCmd.Append(string.Format(" and o.CustPONo >= '{0}'", MyUtility.Convert.GetString(this.txtFromPoNo.Text)));
            }

            if (!MyUtility.Check.Empty(this.txtToPoNo.Text))
            {
                sqlCmd.Append(string.Format(" and o.CustPONo <= '{0}'", MyUtility.Convert.GetString(this.txtToPoNo.Text)));
            }

            sqlCmd.Append(@" ) s order by CustPONo,BuyerDelivery,ShipmodeID,CustCDID");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail.\r\n" + result.ToString());
            }

            this.cmd = sqlCmd.ToString();
            if (this.gridData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            this.listControlBindingSource1.DataSource = this.gridData;

            for (int i = 0; i < this.gridData.Rows.Count; i++)
            {
                if ((int)this.gridData.Rows[i]["muti_flag"] == 2)
                {
                    this.gridDetail.Rows[i].DefaultCellStyle.BackColor = (Convert.ToDouble(this.gridData.Rows[i]["changeColorRank"]) % 2) == 1 ? Color.FromArgb(248, 255, 173) : Color.FromArgb(255, 219, 244);
                }
            }
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
