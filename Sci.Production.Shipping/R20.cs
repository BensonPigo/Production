using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class R20 : Win.Tems.PrintForm
    {
        private DataTable[] PrintTable;
        private string sqlCmd;
        private List<SqlParameter> paras = new List<SqlParameter>();

        /// <inheritdoc/>
        public R20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            string sqlcmd = @"
select [Type] = NULL, [Category] = ''
union
select [Type] = cast(fe.Type as varchar(2)), 
		(CASE WHEN fe.Type=1 THEN '3rd Country'
			 WHEN fe.Type=2 THEN 'Transfer In'
			 WHEN fe.Type=3 THEN 'Transfer Out'
			 WHEN fe.Type=4 THEN 'Local Purchase' ELSE '' END) as [Category]
from FtyExport fe
group by fe.Type";
            DBProxy.Current.Select(null, sqlcmd, out DataTable category);
            MyUtility.Tool.SetupCombox(this.comboCategory, 2, category);
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.paras.Clear();
            if (this.comboCategory.SelectedValue.ToString() == "3")
            {
                if (!this.dateOnBoardDate.HasValue1 && !this.dateOnBoardDate.HasValue2)
                {
                    MyUtility.Msg.InfoBox("Please input < On Board Date > first!!");
                    return false;
                }
            }
            else if (this.comboCategory.SelectedValue.Empty())
            {
                if (!this.dateArrivePortDate.HasValue1 && !this.dateArrivePortDate.HasValue2 && !this.dateOnBoardDate.HasValue1 && !this.dateOnBoardDate.HasValue2)
                {
                    MyUtility.Msg.InfoBox("Please input < Arrive Port Date > or < On Board Date > first!!");
                    return false;
                }
            }
            else
            {
                if (!this.dateArrivePortDate.HasValue1 && !this.dateArrivePortDate.HasValue2)
                {
                    MyUtility.Msg.InfoBox("Please input < Arrive Port Date > first!!");
                    return false;
                }
            }

            #region Where 條件
            string where = string.Empty;

            if (this.dateArrivePortDate.HasValue1 && this.dateArrivePortDate.HasValue2)
            {
                this.paras.Add(new SqlParameter("@arrivePortDate_1", SqlDbType.Date) { Value = this.dateArrivePortDate.Value1 });
                this.paras.Add(new SqlParameter("@arrivePortDate_2", SqlDbType.Date) { Value = this.dateArrivePortDate.Value2 });
                where = $"and fe.PortArrival between @arrivePortDate_1 AND @arrivePortDate_2 " + Environment.NewLine;
            }

            if (this.dateOnBoardDate.HasValue1 && this.dateOnBoardDate.HasValue2)
            {
                this.paras.Add(new SqlParameter("@dateOnBoardDate_1", SqlDbType.Date) { Value = this.dateOnBoardDate.Value1 });
                this.paras.Add(new SqlParameter("@dateOnBoardDate_2", SqlDbType.Date) { Value = this.dateOnBoardDate.Value2 });
                where = $"and fe.OnBoard between @dateOnBoardDate_1 AND @dateOnBoardDate_2 " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.txtWKno_s.Text))
            {
                this.paras.Add(new SqlParameter("@wk_s", SqlDbType.VarChar, 13) { Value = this.txtWKno_s.Text });
                where += "AND fe.ID >= @wk_s " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.txtWKno_e.Text))
            {
                this.paras.Add(new SqlParameter("@wk_e", SqlDbType.VarChar, 13) { Value = this.txtWKno_e.Text });
                where += "AND fe.ID <= @wk_e " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.txtConsignee.Text))
            {
                this.paras.Add(new SqlParameter("@Consignee", SqlDbType.VarChar, 8) { Value = this.txtConsignee.Text });
                where += "AND fe.Consignee = @Consignee " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.txtLocalSupp.TextBox1.Text))
            {
                this.paras.Add(new SqlParameter("@Shipper", SqlDbType.VarChar, 8) { Value = this.txtLocalSupp.TextBox1.Text });
                where += "AND fe.shipper = @Shipper " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.comboCategory.SelectedValue.ToString()))
            {
                this.paras.Add(new SqlParameter("@Type", SqlDbType.TinyInt) { Value = this.comboCategory.SelectedValue.ToString() });
                where += "AND fe.Type = @Type " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.txtshipmode.Text))
            {
                this.paras.Add(new SqlParameter("@ShipModeID", SqlDbType.VarChar, 10) { Value = this.txtshipmode.Text });
                where += "AND fe.ShipModeID = @ShipModeID " + Environment.NewLine;
            }
            #endregion

            #region SQL
            this.sqlCmd = $@"
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
where 1=1
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
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return DBProxy.Current.Select(null, this.sqlCmd, this.paras, out this.PrintTable);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            this.SetCount(this.PrintTable[0].Rows.Count);
            if (this.PrintTable[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return false;
            }

            this.ShowWaitMessage("Excel processing...");

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Shipping_R20.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.PrintTable[0], string.Empty, "Shipping_R20.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[1]);
            MyUtility.Excel.CopyToXls(this.PrintTable[1], string.Empty, "Shipping_R20.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[2]);

            objApp.Sheets[1].Rows.AutoFit();
            objApp.Sheets[2].Rows.AutoFit();
            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);

            this.HideWaitMessage();
            return true;
        }

        private void ComboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 3. Transfer Out
            if (this.comboCategory.SelectedValue.ToString() == "3")
            {
                this.dateArrivePortDate.ReadOnly = true;
                this.dateOnBoardDate.ReadOnly = false;
                this.dateArrivePortDate.Text1 = string.Empty;
                this.dateArrivePortDate.Text2 = string.Empty;
            }
            else if (this.comboCategory.SelectedValue.Empty())
            {
                this.dateArrivePortDate.ReadOnly = false;
                this.dateOnBoardDate.ReadOnly = false;
            }
            else
            {
                this.dateArrivePortDate.ReadOnly = false;
                this.dateOnBoardDate.ReadOnly = true;
                this.dateOnBoardDate.Text1 = string.Empty;
                this.dateOnBoardDate.Text2 = string.Empty;
            }
        }

        private void R20_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }
    }
}
