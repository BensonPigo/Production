using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Sci.MyUtility;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.PPIC
{
    public partial class P25 : Sci.Win.Tems.QueryForm
    {
        public P25(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.EditMode = true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // Grid設定
            this.grid.IsEditingReadOnly = false;
            this.grid.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("BrandID", header: "Brand", width: Widths.Auto(),iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.Auto(), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.Auto(), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.Auto(), iseditingreadonly: true)
                .Text("Category", header: "Category", width: Widths.Auto(), iseditingreadonly: true)
                .Date("SCIDelivery", header: "SCIDelivery", width: Widths.Auto(), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "BuyerDelivery", width: Widths.Auto(), iseditingreadonly: true)
                .Text("FactoryID", header: "FactoryID", width: Widths.Auto(), iseditingreadonly: true)
                .Text("Region", header: "Region", width: Widths.Auto(), iseditingreadonly: true)
                .Text("SizePage", header: "Size Page", width: Widths.Auto(), iseditingreadonly: true)
                .Text("BrandGender", header: "Gender", width: Widths.Auto(), iseditingreadonly: true)
                .Text("SizeCode", header: "Sourcing Size", width: Widths.Auto(), iseditingreadonly: true)
                .Text("SizeSpec", header: "CustOrderSize", width: Widths.Auto(), iseditingreadonly: true)
                .Text("Location", header: "Location", width: Widths.Auto(), iseditingreadonly: true)
                .Text("MoldRef", header: "Mold#Refno", width: Widths.Auto(), iseditingreadonly: true)
                .Text("LabelFor ", header: "Label For", width: Widths.Auto(), iseditingreadonly: true)
                .Text("AgeGroup ", header: "Age Group", width: Widths.Auto(), iseditingreadonly: true)
                .Text("SuppID ", header: "SuppID", width: Widths.Auto(), iseditingreadonly: true)
                .Text("MainSize ", header: "Main Size", width: Widths.Auto(), iseditingreadonly: true)
                .Text("SizeSpec ", header: "SizeSpec", width: Widths.Auto(), iseditingreadonly: true)
                .Text("Remark ", header: "Remark", width: Widths.Auto(), iseditingreadonly: true)
                .Image("...", header: "   ", image: Sci.Properties.Resources.WZLOCATE, settings: this.ColButtonSettings())
                ;
        }

        private DataGridViewGeneratorImageColumnSettings ColButtonSettings()
        {
            var settings = new DataGridViewGeneratorImageColumnSettings();
            settings.CellMouseClick += (s, e) =>
            {
                if (!this.EditMode || e.Button != MouseButtons.Left || e.RowIndex < 0)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                var frm = new Sci.Production.PPIC.P25_PadPrintInUse(dr);
                frm.ShowDialog();
            };
            return settings;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFindNow_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateSCIDel.Value1) || MyUtility.Check.Empty(this.dateSCIDel.Value2))
            {
                MyUtility.Msg.WarningBox("SCI Del cannot be empty.");
                return;
            }

            this.Find();
        }

        private void Find()
        {
            this.ShowWaitMessage("Data Loading...");
            string sqlcmd = $@"
select distinct o.BrandID
,[OrderID] = o.ID
,o.StyleID
,o.SeasonID
,o.Category
,o.SciDelivery
,o.BuyerDelivery
,o.FactoryID
,[Region] = f.PadPrintGroup
,s.SizePage
,s.BrandGender
,[Refno] = ob.Refno
,oqs.SizeCode
,sco.SizeSpec
,s.Location
,pms.MoldRef
,[LabelFor] = case pm.LabelFor 
				when 'O' then 'One Asia' 
				when 'E' then 'EMEA' else pm.LabelFor end
,[AgeGroup] = dd3.Name
,p.SuppID
,[MainSize] = dd4.Name
,pm.SizeSpec
,[Remark] = case when isnull(pms.MoldRef,'') != '' then pms.MoldRef
			else iif(isnull(o.FactoryID,'') = '',' <Orders.FactoryID>','') + 
			iif(isnull(s.SizePage,'') = '' ,' <Style.SizePage>' ,'')+
			iif(isnull(s.BrandGender,'') = '', ' <Style.BrandGender>','')+
			iif(isnull(sco.SizeSpec,'') = '', ' <Style_CustOrderSize>','')+
			' is Null' end

from orders o
left join Factory f on f.ID = o.FactoryID
left join Style s on s.Ukey = o.StyleUkey
left join Order_QtyShip_Detail oqs on oqs.Id = o.ID
left join Style_CustOrderSize sco on sco.StyleUkey = o.StyleUkey
left join Order_BOA ob on ob.Id = o.POID and ob.Refno like '%PAD PRINT%'
left join Brand_SizeCode bsc on bsc.BrandID = o.BrandID and bsc.SizeCode = oqs.SizeCode
left join DropDownList dd1 on dd1.Name = s.BrandGender and dd1.Type = 'PadPrint_Gender'
left join (
	select [PadPrint_PartID] = d.ID,[LocationID]  = dd.id
	from DropDownList d
	inner join DropDownList dd on dd.Type = 'Location'
	and d.Name = dd.Name
	where d.Type = 'PadPrint_Part' 
)dd2 on dd2.LocationID = (select top 1 Location from Style_Location sl where sl.StyleUkey = s.Ukey)
left join DropDownList dd3 on dd3.ID = bsc.AgeGroupID and dd3.Type = 'PadPrint_AgeGroup'
left join PadPrint p on o.BrandID = p.BrandID and p.Junk = 0
left join PadPrint_Mold pm on pm.PadPrint_ukey = p.Ukey
	and pm.Region = f.PadPrintGroup
	and pm.Refno = SUBSTRING(ob.Refno,0,CHARINDEX('-',ob.refno))
	and pm.Gender   = (case when pm.LabelFor = 'O' then dd1.ID else pm.Gender end)
	and pm.Part     = (case when pm.LabelFor = 'O' then dd2.PadPrint_PartID else pm.Part end)
	and pm.AgeGroup = (case when pm.LabelFor = 'O' then bsc.AgeGroupID else pm.AgeGroup end)
left join PadPrint_Mold_Spec pms on pms.PadPrint_ukey = pm.PadPrint_ukey
	and pms.MoldID = pm.MoldID
	and pms.SourceSize = oqs.SizeCode
	and pms.SizePage = s.SizePage
	and isnull(pms.CustomerSize,'') = (case when pm.LabelFor = 'O' then sco.SizeSpec else  isnull(pms.CustomerSize,'') end)
left join DropDownList dd4 on dd4.ID = pm.MainSize and dd4.Type = 'PadPrint_ MainSize'
where 1=1
";
            if (!MyUtility.Check.Empty(this.txtbrand.Text))
            {
                sqlcmd += $" and o.BrandID = '{this.txtbrand.Text}'";
            }

            if (!MyUtility.Check.Empty(this.dateSCIDel.Value1) || !MyUtility.Check.Empty(this.dateSCIDel.Value2))
            {
                sqlcmd += $" and o.SciDelivery between '{((DateTime)this.dateSCIDel.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateSCIDel.Value2).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.txtstyle.Text))
            {
                sqlcmd += $" and o.StyleID = '{this.txtstyle.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                sqlcmd += $" and o.FactoryID = '{this.txtfactory.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSMR.Text))
            {
                sqlcmd += $" and o.SMR = '{this.txtSMR.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtHandle.Text))
            {
                sqlcmd += $" and o.MRHandle = '{this.txtHandle.Text}'";
            }

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail.\r\n" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = gridData;
            this.grid.AutoResizeColumns();
            this.HideWaitMessage();
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            DataTable excelTable = (DataTable)this.listControlBindingSource1.DataSource;
            DataTable printDT = excelTable.Clone();

            // 判斷是否有資料
            if (excelTable == null || excelTable.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }

            this.ShowWaitMessage("Excel Processing...");

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\PPIC_P25.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(printDT, string.Empty, "PPIC_P25.xltx", 2, false, null, objApp); // 將datatable copy to excel
            Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("PPIC_P25");
            Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
        }
    }
}