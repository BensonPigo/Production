using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Ict;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class R25 : Sci.Win.Tems.PrintForm
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
            InitializeComponent();
            this.comboMDivision1.setDefalutIndex(true);
        }

        private void txtfactory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "select distinct ID from Factory WITH (NOLOCK) order by ID";
            Sci.Win.Tools.SelectItem2 item = new Sci.Win.Tools.SelectItem2(sqlWhere, "Factory", "10", this.txtfactory.Text, null, null, null);

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) return;
            this.txtfactory.Text = item.GetSelectedString();
        }

        private void txtfactory_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtfactory.Text))
            {
                return;
            }
            string[] str_multi = this.txtfactory.Text.Split(',');
            string err_factory = "";
            foreach (string chk_str in str_multi)
            {
                if (MyUtility.Check.Seek(chk_str, "Factory", "ID", "Production") == false)
                {
                    err_factory += "," + chk_str;
                }
            }

            if (!err_factory.Equals(""))
            {
                this.txtfactory.Text = "";
                e.Cancel = true;
                MyUtility.Msg.WarningBox(string.Format("< Factory : {0} > not found!!!", err_factory.Substring(1)));
                return;
            }
        }

        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(datekPIETA.Value1) &&
                MyUtility.Check.Empty(dateWhseArrival.Value1) &&
                MyUtility.Check.Empty(dateETA.Value1) &&
                (MyUtility.Check.Empty(txtWK1.Text) && MyUtility.Check.Empty(txtWK2.Text)) &&
                (MyUtility.Check.Empty(txtSP1.Text) && MyUtility.Check.Empty(txtSP2.Text))
                )
            {
                MyUtility.Msg.WarningBox("KPI L/ETA, Arrive W/H, ETA, WK#, SP# can not all empty!");
                return false;
            }
            if (!MyUtility.Check.Empty(datekPIETA.Value1))
            {
                KPIETA1 = ((DateTime)datekPIETA.Value1).ToString("d");
                KPIETA2 = ((DateTime)datekPIETA.Value2).ToString("d");
            }
            else
            {
                KPIETA1 = string.Empty;
                KPIETA2 = string.Empty;
            }

            if (!MyUtility.Check.Empty(dateWhseArrival.Value1))
            {
                WhseArrival1 = ((DateTime)dateWhseArrival.Value1).ToString("d");
                WhseArrival2 = ((DateTime)dateWhseArrival.Value2).ToString("d");
            }
            else
            {
                WhseArrival1 = string.Empty;
                WhseArrival2 = string.Empty;
            }

            if (!MyUtility.Check.Empty(dateETA.Value1))
            {
                ETA1 = ((DateTime)dateETA.Value1).ToString("d");
                ETA2 = ((DateTime)dateETA.Value2).ToString("d");
            }
            else
            {
                ETA1 = string.Empty;
                ETA2 = string.Empty;
            }

            WK1 = txtWK1.Text;
            WK2 = txtWK2.Text;
            SP1 = txtSP1.Text;
            SP2 = txtSP2.Text;
            Brand = txtbrand1.Text;
            Supplier = txtsupplier1.TextBox1.Text;
            M = comboMDivision1.Text;

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {

                Factorys = "'" + this.txtfactory.Text.Replace(",","','") + "'";
            }
            else
            {
                Factorys = string.Empty;
            }

            MtlType = comboDropDownList1.SelectedValue.ToString();

            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region Set SQL Command & SQLParameter
            string strSql = @"
