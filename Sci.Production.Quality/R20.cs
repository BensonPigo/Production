using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R20 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        public R20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            DataTable factory;
            InitializeComponent();
            DBProxy.Current.Select(null, "select distinct FtyGroup from Factory", out factory);
            MyUtility.Tool.SetupCombox(cmb_Factory, 1, factory);
        }

        private void radiobtn_PerLine_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobtn_PerLine.Checked)
            {
                txt_DefectCode.Text = txt_DefectType.Text = "";
                txt_DefectCode.Enabled = txt_DefectType.Enabled = false;
            }
        }

        private void radiobtn_PerCell_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobtn_PerCell.Checked)
            {
                txt_DefectCode.Text = txt_DefectType.Text = "";
                txt_DefectCode.Enabled = txt_DefectType.Enabled = false;
            }
        }

        private void radiobtn_AllData_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobtn_AllData.Checked)
            {
                txt_DefectCode.Text = txt_DefectType.Text = "";
                txt_DefectCode.Enabled = txt_DefectType.Enabled = false;
            }
        }

        private void radiobtn_SummybyDateandStyle_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobtn_SummybyDateandStyle.Checked)
            {
                txt_DefectCode.Enabled = txt_DefectType.Enabled = true;
            }
        }

        private void radioBtn_SummybySP_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtn_SummybySP.Checked)
            {
                txt_DefectCode.Enabled = txt_DefectType.Enabled = true;
            }
        }

        private void radiobtn_SummybyStyle_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobtn_SummybyStyle.Checked)
            {
                txt_DefectCode.Enabled = txt_DefectType.Enabled = true;
            }
        }

        private void radioBtn_Detail_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtn_Detail.Checked)
            {
                txt_DefectCode.Enabled = txt_DefectType.Enabled = true;
            }
        }



        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\AAA.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", "AAA.xltx", 1, true, null, objApp);// 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }
    }
}
