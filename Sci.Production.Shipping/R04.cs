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
            this.destination = this.txtcountryDestination.Text;
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
		,Category = IIF(o.Category = 'B', 'Bulk'
										, 'Sample')
        ,oq.seq
		,pkid.pkid
		,pkINVNo.pkINVNo
		,gb.FCRDate
		,pkPulloutDate.PulloutDate
		,o.CustPONo
		,o.StyleID
		,o.SeasonID
		,oq.Qty
		,o.MDivisionID
		,o.FactoryID
		,Alias = isnull(c.Alias,'')
		,o.PoPrice
		,o.Customize1
		,o.Customize2
		,oq.ShipmodeID
		,SMP = IIF(o.ScanAndPack = 1,'Y','')
		,VasShas = IIF(o.VasShas = 1,'Y','') 
		,ShipQty = (select isnull(sum(ShipQty), 0) 
					from Pullout_Detail WITH (NOLOCK) 
					where OrderID = o.ID and OrderShipmodeSeq = oq.Seq) - [dbo].getInvAdjQty(o.ID,oq.Seq) 
		,Payment = isnull((select Term 
						   from PayTermAR WITH (NOLOCK) 
						   where ID = o.PayTermARID), '')
		,Handle = o.MRHandle+' - '+isnull((select Name + ' #' + ExtNo 
										   from TPEPass1 WITH (NOLOCK) 
										   where ID = o.MRHandle), '') 
		,SMR = o.SMR+' - '+isnull((select Name + ' #' + ExtNo 
								   from TPEPass1 WITH (NOLOCK) 
								   where ID = o.SMR), '')
		,LocalMR = o.LocalMR+' - '+isnull((select Name + ' #' + ExtNo 
										   from Pass1 WITH (NOLOCK) 
										   where ID = o.LocalMR), '')
		,OSReason = oq.OutstandingReason + ' - ' + isnull((select Name 
														   from Reason WITH (NOLOCK) 
														   where ReasonTypeID = 'Delivery_OutStand' and Id = oq.OutstandingReason), '') 
		,oq.OutstandingRemark
from Orders o WITH (NOLOCK) 
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
            {0}
		)a
		order by a.id
		for xml path('')
	),1,1,'')
)gb
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
	select distinct gb.FCRDate,pd.orderid, pd.OrderShipmodeSeq
	from packinglist_detail pd
	inner join PackingList p on p.id = pd.id
	inner join GMTBooking gb on gb.id = p.INVNo 
)gb2 on  gb2.orderid = o.id and gb2.OrderShipmodeSeq = oq.seq
where 1=1 and isnull(ot.IsGMTMaster,0) != 1
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

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            // 填內容值
            int intRowsStart = 2;
            object[,] objArray = new object[1, 31];
            foreach (DataRow dr in this.printData.Rows)
            {
                objArray[0, 0] = dr["BuyerDelivery"];
                objArray[0, 1] = dr["EstPulloutDate"];
                objArray[0, 2] = dr["BrandID"];
                objArray[0, 3] = dr["BuyerID"];
                objArray[0, 4] = dr["ID"];
                objArray[0, 5] = dr["Category"];
                objArray[0, 6] = dr["seq"];
                objArray[0, 7] = dr["pkid"];
                objArray[0, 8] = dr["pkINVNo"];
                objArray[0, 9] = dr["FCRDate"];
                objArray[0, 10] = dr["PulloutDate"];
                objArray[0, 11] = dr["CustPONo"];
                objArray[0, 12] = dr["StyleID"];
                objArray[0, 13] = dr["SeasonID"];
                objArray[0, 14] = dr["Qty"];
                objArray[0, 15] = dr["ShipQty"];
                objArray[0, 16] = dr["MDivisionID"];
                objArray[0, 17] = dr["FactoryID"];
                objArray[0, 18] = dr["Alias"];
                objArray[0, 19] = dr["Payment"];
                objArray[0, 20] = dr["PoPrice"];
                objArray[0, 21] = dr["Customize1"];
                objArray[0, 22] = dr["Customize2"];
                objArray[0, 23] = dr["ShipmodeID"];
                objArray[0, 24] = dr["SMP"];
                objArray[0, 25] = dr["VasShas"];
                objArray[0, 26] = dr["Handle"];
                objArray[0, 27] = dr["SMR"];
                objArray[0, 28] = dr["LocalMR"];
                objArray[0, 29] = dr["OSReason"];
                objArray[0, 30] = dr["OutstandingRemark"];
                worksheet.Range[string.Format("A{0}:AE{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

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
    }
}
