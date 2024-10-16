using Ict;
using Ict.Win;
using Newtonsoft.Json;
using Sci.Data;
using Sci.Production.CallPmsAPI;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static Sci.Production.CallPmsAPI.PackingA2BWebAPI_Model;

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
               .Date("ETD", header: "ETD", width: Widths.AnsiChars(12), iseditingreadonly: true)
               .Text("OrderID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(15))
               .Text("PoNo", header: "PO#", iseditingreadonly: true, width: Widths.AnsiChars(15))
               .Text("StyleID", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(13))
               .Text("LocationDisp", header: "Product Type", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("Description", header: "Style Description", iseditingreadonly: true, width: Widths.AnsiChars(30))
               .Text("Season", header: "Season", iseditingreadonly: true, width: Widths.AnsiChars(8))
               .Numeric("ShipModeSeqQty", header: "Qty(By SP)", width: Widths.AnsiChars(9), decimal_places: 0, integer_places: 9, iseditingreadonly: true)
               .Numeric("CTNQty", header: "CTN(By SP)", width: Widths.AnsiChars(9), decimal_places: 0, integer_places: 9, iseditingreadonly: true)
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

            #region Where
            string where = string.Empty;
            string whereGMT = string.Empty;
            if (!MyUtility.Check.Empty(this.dr_master["Buyer"]))
            {
                where += $@" and b2.id =  '{this.dr_master["Buyer"]}'";
            }

            if (!MyUtility.Check.Empty(this.dr_master["Forwarder"]))
            {
                where += $@" and g.Forwarder =  '{this.dr_master["Forwarder"]}'";
                whereGMT += $@" and g.Forwarder =  '{this.dr_master["Forwarder"]}'";
            }

            if (!MyUtility.Check.Empty(this.dr_master["Dest"]))
            {
                where += $@" and g.Dest =  '{this.dr_master["Dest"]}'";
                whereGMT += $@" and g.Dest =  '{this.dr_master["Dest"]}'";
            }

            if (!MyUtility.Check.Empty(this.dateETD.Value1))
            {
                where += $@" and g.ETD >= '{((DateTime)this.dateETD.Value1).ToString("yyyy/MM/dd")}'";
                whereGMT += $@" and g.ETD >= '{((DateTime)this.dateETD.Value1).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.dateETD.Value2))
            {
                where += $@" and g.ETD <= '{((DateTime)this.dateETD.Value2).ToString("yyyy/MM/dd")}'";
                whereGMT += $@" and g.ETD <= '{((DateTime)this.dateETD.Value2).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.txtPo1.Text))
            {
                where += $@" and o.CustPONo >= '{this.txtPo1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtPo2.Text))
            {
                where += $@" and o.CustPONo <= '{this.txtPo2.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtInvNo1.Text))
            {
                where += $@" and g.ID >= '{this.txtInvNo1.Text}'";
                whereGMT += $@" and g.ID >= '{this.txtInvNo1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtInvNo2.Text))
            {
                where += $@" and g.ID <= '{this.txtInvNo2.Text}'";
                whereGMT += $@" and g.ID <= '{this.txtInvNo2.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSP1.Text))
            {
                where += $@" and o.ID >= '{this.txtSP1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSP2.Text))
            {
                where += $@" and o.ID <= '{this.txtSP2.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtCustCD.Text))
            {
                where += $@"and g.CustCDID = '{this.txtCustCD.Text}'";
                whereGMT += $@"and g.CustCDID = '{this.txtCustCD.Text}'";
            }
            #endregion

            string sqlcmd = $@"
Declare @ID varchar(15) = '{this.dr_master["ID"]}'
Declare @ShipMode varchar(60) = '{this.dr_master["ShipModeID"]}'
Declare @OrderCompanyID NUMERIC(2,0) = {this.dr_master["OrderCompanyID"]}

select ShipQty = sum(pd2.ShipQty),pd2.OrderID,oup.SizeCode,oup.Article ,oup.POPrice
into #tmp_PackingList_Detail
from PackingList_Detail pd2 with (nolock)
inner join PackingList p2 with (nolock) on pd2.ID = p2.ID
inner join Order_UnitPrice oup with (nolock) on oup.Id= pd2.OrderID
											and oup.Article = pd2.Article 
											and oup.SizeCode = pd2.SizeCode
where exists (select 1 from {"{0}"} g with (nolock) where g.id = p2.INVNo)
AND p2.OrderCompanyID = @OrderCompanyID
group by pd2.OrderID,oup.SizeCode,oup.Article,oup.POPrice

select
	selected = 0 
	, [INVNo] = g.id
	, [PoNo] = o.CustPONo
	, [OrderID] = pd.orderid
	, [StyleUkey] = s.Ukey
	, [StyleID] = o.styleid
	, [Description] = s.description
	, [Season] = o.SeasonID
	, [ETD] = g.ETD
	, [Brand] = g.BrandID
	, [ShipmodeID] = g.ShipModeID
	, [POPrice] = cast(r.FOB as float)
	, [ShipModeSeqQty] = sum(pd.ShipQty)
	, [CTNQty] = sum(pd.CTNQty)
	, [FOB]
	, [TtlFOB] = cast(r.FOB * sum(pd.ShipQty) as float)
	, [ActTtlPOPrice] = cast(r.FOB * sum(pd.ShipQty) as float)
	, [DiffTtlFOB] = cast(r.FOB * sum(pd.ShipQty) - r.FOB * sum(pd.ShipQty) as float) -- [ActTtlPOPrice] - [TtlFOB]
	, [Forwarder] = g.Forwarder
	, [NetKg] = cast(round((sum(pd.NW) * isnull(OL.rate, 1) / 100),2) as float)
	, [WeightKg] = cast(round((sum(pd.GW) * isnull(OL.rate, 1) / 100),2) as float)
	, [ActNetKg] = cast(round((sum(pd.NW) * isnull(OL.rate, 1) / 100),2) as float)
	, [ActWeightKg] = cast(round((sum(pd.GW) * isnull(OL.rate, 1) / 100),2) as float)
	, [DiffNw] = 0
	, [DiffGW] = 0
	, [LocalINVNo] = g.id
	, s.Description
	, [HSCode] = ''
	, [COFormType] = ''
	, [COID] = ''
	, [CODate] = null
	-- Grid顯示
	, [InvDate] = g.InvDate
	, [Shipper] = g.Shipper
	, [Dest] = g.Dest
	, [Forwarder] = g.Forwarder
	, [SONo] = g.SONo
	, [Remark] = g.Remark
	, g.CustCDID
	, o.StyleUnit
    , OL.Location
	, LocationDisp = case
		when OL.Location = 'T' then 'TOP' 
        when OL.Location = 'B' then 'BOTTOM' 
        when OL.Location = 'I' then 'INNER'   
        when OL.Location = 'O' then 'OUTER'
        else '' end
from {"{0}"} g with (nolock)
inner join PackingList p with (nolock) on g.ID = p.INVNo
inner join PackingList_Detail pd with (nolock) on pd.id = p.id
inner join Orders o with (nolock) on pd.OrderID = o.ID
inner join Style s with (nolock) on s.Ukey = o.StyleUkey
inner join Brand b with (nolock) on b.id=o.BrandID 
inner join Buyer b2 with (nolock) on b.BuyerID =b2.id
left join Order_Location OL with (nolock) on OL.OrderID = pd.OrderID
outer apply (
	select kd.status 
		from KHExportDeclaration kd 
		where kd.id = @ID
) kd_status
outer apply(
	select OrderID,AvgPrice = sum(ShipQty*POPrice)/sum(ShipQty)
	from #tmp_PackingList_Detail a
	where OrderID = o.ID
	group by OrderID
)POPrice
outer apply(select FOB = isnull(PoPrice.AvgPrice,o.PoPrice) * isnull(OL.rate, 1) / 100)r
where 1=1
and g.ShipModeID = @ShipMode
and g.NonDeclare =0
and (kd_status.status = 'New' or kd_status.Status is null)
and not exists (select * from KHExportDeclaration_Detail kdd2 where (kdd2.Invno=g.id or kdd2.LocalInvno=g.id) and  kdd2.OrderID=o.ID) 
AND p.OrderCompanyID = @OrderCompanyID
{where}
group by pd.OrderID,o.StyleID,s.Description,o.PoPrice,o.SeasonID,g.ID,s.Ukey,g.BrandID,g.ShipModeID,o.POPrice,g.Forwarder,g.InvDate,g.Shipper,g.Dest,g.SONo,g.Remark,PoPrice.AvgPrice,g.ETD,o.CustPONo,g.CustCDID, o.StyleUnit,r.FOB, isnull(OL.rate, 1),OL.Location

";

            DualResult result;
            result = DBProxy.Current.Select(null, string.Format(sqlcmd, "GMTBooking"), out DataTable dt);

            if (!result)
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            #region check A2B

            string sqlGetA2BGMT = $@"
Declare @ID varchar(15) = '{this.dr_master["ID"]}'
Declare @ShipMode varchar(60) = '{this.dr_master["ShipModeID"]}'
Declare @OrderCompanyID NUMERIC(2,0) = {this.dr_master["OrderCompanyID"]}

select distinct g.id,
        g.ETD,
        g.BrandID,
        g.ShipModeID,
        g.Forwarder,
        g.InvDate,
        g.Shipper,
        g.Dest,
        g.SONo,
        g.Remark,
        g.CustCDID,
        g.NonDeclare,
        gd.PLFromRgCode
from GMTBooking g with (nolock)
inner join GMTBooking_Detail gd with (nolock) on gd.ID = g.ID
outer apply (
	select kd.status 
		from KHExportDeclaration kd 
		where kd.id = @ID
) kd_status
where   g.ShipModeID = @ShipMode and
        g.NonDeclare = 0 and
        (kd_status.status = 'New' or kd_status.Status is null)
        AND g.OrderCompanyID = @OrderCompanyID
        {whereGMT}
";

            DataTable dtA2BGMT;
            result = DBProxy.Current.Select(null, sqlGetA2BGMT, out dtA2BGMT);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dtA2BGMT.Rows.Count > 0)
            {
                List<string> listPLFromRgCode = new List<string>();

                listPLFromRgCode = dtA2BGMT.AsEnumerable().Select(s => s["PLFromRgCode"].ToString()).Distinct().ToList();

                foreach (string systemName in listPLFromRgCode)
                {
                    DataTable dtA2BResult;
                    DataBySql dataBySql = new DataBySql()
                    {
                        SqlString = string.Format(sqlcmd, "#tmp"),
                        TmpTable = JsonConvert.SerializeObject(dtA2BGMT),
                    };
                    result = PackingA2BWebAPI.GetDataBySql(systemName, dataBySql, out dtA2BResult);

                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }

                    dtA2BResult.MergeTo(ref dt);
                }
            }
            #endregion

            this.listControlBindingSource1.DataSource = dt;
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
                    && row["INVNo"].EqualString(tmp["INVNo"].ToString()) &&
                    row["OrderID"].EqualString(tmp["OrderID"]) &&
                    row["Location"].EqualString(tmp["Location"])).ToArray();
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
