using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Transactions;
using System.Linq;
using Sci.Production.PublicPrg;

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
        private DataSet allData = new DataSet();
        private IList<string> updateCmds = new List<string>();

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
            string masterID = (e.Master == null) ? "1=0" : string.Format("p.ShipPlanID ='{0}'", MyUtility.Convert.GetString(e.Master["ID"]));
            string sqlCmd = string.Format(
                @"
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
, Pullout.SendToTPE

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

from PackingList p WITH (NOLOCK) 
left join Pullout WITH (NOLOCK)  on Pullout.ID = p.PulloutID and Pullout.Status <> 'NEW'
where {0} 
order by p.ID", masterID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.plData);

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
, WhseNo = isnull(fd.WhseNo,'')
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
left join ForwarderWhse_Detail fd WITH (NOLOCK) on g.ForwarderWhse_DetailUKey = fd.UKey
where {0} 
order by g.ID", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
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

            if (tmp_dt.Select("CYCFS in ('CFS-CFS','CFS-CY')").Count() > 0)
            {
                this.numericBoxCFSCBM.Value = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(tmp_dt.Compute("Sum(TotalCBM)", "CYCFS in ('CFS-CFS','CFS-CY') and ShipModeID not in ('A/C','A/P','E/C','E/P','A/P-C','E/P-C','AIR')")), 2);
            }
            else
            {
                this.numericBoxCFSCBM.Value = 0;
            }

            if (tmp_dt.Select("ShipModeID in('A/C','A/P','E/C','E/P')").Count() > 0)
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
            this.gridDetail.IsEditingReadOnly = false;
        }

        /// <inheritdoc/>
        protected override void OnDetailUIConvertToView()
        {
            base.OnDetailUIConvertToView();
            this.gridDetail.IsEditingReadOnly = true;
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
                this.gridDetail.ValidateControl();
                DataRow dr = this.detailgrid.GetDataRow<DataRow>(this.detailgrid.GetSelectedRowIndex());
                if (dr != null)
                {
                    string filter = string.Format("InvNo = '{0}'", MyUtility.Convert.GetString(dr["ID"]));
                    this.plData.DefaultView.RowFilter = filter;
                }
            };

            this.gridDetail.DataSource = this.listControlBindingSource1;
            this.gridDetail.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("ID", header: "Packing No.", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("OrderShipmodeSeq", header: "Seq", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Delivery", iseditingreadonly: true)
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
                .Text("PulloutID", header: "Pullout ID#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("SendToTPE", header: "Send to SCI", iseditingreadonly: true)
                ;
            #region 欄位值檢查
            this.gridDetail.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.gridDetail.GetDataRow<DataRow>(e.RowIndex);
                    if (this.gridDetail.Columns[e.ColumnIndex].DataPropertyName == this.col_inspdate.DataPropertyName)
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
                    if (this.gridDetail.Columns[e.ColumnIndex].DataPropertyName == this.col_pulloutdate.DataPropertyName)
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
            base.ClickNewAfter();
            this.CurrentMaintain["CDate"] = DateTime.Today;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.listControlBindingSource1.DataSource = this.plData;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Confirmed")
            {
                MyUtility.Msg.WarningBox(string.Format("This record status is < {0} >, can't be edit!", MyUtility.Convert.GetString(this.CurrentMaintain["Status"])));
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) != "New")
            {
                this.btnImportData.Enabled = false;
            }
        }

        /// <inheritdoc/>
        protected override void ClickUndo()
        {
            base.ClickUndo();
            ((DataTable)this.listControlBindingSource1.DataSource).RejectChanges();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            // 表身GMTBooking.ShipModeID 不存在Order_QtyShip 就return
            if (!this.ChkShipMode())
            {
                return false;
            }

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

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult OnSaveDetail(IList<DataRow> details, ITableSchema detailtableschema)
        {
            this.updateCmds.Clear();
            this.gridDetail.EndEdit();
            this.listControlBindingSource1.EndEdit();

            DataTable packingListDt = (DataTable)this.listControlBindingSource1.DataSource;

            // 避免ISP20181052的狀況發生，因此從修改PackingList的語法要跟GMTBooking綁在一起，分開作
            // foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
            // {
            //    if (dr.RowState == DataRowState.Modified || dr.RowState == DataRowState.Added)
            //    {
            //        this.UpdatePLCmd(dr);
            //        continue;
            //    }
            // }

            // 未篩選過的packingList
            DataTable packingList_List = (DataTable)this.listControlBindingSource1.DataSource;

            foreach (DataRow dr in details)
            {
                if (dr.RowState == DataRowState.Modified || dr.RowState == DataRowState.Added)
                {
                    this.updateCmds.Add(string.Format("update GMTBooking set ShipPlanID = '{0}' where ID = '{1}';", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(dr["ID"])));

                    // 根據GMTBooking.ID篩選出對應的PackingList，這些是預先存在Form裡面的，不是DB裡面最新的資料
                    DataTable packingListDt_on_Form = null;
                    var rows = packingList_List.AsEnumerable().Where(o => o["INVNo"].ToString() == MyUtility.Convert.GetString(dr["ID"]));

                    if (rows.Any())
                    {
                        packingListDt_on_Form = rows.CopyToDataTable();
                    }

                    // DB可能已經被修改，因此回DB撈出GMTBooking底下最新的PackingList
                    DataTable packingListDt_new;
                    string sqlCmd = string.Format("SELECT ID,InspDate,InspStatus,PulloutDate FROM PackingList WHERE INVNo='{0}' ", MyUtility.Convert.GetString(dr["ID"]));
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, out packingListDt_new);
                    if (result)
                    {
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
                                this.UpdatePLCmd(pldatarow, MyUtility.Convert.GetString(dr["ID"]));
                            }

                            continue;
                        }
                    }
                    else
                    {
                        DualResult failResult = new DualResult(false, "Update fail!!\r\n" + result.ToString());
                        return failResult;
                    }

                    continue;
                }

                if (dr.RowState == DataRowState.Deleted)
                {
                    this.updateCmds.Add(string.Format("update GMTBooking set ShipPlanID = '' where ID = '{0}';", MyUtility.Convert.GetString(dr["ID", DataRowVersion.Original])));
                    this.updateCmds.Add(this.DeletePLCmd("InvNo", MyUtility.Convert.GetString(dr["ID", DataRowVersion.Original])));
                    continue;
                }
            }

            // DataTable dtPklst = plData;
            //            int count = plData.Rows.Count;

            // foreach (DataRow dr in dtPklst.Rows)
            //            {
            //                // 20161206 確認正確的值都有insert
            //                updateCmds.Add(string.Format("update PackingList set ShipPlanID = '{0}', InspDate='{1}' , InspStatus='{2}',PulloutDate='{3}' where id = '{4}';", MyUtility.Convert.GetString(CurrentMaintain["ID"]), dr["InspDate"], dr["InspStatus"],Convert.ToDateTime(dr["PulloutDate"]).ToString("yyyy/MM/dd"),
            // MyUtility.Convert.GetString(dr["ID"])));
            //            }

            // 執行更新
            if (this.updateCmds.Count != 0)
            {
                DualResult result;

                // 2018/11/09 Benson 加上Transaction
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        result = DBProxy.Current.Executes(null, this.updateCmds);
                        if (!result)
                        {
                            DualResult failResult = new DualResult(false, "Update fail!!\r\n" + result.ToString());
                            return failResult;
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
            this.updateCmds.Clear();
            this.updateCmds.Add(string.Format("update GMTBooking set ShipPlanID = '' where ShipPlanID = '{0}';", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            this.updateCmds.Add(this.DeletePLCmd("ShipPlanID", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            DualResult result = DBProxy.Current.Executes(null, this.updateCmds);

            return result;
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                return false;
            }

            string sqlCmd = string.Format(
                @"select p.ShipPlanID,p.INVNo,g.BrandID,g.ShipModeID, (g.Forwarder+'-'+ls.Abb) as Forwarder, g.CYCFS,
g.SONo,g.CutOffDate,concat(fd.WhseNo,'-',fd.Address) as WhseNo,
iif(g.Status='Confirmed','GB Confirmed',iif(g.SOCFMDate is null,'','S/O Confirmed')) as Status,
g.TotalCTNQty,g.TotalCBM,
(select isnull(sum(pd1.CTNQty),0) from PackingList p1 WITH (NOLOCK) ,PackingList_Detail pd1 WITH (NOLOCK) where p1.INVNo = g.ID and p1.ID = pd1.ID and pd1.ReceiveDate is not null) as ClogCTNQty,
p.ID,(select cast(a.OrderID as nvarchar) +',' from (select distinct OrderID from PackingList_Detail pd WITH (NOLOCK) where pd.ID = p.id) a for xml path('')) as OrderID,
(select oq.BuyerDelivery from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd WITH (NOLOCK) where pd.ID = p.ID) a, Order_QtyShip oq WITH (NOLOCK) where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq) as BuyerDelivery,
p.Status as PLStatus,p.CTNQty,p.CBM,(select sum(CTNQty) from PackingList_Detail pd WITH (NOLOCK) where pd.ID = p.ID and pd.ReceiveDate is not null) as PLClogCTNQty,
p.InspDate,p.InspStatus,p.PulloutDate
from PackingList p WITH (NOLOCK) 
left join GMTBooking g WITH (NOLOCK) on p.INVNo = g.ID
left join ForwarderWhse_Detail fd WITH (NOLOCK) on g.ForwarderWhse_DetailUKey = fd.UKey
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
        private void UpdatePLCmd(DataRow pldatarow, string iNVno)
        {
            this.updateCmds.Add(string.Format(
                "update PackingList set ShipPlanID = '{0}', InspDate = {1}, InspStatus = '{2}', PulloutDate = {3} where ID = '{4}' AND INVno='{5}';",
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                MyUtility.Check.Empty(pldatarow["InspDate"]) ? "null" : "'" + Convert.ToDateTime(pldatarow["InspDate"]).ToString("d") + "'",
                MyUtility.Convert.GetString(pldatarow["InspStatus"]),
                MyUtility.Check.Empty(pldatarow["PulloutDate"]) ? "null" : "'" + Convert.ToDateTime(pldatarow["PulloutDate"]).ToString("d") + "'",
                MyUtility.Convert.GetString(pldatarow["ID"]),
                iNVno));
        }

        // 組(Delete)Update PackingList的SQL
        private string DeletePLCmd(string columnName, string iD)
        {
            return string.Format("update PackingList set ShipPlanID = '', PulloutDate = null, InspDate = null, InspStatus = '' where {0} = '{1}';", columnName, iD);
        }

        // 檢查Pullout report是否已經Confirm
        private bool CheckPullout(DateTime pulloutDate, string mdivisionid)
        {
            return MyUtility.Check.Seek(string.Format("select ID from Pullout WITH (NOLOCK) where PulloutDate = '{0}' and MDivisionID = '{1}' and Status <> 'New'", Convert.ToDateTime(pulloutDate).ToString("d"), mdivisionid));
        }

        // Process Pullout Date Message
        private void PulloutMsg(DataRow dr, DateTime dt)
        {
            MyUtility.Msg.WarningBox("Pullout date:" + Convert.ToDateTime(dt).ToString("d") + " already exist pullout report and have been confirmed, can't modify!");
            dr["PulloutDate"] = dr["PulloutDate"];
        }

        // Import Data
        private void BtnImportData_Click(object sender, EventArgs e)
        {
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
                this.gridDetail.ValidateControl();
                foreach (DataRow pldr in this.plData.Select(string.Format("InvNo = '{0}'", MyUtility.Convert.GetString(this.CurrentDetailData["ID"]))))
                {
                    if (!MyUtility.Check.Empty(pldr["PulloutDate"]))
                    {
                        MyUtility.Msg.WarningBox(string.Format("Pullout date of Packing No.:{0} is not empty, can't delete!", MyUtility.Convert.GetString(pldr["ID"])));
                        return;
                    }
                }

                if (this.DetailDatas.Count - 1 <= 0)
                {
                    string filter = "InvNo = ''";
                    this.plData.DefaultView.RowFilter = filter;
                }
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

            string updateCmd = string.Format("update ShipPlan set Status = 'Checked', CFMDate = '{0}', EditName = '{1}', EditDate = GETDATE() where ID = '{2}'", DateTime.Today.ToString("d"), Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

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

            // Pullout Date有空值就不可以Confirm
            string sqlCmd = string.Format("select ID,InvNo from PackingList WITH (NOLOCK) where ShipPlanID = '{0}' and PulloutDate is null", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Check Pullout Date error:\r\n" + result.ToString());
                return;
            }
            else
            {
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
            }

            // Inspection date不為空但是Inspection status為空就不可以Confirm
            sqlCmd = string.Format("select ID,InvNo from PackingList WITH (NOLOCK) where ShipPlanID = '{0}' and InspDate is not null and InspStatus = ''", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Check Inspection error:\r\n" + result.ToString());
                return;
            }
            else
            {
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
            }

            #region 檢查PackingList_Detail.ReceiveDate欄位不可為空白
            StringBuilder msgReceDate = new StringBuilder();
            DataTable dtRec;
            DBProxy.Current.Select(
                null,
                string.Format(
                    @"
select distinct p1.id 
from PackingList p1
inner join PackingList_Detail p2 on p1.ID=p2.ID
where ShipPlanID='{0}'
and p1.Type='B'
and p2.CTNQty > 0
and p2.ReceiveDate is null ", this.CurrentMaintain["id"]), out dtRec);
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
            if (result = DBProxy.Current.Select(null, strSqlcmd, out dtCfa))
            {
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
            if (result = DBProxy.Current.Select(null, strSqlcmdCLog, out dtCLog))
            {
                if (dtCLog.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCLog.Rows)
                    {
                        warningmsgCLog.Append($@"<PackingList#:{dr["ID"]}, CTN#:{dr["CTNStartNo"]}>" + Environment.NewLine);
                    }

                    MyUtility.Msg.WarningBox("Below records are not in clog, cannot confirm!!\r\n" + warningmsgCLog.ToString());
                    return;
                }
            }

            #endregion

            // 表身GMTBooking.ShipModeID 不存在Order_QtyShip 就return
            if (!this.ChkShipMode())
            {
                return;
            }

            // Garment Booking還沒Confirm就不可以做Confirm
            sqlCmd = string.Format("select ID from GMTBooking WITH (NOLOCK) where ShipPlanID = '{0}' and Status = 'New'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Check GMTBooking error:\r\n" + result.ToString());
                return;
            }
            else
            {
                StringBuilder msg2 = new StringBuilder();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        msg2.Append(string.Format("GB#:{0}", MyUtility.Convert.GetString(dr["ID"])));
                    }

                    MyUtility.Msg.WarningBox("Garment Booking's status not yet confirm, can't confirm!!\r\n" + msg2.ToString());
                    return;
                }
            }

            if (!Prgs.CheckExistsOrder_QtyShip_Detail(ShipPlanID: MyUtility.Convert.GetString(this.CurrentMaintain["ID"])))
            {
                return;
            }

            #region 一張 HC 的出貨日期都必須與 PL 上的出貨日期一致  ISP20191473
            sqlCmd = $@"

select DISTINCT p.ID ,p.ExpressID ,p.PulloutDate 
from PackingList p
inner join PackingList_Detail p2 on p.ID=p2.ID
WHERE p.ShipPlanID='{this.CurrentMaintain["ID"]}' AND p.ExpressID <> ''
AND EXISTS (SELECT 1 FROM Express e WHERE  p.ExpressID=e.ID AND p.PulloutDate <> e.ShipDate)
";
            result = DBProxy.Current.Select(null, sqlCmd, out dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Check Express error:\r\n" + result.ToString());
                return;
            }
            else
            {
                StringBuilder msg2 = new StringBuilder();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        msg2.Append($"Packing List#:{dr["ID"]}" + Environment.NewLine);
                    }

                    MyUtility.Msg.WarningBox("HC Ship Date is different with Pull Out Date." + Environment.NewLine + msg2.ToString());
                    return;
                }
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
            else
            {
                StringBuilder msg2 = new StringBuilder();
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
            P10_ContainerTruck callNextForm = new P10_ContainerTruck(edit, null, null, null, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), this.reload);
            callNextForm.ShowDialog(this);
            this.OnDetailEntered();
            this.Refresh();
        }

        public void reload()
        {
            this.OnDetailEntered();
            this.Refresh();
        }
    }
}
