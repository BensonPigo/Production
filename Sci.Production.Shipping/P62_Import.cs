using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Data;
using System.Linq;

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
            base.OnFormLoaded();

            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
               .CheckBox("selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
               .Text("INVNo", header: "Invoice No.", iseditingreadonly: true, width: Widths.AnsiChars(18))
               .Text("OrderID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(15))
               .Text("CustPONO", header: "PO#", iseditingreadonly: true, width: Widths.AnsiChars(15))
               .Text("StyleID", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(13))
               .Text("Description", header: "Style Description", iseditingreadonly: true, width: Widths.AnsiChars(30))
               .Text("Season", header: "Season", iseditingreadonly: true, width: Widths.AnsiChars(8))
               .Numeric("ShipModeSeqQty", header: "Qty", width: Widths.AnsiChars(9), decimal_places: 0, integer_places: 9, iseditingreadonly: true)
               .Numeric("CTNQty", header: "CTN", width: Widths.AnsiChars(9), decimal_places: 0, integer_places: 9, iseditingreadonly: true)
               .Numeric("FOB", header: "FOB", width: Widths.AnsiChars(9), decimal_places: 3, integer_places: 6, iseditingreadonly: true)
               .Numeric("NetKg", header: "N.W.", width: Widths.AnsiChars(9), decimal_places: 3, integer_places: 6, iseditingreadonly: true)
               .Numeric("WeightKg", header: "G.W.", width: Widths.AnsiChars(9), decimal_places: 3, integer_places: 6, iseditingreadonly: true)
               .Date("InvDate", header: "Invoice Date", width: Widths.AnsiChars(12), iseditingreadonly: true)
               .Text("Shipper", header: "Shipper", iseditingreadonly: true, width: Widths.AnsiChars(8))
               .Text("Dest", header: "Destination", iseditingreadonly: true, width: Widths.AnsiChars(8))
               .Text("Brand", header: "Brand", iseditingreadonly: true, width: Widths.AnsiChars(8))
               .Text("ShipmodeID", header: "Shipmode", iseditingreadonly: true, width: Widths.AnsiChars(8))
               .Text("Forwarder", header: "Forwarder", iseditingreadonly: true, width: Widths.AnsiChars(8))
               .Text("SONo", header: "S/O#", iseditingreadonly: true, width: Widths.AnsiChars(16))
               .Text("Remark", header: "Remark", iseditingreadonly: true, width: Widths.AnsiChars(25))
               ;
        }

        private void LoadData()
        {
            if ((MyUtility.Check.Empty(this.txtPo1.Text) && MyUtility.Check.Empty(this.txtPo2.Text)) &&
                (MyUtility.Check.Empty(this.dateETD.Value1) && MyUtility.Check.Empty(this.dateETD.Value2)) &&
                (MyUtility.Check.Empty(this.txtInvNo1.Text) && MyUtility.Check.Empty(this.txtInvNo2.Text)) &&
                (MyUtility.Check.Empty(this.txtSP1.Text) && MyUtility.Check.Empty(this.txtSP2.Text)))
            {
                MyUtility.Msg.WarningBox("Please input <Invoice No.>, <SP#>, <PO> or <ETD>.");
                return;
            }

            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.txtCustCD.Text))
            {
                where += $@"and g.CustCDID = '{this.txtCustCD.Text}'";
            }

            string sqlcmd = $@"
Declare @ID varchar(15) = '{this.dr_master["ID"]}'
Declare @ShipMode varchar(60) = '{this.dr_master["ShipModeID"]}'

select selected = 0 
, [INVNo] = g.id
,[PoNo] = o.CustPONo
, [OrderID] = pd.orderid
, [StyleUkey] = s.Ukey
, [StyleID] = o.styleid
, [Description] = s.description
, [Season] = o.SeasonID
, [ETD] = g.ETD
, [Brand] = g.BrandID
, [ShipmodeID] = g.ShipModeID
, [POPrice] = isnull(PoPrice.AvgPrice,o.PoPrice)
,[ShipModeSeqQty] = sum(pd.ShipQty)
,[CTNQty] = sum(pd.CTNQty)
,[FOB] = isnull(PoPrice.AvgPrice,o.PoPrice)
,[TtlFOB] = isnull(PoPrice.AvgPrice,o.PoPrice) * sum(pd.ShipQty)
,[ActTtlPOPrice] = isnull(PoPrice.AvgPrice,o.PoPrice) * sum(pd.ShipQty)
, [Forwarder] = g.Forwarder
,[NetKg] =  (case when @ShipMode in ('A/C','A/P') then sum(pd.NW) else
	(sum(pd.NW) + ( sum(pd.NW) * 0.05)) end)
