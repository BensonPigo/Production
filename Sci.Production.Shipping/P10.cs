using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.CallPmsAPI;
using Sci.Production.Class.Command;
using Sci.Production.PublicPrg;
using Sci.Win.UI;
using static Sci.Production.CallPmsAPI.PackingA2BWebAPI;
using static Sci.Production.PublicPrg.Prgs;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P10
    /// </summary>
    public partial class P10 : Win.Tems.Input6
    {
        private Ict.Win.UI.DataGridViewDateBoxColumn col_inspdate;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_pulloutdate;
        private DataTable plData;
        private DataTable dtDeleteGBHistory;
        private DataSet allData = new DataSet();
        private int previousCompanySelectIndex = -1;

        /// <summary>
        /// P10
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultOrder = "AddDate";
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Visible = false;
            this.detailgrid.AllowUserToOrderColumns = true;
            this.InsertDetailGridOnDoubleClick = false;
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID;
            string whereDetail = (e.Master == null) ? "1=0" : string.Format("p.ShipPlanID ='{0}'", MyUtility.Convert.GetString(e.Master["ID"]));
            string sqlCmd = @"
select p.ID
, OrderID = STUFF((	
	select CONCAT(',',cast(a.OrderID as nvarchar)) 
	from (select distinct OrderID from PackingList_Detail pd WITH (NOLOCK) where pd.ID = p.id) a 
	for xml path('')),1,1,''
)
, BuyerDelivery = (
	select oq.BuyerDelivery 
	from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd WITH (NOLOCK) where pd.ID = p.ID) a
	, Order_QtyShip oq WITH (NOLOCK) 
	where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq
)
, p.Status
, p.CTNQty
, p.CBM
, ClogCTNQty = (
	select isnull(sum(CTNQty) ,0)
	from PackingList_Detail pd WITH (NOLOCK) 
	where pd.ID = p.ID and pd.ReceiveDate is not null
    and pd.CFAReceiveDate is null
)
, p.InspDate
, p.InspStatus
, p.PulloutDate
, p.InvNo
, p.MDivisionID
, p.ShipQty
, p.PulloutID
, [OrderShipmodeSeq] = STUFF ((select CONCAT (',', cast (a.OrderShipmodeSeq as nvarchar)) 
                                from (
                                    select pd.OrderShipmodeSeq 
                                    from PackingList_Detail pd WITH (NOLOCK) 
                                    left join AirPP ap With (NoLock) on pd.OrderID = ap.OrderID
                                                                        and pd.OrderShipmodeSeq = ap.OrderShipmodeSeq
                                    where pd.ID = p.id
                                    group by pd.OrderID, pd.OrderShipmodeSeq, ap.ID
                                ) a 
                                for xml path('')
                            ), 1, 1, '') 
							
, [OrderTtlQty] = STUFF ((select CONCAT (',', cast (a.Qty as nvarchar)) 
                    from (
                        select distinct o.id,o.Qty
                        from PackingList_Detail pd WITH (NOLOCK) 
                        inner join orders o with(nolock) on o.id= pd.orderid
                        where pd.ID = p.id
                    ) a 
					order by a.id
                    for xml path('')
                    ), 1, 1, '') 
, [ProdOutputTtlQty] = STUFF ((select CONCAT (',', cast (sum(sod.qaqty) as nvarchar)) 
                from (
                    select pd.OrderID
                    from PackingList_Detail pd WITH (NOLOCK) 
                    where pd.ID = p.id
                    group by pd.OrderID
                ) a 
                inner join SewingOutput_Detail_Detail sod with(nolock) on sod.orderid= a.orderid
				group by sod.OrderId
				order by sod.OrderId
                for xml path('')
                ), 1, 1, '') 
, IDD = STUFF ((select distinct CONCAT (',', Format(oqs.IDD, 'yyyy/MM/dd')) 
                            from PackingList_Detail pd WITH (NOLOCK) 
                            inner join Order_QtyShip oqs with (nolock) on oqs.ID = pd.OrderID and oqs.Seq = pd.OrderShipmodeSeq
                            where pd.ID = p.id and oqs.IDD is not null
                            for xml path('')
                          ), 1, 1, '') 
,[PLFromRgCode] = '{1}'
, CutOffDate = cast(g.CutOffDate as Date)
, p.ShippingReasonIDForTypeCO
, Description = s.Description
from PackingList p WITH (NOLOCK)
left join GMTBooking g WITH (NOLOCK) on p.INVNo = g.ID
left join ShippingReason s WITH (NOLOCK) on s.Type = 'CO' and s.ID = p.ShippingReasonIDForTypeCO
where {0} 
order by p.ID";
            DualResult result = DBProxy.Current.Select(null, string.Format(sqlCmd, whereDetail, string.Empty), out this.plData);

            if (!result)
            {
                this.ShowErr(result);
            }

            masterID = (e.Master == null) ? "1=0" : string.Format("g.ShipPlanID ='{0}'", MyUtility.Convert.GetString(e.Master["ID"]));
            string sqlGMTBooking_Detail = $@"
select distinct gd.PLFromRgCode 
from GMTBooking_Detail gd with (nolock) 
where exists(select 1 from GMTBooking g with (nolock) where g.ID = gd.ID and {masterID} )";

            DataTable dtPLFromRgCode;
            result = DBProxy.Current.Select(null, sqlGMTBooking_Detail, out dtPLFromRgCode);

            if (!result)
            {
                this.ShowErr(result);
                goto continuMain;
            }
            else
            {
                foreach (DataRow drPLFromRgCode in dtPLFromRgCode.Rows)
                {
                    DataTable dtPlDataA2B;
                    string plFromRgCode = drPLFromRgCode["PLFromRgCode"].ToString();
                    result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, string.Format(sqlCmd, whereDetail, plFromRgCode), out dtPlDataA2B);
                    if (!result)
                    {
                        this.ShowErr(result);
                        goto continuMain;
                    }

                    dtPlDataA2B.MergeTo(ref this.plData);
                }
            }

        continuMain:
            string sqlpullout = $@"
ALTER Table #tmp ALTER Column PulloutID Varchar(13)

