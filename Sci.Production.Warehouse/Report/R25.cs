﻿using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class R25 : Win.Tems.PrintForm
    {
        private string KPIETA1;
        private string KPIETA2;
        private string WhseArrival1;
        private string WhseArrival2;
        private string ETA1;
        private string ETA2;
        private string WK1;
        private string WK2;
        private string SP1;
        private string SP2;
        private string Brand;
        private string Supplier;
        private string M;
        private string Factorys;
        private string MtlType;
        private bool RecLessArv;
        private DataTable dataTable;

        public R25(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboMDivision1.SetDefalutIndex(true);
        }

        private void Txtfactory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "select distinct ID from Factory WITH (NOLOCK) order by ID";
            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sqlWhere, "Factory", "10", this.txtfactory.Text, null, null, null);

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtfactory.Text = item.GetSelectedString();
        }

        private void Txtfactory_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtfactory.Text))
            {
                return;
            }

            string[] str_multi = this.txtfactory.Text.Split(',');
            string err_factory = string.Empty;
            foreach (string chk_str in str_multi)
            {
                if (MyUtility.Check.Seek(chk_str, "Factory", "ID", "Production") == false)
                {
                    err_factory += "," + chk_str;
                }
            }

            if (!err_factory.Equals(string.Empty))
            {
                this.txtfactory.Text = string.Empty;
                e.Cancel = true;
                MyUtility.Msg.WarningBox(string.Format("< Factory : {0} > not found!!!", err_factory.Substring(1)));
                return;
            }
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.datekPIETA.Value1) &&
                MyUtility.Check.Empty(this.dateWhseArrival.Value1) &&
                MyUtility.Check.Empty(this.dateETA.Value1) &&
                (MyUtility.Check.Empty(this.txtWK1.Text) && MyUtility.Check.Empty(this.txtWK2.Text)) &&
                (MyUtility.Check.Empty(this.txtSP1.Text) && MyUtility.Check.Empty(this.txtSP2.Text)))
            {
                MyUtility.Msg.WarningBox("KPI L/ETA, Arrive W/H, ETA, WK#, SP# can not all empty!");
                return false;
            }

            if (!MyUtility.Check.Empty(this.datekPIETA.Value1))
            {
                this.KPIETA1 = ((DateTime)this.datekPIETA.Value1).ToString("yyyy/MM/dd");
                this.KPIETA2 = ((DateTime)this.datekPIETA.Value2).ToString("yyyy/MM/dd");
            }
            else
            {
                this.KPIETA1 = string.Empty;
                this.KPIETA2 = string.Empty;
            }

            if (!MyUtility.Check.Empty(this.dateWhseArrival.Value1))
            {
                this.WhseArrival1 = ((DateTime)this.dateWhseArrival.Value1).ToString("yyyy/MM/dd");
                this.WhseArrival2 = ((DateTime)this.dateWhseArrival.Value2).ToString("yyyy/MM/dd");
            }
            else
            {
                this.WhseArrival1 = string.Empty;
                this.WhseArrival2 = string.Empty;
            }

            if (!MyUtility.Check.Empty(this.dateETA.Value1))
            {
                this.ETA1 = ((DateTime)this.dateETA.Value1).ToString("yyyy/MM/dd");
                this.ETA2 = ((DateTime)this.dateETA.Value2).ToString("yyyy/MM/dd");
            }
            else
            {
                this.ETA1 = string.Empty;
                this.ETA2 = string.Empty;
            }

            this.WK1 = this.txtWK1.Text;
            this.WK2 = this.txtWK2.Text;
            this.SP1 = this.txtSP1.Text;
            this.SP2 = this.txtSP2.Text;
            this.Brand = this.txtbrand1.Text;
            this.Supplier = this.txtsupplier1.TextBox1.Text;
            this.M = this.comboMDivision1.Text;
            this.RecLessArv = this.chkRecLessArv.Checked;

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                this.Factorys = "'" + this.txtfactory.Text.Replace(",", "','") + "'";
            }
            else
            {
                this.Factorys = string.Empty;
            }

            this.MtlType = this.comboDropDownList1.SelectedValue.ToString();

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region Set SQL Command & SQLParameter
            string strSql = @"
