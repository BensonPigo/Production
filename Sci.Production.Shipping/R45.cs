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
    /// <summary>
    /// R45
    /// </summary>
    public partial class R45 : Win.Tems.PrintForm
    {
        private DataTable PrintTable;
        private string eta_s;
        private string eta_e;
        private string wk_s;
        private string wk_e;
        private string factory;
        private string consignee;
        private string shipMode;

        /// <summary>
        /// R45
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R45(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (!this.dateETA.HasValue1 || !this.dateETA.HasValue2)
            {
                MyUtility.Msg.InfoBox("Please input <ETA> first!!");
                return false;
            }

            this.eta_s = this.dateETA.Value1.Value.ToString("yyyy/MM/dd");
            this.eta_e = this.dateETA.Value2.Value.ToString("yyyy/MM/dd");
            this.wk_s = this.txtWKno_s.Text;
            this.wk_e = this.txtWKno_e.Text;
            this.factory = this.txtscifactory.Text;
            this.consignee = this.txtConsignee.Text;
            this.shipMode = this.txtshipmode.Text;

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            string sqlCmd = string.Empty;

            #region Where 條件
            List<SqlParameter> paras = new List<SqlParameter>();
            string where = $@" e.Junk = 0 and e.Eta between '{this.eta_s}'AND '{this.eta_e}' 
and (   exists (select 1 from Factory where e.FactoryID = id and IsProduceFty = 1) or
        exists (select 1 
                from Export_Detail ed 
                left join Orders o WITH (NOLOCK) on o.ID = ed.PoID
                outer apply (select [val] = case when ed.PoType = 'M' and ed.FabricType = 'M' 
		                     then (
		                     	select TOP 1 mpo.FactoryID 
		                     	from SciMachine_MachinePO mpo, SciMachine_MachinePO_Detail mpod 
		                     	where mpo.ID = mpod.ID and mpod.ID = ed.PoID and mpod.Seq1 = ed.Seq1 and mpod.seq2 = ed.Seq2
		                     )
                             when ed.PoType = 'M' and ed.FabricType = 'P' 
		                      then (
		                     	select TOP 1 ppo.FactoryID 
		                     	from SciMachine_PartPO ppo, SciMachine_PartPO_Detail ppod 
		                     	where ppo.ID = ppod.ID and ppod.TPEPOID = ed.PoID and ppod.Seq1 = ed.Seq1 and ppod.seq2 = ed.Seq2
		                     ) 
		                     when ed.PoType = 'M' and ed.FabricType = 'O' 
		                     then (
		                     	select TOP 1 mpo.Factoryid 
		                     	from SciMachine_MiscPO mpo, SciMachine_MiscPO_Detail mpod 
		                     	where mpo.ID = mpod.ID and mpod.TPEPOID = ed.PoID and mpod.Seq1 = ed.Seq1 and mpod.seq2 = ed.Seq2
		                     ) 
	                         else o.FactoryID end) FatoryID
                inner join Factory f on f.ID = FatoryID.val and f.IsProduceFty = 1
                where e.ID = ed.ID)
    )";

            if (!MyUtility.Check.Empty(this.wk_s))
            {
                paras.Add(new SqlParameter("@wk_s", this.wk_s));
                where += "AND e.ID >= @wk_s " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.wk_e))
            {
                paras.Add(new SqlParameter("@wk_e", this.wk_e));
                where += "AND e.ID <= @wk_e " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                where += $"AND e.FactoryID = '{this.factory}' " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.consignee))
            {
                paras.Add(new SqlParameter("@Consignee", this.consignee));
                where += "AND e.Consignee = @Consignee " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.shipMode))
            {
                where += $"AND e.ShipModeID = '{this.shipMode}' " + Environment.NewLine;
            }
            #endregion

            #region SQL
            sqlCmd = $@"
