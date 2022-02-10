﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Sci.Production.CallPmsAPI;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R11
    /// </summary>
    public partial class R11 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private int reportType;
        private int reportType2;
        private DateTime? date1;
        private DateTime? date2;
        private DateTime? onBoardDate1;
        private DateTime? onBoardDate2;
        private string brand;
        private string custCD;
        private string dest;
        private string shipMode;
        private string forwarder;
        private bool excludePackingFoc;
        private bool excludePackingLocalOrder;

        /// <summary>
        /// R11
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.radioGarment.Checked = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.txtshipmode.SelectedIndex = -1;
        }

        // Forwarder
        private void TxtForwarder_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string selectCommand;
            selectCommand = @"
select DISTINCT l.ID ,l.Abb
from LocalSupp l WITH (NOLOCK) 
union all
select ID,AbbEN from Supp WITH (NOLOCK) 
order by ID";

            DataTable tbSelect;
            DBProxy.Current.Select(null, selectCommand, out tbSelect);
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(tbSelect, "ID,Abb", "9,13", this.Text, false, ",", "ID,Abb");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            IList<DataRow> selected = item.GetSelecteds();
            this.txtForwarder.Text = item.GetSelectedString();
            this.displayForwarder.Value = MyUtility.Convert.GetString(selected[0]["Abb"]);
        }

        // Forwarder
        private void TxtForwarder_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtForwarder.OldValue != this.txtForwarder.Text)
            {
                if (!MyUtility.Check.Empty(this.txtForwarder.Text))
                {
                    DataRow inputData;
                    string sql = string.Format(
                        @"select * from (
select DISTINCT l.ID ,l.Abb
from LocalSupp l WITH (NOLOCK) 
union all
select ID,AbbEN from Supp WITH (NOLOCK)) a
where a.ID = '{0}'", this.txtForwarder.Text);
                    if (!MyUtility.Check.Seek(sql, out inputData))
                    {
                        this.txtForwarder.Text = string.Empty;
                        this.displayForwarder.Value = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Forwarder: {0} > not found!!!", this.txtForwarder.Text));
                        return;
                    }
                    else
                    {
                        this.txtForwarder.Text = this.txtForwarder.Text;
                        this.displayForwarder.Value = MyUtility.Convert.GetString(inputData["Abb"]);
                    }
                }
                else
                {
                    this.txtForwarder.Text = string.Empty;
                    this.displayForwarder.Value = string.Empty;
                }
            }
        }

        private void RadioGarment_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioGarment.Checked)
            {
                this.labelPulloutDate.Text = "Pullout Date";
                this.labelPulloutDate.Size = new System.Drawing.Size(101, 23);
                this.labelPulloutDate.Location = new System.Drawing.Point(13, 143);
                this.txtbrand.Enabled = true;
                this.txtcustcd.Enabled = true;
                this.dateOnBoardDate.Enabled = true;
            }
            else
            {
                this.labelPulloutDate.Text = "Arrive Port Date \r\n (Ship Date)";
                this.labelPulloutDate.Size = new System.Drawing.Size(101, 36);
                this.labelPulloutDate.Location = new System.Drawing.Point(13, 130);
                this.txtbrand.Enabled = false;
                this.txtcustcd.Enabled = false;
                this.dateOnBoardDate.Enabled = false;
            }

            this.EnableReportType();
        }

        private void RadioRawMaterial_CheckedChanged(object sender, EventArgs e)
        {
            this.EnableReportType();
        }

        private void EnableReportType()
        {
            if (this.radioGarment.Checked)
            {
                this.rdbtnMainList.Enabled = true;
                this.rdbtnDetailList.Enabled = true;
                this.rdbtnMajorItem.Enabled = true;
                this.rdbtnMainList.Checked = true;
                this.chkExcludePackingFOC.Enabled = true;
                this.chkExcludePackingLocalOrder.Enabled = true;
                this.chkExcludePackingFOC.Checked = true;
                this.chkExcludePackingLocalOrder.Checked = true;
            }
            else if (this.radioRawMaterial.Checked)
            {
                this.rdbtnMainList.Enabled = false;
                this.rdbtnDetailList.Enabled = false;
                this.rdbtnMajorItem.Enabled = false;
                this.rdbtnMainList.Checked = false;
                this.rdbtnDetailList.Checked = false;
                this.rdbtnMajorItem.Checked = false;
                this.chkExcludePackingFOC.Enabled = false;
                this.chkExcludePackingLocalOrder.Enabled = false;
                this.chkExcludePackingFOC.Checked = false;
                this.chkExcludePackingLocalOrder.Checked = false;
            }
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.date1 = this.datePulloutDate.Value1;
            this.date2 = this.datePulloutDate.Value2;
            this.onBoardDate1 = this.dateOnBoardDate.Value1;
            this.onBoardDate2 = this.dateOnBoardDate.Value2;
            this.brand = this.txtbrand.Text;
            this.custCD = this.txtcustcd.Text;
            this.dest = this.txtcountryDestination.TextBox1.Text;
            this.shipMode = this.txtshipmode.Text;
            this.forwarder = this.txtForwarder.Text;
            this.reportType = this.radioGarment.Checked ? 1 : 2;
            this.reportType2 = this.rdbtnMainList.Checked ? 1 : this.rdbtnDetailList.Checked ? 2 : 3;
            this.excludePackingFoc = this.chkExcludePackingFOC.Checked;
            this.excludePackingLocalOrder = this.chkExcludePackingLocalOrder.Checked;

            return base.ValidateInput();
        }

        private string sqlPackingListColA2B = @"
