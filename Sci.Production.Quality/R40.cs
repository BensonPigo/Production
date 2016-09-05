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
    public partial class R40 : Sci.Win.Tems.PrintForm
    {
        public R40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            print.Enabled = false;
            this.comboBox_brand.SelectedIndex = 0;
        }
        string Brand;
        string Year;
        string Factory;


        protected override bool ValidateInput()
        {
            Brand = comboBox_brand.SelectedItem.ToString();
            Year = radiobtn_byYear.Checked.ToString();
            Factory = radiobtn_byfactory.Checked.ToString();
            return base.ValidateInput();
        }

     
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (DateTime.Now.Month==01)
            {
                //ADIDASComplain.startdate=this. 
            }
            else
            { 
            
            
            
            }


            return base.OnAsyncDataLoad(e);
        }

   protected override bool OnToExcel(Win.ReportDefinition report)
        {
            //if (dtt == null || dtt.Rows.Count == 0)
            //{
            //    MyUtility.Msg.ErrorBox("Data not found");
            //    return false;
            //}
            //Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Miscellaneous_R40.xltx"); //預先開啟excel app                         
            //MyUtility.Excel.CopyToXls(dtt, "", "Miscellaneous_R40.xltx", 1, true, null, objApp);      // 將datatable copy to excel
            //Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            //if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            //if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            //return true;
            return base.OnToExcel(report);
        }

    }
}
