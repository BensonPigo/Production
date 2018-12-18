using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_CTNStatus
    /// </summary>
    public partial class P01_CTNStatus : Sci.Win.Subs.Base
    {
        private string orderID;
        private bool canRecompute;

        /// <summary>
        /// P01_CTNStatus
        /// </summary>
        /// <param name="orderID">string orderID</param>
        /// <param name="canRecompute">bool canRecompute</param>
        public P01_CTNStatus(string orderID, bool canRecompute)
        {
            this.InitializeComponent();
            this.orderID = orderID;
            this.canRecompute = canRecompute;
            this.SetCombo1Source();
            this.SetCombo2Source();
            MyUtility.Tool.SetupCombox(this.comboSortby, 1, 1, ",Location,Ctn#,Packing List ID,Rec. Date");
            this.comboPackingListID.SelectedIndex = -1;
            this.comboCTN.SelectedIndex = -1;
            this.comboSortby.Text = string.Empty;
            this.labelSortby.Visible = false;
            this.comboSortby.Visible = false;
            this.btnRecompute.Enabled = canRecompute;
        }

        private void SetCombo1Source()
        {
            DataTable packingID;
            string sqlCmd = string.Format("select distinct ID from PackingList_Detail WITH (NOLOCK) where OrderID = '{0}' and CTNStartNo <> '' and CTNQty > 0", this.orderID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out packingID);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Packing ID fail !!");
            }

            MyUtility.Tool.SetupCombox(this.comboPackingListID, 1, packingID);
        }

        private void SetCombo2Source()
        {
            DataTable ctnDt;
            string sqlCmd = string.Format(
                @"select CTNStartNo 
from (select CTNStartNo, MIN(Seq) as Seq from PackingList_Detail WITH (NOLOCK) where OrderID = '{0}' and CTNStartNo <> '' and CTNQty > 0 group by CTNStartNo) a
order by Seq", this.orderID);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out ctnDt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query CTNStartNo fail !!");
            }

            MyUtility.Tool.SetupCombox(this.comboCTN, 1, ctnDt);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable transferDetail, ctnLastStatus;
            #region 組撈Transaction Detail的Sql
            string sqlCmd = string.Format(
                @"with Transferclog
as(
select t.PackingListID,t.CTNStartNo,'Fty Send to Clog' as Type,t.ID,t.TransferDate as TypeDate,'' as Location,t.AddDate as UpdateDate, isnull(pd.Seq,0) as Seq
from TransferToClog t WITH (NOLOCK) 
left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = t.PackingListID and pd.OrderID = t.OrderID and pd.CTNStartNo = t.CTNStartNo
where t.OrderID = '{0}' and pd.CTNQty > 0
),
CReceive
as(
select c.PackingListId,c.CTNStartNo,'Clog Receive from Fty' as Type,c.ID,c.ReceiveDate as TypeDate,c.ClogLocationId as Location,
c.AddDate UpdateDate, isnull(pd.Seq,0) as Seq
from ClogReceive c WITH (NOLOCK) 
left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = c.PackingListID and pd.OrderID = c.OrderID and pd.CTNStartNo = c.CTNStartNo
where c.OrderId = '{0}' and pd.CTNQty > 0
),
CReturn 
as (
select c.PackingListId,c.CTNStartNo,'Clog Return to Fty' as Type,c.ID,c.ReturnDate as TypeDate,'' as Location,
c.AddDate as UpdateDate, isnull(pd.Seq,0) as Seq
from ClogReturn c WITH (NOLOCK) 
left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = c.PackingListID and pd.OrderID = c.OrderID and pd.CTNStartNo = c.CTNStartNo
where c.OrderId = '{0}' and pd.CTNQty > 0
),
TransferCFA
as (
select t.PackingListID,t.CTNStartNo,'Clog Send to CFA' as Type,t.id,t.TransferDate as TypeDate,'' as Location,t.AddDate as UpdateDate
,isnull(pd.Seq,0) as Seq
from TransferToCFA t WITH (NOLOCK) 
left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = t.PackingListID and pd.OrderID = t.OrderID and pd.CTNStartNo = t.CTNStartNo
where t.OrderID = '{0}' and pd.CTNQty > 0  
),
ReceiveCFA
as (
select c.PackingListId,c.CTNStartNo,'CFA Receive from Clog' as Type,c.id,c.ReceiveDate as TypeDate,'' as Location,
c.AddDate UpdateDate ,isnull(pd.Seq,0) as Seq
from CFAReceive c WITH (NOLOCK) 
left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = c.PackingListID and pd.OrderID = c.OrderID and pd.CTNStartNo = c.CTNStartNo
where c.OrderId = '{0}' and pd.CTNQty > 0
),
ReturnCFA
as (
select c.PackingListId,c.CTNStartNo,'CFA Return to ' + c.ReturnTo as Type ,c.id,c.ReturnDate as TypeDate,'' as Location,
c.AddDate as UpdateDate ,isnull(pd.Seq,0) as Seq
from CFAReturn c WITH (NOLOCK) 
left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = c.PackingListID and pd.OrderID = c.OrderID and pd.CTNStartNo = c.CTNStartNo
where c.OrderId = '{0}' and pd.CTNQty > 0 
),
CReceiveCFA 
as (
select c.PackingListId,c.CTNStartNo,'Clog Receive from CFA'  as Type, c.id,c.ReceiveDate as TypeDate,'' as Location,
c.AddDate as UpdateDate ,isnull(pd.Seq,0) as Seq
from ClogReceiveCFA c WITH (NOLOCK) 
left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = c.PackingListID and pd.OrderID = c.OrderID and pd.CTNStartNo = c.CTNStartNo
where c.OrderId = '{0}' and pd.CTNQty > 0  
),
DryRoomReceive 
as (
select c.PackingListId,c.CTNStartNo,'Dry Room Receive from Fty' as Type,c.id,c.ReceiveDate as TypeDate, '' as Location, 
c.AddDate as UpdateDate , isnull(pd.Seq,0) as Seq
from DryReceive c WITH (NOLOCK) 
left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = c.PackingListID and pd.OrderID = c.OrderID and pd.CTNStartNo = c.CTNStartNo
where c.OrderId = '{0}' and pd.CTNQty > 0
),
DryRoomTransfer 
as (
select c.PackingListId,c.CTNStartNo,'Dry Room Transfer to '+ c.TransferTo as Type,c.ID, c.TransferDate as TypeDate , '' as Location, 
c.AddDate as UpdateDate, isnull(pd.Seq,0) as Seq
from DryTransfer c WITH (NOLOCK) 
left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = c.PackingListID and pd.OrderID = c.OrderID and pd.CTNStartNo = c.CTNStartNo
where c.OrderId = '{0}' and pd.CTNQty > 0
)

select * from Transferclog
union all
select * from CReceive
union all
select * from CReturn
union all
select * from TransferCFA
union all
select * from ReceiveCFA
union all
select * from ReturnCFA
union all
select * from CReceiveCFA 
union all
select * from DryRoomReceive 
union all
select * from DryRoomTransfer 
order by PackingListID,Seq,UpdateDate", this.orderID);
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out transferDetail);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query transfer detail fail!!" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = transferDetail;

            sqlCmd = string.Format(
                @"
select [PackingListID] =  p.ID 
,pd.CTNStartNo
,[Scanned] = iif(Scanned.QtyPerCTN=Scanned.ScanQty,'Y','')
,pd.TransferDate
,pd.ReceiveDate
,pd.ReturnDate
,pd.DryReceiveDate
,pd.TransferCFADate
,pd.CFAReceiveDate
,pd.CFAReturnClogDate
,pd.ClogReceiveCFADate
,pd.CFAReturnFtyDate
,p.PulloutDate
,pd.ClogLocationId
,pd.EditLocationDate
,EditLocationName=pd.EditLocationName +'-'+(select name from pass1 where id=pd.EditLocationName)
,pd.Remark
,pd.Seq
from PackingList p WITH (NOLOCK) ,PackingList_Detail pd WITH (NOLOCK) 
outer apply(
	select sum(QtyPerCTN) QtyPerCTN ,sum(ScanQty) ScanQty 
	from PackingList_Detail
	where ID=pd.id and CTNStartNo=pd.CTNStartNo
)Scanned
where pd.OrderID = '{0}' and pd.CTNStartNo <> '' and pd.CTNQty > 0 and p.ID = pd.ID
order by p.ID,pd.Seq", this.orderID);
            result = DBProxy.Current.Select(null, sqlCmd, out ctnLastStatus);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query last status fail!!" + result.ToString());
            }

            this.listControlBindingSource2.DataSource = ctnLastStatus;

            // 設定Grid1的顯示欄位
            this.gridTransactionDetali.IsEditingReadOnly = true;
            this.gridTransactionDetali.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridTransactionDetali)
                .Text("PackingListID", header: "Packing List ID", width: Widths.AnsiChars(15))
                .Text("CTNStartNo", header: "Ctn#", width: Widths.AnsiChars(6))
                .Text("Type", header: "Trans. Type", width: Widths.AnsiChars(20))
                .Date("TypeDate", header: "Trans. Date", width: Widths.AnsiChars(10))
                .Text("Location", header: "Location", width: Widths.AnsiChars(8))
                .DateTime("UpdateDate", header: "Last update datetime", width: Widths.AnsiChars(20));

            // 設定Grid2的顯示欄位
            this.gridLastStatus.IsEditingReadOnly = true;
            this.gridLastStatus.DataSource = this.listControlBindingSource2;
            this.Helper.Controls.Grid.Generator(this.gridLastStatus)
                .Text("PackingListID", header: "Packing List ID", width: Widths.AnsiChars(15))
                .Text("CTNStartNo", header: "Ctn#", width: Widths.AnsiChars(6))
                .Text("Scanned", header: "Scanned", width: Widths.AnsiChars(6))
                .Date("TransferDate", header: "Trans. Date", width: Widths.AnsiChars(10))
                .Date("ReceiveDate", header: "Rec. Date", width: Widths.AnsiChars(10))
                .Date("ReturnDate", header: "Return Date", width: Widths.AnsiChars(10))
                .Date("DryReceiveDate", header: "Dry Room Rec. Date", width: Widths.AnsiChars(10))
                .Date("TransferCFADate", header: "Trans. CFA Date", width: Widths.AnsiChars(10))
                .Date("CFAReceiveDate", header: "CFA Rec. Clog Date", width: Widths.AnsiChars(10))
                .Date("CFAReturnClogDate", header: "CFA Return Clog Date", width: Widths.AnsiChars(10))
                .Date("ClogReceiveCFADate", header: "Clog Rec. CFA Date", width: Widths.AnsiChars(10))
                .Date("CFAReturnFtyDate", header: "CFA Return Fty Date", width: Widths.AnsiChars(10))
                .Date("PulloutDate", header: "Pull-out Date", width: Widths.AnsiChars(10))
                .Text("ClogLocationId", header: "Location", width: Widths.AnsiChars(8))
                .Text("EditLocationDate", header: "Edit Location Date", width: Widths.AnsiChars(10))
                .Text("EditLocationName", header: "Edit Location By", width: Widths.AnsiChars(10))
                .EditText("Remark", header: "Remark", width: Widths.AnsiChars(20));
        }

        private void TabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            this.labelSortby.Visible = e.TabPageIndex == 1;
            this.comboSortby.Visible = e.TabPageIndex == 1;
        }

        // Packing List ID
        private void ComboPackingListID_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetFilter();
        }

        // CTN#
        private void ComboCTN_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetFilter();
        }

        // Sort by
        private void ComboSortby_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SortBy();
        }

        private void SetFilter()
        {
            StringBuilder filterString = new StringBuilder();
            filterString.Append("1=1");
            if (this.comboPackingListID.SelectedIndex != -1)
            {
                filterString.Append(string.Format(" and PackingListID = '{0}'", this.comboPackingListID.SelectedValue.ToString()));
            }

            if (this.comboCTN.SelectedIndex != -1)
            {
                filterString.Append(string.Format(" and CTNStartNo = '{0}'", this.comboCTN.SelectedValue.ToString()));
            }

            if (((DataTable)this.listControlBindingSource1.DataSource) != null)
            {
                ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = filterString.ToString();
            }

            if (((DataTable)this.listControlBindingSource2.DataSource) != null)
            {
                ((DataTable)this.listControlBindingSource2.DataSource).DefaultView.RowFilter = filterString.ToString();
            }
        }

        private void SortBy()
        {
            if (this.comboSortby.SelectedIndex != -1)
            {
                DataTable grid2 = (DataTable)this.listControlBindingSource2.DataSource;
                switch (this.comboSortby.SelectedValue.ToString())
                {
                    case "Location":
                        grid2.DefaultView.Sort = "ClogLocationId";
                        break;
                    case "Ctn#":
                        grid2.DefaultView.Sort = "CTNStartNo";
                        break;
                    case "Packing List ID":
                        grid2.DefaultView.Sort = "PackingListID";
                        break;
                    case "Rec. Date":
                        grid2.DefaultView.Sort = "ReceiveDate";
                        break;
                    default:
                        if (grid2 != null)
                        {
                            grid2.DefaultView.Sort = "PackingListID,Seq";
                        }

                        break;
                }
            }
        }

        // Recompute
        private void BtnRecompute_Click(object sender, EventArgs e)
        {
            bool prgResult = Prgs.UpdateOrdersCTN(this.orderID);
            if (!prgResult)
            {
                MyUtility.Msg.WarningBox("Recompute fail, pls try again!!");
            }
        }
    }
}