Select t.*, Pullout.SendToTPE From #tmp t
Left join Pullout WITH (NOLOCK)  on Pullout.ID = t.PulloutID and Pullout.Status <> 'NEW'";

            if (!(result = MyUtility.Tool.ProcessWithDatatable(this.plData, null, sqlpullout, out this.plData)))
            {
                this.ShowErr(result);
            }

            masterID = (e.Master == null) ? "1=0" : string.Format("g.ShipPlanID ='{0}'", MyUtility.Convert.GetString(e.Master["ID"]));
            this.DetailSelectCommand = string.Format(
                @"
select g.ID
, g.BrandID
, g.ShipModeID
, Forwarder = (g.Forwarder+' - '+(select ls.Abb from LocalSupp ls WITH (NOLOCK) where ls.ID = g.Forwarder)) 
, g.CYCFS
, g.SONo
, g.CutOffDate
, g.ForwarderWhse_DetailUKey
, WhseNo = isnull(fw.WhseCode,'')
, Status = iif(g.Status='Confirmed','GB Confirmed',iif(g.SOCFMDate is null,'','S/O Confirmed'))
, [TotalCTNQty] = isnull(g.TotalCTNQty,0)
, g.TotalCBM
, g.TotalGW
, G.FCRDate
, ClogCTNQty = (
	select isnull(sum(pd.CTNQty),0) from PackingList p WITH (NOLOCK) ,PackingList_Detail pd WITH (NOLOCK) 
	where p.INVNo = g.ID and p.ID = pd.ID and pd.ReceiveDate is not null
    and pd.CFAReceiveDate is null
)
,[TotalShipQty] =  isnull(g.TotalShipQty,0)
from GMTBooking g WITH (NOLOCK) 
left join ForwarderWarehouse_Detail fd WITH (NOLOCK) on g.ForwarderWhse_DetailUKey = fd.UKey
left join ForwarderWarehouse fw WITH (NOLOCK) on fd.id = fw.id
where {0} 
order by g.ID", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        private DualResult RefreshDetailClogCTNQtyFromA2B()
        {
            DualResult result;
            string sqlGMTBooking_Detail = $@"
select distinct gd.PLFromRgCode 
from GMTBooking_Detail gd with (nolock) 
where exists(select 1 from GMTBooking g with (nolock) where g.ID = gd.ID and g.ShipPlanID ='{this.CurrentMaintain["ID"]}' )";

            DataTable dtPLFromRgCode;
            result = DBProxy.Current.Select(null, sqlGMTBooking_Detail, out dtPLFromRgCode);

            if (!result)
            {
                return result;
            }

            if (dtPLFromRgCode.Rows.Count == 0)
            {
                return new DualResult(true);
            }

            string sqlCmd = $@"
select  p.INVNo,
        [ClogCTNQty] = isnull(sum(pd.CTNQty),0) 
from PackingList p WITH (NOLOCK) ,PackingList_Detail pd WITH (NOLOCK) 
	where   p.ShipPlanID ='{this.CurrentMaintain["ID"]}' and 
            p.ID = pd.ID and 
            pd.ReceiveDate is not null and
            pd.CFAReceiveDate is null
group by    p.INVNo
";

            foreach (DataRow drPLFromRgCode in dtPLFromRgCode.Rows)
            {
                DataTable dtClogCTNQtyByInvNo;
                result = PackingA2BWebAPI.GetDataBySql(drPLFromRgCode["PLFromRgCode"].ToString(), sqlCmd, out dtClogCTNQtyByInvNo);
                if (!result)
                {
                    return result;
                }

                foreach (DataRow dr in dtClogCTNQtyByInvNo.Rows)
                {
                    var matchInvNoDatas = this.DetailDatas.Where(s => s["ID"].ToString() == dr["INVNo"].ToString());
                    foreach (var matchedItem in matchInvNoDatas)
                    {
                        matchedItem["ClogCTNQty"] = MyUtility.Convert.GetInt(matchedItem["ClogCTNQty"]) + MyUtility.Convert.GetInt(dr["ClogCTNQty"]);
                    }
                }
            }

            return new DualResult(true);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (!this.EditMode)
            {
                this.comboCompany1.IsOrderCompany = null;
                this.comboCompany1.Junk = null;
                if (this.CurrentMaintain != null && !MyUtility.Check.Empty(this.CurrentMaintain["OrderCompanyID"]))
                {
                    this.comboCompany1.SelectedValue = (object)this.CurrentMaintain["OrderCompanyID"];
                }
            }

            this.btnUpdatePulloutDate.Enabled = !this.EditMode && MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) != "Confirmed" && Prgs.GetAuthority(Env.User.UserID, "P10. Ship Plan", "CanEdit");
            this.SumData();
            string sqlctnr = $@"
declare @ShipPlanID varchar(25) = '{this.CurrentMaintain["id"]}'
select concat(
'20 STD=',(
SELECT COUNT(Type) FROM (
select DISTINCT gc.Type,gc.CTNRno
from GMTBooking_CTNR gc with(nolock)
inner join GMTBooking g with(nolock) on gc.id = g.id
where g.ShipPlanID =@ShipPlanID and type = '20 STD')a
)
,' ; '
,'40 STD=',(
SELECT COUNT(Type) FROM (
select DISTINCT gc.Type,gc.CTNRno
from GMTBooking_CTNR gc with(nolock)
inner join GMTBooking g with(nolock) on gc.id = g.id
where g.ShipPlanID =@ShipPlanID and type = '40 STD')b
)
,' ; '
,'40HQ=',(
SELECT COUNT(Type) FROM (
select DISTINCT gc.Type,gc.CTNRno
from GMTBooking_CTNR gc with(nolock)
inner join GMTBooking g with(nolock) on gc.id = g.id
where g.ShipPlanID =@ShipPlanID and type = '40HQ')c
)
,' ; '
,'45HQ=',(
SELECT COUNT(Type) FROM (
select DISTINCT gc.Type,gc.CTNRno
from GMTBooking_CTNR gc with(nolock)
inner join GMTBooking g with(nolock) on gc.id = g.id
where g.ShipPlanID =@ShipPlanID and type = '45HQ')d
)
)
";
            this.displayTTLContainer.Text = MyUtility.GetValue.Lookup(sqlctnr);

            sqlctnr = string.Format(
                @"
select [Type] = 'T'
	, sdh.ID
	, sdh.GMTBookingID
	, sdh.ReasonID
	, [Description] = sr.Description
	, sdh.BackDate
	, sdh.NewShipModeID
	, sdh.NewPulloutDate
	, sdh.NewDestination
	, sdh.Remark
	, sdh.AddName
	, sdh.AddDate
from ShipPlan_DeleteGBHistory sdh
left join ShippingReason sr on sdh.ReasonID = sr.ID
where sdh.ID = '{0}'", this.CurrentMaintain["id"]);
            DualResult result = DBProxy.Current.Select(null, sqlctnr, out this.dtDeleteGBHistory);
            this.btnDeleteGBHistory.Enabled = false;
            this.btnDeleteGBHistory.ForeColor = Color.Black;
            if (this.dtDeleteGBHistory != null && this.dtDeleteGBHistory.Rows.Count > 0)
            {
                this.btnDeleteGBHistory.Enabled = true;
                this.btnDeleteGBHistory.ForeColor = Color.Red;
            }

            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt != null)
            {
                this.disPulloutDate.Text = MyUtility.Convert.GetDate(dt.Compute("MIN(PulloutDate)", string.Empty)).ToStringEx("yyyy/MM/dd");
            }
        }

        private void SumData()
        {
            DataTable tmp_dt = (DataTable)this.detailgridbs.DataSource;
            if (tmp_dt == null)
            {
                return;
            }

            if (tmp_dt.Rows.Count > 0)
            {
                this.numericBoxTTLCTN.Value = MyUtility.Convert.GetDecimal(tmp_dt.Compute("Sum(TotalCTNQty)", string.Empty));
                this.numericBoxTTLQTY.Value = MyUtility.Convert.GetDecimal(tmp_dt.Compute("Sum(TotalShipQty)", string.Empty));
                this.numericBoxTTLCBM.Value = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(tmp_dt.Compute("Sum(TotalCBM)", string.Empty)), 2);
                this.numericBoxTTLGW.Value = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(tmp_dt.Compute("Sum(TotalGW)", string.Empty)), 2);
            }
            else
            {
                this.numericBoxTTLCTN.Value = 0;
                this.numericBoxTTLQTY.Value = 0;
                this.numericBoxTTLCBM.Value = 0;
                this.numericBoxTTLGW.Value = 0;
            }

            if (tmp_dt.Select("CYCFS in ('CFS-CFS','CFS-CY') and ShipModeID not in ('A/C','A/P','E/C','E/P','A/P-C','E/P-C','AIR')").Count() > 0)
            {
                this.numericBoxCFSCBM.Value = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(tmp_dt.Compute("Sum(TotalCBM)", "CYCFS in ('CFS-CFS','CFS-CY') and ShipModeID not in ('A/C','A/P','E/C','E/P','A/P-C','E/P-C','AIR')")), 2);
            }
            else
            {
                this.numericBoxCFSCBM.Value = 0;
            }

            if (tmp_dt.Select("ShipModeID in('A/C','A/P','E/C','E/P','A/P-C','E/P-C','AIR')").Count() > 0)
            {
                this.numericBoxAIRGW.Value = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(tmp_dt.Compute("Sum(TotalGW)", "ShipModeID in('A/C','A/P','E/C','E/P','A/P-C','E/P-C','AIR')")), 2);
            }
            else
            {
                this.numericBoxAIRGW.Value = 0;
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailUIConvertToMaintain()
        {
            base.OnDetailUIConvertToMaintain();
            this.gridDetailPackingList.IsEditingReadOnly = false;
        }

        /// <inheritdoc/>
        protected override void OnDetailUIConvertToView()
        {
            base.OnDetailUIConvertToView();
            this.gridDetailPackingList.IsEditingReadOnly = true;
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("ID", header: "GB#", width: Widths.AnsiChars(22), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("ShipModeID", header: "Ship Mode", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Forwarder", header: "Forwarder", width: Widths.AnsiChars(17), iseditingreadonly: true)
                .Text("CYCFS", header: "Loading Type", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("SONo", header: "S/O No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .DateTime("CutOffdate", header: "Cut-off Date/Time", iseditingreadonly: true)
                .Date("FCRDate", header: "FCR Date", iseditingreadonly: true)
                .Numeric("WhseNo", header: "Container Terminals", iseditingreadonly: true)
                .Text("Status", header: "GB Status", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Numeric("TotalShipQty", header: "TTL Qty", iseditingreadonly: true)
                .Numeric("TotalCTNQty", header: "TTL CTN", iseditingreadonly: true)
                .Numeric("TotalCBM", header: "Total CBM", decimal_places: 2, iseditingreadonly: true)
                .Numeric("ClogCTNQty", header: "Total CTN Q'ty at C-Logs", iseditingreadonly: true);
            this.detailgrid.SelectionChanged += (s, e) =>
            {
                this.gridDetailPackingList.ValidateControl();
                this.DetailFilter();
            };

            this.gridDetailPackingList.DataSource = this.listControlBindingSource1;
            this.gridDetailPackingList.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridDetailPackingList)
                .Text("ID", header: "Packing No.", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("OrderShipmodeSeq", header: "Seq", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Delivery", iseditingreadonly: true)
                .Text("IDD", header: "Intended Delivery", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Status", header: "Packing Status", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Text("OrderTtlQty", header: "Order Ttl Qty", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("ProdOutputTtlQty", header: "Prod. Output Ttl Qty", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("ShipQty", header: "TTL Qty", iseditingreadonly: true)
                .Numeric("CTNQty", header: "TTL CTN", iseditingreadonly: true)
                .Numeric("CBM", header: "CBM", decimal_places: 3, iseditingreadonly: true)
                .Numeric("ClogCTNQty", header: "CTN Q'ty at C-Logs", iseditingreadonly: true)
                .Date("InspDate", header: "Est. Inspection Date").Get(out this.col_inspdate)
                .Text("InspStatus", header: "Inspection Status", width: Widths.AnsiChars(10))
                .Date("PulloutDate", header: "Pullout Date").Get(out this.col_pulloutdate)
                .Text("PulloutID", header: "Pullout ID#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("SendToTPE", header: "Send to SCI", iseditingreadonly: true)
                ;
            #region 欄位值檢查
            this.gridDetailPackingList.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                    DataRow dr = this.gridDetailPackingList.GetDataRow<DataRow>(e.RowIndex);
                    if (this.gridDetailPackingList.Columns[e.ColumnIndex].DataPropertyName == this.col_inspdate.DataPropertyName)
                    {
                        if (!MyUtility.Check.Empty(e.FormattedValue))
                        {
                            if (MyUtility.Convert.GetDate(e.FormattedValue) != MyUtility.Convert.GetDate(dr["InspDate"]))
                            {
                                if (MyUtility.Convert.GetDate(e.FormattedValue) > DateTime.Today.AddMonths(1) || MyUtility.Convert.GetDate(e.FormattedValue) < DateTime.Today.AddMonths(-1))
                                {
                                    MyUtility.Msg.WarningBox("< Est. Inspection Date > is invalid!!");
                                    dr["InspDate"] = null;
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }

                    // 輸入的Pullout date或原本的Pullout date的Pullout Report如果已經Confirmed的話，就不可以被修改
                    if (this.gridDetailPackingList.Columns[e.ColumnIndex].DataPropertyName == this.col_pulloutdate.DataPropertyName)
                    {
                        if (MyUtility.Convert.GetDate(e.FormattedValue) != MyUtility.Convert.GetDate(dr["PulloutDate"]))
                        {
                            if (!MyUtility.Check.Empty(dr["PulloutDate"]) && this.CheckPullout((DateTime)MyUtility.Convert.GetDate(dr["PulloutDate"]), MyUtility.Convert.GetString(dr["MDivisionID"])))
                            {
                                this.PulloutMsg(dr, (DateTime)MyUtility.Convert.GetDate(dr["PulloutDate"]));
                                e.Cancel = true;
                                dr.EndEdit();
                                return;
                            }

                            if (!MyUtility.Check.Empty(e.FormattedValue) && this.CheckPullout((DateTime)MyUtility.Convert.GetDate(e.FormattedValue), MyUtility.Convert.GetString(dr["MDivisionID"])))
                            {
                                this.PulloutMsg(dr, (DateTime)MyUtility.Convert.GetDate(e.FormattedValue));
                                e.Cancel = true;
                                dr.EndEdit();
                                return;
                            }
                        }
                    }
                }
            };
            this.gridDetailPackingList.CellValidated += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;

                    // 輸入的Pullout date或原本的Pullout date的Pullout Report如果已經Confirmed的話，就不可以被修改
                    if (this.gridDetailPackingList.Columns[e.ColumnIndex].DataPropertyName == this.col_pulloutdate.DataPropertyName)
                    {
                        this.disPulloutDate.Text = MyUtility.Convert.GetDate(dt.Compute("MIN(PulloutDate)", string.Empty)).ToStringEx("yyyy/MM/dd");
                    }
                }
            };
            #endregion
        }

        /// <inheritdoc/>
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            this.listControlBindingSource1.DataSource = this.plData;
            return base.OnRenewDataDetailPost(e);
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            this.comboCompany1.IsOrderCompany = true;
            this.comboCompany1.Junk = false;
            this.comboCompany1.SelectedIndex = -1;
            base.ClickNewAfter();
            this.CurrentMaintain["CDate"] = DateTime.Today;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.disPulloutDate.Text = string.Empty;
            this.listControlBindingSource1.DataSource = this.plData;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            DualResult result = this.RefreshDetailClogCTNQtyFromA2B();
            if (!result)
            {
                return result;
            }

            // 如有問題請查詢：ISP20230067
            // #region  表身任一筆Orders.ID的Orders.GMTComplete 不可為 'S'
            // bool gMTCompleteCheck = this.GMTCompleteCheck();
            // if (!gMTCompleteCheck)
            // {
            //    return false;
            // }
            // #endregion
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Confirmed")
            {
                MyUtility.Msg.WarningBox(string.Format("This record status is < {0} >, can't be edit!", MyUtility.Convert.GetString(this.CurrentMaintain["Status"])));
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailViewPost()
        {
            DualResult result = this.RefreshDetailClogCTNQtyFromA2B();
            if (!result)
            {
                return result;
            }

            return base.OnDetailViewPost();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.comboCompany1.ReadOnly = true;
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) != "New")
            {
                this.btnImportData.Enabled = false;
            }
        }

        /// <inheritdoc/>
        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.RenewData();
            this.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            // 表身GMTBooking.ShipModeID 不存在Order_QtyShip 就return
            if (!this.ChkShipMode())
            {
                return false;
            }

            this.CheckIDD();

            // GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Env.User.Keyword + "SH", "ShipPlan", DateTime.Today, 3, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = id;
            }

            if (this.dtDeleteGBHistory != null && this.dtDeleteGBHistory.Rows.Count > 0)
            {
                var query = this.dtDeleteGBHistory.AsEnumerable().Where(x => x.Field<string>("ReasonID").Empty());
                if (query.Any())
                {
                    MyUtility.Msg.WarningBox("Delete GB History Reason cannot be empty!");
                    return false;
                }
            }

            DataTable dt = this.plData;
            DataTable disp = new DataTable();
            disp.Columns.Add("GB#");
            disp.Columns.Add("Packing No.");
            disp.Columns.Add("SP#");
            disp.Columns.Add("Seq");
            disp.Columns.Add("Pullout Date");

            // PulloutDate日期重複判斷
            var duplicItem = dt.ExtNotDeletedRows()
              .GroupBy(item => new
              {
                  pulloutDate = item["PulloutDate"],
              })
              .Select(item => new
              {
                  item.Key,
                  cnt = item.Count(),
              })
              .Select(item => item);
            if (duplicItem.Count() > 1)
            {
                foreach (DataRow dr2 in dt.Rows)
                {
                    DataRow dispdr = disp.NewRow();
                    dispdr["GB#"] = dr2["InvNo"];
                    dispdr["Packing No."] = dr2["ID"];
                    dispdr["SP#"] = dr2["OrderID"];
                    dispdr["Seq"] = dr2["OrderShipmodeSeq"];
                    dispdr["Pullout Date"] = MyUtility.Convert.GetDate(dr2["PulloutDate"]).ToYYYYMMDD();
                    disp.Rows.Add(dispdr);
                }

                var m = new Win.UI.MsgGridForm(disp, "The following Pullout Date needs to be the same", "Pullout Date needs to be the same", null, MessageBoxButtons.OK)
                {
                    Width = 650,
                };
                m.grid1.Columns[0].Width = 170;
                m.grid1.Columns[1].Width = 120;
                m.grid1.Columns[2].Width = 130;
                m.grid1.Columns[3].Width = 60;
                m.grid1.Columns[4].Width = 130;
                m.text_Find.Width = 140;
                m.btn_Find.Location = new Point(150, 6);
                m.btn_Find.Anchor = AnchorStyles.Left | AnchorStyles.Top;

                m.ShowDialog();

                return false;
            }

            bool needReason = dt.Select("CutOffDate < PulloutDate").Any();

            if (needReason)
            {
                var frm = new P10_PulloutDateReason(dt);
                DialogResult returnResult = frm.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    this.DetailFilter();
                    return false;
                }
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        private void DetailFilter()
        {
            DataRow dr = this.detailgrid.GetDataRow<DataRow>(this.detailgrid.GetSelectedRowIndex());
            if (dr != null && this.plData != null)
            {
                string filter = string.Format("InvNo = '{0}'", MyUtility.Convert.GetString(dr["ID"])); 
                this.plData.DefaultView.RowFilter = filter;
            }
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            if (this.dtDeleteGBHistory != null && this.dtDeleteGBHistory.Rows.Count > 0)
            {
                string sql =
@"update sdh
	set sdh.ReasonID = t.ReasonID
	  , sdh.BackDate = t.BackDate
	  , sdh.NewShipModeID = t.NewShipModeID
	  , sdh.NewPulloutDate = t.NewPulloutDate
	  , sdh.NewDestination = t.NewDestination
	  , sdh.Remark = t.Remark
	  , sdh.AddName = t.AddName
	  , sdh.AddDate = GETDATE()
from ShipPlan_DeleteGBHistory sdh
inner join #tmp t on sdh.ID = t.ID and sdh.GMTBookingID = t.GMTBookingID
and (sdh.ReasonID <> t.ReasonID
    or sdh.BackDate <> t.BackDate
    or sdh.NewShipModeID <> t.NewShipModeID
    or sdh.NewPulloutDate <> t.NewPulloutDate
    or sdh.NewDestination <> t.NewDestination
    or sdh.Remark <> t.Remark
    or sdh.AddName <> t.AddName)

insert into ShipPlan_DeleteGBHistory(ID, GMTBookingID, ReasonID, BackDate, NewShipModeID, NewPulloutDate, NewDestination, Remark, AddName, AddDate)
select t.ID, t.GMTBookingID, t.ReasonID, t.BackDate, t.NewShipModeID, t.NewPulloutDate, t.NewDestination, t.Remark, t.AddName, GETDATE()
from #tmp t
where not exists (select 1 from ShipPlan_DeleteGBHistory sdh where sdh.ID = t.ID and sdh.GMTBookingID = t.GMTBookingID)
";

                DualResult result = MyUtility.Tool.ProcessWithDatatable(this.dtDeleteGBHistory, null, sql, out DataTable dt);
                if (!result)
                {
                    return result;
                }
            }

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();

            DualResult result = this.RefreshDetailClogCTNQtyFromA2B();
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        /// <inheritdoc/>
        protected override DualResult OnSaveDetail(IList<DataRow> details, ITableSchema detailtableschema)
        {
            IList<string> updateCmds = new List<string>();
            Dictionary<string, IList<string>> updateCmdsA2B = new Dictionary<string, IList<string>>();
            this.gridDetailPackingList.EndEdit();
            this.listControlBindingSource1.EndEdit();

            DataTable packingListDt = (DataTable)this.listControlBindingSource1.DataSource;

            // 未篩選過的packingList
            DataTable packingList_List = (DataTable)this.listControlBindingSource1.DataSource;

            foreach (DataRow dr in details)
            {
                if (dr.RowState == DataRowState.Modified || dr.RowState == DataRowState.Added)
                {
                    updateCmds.Add(string.Format("update GMTBooking set ShipPlanID = '{0}' where ID = '{1}';", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(dr["ID"])));
                    string invNo = MyUtility.Convert.GetString(dr["ID"]);

                    // 根據GMTBooking.ID篩選出對應的PackingList，這些是預先存在Form裡面的，不是DB裡面最新的資料
                    DataTable packingListDt_on_Form = null;
                    var rows = packingList_List.AsEnumerable().Where(o => o["INVNo"].ToString() == invNo);

                    if (rows.Any())
                    {
                        packingListDt_on_Form = rows.CopyToDataTable();
                    }

                    // DB可能已經被修改，因此回DB撈出GMTBooking底下最新的PackingList
                    DataTable packingListDt_new;
                    string sqlCmd = "SELECT ID, InspDate, InspStatus, PulloutDate, ShippingReasonIDForTypeCO, [PLFromRgCode] = '{1}' FROM PackingList WHERE INVNo='{0}' ";
                    DualResult result = DBProxy.Current.Select(null, string.Format(sqlCmd, invNo, string.Empty), out packingListDt_new);

                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Update fail!!\r\n" + result.ToString());
                        return failResult;
                    }

                    DataTable dtPLFromRgCode;
                    string sqlGetPLFromRgCode = $"select distinct PLFromRgCode from GMTBooking_Detail with (nolock) where ID = '{invNo}'";

                    result = DBProxy.Current.Select(null, sqlGetPLFromRgCode, out dtPLFromRgCode);
                    if (!result)
                    {
                        return result;
                    }

                    foreach (DataRow drPLFromRgCode in dtPLFromRgCode.Rows)
                    {
                        DataTable dtpPckingListDt_newA2B;
                        string plFromRgCode = drPLFromRgCode["PLFromRgCode"].ToString();

                        result = PackingA2BWebAPI.GetDataBySql(drPLFromRgCode["PLFromRgCode"].ToString(), string.Format(sqlCmd, invNo, plFromRgCode), out dtpPckingListDt_newA2B);
                        if (!result)
                        {
                            return result;
                        }

                        dtpPckingListDt_newA2B.MergeTo(ref packingListDt_new);
                    }

                    foreach (DataRow pldatarow in packingListDt_new.Rows)
                    {
                        // 因為User會修改InspDate、InspStatus、PulloutDate三個欄位，這些欄會在Form上面，因此要把Form上面的PackingList、DB裡的PackingList比對
                        // packingListDt_new是最新的PackingList清單，packingListDt_on_Form是User修改過的PackingList清單
                        DataTable packingList_Merge = null;
                        if (packingListDt_on_Form == null)
                        {
                            return new DualResult(false, $"<GB#>:{MyUtility.Convert.GetString(dr["ID"])} does not have <Packinglist#>");
                        }

                        var rows1 = packingListDt_on_Form.AsEnumerable().Where(o => o["ID"].ToString() == pldatarow["ID"].ToString());
                        if (rows1.Any())
                        {
                            packingList_Merge = rows1.CopyToDataTable();
                        }

                        // 如果該筆資料packingListDt_on_Form有，但不存在packingListDt_new，表示這筆PackingList是被Import到Form上面後，其他功能在DB裡面異動過
                        // 這類的PackingList就不UPDATE
                        if (packingList_Merge.Rows.Count > 0)
                        {
                            pldatarow["InspDate"] = packingList_Merge.Rows[0]["InspDate"];
                            pldatarow["InspStatus"] = packingList_Merge.Rows[0]["InspStatus"];
                            pldatarow["PulloutDate"] = packingList_Merge.Rows[0]["PulloutDate"];
                            pldatarow["ShippingReasonIDForTypeCO"] = packingList_Merge.Rows[0]["ShippingReasonIDForTypeCO"];
                            string updatePlSql = this.UpdatePLCmd(pldatarow, MyUtility.Convert.GetString(dr["ID"]));
                            if (MyUtility.Check.Empty(pldatarow["PLFromRgCode"]))
                            {
                                updateCmds.Add(updatePlSql);
                            }
                            else
                            {
                                string pulloutDateYYYYMMDD = MyUtility.Check.Empty(pldatarow["PulloutDate"]) ? string.Empty : Convert.ToDateTime(pldatarow["PulloutDate"]).ToString("yyyyMMdd");

                                if (MyUtility.Check.Empty(pulloutDateYYYYMMDD))
                                {
                                    updateCmds.Add(string.Format("update GMTBooking_Detail set PulloutDate = null where PackingListID = '{0}'; ", MyUtility.Convert.GetString(pldatarow["ID"])));
                                }
                                else
                                {
                                    updateCmds.Add(string.Format("update GMTBooking_Detail set PulloutDate = '{0}' where PackingListID = '{1}'; ", pulloutDateYYYYMMDD, MyUtility.Convert.GetString(pldatarow["ID"])));
                                }

                                string plFromRgCode = pldatarow["PLFromRgCode"].ToString();
                                if (!updateCmdsA2B.ContainsKey(plFromRgCode))
                                {
                                    updateCmdsA2B.Add(plFromRgCode, new List<string>());
                                }

                                updateCmdsA2B[plFromRgCode].Add(updatePlSql);
                            }
                        }

                        continue;
                    }

                    continue;
                }

                if (dr.RowState == DataRowState.Deleted)
                {
                    updateCmds.Add(string.Format("update GMTBooking set ShipPlanID = '' where ID = '{0}';", MyUtility.Convert.GetString(dr["ID", DataRowVersion.Original])));

                    string updateSqlDeletePacking = this.DeletePLCmd("InvNo", MyUtility.Convert.GetString(dr["ID", DataRowVersion.Original]));
                    updateCmds.Add(updateSqlDeletePacking);
                    List<string> listPLFromRgCode = PackingA2BWebAPI.GetPLFromRgCodeByInvNo(dr["ID", DataRowVersion.Original].ToString());

                    foreach (string plFromRgCode in listPLFromRgCode)
                    {
                        if (!updateCmdsA2B.ContainsKey(plFromRgCode))
                        {
                            updateCmdsA2B.Add(plFromRgCode, new List<string>());
                        }

                        updateCmdsA2B[plFromRgCode].Add(updateSqlDeletePacking);
                    }

                    continue;
                }
            }

            // 執行更新
            if (updateCmds.Count != 0)
            {
                DualResult result;

                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        result = DBProxy.Current.Executes(null, updateCmds);
                        if (!result)
                        {
                            scope.Dispose();
                            DualResult failResult = new DualResult(false, "Update fail!!\r\n" + result.ToString());
                            return failResult;
                        }

                        foreach (KeyValuePair<string, IList<string>> updA2BItems in updateCmdsA2B)
                        {
                            string updateA2BSQL = updA2BItems.Value.JoinToString(string.Empty);
                            result = PackingA2BWebAPI.ExecuteBySql(updA2BItems.Key, updateA2BSQL);

                            if (!result)
                            {
                                scope.Dispose();
                                DualResult failResult = new DualResult(false, "Update fail!!\r\n" + result.ToString());
                                return failResult;
                            }
                        }

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        scope.Dispose();
                        DualResult failResult = new DualResult(false, "Update fail!!\r\n" + ex.Message.ToString());
                        return failResult;
                    }
                }
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) != "New")
            {
                MyUtility.Msg.WarningBox(string.Format("This record status is < {0} >, can't be delete!", MyUtility.Convert.GetString(this.CurrentMaintain["Status"])));
                return false;
            }

            if (this.plData.AsEnumerable().Any(a => !MyUtility.Check.Empty(a["pulloutdate"])))
            {
                MyUtility.Msg.WarningBox("Can't delete this ship plan! already has pullout date!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override DualResult OnDeleteDetails()
        {
            List<string> updateCmds = new List<string>();
            updateCmds.Add(string.Format("update GMTBooking set ShipPlanID = '' where ShipPlanID = '{0}';", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));

            string sqlDeletePackingByShipPlanID = this.DeletePLCmd("ShipPlanID", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            updateCmds.Add(sqlDeletePackingByShipPlanID);

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    List<string> listPLFromRgCode = PackingA2BWebAPI.GetPLFromRgCodeByShipPlanID(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

                    DualResult result = DBProxy.Current.Executes(null, updateCmds);

                    if (!result)
                    {
                        scope.Dispose();
                        return result;
                    }

                    foreach (string plFromRgCode in listPLFromRgCode)
                    {
                        result = PackingA2BWebAPI.ExecuteBySql(plFromRgCode, sqlDeletePackingByShipPlanID);
                        if (!result)
                        {
                            scope.Dispose();
                            return result;
                        }
                    }

                    scope.Complete();
                    scope.Dispose();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return new DualResult(false, ex);
                }
            }

            return new DualResult(true);
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                return false;
            }

            List<string> listPLFromRgCode = PackingA2BWebAPI.GetPLFromRgCodeByShipPlanID(this.CurrentMaintain["ID"].ToString());

            string sqlCmd = string.Format(
                @"select 
p.ShipPlanID,
p.INVNo,
g.BrandID,
g.ShipModeID, 
[Forwarder] = (g.Forwarder+'-'+ls.Abb), 
g.CYCFS,
g.SONo,
g.CutOffDate,
[WhseNo] = concat(fw.WhseCode,'-',fw.Address),
[Status] = iif(g.Status='Confirmed','GB Confirmed',iif(g.SOCFMDate is null,'','S/O Confirmed')),
g.TotalCTNQty,
g.TotalCBM,
[ClogCTNQty] = (select isnull(sum(pd1.CTNQty),0) 
                from PackingList p1 WITH (NOLOCK) ,
                PackingList_Detail pd1 WITH (NOLOCK) where p1.INVNo = p.INVNo and p1.ID = pd1.ID and pd1.ReceiveDate is not null),
p.ID,
[OrderID] = (select cast(a.OrderID as nvarchar) +',' 
                from (select distinct OrderID 
                        from PackingList_Detail pd WITH (NOLOCK) where pd.ID = p.id) a for xml path('')),
[BuyerDelivery] = (select oq.BuyerDelivery from (select top 1 OrderID, OrderShipmodeSeq 
                    from PackingList_Detail pd WITH (NOLOCK) where pd.ID = p.ID) a, 
                        Order_QtyShip oq WITH (NOLOCK) where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq),
[PLStatus] = p.Status,
p.CTNQty,
p.CBM,
[PLClogCTNQty] = (select sum(CTNQty) from PackingList_Detail pd WITH (NOLOCK) where pd.ID = p.ID and pd.ReceiveDate is not null),
p.InspDate,
p.InspStatus,
p.PulloutDate
from PackingList p WITH (NOLOCK) 
left join GMTBooking g WITH (NOLOCK) on p.INVNo = g.ID
left join ForwarderWarehouse_Detail fd WITH (NOLOCK) on g.ForwarderWhse_DetailUKey = fd.UKey
left join ForwarderWarehouse fw WITH (NOLOCK) on fd.id =fw.id
left join LocalSupp ls WITH (NOLOCK) on g.Forwarder = ls.ID
where p.ShipPlanID = '{0}'
order by p.INVNo,p.ID", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DataTable excelData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out excelData);

            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail!!\r\n" + result.ToString());
                return false;
            }

            string sqlGetGMTBookingInfoForA2B = @"
alter table #tmp alter column INVNo varchar(25)

select  t.ShipPlanID,
        t.INVNo,
        g.BrandID,
        g.ShipModeID, 
        [Forwarder] = (g.Forwarder+'-'+ls.Abb), 
        g.CYCFS,
        g.SONo,
        g.CutOffDate,
        [WhseNo] = concat(fw.WhseCode,'-',fw.Address),
        [Status] = iif(g.Status='Confirmed','GB Confirmed',iif(g.SOCFMDate is null,'','S/O Confirmed')),
        g.TotalCTNQty,
        g.TotalCBM,
        t.ClogCTNQty,
        t.ID,
        t.OrderID,
        t.BuyerDelivery,
        t.PLStatus,
        t.CTNQty,
        t.CBM,
        t.PLClogCTNQty,
        t.InspDate,
        t.InspStatus,
        t.PulloutDate
from #tmp t
left join GMTBooking g WITH (NOLOCK) on t.INVNo = g.ID
left join ForwarderWarehouse_Detail fd WITH (NOLOCK) on g.ForwarderWhse_DetailUKey = fd.UKey
left join ForwarderWarehouse fw WITH (NOLOCK) on fd.id  = fw.id
left join LocalSupp ls WITH (NOLOCK) on g.Forwarder = ls.ID
";

            foreach (string plFromRgCode in listPLFromRgCode)
            {
                DataTable dtExcelDataA2B;

                result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, sqlCmd, out dtExcelDataA2B);

                if (!result)
                {
                    MyUtility.Msg.WarningBox("Query data fail!!\r\n" + result.ToString());
                    return false;
                }

                result = MyUtility.Tool.ProcessWithDatatable(dtExcelDataA2B, null, sqlGetGMTBookingInfoForA2B, out dtExcelDataA2B);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Query data fail!!\r\n" + result.ToString());
                    return false;
                }

                dtExcelDataA2B.MergeTo(ref excelData);
            }

            string strXltName = Env.Cfg.XltPathDir + "\\Shipping_P10.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            int intRowsStart = 2;
            int dataRowCount = excelData.Rows.Count;
            object[,] objArray = new object[1, 23];
            for (int i = 0; i < dataRowCount; i++)
            {
                DataRow dr = excelData.Rows[i];
                int rownum = intRowsStart + i;
                objArray[0, 0] = dr["ShipPlanID"];
                objArray[0, 1] = dr["INVNo"];
                objArray[0, 2] = dr["BrandID"];
                objArray[0, 3] = dr["ShipModeID"];
                objArray[0, 4] = dr["Forwarder"];
                objArray[0, 5] = dr["CYCFS"];
                objArray[0, 6] = dr["SONo"];
                objArray[0, 7] = MyUtility.Check.Empty(dr["CutOffDate"]) ? string.Empty : Convert.ToDateTime(dr["CutOffDate"]).ToString(string.Format("{0}", Env.Cfg.DateTimeStringFormat));
                objArray[0, 8] = dr["WhseNo"];
                objArray[0, 9] = dr["Status"];
                objArray[0, 10] = dr["TotalCTNQty"];
                objArray[0, 11] = dr["TotalCBM"];
                objArray[0, 12] = dr["ClogCTNQty"];
                objArray[0, 13] = dr["ID"];
                objArray[0, 14] = dr["OrderID"];
                objArray[0, 15] = dr["BuyerDelivery"];
                objArray[0, 16] = dr["PLStatus"];
                objArray[0, 17] = dr["CTNQty"];
                objArray[0, 18] = dr["CBM"];
                objArray[0, 19] = dr["PLClogCTNQty"];
                objArray[0, 20] = dr["InspDate"];
                objArray[0, 21] = dr["InspStatus"];
                objArray[0, 22] = dr["PulloutDate"];
                worksheet.Range[string.Format("A{0}:W{0}", rownum)].Value2 = objArray;
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_P10");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return base.ClickPrint();
        }

        // 組Update PackingList的SQL
        private string UpdatePLCmd(DataRow pldatarow, string iNVno)
        {
            string updateCmd = string.Empty;

            updateCmd += string.Format(
            "update PackingList set ShipPlanID = '{0}', InspDate = {1}, InspStatus = '{2}', PulloutDate = {3}, ShippingReasonIDForTypeCO = '{6}' where ID = '{4}' AND INVno='{5}';",
            MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
            MyUtility.Check.Empty(pldatarow["InspDate"]) ? "null" : "'" + Convert.ToDateTime(pldatarow["InspDate"]).ToString("yyyy/MM/dd") + "'",
            MyUtility.Convert.GetString(pldatarow["InspStatus"]),
            MyUtility.Check.Empty(pldatarow["PulloutDate"]) ? "null" : "'" + Convert.ToDateTime(pldatarow["PulloutDate"]).ToString("yyyy/MM/dd") + "'",
            MyUtility.Convert.GetString(pldatarow["ID"]),
            iNVno,
            MyUtility.Convert.GetString(pldatarow["ShippingReasonIDForTypeCO"]));

            return updateCmd;
        }

        // 組(Delete)Update PackingList的SQL
        private string DeletePLCmd(string columnName, string iD)
        {
            return string.Format("update PackingList set ShipPlanID = '', PulloutDate = null, InspDate = null, InspStatus = '' where {0} = '{1}';", columnName, iD);
        }

        // 檢查Pullout report是否已經Confirm
        private bool CheckPullout(DateTime pulloutDate, string mdivisionid)
        {
            return MyUtility.Check.Seek(string.Format("select ID from Pullout WITH (NOLOCK) where PulloutDate = '{0}' and MDivisionID = '{1}' and Status <> 'New'", Convert.ToDateTime(pulloutDate).ToString("yyyy/MM/dd"), mdivisionid));
        }

        // Process Pullout Date Message
        private void PulloutMsg(DataRow dr, DateTime dt)
        {
            MyUtility.Msg.WarningBox("Pullout date:" + Convert.ToDateTime(dt).ToString("yyyy/MM/dd") + " already exist pullout report and have been confirmed, can't modify!");
            dr["PulloutDate"] = dr["PulloutDate"];
        }

        // Import Data
        private void BtnImportData_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["OrderCompanyID"]))
            {
                MyUtility.Msg.WarningBox("[Order Company] cannot be empty.");
                return;
            }

            P10_ImportData callNextForm = new P10_ImportData(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, (DataTable)this.listControlBindingSource1.DataSource);
            callNextForm.ShowDialog(this);
        }

        // Update Pullout Date
        private void BtnUpdatePulloutDate_Click(object sender, EventArgs e)
        {
            P10_UpdatePulloutDate callNextForm = new P10_UpdatePulloutDate(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
            this.RenewData();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridDelete()
        {
            // 檢查此筆記錄的Pullout Data是否還有值，若是則出訊息告知且無法刪除
            if (this.DetailDatas.Count > 0)
            {
                if (MyUtility.Msg.QuestionBox("Are you sure to delete GB# <GB#>？") == DialogResult.No)
                {
                    return;
                }

                this.gridDetailPackingList.ValidateControl();

                foreach (DataRow pldr in this.plData.Select(string.Format("InvNo = '{0}'", MyUtility.Convert.GetString(this.CurrentDetailData["ID"]))))
                {
                    if (!MyUtility.Check.Empty(pldr["PulloutDate"]))
                    {
                        MyUtility.Msg.WarningBox(string.Format("Pullout date of Packing No.:{0} is not empty, can't delete!", MyUtility.Convert.GetString(pldr["ID"])));
                        return;
                    }

                    this.plData.Rows.Remove(pldr);
                }

                // add into DeleteGBHistory
                DataRow dr = this.dtDeleteGBHistory.NewRow();
                dr["Type"] = "N";
                dr["ID"] = this.CurrentMaintain["id"];
                dr["GMTBookingID"] = this.CurrentDetailData["ID"];
                dr["NewShipModeID"] = string.Empty;
                dr["NewDestination"] = string.Empty;
                dr["Remark"] = string.Empty;
                dr["AddName"] = Env.User.UserID;
                dr["AddDate"] = DateTime.Now;
                this.dtDeleteGBHistory.Rows.Add(dr);

                // show btnDeleteGBHistory
                this.btnDeleteGBHistory.Enabled = true;
                this.btnDeleteGBHistory.ForeColor = Color.Red;

                if (this.DetailDatas.Count - 1 <= 0)
                {
                    string filter = "InvNo = ''";
                    this.plData.DefaultView.RowFilter = filter;
                }

                var frm = new P10_DeleteGarmentBookingHistory(this.dtDeleteGBHistory, true);
                frm.ShowDialog(this);
            }

            base.OnDetailGridDelete();
        }

        /// <inheritdoc/>
        protected override void ClickCheck()
        {
            base.ClickCheck();

            // 表身GMTBooking.ShipModeID 不存在Order_QtyShip 就return
            if (!this.ChkShipMode())
            {
                return;
            }

            string updateCmd = string.Format("update ShipPlan set Status = 'Checked', CFMDate = '{0}', EditName = '{1}', EditDate = GETDATE() where ID = '{2}'", DateTime.Today.ToString("yyyy/MM/dd"), Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Check fail !\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickUncheck()
        {
            base.ClickUncheck();
            string updateCmd = string.Format("update ShipPlan set Status = 'New', CFMDate = null, EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Uncheck fail !\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            // 如有問題請查詢：ISP20230067
            // #region  表身任一筆Orders.ID的Orders.GMTComplete 不可為 'S'
            // bool gMTCompleteCheck = this.GMTCompleteCheck();
            // if (!gMTCompleteCheck)
            // {
            //    return;
            // }
            // #endregion
            this.CheckIDD();
            this.CheckPulloutputIDD();

            string sql = $"select * from GMTBooking g left join GMTBooking_CTNR gc on gc.ID = g.ID where g.ShipPlanID = '{this.CurrentMaintain["ID"].ToString()}' and gc.ID is null";

            DataTable dtChk;
            DualResult resultChk = DBProxy.Current.Select(null, sql, out dtChk);
            if (!resultChk)
            {
                MyUtility.Msg.ErrorBox("Check GMTBooking error:\r\n" + resultChk.ToString());
                return;
            }

            if (dtChk.Rows.Count > 0)
            {
                string strChk = dtChk.AsEnumerable().Where(r => r["CYCFS"].ToString() == "CY-CY").Select(s => s["ID"].ToString()).ToList().JoinToString("、");

                if (!string.IsNullOrEmpty(strChk))
                {
                    MyUtility.Msg.ErrorBox($"GB#:{strChk}\r\n\r\nLoading types is CY-CY,must be exist in 'Container/Truck'!");
                    return;
                }
            }

            List<string> listPLFromRgCode = PackingA2BWebAPI.GetPLFromRgCodeByShipPlanID(this.CurrentMaintain["ID"].ToString());

            // Pullout Date有空值就不可以Confirm
            string sqlCmd = string.Format("select ID,InvNo from PackingList WITH (NOLOCK) where ShipPlanID = '{0}' and PulloutDate is null", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Check Pullout Date error:\r\n" + result.ToString());
                return;
            }

            foreach (string plFromRgCode in listPLFromRgCode)
            {
                DataTable dtCheckPulloutA2B;
                result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, sqlCmd, out dtCheckPulloutA2B);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Check Pullout Date error:\r\n" + result.ToString());
                    return;
                }

                dtCheckPulloutA2B.MergeTo(ref dt);
            }

            StringBuilder msg = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    msg.Append(string.Format("GB#: {0}, Packing No:{1}\n\r", MyUtility.Convert.GetString(dr["InvNo"]), MyUtility.Convert.GetString(dr["ID"])));
                }

                MyUtility.Msg.WarningBox("Below data's pullout date is empty, can't confirm!!\r\n" + msg.ToString());
                return;
            }

            // Inspection date不為空但是Inspection status為空就不可以Confirm
            sqlCmd = string.Format("select ID,InvNo from PackingList WITH (NOLOCK) where ShipPlanID = '{0}' and InspDate is not null and InspStatus = ''", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Check Inspection error:\r\n" + result.ToString());
                return;
            }

            foreach (string plFromRgCode in listPLFromRgCode)
            {
                DataTable dtCheckInspectionA2B;
                result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, sqlCmd, out dtCheckInspectionA2B);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Check Inspection error:\r\n" + result.ToString());
                    return;
                }

                dtCheckInspectionA2B.MergeTo(ref dt);
            }

            StringBuilder msg1 = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    msg1.Append(string.Format("GB#: {0}, Packing No:{1}\n\r", MyUtility.Convert.GetString(dr["InvNo"]), MyUtility.Convert.GetString(dr["ID"])));
                }

                MyUtility.Msg.WarningBox("Below data's est. inspection date not empty but inspection status is empty, can't confirm!!\r\n" + msg1.ToString());
                return;
            }

            #region 檢查PackingList_Detail.ReceiveDate欄位不可為空白
            StringBuilder msgReceDate = new StringBuilder();
            DataTable dtRec;
            string sqlCheckReceiveDate = string.Format(
                    @"
select distinct p1.id 
from PackingList p1
inner join PackingList_Detail p2 on p1.ID=p2.ID
where ShipPlanID='{0}'
and p1.Type='B'
and p2.CTNQty > 0
and p2.ReceiveDate is null ", this.CurrentMaintain["id"]);

            string sqlCheckPLCtnRecvFMRgCodeDate = string.Format(
                    @"
select distinct p.id 
from PackingList p
where ShipPlanID='{0}'
and p.Type='B'
and p.CTNQty > 0
and p.PLCtnRecvFMRgCodeDate is null ", this.CurrentMaintain["id"]);

            result = DBProxy.Current.Select(null, sqlCheckReceiveDate, out dtRec);

            if (!result)
            {
                MyUtility.Msg.ErrorBox("Check ReceiveDate error:\r\n" + result.ToString());
                return;
            }

            foreach (string plFromRgCode in listPLFromRgCode)
            {
                string sqlCheckA2BSisterFty = "select CartonTransferToSisterFty from system with (nolock)";
                DataRow drResult;
                PackingA2BResult seekDataResult = PackingA2BWebAPI.SeekBySql(plFromRgCode, sqlCheckA2BSisterFty, out drResult);

                if (!seekDataResult)
                {
                    MyUtility.Msg.ErrorBox("Check ReceiveDate error:\r\n" + seekDataResult.ToString());
                    return;
                }

                if (MyUtility.Convert.GetBool(drResult["CartonTransferToSisterFty"]))
                {
                    DataTable dtCheckPLCtnRecvA2B;
                    result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, sqlCheckPLCtnRecvFMRgCodeDate, out dtCheckPLCtnRecvA2B);
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox("Check PLCtnRecvA2B error:\r\n" + result.ToString());
                        return;
                    }

                    if (dtCheckPLCtnRecvA2B.Rows.Count > 0)
                    {
                        string unCfmPackIDs = dtCheckPLCtnRecvA2B.AsEnumerable().Select(s => s["id"].ToString()).JoinToString(Environment.NewLine);
                        MyUtility.Msg.WarningBox($@"The below PL not yet received from sister factory!! Cannot confirm!
{unCfmPackIDs}
");
                        return;
                    }
                }
                else
                {
                    DataTable dtCheckReceiveDateA2B;
                    result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, sqlCheckReceiveDate, out dtCheckReceiveDateA2B);
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox("Check ReceiveDate error:\r\n" + result.ToString());
                        return;
                    }

                    dtCheckReceiveDateA2B.MergeTo(ref dtRec);
                }
            }

            if (!MyUtility.Check.Empty(dtRec) || dtRec.Rows.Count > 0)
            {
                foreach (DataRow dr in dtRec.Rows)
                {
                    msgReceDate.Append(string.Format("Packing No:{0}\n\r", MyUtility.Convert.GetString(dr["ID"])));
                }
            }

            if (msgReceDate.Length > 0)
            {
                MyUtility.Msg.WarningBox("The CTNs were not received by CLog yet!! Cannot confirm!!\r\n" + msgReceDate.ToString());
                return;
            }

            #endregion

            #region 檢查是否還有箱子在CFA
            DataTable dtCfa;
            StringBuilder warningmsg = new StringBuilder();
            string strSqlcmd =
                   $@"