select
	WK=ed.ID,
	e.eta,
	o.FactoryID,
    e.Consignee,
    e.ShipModeID,
    e.CYCFS,
    e.Blno,
    e2.Packages,
    e.Vessel,
    [ProdFactory]=o.FactoryID,
    o.OrderTypeID,
	o.ProjectID,
	Category = case when o.Category = 'B' then 'Bulk'
					when o.Category = 'M' then 'Material'
					when o.Category = 'S' then 'Sample'
					when o.Category = 'T' then 'SMLT'
					when o.Category = 'G' then 'Garment'
					when o.Category = 'O' then 'Other'
	end,
	o.BrandID,
    o.seasonid,
    po.styleid,
    s.StyleName,
	ed.PoID,
	seq = ed.Seq1+' '+ed.Seq2,
	ed.Refno,
	[Color] = dbo.GetColorMultipleID(o.BrandID,psd.ColorID) ,
	[Description] = dbo.getmtldesc(ed.POID,ed.seq1,ed.seq2,2,0),
	[MtlType]=case when ed.FabricType = 'F' then 'Fabric'
				   when ed.FabricType = 'A' then 'Accessory' end,
    f.MtlTypeID,
	f.WeaveTypeID,
	ed.suppid,
	[SuppName] = supp.AbbEN,
	ed.UnitId,
	psd.StockUnit,
    psd.SizeSpec,
    [ShipQty]=ed.Qty,
    ed.FOC,
    ed.NetKg,
    ed.WeightKg,
	[ArriveQty]=isnull(ed.Qty,0)+isnull(ed.foc,0),
    [ContainerType] = ContainerType.Val,
    [ContainerNo] = ContainerNo.Val,
    e.PortArrival,
	e.WhseArrival,
	o.KPILETA,
	 (SELECT MinSciDelivery FROM DBO.GetSCI(ed.Poid,o.Category)) as [Earliest SCI Delivery],
	EarlyDays=DATEDIFF(day,e.WhseArrival,o.KPILETA),
	[MR_Mail]=TEPPOHandle.Email,
    [SMR_Mail]=TEPPOSMR.Email,
    [EditName]=dbo.getTPEPass1(e.EditName),
	t.TotalRollsCalculated
INTO #tmp
from Export e
Inner join Export_Detail ed on e.ID = ed.ID
inner join orders o on o.id = ed.poid
left join Style s with (nolock) on s.Ukey = o.StyleUkey
left join supp on supp.id = ed.suppid
left join PO_Supp_Detail psd on psd.id = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
left join po on po.id = ed.PoID
left join TPEPass1 TEPPOHandle on TEPPOHandle.id = po.POHandle
left join TPEPass1 TEPPOSMR on TEPPOSMR.id = po.POSMR
left join Fabric f with(nolock)on f.SCIRefno = psd.SCIRefno
outer apply (
    select [Packages] = sum(e2.Packages)
    from Export e2 with (nolock) 
    where e2.Blno = e.Blno
)e2
OUTER APPLY(
		SELECT [Val] = STUFF((
		SELECT DISTINCT ','+esc.ContainerType
		FROM Export_ShipAdvice_Container esc
		WHERE esc.Export_DetailUkey=ed.Ukey
		AND esc.ContainerType <> '' AND esc.ContainerNo <> ''
		FOR XML PATH('')
	),1,1,'')
)ContainerType
OUTER APPLY(
		SELECT [Val] = STUFF((
		SELECT DISTINCT ','+esc.ContainerNo
		FROM Export_ShipAdvice_Container esc
		WHERE esc.Export_DetailUkey=ed.Ukey
		AND esc.ContainerType <> '' AND esc.ContainerNo <> ''
		FOR XML PATH('')
	),1,1,'')
)ContainerNo
cross apply(
	select [TotalRollsCalculated] = count(1)
	from dbo.View_AllReceivingDetail rd WITH (NOLOCK) 
	where rd.PoId = ed.POID and rd.Seq1 = ed.SEQ1 and rd.Seq2 = ed.SEQ2 AND ed.FabricType='F'
	group by rd.WhseArrival,rd.InvNo,rd.ExportId,rd.Id,rd.PoId,RD.seq1,RD.seq2,rd.StockType
) t
where exists (select 1 from Factory where o.FactoryId = id and IsProduceFty = 1)
and ed.PoType = 'G' 

