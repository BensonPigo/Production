using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci;
using Sci.Data;
using Ict;
using Ict.Win;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class R02 : Sci.Win.Tems.PrintForm
    {
        DataTable dt;
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.EditMode = true;
        }

        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateRange1.TextBox1.Value))
            {
                MyUtility.Msg.WarningBox("Issue date can't be empty!!");
                return false;
            }
            return base.ValidateInput();
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            SetCount(dt.Rows.Count);
            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return false;
            }

            try
            {
                //return MyUtility.Excel.CopyToXls(dt, "", "Warehouse_R02.xltx", 1,true);
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R02.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(dt, "", "Warehouse_R02.xltx", 1, showExcel: false, showSaveMsg: true, excelApp: objApp);

                MyUtility.Msg.WaitWindows("Excel Processing...");
                Excel.Worksheet worksheet = objApp.Sheets[1];
                for (int i = 1; i <= dt.Rows.Count; i++) worksheet.Cells[i + 1, 7] = ((string)((Excel.Range)worksheet.Cells[i + 1, 7]).Value).Trim();
                objApp.Visible = true;

                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
                if (worksheet != null) Marshal.FinalReleaseComObject(worksheet);    //釋放worksheet
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            //return base.OnAsyncDataLoad(e);
            DualResult result = Result.True;

            try
            {
                string sqlcmd = string.Format(@"select a.issuedate, b.frompoid poid, b.fromseq1 seq1,b.fromseq2 seq2, sum(b.qty) qty, 
isnull((select stockunit from po_supp_detail where id= b.FromPOID and seq1 = b.FromSeq1 and seq2 = b.FromSeq2),'') as unit
,dbo.getmtldesc(b.FromPOID,b.FromSeq1,b.FromSeq2,2,0) as description
from dbo.SubTransfer a inner join dbo.SubTransfer_Detail b on a.id = b.id
where a.Status = 'Confirmed' and a.type='D' 
and a.issuedate between '{0}' and '{1}'
group by a.issuedate , b.FromPOID, b.FromSeq1,b.FromSeq2
order by a.issuedate, b.FromPOID, b.FromSeq1,b.FromSeq2
", dateRange1.Text1, dateRange1.Text2);
                result = DBProxy.Current.Select(null, sqlcmd, out dt);
                if (!result) return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