select distinct p1.INVNo,p2.OrderID,p2.ID
from PackingList p1
inner join PackingList_Detail p2 on p1.ID=p2.ID
where ShipPlanID='{this.CurrentMaintain["id"]}'
and p2.CFAReceiveDate is not null
and p2.CFAReturnClogDate is null
and p2.CTNQty > 0";

            result = DBProxy.Current.Select(null, strSqlcmd, out dtCfa);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            foreach (string plFromRgCode in listPLFromRgCode)
            {
                DataTable dtCheckCfaA2B;
                result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, strSqlcmd, out dtCheckCfaA2B);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                dtCheckCfaA2B.MergeTo(ref dtCfa);
            }

            if (dtCfa.Rows.Count > 0)
            {
                foreach (DataRow dr in dtCfa.Rows)
                {
                    warningmsg.Append($@"GB#: {dr["INVNo"]}, SP: {dr["OrderID"]}
, Packing#: {dr["ID"]}" + Environment.NewLine);
                }

                MyUtility.Msg.WarningBox("The CTNs are in CFA now, Cannot confirm!" + Environment.NewLine + warningmsg.ToString());
                return;
            }

            #endregion

            #region 檢查是否還有箱子在CLog
            DataTable dtCLog;
            StringBuilder warningmsgCLog = new StringBuilder();
            string strSqlcmdCLog =
                   $@"
select p2.ID,p2.CTNStartNo
from GMTBooking g
inner join PackingList p1 on p1.INVNo = g.id
inner join PackingList_Detail p2 on p1.ID=p2.ID
where g.ShipPlanID='{this.CurrentMaintain["id"]}'
and (TransferCFADate is not null or ReceiveDate is null)
and p2.CTNQty > 0
and p1.Type <> 'S'
";
            result = DBProxy.Current.Select(null, strSqlcmdCLog, out dtCLog);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            foreach (string plFromRgCode in listPLFromRgCode)
            {
                DataTable dtCheckClogA2B;
                result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, strSqlcmdCLog, out dtCheckClogA2B);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                dtCheckClogA2B.MergeTo(ref dtCLog);
            }

            if (dtCLog.Rows.Count > 0)
            {
                foreach (DataRow dr in dtCLog.Rows)
                {
                    warningmsgCLog.Append($@"<PackingList#:{dr["ID"]}, CTN#:{dr["CTNStartNo"]}>" + Environment.NewLine);
                }

                MyUtility.Msg.WarningBox("Below records are not in clog, cannot confirm!!\r\n" + warningmsgCLog.ToString());
                return;
            }

            #endregion

            // 表身GMTBooking.ShipModeID 不存在Order_QtyShip 就return
            if (!this.ChkShipMode())
            {
                return;
            }

            // Garment Booking還沒Confirm就不可以做Confirm
            StringBuilder msg2 = new StringBuilder();
            sqlCmd = string.Format("select ID from GMTBooking WITH (NOLOCK) where ShipPlanID = '{0}' and Status = 'New'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Check GMTBooking error:\r\n" + result.ToString());
                return;
            }

            msg2 = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    msg2.Append(string.Format("GB#:{0}", MyUtility.Convert.GetString(dr["ID"])));
                }

                MyUtility.Msg.WarningBox("Garment Booking's status not yet confirm, can't confirm!!\r\n" + msg2.ToString());
                return;
            }

            if (!Prgs.CheckExistsOrder_QtyShip_Detail(shipPlanID: MyUtility.Convert.GetString(this.CurrentMaintain["ID"])))
            {
                return;
            }

            foreach (string plFromRgCode in listPLFromRgCode)
            {
                if (!Prgs.CheckExistsOrder_QtyShip_Detail(shipPlanID: MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), plFromRgCode: plFromRgCode))
                {
                    return;
                }
            }

            #region 一張 HC 的出貨日期都必須與 PL 上的出貨日期一致  ISP20191473
            sqlCmd = $@"

