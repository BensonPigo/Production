using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
namespace Sci.Production.Warehouse
{
    public partial class R21 : Sci.Win.Tems.PrintForm
    {
        string StartSPNo, EndSPNo, MDivision, Factory, StartRefno, EndRefno, Color, MT, ST;
        int ReportType;
        bool boolCheckQty;
        DataTable printData;

        public R21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();

            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("All", "All");
            comboBox1_RowSource.Add("F", "Fabric");
            comboBox1_RowSource.Add("A", "Accessory");
            cmbMaterialType.DataSource = new BindingSource(comboBox1_RowSource, null);
            cmbMaterialType.ValueMember = "Key";
            cmbMaterialType.DisplayMember = "Value";
            cmbMaterialType.SelectedIndex = 0;

            Dictionary<String, String> comboBox2_RowSource = new Dictionary<string, string>();
            comboBox2_RowSource.Add("All", "All");
            comboBox2_RowSource.Add("B", "Bulk");
            comboBox2_RowSource.Add("I", "Inventory");
            comboBox2_RowSource.Add("O", "Scrap");
            cmbStockType.DataSource = new BindingSource(comboBox2_RowSource, null);
            cmbStockType.ValueMember = "Key";
            cmbStockType.DisplayMember = "Value";
            cmbStockType.SelectedIndex = 0;
        }

