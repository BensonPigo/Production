using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <summary>
    /// P22
    /// </summary>
    public partial class P22 : Win.Tems.QueryForm
    {
        /// <summary>
        /// P22
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P22(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Date("CFMDate", header: "Confirm Date", iseditingreadonly: true)
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
            .Text("ConfirmedBy", header: "Confirmed By", width: Widths.Auto(), iseditingreadonly: false)
            .Text("RepackPackID", header: "Repack To Pack ID", width: Widths.AnsiChars(15), iseditable: false)
            .Text("RepackOrderID", header: "Repack To SP #", width: Widths.AnsiChars(15), iseditable: false)
            .Text("RepackCtnStartNo", header: "Repack To CTN #", width: Widths.AnsiChars(6), iseditable: false)
            ;
            this.grid1.AutoResizeColumns();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;

            string dateCFMDate1 = string.Empty;
            string dateCFMDate2 = string.Empty;
            string packid = string.Empty;
            string sp = string.Empty;
            string sqlwhere = string.Empty;
            if (!MyUtility.Check.Empty(this.dateConfirm.TextBox1.Value))
            {
                dateCFMDate1 = this.dateConfirm.TextBox1.Text;
                dateCFMDate2 = this.dateConfirm.TextBox2.Text;
                sqlwhere += $@" and pe.CFMDate between @dateCFMDate1 and @dateCFMDate2 ";
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
declare @dateCFMDate1 date = '{dateCFMDate1}'
declare @dateCFMDate2 date = '{dateCFMDate2}'
declare @packid nvarchar(20) = '{packid}'
declare @sp nvarchar(20) = '{sp}'

select distinct
	pe.CFMDate
	,[PackingListID] = iif(pd.OrigID = '',pd.ID, pd.OrigID)
	,[CTNStartNo] = iif(pd.OrigCTNStartNo = '',pd.CTNStartNo, pd.OrigCTNStartNo)
	,[OrderID] = iif(pd.OrigOrderID = '',pd.OrderID, pd.OrigOrderID)
	,o.CustPONo
	,o.StyleID
	,o.BrandID
	,Country.Alias
	,o.BuyerDelivery
	,o.SciDelivery
	,[ConfirmedBy] = dbo.getPass1(pe.AddName)
	,[ErrorType] = x.PackingErrorID+'-'+pr.Description
    , [RepackPackID] = iif(pd.OrigID != '',pd.ID, pd.OrigID)
    , [RepackOrderID] = iif(pd.OrigOrderID != '',pd.OrderID, pd.OrigOrderID)
    , [RepackCtnStartNo] = iif(pd.OrigCTNStartNo != '',pd.CTNStartNo, pd.OrigCTNStartNo)
    , ShipQty=(select sum(ShipQty) from PackingList_Detail pd2 with(nolock) where pd2.id=pd.id and pd2.ctnstartno=pd.ctnstartno)
from PackErrCFM pe with(nolock)
left join orders o with(nolock) on pe.OrderID = o.ID
left join Country with(nolock) on Country.id = o.Dest
outer apply(
	select top 1 PackingErrorID
	from PackErrTransfer pt with(nolock)
	where pt.PackingListID=pe.PackingListID and pe.CTNStartNo = pt.CTNStartNo and pe.OrderID=pt.OrderID and pe.MDivisionID=pt.MDivisionID and pt.AddDate<pe.AddDate
	order by pt.AddDate desc
)x
left join PackingError pr with(nolock) on pr.ID=x.PackingErrorID and pr.Type='TP'
left join PackingList_Detail pd WITH (NOLOCK) on  pd.SCICtnNo = pe.SCICtnNo 
where 1=1
{sqlwhere}
order by iif(pd.OrigID = '',pd.ID, pd.OrigID),iif(pd.OrigCTNStartNo = '',pd.CTNStartNo, pd.OrigCTNStartNo),pe.CFMDate
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