select p.ID ,p.ExpressID ,p.PulloutDate 
from PackingList p
WHERE p.ShipPlanID='{this.CurrentMaintain["ID"]}' AND p.ExpressID <> ''
AND EXISTS (SELECT 1 FROM Express e WHERE  p.ExpressID=e.ID AND p.PulloutDate <> e.ShipDate)
";
            result = DBProxy.Current.Select(null, sqlCmd, out dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Check Express error:\r\n" + result.ToString());
                return;
            }

            foreach (string plFromRgCode in listPLFromRgCode)
            {
                DataTable dtCheckHCDateA2B;
                result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, sqlCmd, out dtCheckHCDateA2B);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                dtCheckHCDateA2B.MergeTo(ref dt);
            }

            msg2 = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    msg2.Append($"Packing List#:{dr["ID"]}" + Environment.NewLine);
                }

                MyUtility.Msg.WarningBox("HC Ship Date is different with Pull Out Date." + Environment.NewLine + msg2.ToString());
                return;
            }
            #endregion

            #region 檢查ShipMode，是否被設定「必須建立HC」ISP20191473
            sqlCmd = $@"

select 
	 p.INVNo
    ,p.ShipModeID
	,p.ID
	,p.ShipPlanID
	,e.ID
from PackingList p WITH (NOLOCK) 
LEFT JOIN GMTBooking g WITH (NOLOCK)  ON p.INVNo=g.ID
LEFT JOIN Express e ON e.ID=p.ExpressID
where p.ShipPlanID ='{this.CurrentMaintain["ID"]}'
AND p.ShipModeID IN ( SELECT ID FROM ShipMode WHERE NeedCreateIntExpress=1 )
AND e.ID IS NULL 
";
            result = DBProxy.Current.Select(null, sqlCmd, out dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Check Express error:\r\n" + result.ToString());
                return;
            }

            foreach (string plFromRgCode in listPLFromRgCode)
            {
                DataTable dtCheckHCDateA2B;
                result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, sqlCmd, out dtCheckHCDateA2B);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                dtCheckHCDateA2B.MergeTo(ref dt);
            }

            msg2 = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    msg2.Append($"Packing List#:{dr["ID"]}" + Environment.NewLine);
                }

                string strCmdFindShipmode = @"
