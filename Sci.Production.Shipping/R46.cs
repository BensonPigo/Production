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
    public partial class R46 : Win.Tems.PrintForm
    {
        private DataTable[] PrintTable;
        private string sqlCmd;
        private List<SqlParameter> paras = new List<SqlParameter>();

        /// <inheritdoc/>
        public R46(ToolStripMenuItem menuitem)
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
select [Category] = (CASE WHEN fe.Type=1 THEN '3rd Country'
			 WHEN fe.Type=2 THEN 'Transfer In'
			 WHEN fe.Type=3 THEN 'Transfer Out'
			 WHEN fe.Type=4 THEN 'Local Purchase' ELSE '' END)
	 , SisFtyID as [Sis Fty WK#]
	 , fe.id as [WK#]
	 , vd.Blno as [B/L#]
	 , [Declaration ID] = vd.ID
	 , [Customs Declare#] = vd.DeclareNo
	 , [MaterialType] = fed.MtlTypeID
	 , [RefNo] = fed.Refno
	 , [Desc] = isnull(
		iif(fe.Type = 4,(select Description from LocalItem WITH (NOLOCK) where RefNo = fed.RefNo)
			,(select DescDetail from Fabric WITH (NOLOCK) where SCIRefno = fed.SCIRefNo)),'')
	 --, [ReceivingQty] =  case fe.Type when 1 then [3rd].Qty
		--							  when 2 then TransferIn.Qty
		--							  when 3 then TransferOut.Qty
		--							  when 4 then LocalPo.Qty
		--							  when 5 then mipo.Qty end
	, [ReceivingQty] = fed.Qty
	 , [Unit] = fed.UnitId
	 , [CustomsCode] = vdd.NLCode
	 , [HSCode] = vdd.HSCode
	 , [ContractNo] = vd.VNContractID
	 , Shipper as [Shipper]
	 , Consignee as [Consignee]
	 , fe.ShipModeID as [ShipMode]
	 , CYCFS as [Container Type]
	 , INVNo as [Invoice#]
	 , Vessel as [Vessel]
	 , Packages as [Packages]
	 , fe.NetKg as [N.W.]
	 , fe.WeightKg as [G.W.]
	 , fe.Cbm as [CBM]
	 , OnBoard as [On Board Date]
	 , PortArrival as [Arrive Port Date]
	 , WhseArrival as [Arrive W/H Date]
	 , DocArrival as [Dox Rcv Date]
	 , iif(NoCharges=1,'Y','N') as [No Import Charge]
	 , iif(NonDeclare=1,'Y','N')as [Non Declare]
into #tmp_FtyExport
from FtyExport fe WITH (NOLOCK)
left join FtyExport_Detail fed WITH (NOLOCK) on fe.ID = fed.ID
LEFT JOIN VNImportDeclaration vd WITH(NOLOCK) ON 
(
	(vd.Blno !='' and fe.Blno = vd.Blno) 
	or  
	fe.id = vd.WKNo 
)
AND vd.IsFtyExport = 1
outer apply(
	select 	distinct vddd.NLCode,vdd.HSCode
	from dbo.VNImportDeclaration_Detail vdd
	inner join VNImportDeclaration_Detail_Detail vddd on vdd.ID = vddd.ID and vdd.NLCode = vddd.NLCode
	where vdd.id = vd.ID
	and vddd.Refno = fed.Refno 
) vdd
Outer APPLY (
	select Qty = sum(s.ShipQty + s.ShipFOC)
	from PO_Supp_Detail s
	where s.ID = fed.POID
	and s.SEQ1 = fed.Seq1
	and s.SEQ2 = fed.Seq2
) [3rd]
Outer APPLY (
	select Qty = sum(s.Qty)
	from TransferIn_Detail s
	where s.POID = fed.POID
	and s.SEQ1 = fed.Seq1
	and s.SEQ2 = fed.Seq2
) TransferIn
Outer APPLY (
	select Qty = sum(s.Qty)
	from TransferOut_Detail s
	where s.POID = fed.POID
	and s.SEQ1 = fed.Seq1
	and s.SEQ2 = fed.Seq2
) TransferOut
Outer APPLY (
	select Qty = sum(s.Qty)
	from LocalPO_Detail s
	where s.ID = fed.TransactionID
	and s.POID = fed.POID
	and s.Refno = fed.RefNo
	and SUBSTRING(s.Id+s.ThreadColorID,1,26) = fed.SCIRefno
) LocalPo
Outer APPLY (
	select Qty = sum(mpd.InQty) 
	from dbo.SciMachine_MiscPO mp 
	inner join dbo.SciMachine_MiscPO_Detail mpd on mpd.ID = mp.ID
	left join dbo.SciMachine_Misc m on m.ID = mpd.MiscID
	left join Production.dbo.LocalSupp ls on ls.ID = mp.LocalSuppID	
	where mp.PurchaseFrom = 'L'
	and ls.IsMiscOverseas = 1
	and fed.TransactionID = mpd.id
	and fed.Seq1 = mpd.Seq1
	and fed.Seq2 = mpd.Seq2
)mipo
where 1=1
{where}

select distinct 
	   [Category]
	 , [Sis Fty WK#]
	 , [WK#]
	 , [B/L#]
	 , [Declaration ID]
	 , [Customs Declare#]
	 , [MaterialType] = [MtlTypeID].value
	 , [RefNo] = Refno.value
	 , [Desc]  = [Desc].value
	 , [ReceivingQty] = ReceivingQty.value
	 , [Unit] =  Unit.value
	 , [CustomsCode]
	 , [HSCode]
	 , [ContractNo] 
	 , [Shipper]
	 , [Consignee]
	 , [ShipMode]
	 , [Container Type]
	 , [Invoice#]
	 , [Vessel]
	 , [Packages]
	 , [N.W.]
	 , [G.W.]
	 , [CBM]
	 , [On Board Date]
	 , [Arrive Port Date]
	 , [Arrive W/H Date]
	 , [Dox Rcv Date]
	 , [No Import Charge]
	 , [Non Declare]
into #tmpFinal	 
from #tmp_FtyExport t
outer apply(
	select value = Stuff((
		select concat(',',[MtlTypeID])
		from (
			select distinct [MtlTypeID] =  s.MaterialType
			from #tmp_FtyExport s
			where s.[WK#] = t.[WK#] and s.MaterialType = t.MaterialType
			)s
		for xml path('')
		), 1, 1, '')
)  [MtlTypeID]
outer apply(
	select value = Stuff((
		select concat(',',Refno)
		from (
			select distinct Refno =  s.RefNo
			from #tmp_FtyExport s
			where s.[WK#] = t.[WK#] and s.RefNo = t.RefNo
			)s
		for xml path('')
		), 1, 1, '')
)  Refno
outer apply(
	select value = Stuff((
		select concat(',',[Description])
		from (
			select distinct [Description] = s.[Desc]
			from #tmp_FtyExport s
			where s.[WK#] = t.[WK#] and s.[Desc] = t.[Desc]
			)s
		for xml path('')
		), 1, 1, '')
)  [Desc]
outer apply(
	select value = Stuff((
		select concat(',',[Unit])
		from (
			select distinct [Unit] =  s.[Unit]
			from #tmp_FtyExport s
			where s.[WK#] = t.[WK#] and s.Unit = t.Unit
			)s
		for xml path('')
		), 1, 1, '')
)  [Unit]
outer apply(
	select value = sum(s.ReceivingQty) 
	from #tmp_FtyExport s
	where s.[WK#] = t.[WK#]
	and s.MaterialType = t.MaterialType
	and s.RefNo = t.RefNo
	and s.Unit = t.Unit
) ReceivingQty

select * from #tmpFinal	
order by [WK#]

-- Details
select distinct fed.ID
     , isnull(o.FactoryID,'') as [Prod. Factory]
     , fed.POID as [SP#]
	 , isnull(o.BrandID,'') as [Brand]
	 , o.BuyerDelivery as [Buyer Del.]
	 , o.SciDelivery as [SCI Del.]
	 , (left(fed.Seq1+' ',3)+'-'+fed.Seq2) as Seq
	 , iif(fe.Type = 4,(select Abb from LocalSupp WITH (NOLOCK) where ID = fed.SuppID),(select AbbEN from Supp WITH (NOLOCK) where ID = fed.SuppID)) as [Supplier]
	 , fed.RefNo as [Ref#]
	 , isnull(iif(fe.Type = 4,(select Description from LocalItem WITH (NOLOCK) where RefNo = fed.RefNo),(select DescDetail from Fabric WITH (NOLOCK) where SCIRefno = fed.SCIRefNo)),'') as [Description]
	 , (case when fed.FabricType = 'F' then 'Fabric' when fed.FabricType = 'A' then 'Accessory' else '' end) as [Type]
	 , fed.MtlTypeID
	 , t.[Declaration ID]
	 , t.[Customs Declare#]
	 , t.[CustomsCode] 
	 , t.[HSCode]
	 , t.[ContractNo]
	 , fed.UnitID
	 , fed.Qty
	 , fed.NetKg as [N.W.(kg)]
	 , fed.WeightKg as [N.W.(kg)]
from FtyExport_Detail fed WITH (NOLOCK) 
inner join FtyExport fe WITH (NOLOCK) on fe.ID = fed.ID
inner join #tmpFinal t on t.[WK#] = fed.ID and fed.RefNo = t.RefNo
left join Orders o WITH (NOLOCK) on o.ID = fed.PoID
where 1=1
order by fed.ID,fed.POID

drop table #tmp_FtyExport,#tmpFinal;
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

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Shipping_R46.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.PrintTable[0], string.Empty, "Shipping_R46.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[1]);
            MyUtility.Excel.CopyToXls(this.PrintTable[1], string.Empty, "Shipping_R46.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[2]);

            // 限制欄寬長度
            objApp.Sheets[1].Columns[1].ColumnWidth = 15;
            objApp.Sheets[1].Columns[9].ColumnWidth = 60;
            objApp.Sheets[1].Range[$"I2:I{this.PrintTable[0].Rows.Count + 1}"].WrapText = false;

            objApp.Sheets[2].Columns[10].ColumnWidth = 70;
            objApp.Sheets[2].Range[$"J2:J{this.PrintTable[1].Rows.Count + 1}"].WrapText = false;
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

        private void R46_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }
    }
}
