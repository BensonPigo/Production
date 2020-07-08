using System;
using System.Data;
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
                @"
	select
	ID,
	CTNStartNo,
	OrderID,
	Seq,
	OrigID,
	OrigOrderID,
	OrigCTNStartNo
	into #PackingList_Detail
	from PackingList_Detail 
	where OrderID = '{0}' 
          and CTNQty > 0

	--Transferclog
	select *
	into #Transferclog
	from(
		select PackingListID = pd.ID
				, pd.CTNStartNo
		        , Type = 'Fty Send to Clog'
		        , t.ID
		        , TypeDate = t.TransferDate
		        , Location = '' 
		        , UpdateDate = t.AddDate 
		        , Seq = isnull(pd.Seq,0) 
		        , pd.OrigID
		        , pd.OrigOrderID
		        , pd.OrigCTNStartNo
		from #PackingList_Detail pd 
		inner join TransferToClog t  WITH (NOLOCK) on pd.ID = t.PackingListID 
															and pd.OrderID = t.OrderID 
															and pd.CTNStartNo = t.CTNStartNo
		where t.PackingListID != ''
		      and t.OrderID != ''
		      and t.CTNStartNo != ''
		union all
		select PackingListID = pd.ID
				, pd.CTNStartNo
		        , Type = 'Fty Send to Clog'
		        , t.ID
		        , TypeDate = t.TransferDate
		        , Location = '' 
		        , UpdateDate = t.AddDate 
		        , Seq = isnull(pd.Seq,0) 
		        , pd.OrigID
		        , pd.OrigOrderID
		        , pd.OrigCTNStartNo
		from #PackingList_Detail pd 
		inner join TransferToClog t  WITH (NOLOCK) on pd.OrigID = t.PackingListID
														 	and pd.OrigOrderID = t.OrderID
														 	and pd.OrigCTNStartNo = t.CTNStartNo
		where t.PackingListID != ''
		      and t.OrderID != ''
		      and t.CTNStartNo != '') t

	
		
	--CReceive
	select *
	into #CReceive
	from (
		select PackingListID = pd.ID
				, pd.CTNStartNo
		        , Type = 'Clog Receive from Fty' 
		        , c.ID
		        , TypeDate = c.ReceiveDate 
		        , Location = c.ClogLocationId 
		        , UpdateDate = c.AddDate 
		        , Seq = isnull(pd.Seq,0) 
		        , pd.OrigID
		        , pd.OrigOrderID
		        , pd.OrigCTNStartNo
		from #PackingList_Detail pd 
		inner join ClogReceive c WITH (NOLOCK) on pd.ID = c.PackingListID 
												  and pd.OrderID = c.OrderID 
												  and pd.CTNStartNo = c.CTNStartNo
		where c.PackingListID != ''
		      and c.OrderID != ''
		      and c.CTNStartNo != ''
		union
		select PackingListID = pd.ID
				, pd.CTNStartNo
		        , Type = 'Clog Receive from Fty' 
		        , c.ID
		        , TypeDate = c.ReceiveDate 
		        , Location = c.ClogLocationId 
		        , UpdateDate = c.AddDate 
		        , Seq = isnull(pd.Seq,0) 
		        , pd.OrigID
		        , pd.OrigOrderID
		        , pd.OrigCTNStartNo
		from #PackingList_Detail pd 
		inner join ClogReceive c WITH (NOLOCK) on pd.OrigID = c.PackingListID
														 	and pd.OrigOrderID = c.OrderID
														 	and pd.OrigCTNStartNo = c.CTNStartNo
		where c.PackingListID != ''
		      and c.OrderID != ''
		      and c.CTNStartNo != ''
	) t

	--CReturn
	select *
	into #CReturn
	from (
	select PackingListID = pd.ID
			, pd.CTNStartNo
            , Type = 'Clog Return to Fty' 
            , c.ID
            , TypeDate = c.ReturnDate 
            , Location = '' 
            , UpdateDate = c.AddDate 
            , Seq = isnull(pd.Seq,0) 
            , pd.OrigID
            , pd.OrigOrderID
            , pd.OrigCTNStartNo
    from #PackingList_Detail pd  
    inner join ClogReturn c WITH (NOLOCK) on pd.ID = c.PackingListID 
													  and pd.OrderID = c.OrderID 
													  and pd.CTNStartNo = c.CTNStartNo
                                                      
	where c.PackingListID != ''
          and c.OrderID != ''
          and c.CTNStartNo != ''
	union all
	select PackingListID = pd.ID
			, pd.CTNStartNo
            , Type = 'Clog Return to Fty' 
            , c.ID
            , TypeDate = c.ReturnDate 
            , Location = '' 
            , UpdateDate = c.AddDate 
            , Seq = isnull(pd.Seq,0) 
            , pd.OrigID
            , pd.OrigOrderID
            , pd.OrigCTNStartNo
    from #PackingList_Detail pd  
    inner join ClogReturn c WITH (NOLOCK) on   pd.OrigID = c.PackingListID
    												 	and pd.OrigOrderID = c.OrderID
    												 	and pd.OrigCTNStartNo = c.CTNStartNo
                                                      
	where c.PackingListID != ''
          and c.OrderID != ''
          and c.CTNStartNo != ''
	) t
	

	--TransferCFA
	select *
	into #TransferCFA
	from (
		select PackingListID = pd.ID
				, pd.CTNStartNo
				, Type = 'Clog Send to CFA' 
				, t.id
				, TypeDate = t.TransferDate 
				, Location = '' 
				, UpdateDate = t.AddDate  
				, Seq = isnull(pd.Seq,0) 
				, pd.OrigID
				, pd.OrigOrderID
				, pd.OrigCTNStartNo
		from #PackingList_Detail pd
		inner join TransferToCFA t WITH (NOLOCK) on pd.ID = t.PackingListID 
													and pd.OrderID = t.OrderID 
													and pd.CTNStartNo = t.CTNStartNo
														 
		where  t.PackingListID != ''
		      and t.OrderID != ''
		      and t.CTNStartNo != ''
		union all
		select PackingListID = pd.ID
				, pd.CTNStartNo
				, Type = 'Clog Send to CFA' 
				, t.id
				, TypeDate = t.TransferDate 
				, Location = '' 
				, UpdateDate = t.AddDate  
				, Seq = isnull(pd.Seq,0) 
				, pd.OrigID
				, pd.OrigOrderID
				, pd.OrigCTNStartNo
		from #PackingList_Detail pd
		inner join TransferToCFA t WITH (NOLOCK) on pd.OrigID = t.PackingListID
    												 	and pd.OrigOrderID = t.OrderID
    												 	and pd.OrigCTNStartNo = t.CTNStartNo
														 
		where  t.PackingListID != ''
		      and t.OrderID != ''
		      and t.CTNStartNo != ''
	) t
	
	--ReceiveCFA
	select *
	into #ReceiveCFA
	from (
		select PackingListID = pd.ID
				, pd.CTNStartNo
				, Type = 'CFA Receive from Clog' 
				, c.id
				, TypeDate = c.ReceiveDate 
				, Location = '' 
				, UpdateDate  = c.AddDate 
				, Seq = isnull(pd.Seq, 0) 
				, pd.OrigID
				, pd.OrigOrderID
				, pd.OrigCTNStartNo
		from #PackingList_Detail pd 
		inner join CFAReceive c WITH (NOLOCK) on pd.ID = c.PackingListID 
												 and pd.OrderID = c.OrderID 
												 and pd.CTNStartNo = c.CTNStartNo
		where  c.PackingListID != ''
		      and c.OrderID != ''
		      and c.CTNStartNo != ''
		union all
		select PackingListID = pd.ID
				, pd.CTNStartNo
				, Type = 'CFA Receive from Clog' 
				, c.id
				, TypeDate = c.ReceiveDate 
				, Location = '' 
				, UpdateDate  = c.AddDate 
				, Seq = isnull(pd.Seq, 0) 
				, pd.OrigID
				, pd.OrigOrderID
				, pd.OrigCTNStartNo
		from #PackingList_Detail pd 
		inner join CFAReceive c WITH (NOLOCK) on pd.OrigID = c.PackingListID
												 and pd.OrigOrderID = c.OrderID
												 and pd.OrigCTNStartNo = c.CTNStartNo
		where  c.PackingListID != ''
		      and c.OrderID != ''
		      and c.CTNStartNo != ''
	) t
	
	--ReturnCFA
	select *
	into #ReturnCFA
	from (
		select PackingListID = pd.ID
				, pd.CTNStartNo
				, Type = 'CFA Return to ' + c.ReturnTo 
				, c.id
				, TypeDate = c.ReturnDate 
				, Location = '' 
				, UpdateDate  = c.AddDate 
				, Seq = isnull(pd.Seq, 0) 
				, pd.OrigID
				, pd.OrigOrderID
				, pd.OrigCTNStartNo
		from #PackingList_Detail pd 
		inner join CFAReturn c WITH (NOLOCK) on pd.ID = c.PackingListID 
												and pd.OrderID = c.OrderID 
												and pd.CTNStartNo = c.CTNStartNo
		where c.PackingListID != ''
		      and c.OrderID != ''
		      and c.CTNStartNo != ''
		union all
		select PackingListID = pd.ID
				, pd.CTNStartNo
				, Type = 'CFA Return to ' + c.ReturnTo 
				, c.id
				, TypeDate = c.ReturnDate 
				, Location = '' 
				, UpdateDate  = c.AddDate 
				, Seq = isnull(pd.Seq, 0) 
				, pd.OrigID
				, pd.OrigOrderID
				, pd.OrigCTNStartNo
		from #PackingList_Detail pd 
		inner join CFAReturn c WITH (NOLOCK) on pd.OrigID = c.PackingListID
												and pd.OrigOrderID = c.OrderID
												and pd.OrigCTNStartNo = c.CTNStartNo			
		where c.PackingListID != ''
		      and c.OrderID != ''
		      and c.CTNStartNo != ''
	) t
	

	--CReceiveCFA
	select *
	into #CReceiveCFA
	from (
	select PackingListID = pd.ID
			, pd.CTNStartNo
			, Type = 'Clog Receive from CFA'  
			, c.id
			, TypeDate = c.ReceiveDate 
			, Location = '' 
			, UpdateDate = c.AddDate 
			, Seq = isnull(pd.Seq, 0) 
    		, pd.OrigID
    		, pd.OrigOrderID
    		, pd.OrigCTNStartNo
	from #PackingList_Detail pd
	inner join ClogReceiveCFA c WITH (NOLOCK) on pd.ID = c.PackingListID 
												 and pd.OrderID = c.OrderID 
												 and pd.CTNStartNo = c.CTNStartNo
	where c.PackingListID != ''
          and c.OrderID != ''
          and c.CTNStartNo != ''
	union all
	select PackingListID = pd.ID
			, pd.CTNStartNo
			, Type = 'Clog Receive from CFA'  
			, c.id
			, TypeDate = c.ReceiveDate 
			, Location = '' 
			, UpdateDate = c.AddDate 
			, Seq = isnull(pd.Seq, 0) 
    		, pd.OrigID
    		, pd.OrigOrderID
    		, pd.OrigCTNStartNo
	from #PackingList_Detail pd
	inner join ClogReceiveCFA c WITH (NOLOCK) on pd.OrigID = c.PackingListID
    										     and pd.OrigOrderID = c.OrderID
    										     and pd.OrigCTNStartNo = c.CTNStartNo
	where c.PackingListID != ''
          and c.OrderID != ''
          and c.CTNStartNo != ''
	) t
	

	--DryRoomReceive
	select * 
	into #DryRoomReceive
	from (
		select PackingListID = pd.ID
				, pd.CTNStartNo
				, Type = 'Dry Room Scan' 
				, c.id
				, TypeDate = c.ReceiveDate 
				, Location = '' 
				, UpdateDate = c.AddDate 
				, Seq = isnull(pd.Seq, 0) 
				, pd.OrigID
				, pd.OrigOrderID
				, pd.OrigCTNStartNo
		from #PackingList_Detail pd 
		inner join DryReceive c WITH (NOLOCK) on pd.ID = c.PackingListID 
												 and pd.OrderID = c.OrderID 
												 and pd.CTNStartNo = c.CTNStartNo
														
		where c.PackingListID != ''
		      and c.OrderID != ''
		      and c.CTNStartNo != ''
		union all
		select PackingListID = pd.ID
				, pd.CTNStartNo
				, Type = 'Dry Room Scan' 
				, c.id
				, TypeDate = c.ReceiveDate 
				, Location = '' 
				, UpdateDate = c.AddDate 
				, Seq = isnull(pd.Seq, 0) 
				, pd.OrigID
				, pd.OrigOrderID
				, pd.OrigCTNStartNo
		from #PackingList_Detail pd 
		inner join DryReceive c WITH (NOLOCK) on pd.OrigID = c.PackingListID
														 	and pd.OrigOrderID = c.OrderID
														 	and pd.OrigCTNStartNo = c.CTNStartNo
														
		where c.PackingListID != ''
		      and c.OrderID != ''
		      and c.CTNStartNo != ''
	) t
	

	--DryRoomTransfer
	select *
	into #DryRoomTransfer
	from (
		select PackingListID = pd.ID
				, pd.CTNStartNo
				, Type = 'Dry Room Transfer to '+ c.TransferTo
				, c.ID
				, TypeDate = c.TransferDate 
				, Location = '' 
				, UpdateDate = c.AddDate 
				, Seq = isnull(pd.Seq, 0) 
				, pd.OrigID
				, pd.OrigOrderID
				, pd.OrigCTNStartNo
		from #PackingList_Detail pd
		inner join DryTransfer c WITH (NOLOCK) on pd.ID = c.PackingListID 
												  and pd.OrderID = c.OrderID 
												  and pd.CTNStartNo = c.CTNStartNo
														
		where c.PackingListID != ''
		      and c.OrderID != ''
		      and c.CTNStartNo != ''
		union all
		select PackingListID = pd.ID
				, pd.CTNStartNo
				, Type = 'Dry Room Transfer to '+ c.TransferTo
				, c.ID
				, TypeDate = c.TransferDate 
				, Location = '' 
				, UpdateDate = c.AddDate 
				, Seq = isnull(pd.Seq, 0) 
				, pd.OrigID
				, pd.OrigOrderID
				, pd.OrigCTNStartNo
		from #PackingList_Detail pd
		inner join DryTransfer c WITH (NOLOCK) on pd.OrigID = c.PackingListID
														 	and pd.OrigOrderID = c.OrderID
														 	and pd.OrigCTNStartNo = c.CTNStartNo
														
		where c.PackingListID != ''
		      and c.OrderID != ''
		      and c.CTNStartNo != ''
	) t
	
	--MDScan
	select *
	into #MDScan
	from (
		select m.PackingListID
			, m.CTNStartNo
			, [type] = 'MD Room Scan'
			, m.Ukey
			, m.ScanDate
			, [Location] = ''
			, [UpdateDate] = m.AddDate
			, [seq] = pd.Seq
			, pd.OrigID
			, pd.OrigOrderID
			, pd.OrigCTNStartNo
		from MDScan m WITH (NOLOCK)
		inner join #PackingList_Detail pd on pd.ID = m.PackingListID
												  and pd.OrderID = m.OrderID 
												  and pd.CTNStartNo = m.CTNStartNo 
		where m.PackingListID != ''
		      and m.OrderID != ''
		      and m.CTNStartNo != ''

		union all
		select m.PackingListID
			, m.CTNStartNo
			, [type] = 'MD Room Scan'
			, m.Ukey
			, m.ScanDate
			, [Location] = ''
			, [UpdateDate] = m.AddDate
			, [seq] = pd.Seq
			, pd.OrigID
			, pd.OrigOrderID
			, pd.OrigCTNStartNo
		from MDScan m WITH (NOLOCK)
		inner join #PackingList_Detail pd on pd.OrigID = m.PackingListID
												  and pd.OrigOrderID = m.OrderID 
												  and pd.OrigCTNStartNo = m.CTNStartNo
		where m.PackingListID != ''
		      and m.OrderID != ''
		      and m.CTNStartNo != ''
	) t

