using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Data;
using System.Data.SqlClient;
using Ict;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Subcon
{
    public partial class R32 : Sci.Win.Tems.PrintForm
    {
        string StartDate, EndDate, SubProcess, Factory;
        DataTable printData;

        public R32(ToolStripMenuItem menuitem)
            :base(menuitem)
        {
            InitializeComponent();
            #region set ComboSubPress
            DataTable dtSubPress;
            DBProxy.Current.Select(null, @"
select Id 
from SubProcess 
where IsRFIDProcess=1
", out dtSubPress);
            MyUtility.Tool.SetupCombox(comboSubProcess, 1, dtSubPress);
            #endregion
            #region set ComboFactory
            DataTable dtFactory;
            DBProxy.Current.Select(null, @"
select FtyGroup = ''

union all
select distinct FTYGroup 
from Factory 
where Junk != 1", out dtFactory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, dtFactory);
            comboFactory.Text = Sci.Env.User.Factory;
            #endregion 
        }

        protected override bool ValidateInput()
        {
            #region 判斷必輸條件
            if (dateFarmOutDate.Value1.Empty() || dateFarmOutDate.Value2.Empty())
            {
                MyUtility.Msg.InfoBox("Farm Out Date Can't be empty.");
                return false;
            }
            #endregion
            #region Set Value
            StartDate = ((DateTime)dateFarmOutDate.Value1).ToString("yyyy-MM-dd");
            EndDate = ((DateTime)dateFarmOutDate.Value2).ToString("yyyy-MM-dd");
            SubProcess = comboSubProcess.Text;
            Factory = comboFactory.Text;
            #endregion 
            return true;
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region SQl Parameters
            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            listSqlPar.Add(new SqlParameter("@StartDate", StartDate));
            listSqlPar.Add(new SqlParameter("@EndDate", EndDate));
            listSqlPar.Add(new SqlParameter("@SubProcess", SubProcess));
            listSqlPar.Add(new SqlParameter("@Factory", Factory));
            #endregion 
            #region SQL Filte
            List<string> filte = new List<string>();
            if (!Factory.Empty())
                filte.Add("and O.FactoryID = @Factory  --Factory"); 
            #endregion
            #region SQL CMD
            string sqlCmd = string.Format(@"
SELECT  O.FactoryID
		, O.ID
		, O.StyleID
		, BD.SizeCode
		, BD.BundleGroup
		, BTD.BundleNo
		, BodyCutNo = Concat(B.FabricPanelCode, ' ', B.Cutno)
		, BD.Qty
		, Color = Concat(B.Article, ' ', B.Colorid)
		, BT.IssueDate
		, BTD_IN.ReceiveDate
        , [Subcon] = BT.EndSite + '-' + ls.Abb 
		, '' remark        
FROM BundleTrack BT
left join BundleTrack_detail BTD on BT.Id=BTD.Id
left join Orders O on O.ID=BTD.orderid
left join Bundle_Detail BD on BD.BundleNo=BTD.BundleNo
left join Bundle B on B.ID =BD.Id
left join BundleTrack_detail BTD_IN on BTD_IN.Id like 'TC%' and BD.BundleNo=BTD_IN.BundleNo  --Farm In Date
left join localSupp ls on ls.id=bt.endsite
WHERE	BT.Id LIKE 'TB%'  --Farm out
		and BT.IssueDate >= @StartDate  --Farm Out Date start
		and BT.IssueDate <= @EndDate  --Farm Out Date End
        and BT.StartProcess = @SubProcess  --Sub Process
        {0}
ORDER BY  BD.BundleNo", filte.JoinToString("\r\n"));
            #endregion
            #region Get Data
            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlCmd, listSqlPar, out printData))
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
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R32.xltx");
            MyUtility.Excel.CopyToXls(printData, "", "Subcon_R32.xltx", 1, showExcel: false, excelApp: objApp);
            Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Columns.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Subcon_R32");
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
