using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <summary>
    /// P21
    /// </summary>
    public partial class P21 : Win.Tems.QueryForm
    {
        /// <summary>
        /// P21
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Date("TransferDate", header: "Transfer Date", iseditingreadonly: true)
            .Text("PackingListID", header: "Pack ID", width: Widths.Auto(), iseditingreadonly: false)
            .Text("CTNStartNo", header: "CTN#", width: Widths.Auto(), iseditingreadonly: false)
            .Numeric("ShipQty", header: "Pack Qty", iseditingreadonly: true)
            .Text("OrderID", header: "SP#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("CustPONo", header: "PO#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("StyleID", header: "Style#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("BrandID", header: "Brand", width: Widths.Auto(), iseditingreadonly: false)
            .Text("ErrorType", header: "ErrorType", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Alias", header: "Destination", width: Widths.Auto(), iseditingreadonly: false)
            .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.Auto(), iseditingreadonly: false)
            .Date("SciDelivery", header: "SCI Delivery", width: Widths.Auto(), iseditingreadonly: false)
            .Text("TransferredBy", header: "Transferred By", width: Widths.Auto(), iseditingreadonly: false)
            .Date("CFMDate", header: "Confirm Date", iseditingreadonly: true)
            .Text("ConfirmedBy", header: "Confirmed By", width: Widths.Auto(), iseditingreadonly: true)
            .Text("RepackPackID", header: "Repack To Pack ID", width: Widths.AnsiChars(15), iseditable: false)
            .Text("RepackOrderID", header: "Repack To SP #", width: Widths.AnsiChars(15), iseditable: false)
            .Text("RepackCtnStartNo", header: "Repack To CTN #", width: Widths.AnsiChars(6), iseditable: false)
            ;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;

            string dateTransferDate1 = string.Empty;
            string dateTransferDate2 = string.Empty;
            string packid = string.Empty;
            string sp = string.Empty;
            string sqlwhere = string.Empty;
            if (!MyUtility.Check.Empty(this.dateTransfer.TextBox1.Value))
            {
                dateTransferDate1 = this.dateTransfer.TextBox1.Text;
                dateTransferDate2 = this.dateTransfer.TextBox2.Text;
                sqlwhere += $@" and pe.TransferDate between @dateTransferDate1 and @dateTransferDate2 ";
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

            string sqlcmd = $@"
declare @dateTransferDate1 date = '{dateTransferDate1}'
declare @dateTransferDate2 date = '{dateTransferDate2}'
declare @packid nvarchar(20) = '{packid}'
declare @sp nvarchar(20) = '{sp}'

select distinct
	pe.TransferDate
	,[PackingListID] = iif(pd.OrigID = '',pd.ID, pd.OrigID)
	,[CTNStartNo] = iif(pd.OrigCTNStartNo = '',pd.CTNStartNo, pd.OrigCTNStartNo)
	,[OrderID] = iif(pd.OrigOrderID = '',pd.OrderID, pd.OrigOrderID)
	,o.CustPONo
	,o.StyleID
	,o.BrandID
	,Country.Alias
	,o.BuyerDelivery
	,o.SciDelivery
	,[TransferredBy] = dbo.getPass1(pe.AddName)
    ,x.CFMDate
    ,[ConfirmedBy] = dbo.getPass1(x.AddName)
    ,[ErrorType] = pe.PackingErrorID+'-'+perr.Description
    ,[RepackPackID] = iif(pd.OrigID != '',pd.ID, pd.OrigID)
    ,[RepackOrderID] = iif(pd.OrigOrderID != '',pd.OrderID, pd.OrigOrderID)
    ,[RepackCtnStartNo] = iif(pd.OrigCTNStartNo != '',pd.CTNStartNo, pd.OrigCTNStartNo)
    , ShipQty=(select sum(ShipQty) from PackingList_Detail pd2 with(nolock) where pd2.id=pd.id and pd2.ctnstartno=pd.ctnstartno)
from PackErrTransfer pe with(nolock)
left join orders o with(nolock) on pe.OrderID = o.ID
left join Country with(nolock) on Country.id = o.Dest
left join PackingError perr with (nolock) on pe.PackingErrorID = perr.ID and perr.Type='TP'
left join PackingList_Detail pd WITH (NOLOCK) on  pd.SCICtnNo = pe.SCICtnNo 
outer apply(
	select top 1 CFMDate,AddName
	from PackErrCFM pt with(nolock)
	where pt.PackingListID=pe.PackingListID and pt.CTNStartNo = pe.CTNStartNo  and pe.MDivisionID=pt.MDivisionID and pt.AddDate>pe.AddDate
	order by pt.AddDate
)x
where 1=1
{sqlwhere}
order by iif(pd.OrigID = '',pd.ID, pd.OrigID),iif(pd.OrigCTNStartNo = '',pd.CTNStartNo, pd.OrigCTNStartNo),pe.TransferDate
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

            this.listControlBindingSource1.DataSource = dt;
            this.grid1.AutoResizeColumns();
        }
    }
}