select
	WK=ed.ID,
	e.eta,
	e.WhseArrival,
	o.KPILETA,
	 (SELECT MinSciDelivery FROM DBO.GetSCI(ed.Poid,o.Category)) as [Earliest SCI Delivery],
	EarlyDays=DATEDIFF(day,e.WhseArrival,o.KPILETA),
	o.FactoryID,
    po.styleid,
	ed.PoID,
	seq = ed.Seq1+' '+ed.Seq2,
	ed.suppid,
	suppname = supp.AbbEN,
	ed.Refno,
	f.WeaveTypeID,
	[description] = dbo.getmtldesc(ed.POID,ed.seq1,ed.seq2,2,0),
	[Color] = dbo.GetColorMultipleID(o.BrandID,psd.ColorID) ,
	o.ProjectID,
	[MtlType]=case when ed.FabricType = 'F' then 'Fabric'
				   when ed.FabricType = 'A' then 'Accessory' end,
	isnull(ed.Qty,0)+isnull(ed.foc,0),
	ed.UnitId,
	o.BrandID,
	Category = case when o.Category = 'B' then 'Bulk'
					when o.Category = 'M' then 'Material'
					when o.Category = 'S' then 'Sample'
					when o.Category = 'T' then 'SMLT'
					when o.Category = 'G' then 'Garment'
					when o.Category = 'O' then 'Other'
	end,
	TEPPOHandle.Email,
    TEPPOSMR.Email
from Export e
Inner join Export_Detail ed on e.ID = ed.ID
inner join orders o on o.id = ed.poid
left join supp on supp.id = ed.suppid
left join PO_Supp_Detail psd on psd.id = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
left join po on po.id = ed.PoID
left join TPEPass1 TEPPOHandle on TEPPOHandle.id = po.POHandle
left join TPEPass1 TEPPOSMR on TEPPOSMR.id = po.POSMR
left join Fabric f with(nolock)on f.SCIRefno = psd.SCIRefno
where exists (select 1 from Factory where o.FactoryId = id and IsProduceFty = 1)
and ed.PoType = 'G' 

";
            if (!MyUtility.Check.Empty(KPIETA1))
            {
                strSql += $@" and o.KPILETA between '{KPIETA1}' and '{KPIETA2}' ";
            }
            if (!MyUtility.Check.Empty(WhseArrival1))
            {
                strSql += $@" and e.WhseArrival between '{WhseArrival1}' and '{WhseArrival2}' ";
            }
            if (!MyUtility.Check.Empty(ETA1))
            {
                strSql += $@" and e.eta between '{ETA1}' and '{ETA2}' ";
            }

            if (!MyUtility.Check.Empty(WK1))
            {
                strSql += $@" and ed.ID >= '{WK1}' ";
            }
            if (!MyUtility.Check.Empty(WK2))
            {
                strSql += $@" and ed.ID <= '{WK2}' ";
            }

            if (!MyUtility.Check.Empty(SP1))
            {
                strSql += $@" and ed.PoID >= '{SP1}' ";
            }
            if (!MyUtility.Check.Empty(SP2))
            {
                strSql += $@" and ed.PoID <= '{SP2}' ";
            }

            if (!MyUtility.Check.Empty(Brand))
            {
                strSql += $@" and o.BrandID = '{Brand}' ";
            }

            if (!MyUtility.Check.Empty(Supplier))
            {
                strSql += $@" and ed.suppid = '{Supplier}' ";
            }

            if (!MyUtility.Check.Empty(M))
            {
                strSql += $@" and o.MDivisionID = '{M}' ";
            }

            if (!MyUtility.Check.Empty(Factorys))
            {
                strSql += $@" and o.FactoryID in ({Factorys}) ";
            }

            strSql += $@" and ed.FabricType in ({MtlType})";
            #endregion 
            #region SQL Data Loading...
            DualResult result = DBProxy.Current.Select(null, strSql,  out dataTable);
            #endregion

            if (result) return Result.True;
            else return new DualResult(false, "Query data fail\r\n" + result.ToString());
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                this.SetCount(dataTable.Rows.Count);
                this.ShowWaitMessage("Excel Processing...");

                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R25.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(dataTable, null, "Warehouse_R25.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);
                Excel.Worksheet worksheet = objApp.Sheets[1];

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_R25");
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
