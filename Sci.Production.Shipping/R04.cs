using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R04
    /// </summary>
    public partial class R04 : Sci.Win.Tems.PrintForm
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
        private bool includeLO;
        private DataTable printData;

        /// <summary>
        /// R04
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboM.Text = Sci.Env.User.Keyword;
            this.comboFactory.SelectedIndex = -1;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            // if (MyUtility.Check.Empty(dateRange1.Value1))
            // {
            //    MyUtility.Msg.WarningBox("Buyer Delivery can't empty!!");
            //    return false;
            // }
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

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string whereFCRDate = string.Empty;
            string whereFCRDateOut = string.Empty;
            if (!MyUtility.Check.Empty(this.fCRDate1))
            {
                whereFCRDate += string.Format(" and gb.FCRDate >= '{0}' ", Convert.ToDateTime(this.fCRDate1).ToString("d"));
                whereFCRDateOut += string.Format(" and gb2.FCRDate >= '{0}' ", Convert.ToDateTime(this.fCRDate1).ToString("d"));
            }

            if (!MyUtility.Check.Empty(this.fCRDate2))
            {
                whereFCRDate += string.Format(" and gb.FCRDate <= '{0}' ", Convert.ToDateTime(this.fCRDate2).ToString("d"));
                whereFCRDateOut += string.Format(" and gb2.FCRDate <= '{0}' ", Convert.ToDateTime(this.fCRDate2).ToString("d"));
            }

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(
@"select 	oq.BuyerDelivery
		,oq.EstPulloutDate
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
		,o.CustPONo
		,o.StyleID
		,o.SeasonID
		,oq.Qty
		,ShipQty = (select isnull(sum(ShipQty), 0) 
					from Pullout_Detail WITH (NOLOCK) 
					where OrderID = o.ID and OrderShipmodeSeq = oq.Seq) - [dbo].getInvAdjQty(o.ID,oq.Seq) 
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
        ,plds.CTNQty
		,plds.GW
		,cbm.CTNQty
		,oq.ShipmodeID
        ,[Loading Type] = gbCYCFS.CYCFS
        ,OSReason = oq.OutstandingReason + ' - ' + isnull((select Name 
														   from Reason WITH (NOLOCK) 
														   where ReasonTypeID = 'Delivery_OutStand' and Id = oq.OutstandingReason), '') 
		,oq.OutstandingRemark
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
        ,[Carton Qty at C-Log=Pack Qty] = '=IF(INDEX(V:V,ROW()) = INDEX(AA:AA,ROW()), ""True"", """")'
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
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq
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
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq
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
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq
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
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq
            
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
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq
            
		)a
		order by a.id
		for xml path('')
	),1,1,'')
)gbCYCFS
outer apply(
	select top 1 p.CustCDID
	from packinglist_detail pd
	inner join PackingList p on p.id = pd.id
	where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq
)CustCDID
outer apply(
	select CTNQty=sum(pd.CTNQty),GW=sum(pd.GW),ShipQty=sum(pd.ShipQty)
	from packinglist_detail pd
	inner join PackingList p on p.id = pd.id
	where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq
)plds
outer apply(
	select CTNQty=round(sum(l.CBM),4)
	from packinglist_detail pd
	inner join LocalItem l on l.refno = pd.refno
	where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq
    and pd.CTNQty > 0
)cbm

outer apply(
	select PulloutDate = stuff((
		select concat(',',a.PulloutDate)
		from(
			select distinct p.PulloutDate,p.id
			from packinglist_detail pd
			inner join PackingList p on p.id = pd.id
			where pd.orderid = o.id and pd.OrderShipmodeSeq = oq.seq
		)a
		order by a.id
		for xml path('')
	),1,1,'')
)pkPulloutDate
left join
(
	select distinct gb.FCRDate,pd.orderid, pd.OrderShipmodeSeq,p.ShipPlanID,gb.SONo,gb.SOCFMDate,gb.CutOffDate
	from packinglist_detail pd
	inner join PackingList p on p.id = pd.id
	inner join GMTBooking gb on gb.id = p.INVNo 
)gb2 on  gb2.orderid = o.id and gb2.OrderShipmodeSeq = oq.seq
where 1=1 and isnull(ot.IsGMTMaster,0) != 1

AND oq.Qty <>( (select isnull(sum(ShipQty), 0) from Pullout_Detail WITH (NOLOCK) where OrderID = o.ID and OrderShipmodeSeq = oq.Seq) 
				- [dbo].getInvAdjQty(o.ID,oq.Seq) )

and o.PulloutComplete=0 and o.Qty > 0", whereFCRDate));

            if (!MyUtility.Check.Empty(this.buyerDlv1))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery >= '{0}' ", Convert.ToDateTime(this.buyerDlv1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.buyerDlv2))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery <= '{0}' ", Convert.ToDateTime(this.buyerDlv2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.estPullout1))
            {
                sqlCmd.Append(string.Format(" and oq.EstPulloutDate between '{0}' and '{1}'", Convert.ToDateTime(this.estPullout1).ToString("d"), Convert.ToDateTime(this.estPullout2).ToString("d")));
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

            if (!MyUtility.Check.Empty(whereFCRDateOut))
            {
                sqlCmd.Append(whereFCRDateOut);
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

            sqlCmd.Append(" order by oq.BuyerDelivery,o.ID,oq.seq");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
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
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_R04_EstimateOutstandingShipmentReport.xltx";
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
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Shipping_R04_EstimateOutstandingShipmentReport");
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
                this.txtcountryDestination.TextBox1.Text = MyUtility.GetValue.Lookup(string.Format("SELECT CountryID FROM CustCD WITH (NOLOCK) WHERE BrandID = '{0}' AND ID = '{1}'", this.txtbrand.Text , this.txtcustcd.Text));
            }
        }
    }
}
