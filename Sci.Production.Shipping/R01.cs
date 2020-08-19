using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R01
    /// </summary>
    public partial class R01 : Win.Tems.PrintForm
    {
        private string shipper;
        private string brand;
        private string shipmode;
        private string shipterm;
        private string dest;
        private string status;
        private string reportType;
        private DateTime? invdate1;
        private DateTime? invdate2;
        private DateTime? etd1;
        private DateTime? etd2;
        private DateTime? FCRDate1;
        private DateTime? CutOffDate1;
        private DateTime? SOCFMDate1;
        private DateTime? FCRDate2;
        private DateTime? CutOffDate2;
        private DateTime? SOCFMDate2;
        private DateTime? Delivery1;
        private DateTime? Delivery2;
        private DataTable printData;
        private bool hasDelivery;

        /// <summary>
        /// R01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboShipper, 1, factory);
            this.comboShipper.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(this.comboStatus, 1, 1, "All,Confirmed,UnConfirmed");
            this.comboStatus.SelectedIndex = 0;
            this.txtshipmodeShippingMode.SelectedIndex = -1;
            this.radioMainList.Checked = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            // if (MyUtility.Check.Empty(dateRange1.Value1))
            // {
            //    MyUtility.Msg.WarningBox("Invoice Date can't empty!!");
            //    return false;
            // }
            this.hasDelivery = false;
            this.shipper = this.comboShipper.Text;
            this.brand = this.txtbrand.Text;
            this.shipmode = this.txtshipmodeShippingMode.Text;
            this.shipterm = this.txtshiptermShipmentTerm.Text;
            this.dest = this.txtcountryDestination.TextBox1.Text;
            this.status = this.comboStatus.Text;
            this.reportType = this.radioMainList.Checked ? "1" : "2";
            this.invdate1 = this.dateInvoiceDate.Value1;
            this.invdate2 = this.dateInvoiceDate.Value2;
            this.etd1 = this.dateETD.Value1;
            this.etd2 = this.dateETD.Value2;

            this.FCRDate1 = this.dateFCR.Value1;
            this.CutOffDate1 = this.dateCutoff.Value1;
            this.SOCFMDate1 = this.dateConfirm.Value1;

            this.FCRDate2 = this.dateFCR.Value2;
            this.CutOffDate2 = this.dateCutoff.Value2;
            this.SOCFMDate2 = this.dateConfirm.Value2;

            this.Delivery1 = this.dateDelivery.Value1;
            this.Delivery2 = this.dateDelivery.Value2;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            StringBuilder sqlCmd_where = new StringBuilder();

            if (this.reportType == "1")
            {
                sqlCmd.Append(string.Format(@"select 
g.ID
,g.Shipper
,g.BrandID
,g.InvDate
,g.FCRDate
,g.CustCDID
,(g.Dest+' - '+isnull(c.Alias,'')) as Dest
,g.ShipModeID
,g.CYCFS
,g.ShipTermID
,[Handle] = dbo.getPass1(g.Handle)
,[Forwarder] = (g.Forwarder+' - '+isnull(ls.Abb,''))
,g.Vessel
,g.ETD
,g.ETA
,g.SONo
,g.SOCFMDate
,g.CutOffDate
,g.ShipPlanID
,sp.Status
,g.TotalShipQty
,g.TotalCTNQty
,isnull((select CTNRNo+'/'+TruckNo+',' from GMTBooking_CTNR WITH (NOLOCK) where ID = g.ID for xml path('')),'') as CTNTruck
,g.TotalGW
,g.TotalCBM
,g.AddDate
,IIF(g.Status = 'Confirmed',g.EditDate,null) as ConfirmDate
,g.Remark
,[PulloutComplete]=IIF(PulloutIdCount.Value = PulloutIdConfirmLockCount.Value AND PulloutIdCount.Value > 0 ,'True' ,'False')

from GMTBooking g WITH (NOLOCK) 
left join Country c WITH (NOLOCK) on c.ID = g.Dest
left join LocalSupp ls WITH (NOLOCK) on ls.ID = g.Forwarder
left join ShipPlan sp WITH (NOLOCK) on sp.ID = g.ShipPlanID
OUTER APPLY(
	SELECT [Value]=Count(ID)
	 FROM
	 (
		SELECT DISTINCT po.iD
		FROM PackingList pl
		INNER JOIN Pullout po ON pl.PulloutID=po.ID
		WHERE INVNo=g.ID
	) a
)PulloutIdCount
OUTER APPLY(
	SELECT [Value]=Count(ID)
	 FROM
	 (
		SELECT DISTINCT po.iD
		FROM PackingList pl
		INNER JOIN Pullout po ON pl.PulloutID=po.ID
		WHERE INVNo=g.ID AND  (po.Status = 'Confirmed' OR po.Status = 'Locked')
	) a
)PulloutIdConfirmLockCount
where 1=1"));
            }
            else
            {
                #region 準備Tmp

                sqlCmd.Append(@"

SELECT DISTINCT [GBID]=g.ID,[PackingListID]=pl.ID,[PulloutID]=pl.PulloutID,[OrderID]=pld.OrderID
INTO #tmp1
from GMTBooking g WITH (NOLOCK) 
INNER join PackingList pl WITH (NOLOCK) on pl.INVNo = g.ID
INNER join PackingList_Detail pld WITH (NOLOCK) on pl.id = pld.id
where pl.ID<>'' and 1=1 
");

                #region Where 條件

                if (!MyUtility.Check.Empty(this.invdate1))
                {
                    sqlCmd_where.Append(string.Format(" and g.InvDate >= '{0}' ", Convert.ToDateTime(this.invdate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.invdate2))
                {
                    sqlCmd_where.Append(string.Format(" and g.InvDate <= '{0}' ", Convert.ToDateTime(this.invdate2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.shipper))
                {
                    sqlCmd_where.Append(string.Format(" and g.Shipper = '{0}'", this.shipper));
                }

                if (!MyUtility.Check.Empty(this.brand))
                {
                    sqlCmd_where.Append(string.Format(" and g.BrandID = '{0}'", this.brand));
                }

                if (!MyUtility.Check.Empty(this.shipmode))
                {
                    sqlCmd_where.Append(string.Format(" and g.BrandID = '{0}'", this.brand));
                }

                if (!MyUtility.Check.Empty(this.shipterm))
                {
                    sqlCmd_where.Append(string.Format(" and g.ShipTermID = '{0}'", this.shipterm));
                }

                if (!MyUtility.Check.Empty(this.dest))
                {
                    sqlCmd_where.Append(string.Format(" and g.Dest = '{0}'", this.dest));
                }

                if (!MyUtility.Check.Empty(this.etd1))
                {
                    sqlCmd_where.Append(string.Format(" and g.ETD >= '{0}' ", Convert.ToDateTime(this.etd1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.etd2))
                {
                    sqlCmd_where.Append(string.Format(" and g.ETD <= '{0}' ", Convert.ToDateTime(this.etd2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.FCRDate1))
                {
                    sqlCmd_where.Append(string.Format(" and g.FCRDate >= '{0}' ", Convert.ToDateTime(this.FCRDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.FCRDate2))
                {
                    sqlCmd_where.Append(string.Format(" and g.FCRDate <= '{0}' ", Convert.ToDateTime(this.FCRDate2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.CutOffDate1))
                {
                    sqlCmd_where.Append(string.Format(" and CAST( g.CutOffDate AS DATE) >= '{0}' ", Convert.ToDateTime(this.CutOffDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.CutOffDate2))
                {
                    sqlCmd_where.Append(string.Format(" and CAST( g.CutOffDate AS DATE) <= '{0}' ", Convert.ToDateTime(this.CutOffDate2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.SOCFMDate1))
                {
                    sqlCmd_where.Append(string.Format(" and g.SOCFMDate >= '{0}' ", Convert.ToDateTime(this.SOCFMDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.SOCFMDate2))
                {
                    sqlCmd_where.Append(string.Format(" and g.SOCFMDate <= '{0}' ", Convert.ToDateTime(this.SOCFMDate2).ToString("d")));
                }

                if (this.status == "Confirmed")
                {
                    sqlCmd_where.Append(" and g.Status = 'Confirmed'");
                }
                else if (this.status == "UnConfirmed")
                {
                    sqlCmd_where.Append(" and g.Status <> 'Confirmed'");
                }

                #endregion

                sqlCmd.Append(Environment.NewLine + sqlCmd_where.ToString());

                sqlCmd.Append(@"

SELECT 
t.GBID
,t.PackingListID
,t.PulloutID
,t.OrderID
,[AddDate]=CASE WHEN Pullout_Revise.AddDate IS NOT NULL THEN Pullout_Revise.AddDate
		 ELSE  (SELECT TOP 1 SendToTPE FROM Pullout WHERE ID=t.PulloutID) 
		 END
INTO #tmp2
FROm #tmp1  t
OUTER APPLY(
	SELECT TOP 1 AddDate
	FROM Pullout_Revise 
	WHERE  ID=t.PulloutID 
	AND OrderID=t.OrderID
	ORDER BY AddDate DESC
)Pullout_Revise

");

                #endregion

                sqlCmd.Append(string.Format(@"
select DISTINCT

g.ID
,g.Shipper
,g.BrandID
,g.InvDate
,pl.MDivisionID
,isnull(pl.ID,'') as PackID
,[POno]=STUFF ((select CONCAT (',',a.CustPONo) 
                            from (
                                select distinct o.CustPONo
                                from PackingList_Detail pd WITH (NOLOCK) 
								left join orders o WITH (NOLOCK) on o.id = pd.OrderID
                                where pd.ID = pl.id AND o.CustPONo<>'' AND o.CustPONo IS NOT NULL
                            ) a 
                            for xml path('')
                          ), 1, 1, '') 
,pl.PulloutDate
--
,g.CutOffDate
,[SoConfirmDate]=g.SOCFMDate
,[Terminal/Whse#]= fd.WhseNo
,g.ETD
,g.ETA
,PulloutReportConfirmDate.PulloutReportConfirmDate
,[PulloutID]=pl.PulloutID
--
,isnull(pl.ShipQty,0) as ShipQty,isnull(pl.CTNQty,0) as CTNQty
,isnull(pl.GW,0) as GW
,isnull(pl.CBM,0) as CBM
,g.CustCDID
,(g.Dest+' - '+isnull(c.Alias,'')) as Dest,IIF(g.Status = 'Confirmed',g.EditDate,null) as ConfirmDate
,g.AddName+' '+isnull(p.Name,'') as AddName
,g.AddDate
,g.Remark
,isnull((select cast(a.OrderID as nvarchar) +',' from (select distinct OrderID from PackingList_Detail pd WITH (NOLOCK) where pd.ID = pl.ID) a for xml path('')),'') as OrderID
,(select oq.BuyerDelivery from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd WITH (NOLOCK) where pd.ID = pl.ID) a
, Order_QtyShip oq WITH (NOLOCK) where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq) as BuyerDelivery
,(select oq.SDPDate from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd WITH (NOLOCK) where pd.ID = pl.ID) a, Order_QtyShip oq WITH (NOLOCK) where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq) as SDPDate
,[OrderShipmodeSeq] = 
STUFF ((
select CONCAT (',', cast (a.OrderShipmodeSeq as nvarchar)) 
    from (
        select distinct pd.OrderShipmodeSeq 
        from PackingList_Detail pd WITH (NOLOCK) 
        left join AirPP ap With (NoLock) on pd.OrderID = ap.OrderID
        and pd.OrderShipmodeSeq = ap.OrderShipmodeSeq
        where pd.ID = pl.id
        group by pd.OrderID, pd.OrderShipmodeSeq, ap.ID
    ) a 
    for xml path('')
), 1, 1, '') 
, g.SONo
, g.ShipModeID
, g.CYCFS
, g.Vessel
, g.BLNo
, g.BL2No
from GMTBooking g WITH (NOLOCK) 
left join PackingList pl WITH (NOLOCK) on pl.INVNo = g.ID
left join Country c WITH (NOLOCK) on c.ID = g.Dest
left join Pass1 p WITH (NOLOCK) on p.ID = g.AddName
LEFT JOIN ForwarderWhse_Detail fd ON g.ForwarderWhse_DetailUKey=fd.UKey
OUTER APPLY(
SELECT [PulloutReportConfirmDate]=STUFF ((
		SELECT CONCAT (',',a.AddDate) FROM (
			SELECT [AddDate]=convert(varchar, t.AddDate , 111)
			FROM #tmp2 t
			WHERE t.GBID=g.ID
			AND t.PackingListID=pl.ID
			AND t.PulloutID=pl.PulloutID
		)a WHERE a.AddDate <> '' for xml path('')
	),1,1,'')
)PulloutReportConfirmDate

where pl.ID<>'' and 1=1 "));
            }

            #region Where 條件

            if (!MyUtility.Check.Empty(this.invdate1))
            {
                sqlCmd.Append(string.Format(" and g.InvDate >= '{0}' ", Convert.ToDateTime(this.invdate1).ToString("d")));
                sqlCmd_where.Append(string.Format(" and g.InvDate >= '{0}' ", Convert.ToDateTime(this.invdate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.invdate2))
            {
                sqlCmd.Append(string.Format(" and g.InvDate <= '{0}' ", Convert.ToDateTime(this.invdate2).ToString("d")));
                sqlCmd_where.Append(string.Format(" and g.InvDate <= '{0}' ", Convert.ToDateTime(this.invdate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.shipper))
            {
                sqlCmd.Append(string.Format(" and g.Shipper = '{0}'", this.shipper));
                sqlCmd_where.Append(string.Format(" and g.Shipper = '{0}'", this.shipper));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and g.BrandID = '{0}'", this.brand));
                sqlCmd_where.Append(string.Format(" and g.BrandID = '{0}'", this.brand));
            }

            if (!MyUtility.Check.Empty(this.shipmode))
            {
                sqlCmd.Append(string.Format(" and g.ShipModeID = '{0}'", this.shipmode));
                sqlCmd_where.Append(string.Format(" and g.BrandID = '{0}'", this.brand));
            }

            if (!MyUtility.Check.Empty(this.shipterm))
            {
                sqlCmd.Append(string.Format(" and g.ShipTermID = '{0}'", this.shipterm));
                sqlCmd_where.Append(string.Format(" and g.ShipTermID = '{0}'", this.shipterm));
            }

            if (!MyUtility.Check.Empty(this.dest))
            {
                sqlCmd.Append(string.Format(" and g.Dest = '{0}'", this.dest));
                sqlCmd_where.Append(string.Format(" and g.Dest = '{0}'", this.dest));
            }

            if (!MyUtility.Check.Empty(this.etd1))
            {
                sqlCmd.Append(string.Format(" and g.ETD >= '{0}' ", Convert.ToDateTime(this.etd1).ToString("d")));
                sqlCmd_where.Append(string.Format(" and g.Dest = '{0}'", this.dest));
            }

            if (!MyUtility.Check.Empty(this.etd2))
            {
                sqlCmd.Append(string.Format(" and g.ETD <= '{0}' ", Convert.ToDateTime(this.etd2).ToString("d")));
                sqlCmd_where.Append(string.Format(" and g.ETD <= '{0}' ", Convert.ToDateTime(this.etd2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.FCRDate1))
            {
                sqlCmd.Append(string.Format(" and g.FCRDate >= '{0}' ", Convert.ToDateTime(this.FCRDate1).ToString("d")));
                sqlCmd_where.Append(string.Format(" and g.FCRDate >= '{0}' ", Convert.ToDateTime(this.FCRDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.FCRDate2))
            {
                sqlCmd.Append(string.Format(" and g.FCRDate <= '{0}' ", Convert.ToDateTime(this.FCRDate2).ToString("d")));
                sqlCmd_where.Append(string.Format(" and g.FCRDate <= '{0}' ", Convert.ToDateTime(this.FCRDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.CutOffDate1))
            {
                sqlCmd.Append(string.Format(" and CAST( g.CutOffDate AS DATE) >= '{0}' ", Convert.ToDateTime(this.CutOffDate1).ToString("d")));
                sqlCmd_where.Append(string.Format(" and CAST( g.CutOffDate AS DATE) >= '{0}' ", Convert.ToDateTime(this.CutOffDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.CutOffDate2))
            {
                sqlCmd.Append(string.Format(" and CAST( g.CutOffDate AS DATE) <= '{0}' ", Convert.ToDateTime(this.CutOffDate2).ToString("d")));
                sqlCmd_where.Append(string.Format(" and CAST( g.CutOffDate AS DATE) <= '{0}' ", Convert.ToDateTime(this.CutOffDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.SOCFMDate1))
            {
                sqlCmd.Append(string.Format(" and g.SOCFMDate >= '{0}' ", Convert.ToDateTime(this.SOCFMDate1).ToString("d")));
                sqlCmd_where.Append(string.Format(" and g.SOCFMDate >= '{0}' ", Convert.ToDateTime(this.SOCFMDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.SOCFMDate2))
            {
                sqlCmd.Append(string.Format(" and g.SOCFMDate <= '{0}' ", Convert.ToDateTime(this.SOCFMDate2).ToString("d")));
                sqlCmd_where.Append(string.Format(" and g.SOCFMDate <= '{0}' ", Convert.ToDateTime(this.SOCFMDate2).ToString("d")));
            }

            if (this.status == "Confirmed")
            {
                sqlCmd.Append(" and g.Status = 'Confirmed'");
                sqlCmd_where.Append(" and g.Status = 'Confirmed'");
            }
            else if (this.status == "UnConfirmed")
            {
                sqlCmd.Append(" and g.Status <> 'Confirmed'");
                sqlCmd_where.Append(" and g.Status <> 'Confirmed'");
            }

            if (!MyUtility.Check.Empty(this.Delivery1))
            {
                this.hasDelivery = true;

                sqlCmd.Append(string.Format(
                    @"
and exists (select 1
            from (
                select pd.OrderID
                       , pd.OrderShipmodeSeq 
                from PackingList_Detail pd WITH (NOLOCK) 
                inner join PackingList pl2 on pd.ID = pl2.id where pl2.INVNo = g.ID 
            ) a
            inner join Order_QtyShip oq WITH (NOLOCK) on a.OrderID = oq.Id 
                                                         and a.OrderShipmodeSeq = oq.Seq 
                                                         and oq.BuyerDelivery between '{0}' and '{1}' )",
                    Convert.ToDateTime(this.Delivery1).ToString("d"),
                    Convert.ToDateTime(this.Delivery2).ToString("d")));
            }

            #endregion

            sqlCmd.Append(" order by g.ID" + Environment.NewLine + " DROP TABLE #tmp1,#tmp2");

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
            string strXltName = Env.Cfg.XltPathDir + (this.reportType == "1" ? "\\Shipping_R01_MainList.xltx" : "\\Shipping_R01_DetailList.xltx");
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            // 填內容值
            if (this.reportType == "1")
            {
                int intRowsStart = 3;
                object[,] objArray = new object[1, 29];
                foreach (DataRow dr in this.printData.Rows)
                {
                    objArray[0, 0] = dr["ID"];
                    objArray[0, 1] = dr["Shipper"];
                    objArray[0, 2] = dr["BrandID"];
                    objArray[0, 3] = dr["InvDate"];
                    objArray[0, 4] = dr["FCRDate"];
                    objArray[0, 5] = dr["CustCDID"];
                    objArray[0, 6] = dr["Dest"];
                    objArray[0, 7] = dr["ShipModeID"];
                    objArray[0, 8] = dr["CYCFS"];
                    objArray[0, 9] = dr["ShipTermID"];
                    objArray[0, 10] = dr["Handle"];
                    objArray[0, 11] = dr["Forwarder"];
                    objArray[0, 12] = dr["Vessel"];
                    objArray[0, 13] = dr["ETD"];
                    objArray[0, 14] = dr["ETA"];
                    objArray[0, 15] = dr["SONo"];
                    objArray[0, 16] = dr["SOCFMDate"];
                    objArray[0, 17] = dr["CutOffDate"];
                    objArray[0, 18] = dr["ShipPlanID"];
                    objArray[0, 19] = dr["Status"];
                    objArray[0, 20] = MyUtility.Check.Empty(dr["CTNTruck"]) ? dr["CTNTruck"] : MyUtility.Convert.GetString(dr["CTNTruck"]).Substring(0, MyUtility.Convert.GetString(dr["CTNTruck"]).Length - 1);
                    objArray[0, 21] = dr["TotalShipQty"];
                    objArray[0, 22] = dr["TotalCTNQty"];
                    objArray[0, 23] = dr["TotalGW"];
                    objArray[0, 24] = dr["TotalCBM"];
                    objArray[0, 25] = dr["AddDate"];
                    objArray[0, 26] = dr["ConfirmDate"];
                    objArray[0, 27] = dr["Remark"];
                    objArray[0, 28] = dr["PulloutComplete"].ToString();
                    worksheet.Range[string.Format("A{0}:AC{0}", intRowsStart)].Value2 = objArray;
                    intRowsStart++;
                }
            }
            else
            {
                int intRowsStart = 3;
                object[,] objArray = new object[1, 36];
                foreach (DataRow dr in this.printData.Rows)
                {
                    objArray[0, 0] = dr["ID"];
                    objArray[0, 1] = dr["Shipper"];
                    objArray[0, 2] = dr["BrandID"];
                    objArray[0, 3] = dr["InvDate"];
                    objArray[0, 4] = dr["MDivisionID"];
                    objArray[0, 5] = dr["PackID"];
                    objArray[0, 6] = MyUtility.Check.Empty(dr["OrderID"]) ? dr["OrderID"] : MyUtility.Convert.GetString(dr["OrderID"]).Substring(0, MyUtility.Convert.GetString(dr["OrderID"]).Length - 1);
                    objArray[0, 7] = dr["OrderShipmodeSeq"];
                    objArray[0, 8] = dr["POno"];
                    objArray[0, 9] = dr["BuyerDelivery"];
                    objArray[0, 10] = dr["SDPDate"];
                    objArray[0, 11] = dr["PulloutDate"];
                    objArray[0, 12] = dr["CutOffDate"];
                    objArray[0, 13] = dr["SONo"];
                    objArray[0, 14] = dr["SoConfirmDate"];
                    objArray[0, 15] = dr["Terminal/Whse#"];
                    objArray[0, 16] = dr["PulloutReportConfirmDate"];
                    objArray[0, 17] = dr["PulloutID"];
                    objArray[0, 18] = dr["ShipModeID"];
                    objArray[0, 19] = dr["CYCFS"];
                    objArray[0, 20] = dr["ShipQty"];
                    objArray[0, 21] = dr["CTNQty"];
                    objArray[0, 22] = dr["GW"];
                    objArray[0, 23] = dr["CBM"];
                    objArray[0, 24] = dr["CustCDID"];
                    objArray[0, 25] = dr["Dest"];
                    objArray[0, 26] = dr["ConfirmDate"];
                    objArray[0, 27] = dr["AddName"];
                    objArray[0, 28] = dr["AddDate"];
                    objArray[0, 29] = dr["ETD"];
                    objArray[0, 30] = dr["ETA"];
                    objArray[0, 31] = dr["BLNo"];
                    objArray[0, 32] = dr["BL2No"];
                    objArray[0, 33] = dr["Vessel"];
                    objArray[0, 34] = dr["Remark"];
                    worksheet.Range[string.Format("A{0}:AH{0}", intRowsStart)].Value2 = objArray;
                    if (this.hasDelivery &&
                        (MyUtility.Convert.GetDate(dr["BuyerDelivery"]) < this.dateDelivery.Value1 ||
                        MyUtility.Convert.GetDate(dr["BuyerDelivery"]) > this.dateDelivery.Value2))
                    {
                        worksheet.Range[string.Format("A{0}:AI{0}", intRowsStart)].Font.Color = ColorTranslator.ToOle(Color.Red);
                    }

                    intRowsStart++;
                }
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.CreateCustomizedExcel(ref worksheet);
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(this.reportType == "1" ? "Shipping_R01_MainList" : "Shipping_R01_DetailList");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

        private void Radio_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioMainList.Checked)
            {
                this.ReportType = "MainList";
                this.dateDelivery.ReadOnly = true;
                this.dateDelivery.Value1 = null;
                this.dateDelivery.Value2 = null;
            }

            if (this.radioDetailList.Checked)
            {
                this.ReportType = "DetailList";
                this.dateDelivery.ReadOnly = false;
            }
        }
    }
}
