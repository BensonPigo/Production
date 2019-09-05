﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci;
using System.Linq;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P05_ImportFromPackingList
    /// </summary>
    public partial class P05_ImportFromPackingList : Sci.Win.Tems.QueryForm
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataRow masterData;
        private DataTable gridData;
        private DataTable detailData;
        private DataTable selectData;

        /// <summary>
        /// P05_ImportFromPackingList
        /// </summary>
        /// <param name="masterData">masterData</param>
        /// <param name="detailData">detailData</param>
        public P05_ImportFromPackingList(DataRow masterData, DataTable detailData)
        {
            this.InitializeComponent();

            // this.txtmultifactoryFactory.Text = Sci.Env.User.FactoryList;
            this.txtmultifactoryFactory.Text = MyUtility.GetValue.Lookup("select stuff((select distinct concat(',',ID)  from Factory WITH (NOLOCK) where Junk = 0 and IsProduceFty = 1 for xml path('')),1,1,'')");
            this.masterData = masterData;
            this.detailData = detailData;
            this.txtmultifactoryFactory.checkProduceFty = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridImport.IsEditingReadOnly = false;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("ID", header: "Packing#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("CustCDID", header: "CustCD", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Date("SDPDate", header: "SDP Date", iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Delivery", iseditingreadonly: true)
                .Numeric("ShipQty", header: "Q'ty", iseditingreadonly: true)
                .Numeric("CTNQty", header: "ttl CTNs", iseditingreadonly: true)
                .Numeric("NW", header: "N.W.", decimal_places: 3, iseditingreadonly: true)
                .Numeric("NNW", header: "N.N.W.", decimal_places: 3, iseditingreadonly: true);
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            #region 組SQL語法
            sqlCmd.Append(string.Format(
                @"
with IniBulkPack as (
    select  1 as Selected
            , p.id
            , pd.OrderID
            , p.CustCDID
            , oq.SDPDate
            , oq.BuyerDelivery
            , p.ShipQty
            , p.CTNQty
            , p.NW
            , p.NNW
            , 'Y' as GMTBookingLock
            , p.MDivisionID
            , p.CargoReadyDate
            , p.PulloutDate
            , p.GW
            , p.CBM
            , p.Status
            , p.InspDate
            , ClogCTNQty = (select sum(CTNQty) 
                            from PackingList_Detail pd1 WITH (NOLOCK) 
                            where   pd1.ID = p.ID 
                                    and pd1.ReceiveDate is not null)
    from PackingList p WITH (NOLOCK) 
    left Join PackingList_Detail pd WITH (NOLOCK) on p.ID = pd.ID
    Left Join Order_QtyShip oq WITH (NOLOCK) on  pd.OrderID = oq.Id 
                                                 and pd.OrderShipmodeSeq = oq.Seq
    left join Orders o WITH (NOLOCK) on pd.OrderID = o.ID
    where   p.Type = 'B'
            and '{0}' like '%'+rtrim(o.FactoryID)+'%'
            and p.INVNo = ''
            and p.ShipModeID = '{1}'
            and p.BrandID = '{2}'
            and p.Dest = '{3}'
            and p.CustCDID = '{4}'
), IniSamplePack as (
    select  iif(p.CustCDID = '{4}',1,0) as Selected
            , p.id
            , p.OrderID
            , p.CustCDID
            , oq.SDPDate
            , oq.BuyerDelivery
            , p.ShipQty
            , p.CTNQty
            , p.NW
            , p.NNW
            , 'Y' as GMTBookingLock
            , p.MDivisionID
            , p.CargoReadyDate
            , p.PulloutDate
            , p.GW
            , p.CBM
            , p.Status
            , p.InspDate
            , 0 as ClogCTNQty
    from PackingList p WITH (NOLOCK) 
    left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = p.ID
    left join Order_QtyShip oq WITH (NOLOCK) on  oq.Id = pd.OrderID 
                                                 and oq.Seq = pd.OrderShipmodeSeq
    left join Orders o WITH (NOLOCK) on pd.OrderID = o.ID
    where   p.INVNo = '' 
            and p.Type = 'S' 
            and '{0}' like '%'+rtrim(o.FactoryID)+'%' 
            and p.Dest = '{3}' 
            and p.ShipModeID = '{1}'
), AllPackData as (
    select * 
    from IniBulkPack
    
    union all
    select * 
    from IniSamplePack
), InvalidData as (
    select  distinct ID
    from AllPackData
    where   1=1",
                this.txtmultifactoryFactory.Text,
                MyUtility.Convert.GetString(this.masterData["ShipModeID"]),
                MyUtility.Convert.GetString(this.masterData["BrandID"]),
                MyUtility.Convert.GetString(this.masterData["Dest"]),
                MyUtility.Convert.GetString(this.masterData["CustCDID"])));

            if (!MyUtility.Check.Empty(this.dateSDPDate.Value1))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and SDPDate >= '{0}' ", Convert.ToDateTime(this.dateSDPDate.Value1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateSDPDate.Value2))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and SDPDate <= '{0}' ", Convert.ToDateTime(this.dateSDPDate.Value2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateDelivery.Value1))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and BuyerDelivery >= '{0}' ", Convert.ToDateTime(this.dateDelivery.Value1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateDelivery.Value2))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and BuyerDelivery <= '{0}' ", Convert.ToDateTime(this.dateDelivery.Value2).ToString("d")));
            }


            if (!MyUtility.Check.Empty(this.txtSpStart.Text))
            {
                sqlCmd.Append($"AND OrderID >='{this.txtSpStart.Text}'");
            }

            if (!MyUtility.Check.Empty(this.txtSPEnd.Text))
            {
                sqlCmd.Append($"AND OrderID <='{this.txtSPEnd.Text}'");
            }

            sqlCmd.Append(@"
), PackData as (
    select  Selected
            , ID
            , CustCDID
            , min(SDPDate) as SDPDate
            , min(BuyerDelivery) as BuyerDelivery
            , ShipQty
            , CTNQty
            , NW
            , NNW
            , GMTBookingLock
            , MDivisionID
            , CargoReadyDate
            , PulloutDate
            , GW
            , CBM
            , Status
            , InspDate
            , ClogCTNQty
    from AllPackData 
    where   id in (
                select ID 
                from InvalidData 
                where ID is not null)
    group by Selected,ID,CustCDID,ShipQty,CTNQty,NW,NNW,GMTBookingLock,MDivisionID,CargoReadyDate,PulloutDate,GW,CBM,Status,InspDate,ClogCTNQty
)
select  pd.Selected
        , pd.ID
        , OrderID = stuff ((select ',' + cast(a.OrderID as nvarchar) 
                            from (
                                select  distinct OrderID 
                                from PackingList_Detail pl WITH (NOLOCK) 
                                where pl.ID = pd.ID
                            ) a for xml path('')
                           ), 1, 1, '')
        , pd.CustCDID
        , pd.SDPDate
        , pd.BuyerDelivery
        , pd.ShipQty
        , pd.CTNQty
        , pd.NW
        , pd.NNW
        , pd.GMTBookingLock
        , pd.MDivisionID
        , pd.CargoReadyDate
        , pd.PulloutDate
        , pd.GW
        , pd.CBM
        , pd.Status
        , pd.InspDate
        , pd.ClogCTNQty
