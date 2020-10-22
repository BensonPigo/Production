using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class P62_Import : Sci.Win.Subs.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;

        /// <inheritdoc/>
        public P62_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.LoadData();
            base.OnFormLoaded();
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
               .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
               .Text("INVNo", header: "Invoice No.", iseditingreadonly: true, width: Widths.AnsiChars(18))
               .Text("OrderID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(15))
               .Text("StyleID", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(13))
               .Text("Description", header: "Style Description", iseditingreadonly: true, width: Widths.AnsiChars(30))
               .Text("Brand", header: "Brand", iseditingreadonly: true, width: Widths.AnsiChars(8))
               .Text("ShipModeID", header: "Shipmode", iseditingreadonly: true, width: Widths.AnsiChars(8))
               .Numeric("POPrice", header: "FOB", width: Widths.AnsiChars(9), decimal_places: 4, integer_places: 5, iseditingreadonly: true)
               .Numeric("CTNQty", header: "CTN", width: Widths.AnsiChars(9), decimal_places: 0, integer_places: 9, iseditingreadonly: true)
               .Numeric("ShipModeSeqQty", header: "Qty", width: Widths.AnsiChars(9), decimal_places: 0, integer_places: 9, iseditingreadonly: true)
               .Numeric("NetKg", header: "N.W.", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 7, iseditingreadonly: true)
               .Numeric("WeightKg", header: "G.W.", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 7, iseditingreadonly: true)
               .Text("Forwarder", header: "Forwarder", iseditingreadonly: true, width: Widths.AnsiChars(8))
               ;
        }

        private void LoadData()
        {
            DataTable dt;
            string sqlcmd = $@"
Declare @ID varchar(15) = '{this.dr_master["ID"]}'
Declare @ShipMode varchar(60) = '{this.dr_master["ShipModeID"]}'
Declare @ETD date = '{((DateTime)this.dr_master["ETD"]).ToString("yyyy/MM/dd")}'
Declare @CustCD varchar(16) = '{this.dr_master["CustCDID"]}'
Declare @Buyer varchar(8) = '{this.dr_master["Buyer"]}'
Declare @Forwarder varchar(6) = '{this.dr_master["Forwarder"]}'
Declare @Dest varchar(2) = '{this.dr_master["Dest"]}'

-- 1. 彈窗範例畫面SQL
select selected = 0 
	, [INVNo] = g.id
    , [OrderID] = pld.orderid
    , [StyleID] = o.styleid
    , [Description] = s.description
    , [Brand] = g.BrandID
    , [ShipmodeID] = g.ShipModeID
    , [POPrice] = o.POPrice
    , [ShipModeSeqQty] = sum(pld.ShipQty)
    , [CTNQty] = sum(pld.CTNQty)
    , [Forwarder] = g.Forwarder
    , [NetKg]  = (case when @ShipMode in ('A/C','A/P') then sum(pld.NW)
        else (sum(pld.NW) + ( sum(pld.NW) * 0.05))
		end)
    , [WeightKg] = (case when @ShipMode in ('A/C','A/P') then sum(pld.GW)
        else (sum(pld.GW) + ( sum(pld.GW) * 0.05))
    end) 
	, [LocalINVNo] = g.id
	, s.Description
	, [HSCode] = ''
	, [COFormType] = ''
	, [COID] = ''
	, [CODate] = null
from GMTBooking g
inner join packinglist pl on pl.invno=g.id
inner join packinglist_detail pld on pl.id=pld.id
inner join orders o on o.id=pld.orderid
inner join Buyer b2 on o.BrandID =b2.id
inner join style s on s.ukey=o.styleukey
outer apply (
	select kd.status 
		from KHExportDeclaration kd 
		where 1=1
		and kd.id=@ID
) kd_status
where 1=1
and g.ETD = @ETD
and g.CustCDID = @CustCD
and g.ShipModeID = @ShipMode
and (b2.id= @Buyer or @Buyer ='')
and (g.Forwarder = @Forwarder or @Forwarder ='')
and (g.Dest = @Dest or @Dest ='')
and g.NonDeclare =0
and not exists (select * from KHExportDeclaration_Detail kdd2 where (kdd2.Invno=g.id or kdd2.LocalInvno=g.id) and  kdd2.OrderID=o.ID) 
and (kd_status.status = 'New' or kd_status.Status is null)
group by g.ID,pld.OrderID,o.StyleID,s.Description,g.BrandID,g.ShipModeID,o.PoPrice
,g.Forwarder,g.TotalNW,g.TotalGW
";
            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlcmd, out dt))
            {
                this.listControlBindingSource1.DataSource = dt;
            }
            else
            {
                this.ShowErr(sqlcmd, result);
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select at least one.", "Warnning");
                return;
            }

            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt_detail.AsEnumerable()
                    .Where(row => row.RowState != DataRowState.Deleted
                    && row["INVNo"].EqualString(tmp["INVNo"].ToString()) && row["OrderID"].EqualString(tmp["OrderID"])).ToArray();
                if (findrow.Length == 0)
                {
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    this.dt_detail.ImportRow(tmp);
                }
            }

            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