";
            if (!MyUtility.Check.Empty(this.KPIETA1))
            {
                strSql += $@" and o.KPILETA between '{this.KPIETA1}' and '{this.KPIETA2}' ";
            }

            if (!MyUtility.Check.Empty(this.WhseArrival1))
            {
                strSql += $@" and e.WhseArrival between '{this.WhseArrival1}' and '{this.WhseArrival2}' ";
            }

            if (!MyUtility.Check.Empty(this.ETA1))
            {
                strSql += $@" and e.eta between '{this.ETA1}' and '{this.ETA2}' ";
            }

            if (!MyUtility.Check.Empty(this.WK1))
            {
                strSql += $@" and ed.ID >= '{this.WK1}' ";
            }

            if (!MyUtility.Check.Empty(this.WK2))
            {
                strSql += $@" and ed.ID <= '{this.WK2}' ";
            }

            if (!MyUtility.Check.Empty(this.SP1))
            {
                strSql += $@" and ed.PoID >= '{this.SP1}' ";
            }

            if (!MyUtility.Check.Empty(this.SP2))
            {
                strSql += $@" and ed.PoID <= '{this.SP2}' ";
            }

            if (!MyUtility.Check.Empty(this.Brand))
            {
                strSql += $@" and o.BrandID = '{this.Brand}' ";
            }

            if (!MyUtility.Check.Empty(this.Supplier))
            {
                strSql += $@" and ed.suppid = '{this.Supplier}' ";
            }

            if (!MyUtility.Check.Empty(this.M))
            {
                strSql += $@" and o.MDivisionID = '{this.M}' ";
            }

            if (!MyUtility.Check.Empty(this.Factorys))
            {
                strSql += $@" and o.FactoryID in ({this.Factorys}) ";
            }

            strSql += $@" and ed.FabricType in ({this.MtlType})";

            strSql += $@"

 select 
	WK, t.eta, t.FactoryID, Consignee, ShipModeID, CYCFS, Blno, Packages, Vessel, [ProdFactory], OrderTypeID, ProjectID, Category ,
	BrandID, seasonid, styleid, t.StyleName, t.PoID, seq, Refno,	[Color] , [Description], [MtlType],MtlTypeID ,WeaveTypeID, suppid, [SuppName] 
	, UnitId
	, SizeSpec,
    [ShipQty]=SUM(t.ShipQty),
    [FOC]=SUM(FOC),
    [NetKg]=SUM(NetKg),
    [WeightKg]=SUM(WeightKg),
    [ArriveQty]=  SUM(ArriveQty),
    [ArriveQty_StockUnit]=dbo.[GetUnitQty](UnitId,StockUnit,SUM(ArriveQty)),
	Receiving_Detail.ReceiveQty,
    t.TotalRollsCalculated,
	StockUnit,
    [ContainerType] ,[ContainerNo] ,PortArrival,t.WhseArrival,KPILETA,
	[Earliest SCI Delivery],
	EarlyDays,
	[MR_Mail],
    [SMR_Mail],
    t.EditName
 from #tmp t
OUTER APPLY(
	SELECT [ReceiveQty]=SUM(rd.StockQty)
	FROM Receiving r 
	INNER JOIN Receiving_Detail rd ON rd.ID = r.ID AND rd.PoId = t.PoID ANd rd.Seq1 + ' ' +rd.Seq2 = t.Seq
	WHERE t.WK = r.ExportId AND r.Status = 'Confirmed'
)Receiving_Detail
 GROUP BY 
	WK,t.eta,t.FactoryID,Consignee,ShipModeID,CYCFS,Blno,Packages,Vessel,[ProdFactory],OrderTypeID,ProjectID,Category ,BrandID, seasonid,styleid,t.PoID,seq,
	Refno,[Color] ,[Description],[MtlType],WeaveTypeID,suppid,[SuppName] ,UnitId,SizeSpec,[ContainerType] ,[ContainerNo] ,PortArrival,
    t.WhseArrival,KPILETA,[Earliest SCI Delivery],EarlyDays,[MR_Mail],[SMR_Mail],t.EditName,ReceiveQty,StockUnit, t.StyleName, t.TotalRollsCalculated, MtlTypeID
HAVING 1=1
";
            if (this.RecLessArv)
            {
                strSql += $@" AND  (Receiving_Detail.ReceiveQty is null or (Receiving_Detail.ReceiveQty < dbo.[GetUnitQty](UnitId,StockUnit,SUM(ArriveQty))))  ";
            }

            strSql += $@"drop table #tmp";

            #endregion
            #region SQL Data Loading...
            DualResult result = DBProxy.Current.Select(null, strSql,  out this.dataTable);
            #endregion

            if (result)
            {
                return Ict.Result.True;
            }
            else
            {
                return new DualResult(false, "Query data fail\r\n" + result.ToString());
            }
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.dataTable != null && this.dataTable.Rows.Count > 0)
            {
                this.SetCount(this.dataTable.Rows.Count);
                this.ShowWaitMessage("Excel Processing...");

                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_R25.xltx"); // 預先開啟excel app
                Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R25.xltx", objApp);
                com.ColumnsAutoFit = false;
                com.WriteTable(this.dataTable, 2);

                Excel.Worksheet worksheet = objApp.Sheets[1];

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Warehouse_R25");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
                #endregion
                this.HideWaitMessage();
            }
            else
            {
                this.SetCount(0);
                MyUtility.Msg.InfoBox("Data not found!!");
            }

            return true;
        }
    }
}