,[WeightKg] = (case when @ShipMode in ('A/C','A/P') then sum(pd.GW) else
	(sum(pd.GW) + ( sum(pd.GW) * 0.05)) end)
,[ActNetKg] = (case when @ShipMode in ('A/C','A/P') then sum(pd.NW) else
	(sum(pd.NW) + ( sum(pd.NW) * 0.05)) end)
,[ActWeightKg] = (case when @ShipMode in ('A/C','A/P') then sum(pd.GW) else
	(sum(pd.GW) + ( sum(pd.GW) * 0.05)) end)
,[DiffNw] = 0
,[DiffGW] = 0
, [LocalINVNo] = g.id
, s.Description
, [HSCode] = ''
, [COFormType] = ''
, [COID] = ''
, [CODate] = null
-- Grid顯示
,[InvDate] = g.InvDate
,[Shipper] = g.Shipper
,[Dest] = g.Dest
,[Forwarder] = g.Forwarder
,[SONo] = g.SONo
,[Remark] = g.Remark
,g.CustCDID
from GMTBooking g
inner join PackingList p on g.ID = p.INVNo
inner join PackingList_Detail pd on pd.id = p.id
inner join Orders o on pd.OrderID = o.ID
inner join Style s on s.Ukey = o.StyleUkey
inner join Buyer b2 on o.BrandID =b2.id
outer apply (
	select kd.status 
		from KHExportDeclaration kd 
		where 1=1
		and kd.id=@ID
) kd_status
outer apply(
	select OrderID,AvgPrice = sum(ShipQty*POPrice)/sum(ShipQty)
	from (
		select ShipQty = sum(pd2.ShipQty),pd2.OrderID,oup.SizeCode,oup.Article ,oup.POPrice
		from PackingList_Detail pd2
		inner join PackingList p1 on pd2.ID = p1.ID
		inner join Order_UnitPrice oup on oup.Id= pd2.OrderID
		and oup.Article = pd2.Article and oup.SizeCode = pd2.SizeCode
		where INVNo = g.ID
		group by pd2.OrderID,oup.SizeCode,oup.Article,oup.POPrice
	) a
	where OrderID = o.ID
	group by OrderID
)POPrice
where 1=1
and g.ShipModeID = @ShipMode
and g.NonDeclare =0
and (kd_status.status = 'New' or kd_status.Status is null)
and not exists (select * from KHExportDeclaration_Detail kdd2 where (kdd2.Invno=g.id or kdd2.LocalInvno=g.id) and  kdd2.OrderID=o.ID) 
{where}
";
            #region Where
            if (!MyUtility.Check.Empty(this.dr_master["Buyer"]))
            {
                sqlcmd += $@" and b2.id =  '{this.dr_master["Buyer"]}'";
            }

            if (!MyUtility.Check.Empty(this.dr_master["Forwarder"]))
            {
                sqlcmd += $@" and g.Forwarder =  '{this.dr_master["Forwarder"]}'";
            }

            if (!MyUtility.Check.Empty(this.dr_master["Dest"]))
            {
                sqlcmd += $@" and g.Dest =  '{this.dr_master["Dest"]}'";
            }

            if (!MyUtility.Check.Empty(this.dateETD.Value1))
            {
                sqlcmd += $@" and g.ETD >= '{((DateTime)this.dateETD.Value1).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.dateETD.Value2))
            {
                sqlcmd += $@" and g.ETD <= '{((DateTime)this.dateETD.Value2).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.txtPo1.Text))
            {
                sqlcmd += $@" and o.CustPONo >= '{this.txtPo1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtPo2.Text))
            {
                sqlcmd += $@" and o.CustPONo <= '{this.txtPo2.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtInvNo1.Text))
            {
                sqlcmd += $@" and g.ID >= '{this.txtInvNo1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtInvNo2.Text))
            {
                sqlcmd += $@" and g.ID <= '{this.txtInvNo2.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSP1.Text))
            {
                sqlcmd += $@" and o.ID >= '{this.txtSP1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSP2.Text))
            {
                sqlcmd += $@" and o.ID <= '{this.txtSP2.Text}'";
            }
            #endregion

            sqlcmd += @" group by pd.OrderID,o.StyleID,s.Description,o.PoPrice,o.SeasonID,g.ID,s.Ukey,g.BrandID,g.ShipModeID,o.POPrice,g.Forwarder,g.InvDate,g.Shipper,g.Dest,g.SONo,g.Remark,PoPrice.AvgPrice,g.ETD,o.CustPONo";

            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt))
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
                    this.dt_detail.ImportRowAdded(tmp);
                }
            }

            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }
    }
}
