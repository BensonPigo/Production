using System;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R14
    /// </summary>
    public partial class R14 : Win.Tems.PrintForm
    {
        private DataTable printData;

        private string ShipPlanID1;
        private string ShipPlanID2;
        private DateTime? InvoiceDate1;
        private DateTime? InvoiceDate2;
        private DateTime? PulloutDate1;
        private DateTime? PulloutDate2;
        private DateTime? ETD1;
        private DateTime? ETD2;
        private string ShipMode;
        private string Status;
        private string Brand;
        private int Type;

        /// <summary>
        /// R14
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtshipmode.SelectedIndex = -1;
            this.cmbStatus.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.ShipPlanID1 = this.txtShipPlanID1.Text;
            this.ShipPlanID2 = this.txtShipPlanID2.Text;
            this.Brand = this.txtbrand.Text;
            this.InvoiceDate1 = this.dateInvoice.Value1;
            this.InvoiceDate2 = this.dateInvoice.Value2;
            this.PulloutDate1 = this.datePulloutDate.Value1;
            this.PulloutDate2 = this.datePulloutDate.Value2;
            this.ETD1 = this.dateETD.Value1;
            this.ETD2 = this.dateETD.Value2;
            this.ShipMode = this.txtshipmode.Text;
            this.Status = this.cmbStatus.Text;
            this.Type = this.rdbtnDetail.Checked ? 1 : this.rdbtnSummary.Checked ? 2 : 3;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region where
            string where = string.Empty;

            if (!MyUtility.Check.Empty(this.ShipPlanID1))
            {
                where += $" and s.ID >= '{this.ShipPlanID1}' ";
            }

            if (!MyUtility.Check.Empty(this.ShipPlanID2))
            {
                where += $" and s.ID <= '{this.ShipPlanID2}' ";
            }

            if (!MyUtility.Check.Empty(this.Brand))
            {
                where += $" and g.BrandID = '{this.Brand}' ";
            }

            if (!MyUtility.Check.Empty(this.InvoiceDate1))
            {
                where += $" and g.InvDate >= '{((DateTime)this.InvoiceDate1).ToString("yyyy/MM/dd")}' ";
            }

            if (!MyUtility.Check.Empty(this.InvoiceDate2))
            {
                where += $" and g.InvDate <=  '{((DateTime)this.InvoiceDate2).ToString("yyyy/MM/dd")}' ";
            }

            if (!MyUtility.Check.Empty(this.PulloutDate1))
            {
                where += $@" 
 and exists (
	select 1
	from PackingList p with(nolock)
	where p.ShipPlanID = s.ID and p.InvNo = g.ID
	and p.PulloutDate between '{((DateTime)this.PulloutDate1).ToString("yyyy/MM/dd")}'and '{((DateTime)this.PulloutDate2).ToString("yyyy/MM/dd")}'
)
";
            }

            if (!MyUtility.Check.Empty(this.ETD1))
            {
                where += $" g.ETD >= '{((DateTime)this.ETD1).ToString("yyyy/MM/dd")}' ";
            }

            if (!MyUtility.Check.Empty(this.ETD2))
            {
                where += $" g.ETD <=  '{((DateTime)this.ETD2).ToString("yyyy/MM/dd")}' ";
            }

            if (!MyUtility.Check.Empty(this.ShipMode))
            {
                where += $" and g.ShipModeID = '{this.ShipMode}' ";
            }

            if (!MyUtility.Convert.GetString(this.Status).ToLower().EqualString("all"))
            {
                where += $" and s.Status = '{this.Status}' ";
            }
            #endregion
            string sqlCmd = string.Empty;
            #region Detail
            if (this.Type == 1)
            {
                sqlCmd = $@"
select s.ID,g.ID,g.BrandID,g.ShipModeID,g.Forwarder,g.CYCFS,g.CutOffDate,
	gg.ct,s.Status,
	pp.TTLShipQty,pp.TTLCTNQty,pp.TTLCBM,
	pc.ct
from ShipPlan s with(nolock)
left join GMTBooking g with(nolock) on g.ShipPlanID = s.ID
outer apply(
	select TTLShipQty=sum(p.ShipQty),TTLCTNQty=sum(p.CTNQty),TTLCBM=sum(p.CBM)
	from PackingList p with(nolock)
	where p.ShipPlanID = s.ID and p.InvNo = g.ID
)pp
outer apply(
	select ct = sum(pd.ctnQty)
	from PackingList p with(nolock)
	inner join PackingList_Detail pd with(nolock) on pd.id = p.id
	where p.ShipPlanID = s.ID 
            and p.InvNo = g.ID 
            and pd.ReceiveDate is not null 
            and ReturnDate is null
)pc
outer apply(
	select ct = count(1)
	from(
		select distinct gc.CTNRNo
		from GMTBooking_CTNR gc with(nolock)
		where gc.id = g.ID
	)a
)gg
where 1=1
{where}
order by g.BrandID,s.ID,g.ID,g.ShipModeID,g.CYCFS
";
            }
            #endregion
            #region Summary
            if (this.Type == 2)
            {
                sqlCmd = $@"select g.BrandID,g.ID,gcc.Type,gcc.ct,pp.TTLShipQty,pp.TTLCBM
into #tmp
from ShipPlan s with(nolock)
left join GMTBooking g with(nolock) on g.ShipPlanID = s.ID
outer apply(
	select gc.Type,ct = count(1)
	from GMTBooking_CTNR gc with(nolock)
	where gc.ID = g.ID
	group by gc.Type
)gcc
outer apply(
	select TTLShipQty=sum(p.ShipQty),TTLCBM=sum(p.CBM)
	from PackingList p with(nolock)
	where p.ShipPlanID = s.ID and p.InvNo = g.ID 
)pp
outer apply(
	select p.PulloutDate
	from PackingList p with(nolock)
	where p.ShipPlanID = s.ID and p.InvNo = g.ID
)ppd
where 1=1
{where}

select g.BrandID
into #tmpa
from ShipPlan s with(nolock)
left join GMTBooking g with(nolock) on g.ShipPlanID = s.ID
where 1=1
{where}

select a.*,
	isnull(CFS.GBct,0),isnull(CFS.TTLShipQty,0),isnull(CFS.TTLCBM,0),CFS=0,
	isnull(STD20.GBct,0),isnull(STD20.TTLShipQty,0),isnull(STD20.TTLCBM,0),STD20=0,
	isnull(STD40.GBct,0),isnull(STD40.TTLShipQty,0),isnull(STD40.TTLCBM,0),STD40=0,
	isnull(HQ40.GBct,0),isnull(HQ40.TTLShipQty,0),isnull(HQ40.TTLCBM,0),HQ40=0,
	isnull(HQ45.GBct,0),isnull(HQ45.TTLShipQty,0),isnull(HQ45.TTLCBM,0),HQ45=0,
	isnull(AIR.GBct,0),isnull(AIR.TTLShipQty,0),isnull(AIR.TTLCBM,0),AIR=0
from(
    select t.BrandID,GBct = COUNT(1)
    from #tmpa t
    group by t.BrandID
)a
left join(
	select t.BrandID,GBct = COUNT(ID),TTLShipQty=sum(TTLShipQty),TTLCBM=sum(TTLCBM)
	from #tmp t
	where t.Type = 'CFS'
	group by t.BrandID
)CFS on a.BrandID = CFS.BrandID
left join(
	select t.BrandID,GBct = COUNT(ID),TTLShipQty=sum(TTLShipQty),TTLCBM=sum(TTLCBM)
	from #tmp t
	where t.Type = '20 STD'
	group by t.BrandID
)STD20 on a.BrandID = STD20.BrandID
left join(
	select t.BrandID,GBct = COUNT(ID),TTLShipQty=sum(TTLShipQty),TTLCBM=sum(TTLCBM)
	from #tmp t
	where t.Type = '40 STD'
	group by t.BrandID	
)STD40 on a.BrandID = STD40.BrandID
left join(
	select t.BrandID,GBct = COUNT(ID),TTLShipQty=sum(TTLShipQty),TTLCBM=sum(TTLCBM)
	from #tmp t
	where t.Type = '40HQ'
	group by t.BrandID	
)HQ40 on a.BrandID = HQ40.BrandID
left join(
	select t.BrandID,GBct = COUNT(ID),TTLShipQty=sum(TTLShipQty),TTLCBM=sum(TTLCBM)
	from #tmp t
	where t.Type = '45HQ'
	group by t.BrandID	
)HQ45 on a.BrandID = HQ45.BrandID
left join(
	select t.BrandID,GBct = COUNT(ID),TTLShipQty=sum(TTLShipQty),TTLCBM=sum(TTLCBM)
	from #tmp t
	where t.Type = 'AIR'
	group by t.BrandID	
)AIR on a.BrandID = AIR.BrandID
drop table #tmp,#tmpa
";
            }
            #endregion
            #region Container Detail
            if (this.Type == 3)
            {
                sqlCmd = $@"
select s.ID,g.ID,g.CYCFS,gc.Type,gc.CTNRNo,gc.SealNo,gc.TruckNo,g.SONo
from ShipPlan s with(nolock)
inner join GMTBooking g with(nolock) on g.ShipPlanID = s.ID
inner join GMTBooking_CTNR gc with(nolock) on gc.ID = g.ID
outer apply(
	select p.PulloutDate
	from PackingList p with(nolock)
	where p.ShipPlanID = s.ID and p.InvNo = g.ID
)ppd
where 1=1
{where}
order by s.ID,g.ID
";
            }
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.printData);
            return result;
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
            string excelName = this.Type == 1 ? "Shipping_R14_Detail" : this.Type == 2 ? "Shipping_R14_Summary" : "Shipping_R14_ContainerDetail";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{excelName}.xltx");
            int startrow = this.Type == 1 ? 1 : this.Type == 2 ? 2 : 1;
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, $"{excelName}.xltx", startrow, false, null, excelApp, wSheet: excelApp.Sheets[1]);

            #region 釋放上面開啟過excel物件
            string strExcelName = Class.MicrosoftFile.GetName(excelName);
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();

            if (excelApp != null)
            {
                Marshal.FinalReleaseComObject(excelApp);
            }
            #endregion

            this.HideWaitMessage();
            strExcelName.OpenFile();
            return true;
        }
    }
}