select  p.Type,
        p.PulloutDate,
        p.INVNo,
        p.ID,
        p.MDivisionID,
        p.BrandID,
        p.CustCDID,
        p.ShipModeID,
        p.ShipQty,
        p.CTNQty,
        p.GW,
        p.CBM
from PackingList p with (nolock)
";

        private string sqlPackingListDetailColA2B = @"
select  distinct
        pd.ID,
        pd.OrderID,
        pd.OrderShipmodeSeq
from PackingList_Detail pd with (nolock)
";

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult result;
            DataTable dtPackingListA2B;
            DataTable dtPackingList_DetailA2B;

            result = DBProxy.Current.Select(null, this.sqlPackingListColA2B + " where 1 = 0", out dtPackingListA2B);
            if (!result)
            {
                return result;
            }

            result = DBProxy.Current.Select(null, this.sqlPackingListDetailColA2B + " where 1 = 0", out dtPackingList_DetailA2B);
            if (!result)
            {
                return result;
            }

            #region get A2B Data
            string sqlGetGMTBooking_Detail = $@"
select distinct PLFromRgCode, PackingListID 
from GMTBooking_Detail with (nolock)
where 1 = 1 ";

            if (!MyUtility.Check.Empty(this.date1))
            {
                sqlGetGMTBooking_Detail += string.Format(" and PulloutDate >= '{0}'", Convert.ToDateTime(this.date1).ToString("yyyyMMdd"));
            }

            if (!MyUtility.Check.Empty(this.date2))
            {
                sqlGetGMTBooking_Detail += string.Format(" and PulloutDate <= '{0}'", Convert.ToDateTime(this.date2).ToString("yyyyMMdd"));
            }

            DataTable dtGMTBooking_Detail;

            result = DBProxy.Current.Select(null, sqlGetGMTBooking_Detail, out dtGMTBooking_Detail);
            if (!result)
            {
                return result;
            }

            if (dtGMTBooking_Detail.Rows.Count > 0)
            {
                var groupGMTBooking_Detail = dtGMTBooking_Detail.AsEnumerable()
                                                .GroupBy(s => s["PLFromRgCode"].ToString())
                                                .Select(s => new
                                                {
                                                    PLFromRgCode = s.Key,
                                                    WherePackID = s.Select(groupItem => $"'{groupItem["PackingListID"]}'").JoinToString(","),
                                                });

                foreach (var groupItem in groupGMTBooking_Detail)
                {
                    string sqlGetPackingListA2B = $@"
{this.sqlPackingListColA2B}
where p.ID in ({groupItem.WherePackID})
";

                    string sqlGetPackingListDetailA2B = $@"
{this.sqlPackingListDetailColA2B}
where pd.ID in ({groupItem.WherePackID})
";
                    DataTable dtResultA2B;

                    result = PackingA2BWebAPI.GetDataBySql(groupItem.PLFromRgCode, sqlGetPackingListA2B, out dtResultA2B);
                    if (!result)
                    {
                        return result;
                    }

                    dtResultA2B.MergeTo(ref dtPackingListA2B);

                    result = PackingA2BWebAPI.GetDataBySql(groupItem.PLFromRgCode, sqlGetPackingListDetailA2B, out dtResultA2B);
                    if (!result)
                    {
                        return result;
                    }

                    dtResultA2B.MergeTo(ref dtPackingList_DetailA2B);
                }
            }
            #endregion

            SqlConnection sqlConnection;
            result = DBProxy._OpenConnection(null, out sqlConnection);

            if (!result)
            {
                return result;
            }

            DataTable dtEmpty;
            using (sqlConnection)
            {
                result = MyUtility.Tool.ProcessWithDatatable(dtPackingListA2B, null, "select [Empty] = 1", out dtEmpty, temptablename: "#tmpPackingA2B", conn: sqlConnection);
                if (!result)
                {
                    return result;
                }

                result = MyUtility.Tool.ProcessWithDatatable(dtPackingList_DetailA2B, null, "select [Empty] = 1", out dtEmpty, temptablename: "#tmpPackingDetailA2B", conn: sqlConnection);
                if (!result)
                {
                    return result;
                }

                StringBuilder sqlCmd = new StringBuilder();
                sqlCmd.Append(@"

alter table #tmpPackingA2B alter column Type varchar(1)
alter table #tmpPackingA2B alter column INVNo varchar(25)
alter table #tmpPackingA2B alter column ID varchar(13)
alter table #tmpPackingA2B alter column BrandID varchar(8)
alter table #tmpPackingA2B alter column CustCDID varchar(16)
alter table #tmpPackingA2B alter column ShipModeID varchar(10)

alter table #tmpPackingDetailA2B alter column ID varchar(13)
alter table #tmpPackingDetailA2B alter column OrderID varchar(13)
alter table #tmpPackingDetailA2B alter column OrderShipmodeSeq varchar(2);

select * 
into #tmpPackingA2B_final
from (
select  Type,
        PulloutDate,
        INVNo,
        ID,
        MDivisionID,
        BrandID,
        CustCDID,
        ShipModeID,
        ShipQty,
        CTNQty,
        GW,
        CBM 
from PackingList with (nolock)
union all
select  p.Type,
        p.PulloutDate,
        p.INVNo,
        p.ID,
        p.MDivisionID,
        p.BrandID,
        p.CustCDID,
        p.ShipModeID,
        p.ShipQty,
        p.CTNQty,
        p.GW,
        p.CBM
from #tmpPackingA2B p 
) a


select * 
into #tmpPackingDetailA2B_final
from (
select  distinct
        pd.ID,
        pd.OrderID,
        pd.OrderShipmodeSeq
from PackingList_Detail pd with (nolock)
union all
select  distinct
        pd.ID,
        pd.OrderID,
        pd.OrderShipmodeSeq
from #tmpPackingDetailA2B pd 
) a
;
");

                if (this.reportType == 1)
                {
                    if (this.reportType2 == 1)
                    {
                        sqlCmd.Append(@"
with GBData
as (
    select 
    	   IE = 'Export'
           , Type = 'GARMENT'
           , g.ID
           , OnBoardDate = g.ETD
           , g.Shipper 
           , [Foundry] = iif(ISNULL(g.Foundry,'0') = '0', '' , 'Y')
           , g.BrandID
           , Category = 
		    Stuff((
				select distinct concat(',', IIF(pack.Type = 'B','Bulk',IIF(pack.Type = 'S','Sample','')) )
                from (
                        select  p.Type
				        from #tmpPackingA2B_final p WITH (NOLOCK) where p.INVNo = g.ID
                    ) pack
				for xml path('')
		    ),1,1,'')
		   , g.CustCDID
		   , g.Dest
		   , g.ShipModeID
		   , PulloutDate = (select MAX(pack.PulloutDate) 
                            from (
                                select PulloutDate from #tmpPackingA2B_final WITH (NOLOCK) where INVNo = g.ID
                                ) pack
                            )
		   , g.TotalShipQty
		   , g.TotalCTNQty
		   , g.TotalGW
		   , g.TotalCBM
		   , Forwarder = g.Forwarder+'-'+isnull(l.Abb,'')
		   , BLNo = ''
		   , [NoExportCharges] = iif(isnull(g.NoExportCharges,0)=1,'V','')
    from GMTBooking g WITH (NOLOCK) 
    left join LocalSupp l WITH (NOLOCK) on l.ID = g.Forwarder
    where not exists (
			    select 1 
			    from ShareExpense WITH (NOLOCK) 
			    where InvNo = g.ID) ");

                        if (!MyUtility.Check.Empty(this.date1))
                        {
                            sqlCmd.Append(string.Format(
                                @"
and (
        exists(select 1 from #tmpPackingA2B_final p WITH (NOLOCK) where p.INVNo = g.ID and p.PulloutDate >= '{0}' and p.PulloutDate <= '{1}')
    )
",
                                Convert.ToDateTime(this.date1).ToString("yyyyMMdd"),
                                Convert.ToDateTime(this.date2).ToString("yyyyMMdd")));
                        }

                        if (!MyUtility.Check.Empty(this.onBoardDate1))
                        {
                            sqlCmd.Append(string.Format(" and CONVERT(DATE,g.ETD) >= '{0}'", Convert.ToDateTime(this.onBoardDate1).ToString("yyyyMMdd")));
                        }

                        if (!MyUtility.Check.Empty(this.onBoardDate2))
                        {
                            sqlCmd.Append(string.Format(" and CONVERT(DATE,g.ETD) <= '{0}'", Convert.ToDateTime(this.onBoardDate2).ToString("yyyyMMdd")));
                        }

                        if (!MyUtility.Check.Empty(this.brand))
                        {
                            sqlCmd.Append(string.Format(" and g.BrandID = '{0}'", this.brand));
                        }

                        if (!MyUtility.Check.Empty(this.custCD))
                        {
                            sqlCmd.Append(string.Format(" and g.CustCDID = '{0}'", this.custCD));
                        }

                        if (!MyUtility.Check.Empty(this.dest))
                        {
                            sqlCmd.Append(string.Format(" and g.Dest = '{0}'", this.dest));
                        }

                        if (!MyUtility.Check.Empty(this.shipMode))
                        {
                            sqlCmd.Append(string.Format(" and g.ShipModeID = '{0}'", this.shipMode));
                        }

                        if (!MyUtility.Check.Empty(this.forwarder))
                        {
                            sqlCmd.Append(string.Format(" and g.Forwarder = '{0}'", this.forwarder));
                        }

                        if (this.excludePackingFoc)
                        {
                            sqlCmd.Append(string.Format(@" 
and (  
        exists(select 1 from  #tmpPackingA2B_final p WITH (NOLOCK) where p.INVNo = g.ID and p.Type != 'F' )
    )
"));
                        }

                        if (this.excludePackingLocalOrder)
                        {
                            sqlCmd.Append(string.Format(@" 
and (
        exists(select 1 from  #tmpPackingA2B_final p WITH (NOLOCK) where p.INVNo = g.ID and p.Type != 'L' ) 
    )
"));
                        }

                        StringBuilder whereForPLData = new StringBuilder();
                        if (!MyUtility.Check.Empty(this.date1))
                        {
                            whereForPLData.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(this.date1).ToString("yyyyMMdd")));
                        }

                        if (!MyUtility.Check.Empty(this.date2))
                        {
                            whereForPLData.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(this.date2).ToString("yyyyMMdd")));
                        }

                        if (!MyUtility.Check.Empty(this.brand))
                        {
                            whereForPLData.Append(string.Format(" and p.BrandID = '{0}'", this.brand));
                        }

                        if (!MyUtility.Check.Empty(this.custCD))
                        {
                            whereForPLData.Append(string.Format(" and p.CustCDID = '{0}'", this.custCD));
                        }

                        if (!MyUtility.Check.Empty(this.shipMode))
                        {
                            whereForPLData.Append(string.Format(" and p.ShipModeID = '{0}'", this.shipMode));
                        }

                        if (this.excludePackingFoc)
                        {
                            whereForPLData.Append(string.Format(" and p.Type != 'F' "));
                        }

                        if (this.excludePackingLocalOrder)
                        {
                            whereForPLData.Append(string.Format(" and p.Type != 'L' "));
                        }

                        sqlCmd.Append($@"
),PLData as (
    select  IE = 'Export'
			, Type = 'GARMENT'
			, p.ID
			, OnBoardDate = null 
			, p.MDivisionID
            , [Foundry] = ''
			, p.BrandID
			, Category = IIF((select top 1 o.Category 
							  from Orders o WITH (NOLOCK) 
							  	   , #tmpPackingDetailA2B pd WITH (NOLOCK) 
							  where pd.ID = p.ID 
							  		and o.ID = pd.OrderID
							  ) ='B','Bulk','Sample') 
			, p.CustCDID
			, Dest = ''  
			, p.ShipModeID
			, p.PulloutDate
			, p.ShipQty
			, p.CTNQty
			, p.GW
			, p.CBM
			, Forwarder = ''
			, BLNo = ''
			, [NoExportCharges] = ''
	from  #tmpPackingA2B_final p WITH (NOLOCK) 
	where (p.Type = 'F' or p.Type = 'L')
    and not exists (
		  		select 1 
		  		from ShareExpense WITH (NOLOCK) 
		  		where InvNo = p.ID) 
    {whereForPLData}
)");

                        sqlCmd.Append(@"
select * from GBData
union all
select * from PLData");
                    }
                    else if (this.reportType2 == 2)
                    {
                        StringBuilder whereForReportType2 = new StringBuilder();
                        if (!MyUtility.Check.Empty(this.date1))
                        {
                            whereForReportType2.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(this.date1).ToString("yyyyMMdd")));
                        }

                        if (!MyUtility.Check.Empty(this.date2))
                        {
                            whereForReportType2.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(this.date2).ToString("yyyyMMdd")));
                        }

                        if (!MyUtility.Check.Empty(this.onBoardDate1))
                        {
                            whereForReportType2.Append(string.Format(" and CONVERT(DATE,g.ETD) >= '{0}'", Convert.ToDateTime(this.onBoardDate1).ToString("yyyyMMdd")));
                        }

                        if (!MyUtility.Check.Empty(this.onBoardDate2))
                        {
                            whereForReportType2.Append(string.Format(" and CONVERT(DATE,g.ETD) <= '{0}'", Convert.ToDateTime(this.onBoardDate2).ToString("yyyyMMdd")));
                        }

                        if (!MyUtility.Check.Empty(this.brand))
                        {
                            whereForReportType2.Append(string.Format(" and g.BrandID = '{0}'", this.brand));
                        }

                        if (!MyUtility.Check.Empty(this.custCD))
                        {
                            whereForReportType2.Append(string.Format(" and g.CustCDID = '{0}'", this.custCD));
                        }

                        if (!MyUtility.Check.Empty(this.dest))
                        {
                            whereForReportType2.Append(string.Format(" and g.Dest = '{0}'", this.dest));
                        }

                        if (!MyUtility.Check.Empty(this.shipMode))
                        {
                            whereForReportType2.Append(string.Format(" and g.ShipModeID = '{0}'", this.shipMode));
                        }

                        if (!MyUtility.Check.Empty(this.forwarder))
                        {
                            whereForReportType2.Append(string.Format(" and g.Forwarder = '{0}'", this.forwarder));
                        }

                        if (this.excludePackingFoc)
                        {
                            whereForReportType2.Append(string.Format(" and p.Type != 'F' "));
                        }

                        if (this.excludePackingLocalOrder)
                        {
                            whereForReportType2.Append(string.Format(" and p.Type != 'L' "));
                        }
                        #region 組SQL Command

                        sqlCmd.Append($@"
with GBDataA2B
as (
    select 
    	   IE = 'Export'
           , Type = 'GARMENT'
           , g.ID
           , OnBoardDate = g.ETD
           , g.Shipper
		   , [Factory]=Factory.Value
           , [Foundry] = iif(ISNULL(g.Foundry,'0') = '0', '' , 'Y')
           , g.BrandID
           , Category = IIF(p.Type = 'B','Bulk',IIF(p.Type = 'S','Sample','')) 
           , OrderQty = isnull((select sum(a.Qty) 
           						from (
									select distinct oq.Id
										   , oq.Seq
										   , oq.Qty
									from #tmpPackingDetailA2B_final pd WITH (NOLOCK) 
										 , Order_QtyShip oq WITH (NOLOCK) 
									where pd.ID = p.ID 
										  and pd.OrderID = oq.Id 
										  and pd.OrderShipmodeSeq = oq.Seq) a
								),0)
		   , g.CustCDID
		   , g.Dest
		   , g.ShipModeID
		   , PulloutDate = (select MAX(pack.PulloutDate) 
                            from (
                                select PulloutDate from  #tmpPackingA2B_final WITH (NOLOCK) where INVNo = g.ID
                                ) pack
                            )
		   , g.TotalShipQty
		   , g.TotalCTNQty
		   , g.TotalGW
		   , g.TotalCBM
		   , Forwarder = g.Forwarder+'-'+isnull(l.Abb,'')
		   , BLNo = ''
		   , [NoExportCharges] = iif(isnull(g.NoExportCharges,0)=1,'V','')
		   , [PackingListID]=p.id
           , SP=SP.Value
    from GMTBooking g WITH (NOLOCK) 
    inner join #tmpPackingA2B_final p WITH (NOLOCK) on p.INVNo = g.ID
    left join LocalSupp l WITH (NOLOCK) on l.ID = g.Forwarder
	OUTER APPLY (
					SELECT  [Value]= STUFF(
											(
												SELECT Distinct ','+o.FactoryID
												FROM #tmpPackingDetailA2B_final pd WITH (NOLOCK) 
												left join Orders o WITH (NOLOCK) on o.ID = pd.orderID
												WHERE pd.ID = p.ID
												FOR XML PATH('')
											)
	
										, 1, 1, '')	
	)Factory
	OUTER APPLY (
        SELECT  [Value]= STUFF(
            (
                SELECT Distinct ','+pd.OrderID
                FROM #tmpPackingDetailA2B_final pd WITH (NOLOCK) 
                WHERE pd.ID = p.ID
                FOR XML PATH('')
            )
            , 1, 1, '')	
	)SP
    where not exists (
			    select 1 
			    from ShareExpense WITH (NOLOCK) 
			    where InvNo = g.ID) 
            {whereForReportType2}");

                        StringBuilder whereForPLData = new StringBuilder();
                        if (!MyUtility.Check.Empty(this.date1))
                        {
                            whereForPLData.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(this.date1).ToString("yyyyMMdd")));
                        }

                        if (!MyUtility.Check.Empty(this.date2))
                        {
                            whereForPLData.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(this.date2).ToString("yyyyMMdd")));
                        }

                        if (!MyUtility.Check.Empty(this.brand))
                        {
                            whereForPLData.Append(string.Format(" and p.BrandID = '{0}'", this.brand));
                        }

                        if (!MyUtility.Check.Empty(this.custCD))
                        {
                            whereForPLData.Append(string.Format(" and p.CustCDID = '{0}'", this.custCD));
                        }

                        if (!MyUtility.Check.Empty(this.shipMode))
                        {
                            whereForPLData.Append(string.Format(" and p.ShipModeID = '{0}'", this.shipMode));
                        }

                        if (this.excludePackingFoc)
                        {
                            whereForPLData.Append(string.Format(" and p.Type != 'F' "));
                        }

                        if (this.excludePackingLocalOrder)
                        {
                            whereForPLData.Append(string.Format(" and p.Type != 'L' "));
                        }

                        sqlCmd.Append($@"),
GBDataAll as (
    select  IE = 'Export'
			, Type = 'GARMENT'
			, p.ID
			, OnBoardDate = null 
			, p.MDivisionID
		    , [Factory]=Factory.Value
            , [Foundry] = ''
			, p.BrandID
			, Category = IIF((select top 1 o.Category 
							  from Orders o WITH (NOLOCK) 
							  	   , #tmpPackingDetailA2B_final pd WITH (NOLOCK) 
							  where pd.ID = p.ID 
							  		and o.ID = pd.OrderID
							  ) ='B','Bulk','Sample') 
			, OrderQty = isnull((select sum(a.Qty) 
								 from (
								 	select distinct oq.Id
								 		   , oq.Seq
								 		   , oq.Qty 
								 	from #tmpPackingDetailA2B_final pd WITH (NOLOCK) 
								 		 , Order_QtyShip oq WITH (NOLOCK) 
								 	where pd.ID = p.ID 
								 		  and pd.OrderID = oq.Id 
								 		  and pd.OrderShipmodeSeq = oq.Seq
								) a),0)
			, p.CustCDID
			, Dest = ''  
			, p.ShipModeID
			, p.PulloutDate
			, p.ShipQty
			, p.CTNQty
			, p.GW
			, p.CBM
			, Forwarder = ''
			, BLNo = ''
			, [NoExportCharges] = ''
			, [PackingListID]=p.id
            , SP=SP.Value            
	from #tmpPackingA2B_final p WITH (NOLOCK) 
	OUTER APPLY (
					SELECT  [Value]= STUFF(
											(
												SELECT Distinct ','+o.FactoryID
												FROM #tmpPackingDetailA2B_final pd WITH (NOLOCK) 
												left join Orders o WITH (NOLOCK) on o.ID = pd.orderID
												WHERE pd.ID = p.ID
												FOR XML PATH('')
											)
	
										, 1, 1, '')	
	)Factory
	OUTER APPLY (
        SELECT  [Value]= STUFF(
            (
                SELECT Distinct ','+pd.OrderID
                FROM #tmpPackingDetailA2B_final pd WITH (NOLOCK) 
                WHERE pd.ID = p.ID
                FOR XML PATH('')
            )
            , 1, 1, '')	
	)SP
	where (p.Type = 'F' or p.Type = 'L')
    and not exists (
		  		select 1 
		  		from ShareExpense WITH (NOLOCK) 
		  		where InvNo = p.ID)
    {whereForPLData}
");

                        sqlCmd.Append(@")
select * from GBDataA2B
union all
select * from GBDataAll
");
                        #endregion
                    }
                    else
                    {
                        sqlCmd.Append(@"
with GBData
as (
    select 
    	   IE = 'Export'
           , Type = 'GARMENT'
           , g.ID
           , OnBoardDate = g.ETD
           , g.Shipper 
           , [Foundry] = iif(ISNULL(g.Foundry,'0') = '0', '' , 'Y')
           , g.BrandID
           , Category = Stuff((
				select distinct CONCAT(',', pack.Name) 
                from    (
                                select d.Name
				                from    #tmpPackingA2B_final p WITH (NOLOCK),
				                        DropDownList d WITH (NOLOCK) 
				                where   p.INVNo = g.ID and
				                        p.Type = REPLACE(d.ID,'''','') and
				                        d.Type='Pms_ReportCategory'
                        ) pack
				for xml path('')
		   ),1,1,'')
		   , g.CustCDID
		   , g.Dest
		   , g.ShipModeID
		   , PulloutDate = (select MAX(pack.PulloutDate) 
                            from (
                                select PulloutDate from  #tmpPackingA2B_final WITH (NOLOCK) where INVNo = g.ID
                                ) pack
                            )
		   , g.TotalShipQty
		   , g.TotalCTNQty
		   , g.TotalGW
		   , g.TotalCBM
		   , Forwarder = g.Forwarder+'-'+isnull(l.Abb,'')
		   , g.BLNo
		   , g.BL2No
		   , [NoExportCharges] = iif(isnull(g.NoExportCharges,0)=1,'V','')
    from GMTBooking g WITH (NOLOCK) 
    left join LocalSupp l WITH (NOLOCK) on l.ID = g.Forwarder
   outer apply(
	-- 只要cnt = 0, 沒資料 = 就存在
		select cnt = count(1) from (
			select distinct sap.AccountID from ShareExpense se
			inner join ShippingAP_Detail sap WITH (NOLOCK) on sap.ID = se.ShippingAPID
			where 1=1
			and se.InvNo = g.id
			and sap.AccountID in ('61022001')
		) a
	)major1
	outer apply(	
	-- 只要cnt != 2, 代表任何一筆都沒有 = 就存在
		select cnt = count(1) from (
			select distinct sap.AccountID from ShareExpense se
			inner join ShippingAP_Detail sap WITH (NOLOCK) on sap.ID = se.ShippingAPID
			where 1=1
			and se.InvNo = g.id
			and sap.AccountID in ('61092101','61092106')
		) a
	)major2
	outer apply(	
	-- 只要 cnt = 0,代表這三筆都沒有 = 就存在
		select cnt = count(1) from (
			select distinct sap.AccountID from ShareExpense se
			inner join ShippingAP_Detail sap WITH (NOLOCK) on sap.ID = se.ShippingAPID
			where 1=1
			and se.InvNo = g.id
			and sap.AccountID in ('61022003','6102','61021005')
		) a
	)major3
    where 1=1
	and (
			(major1.cnt = 0 and g.ShipModeID in ('A/C','A/P-C','E/C','E/P-C','RAIL','RIVER','S-A/C','SEA','SEA-TRUCK'))
			or 
			(major2.cnt != 2 and g.ShipModeID in ('A/P','E/P','S-A/P'))
			or 
			(major3.cnt = 0) and g.ShipModeID in ('TRUCK')
		)
");

                        if (!MyUtility.Check.Empty(this.date1))
                        {
                            sqlCmd.Append(string.Format(
                                @"
and (
        exists(select 1 from #tmpPackingA2B_final p WITH (NOLOCK) where INVNo = g.ID and p.PulloutDate >= '{0}' and p.PulloutDate <= '{1}')
    )
",
                                Convert.ToDateTime(this.date1).ToString("yyyyMMdd"),
                                Convert.ToDateTime(this.date2).ToString("yyyyMMdd")));
                        }

                        if (!MyUtility.Check.Empty(this.onBoardDate1))
                        {
                            sqlCmd.Append(string.Format(" and CONVERT(DATE,g.ETD) >= '{0}'", Convert.ToDateTime(this.onBoardDate1).ToString("yyyyMMdd")));
                        }

                        if (!MyUtility.Check.Empty(this.onBoardDate2))
                        {
                            sqlCmd.Append(string.Format(" and CONVERT(DATE,g.ETD) <= '{0}'", Convert.ToDateTime(this.onBoardDate2).ToString("yyyyMMdd")));
                        }

                        if (!MyUtility.Check.Empty(this.brand))
                        {
                            sqlCmd.Append(string.Format(" and g.BrandID = '{0}'", this.brand));
                        }

                        if (!MyUtility.Check.Empty(this.custCD))
                        {
                            sqlCmd.Append(string.Format(" and g.CustCDID = '{0}'", this.custCD));
                        }

                        if (!MyUtility.Check.Empty(this.dest))
                        {
                            sqlCmd.Append(string.Format(" and g.Dest = '{0}'", this.dest));
                        }

                        if (!MyUtility.Check.Empty(this.shipMode))
                        {
                            sqlCmd.Append(string.Format(" and g.ShipModeID = '{0}'", this.shipMode));
                        }

                        if (!MyUtility.Check.Empty(this.forwarder))
                        {
                            sqlCmd.Append(string.Format(" and g.Forwarder = '{0}'", this.forwarder));
                        }

                        if (this.excludePackingFoc)
                        {
                            sqlCmd.Append(string.Format(@" 
and (
        exists(select 1 from  #tmpPackingA2B_final p WITH (NOLOCK) where p.INVNo = g.ID and p.Type != 'F' ) 
    )
"));
                        }

                        if (this.excludePackingLocalOrder)
                        {
                            sqlCmd.Append(string.Format(@" 
and (
        exists(select 1 from  #tmpPackingA2B_final p WITH (NOLOCK) where p.INVNo = g.ID and p.Type != 'L' ) 
    )
"));
                        }

                        StringBuilder whereForPLData = new StringBuilder();
                        if (!MyUtility.Check.Empty(this.date1))
                        {
                            whereForPLData.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(this.date1).ToString("yyyyMMdd")));
                        }

                        if (!MyUtility.Check.Empty(this.date2))
                        {
                            whereForPLData.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(this.date2).ToString("yyyyMMdd")));
                        }

                        if (!MyUtility.Check.Empty(this.brand))
                        {
                            whereForPLData.Append(string.Format(" and p.BrandID = '{0}'", this.brand));
                        }

                        if (!MyUtility.Check.Empty(this.custCD))
                        {
                            whereForPLData.Append(string.Format(" and p.CustCDID = '{0}'", this.custCD));
                        }

                        if (!MyUtility.Check.Empty(this.shipMode))
                        {
                            whereForPLData.Append(string.Format(" and p.ShipModeID = '{0}'", this.shipMode));
                        }

                        if (this.excludePackingFoc)
                        {
                            whereForPLData.Append(string.Format(" and p.Type != 'F' "));
                        }

                        if (this.excludePackingLocalOrder)
                        {
                            whereForPLData.Append(string.Format(" and p.Type != 'L' "));
                        }

                        sqlCmd.Append($@"
),GBDataAll as (
select  IE = 'Export'
			, Type = 'GARMENT'
			, p.ID
			, OnBoardDate = null 
			, p.MDivisionID
            , [Foundry] = ''
			, p.BrandID
			, Category = (
				select top 1 d.name 
				from Orders o WITH (NOLOCK) 
					, #tmpPackingDetailA2B_final pd WITH (NOLOCK) 
					, DropDownList d WITH (NOLOCK) 
				where pd.ID = p.ID 
					and o.ID = pd.OrderID
					and o.Category = REPLACE(d.ID,'''','') 
					and d.Type='Pms_ReportCategory'
				) 
			, p.CustCDID
			, Dest = ''  
			, p.ShipModeID
			, p.PulloutDate
			, p.ShipQty
			, p.CTNQty
			, p.GW
			, p.CBM
			, Forwarder = ''
			, BLNo = ''
            , BL2No = ''
			, [NoExportCharges] = ''
	from #tmpPackingA2B_final p WITH (NOLOCK) 
	where (p.Type = 'F' or p.Type = 'L')
    and not exists (
		  		select 1 
		  		from ShareExpense WITH (NOLOCK) 
		  		where InvNo = p.ID) 
        {whereForPLData}
");

                        sqlCmd.Append(@")
select * from GBData
union all
select * from GBDataAll");
                    }
                }
                else
                {
                    #region 組SQL Command
                    sqlCmd.Append(@"
with ExportData as (
	select IE = 'Import'
		   , Type = 'MATERIAL'
		   , e.ID
		   , e.ImportCountry
		   , e.ShipModeID
		   , e.PortArrival
		   , e.WeightKg
		   , e.Cbm
		   , Forwarder = e.Forwarder+'-'+isnull(s.AbbEN,'')
		   , e.Blno
		   , APId1 = (select top 1 ShippingAPID 
		   			  from ShareExpense WITH (NOLOCK) 
		   			  where Blno = e.Blno)
		   , APId2 = (select top 1 ShippingAPID 
		   			  from ShareExpense WITH (NOLOCK) 
		   			  where WKNo = e.ID)
		   , [NoImportChg] = iif(isnull(e.NoImportCharges,0) = 0,'','V')
		   , LoadingOrigin = Concat (e.ExportPort, '-', e.ExportCountry)
		   , DoortoDoorDelivery = iif(dtd1.v = 1 or dtd2.v = 1,'Y','')
		   , e.FactoryID
		   , e.Consignee
           , e.ETA
	from Export e WITH (NOLOCK) 
	left join Supp s WITH (NOLOCK) on s.ID = e.Forwarder
	outer apply(
		select v=1
		from Door2DoorDelivery 
		where ExportPort = e.ExportPort
		      and ExportCountry = e.ExportCountry
		      and ImportCountry =e.ImportCountry
		      and ShipModeID = e.ShipModeID
		      and Vessel =e.Vessel
	)dtd1
	outer apply(
		select v=1
		from Door2DoorDelivery 
		where ExportPort = e.ExportPort
		      and ExportCountry = e.ExportCountry
		      and ImportCountry =e.ImportCountry
		      and ShipModeID = e.ShipModeID
		      and Vessel =''
	)dtd2
	where e.Junk = 0");
                    if (!MyUtility.Check.Empty(this.date1))
                    {
                        sqlCmd.Append(string.Format(" and e.PortArrival >= '{0}'", Convert.ToDateTime(this.date1).ToString("yyyyMMdd")));
                    }

                    if (!MyUtility.Check.Empty(this.date2))
                    {
                        sqlCmd.Append(string.Format(" and e.PortArrival <= '{0}'", Convert.ToDateTime(this.date2).ToString("yyyyMMdd")));
                    }

                    if (!MyUtility.Check.Empty(this.dest))
                    {
                        sqlCmd.Append(string.Format(" and e.ImportCountry = '{0}'", this.dest));
                    }

                    if (!MyUtility.Check.Empty(this.shipMode))
                    {
                        sqlCmd.Append(string.Format(" and e.ShipModeID = '{0}'", this.shipMode));
                    }

                    if (!MyUtility.Check.Empty(this.forwarder))
                    {
                        sqlCmd.Append(string.Format(" and e.Forwarder = '{0}'", this.forwarder));
                    }

                    sqlCmd.Append(@"),
FtyExportData as (
	select IE = IIF(f.Type = 3,'Export','Import')
		   , Type = IIF(f.Type = 1,'3rd Country',IIF(f.Type = 2,'Transfer In',IIF(f.Type = 3,'Transfer Out','Local Purchase')))
		   , f.ID
		   , f.ImportCountry
		   , f.ShipModeID
		   , f.PortArrival
		   , f.WeightKg
		   , f.Cbm
		   , f.Forwarder+'-'+isnull(l.Abb,'') as Forwarder
		   , f.Blno
		   , [NoImportChg] = iif(isNull(f.NoCharges,0) = 1,'V','')
		   , LoadingOrigin = Concat (f.ExportPort, '-', f.ExportCountry)
		   , DoortoDoorDelivery =''
		   , [FactoryID]=''
		   , f.Consignee
           , [ETA] = null
	from FtyExport f WITH (NOLOCK) 
	left join LocalSupp l WITH (NOLOCK) on l.ID = f.Forwarder
	where not exists (
				select 1 
				from ShareExpense WITH (NOLOCK) 
				where WKNo = f.ID)");

                    if (!MyUtility.Check.Empty(this.date1))
                    {
                        sqlCmd.Append(string.Format(" and f.PortArrival >= '{0}'", Convert.ToDateTime(this.date1).ToString("yyyy/MM/dd")));
                    }

                    if (!MyUtility.Check.Empty(this.date2))
                    {
                        sqlCmd.Append(string.Format(" and f.PortArrival <= '{0}'", Convert.ToDateTime(this.date2).ToString("yyyy/MM/dd")));
                    }

                    if (!MyUtility.Check.Empty(this.dest))
                    {
                        sqlCmd.Append(string.Format(" and f.ImportCountry = '{0}'", this.dest));
                    }

                    if (!MyUtility.Check.Empty(this.shipMode))
                    {
                        sqlCmd.Append(string.Format(" and f.ShipModeID = '{0}'", this.shipMode));
                    }

                    if (!MyUtility.Check.Empty(this.forwarder))
                    {
                        sqlCmd.Append(string.Format(" and f.Forwarder = '{0}'", this.forwarder));
                    }

                    sqlCmd.Append(@")

select	IE
		, Type
		, ID
		, ETA
		, FactoryID
		, Consignee
		, '' as Category
		, 0 as OrderQty
		, LoadingOrigin
		, ImportCountry
		, ShipModeID
		, PortArrival
		, 0 as ShipQty
		, 0 as CTNQty
		, WeightKg
		, Cbm
		, Forwarder
		, Blno
		, NoImportChg
		, DoortoDoorDelivery 
        , '' as BrandID
from ExportData
where (Blno <> '' and APId1 is null) 
	  or (Blno = '' and APId2 is null)
union all
select	IE
		, Type
		, ID
		, ETA
		, FactoryID
		, Consignee
		, Category = '' 
		, OrderQty = 0 
		, LoadingOrigin
		, ImportCountry
		, ShipModeID
		, PortArrival
		, ShipQty = 0
		, CTNQty = 0
		, WeightKg
		, Cbm
		, Forwarder
		, Blno
		, NoImportChg
		, DoortoDoorDelivery 
        , BrandID = '' 
from FtyExportData");
                    #endregion
                }

                result = DBProxy.Current.SelectByConn(sqlConnection, sqlCmd.ToString(), out this.printData);
            }

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
            string excelFile = string.Empty;
            if (this.reportType == 1)
            {
                if (this.reportType2 == 1)
                {
                    excelFile = "Shipping_R11_NonSharedListGarment_Main.xltx";
                }
                else if (this.reportType2 == 2)
                {
                    excelFile = "Shipping_R11_NonSharedListGarment.xltx";
                }
                else
                {
                    excelFile = "Shipping_R11_NonSharedListGarment_MajorItem.xltx";
                }
            }
            else
            {
                excelFile = "Shipping_R11_NonSharedListMaterial.xltx";
            }

            bool result = MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltfile: excelFile, headerRow: 1);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString(), "Warning");
            }

            this.HideWaitMessage();
            return true;
        }
    }
}