select STUFF((
            SELECT Concat (',', ID )
            FROM ShipMode
            WHERE NeedCreateIntExpress = 1
            for xml path(''))
        , 1, 1, '')";

                string strListShipmode = MyUtility.GetValue.Lookup(strCmdFindShipmode);

                MyUtility.Msg.WarningBox($"Ship mode ({strListShipmode}) needs to Create International Express. Please create data at [Shippping P02]." + Environment.NewLine + msg2.ToString());
                return;
            }
            #endregion

            // 有Cancel Order 不能confirmed
            string errmsg = Prgs.ChkCancelOrder(this.CurrentMaintain["id"].ToString());
            if (!MyUtility.Check.Empty(errmsg))
            {
                MyUtility.Msg.WarningBox(errmsg);
                return;
            }

            string updateCmd = string.Format("update ShipPlan set Status = 'Confirmed',CFMDate = GETDATE(), EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Confirm fail !\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            string updateCmd = string.Format("update ShipPlan set Status = 'Checked',CFMDate =Null ,EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Unconfirm fail !\r\n" + result.ToString());
                return;
            }
        }

        private void Detailgridbs_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.SumData();
        }

        /// <summary>
        /// 檢查Ship Mode
        /// 表身GMTBooking.ShipModeID 不存在Order_QtyShip 就return
        /// </summary>
        /// <returns>bool</returns>
        private bool ChkShipMode()
        {
            var dtShipMode = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(s => s.RowState != DataRowState.Deleted);
            if (dtShipMode == null || dtShipMode.Count() == 0)
            {
                return false;
            }

            DualResult result;
            DataTable dtCheckResult;
            StringBuilder msg = new StringBuilder();
            foreach (DataRow dr in dtShipMode)
            {
                string strSql = $@"
select distinct oq.ID,oq.Seq,oq.ShipmodeID
from PackingList p with (nolock)
inner join PackingList_Detail pd with (nolock) on p.ID=pd.ID
inner join Order_QtyShip oq with (nolock) on oq.id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
inner join Orders o with (nolock) on o.ID = pd.OrderID
where p.INVNo='{dr["ID"]}' and o.Category <> 'S' and oq.ShipmodeID <> '{dr["ShipModeID"]}'";

                result = DBProxy.Current.Select(null, strSql, out dtCheckResult);

                if (!result)
                {
                    this.ShowErr(result);
                    return result;
                }

                List<string> listPLFromRgCode = PackingA2BWebAPI.GetPLFromRgCodeByInvNo(dr["ID"].ToString());

                if (listPLFromRgCode.Count > 0)
                {
                    DataTable dtCheckA2bResult;
                    foreach (string plFromRgCode in listPLFromRgCode)
                    {
                        result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, strSql, out dtCheckA2bResult);
                        if (!result)
                        {
                            this.ShowErr(result);
                            return result;
                        }

                        dtCheckA2bResult.MergeTo(ref dtCheckResult);
                    }
                }

                if (dtCheckResult.Rows.Count > 0)
                {
                    foreach (DataRow drError in dtCheckResult.Rows)
                    {
                        msg.Append($"Order ID:{drError["ID"]},   Seq{drError["Seq"]},   Shipping Mode:{drError["ShipmodeID"]} \r\n");
                    }
                }

                if (msg.Length > 0)
                {
                    MyUtility.Msg.WarningBox($@"Shipping mode is inconsistent!!
GB#:{dr["ID"]},   Shipping Mode:{dr["ShipModeID"]}
{msg.ToString()}
");
                    return false;
                }
            }

            return true;
        }

        private void BtnContainerTruck_Click(object sender, EventArgs e)
        {
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Please create ship plan first!!");
                return;
            }

            bool edit = MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToLower().EqualString("new") && this.Perm.Edit;
            P10_ContainerTruck callNextForm = new P10_ContainerTruck(edit, null, null, null, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), this.Reload);
            callNextForm.ShowDialog(this);
            this.OnDetailEntered();
            this.Refresh();
        }

        /// <summary>
        /// Reload
        /// </summary>
        public void Reload()
        {
            this.OnDetailEntered();
            this.Refresh();
        }

        private void CheckIDD()
        {
            if (this.plData.Rows.Count == 0)
            {
                return;
            }

            #region 檢查傳入的SP 維護的IDD是否都為同一天(沒維護度不判斷)
            List<Order_QtyShipKey> listOrder_QtyShipKey = new List<Order_QtyShipKey>();

            string wherePackIDs = this.plData.AsEnumerable().Select(s => $"'{s["ID"].ToString()}'").Distinct().JoinToString(",");
            string sqlGetOrderSeq = @"
select  distinct pd.OrderID, pd.OrderShipmodeSeq
from PackingList_Detail pd with (nolock)
where   pd.ID in ({0})
";
            DataTable dtOrderSeq;

            DualResult result = DBProxy.Current.Select(null, string.Format(sqlGetOrderSeq, wherePackIDs), out dtOrderSeq);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            foreach (DataRow dr in dtOrderSeq.Rows)
            {
                listOrder_QtyShipKey.Add(new Order_QtyShipKey
                {
                    SP = dr["OrderID"].ToString(),
                    Seq = dr["OrderShipmodeSeq"].ToString(),
                });
            }

            var needGetDataFromA2B = this.plData.AsEnumerable().Where(s => !MyUtility.Check.Empty(s["PLFromRgCode"]));

            if (needGetDataFromA2B.Any())
            {
                var listPLFromRgCode = needGetDataFromA2B.Select(s => s["PLFromRgCode"].ToString()).Distinct();

                foreach (string plFromRgCode in listPLFromRgCode)
                {
                    wherePackIDs = needGetDataFromA2B.Where(s => s["PLFromRgCode"].ToString() == plFromRgCode)
                        .Select(s => $"'{s["ID"].ToString()}'").Distinct().JoinToString(",");
                    result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, string.Format(sqlGetOrderSeq, wherePackIDs), out dtOrderSeq);

                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }

                    foreach (DataRow dr in dtOrderSeq.Rows)
                    {
                        listOrder_QtyShipKey.Add(new Order_QtyShipKey
                        {
                            SP = dr["OrderID"].ToString(),
                            Seq = dr["OrderShipmodeSeq"].ToString(),
                        });
                    }
                }
            }

            Prgs.CheckIDDSame(listOrder_QtyShipKey);
            #endregion
        }

        private void CheckPulloutputIDD()
        {
            if (this.plData.Rows.Count == 0)
            {
                return;
            }

            #region 檢查傳入的SP 維護的IDD與PulloutputDate是否都為同一天(沒維護不判斷)
            List<Order_QtyShipKey> listOrder_QtyShipKey = new List<Order_QtyShipKey>();

            string sqlGetOrderSeq = $@"
alter table #tmp alter column ID varchar(13)
select  distinct pd.OrderID, pd.OrderShipmodeSeq, t.PulloutDate
from PackingList_Detail pd with (nolock)
inner join #tmp t on t.ID = pd.ID
";
            DataTable dtOrderSeq;

            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.plData, "ID,PulloutDate", sqlGetOrderSeq, out dtOrderSeq);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            foreach (DataRow dr in dtOrderSeq.Rows)
            {
                listOrder_QtyShipKey.Add(new Order_QtyShipKey
                {
                    SP = dr["OrderID"].ToString(),
                    Seq = dr["OrderShipmodeSeq"].ToString(),
                    PulloutDate = MyUtility.Convert.GetDate(dr["PulloutDate"]),
                });
            }

            Prgs.CheckIDDSamePulloutDate(listOrder_QtyShipKey);
            #endregion

        }

        private void BtnDeleteGBHistory_Click(object sender, EventArgs e)
        {
            if (this.dtDeleteGBHistory == null || this.dtDeleteGBHistory.Rows.Count == 0)
            {
                return;
            }

            var frm = new P10_DeleteGarmentBookingHistory(this.dtDeleteGBHistory);
            frm.ShowDialog(this);
        }

        private bool GMTCompleteCheck()
        {
            #region 表身任一筆Orders.ID的Orders.GMTComplete 不可為 'S'
            DataTable packingList_List = (DataTable)this.listControlBindingSource1.DataSource;

            DataTable isGMTComplete = new DataTable();
            isGMTComplete.ColumnsStringAdd("SP#");
            isGMTComplete.ColumnsDateTimeAdd("Complete Date");
            foreach (DataRow dr in packingList_List.Rows)
            {
                // 拆解Order ID
                List<string> orders = MyUtility.Convert.GetString(dr["OrderID"]).Split(',').ToList();
                foreach (var order in orders)
                {
                    string cmd = $@"SELECT [SP#]=ID ,[Complete Date]=CMPLTDATE FROM Orders WITH(NOLOCK) WHERE GMTComplete='S' AND ID = '{order}'";
                    DataTable dt;
                    DBProxy.Current.Select(null, cmd, out dt);
                    bool find = dt.Rows.Count > 0;
                    if (find)
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            isGMTComplete.ImportRow(r);
                        }
                    }
                }
            }

            if (isGMTComplete.Rows.Count > 0)
            {
                var m = MyUtility.Msg.ShowMsgGrid(isGMTComplete, "GMT Complete Status is updated to S on following dates, assuming the shipment is still to be arranged, please contact TPE Finance Dept. to update GMT Complete Status", "GMT Complete Status check");
                m.Width = 800;
                m.grid1.Columns[0].Width = 150;
                m.grid1.Columns[1].Width = 150;
                m.TopMost = true;
                return false;
            }
            else
            {
                return true;
            }
            #endregion
        }

        private void ComboCompany1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.IsDetailInserting || this.DetailDatas.Count == 0 || this.previousCompanySelectIndex == -1 || this.previousCompanySelectIndex == this.comboCompany1.SelectedIndex)
            {
                this.previousCompanySelectIndex = this.comboCompany1.SelectedIndex;
                return;
            }

            DialogResult result = MyUtility.Msg.QuestionBox("[Order Company] has been changed and all PL data will be clear.");
            if (result == DialogResult.Yes)
            {
                this.DetailDatas.Delete();
                this.plData.Clear();
                this.previousCompanySelectIndex = this.comboCompany1.SelectedIndex;
            }
            else
            {
                this.comboCompany1.SelectedIndex = this.previousCompanySelectIndex;
            }
        }
    }
}