        protected override bool ValidateInput()
        {
            StartSPNo = textStartSP.Text ;
            EndSPNo = textEndSP.Text;
            MDivision = txtMdivision1.Text;
            Factory = txtfactory1.Text;
            StartRefno = textStartRefno.Text;
            EndRefno = textEndRefno.Text;
            Color = textColor.Text;
            MT = cmbMaterialType.SelectedValue.ToString();
            ST = cmbStockType.SelectedValue.ToString();
            ReportType = rdbtnDetail.Checked ? 0 : 1;
            boolCheckQty = checkQty.Checked;
            return true;
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {            
            StringBuilder sqlcmd = new StringBuilder();
            
            if (ReportType == 0)
            {
                #region 主要sql Detail
                sqlcmd.Append(@"
select
	[M] = o.MDivisionID
	,[Factory] = o.FactoryID
	,[SP#] = psd.id
	,[Brand] = o.BrandID
	,[Style] = o.StyleID
	,[Season] = o.SeasonID
	,[Project] = o.ProjectID
	,[Program] = o.ProgramID
	,[Seq1] = psd.SEQ1
	,[Seq2] = psd.SEQ2
	,[Material Type] = psd.FabricType
	,[Refno] = psd.Refno
	,[SCI Refno] = psd.SCIRefno
	,[Description] = d.Description
	,[Color] = psd.ColorID
	,[Size] = psd.SizeSpec
	,[Stock Unit] = psd.StockUnit
	,[Purchase Qty] = dbo.GetUnitQty(psd.PoUnit, psd.StockUnit, psd.Qty)
	,[Ship Qty] = dbo.GetUnitQty(psd.PoUnit, psd.StockUnit, psd.ShipQty)
	,[Roll] = fi.Roll
	,[Dyelot] = fi.Dyelot
	,[Stock Type] = case when fi.StockType = 'B' then 'Bulk'
						 when fi.StockType = 'I' then 'Inventory'
						 when fi.StockType = 'O' then 'Scrap'
						 end
	,[In Qty] = round(fi.InQty,2)
	,[Out Qty] = round(fi.OutQty,2)
	,[Adjust Qty] = round(fi.AdjustQty,2)
	,[Balance Qty] = round(fi.InQty,2) - round(fi.OutQty,2) + round(fi.AdjustQty,2)
	,[Location] = f.MtlLocationID
from Orders o with (nolock)
inner join PO_Supp_Detail psd with (nolock) on psd.id = o.id
left join FtyInventory fi with (nolock) on fi.POID = psd.id and fi.Seq1 = psd.SEQ1 and fi.Seq2 = psd.SEQ2
outer apply
(
	select MtlLocationID = stuff(
	(
		select concat(',',MtlLocationID)
		from FtyInventory_Detail fid with (nolock) 
		where fid.Ukey = fi.Ukey
		for xml path('')
	),1,1,'')
)f
outer apply
(
	select Description 
	from Fabric f with (nolock)
	where f.SCIRefno = psd.SCIRefno
)d
where 1=1
");
                #endregion
            }
            else
            {
                #region 主要sql summary
                sqlcmd.Append(@"
select
	[M] = o.MDivisionID
	,[Factory] = o.FactoryID
	,[SP#] = psd.id
	,[Brand] = o.BrandID
	,[Style] = o.StyleID
	,[Season] = o.SeasonID
	,[Project] = o.ProjectID
	,[Program] = o.ProgramID
	,[Seq1] = psd.SEQ1
	,[Seq2] = psd.SEQ2
	,[Material Type] = psd.FabricType
	,[Refno] = psd.Refno
	,[SCI Refno] = psd.SCIRefno
    ,[Description] = d.Description
	,[Color] = psd.ColorID
	,[Size] = psd.SizeSpec
	,[Stock Unit] = psd.StockUnit
	,[Purchase Qty] = round(ISNULL(r.RateValue,1) * psd.Qty,2)
	,[Ship Qty] = round(ISNULL(r.RateValue,1) * psd.ShipQty,2)
	,[In Qty] = round(mpd.InQty,2)
	,[Out Qty] = round(mpd.OutQty,2)
	,[Adjust Qty] = round(mpd.AdjustQty,2)
	,[Balance Qty] = round(mpd.InQty,2) - round(mpd.OutQty,2) + round(mpd.AdjustQty,2)
	,[Bulk Qty] = (round(mpd.InQty,2) - round(mpd.OutQty,2) + round(mpd.AdjustQty,2)) - round(mpd.LInvQty,2)
	,[Inventory Qty] = round(mpd.LInvQty,2)
	,[Scrap Qty] = round(mpd.LObQty ,2)
	,[Bulk Location] = mpd.ALocation
	,[Inventory Location] = mpd.BLocation
from Orders o with (nolock)
inner join PO_Supp_Detail psd with (nolock) on psd.id = o.id
left join MDivisionPoDetail mpd with (nolock) on mpd.POID = psd.id and mpd.Seq1 = psd.SEQ1 and mpd.seq2 = psd.SEQ2
outer apply
(
	select Description 
	from Fabric f with (nolock)
	where f.SCIRefno = psd.SCIRefno
)d
outer apply
(
	select RateValue
	from Unit_Rate WITH (NOLOCK) 
	where UnitFrom = psd.PoUnit and UnitTo = psd.StockUnit
)r
where 1=1
");
                #endregion
            }

            #region where條件
            if (!MyUtility.Check.Empty(StartSPNo))
                sqlcmd.Append(string.Format(" and psd.id >= '{0}'", StartSPNo));
            if (!MyUtility.Check.Empty(EndSPNo))
                sqlcmd.Append(string.Format(" and (psd.id <= '{0}' or psd.id like '{0}%')", EndSPNo));
            if (!MyUtility.Check.Empty(MDivision))
                sqlcmd.Append(string.Format(" and o.MDivisionID = '{0}'", MDivision));
            if (!MyUtility.Check.Empty(Factory))
                sqlcmd.Append(string.Format(" and o.FtyGroup = '{0}'", Factory));
            if (!MyUtility.Check.Empty(StartRefno))
                sqlcmd.Append(string.Format(" and psd.Refno >= '{0}'", StartRefno));
            if (!MyUtility.Check.Empty(EndRefno))
                sqlcmd.Append(string.Format(" and (psd.Refno <= '{0}' or psd.Refno like '{0}%')", EndRefno));
            if (!MyUtility.Check.Empty(Color))
                sqlcmd.Append(string.Format(" and psd.ColorID = '{0}'", Color));

            if (!MyUtility.Check.Empty(MT))
            {
                if (MT != "All")
                    sqlcmd.Append(string.Format(" and psd.FabricType = '{0}'", MT));
            }
            if (!MyUtility.Check.Empty(ST))
            {
                if (ST != "All")
                {
                    if (ReportType == 0)
                    {
                        sqlcmd.Append(string.Format(" and fi.StockType = '{0}'", ST));
                    }
                    else
                    {
                        if (ST == "B")
                            sqlcmd.Append(" and (round(mpd.InQty,2) - round(mpd.OutQty,2) + round(mpd.AdjustQty,2)) - round(mpd.LInvQty,2)>0");
                        else if (ST == "I")
                            sqlcmd.Append(" and round(mpd.LInvQty,2) > 0");
                        else if (ST == "O")
                            sqlcmd.Append(" and round(mpd.LObQty ,2) > 0");
                    }
                }
            }
            if (boolCheckQty)
            {
                if (ReportType == 0)
                    sqlcmd.Append(" and (round(fi.InQty,2) - round(fi.OutQty,2) + round(fi.AdjustQty,2))>0");
                else
                    sqlcmd.Append(" and round(mpd.InQty,2) - round(mpd.OutQty,2) + round(mpd.AdjustQty,2)>0");
            }
            #endregion

            #region Get Data
            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlcmd.ToString(), out printData))
            {
                return result;
            }
            #endregion
            return Result.True;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region check printData
            if (printData == null || printData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            #endregion
            this.SetCount(printData.Rows.Count);
            this.ShowWaitMessage("Excel Processing");
            #region To Excel
            string reportname = "";
            if (ReportType == 0)
                reportname = "Warehouse_R21_Detail.xltx";
            else
                reportname = "Warehouse_R21_Summary.xltx";

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\"+reportname);
            MyUtility.Excel.CopyToXls(printData, "", reportname, 1, showExcel: false, excelApp: objApp);
            Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Columns.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName((ReportType == 0) ? "Warehouse_R21_Detail" : "Warehouse_R21_Summary");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
