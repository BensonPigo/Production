﻿using Ict;
using Sci.Data;
using System;
using System.ComponentModel;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class R04 : Win.Tems.PrintForm
    {
        private DateTime? buyerDlv1;
        private DateTime? buyerDlv2;
        private DateTime? estPullout1;
        private DateTime? estPullout2;
        private DateTime? fCRDate1;
        private DateTime? fCRDate2;
        private string brand;
        private string mDivision;
        private string orderNo;
        private string factory;
        private string category;
        private string buyer;
        private string custCD;
        private string destination;
        private int summaryBy;
        private bool includeLO;
        private DataTable printData;
        private string whereExcludePullout = string.Empty;
        private string whereExcludePulloutOuterApply = string.Empty;

        /// <inheritdoc/>
        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out DataTable mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out DataTable factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            MyUtility.Tool.SetupCombox(this.comboSummaryBy, 2, 1, "0,SP# and Seq,1,PL#");
            this.comboM.Text = Env.User.Keyword;
            this.comboFactory.SelectedIndex = -1;
            this.comboSummaryBy.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.mDivision = this.comboM.Text;
            this.buyerDlv1 = this.dateBuyerDelivery.Value1;
            this.buyerDlv2 = this.dateBuyerDelivery.Value2;
            this.estPullout1 = this.dateEstimatePullout.Value1;
            this.estPullout2 = this.dateEstimatePullout.Value2;
            this.fCRDate1 = this.dateFCRDate.Value1;
            this.fCRDate2 = this.dateFCRDate.Value2;
            this.brand = this.txtbrand.Text;
            this.factory = this.comboFactory.Text;
            this.category = this.comboCategory.SelectedValue.ToString();
            this.buyer = this.txtbuyer.Text;
            this.custCD = this.txtcustcd.Text;
            this.destination = this.txtcountryDestination.TextBox1.Text;
            this.orderNo = this.txtOrderNo.Text;
            this.includeLO = this.checkIncludeLocalOrder.Checked;
            this.summaryBy = this.comboSummaryBy.SelectedIndex;

            if (this.chkExcludePullout.Checked)
            {
                this.whereExcludePullout = $@"
and (
        exists(
            select 1 from packinglist p with (nolock)
	        		 where exists(select 1 from packinglist_detail pd with (nolock) 
                                    where   p.id = pd.id and 
                                            pd.orderid = o.id and 
                                            pd.OrderShipmodeSeq = oq.seq)  and
                            (p.PulloutDate is null or p.PulloutID is null)
                
        )
        or
        not exists (
            select 1 from packinglist p with (nolock)
	        		 where exists(select 1 from packinglist_detail pd with (nolock) 
                                    where   p.id = pd.id and 
                                            pd.orderid = o.id and 
                                            pd.OrderShipmodeSeq = oq.seq)
        )
    )
";
                this.whereExcludePulloutOuterApply = " and (p.PulloutDate is null or p.PulloutID is null)";
            }
            else
            {
                this.whereExcludePullout = string.Empty;
                this.whereExcludePulloutOuterApply = string.Empty;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();

            if (this.summaryBy == 0)
            {
                sqlCmd = this.SummaryBySP(sqlCmd);
            }
            else
            {
                sqlCmd = this.SummaryByPL(sqlCmd);
            }

            sqlCmd = this.SQLWhere(sqlCmd);

            if (this.summaryBy == 0)
            {
                // sqlCmd = this.SummaryBySP2(sqlCmd); // ISP20201243 先加但先不要
            }
            else
            {
                sqlCmd = this.SummaryByPL2(sqlCmd);
                sqlCmd = this.SQLWhere(sqlCmd);
            }

            sqlCmd.Append(" order by oq.BuyerDelivery,o.ID,oq.seq");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Env.Cfg.XltPathDir + "\\Shipping_R04_EstimateOutstandingShipmentReport.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Shipping_R04_EstimateOutstandingShipmentReport.xltx", 1, false, null, excel, wSheet: excel.Sheets[1]);

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_R04_EstimateOutstandingShipmentReport");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

        // CustCD
        private void Txtcustcd_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.txtcustcd.Text) && this.txtcustcd.OldValue != this.txtcustcd.Text)
            {
                this.txtcountryDestination.TextBox1.Text = MyUtility.GetValue.Lookup(string.Format("SELECT CountryID FROM CustCD WITH (NOLOCK) WHERE BrandID = '{0}' AND ID = '{1}'", this.txtbrand.Text, this.txtcustcd.Text));
            }
        }

        private StringBuilder SummaryBySP(StringBuilder sqlCmd)
        {
            sqlCmd.Append(
$@"select 	oq.BuyerDelivery
		,oq.IDD
		,o.BrandID
		,b.BuyerID
		,o.ID
        ,[Cancel] = IIF(o.Junk=1,'Y','N')
		,Category = IIF(o.Category = 'B', 'Bulk'
										, 'Sample')
        ,oq.seq
        ,[If Partial] = (select iif(count(1) > 1, 'Y', '') from Order_QtyShip with (nolock) where ID = o.ID)
		,pkid.pkid
        ,pkstatus.v
		,pkINVNo.pkINVNo
		,gb.FCRDate
		,pkPulloutDate.PulloutDate
        ,pkPulloutID.PulloutID
		,o.CustPONo
		,o.StyleID
		,o.SeasonID
		,oq.Qty
		,ShipQty = (select isnull(sum(ShipQty), 0) 
					from Pullout_Detail WITH (NOLOCK) 
					where OrderID = o.ID and OrderShipmodeSeq = oq.Seq)
        ,FOCBalQty = isnull(dbo.GetFocStockByOrder(o.ID),0)
		,OrderTtlQty=o.Qty
		,ShipTtlQty=isnull(plds.ShipQty,0)
        ,plds.CTNQty
        ,gb2.SONo
        ,gb2.SOCFMDate
        ,gb2.CutOffDate
		,[ShipPlanID] = gb2.ShipPlanID
        ,[Carton Qty at C-Log] = isnull(o.ClogCTN, 0)
        ,[SP Prod. Output Qty] = [dbo].[getMinCompleteSewQty](o.ID, null, null)
		,o.MDivisionID
		,o.ftygroup
        ,[KPI Factory] = f.Kpicode
        ,o.CustCDID
		,Alias = isnull(c.Alias,'')
        ,o.OrderTypeID
        ,[On Site] = iif(o.OnSiteSample = 1, 'Y', '')
        ,[BuyBack] = iif(exists(select 1 from Order_BuyBack with (nolock) where ID = o.ID), 'Y', '')
		,Payment = isnull((select Term 
						   from PayTermAR WITH (NOLOCK) 
						   where ID = o.PayTermARID), '')
		,o.PoPrice
		,o.Customize1
		,o.Customize2
		,plds.GW
        ,plds.VM
		,cbm.CTNQty
		,oq.ShipmodeID
        ,[Loading Type] = gbCYCFS.CYCFS
        ,OSReason = o.OutstandingReason + ' - ' + isnull((select Name 
														   from Reason WITH (NOLOCK) 
														   where ReasonTypeID = 'Delivery_OutStand' and Id = o.OutstandingReason), '') 
		,o.OutstandingRemark
        ,o.EstPODD
		,SMP = IIF(o.ScanAndPack = 1,'Y','')
		,VasShas = IIF(o.VasShas = 1,'Y','') 
		,Handle = o.MRHandle+' - '+isnull((select Name + ' #' + ExtNo 
										   from TPEPass1 WITH (NOLOCK) 
										   where ID = o.MRHandle), '') 
		,SMR = o.SMR+' - '+isnull((select Name + ' #' + ExtNo 
								   from TPEPass1 WITH (NOLOCK) 
								   where ID = o.SMR), '')
		,LocalMR = o.LocalMR+' - '+isnull((select Name + ' #' + ExtNo 
										   from Pass1 WITH (NOLOCK) 
										   where ID = o.LocalMR), '')
        ,[Carton Qty at C-Log=Pack Qty] = '=IF(INDEX(W:W,ROW()) = INDEX(AB:AB,ROW()), ""True"", """")'
		,[ReturnedQtyBySeq] = [dbo].getInvAdjQty(o.ID,oq.Seq)
		,[HC] = pkExpressID.ExpressID
		,[HCStatus] = pkExpressStatus.ExpressStatus
from Orders o WITH (NOLOCK) 
inner join Factory f with (nolock) on o.FactoryID = f.ID and f.IsProduceFty=1
inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.Id
left join OrderType ot WITH (NOLOCK) on ot.BrandID = o.BrandID and ot.id = o.OrderTypeID
left join Country c WITH (NOLOCK) on o.Dest = c.ID
left join Brand b WITH (NOLOCK) on o.BrandID=b.id
outer apply(
	select pkid = stuff((
		select concat(',',a.id)
		from(
			select distinct pd.id
			from packinglist_detail pd
            inner join packinglist p on p.id = pd.id
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq {this.whereExcludePulloutOuterApply}
		)a
		order by a.id
		for xml path('')
	),1,1,'')
)pkid
outer apply(
	select v = stuff((
		select concat(',',a.Status)
		from(
			select distinct pd.id,p.Status
			from packinglist_detail pd
            inner join packinglist p on p.id = pd.id
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq {this.whereExcludePulloutOuterApply}
		)a
		order by a.id
		for xml path('')
	),1,1,'')
)pkstatus
outer apply(
	select pkINVNo = stuff((
		select concat(',',a.INVNo)
		from(
			select distinct p.INVNo,p.id
			from packinglist_detail pd
			inner join PackingList p on p.id = pd.id
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq {this.whereExcludePulloutOuterApply}
		)a
		order by a.id
		for xml path('')
	),1,1,'')
)pkINVNo
outer apply(
	select FCRDate = stuff((
		select concat(',',a.FCRDate)
		from(
			select distinct gb.FCRDate,p.id
			from packinglist_detail pd
			inner join PackingList p on p.id = pd.id
			inner join GMTBooking gb on gb.id = p.INVNo
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq {this.whereExcludePulloutOuterApply}
            
		)a
		order by a.id
		for xml path('')
	),1,1,'')
)gb
outer apply(
	select CYCFS = stuff((
		select concat(',',a.CYCFS)
		from(
			select distinct gb.CYCFS,p.id
			from packinglist_detail pd
			inner join PackingList p on p.id = pd.id
			inner join GMTBooking gb on gb.id = p.INVNo
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq {this.whereExcludePulloutOuterApply}
            
		)a
		order by a.id
		for xml path('')
	),1,1,'')
)gbCYCFS
outer apply(
	select top 1 p.CustCDID
	from packinglist_detail pd
	inner join PackingList p on p.id = pd.id
	where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq {this.whereExcludePulloutOuterApply}
)CustCDID
outer apply(
	select CTNQty=sum(pd.CTNQty),GW=sum(pd.GW),ShipQty=sum(pd.ShipQty),VM = sum(APPEstAmtVW)
	from packinglist_detail pd
	inner join PackingList p on p.id = pd.id
	where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq {this.whereExcludePulloutOuterApply}
)plds
outer apply(
	select CTNQty=round(sum(l.CBM),4)
	from packinglist_detail pd
    inner join PackingList p on p.id = pd.id
	inner join LocalItem l on l.refno = pd.refno
	where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq {this.whereExcludePulloutOuterApply}
    and pd.CTNQty > 0
)cbm

outer apply(
	select PulloutDate = stuff((
		select concat(',',a.PulloutDate)
		from(
			select distinct p.PulloutDate,p.id
			from packinglist_detail pd
			inner join PackingList p on p.id = pd.id {this.whereExcludePulloutOuterApply}
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq
		)a
		order by a.id
		for xml path('')
	),1,1,'')
)pkPulloutDate
outer apply(
	select PulloutID = stuff((
		select concat(',',PulloutID)
		from(
			select distinct p.PulloutID
			from packinglist_detail pd
			inner join PackingList p on p.id = pd.id
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq {this.whereExcludePulloutOuterApply}
		)a
		order by a.PulloutID
		for xml path('')
	),1,1,'')
)pkPulloutID
left join
(
	select distinct gb.FCRDate,pd.orderid, pd.OrderShipmodeSeq,p.ShipPlanID,gb.SONo,gb.SOCFMDate,gb.CutOffDate
	from packinglist_detail pd
	inner join PackingList p on p.id = pd.id {this.whereExcludePulloutOuterApply}
	inner join GMTBooking gb on gb.id = p.INVNo 
)gb2 on  gb2.orderid = o.id and gb2.OrderShipmodeSeq = oq.seq
outer apply(
	select ExpressID = stuff((
		select concat(',',a.ExpressID)
		from(
			select distinct p.ExpressID,p.id
			from packinglist_detail pd
			inner join PackingList p on p.id = pd.id
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq {this.whereExcludePulloutOuterApply}
		)a
		order by a.id
		for xml path('')
	),1,1,'')
)pkExpressID
outer apply(
	select ExpressStatus = stuff((
		select concat(',',a.Status)
		from(
			select distinct e.Status,p.ID
			from packinglist_detail pd
			inner join PackingList p on p.id = pd.id
			inner join Express e on p.ExpressID = e.ID
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq {this.whereExcludePulloutOuterApply}
		)a
		order by a.id
		for xml path('')
	),1,1,'')
)pkExpressStatus
where ((isnull(ot.IsGMTMaster,0) != 1
	and o.PulloutComplete=0 and o.Qty > 0
	and not exists (select 1 from Order_Finish Where ID = o.ID))
or ( 
	exists (select 1 from Order_Finish Where ID = o.ID)
	and (select FOCQty from Order_Finish Where ID = o.ID) < (select dbo.GetFocStockByOrder(o.ID))
))
and exists (select 1 from Factory f where o.FactoryId = id and f.IsProduceFty = 1)
{this.whereExcludePullout}
");

            return sqlCmd;
        }

        private StringBuilder SummaryBySP2(StringBuilder sqlCmd)
        {
            sqlCmd.Append(@"
union

select 	oq.BuyerDelivery
		,oq.IDD
		,o.BrandID
		,b.BuyerID
		,o.ID
        ,[Cancel] = IIF(o.Junk=1,'Y','N')
		,Category = IIF(o.Category = 'B', 'Bulk'
										, 'Sample')
        ,oq.seq
        ,[If Partial] = (select iif(count(1) > 1, 'Y', '') from Order_QtyShip with (nolock) where ID = o.ID)
		,NULL
		,NULL
		,NULL
		,NULL
		,NULL
		,NULL
		,o.CustPONo
		,o.StyleID
		,o.SeasonID
		,oq.Qty
		,ShipQty = isnull(ShipQty.ShipQty,0)
        ,FOCBalQty = isnull(dbo.GetFocStockByOrder(o.ID),0)
		,OrderTtlQty=o.Qty
		,NULL
		,NULL
		,NULL
		,NULL
		,NULL
		,NULL
        ,[Carton Qty at C-Log] = isnull(o.ClogCTN, 0)
        ,[SP Prod. Output Qty] = [dbo].[getMinCompleteSewQty](o.ID, null, null)
		,o.MDivisionID
		,o.ftygroup
        ,[KPI Factory] = f.Kpicode
        ,o.CustCDID
		,Alias = isnull(c.Alias,'')
        ,o.OrderTypeID
        ,[On Site] = iif(o.OnSiteSample = 1, 'Y', '')
        ,[BuyBack] = iif(exists(select 1 from Order_BuyBack with (nolock) where ID = o.ID), 'Y', '')
		,Payment = isnull((select Term 
						   from PayTermAR WITH (NOLOCK) 
						   where ID = o.PayTermARID), '')
		,o.PoPrice
		,o.Customize1
		,o.Customize2
		,NULL
		,NULL
		,oq.ShipmodeID
		,NULL
        ,OSReason = o.OutstandingReason + ' - ' + isnull((select Name 
														   from Reason WITH (NOLOCK) 
														   where ReasonTypeID = 'Delivery_OutStand' and Id = o.OutstandingReason), '') 
		,o.OutstandingRemark
        ,o.EstPODD
		,SMP = IIF(o.ScanAndPack = 1,'Y','')
		,VasShas = IIF(o.VasShas = 1,'Y','') 
		,Handle = o.MRHandle+' - '+isnull((select Name + ' #' + ExtNo 
										   from TPEPass1 WITH (NOLOCK) 
										   where ID = o.MRHandle), '') 
		,SMR = o.SMR+' - '+isnull((select Name + ' #' + ExtNo 
								   from TPEPass1 WITH (NOLOCK) 
								   where ID = o.SMR), '')
		,LocalMR = o.LocalMR+' - '+isnull((select Name + ' #' + ExtNo 
										   from Pass1 WITH (NOLOCK) 
										   where ID = o.LocalMR), '')
        ,[Carton Qty at C-Log=Pack Qty] = '=IF(INDEX(W:W,ROW()) = INDEX(AB:AB,ROW()), ""True"", """")'
		,[ReturnedQtyBySeq] = [dbo].getInvAdjQty(o.ID, oq.Seq)
		,NULL
		,NULL

from Orders o WITH(NOLOCK)
inner join Factory f with(nolock) on o.FactoryID = f.ID and f.IsProduceFty= 1
inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.Id
left join OrderType ot WITH (NOLOCK) on ot.BrandID = o.BrandID and ot.id = o.OrderTypeID
left join Country c WITH (NOLOCK) on o.Dest = c.ID
left join Brand b WITH (NOLOCK) on o.BrandID= b.id
outer apply(select ShipQty = isnull(sum(ShipQty), 0) from Pullout_Detail WITH (NOLOCK) where OrderID = o.ID and OrderShipmodeSeq = oq.Seq) ShipQty

where 1=1 and isnull(ot.IsGMTMaster,0) != 1
and o.PulloutComplete=0
and o.Qty > 0
and isnull(oq.Qty,0) - isnull(ShipQty.ShipQty,0) > 0
and exists (select 1 from Factory f where o.FactoryId = id and f.IsProduceFty = 1)
");

            return sqlCmd;
        }

        private StringBuilder SummaryByPL(StringBuilder sqlCmd)
        {
            sqlCmd.Append(
$@"select oq.BuyerDelivery
		,oq.IDD
		,o.BrandID
		,b.BuyerID
		,o.ID
        ,[Cancel] = IIF(o.Junk=1,'Y','N')
		,Category = IIF(o.Category = 'B', 'Bulk'
										, 'Sample')
        ,oq.seq
        ,[If Partial] = (select iif(count(1) > 1, 'Y', '') from Order_QtyShip with (nolock) where ID = o.ID)
		,p.ID
        ,p.Status
		,p.INVNo
		,gb.FCRDate
		,p.PulloutDate
        ,p.PulloutID
		,o.CustPONo
		,o.StyleID
		,o.SeasonID
		,oq.Qty
		,ShipQty = (select isnull(sum(ShipQty), 0) 
					from Pullout_Detail WITH (NOLOCK) 
					where OrderID = o.ID and OrderShipmodeSeq = oq.Seq)
        ,FOCBalQty = isnull(dbo.GetFocStockByOrder(o.ID),0)
		,OrderTtlQty=o.Qty
		,ShipTtlQty=isnull(plds.ShipQty,0)
        ,plds.CTNQty
        ,gb.SONo
        ,gb.SOCFMDate
        ,gb.CutOffDate
		,p.ShipPlanID
        ,[Carton Qty at C-Log] = isnull(o.ClogCTN, 0)
        ,[SP Prod. Output Qty] = [dbo].[getMinCompleteSewQty](o.ID, null, null)
		,o.MDivisionID
		,o.ftygroup
        ,[KPI Factory] = f.Kpicode
        ,o.CustCDID
		,Alias = isnull(c.Alias,'')
        ,o.OrderTypeID
        ,[On Site] = iif(o.OnSiteSample = 1, 'Y', '')
        ,[BuyBack] = iif(exists(select 1 from Order_BuyBack with (nolock) where ID = o.ID), 'Y', '')
		,Payment = isnull((select Term 
						   from PayTermAR WITH (NOLOCK) 
						   where ID = o.PayTermARID), '')
		,o.PoPrice
		,o.Customize1
		,o.Customize2
		,plds.GW
        ,plds.VM
		,cbm.CTNQty
		,oq.ShipmodeID
        ,[Loading Type] = gb.CYCFS
        ,OSReason = o.OutstandingReason + ' - ' + isnull((select Name 
														   from Reason WITH (NOLOCK) 
														   where ReasonTypeID = 'Delivery_OutStand' and Id = o.OutstandingReason), '') 
		,o.OutstandingRemark
        ,o.EstPODD
		,SMP = IIF(o.ScanAndPack = 1,'Y','')
		,VasShas = IIF(o.VasShas = 1,'Y','') 
		,Handle = o.MRHandle+' - '+isnull((select Name + ' #' + ExtNo 
										   from TPEPass1 WITH (NOLOCK) 
										   where ID = o.MRHandle), '') 
		,SMR = o.SMR+' - '+isnull((select Name + ' #' + ExtNo 
								   from TPEPass1 WITH (NOLOCK) 
								   where ID = o.SMR), '')
		,LocalMR = o.LocalMR+' - '+isnull((select Name + ' #' + ExtNo 
										   from Pass1 WITH (NOLOCK) 
										   where ID = o.LocalMR), '')
        ,[Carton Qty at C-Log=Pack Qty] = '=IF(INDEX(W:W,ROW()) = INDEX(AB:AB,ROW()), ""True"", """")'
		,[ReturnedQtyBySeq] = [dbo].getInvAdjQty(o.ID,oq.Seq)
		,[HC] = p.ExpressID
		,[HCStatus] = p.ExpressStatus
from Orders o WITH (NOLOCK) 
inner join Factory f with (nolock) on o.FactoryID = f.ID and f.IsProduceFty=1
inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.Id
left join OrderType ot WITH (NOLOCK) on ot.BrandID = o.BrandID and ot.id = o.OrderTypeID
left join Country c WITH (NOLOCK) on o.Dest = c.ID
left join Brand b WITH (NOLOCK) on o.BrandID=b.id
outer apply(
	select distinct p.ID, p.Status, p.PulloutDate, p.PulloutID, p.INVNo ,p.ExpressID, [ExpressStatus] = e.Status, p.ShipPlanID
	from PackingList_Detail pd WITH (NOLOCK)
	inner join PackingList p WITH (NOLOCK) on p.ID = pd.ID
	left join Express e on p.ExpressID = e.ID
	where pd.OrderID = o.ID and pd.OrderShipmodeSeq = oq.Seq {this.whereExcludePulloutOuterApply}
)p
outer apply(
	select CTNQty=sum(pd.CTNQty),GW=sum(pd.GW),ShipQty=sum(pd.ShipQty),VM=sum(APPEstAmtVW)
	from packinglist_detail pd
	inner join PackingList p on p.id = pd.id
	where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq {this.whereExcludePulloutOuterApply}
)plds
outer apply(
	select CTNQty=round(sum(l.CBM),4)
	from packinglist_detail pd
    inner join PackingList p on p.id = pd.id
	inner join LocalItem l on l.refno = pd.refno
	where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq {this.whereExcludePulloutOuterApply}
    and pd.CTNQty > 0
)cbm
outer apply
(
	select distinct gb.FCRDate,gb.SONo,gb.SOCFMDate,gb.CutOffDate,gb.CYCFS
	from GMTBooking gb WITH (NOLOCK)
	where gb.id = p.INVNo
)gb 
where ((isnull(ot.IsGMTMaster,0) != 1
	and o.PulloutComplete=0 and o.Qty > 0
	and not exists (select 1 from Order_Finish Where ID = o.ID))
or ( 
	exists (select 1 from Order_Finish Where ID = o.ID)
	and (select FOCQty from Order_Finish Where ID = o.ID) < (select dbo.GetFocStockByOrder(o.ID))
))
and exists (select 1 from Factory f where o.FactoryId = id and f.IsProduceFty = 1)
{this.whereExcludePullout}
");

            return sqlCmd;
        }

        private StringBuilder SummaryByPL2(StringBuilder sqlCmd)
        {
            sqlCmd.Append($@"
union

select oq.BuyerDelivery
        , oq.IDD
        , o.BrandID
        , b.BuyerID
        , o.ID
        ,[Cancel] = IIF(o.Junk = 1, 'Y', 'N')
        , Category = IIF(o.Category = 'B', 'Bulk'
                                        , 'Sample')
        , oq.seq
        ,[If Partial] = (select iif(count(1) > 1, 'Y', '') from Order_QtyShip with (nolock) where ID = o.ID)
		,NULL
		,NULL
		,NULL
		,NULL
		,NULL
        ,NULL
		,o.CustPONo
		,o.StyleID
		,o.SeasonID
		,oq.Qty
		,ShipQty.ShipQty
        ,FOCBalQty = isnull(dbo.GetFocStockByOrder(o.ID),0)
		,OrderTtlQty = o.Qty
		,ShipTtlQty=isnull(plds.ShipQty,0)
        ,plds.CTNQty
		,NULL
		,NULL
		,NULL
		,NULL
        ,[Carton Qty at C-Log] = isnull(o.ClogCTN, 0)
        ,[SP Prod.Output Qty] = [dbo].[getMinCompleteSewQty] (o.ID, null, null)
		,o.MDivisionID
		,o.ftygroup
        ,[KPI Factory] = f.Kpicode
        ,o.CustCDID
		,Alias = isnull(c.Alias,'')
        ,o.OrderTypeID
        ,[On Site] = iif(o.OnSiteSample = 1, 'Y', '')
        ,[BuyBack] = iif(exists(select 1 from Order_BuyBack with (nolock) where ID = o.ID), 'Y', '')
		,Payment = isnull((select Term

                           from PayTermAR WITH (NOLOCK)
                           where ID = o.PayTermARID), '')
		,o.PoPrice
		,o.Customize1
		,o.Customize2
		,NULL
		,NULL
		,NULL
		,oq.ShipmodeID
		,NULL
        ,OSReason = o.OutstandingReason + ' - ' + isnull((select Name

                                                           from Reason WITH (NOLOCK)
                                                           where ReasonTypeID = 'Delivery_OutStand' and Id = o.OutstandingReason), '') 
		,o.OutstandingRemark
        ,o.EstPODD
		,SMP = IIF(o.ScanAndPack = 1,'Y','')
		,VasShas = IIF(o.VasShas = 1,'Y','')
		,Handle = o.MRHandle+' - '+isnull((select Name + ' #' + ExtNo
                                           from TPEPass1 WITH (NOLOCK)
                                           where ID = o.MRHandle), '') 
		,SMR = o.SMR+' - '+isnull((select Name + ' #' + ExtNo
                                   from TPEPass1 WITH (NOLOCK)
                                   where ID = o.SMR), '')
		,LocalMR = o.LocalMR+' - '+isnull((select Name + ' #' + ExtNo
                                           from Pass1 WITH (NOLOCK)
                                           where ID = o.LocalMR), '')
        ,[Carton Qty at C-Log=Pack Qty] = '=IF(INDEX(W:W,ROW()) = INDEX(AB:AB,ROW()), ""True"", """")'
		,[ReturnedQtyBySeq] = [dbo].getInvAdjQty(o.ID, oq.Seq)
		,NULL
		,NULL
from Orders o WITH(NOLOCK)
inner join Factory f with(nolock) on o.FactoryID = f.ID and f.IsProduceFty= 1
inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.Id
left join OrderType ot WITH (NOLOCK) on ot.BrandID = o.BrandID and ot.id = o.OrderTypeID
left join Country c WITH (NOLOCK) on o.Dest = c.ID
left join Brand b WITH (NOLOCK) on o.BrandID= b.id
outer apply(select ShipQty = isnull(sum(ShipQty), 0) from Pullout_Detail WITH (NOLOCK) where OrderID = o.ID and OrderShipmodeSeq = oq.Seq) ShipQty
outer apply(
	select CTNQty=sum(pd.CTNQty),GW=sum(pd.GW),ShipQty=sum(pd.ShipQty),VM=sum(APPEstAmtVW)
	from packinglist_detail pd
	inner join PackingList p on p.id = pd.id
	where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq {this.whereExcludePulloutOuterApply}
)plds
outer apply(
	select distinct p.ID, p.Status, p.PulloutDate, p.INVNo ,p.ExpressID, [ExpressStatus] = e.Status, p.ShipPlanID
	from PackingList_Detail pd WITH (NOLOCK)
	inner join PackingList p WITH (NOLOCK) on p.ID = pd.ID
	left join Express e on p.ExpressID = e.ID
	where pd.OrderID = o.ID and pd.OrderShipmodeSeq = oq.Seq {this.whereExcludePulloutOuterApply}
)p


where ((isnull(ot.IsGMTMaster,0) != 1
	and o.PulloutComplete=0 and o.Qty > 0
	and isnull(oq.Qty,0) - isnull(ShipQty.ShipQty,0) > 0
	and not exists (select 1 from Order_Finish Where ID = o.ID))
or ( 
	exists (select 1 from Order_Finish Where ID = o.ID)
	and (select FOCQty from Order_Finish Where ID = o.ID) < (select dbo.GetFocStockByOrder(o.ID))
))
AND p.PulloutDate IS NOT NULL
and exists (select 1 from Factory f where o.FactoryId = id and f.IsProduceFty = 1)
{this.whereExcludePullout}
 ");

            return sqlCmd;
        }

        private StringBuilder SQLWhere(StringBuilder sqlCmd)
        {
            if (!MyUtility.Check.Empty(this.fCRDate1))
            {
                sqlCmd.Append(string.Format(" and gb2.FCRDate >= '{0}' ", Convert.ToDateTime(this.fCRDate1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.fCRDate2))
            {
                sqlCmd.Append(string.Format(" and gb2.FCRDate <= '{0}' ", Convert.ToDateTime(this.fCRDate2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.buyerDlv1))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery >= '{0}' ", Convert.ToDateTime(this.buyerDlv1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.buyerDlv2))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery <= '{0}' ", Convert.ToDateTime(this.buyerDlv2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.estPullout1))
            {
                sqlCmd.Append(string.Format(" and oq.EstPulloutDate between '{0}' and '{1}'", Convert.ToDateTime(this.estPullout1).ToString("yyyy/MM/dd"), Convert.ToDateTime(this.estPullout2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", this.brand));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlCmd.Append(string.Format(" and o.MDivisionID = '{0}'", this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.orderNo))
            {
                sqlCmd.Append(string.Format(" and o.Customize1 = '{0}'", this.orderNo));
            }

            if (!MyUtility.Check.Empty(this.buyer))
            {
                sqlCmd.Append(string.Format(" and b.BuyerID = '{0}'", this.buyer));
            }

            if (!MyUtility.Check.Empty(this.custCD))
            {
                sqlCmd.Append(string.Format(" and o.CustCDID = '{0}'", this.custCD));
            }

            if (!this.chkIncludeCancelOrder.Checked)
            {
                sqlCmd.Append($" and o.Junk = 0");
            }

            if (!MyUtility.Check.Empty(this.destination))
            {
                sqlCmd.Append(string.Format(" and o.Dest = '{0}'", this.destination));
            }

            sqlCmd.Append($" and o.Category in ({this.category})");

            if (!this.includeLO)
            {
                sqlCmd.Append(" and o.LocalOrder = 0");
            }

            return sqlCmd;
        }

        private void ComboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ChkExcludePulloutStatusChange();
        }

        private void ComboSummaryBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ChkExcludePulloutStatusChange();
        }

        private void ChkExcludePulloutStatusChange()
        {
            if (
                ((this.comboCategory.Text == "Bulk" || this.comboCategory.Text == "Garment" || this.comboCategory.Text == "Bulk+Garment") && this.comboSummaryBy.Text == "SP# and Seq") ||
                (this.comboCategory.Text == "Sample" && this.comboSummaryBy.Text == "PL#"))
            {
                this.chkExcludePullout.ReadOnly = false;
            }
            else
            {
                this.chkExcludePullout.ReadOnly = true;
                this.chkExcludePullout.Checked = false;
            }
        }
    }
}