SELECT e.ID
,e.Blno
,[MaterialType] = f.MtlTypeID
,[RefNo] = f.Refno
,[Desc] = isnull(f.DescDetail,'')
,[ReceivingQty] =  iif(ed.PoType='M'
                        , (case when ed.FabricType = 'M' then mpo.OrderQty
                                when ed.FabricType = 'P' then ppo.OrderQty 
			                    when ed.FabricType = 'O' then mipo.OrderQty 
			                    else 0 
                            end)
                        ,psd.Qty)
,[Unit] = ed.UnitId
,e.FactoryID
,e.Consignee
,e.ShipModeID
,e.CYCFS
,e.InvNo
,[Payer] = (case when  e.Payer= 'S' then 'By Sci Taipei Office(Sender)'
			when Payer= 'M' then 'By Mill(Sender)'
			when Payer= 'F' then 'By Factory(Receiver)'
			else '' end)
,e.Vessel
,e.ExportPort
,e.ExportCountry
,e.Packages
,e.NetKg
,e.WeightKg
,e.CBM
,e.Eta
,e.PackingArrival
,e.DocArrival
,e.PortArrival
,e.WhseArrival
,[NoImportCharge] =	IIF(e.NoImportCharges=1,'Y','N')
,[Replacement] = IIF(e.Replacement=1,'Y','N')
,[Delay] =	IIF(e.Delay=1,'Y','N')
,[NonDeclare] =	IIF(e.NonDeclare=1,'Y','N')

,[DoorToDoorDelivery] =IIF(Dtdd.DoorToDoorDelivery=1,'Y','N')
,[SQCS] = IIF(e.SQCS=1,'Y','N')
,[RemarkFromTPE] =ISNULL(e.Remark,'')
,[RemarkToTPE] =	ISNULL(e.Remark_Factory,'')
into #tmp
FROM Export e WITH(NOLOCK)
left join Export_Detail ed WITH(NOLOCK) on ed.ID = e.ID
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
OUTER APPLY(
	SELECT 1 as [DoorToDoorDelivery]
	FROM Door2DoorDelivery 
	WHERE ExportPort = e.ExportPort
			and ExportCountry =e.ExportCountry
			and ImportCountry = e.ImportCountry
			and ShipModeID = e.ShipModeID
			and Vessel =e.Vessel
	UNION 
	SELECT 1 as [DoorToDoorDelivery]
	FROM Door2DoorDelivery
	WHERE ExportPort = e.ExportPort
			and ExportCountry =e.ExportCountry
			and ImportCountry = e.ImportCountry
			and ShipModeID = e.ShipModeID
			and Vessel  =''
)Dtdd
Outer APPLY (
	select mpod.ID, mpod.Seq1, mpod.Seq2, mpo.FactoryID, OrderQty = sum(mpod.Qty)
	from SciMachine_MachinePO mpo
	inner join SciMachine_MachinePO_Detail mpod on mpo.ID = mpod.ID 
	where ed.FabricType = 'M'
			and mpod.ID = ed.PoID 
			and mpod.Seq1 = ed.Seq1 
			and mpod.Seq2 = ed.Seq2
	group by  mpod.ID, mpod.Seq1, mpod.Seq2, mpo.FactoryID
)mpo
Outer APPLY (
	select ppod.TPEPOID, ppod.Seq1, ppod.Seq2, ppo.FactoryID, OrderQty = sum(ppod.Qty)
	from SciMachine_PartPO ppo
	inner join SciMachine_PartPO_Detail ppod on ppo.ID = ppod.ID 
	where ed.FabricType = 'P'
		and ppod.TPEPOID = ed.PoID 
		and ppod.Seq1 = ed.Seq1 
		and ppod.Seq2 = ed.Seq2
	group by ppod.TPEPOID, ppod.Seq1, ppod.Seq2, ppo.FactoryID
)ppo
Outer APPLY (
	select mpod.TPEPOID, mpod.Seq1, mpod.Seq2, mpo.Factoryid, OrderQty = sum(mpod.Qty) 
	from SciMachine_MiscPO mpo
	inner join SciMachine_MiscPO_Detail mpod on mpo.ID = mpod.ID 
	where ed.FabricType = 'O'
		and mpod.TPEPOID = ed.PoID 
		and mpod.Seq1 = ed.Seq1 
		and mpod.Seq2 = ed.Seq2      
	group by mpod.TPEPOID, mpod.Seq1, mpod.Seq2, mpo.Factoryid
)mipo
WHERE {where}


