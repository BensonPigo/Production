using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Sci.Win;

namespace Sci.Production.Shipping
{
    public partial class R20 : Sci.Win.Tems.PrintForm
    {
        private DataTable[] PrintTable;
        private string arrivePortDate_s;
        private string arrivePortDate_e;
        private string wk_s;
        private string wk_e;
        private string consignee;
        private string shipper;
        private string category;
        private string shipMode;

        public R20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable category;
            string sqlcmd = @"
select [Type] = '', [Category] = ''
union
select [Type] = cast(fe.Type as varchar(2)), 
		(CASE WHEN fe.Type=1 THEN '3rd Country'
			 WHEN fe.Type=2 THEN 'Transfer In'
			 WHEN fe.Type=3 THEN 'Transfer Out'
			 WHEN fe.Type=4 THEN 'Local Purchase' ELSE '' END) as [Category]
from FtyExport fe
group by fe.Type";
            DBProxy.Current.Select(null, sqlcmd, out category);
            MyUtility.Tool.SetupCombox(this.comboCategory, 2, category);
        }

        protected override bool ValidateInput()
        {
            if (!this.dateArrivePortDate.HasValue1 || !this.dateArrivePortDate.HasValue2)
            {
                MyUtility.Msg.InfoBox("Please input < Arrive Port Date > first!!");
                return false;
            }

            this.arrivePortDate_s = this.dateArrivePortDate.Value1.Value.ToString("yyyyMMdd");
            this.arrivePortDate_e = this.dateArrivePortDate.Value2.Value.ToString("yyyyMMdd");
            this.wk_s = this.txtWKno_s.Text;
            this.wk_e = this.txtWKno_e.Text;
            this.consignee = this.txtConsignee.Text;
            this.shipper = this.txtLocalSupp.TextBox1.Text;
            this.category = this.comboCategory.SelectedValue.ToString();
            this.shipMode = this.txtshipmode.Text;

            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            string sqlCmd = string.Empty;

            #region Where 條件
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@arrivePortDate_s", this.arrivePortDate_s));
            paras.Add(new SqlParameter("@arrivePortDate_e", this.arrivePortDate_e));
            string where = $"where fe.PortArrival between @arrivePortDate_s AND @arrivePortDate_e " + Environment.NewLine;

            if (!MyUtility.Check.Empty(this.wk_s))
            {
                paras.Add(new SqlParameter("@wk_s", this.wk_s));
                where += "AND fe.ID >= @wk_s " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.wk_e))
            {
                paras.Add(new SqlParameter("@wk_e", this.wk_e));
                where += "AND fe.ID <= @wk_e " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.consignee))
            {
                paras.Add(new SqlParameter("@Consignee", this.consignee));
                where += "AND fe.Consignee = @Consignee " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.shipper))
            {
                paras.Add(new SqlParameter("@Shipper", this.shipper));
                where += "AND fe.shipper = @Shipper " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.category))
            {
                paras.Add(new SqlParameter("@Category", this.category));
                where += "AND fe.Type = @Category " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.shipMode))
            {
                paras.Add(new SqlParameter("@ShipMode", this.shipMode));
                where += "AND fe.ShipModeID = @ShipMode " + Environment.NewLine;
            }
            #endregion

            #region SQL
            sqlCmd = $@"
-- Summary
select (CASE WHEN fe.Type=1 THEN '3rd Country'
			 WHEN fe.Type=2 THEN 'Transfer In'
			 WHEN fe.Type=3 THEN 'Transfer Out'
			 WHEN fe.Type=4 THEN 'Local Purchase' ELSE '' END) as [Category]
	 , SisFtyID as [Sis Fty WK#]
	 , fe.id as [WK#]
	 , Blno as [B/L#]
	 , isnull(BLNo.ID, WKNo.ID) [Declaration ID]
	 , isnull(BLNo.DeclareNo, WKNo.DeclareNo) [Customs Declare#]
	 , Shipper as [Shipper]
	 , Consignee as [Consignee]
	 , ShipModeID as [ShipMode]
	 , CYCFS as [Container Type]
	 , INVNo as [Invoice#]
	 , Vessel as [Vessel]
	 , Packages as [Packages]
	 , NetKg as [N.W.]
	 , WeightKg as [G.W.]
	 , Cbm as [CBM]
	 , OnBoard as [On Board Date]
	 , PortArrival as [Arrive Port Date]
	 , WhseArrival as [Arrive W/H Date]
	 , DocArrival as [Dox Rcv Date]
	 , iif(NoCharges=1,'Y','N') as [No Import Charge]
	 , iif(NonDeclare=1,'Y','N')as [Non Declare]
into #tmp_FtyExport
from FtyExport fe WITH (NOLOCK)
outer apply (
	select ID, DeclareNo
	from VNImportDeclaration WITH (NOLOCK)
	where WKNo = fe.ID and IsFtyExport = 1
)WKNo
outer apply (
	select ID, DeclareNo
	from VNImportDeclaration WITH (NOLOCK)
	where BLNo = fe.Blno and IsFtyExport = 1
)BLNo
{where}

select *
from #tmp_FtyExport fe
order by fe.[WK#]
  
-- Details
select ed.ID
     , isnull(o.FactoryID,'') as [Prod. Factory]
     , ed.POID as [SP#]
	 , isnull(o.BrandID,'') as [Brand]
	 , o.BuyerDelivery as [Buyer Del.]
	 , o.SciDelivery as [SCI Del.]
	 , (left(ed.Seq1+' ',3)+'-'+ed.Seq2) as Seq
	 , iif(fe.Type = 4,(select Abb from LocalSupp WITH (NOLOCK) where ID = ed.SuppID),(select AbbEN from Supp WITH (NOLOCK) where ID = ed.SuppID)) as [Supplier]
	 , ed.RefNo as [Ref#]
	 , isnull(iif(fe.Type = 4,(select Description from LocalItem WITH (NOLOCK) where RefNo = ed.RefNo),(select DescDetail from Fabric WITH (NOLOCK) where SCIRefno = ed.SCIRefNo)),'') as [Description]
	 , (case when ed.FabricType = 'F' then 'Fabric' when ed.FabricType = 'A' then 'Accessory' else '' end) as [Type]
	 , ed.MtlTypeID
	 , ed.UnitID
	 , ed.Qty
	 , ed.NetKg as [N.W.(kg)]
	 , ed.WeightKg as [N.W.(kg)]
from FtyExport_Detail ed WITH (NOLOCK) 
inner join FtyExport fe WITH (NOLOCK) on fe.ID = ed.ID
left join Orders o WITH (NOLOCK) on o.ID = ed.PoID
where exists (select 1 from #tmp_FtyExport where [WK#] = ed.ID)
order by ed.ID,ed.POID

drop table #tmp_FtyExport;
";
            #endregion

            return DBProxy.Current.Select(null, sqlCmd, paras, out this.PrintTable);
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            this.SetCount(this.PrintTable[0].Rows.Count);
            if (this.PrintTable[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return false;
            }

            this.ShowWaitMessage("Excel processing...");

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Shipping_R20.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.PrintTable[0], string.Empty, "Shipping_R20.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[1]); // 將datatable copy to excel
            MyUtility.Excel.CopyToXls(this.PrintTable[1], string.Empty, "Shipping_R20.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[2]); // 將datatable copy to excel

            objApp.Sheets[1].Rows.AutoFit();
            objApp.Sheets[2].Rows.AutoFit();
            #region Save & Show Excel
            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