from PackData pd");
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.gridData);
            if (result)
            {
                if (this.gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            else
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return;
            }

            this.listControlBindingSource1.DataSource = this.gridData;
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            this.gridData = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(this.gridData))
            {
                return;
            }

            if (this.gridData.Rows.Count > 0)
            {
                StringBuilder allPackID = new StringBuilder();

                DataRow[] dr = this.gridData.Select("Selected = 1");
                if (dr.Length > 0)
                {
                    // 檢查相同SP#不同PackingList#不可以匯入到同一張GarmentBooking中
                    foreach (DataRow currentRow in dr)
                    {
                        allPackID.Append("'" + MyUtility.Convert.GetString(currentRow["ID"]) + "',");
                    }

                    foreach (DataRow currentRow in this.detailData.Rows)
                    {
                        if (currentRow.RowState!= DataRowState.Deleted)
                        {
                            allPackID.Append("'" + MyUtility.Convert.GetString(currentRow["ID"]) + "',");
                        }
                    }

                    if (allPackID.Length > 0)
                    {
                        string sqlCmd = string.Format(
                            @"
select  distinct b.OrderID
        , b.ID
from (
    select  OrderID
            , COUNT(OrderID) as CNT
    from (
        select  distinct ID
                , OrderID 
        from PackingList_Detail WITH (NOLOCK) 
        where ID in ({0})
    ) a
    group by OrderID
    Having COUNT(OrderID) > 1
) a, PackingList_Detail b
where b.ID in ({0})
and a.OrderID = b.OrderID", allPackID.ToString().Substring(0, allPackID.Length - 1));
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.selectData);
                        if (result)
                        {
                            if (this.selectData.Rows.Count > 0)
                            {
                                StringBuilder allID = new StringBuilder();
                                foreach (DataRow currentRow in this.selectData.Rows)
                                {
                                    allID.Append("SP#:" + MyUtility.Convert.GetString(currentRow["OrderID"]) + "   Packing#:" + MyUtility.Convert.GetString(currentRow["ID"]) + "\r\n");
                                }

                                MyUtility.Msg.WarningBox("Can't import SP# with different Packing# in the same Inv# !!\r\n" + allID.ToString());
                                return;
                            }
                        }
                        else
                        {
                            MyUtility.Msg.ErrorBox(result.ToString());
                            return;
                        }
                    }

                    foreach (DataRow currentRow in dr)
                    {
                        DataRow[] findrow = this.detailData.Select(string.Format("ID = '{0}'", MyUtility.Convert.GetString(currentRow["ID"])));
                        if (findrow.Length == 0)
                        {
                            currentRow.AcceptChanges();
                            currentRow.SetAdded();
                            this.detailData.ImportRow(currentRow);
                        }
                    }

                    DataTable tmp = dr.CopyToDataTable();
                    string sqlcmd = $@"
SELECT AirPP.Forwarder,t.id
From #tmp t
inner join PackingList_Detail pd with(nolock) on pd.id = t.id
inner join PackingList p with(nolock) on p.id = pd.id and p.ShipModeID in('A/P','S-A/P','E/P')
inner join AirPP with(nolock) on AirPP.OrderID = pd.OrderID and AirPP.OrderShipmodeSeq = pd.OrderShipmodeSeq
";
                    DataTable dt;
                    DualResult dualResult = MyUtility.Tool.ProcessWithDatatable(tmp, string.Empty, sqlcmd, out dt);
                    if (!dualResult)
                    {
                        this.ShowErr(dualResult);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        List<string> packingListID = dt.AsEnumerable().Where(w => MyUtility.Convert.GetString(w["Forwarder"]) != MyUtility.Convert.GetString(this.masterData["Forwarder"])).Select(s => MyUtility.Convert.GetString(s["id"])).Distinct().ToList();
                        if (packingListID.Count > 0)
                        {
                            string pid = string.Join(",", packingListID);
                            string msg = $@"Forwarder is different from APP request, please double check.
Packing List : {pid}";
                            MyUtility.Msg.WarningBox(msg);
                        }
                    }
                }
            }

            MyUtility.Msg.InfoBox("Import finished!");
            this.Close();
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
