using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using static Sci.MyUtility;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Subcon
{
    public partial class R20 : Sci.Win.Tems.PrintForm
    {
        private string type;
        private string supplier;
        private DataTable dtPrint;
        public R20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboBoxType, 2, 1, "Thread,Thread");
            this.comboBoxType.SelectedIndex = 0;
        }

        protected override bool ValidateInput()
        {
            // 預設一個Type "Thread", 之後可能會有Label,Carton....
            switch (comboBoxType.Text)
            {
                case "Thread":
                    type = @"'SP_THREAD','EMB_THREAD'";
                    break;
            }
            supplier = this.txtlocalSupp.TextBox1.Text;
            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            #region Filter
            List<string> listSQLFilter = new List<string>();
            if (!MyUtility.Check.Empty(type))
            {
                listSQLFilter.Add($"and l.Category in ({type})");
            }

            if (!MyUtility.Check.Empty(supplier))
            {
                listSQLFilter.Add($"and l.LocalSuppid='{supplier}'");
            }
            #endregion

            #region SqlCmd
            string strcmd = string.Empty;
            strcmd = $@"
select l.Refno
    ,junk,Description,Category,UnitID,LocalSuppid
    ,l.Price,CurrencyID,ThreadTypeID,ThreadTex,Weight,AxleWeight
    ,MeterToCone,QuotDate,l.AddName,l.AddDate,l.EditName,l.EditDate
    ,NLCode,HSCode,CustomsUnit,ArtTkt
    ,lt.BuyerID,lt.ThreadColorGroupID,lt.Price
    ,lt.AddName,lt.AddDate,lt.EditName,lt.EditDate
from LocalItem l
left join LocalItem_ThreadBuyerColorGroupPrice lt on l.RefNo=lt.Refno
where 1=1
{listSQLFilter.JoinToString($"{Environment.NewLine} ")}
";
            #endregion

            DualResult res = DBProxy.Current.Select(string.Empty, strcmd, out dtPrint);
            if (!res)
            {
                ShowErr(res);
            }
            return Result.True;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            if (dtPrint.Rows.Count < 1)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            this.SetCount(dtPrint.Rows.Count);
            Excel.Application objApp = new Excel.Application();
            Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\SubCon_R20.xltx", objApp);
            Excel.Worksheet worksheet = objApp.Sheets[1];
            com.WriteTable(dtPrint, 3);
            worksheet.get_Range($"A3:AC{MyUtility.Convert.GetString(2 + dtPrint.Rows.Count)}").Borders.LineStyle = Excel.XlLineStyle.xlContinuous; // 畫線
            worksheet.Columns[16].NumberFormatLocal = "yyyy/MM/dd hh:mm";
            worksheet.Columns[18].NumberFormatLocal = "yyyy/MM/dd hh:mm";
            worksheet.Columns[27].NumberFormatLocal = "yyyy/MM/dd hh:mm";
            worksheet.Columns[29].NumberFormatLocal = "yyyy/MM/dd hh:mm";

            com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
            objApp.Visible = true;
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }
    }
}