select distinct
t.ID
,t.Blno
,[DeclarationID] = vd.ID
,DeclareNo
,[MaterialType] = [MtlTypeID].value
,[RefNo] = vddd.Refno
,[Desc]  = [Desc].value
,[ReceivingQty] = isnull(ReceivingQty.value,0)
,[Unit] = Unit.value
,[CustomsCode] = vddd.NLCode
,[HSCode] = vdd.HSCode
,[Customs Qty] = ROUND(vddd.Qty,2)
,[Customs Unit] = vdd.UnitID
,[ContractNo] = vd.VNContractID
,FactoryID
,Consignee
,t.ShipModeID
,CYCFS
,InvNo
,[Payer]
,Vessel
,ExportPort
,ExportCountry
,Packages
,NetKg
,WeightKg
,CBM
,Eta
,PackingArrival
,DocArrival
,PortArrival
,WhseArrival
,[NoImportCharge]
,[Replacement]
,[Delay]
,[NonDeclare]
,[DoorToDoorDelivery]
,[SQCS]
,[RemarkFromTPE]
from #tmp t
LEFT JOIN VNImportDeclaration vd WITH(NOLOCK) ON 
(
	(vd.Blno !='' and t.Blno = vd.Blno) 
	or  	
	(vd.BLNo = '' and vd.WKNo = t.ID)
)　AND vd.IsFtyExport = 0　and vd.Status = 'Confirmed'
left join VNImportDeclaration_Detail vdd on vdd.ID = vd.ID
inner join VNImportDeclaration_Detail_Detail vddd on vddd.ID = vd.ID and vddd.NLCode = vdd.NLCode and t.Refno = vddd.Refno
outer apply(
	select value = Stuff((
		select concat(',',[MtlTypeID])
		from (
			select distinct [MtlTypeID] =  s.MaterialType
			from #tmp s
			where s.ID = t.id and s.MaterialType = t.MaterialType
			)s
		for xml path('')
		), 1, 1, '')
)  [MtlTypeID]
outer apply(
	select value = Stuff((
		select concat(',',[Description])
		from (
			select distinct [Description] = s.[Desc]
			from #tmp s
			where s.ID = t.id and s.[Desc] = t.[Desc]
			)s
		for xml path('')
		), 1, 1, '')
)  [Desc]
outer apply(
	select value = Stuff((
		select concat(',',[Unit])
		from (
			select distinct [Unit] =  s.[Unit]
			from #tmp s
			where s.ID = t.id and s.Unit = t.Unit
			)s
		for xml path('')
		), 1, 1, '')
)  [Unit]
outer apply(
	select value = sum(s.ReceivingQty) 
	from #tmp s
	where s.ID = t.ID
	and s.MaterialType = t.MaterialType
	and s.RefNo = t.RefNo
	and s.Unit = t.Unit
) ReceivingQty
ORDER BY FactoryID, ID

drop table #tmp
";
            #endregion

            return DBProxy.Current.Select(null, sqlCmd, paras, out this.PrintTable);
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(ReportDefinition report)
        {
            this.SetCount(this.PrintTable.Rows.Count);
            if (this.PrintTable.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return false;
            }

            this.ShowWaitMessage("Excel processing...");

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Shipping_R45.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.PrintTable, string.Empty, "Shipping_R45.xltx", 1, false, null, objApp); // 將datatable copy to excel

            #region Save & Show Excel

            // Desc 固定欄寬&取消自動換列 避免資料太多導致整欄高度上長
            objApp.Columns[7].ColumnWidth = 60;
            objApp.Sheets[1].Range[$"G2:G{this.PrintTable.Rows.Count + 1}"].WrapText = false;
            objApp.Visible = true;

            Marshal.ReleaseComObject(objApp);

            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
