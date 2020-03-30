using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    public partial class P07 : Sci.Win.Tems.QueryForm
    {
        public P07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Date("ReceiveDate", header: "Scan Date", iseditingreadonly: true)
            .Text("PackingListID", header: "Pack ID", width: Widths.Auto(), iseditingreadonly: false)
            .Text("CTNStartNo", header: "CTN#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("OrderID", header: "SP#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("CustPONo", header: "PO#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("StyleID", header: "Style#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("BrandID", header: "Brand", width: Widths.Auto(), iseditingreadonly: false)
            .Text("Alias", header: "Destination", width: Widths.Auto(), iseditingreadonly: false)
            .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.Auto(), iseditingreadonly: false)
            .Date("SciDelivery", header: "SCI Delivery", width: Widths.Auto(), iseditingreadonly: false)
            .Text("ReceivedBy", header: "Scan By", width: Widths.Auto(), iseditingreadonly: false)
            .DateTime("AddDate", header: "Scan Time", width: Widths.Auto(), iseditingreadonly: false)
            .Text("RepackPackID", header: "Repack To Pack ID", width: Widths.AnsiChars(15), iseditable: false)
            .Text("RepackOrderID", header: "Repack To SP #", width: Widths.AnsiChars(15), iseditable: false)
            .Text("RepackCtnStartNo", header: "Repack To CTN #", width: Widths.AnsiChars(6), iseditable: false)
            ;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;

            string datereceive1 = string.Empty, datereceive2 = string.Empty, packid = string.Empty, sp = string.Empty;
            string sqlwhere = string.Empty;
            if (!MyUtility.Check.Empty(this.dateReceive.TextBox1.Value))
            {
                datereceive1 = this.dateReceive.TextBox1.Text;
                datereceive2 = this.dateReceive.TextBox2.Text;
                sqlwhere += $@" and dr.ReceiveDate between @datereceive1 and @datereceive2 ";
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                packid = this.txtPackID.Text;
                sqlwhere += $@" and  (dr.PackingListID = @packid or  pd.OrigID = @packid) ";
            }

            if (!MyUtility.Check.Empty(this.txtsp.Text))
            {
                sp = this.txtsp.Text;
                sqlwhere += $@" and (dr.OrderID = @sp or pd.OrigOrderID = @sp) ";
            }

            this.ShowWaitMessage("Data Loading...");

            string sqlcmd = $@"
declare @datereceive1 date = '{datereceive1}'
declare @datereceive2 date = '{datereceive2}'
declare @packid nvarchar(20) = '{packid}'
declare @sp nvarchar(20) = '{sp}'

select dr.ReceiveDate
	    , [PackingListID] = iif(isnull(pd.OrigID, '') = '', dr.PackingListID, pd.OrigID)
	    , [CTNStartNo] = iif(isnull(pd.OrigCTNStartNo, '') = '', dr.CTNStartNo, pd.OrigCTNStartNo)
	    , [OrderID] = iif(isnull(pd.OrigOrderID, '') = '', dr.OrderID, pd.OrigOrderID)
	    , o.CustPONo
	    , o.StyleID
	    , o.BrandID
	    , Country.Alias
	    , os.BuyerDelivery
	    , o.SciDelivery
	    , ReceivedBy = dbo.getPass1(dr.AddName)
        , [AddDate] = format(dr.AddDate, 'yyyy/MM/dd HH:mm:ss')
        , [RepackPackID] = iif(pd.OrigID != '',pd.ID, pd.OrigID)
        , [RepackOrderID] = iif(pd.OrigOrderID != '',pd.OrderID, pd.OrigOrderID)
        , [RepackCtnStartNo] = iif(pd.OrigCTNStartNo != '',pd.CTNStartNo, pd.OrigCTNStartNo)
from DRYReceive dr with(nolock)
left join orders o with(nolock) on dr.OrderID = o.ID
left join Country with(nolock) on Country.id = o.Dest
outer apply (
    select top 1 *
    from PackingList_Detail pd with (nolock)
    where dr.SCICtnNo = pd.SCICtnNo
	        AND dr.OrderID = pd.OrderID
	        AND dr.CTNStartNo  = pd.CTNStartNo
            AND dr.PackingListID = pd.id 
) pd
left join Order_QtyShip os on pd.OrderID = os.Id
							and pd.OrderShipmodeSeq = os.Seq
where 1=1
    {sqlwhere}

 ORDER BY dr.ReceiveDate,[PackingListID],[CTNStartNo],[OrderID]
";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Datas not found!");
            }

            this.HideWaitMessage();
            this.listControlBindingSource1.DataSource = dt;
            this.grid1.AutoResizeColumns();
        }
    }
}
