using System;
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
        DataTable dataTable;

        public R25(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboMDivision1.SetDefalutIndex(true);
        }

        private void txtfactory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
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

        private void txtfactory_Validating(object sender, CancelEventArgs e)
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
                this.KPIETA1 = ((DateTime)this.datekPIETA.Value1).ToString("d");
                this.KPIETA2 = ((DateTime)this.datekPIETA.Value2).ToString("d");
            }
            else
            {
                this.KPIETA1 = string.Empty;
                this.KPIETA2 = string.Empty;
            }

            if (!MyUtility.Check.Empty(this.dateWhseArrival.Value1))
            {
                this.WhseArrival1 = ((DateTime)this.dateWhseArrival.Value1).ToString("d");
                this.WhseArrival2 = ((DateTime)this.dateWhseArrival.Value2).ToString("d");
            }
            else
            {
                this.WhseArrival1 = string.Empty;
                this.WhseArrival2 = string.Empty;
            }

            if (!MyUtility.Check.Empty(this.dateETA.Value1))
            {
                this.ETA1 = ((DateTime)this.dateETA.Value1).ToString("d");
                this.ETA2 = ((DateTime)this.dateETA.Value2).ToString("d");
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
    e.Blno,
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
    po.styleid,
	ed.PoID,
	seq = ed.Seq1+' '+ed.Seq2,
	ed.Refno,
	[Color] = dbo.GetColorMultipleID(o.BrandID,psd.ColorID) ,
	[Description] = dbo.getmtldesc(ed.POID,ed.seq1,ed.seq2,2,0),
	[MtlType]=case when ed.FabricType = 'F' then 'Fabric'
				   when ed.FabricType = 'A' then 'Accessory' end,
	f.WeaveTypeID,
	ed.suppid,
	[SuppName] = supp.AbbEN,
	ed.UnitId,
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
	TEPPOHandle.Email,
    TEPPOSMR.Email,
    [EditName]=dbo.getTPEPass1(e.EditName)
from Export e
Inner join Export_Detail ed on e.ID = ed.ID
inner join orders o on o.id = ed.poid
left join supp on supp.id = ed.suppid
left join PO_Supp_Detail psd on psd.id = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
left join po on po.id = ed.PoID
left join TPEPass1 TEPPOHandle on TEPPOHandle.id = po.POHandle
left join TPEPass1 TEPPOSMR on TEPPOSMR.id = po.POSMR
left join Fabric f with(nolock)on f.SCIRefno = psd.SCIRefno
OUTER APPLY(
		SELECT [Val] = STUFF((
		SELECT DISTINCT ','+esc.ContainerType
		FROM Export_ShipAdvice_Container esc
		WHERE esc.Export_Detail_Ukey=ed.Ukey
		AND esc.ContainerType <> '' AND esc.ContainerNo <> ''
		FOR XML PATH('')
	),1,1,'')
)ContainerType
OUTER APPLY(
		SELECT [Val] = STUFF((
		SELECT DISTINCT ','+ esc.ContainerNo
		FROM Export_ShipAdvice_Container esc
		WHERE esc.Export_Detail_Ukey=ed.Ukey
		AND esc.ContainerType <> '' AND esc.ContainerNo <> ''
		FOR XML PATH('')
	),1,1,'')
)ContainerNo
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

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.dataTable != null && this.dataTable.Rows.Count > 0)
            {
                this.SetCount(this.dataTable.Rows.Count);
                this.ShowWaitMessage("Excel Processing...");

                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_R25.xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.dataTable, null, "Warehouse_R25.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);
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
