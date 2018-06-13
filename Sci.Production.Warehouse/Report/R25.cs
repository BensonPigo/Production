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

namespace Sci.Production.Warehouse
{
    public partial class R25 : Sci.Win.Tems.PrintForm
    {
        string strSp1, strSp2,Eta1,Eta2, strM, strFty;
        DataTable dataTable;
        public R25(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.comboMDivision1.setDefalutIndex(true);
            this.comboFactory1.setDataSource(this.comboMDivision1.Text);
        }

        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateETA.TextBox1.Value)|| MyUtility.Check.Empty(dateETA.TextBox2.Value))
            {
                MyUtility.Msg.WarningBox("ETA cannot be empty !");
                return false;
            }
            strSp1 = txtSPNoStart.Text.Trim();
            strSp2 = txtSPNoEnd.Text.Trim();
            Eta1 = ((DateTime)dateETA.TextBox1.Value).ToString("yyyy/MM/dd");
            Eta2 = ((DateTime)dateETA.TextBox2.Value).ToString("yyyy/MM/dd");

            strM = comboMDivision1.Text;
            strFty = comboFactory1.Text;
            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region Set SQL Command & SQLParameter
            string strSql = @"
select
	WK=ed.ID,
	e.eta,
	o.FtyGroup,
	ed.PoID,
	seq = ed.Seq1+' '+ed.Seq2,
	ed.suppid,
	suppname = supp.AbbEN,
	ed.Refno,
	[description] = dbo.getmtldesc(ed.POID,ed.seq1,ed.seq2,2,0),
	[Color] = dbo.GetColorMultipleID(o.BrandID,psd.ColorID) ,
	o.ProjectID,
	[MtlType]=case when ed.FabricType = 'F' then 'Fabric'
				   when ed.FabricType = 'A' then 'Accessory' end,
	ed.Qty,
	ed.UnitId,
	o.BrandID,
	Category = case when o.Category = 'B' then 'Bulk'
					when o.Category = 'M' then 'Material'
					when o.Category = 'S' then 'Sample'
					when o.Category = 'T' then 'SMLT'
					when o.Category = 'G' then 'Garment'
					when o.Category = 'O' then 'Other'
	end,
	TPEPass1.Email
from Export e
Inner join Export_Detail ed on e.ID = ed.ID
inner join orders o on o.id = ed.poid
left join supp on supp.id = ed.suppid
left join PO_Supp_Detail psd on psd.id = ed.PoID and psd.SEQ1 = ed.Seq1 and psd.SEQ2 = ed.Seq2
left join po on po.id = ed.PoID
left join TPEPass1 on TPEPass1.id = po.POHandle
where 1 = 1 and ed.PoType = 'G' and (ed.FabricType = 'F' or ed.FabricType = 'A')
";
            if (!MyUtility.Check.Empty(strSp1))
            {
                strSql += $@" and ed.PoID >= '{strSp1}' ";
            }
            if (!MyUtility.Check.Empty(strSp2))
            {
                strSql += $@" and ed.PoID <= '{strSp2}' ";
            }

            if (!MyUtility.Check.Empty(Eta1))
            {
                strSql += $@" and e.Eta >= '{Eta1}' ";
            }
            if (!MyUtility.Check.Empty(Eta2))
            {
                strSql += $@" and e.Eta <= '{Eta2}' ";
            }

            if (!MyUtility.Check.Empty(strM))
            {
                strSql += $@" and o.MDivisionID = '{strM}' ";
            }
            if (!MyUtility.Check.Empty(strFty))
            {
                strSql += $@" and o .FtyGroup = '{strFty}' ";
            }

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
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

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