select * from #Transferclog
union all
select * from #CReceive
union all
select * from #CReturn
union all
select * from #TransferCFA
union all
select * from #ReceiveCFA
union all
select * from #ReturnCFA
union all
select * from #CReceiveCFA 
union all
select * from #DryRoomReceive 
union all
select * from #DryRoomTransfer 
union all
select * from #MDScan
order by PackingListID,Seq,UpdateDate

drop table #PackingList_Detail,#Transferclog,#CReceive,#CReturn,#TransferCFA,#ReceiveCFA,#ReturnCFA,#CReceiveCFA ,#DryRoomReceive ,#DryRoomTransfer ,#MDScan
", this.orderID);
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
,pd.CFALocationID
,pd.EditCFALocationDate
,EditCFALocationName=concat(pd.EditCFALocationName,'-'+(select name from pass1 where id=pd.EditCFALocationName))
,pd.Remark
,pd.Seq
,pd.MDScanDate
,pd.MDFailQty
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
                .DateTime("UpdateDate", header: "Last update datetime", width: Widths.AnsiChars(20))
                .Text("OrigID", header: "Repack from Pack ID", width: Widths.AnsiChars(15))
                .Text("OrigCTNStartNo", header: "Repack from Ctn#", width: Widths.AnsiChars(6));

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
                .Date("DryReceiveDate", header: "Dry Room Scan Date", width: Widths.AnsiChars(10))
                .Date("MDScanDate", header: "MD Room Scan Date", width: Widths.AnsiChars(10))
                .Text("MDFailQty", header: "MD Discrepancy", width: Widths.AnsiChars(6))
                .Date("TransferCFADate", header: "Trans. CFA Date", width: Widths.AnsiChars(10))
                .Date("CFAReceiveDate", header: "CFA Rec. Clog Date", width: Widths.AnsiChars(10))
                .Date("CFAReturnClogDate", header: "CFA Return Clog Date", width: Widths.AnsiChars(10))
                .Date("ClogReceiveCFADate", header: "Clog Rec. CFA Date", width: Widths.AnsiChars(10))
                .Date("CFAReturnFtyDate", header: "CFA Return Fty Date", width: Widths.AnsiChars(10))
                .Date("PulloutDate", header: "Pull-out Date", width: Widths.AnsiChars(10))
                .Text("ClogLocationId", header: "Clog Location", width: Widths.AnsiChars(8))
                .Text("EditLocationDate", header: "Edit Clog Location Date", width: Widths.AnsiChars(10))
                .Text("EditLocationName", header: "Edit Clog Location By", width: Widths.AnsiChars(10))
                .Text("CFALocationID", header: "CFA Location", width: Widths.AnsiChars(8))
                .Text("EditCFALocationDate", header: "Edit Clog Location Date", width: Widths.AnsiChars(10))
                .Text("EditCFALocationName", header: "Edit Clog Location By", width: Widths.AnsiChars(10))
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
