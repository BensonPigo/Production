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
    public partial class P09 : Sci.Win.Tems.QueryForm
    {
        public P09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.grid1)
                .Date("ScanDate", header: "Scan Date", iseditingreadonly: true)
                .Text("PackingListID", header: "Pack ID", width: Widths.Auto(), iseditingreadonly: true)
                .Text("CTNStartNo", header: "CTN#", iseditingreadonly: true)
                .Text("CartonQty", header: "Carton Qty", width: Widths.Auto(), iseditingreadonly: true)
                .Text("MDFailQty", header: "Discrepancy", width: Widths.Auto(), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.Auto(), iseditingreadonly: true)
                .Text("CustPONo", header: "PO#", width: Widths.Auto(), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.Auto(), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.Auto(), iseditingreadonly: true)
                .Text("Alias", header: "Destination", width: Widths.Auto(), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.Auto(), iseditingreadonly: true)
                .Date("SciDelivery", header: "SCI Delivery", width: Widths.Auto(), iseditingreadonly: true)
                .Text("ReceivedBy", header: "Scan By", width: Widths.Auto(), iseditingreadonly: true)
                .Text("AddDate", header: "Scan Time", width: Widths.Auto(), iseditingreadonly: true)
                .Text("RepackPackID", header: "Repack To Pack ID", width: Widths.AnsiChars(15), iseditable: false)
                .Text("RepackOrderID", header: "Repack To SP#", width: Widths.AnsiChars(15), iseditable: false)
                .Text("RepackCtnStartNo", header: "Repack To CTN#", width: Widths.AnsiChars(6), iseditable: false);
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;

            string dateTransfer1 = string.Empty, dateTransfer2 = string.Empty, packid = string.Empty, sp = string.Empty;
            string sqlwhere = string.Empty;
            if (this.dateTransfer.HasValue)
            {
                dateTransfer1 = this.dateTransfer.Value1.Value.ToShortDateString();
                dateTransfer2 = this.dateTransfer.Value2.Value.AddDays(1).AddSeconds(-1).ToString("yyyy/MM/dd HH:mm:ss");
                sqlwhere += $@" and md.ScanDate between @TransferDate1 and @TransferDate2 ";
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                packid = this.txtPackID.Text;
                sqlwhere += $@" and (pd.ID = @packid or  pd.OrigID = @packid) ";
            }

            if (!MyUtility.Check.Empty(this.txtsp.Text))
            {
                sp = this.txtsp.Text;
                sqlwhere += $@" and (pd.OrderID = @sp or pd.OrigOrderID = @sp) ";
            }

            this.ShowWaitMessage("Data Loading...");

            string sqlcmd = $@"
declare @TransferDate1  datetime = '{dateTransfer1}'
declare @TransferDate2  datetime = '{dateTransfer2}'
declare @packid nvarchar(20) = '{packid}'
declare @sp nvarchar(20) = '{sp}'

select DISTINCT md.ScanDate
	, [PackingListID] = iif(isnull(pd.OrigID, '') = '', md.PackingListID, pd.OrigID)
	, [CTNStartNo] = iif(isnull(pd.OrigCTNStartNo, '') = '', md.CTNStartNo, pd.OrigCTNStartNo)
	, [CartonQty] = md.CartonQty * iif(ol.LocationQty = 0, sl.LocationQty, ol.LocationQty)
	, pd.MDFailQty
	, [OrderID] = iif(isnull(pd.OrigOrderID, '') = '', md.OrderID, pd.OrigOrderID)
	, o.CustPONo
	, o.StyleID
	, o.BrandID
	, Country.Alias
	, os.BuyerDelivery
	, o.SciDelivery
	, ReceivedBy = dbo.getPass1(md.AddName)
    , [AddDate] = format(md.AddDate, 'yyyy/MM/dd HH:mm:ss')
	, [RepackPackID] = iif(pd.OrigID != '',pd.ID, pd.OrigID)
    , [RepackOrderID] = iif(pd.OrigOrderID != '',pd.OrderID, pd.OrigOrderID)
    , [RepackCtnStartNo] = iif(pd.OrigCTNStartNo != '',pd.CTNStartNo, pd.OrigCTNStartNo)
from MDScan md with(nolock)
left join orders o with(nolock) on md.OrderID = o.ID
left join Country with(nolock) on Country.id = o.Dest
left join PackingList_Detail pd WITH (NOLOCK) on md.SCICtnNo = pd.SCICtnNo
													AND md.OrderID = pd.OrderID
													AND md.CTNStartNo = pd.CTNStartNo
                                                    AND md.PackingListID = pd.id 
													AND pd.CTNQty <> 0
left join Order_QtyShip os on pd.OrderID = os.Id
							and pd.OrderShipmodeSeq = os.Seq
outer apply
(
	select [LocationQty] = count(distinct Location)
	from Order_Location with(nolock)
	where OrderId = md.OrderID
)ol
outer apply
(
	select [LocationQty] = count(distinct Location)
	from Style_Location with(nolock)
	where StyleUkey = o.StyleUkey
)sl
where 1=1
{sqlwhere}

 ORDER BY md.ScanDate,[PackingListID],[CTNStartNo],[OrderID]
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
