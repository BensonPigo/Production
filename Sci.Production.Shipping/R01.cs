using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Linq;
using Sci.Production.CallPmsAPI;
using Newtonsoft.Json;

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
        private string category;
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
            this.category = this.comboDropDownList1.SelectedValue.ToString();
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

        private string sqlGetLocalMain = @"
select 
    g.ID
    ,g.Shipper
    ,g.BrandID
    ,g.FCRDate
    ,g.CustCDID
    ,Dest = (g.Dest+' - '+isnull(c.Alias,''))
    ,[POD] = PulloutPortPOD.POD
    ,g.ShipModeID
    ,g.ShipTermID
    ,g.DocumentRefNo
    ,[Forwarder] = (g.Forwarder+' - '+isnull(ls.Abb,''))
    ,g.SONo
    ,[SoConfirmDate] = g.SOCFMDate
    ,g.CutOffDate
	,[Terminal/Whse#] = fw.WhseCode
    ,g.ShipPlanID
    ,[ShipPlan Status] = sp.Status
    ,g.CYCFS	
    ,CTNTruck = stuff((select ','  + CTNRNo+'/'+TruckNo from GMTBooking_CTNR WITH (NOLOCK) where ID = g.ID for xml path('')),1,1,'')
    ,g.TotalShipQty
    ,g.TotalCTNQty
    ,g.TotalGW
    ,VM=(
			select sum(pld.APPEstAmtVW)
			from PackingList pl WITH (NOLOCK)
			inner join PackingList_Detail pld WITH (NOLOCK) on pl.id = pld.id
			where pl.INVNo =g.ID
		)
    ,g.TotalCBM
	,[Carton Qty at C-Log] = (
		select sum(pld.CTNQty)
		from PackingList pl WITH (NOLOCK)
		inner join PackingList_Detail pld WITH (NOLOCK) on pl.id = pld.id
		where pl.INVNo =g.ID)
	,[Sewing Inline] = (
		select min(o.SewInLine)
		from PackingList pl WITH (NOLOCK)
		inner join PackingList_Detail pld WITH (NOLOCK) on pl.id = pld.id
		inner join orders o WITH (NOLOCK) on o.id = pld.OrderID
		where pl.INVNo =g.ID)
	,[Sewing Offline] = (
		select min(o.SewOffLine)
		from PackingList pl WITH (NOLOCK)
		inner join PackingList_Detail pld WITH (NOLOCK) on pl.id = pld.id
		inner join orders o WITH (NOLOCK) on o.id = pld.OrderID
		where pl.INVNo =g.ID)
    ,ConfirmDate = IIF(g.Status = 'Confirmed',g.EditDate,null)
    ,[PulloutComplete]=IIF(PulloutIdCount.Value = PulloutIdConfirmLockCount.Value AND PulloutIdCount.Value > 0 ,'True' ,'False')
	,[Pull Out Date] = (select min(pl.PulloutDate) from PackingList pl WITH (NOLOCK) where pl.INVNo =g.ID)
	,[PulloutID] = (select min(pl.PulloutID) from PackingList pl WITH (NOLOCK) where pl.INVNo =g.ID)
    ,g.ETD
    ,g.ETA
    ,g.Vessel
    ,g.Remark
    ,[Handle] = dbo.getPass1(g.Handle)
    ,g.AddDate
into #tmpGMT
from GMTBooking g WITH (NOLOCK) 
left join Country c WITH (NOLOCK) on c.ID = g.Dest
left join LocalSupp ls WITH (NOLOCK) on ls.ID = g.Forwarder
left join ShipPlan sp WITH (NOLOCK) on sp.ID = g.ShipPlanID
left join ForwarderWarehouse_Detail fd WITH (NOLOCK) on fd.UKey = g.ForwarderWhse_DetailUKey
left join ForwarderWarehouse fw WITH (NOLOCK) on fw.id=fd.id
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
OUTER APPLY(
    select [POD] = concat(p.ID, '-', p.Name)
    from PulloutPort p
    where g.DischargePortID = p.ID
)PulloutPortPOD
where 1=1 {0}

select  g2.ID, [QAQty] = sum(sodd.QAQty)
into    #SewingOutput
from    (
        select  distinct g.ID, pld.OrderID
        from    #tmpGMT g
        inner join PackingList pl with (nolock) on pl.INVNo = g.ID
        inner join PackingList_Detail pld WITH (NOLOCK) on pl.id = pld.id
        ) g2
inner join SewingOutput_Detail_Detail sodd with (nolock) on sodd.OrderID = g2.OrderId
group by g2.ID

select  [ID] = pl.INVNo, [Category] = dl.Name, o.MDivisionID, [BuyerDelivery] = min(oq.BuyerDelivery)
into    #Packing
from PackingList pl WITH (NOLOCK)
inner join PackingList_Detail pld WITH (NOLOCK) on pl.id = pld.id
inner join orders o WITH (NOLOCK) on o.id = pld.OrderID
inner join DropDownList dl on dl.id = o.Category and dl.type = 'category'
inner join Order_QtyShip oq WITH (NOLOCK) on pld.OrderID = oq.Id and pld.OrderShipmodeSeq = oq.Seq
where pl.INVNo in (select ID from #tmpGMT)
group by pl.INVNo, dl.Name, o.MDivisionID

select  g.ID
	,Category = stuff((
			select distinct concat(',', p.Category)
			from #Packing p
			where p.ID = g.ID
			for xml path('')
		),1,1,'')
    ,g.Shipper
	,MDivisionID = stuff((
			select distinct concat(',', p.MDivisionID)
			from #Packing p
			where p.ID = g.ID
			for xml path('')
		),1,1,'')
    ,g.BrandID
	,BuyerDelivery = (select top 1 p.BuyerDelivery
		                from #Packing p
			            where p.ID = g.ID)
    ,g.FCRDate
    ,g.CustCDID
    ,g.Dest
    ,g.POD
    ,g.ShipModeID
    ,g.ShipTermID
    ,g.DocumentRefNo
    ,g.Forwarder
    ,g.SONo
    ,g.[SoConfirmDate]
    ,g.CutOffDate
	,g.[Terminal/Whse#]
    ,g.ShipPlanID
    ,g.[ShipPlan Status]
    ,g.CYCFS	
    ,g.CTNTruck
    ,g.TotalShipQty
    ,g.TotalCTNQty
    ,g.TotalGW
    ,g.VM
    ,g.TotalCBM
	,[Ttl. Prod. Out Qty] = s.QAQty
	,g.[Carton Qty at C-Log]
	,g.[Sewing Inline]
	,g.[Sewing Offline]
    ,g.ConfirmDate
    ,g.[PulloutComplete]
	,g.[Pull Out Date]
	,g.PulloutID
    ,g.ETD
    ,g.ETA
    ,g.Vessel
    ,g.Remark
    ,g.Handle
    ,g.AddDate
from #tmpGMT g
left join #SewingOutput s on s.ID = g.ID
order by g.ID
";

        private string sqlGetLocal = @"
select DISTINCT
    g.ID
	,Category = stuff((
			select distinct concat(',', dl.Name)
			from PackingList_Detail pld WITH (NOLOCK)
			inner join orders o WITH (NOLOCK) on o.id = pld.OrderID
			inner join DropDownList dl on dl.id = o.Category and dl.type = 'category'
			where pl.id = pld.id
			and pl.INVNo =g.ID
			for xml path('')
		),1,1,'')
    ,g.Shipper
    ,pl.MDivisionID
    ,g.BrandID
    ,BuyerDelivery = (select oq.BuyerDelivery from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd WITH (NOLOCK) where pd.ID = pl.ID) a, Order_QtyShip oq WITH (NOLOCK) where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq)
	,g.FCRDate
    ,OrderID = stuff((select concat(',',a.OrderID) from (select distinct OrderID from PackingList_Detail pd WITH (NOLOCK) where pd.ID = pl.ID) a for xml path('')), 1, 1, '')
	,[OrderShipmodeSeq] = STUFF ((select CONCAT (',', a.OrderShipmodeSeq) from (select distinct pd.OrderShipmodeSeq from PackingList_Detail pd WITH (NOLOCK) where pd.ID = pl.id) a for xml path('')), 1, 1, '') 
    ,[POno] = STUFF((
		select CONCAT (',',a.CustPONo) 
		from (
			select distinct o.CustPONo
			from PackingList_Detail pd WITH (NOLOCK) 
			left join orders o WITH (NOLOCK) on o.id = pd.OrderID
			where pd.ID = pl.id AND o.CustPONo<>'' AND o.CustPONo IS NOT NULL
		) a 
		for xml path('')), 1, 1, '') 
    ,PackID = pl.ID
    ,g.CustCDID
    ,Dest = concat(g.Dest, ' - ' + c.Alias)
    ,[POD] = PulloutPortPOD.POD
    ,g.ShipModeID
	,g.ShipTermID
    ,g.DocumentRefNo
    ,[Forwarder] = (g.Forwarder+' - '+isnull(ls.Abb,''))
	,g.SONo
    ,[SoConfirmDate]=g.SOCFMDate
    ,g.CutOffDate
    ,[Terminal/Whse#]= fw.WhseCode
	,g.ShipPlanID
    ,[ShipPlan Status] = sp.Status
    ,g.CYCFS
    ,CTNTruck = stuff((select ',' + CTNRNo+'/'+TruckNo from GMTBooking_CTNR WITH (NOLOCK) where ID = g.ID for xml path('')),1,1,'')
    ,ShipQty = isnull(pl.ShipQty,0)
    ,CTNQty = isnull(pl.CTNQty,0)
    ,GW = isnull(pl.GW,0)
    ,VM=(
			select sum(pld.APPEstAmtVW)
			from PackingList pl WITH (NOLOCK)
			inner join PackingList_Detail pld WITH (NOLOCK) on pl.id = pld.id
			where pl.INVNo =g.ID
		)
    ,CBM = isnull(pl.CBM,0)
	,[Ttl. Prod. Out Qty] = (
		select sum(QAQty)
		from SewingOutput_Detail_Detail sodd with(nolock)
		where exists (
			select 1
			from PackingList_Detail pld WITH (NOLOCK)
			where pl.id = pld.id
			and pl.INVNo =g.ID
			and pld.OrderID = sodd.OrderId))
	,[Carton Qty at C-Log] = (
		select sum(pld.CTNQty)
		from PackingList_Detail pld WITH (NOLOCK)
		where pl.id = pld.id
		and pl.INVNo =g.ID)
	,[Sewing Inline] = (
		select min(o.SewInLine)
		from PackingList_Detail pld WITH (NOLOCK)
		inner join orders o WITH (NOLOCK) on o.id = pld.OrderID
		where pl.id = pld.id
		and pl.INVNo =g.ID)
	,[Sewing Offline] = (
		select min(o.SewOffLine)
		from PackingList_Detail pld WITH (NOLOCK)
		inner join orders o WITH (NOLOCK) on o.id = pld.OrderID
		where pl.id = pld.id
		and pl.INVNo =g.ID)
    ,ConfirmDate = IIF(g.Status = 'Confirmed', g.EditDate, null)
    ,PulloutReportConfirmDate.PulloutReportConfirmDate
    ,pl.PulloutDate
    ,[PulloutID]=pl.PulloutID
    ,g.ETD
    ,g.ETA
    ,g.BLNo
    ,g.BL2No
    ,g.Vessel
    ,g.Remark
    ,AddName = concat(g.AddName, ' ' + p.Name)
    ,g.AddDate
from GMTBooking g WITH (NOLOCK) 
left join PackingList pl WITH (NOLOCK) on pl.INVNo = g.ID
left join Country c WITH (NOLOCK) on c.ID = g.Dest
left join Pass1 p WITH (NOLOCK) on p.ID = g.AddName
left join ForwarderWarehouse_Detail fd ON g.ForwarderWhse_DetailUKey=fd.UKey
left join ForwarderWarehouse fw WITH (NOLOCK) on fw.id=fd.id
left join LocalSupp ls WITH (NOLOCK) on ls.ID = g.Forwarder
left join ShipPlan sp WITH (NOLOCK) on sp.ID = g.ShipPlanID
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
OUTER APPLY(
    select [POD] = concat(p.ID, '-', p.Name)
    from PulloutPort p
    where g.DischargePortID = p.ID
)PulloutPortPOD

where pl.ID<>'' and 1=1 {0}
order by g.ID
";

        private string sqlGetA2B = @"
select DISTINCT
    g.ID
    ,[PackID] = gd.PackingListID
    ,g.Shipper
    ,g.BrandID
    ,g.InvDate
    ,g.CutOffDate
    ,[SoConfirmDate]=g.SOCFMDate
    ,[Terminal/Whse#]= fw.WhseCode
    ,g.ETD
    ,g.ETA
    ,g.CustCDID
    ,(g.Dest+' - '+isnull(c.Alias,'')) as Dest,IIF(g.Status = 'Confirmed',g.EditDate,null) as ConfirmDate
    ,g.DocumentRefNo
    ,[Forwarder] = (g.Forwarder+' - '+isnull(ls.Abb,''))
    ,g.AddName+' '+isnull(p.Name,'') as AddName
    ,g.AddDate
    ,g.Remark
    , g.SONo
    , g.ShipModeID
    , g.CYCFS
    , g.Vessel
    , g.BLNo
    , g.BL2No
    , [POD] = PulloutPortPOD.POD
    , gd.PLFromRgCode
	,g.FCRDate
	,g.ShipTermID
	,g.ShipPlanID
    ,[ShipPlan Status] = sp.Status
    ,CTNTruck = stuff((select ',' + CTNRNo+'/'+TruckNo from GMTBooking_CTNR WITH (NOLOCK) where ID = g.ID for xml path('')),1,1,'')
    ,g.TotalShipQty
    ,g.TotalCTNQty
    ,g.TotalGW
    ,VM=(
			select sum(pld.APPEstAmtVW)
			from PackingList pl WITH (NOLOCK)
			inner join PackingList_Detail pld WITH (NOLOCK) on pl.id = pld.id
			where pl.INVNo =g.ID
		)
    ,g.TotalCBM
    ,[Handle] = dbo.getPass1(g.Handle)
from GMTBooking g WITH (NOLOCK) 
inner join GMTBooking_Detail gd with (nolock) on g.ID = gd.ID
left join Country c WITH (NOLOCK) on c.ID = g.Dest
left join Pass1 p WITH (NOLOCK) on p.ID = g.AddName
left join ForwarderWarehouse_Detail fd ON g.ForwarderWhse_DetailUKey=fd.UKey
left join ForwarderWarehouse fw on fw.id = fd.id
left join LocalSupp ls WITH (NOLOCK) on ls.ID = g.Forwarder
left join ShipPlan sp WITH (NOLOCK) on sp.ID = g.ShipPlanID
OUTER APPLY(
    select [POD] = concat(p.ID, '-', p.Name)
    from PulloutPort p
    where g.DischargePortID = p.ID
)PulloutPortPOD

where 1=1 {0}";

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            StringBuilder sqlCmd_where = new StringBuilder();
            string sqlWherePack = string.Empty;

            #region Where 條件

            if (!MyUtility.Check.Empty(this.invdate1))
            {
                sqlCmd_where.Append(string.Format(" and g.InvDate >= '{0}' ", Convert.ToDateTime(this.invdate1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.invdate2))
            {
                sqlCmd_where.Append(string.Format(" and g.InvDate <= '{0}' ", Convert.ToDateTime(this.invdate2).ToString("yyyy/MM/dd")));
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
                sqlCmd_where.Append(string.Format(" and g.ShipModeID = '{0}'", this.shipmode));
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
                sqlCmd_where.Append(string.Format(" and g.ETD >= '{0}' ", Convert.ToDateTime(this.etd1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.etd2))
            {
                sqlCmd_where.Append(string.Format(" and g.ETD <= '{0}' ", Convert.ToDateTime(this.etd2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.FCRDate1))
            {
                sqlCmd_where.Append(string.Format(" and g.FCRDate >= '{0}' ", Convert.ToDateTime(this.FCRDate1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.FCRDate2))
            {
                sqlCmd_where.Append(string.Format(" and g.FCRDate <= '{0}' ", Convert.ToDateTime(this.FCRDate2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.CutOffDate1))
            {
                sqlCmd_where.Append(string.Format(" and CAST( g.CutOffDate AS DATE) >= '{0}' ", Convert.ToDateTime(this.CutOffDate1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.CutOffDate2))
            {
                sqlCmd_where.Append(string.Format(" and CAST( g.CutOffDate AS DATE) <= '{0}' ", Convert.ToDateTime(this.CutOffDate2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.SOCFMDate1))
            {
                sqlCmd_where.Append(string.Format(" and g.SOCFMDate >= '{0}' ", Convert.ToDateTime(this.SOCFMDate1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.SOCFMDate2))
            {
                sqlCmd_where.Append(string.Format(" and g.SOCFMDate <= '{0}' ", Convert.ToDateTime(this.SOCFMDate2).ToString("yyyy/MM/dd")));
            }

            if (this.status == "Confirmed")
            {
                sqlCmd_where.Append(" and g.Status = 'Confirmed'");
            }
            else if (this.status == "UnConfirmed")
            {
                sqlCmd_where.Append(" and g.Status <> 'Confirmed'");
            }

            if (!MyUtility.Check.Empty(this.category))
            {
                sqlWherePack += string.Format(
                    @"
and exists(
    select 1
    from PackingList pl WITH (NOLOCK)
    inner join PackingList_Detail pld WITH (NOLOCK) on pl.id = pld.id
    inner join orders o WITH (NOLOCK) on o.id = pld.OrderID
    where pl.INVNo =g.ID
    and o.Category in ({0})
)", this.category);
            }

            if (!MyUtility.Check.Empty(this.Delivery1))
            {
                this.hasDelivery = true;

                sqlWherePack += string.Format(
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
                    Convert.ToDateTime(this.Delivery1).ToString("yyyy/MM/dd"),
                    Convert.ToDateTime(this.Delivery2).ToString("yyyy/MM/dd"));
            }
            #endregion

            string sqlGetLocalMainFin = string.Format(this.sqlGetLocalMain, sqlCmd_where + sqlWherePack);
            string sqlGetLocalFin = string.Format(this.sqlGetLocal, sqlCmd_where　+ sqlWherePack);
            string sqlGetA2BFin = string.Format(this.sqlGetA2B, sqlCmd_where);

            if (this.reportType != "1")
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

            }

            sqlCmd.Append("{0}");

            if (this.reportType != "1")
            {
                sqlCmd.Append(Environment.NewLine + " DROP TABLE #tmp1,#tmp2");
            }

            DualResult result;
            if (this.reportType == "1")
            {
                result = DBProxy.Current.Select(null, string.Format(sqlCmd.ToString(), sqlGetLocalMainFin), out this.printData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

                #region GetA2B Data
                DataTable dtMainA2B;

                result = DBProxy.Current.Select(null, string.Format(sqlCmd.ToString(), sqlGetA2BFin), out dtMainA2B);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

                if (dtMainA2B.Rows.Count > 0)
                {
                    string getPackFromA2B = $@"

select  g2.ID, [QAQty] = sum(sodd.QAQty)
into    #SewingOutput
from    (
        select  distinct g.ID, pld.OrderID
        from    #tmp g
        inner join PackingList pl with (nolock) on pl.INVNo = g.ID
        inner join PackingList_Detail pld WITH (NOLOCK) on pl.id = pld.id
        ) g2
inner join SewingOutput_Detail_Detail sodd with (nolock) on sodd.OrderID = g2.OrderId
group by g2.ID

select  [ID] = pl.INVNo, [Category] = isnull(dl.Name, ''), o.MDivisionID, [BuyerDelivery] = min(oq.BuyerDelivery)
into    #Packing
from PackingList pl WITH (NOLOCK)
inner join PackingList_Detail pld WITH (NOLOCK) on pl.id = pld.id
inner join orders o WITH (NOLOCK) on o.id = pld.OrderID
inner join Order_QtyShip oq WITH (NOLOCK) on pld.OrderID = oq.Id and pld.OrderShipmodeSeq = oq.Seq
left join DropDownList dl on dl.id = o.Category and dl.type = 'category'
where exists(select 1 from #tmp where ID = pl.INVNo) 
group by pl.INVNo, isnull(dl.Name, ''), o.MDivisionID


select DISTINCT
    g.ID
	,Category = stuff((
			select distinct concat(',', p.Category)
			from #Packing p
			where p.ID = g.ID
			for xml path('')
		),1,1,'')
    ,g.Shipper
	,MDivisionID = stuff((
			select distinct concat(',', p.MDivisionID)
			from #Packing p
			where p.ID = g.ID
			for xml path('')
		),1,1,'')
    ,g.BrandID
	,BuyerDelivery = (select top 1 p.BuyerDelivery
		                from #Packing p
			            where p.ID = g.ID)
	,g.FCRDate
    ,g.CustCDID
    ,g.Dest
    ,g.POD
    ,g.ShipModeID
    ,g.ShipTermID
    ,g.DocumentRefNo
    ,g.Forwarder
    ,g.SONo
    ,g.[SoConfirmDate]
    ,g.CutOffDate
    ,g.[Terminal/Whse#]
	,g.ShipPlanID
    ,g.[ShipPlan Status]
    ,g.CYCFS
    ,g.CTNTruck
    ,g.TotalShipQty
    ,g.TotalCTNQty
    ,g.TotalGW
    ,g.TotalCBM
	,[Ttl. Prod. Out Qty] = s.QAQty
	,[Carton Qty at C-Log] = (
		select sum(pld.CTNQty)
		from PackingList pl WITH (NOLOCK)
		inner join PackingList_Detail pld WITH (NOLOCK) on pl.id = pld.id
		where pl.INVNo =g.ID)
	,[Sewing Inline] = (
		select min(o.SewInLine)
		from PackingList pl WITH (NOLOCK)
		inner join PackingList_Detail pld WITH (NOLOCK) on pl.id = pld.id
		inner join orders o WITH (NOLOCK) on o.id = pld.OrderID
		where pl.INVNo =g.ID)
	,[Sewing Offline] = (
		select min(o.SewOffLine)
		from PackingList pl WITH (NOLOCK)
		inner join PackingList_Detail pld WITH (NOLOCK) on pl.id = pld.id
		inner join orders o WITH (NOLOCK) on o.id = pld.OrderID
		where pl.INVNo =g.ID)
    ,g.ConfirmDate
    ,[PulloutComplete]=IIF(PulloutIdCount.Value = PulloutIdConfirmLockCount.Value AND PulloutIdCount.Value > 0 ,'True' ,'False')
	,[Pull Out Date] = (select min(pl.PulloutDate) from PackingList pl WITH (NOLOCK) where pl.INVNo =g.ID)
	,[PulloutID] = (select min(pl.PulloutID) from PackingList pl WITH (NOLOCK) where pl.INVNo =g.ID)
    ,g.ETD
    ,g.ETA
    ,g.Vessel
    ,g.Remark
    ,g.Handle
    ,g.AddDate
    ,OrderID = stuff((
			select distinct concat(',',  pld.OrderID)
			from PackingList pl WITH (NOLOCK)
			inner join PackingList_Detail pld WITH (NOLOCK) on pl.id = pld.id
			where pl.INVNo =g.ID
			for xml path('')
		),1,1,'')
from #tmp g 
left join #SewingOutput s on s.ID = g.ID
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
where 1 = 1 {sqlWherePack}

drop table #Packing, #tmp, #SewingOutput
";

                    var listGroupMainA2B = dtMainA2B.AsEnumerable()
                                            .GroupBy(s => s["PLFromRgCode"].ToString())
                                            .Select(s => new
                                            {
                                                PLFromRgCode = s.Key,
                                                GMTData = s.CopyToDataTable(),
                                            });

                    DataTable dtFinalMainA2B = new DataTable();

                    foreach (var groupItem in listGroupMainA2B)
                    {
                        PackingA2BWebAPI_Model.DataBySql dataBySql = new PackingA2BWebAPI_Model.DataBySql()
                        {
                            SqlString = getPackFromA2B,
                            TmpTable = JsonConvert.SerializeObject(groupItem.GMTData),
                        };

                        DataTable dtPackA2B;
                        result = PackingA2BWebAPI.GetDataBySql(groupItem.PLFromRgCode, dataBySql, out dtPackA2B);

                        if (!result)
                        {
                            return result;
                        }

                        dtPackA2B.MergeTo(ref dtFinalMainA2B);
                    }

                    if (dtFinalMainA2B.Rows.Count > 0)
                    {
                        string sqlGetPulloutReportConfirmDate = $@"
alter table #tmp alter column PulloutID varchar(13)
select g.ID
    , g.Category
    , g.Shipper
    , g.MDivisionID
    , g.BrandID
    , g.BuyerDelivery
	, g.FCRDate
    , g.CustCDID
    , g.Dest
    , g.POD
    , g.ShipModeID
	, g.ShipTermID
    , g.DocumentRefNo
    , g.Forwarder
    , g.SONo
    , g.[SoConfirmDate]
    , g.CutOffDate
    , g.[Terminal/Whse#]
	, g.ShipPlanID
    , g.[ShipPlan Status]
    , g.CYCFS
    , g.CTNTruck
    , g.TotalShipQty
    , g.TotalCTNQty
    , g.TotalGW
    , g.TotalCBM
    , g.[Ttl. Prod. Out Qty]
    , g.[Carton Qty at C-Log]
    , g.[Sewing Inline]
    , g.[Sewing Offline]
    , g.ConfirmDate
    , g.[PulloutComplete]
    , g.[Pull Out Date]
    , g.[PulloutID]
    , g.ETD
    , g.ETA
    , g.Vessel
    , g.Remark
    , g.Handle
    , g.AddDate
from #tmp g
outer apply(
        SELECT  [val] =  Stuff((select CONCAT (',',a.AddDate) 
                                FROM    (
                                            select  [AddDate] = CASE WHEN Pullout_ReviseAddDate.val IS NOT NULL THEN  Pullout_ReviseAddDate.val
		                                            ELSE  (SELECT TOP 1 SendToTPE FROM Pullout with (nolock) WHERE ID = g.PulloutID) 
		                                            END
                                            from dbo.SplitString(g.OrderID, ',') packOrders
					                        outer apply( SELECT TOP 1 [val] = pr.AddDate
           			                        				FROM Pullout_Revise pr with (nolock)
           			                        				WHERE  pr.ID = g.PulloutID 
           			                        				AND pr.OrderID = packOrders.Data
           			                        				ORDER BY AddDate DESC
					                        			) Pullout_ReviseAddDate
                                        ) a 
                                WHERE a.AddDate <> '' for xml path(''))
                               ,1,1,'')
)   PulloutReportConfirmDate

";
                        result = MyUtility.Tool.ProcessWithDatatable(dtFinalMainA2B, null, sqlGetPulloutReportConfirmDate, out dtFinalMainA2B);
                        if (!result)
                        {
                            return result;
                        }

                        dtFinalMainA2B.MergeTo(ref this.printData);
                    }
                }
                #endregion
            }
            else
            {
                result = DBProxy.Current.Select(null, string.Format(sqlCmd.ToString(), sqlGetLocalFin), out this.printData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

                #region GetA2B Data
                DataTable dtMainA2B;

                result = DBProxy.Current.Select(null, string.Format(sqlCmd.ToString(), sqlGetA2BFin), out dtMainA2B);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

                if (dtMainA2B.Rows.Count > 0)
                {
                    string getPackFromA2B = @"

select DISTINCT
    g.ID
	,Category = stuff((
			select distinct concat(',', dl.Name)
			from PackingList_Detail pld WITH (NOLOCK)
			inner join orders o WITH (NOLOCK) on o.id = pld.OrderID
			inner join DropDownList dl on dl.id = o.Category and dl.type = 'category'
			where pl.id = pld.id
			and pl.INVNo =g.ID
			for xml path('')
		),1,1,'')
    ,g.Shipper
    ,pl.MDivisionID
    ,g.BrandID
    ,BuyerDelivery = (select oq.BuyerDelivery from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd WITH (NOLOCK) where pd.ID = pl.ID) a, Order_QtyShip oq WITH (NOLOCK) where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq)
	,g.FCRDate
    ,OrderID = stuff((select concat(',',a.OrderID) from (select distinct OrderID from PackingList_Detail pd WITH (NOLOCK) where pd.ID = pl.ID) a for xml path('')), 1, 1, '')
	,[OrderShipmodeSeq] = STUFF ((select CONCAT (',', a.OrderShipmodeSeq) from (select distinct pd.OrderShipmodeSeq from PackingList_Detail pd WITH (NOLOCK) where pd.ID = pl.id) a for xml path('')), 1, 1, '') 
    ,[POno] = STUFF((
		select CONCAT (',',a.CustPONo) 
		from (
			select distinct o.CustPONo
			from PackingList_Detail pd WITH (NOLOCK) 
			left join orders o WITH (NOLOCK) on o.id = pd.OrderID
			where pd.ID = pl.id AND o.CustPONo<>'' AND o.CustPONo IS NOT NULL
		) a 
		for xml path('')), 1, 1, '') 
    ,g.PackID
    ,g.CustCDID
    ,g.Dest
    ,g.POD
    ,g.ShipModeID
	,g.ShipTermID
    ,g.DocumentRefNo
    ,g.Forwarder
	,g.SONo
    ,g.[SoConfirmDate]
    ,g.CutOffDate
    ,g.[Terminal/Whse#]
	,g.ShipPlanID
    ,g.[ShipPlan Status]
    ,g.CYCFS
    ,g.CTNTruck
    ,ShipQty = isnull(pl.ShipQty,0)
    ,CTNQty = isnull(pl.CTNQty,0)
    ,GW = isnull(pl.GW,0)
    ,CBM = isnull(pl.CBM,0)
	,[Ttl. Prod. Out Qty] = (
		select sum(QAQty)
		from SewingOutput_Detail_Detail sodd with(nolock)
		where exists (
			select 1
			from PackingList_Detail pld WITH (NOLOCK)
			where pl.id = pld.id
			and pl.INVNo =g.ID
			and pld.OrderID = sodd.OrderId))
	,[Carton Qty at C-Log] = (
		select sum(pld.CTNQty)
		from PackingList_Detail pld WITH (NOLOCK)
		where pl.id = pld.id
		and pl.INVNo =g.ID)
	,[Sewing Inline] = (
		select min(o.SewInLine)
		from PackingList_Detail pld WITH (NOLOCK)
		inner join orders o WITH (NOLOCK) on o.id = pld.OrderID
		where pl.id = pld.id
		and pl.INVNo =g.ID)
	,[Sewing Offline] = (
		select min(o.SewOffLine)
		from PackingList_Detail pld WITH (NOLOCK)
		inner join orders o WITH (NOLOCK) on o.id = pld.OrderID
		where pl.id = pld.id
		and pl.INVNo =g.ID)
    ,g.ConfirmDate
    ,pl.PulloutDate
    ,[PulloutID]=pl.PulloutID
    ,g.ETD
    ,g.ETA
    ,g.BLNo
    ,g.BL2No
    ,g.Vessel
    ,g.AddName
    ,g.AddDate
    ,g.Remark
from #tmp g 
inner join PackingList pl WITH (NOLOCK) on pl.ID = g.PackID
";

                    var listGroupMainA2B = dtMainA2B.AsEnumerable()
                                            .GroupBy(s => s["PLFromRgCode"].ToString())
                                            .Select(s => new
                                            {
                                                PLFromRgCode = s.Key,
                                                GMTData = s.CopyToDataTable(),
                                            });

                    DataTable dtFinalMainA2B = new DataTable();

                    foreach (var groupItem in listGroupMainA2B)
                    {
                        PackingA2BWebAPI_Model.DataBySql dataBySql = new PackingA2BWebAPI_Model.DataBySql()
                        {
                            SqlString = getPackFromA2B,
                            TmpTable = JsonConvert.SerializeObject(groupItem.GMTData),
                        };

                        DataTable dtPackA2B;
                        result = PackingA2BWebAPI.GetDataBySql(groupItem.PLFromRgCode, dataBySql, out dtPackA2B);

                        if (!result)
                        {
                            return result;
                        }

                        dtPackA2B.MergeTo(ref dtFinalMainA2B);
                    }

                    if (dtFinalMainA2B.Rows.Count > 0)
                    {
                        string sqlGetPulloutReportConfirmDate = $@"
alter table #tmp alter column PulloutID varchar(13)
select g.ID
    , g.Category
    , g.Shipper
    , g.MDivisionID
    , g.BrandID
    , g.BuyerDelivery
	, g.FCRDate
    , g.OrderID
    , g.OrderShipmodeSeq
    , g.POno
    , g.PackID
    , g.CustCDID
    , g.Dest
    , g.POD
    , g.ShipModeID
	, g.ShipTermID
    , g.DocumentRefNo
    , g.Forwarder
    , g.SONo
    , g.[SoConfirmDate]
    , g.CutOffDate
    , g.[Terminal/Whse#]
	, g.ShipPlanID
    , g.[ShipPlan Status]
    , g.CYCFS
    , g.CTNTruck
    , g.ShipQty
    , g.CTNQty
    , g.GW
    , g.CBM
    , g.[Ttl. Prod. Out Qty]
    , g.[Carton Qty at C-Log]
    , g.[Sewing Inline]
    , g.[Sewing Offline]
    , g.ConfirmDate
    , [PulloutReportConfirmDate] = PulloutReportConfirmDate.val
    , g.PulloutDate
    , g.PulloutID
    , g.ETD
    , g.ETA
    , g.BLNo
    , g.BL2No
    , g.Vessel
    , g.Remark
    , g.AddName
    , g.AddDate
from #tmp g
outer apply(
        SELECT  [val] =  Stuff((select CONCAT (',',format(a.AddDate,'yyyy/MM/dd')) 
                                FROM    (
                                            select  [AddDate] = CASE WHEN Pullout_ReviseAddDate.val IS NOT NULL THEN  Pullout_ReviseAddDate.val
		                                            ELSE  (SELECT TOP 1 SendToTPE FROM Pullout with (nolock) WHERE ID = g.PulloutID) 
		                                            END
                                            from dbo.SplitString(g.OrderID, ',') packOrders
					                        outer apply( SELECT TOP 1 [val] = pr.AddDate
           			                        				FROM Pullout_Revise pr with (nolock)
           			                        				WHERE  pr.ID = g.PulloutID 
           			                        				AND pr.OrderID = packOrders.Data
           			                        				ORDER BY AddDate DESC
					                        			) Pullout_ReviseAddDate
                                        ) a 
                                WHERE a.AddDate <> '' for xml path(''))
                               ,1,1,'')
)   PulloutReportConfirmDate

";
                        result = MyUtility.Tool.ProcessWithDatatable(dtFinalMainA2B, null, sqlGetPulloutReportConfirmDate, out dtFinalMainA2B);
                        if (!result)
                        {
                            return result;
                        }

                        dtFinalMainA2B.MergeTo(ref this.printData);
                    }
                }
                #endregion
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
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, strXltName, 2, false, null, excel);
            }
            else
            {
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, strXltName, 2, false, null, excel);

                int intRowsStart = 3;
                foreach (DataRow dr in this.printData.Rows)
                {
                    if (this.hasDelivery &&
                        (MyUtility.Convert.GetDate(dr["BuyerDelivery"]) < this.dateDelivery.Value1 ||
                        MyUtility.Convert.GetDate(dr["BuyerDelivery"]) > this.dateDelivery.Value2))
                    {
                        worksheet.Range[string.Format("A{0}:AT{0}", intRowsStart)].Font.Color = ColorTranslator.ToOle(Color.Red);
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
