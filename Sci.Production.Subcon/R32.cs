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


--筆記：Farm Out=BundleTrack.IssueDate   、   Farm In = BundleTrack_Detail.ReceiveDate

--先找出User 輸入條件的 Farm Out區間
SELECT  DISTINCT  bd.BundleNo,b.StartProcess,[MaxOutDate]=Max(b.IssueDate)
INTO #MaxOutDateList
FROM BundleTrack b
INNER JOIN BundleTrack_detail bd ON b.ID = bd.id
WHERE   b.Id LIKE 'TB%' 
		and b.IssueDate >= @StartDate 
		and b.IssueDate <= @EndDate
        and b.StartProcess = @SubProcess  
{0}
GROUP BY bd.BundleNo,b.StartProcess

--根據該區間內的 BundleNo + StartProcess，找出所有Farm In，沒有的話放NULL
SELECT  DISTINCT  t.BundleNo,t.StartProcess,t.MaxOutDate,[MaxInDate]=Max(bd.ReceiveDate)
INTO #summary
FROM BundleTrack b
INNER JOIN BundleTrack_detail bd ON b.ID = bd.id 
RIGHT JOIN #MaxOutDateList t ON t.BundleNo=bd.BundleNo AND t.StartProcess=b.StartProcess AND  (bd.Id LIKE 'TC%' )
GROUP BY t.BundleNo,t.StartProcess,t.MaxOutDate

--用上面清單，串接所有的BundleTrack、BundleTrack_Detail，  BundleNo、StartProcess必須完全符合
SELECT DISTINCT 
		 O.FactoryID
		, O.ID
		, O.StyleID
		, BD.SizeCode
		, BD.BundleGroup
		, BTD.BundleNo
		, BodyCutNo = Concat(B.FabricPanelCode, ' ', B.Cutno)
		, BD.Qty
		, Color = Concat(B.Article, ' ', B.Colorid)
		, summary.MaxOutDate
		, summary.MaxInDate
        , [Subcon] = NewestFarmOut.EndSite + '-' + ls.Abb 
		, '' remark  
FROM BundleTrack BT
LEFT JOIN BundleTrack_detail BTD on BT.Id=BTD.Id
INNER JOIN #summary summary ON summary.BundleNo=BTD.BundleNo AND  summary.StartProcess=BT.StartProcess

--避免資料發散，因此以最新Farm out那筆的BundleTrack.EndSite去關聯localSupp
OUTER APPLY(
		SELECT TOP 1 [EndSite]=a.EndSite
		FROM BundleTrack a
		INNER JOIN BundleTrack_detail b ON a.id = b.id
		WHERE BundleNo=summary.BundleNo AND StartProcess=summary.StartProcess
		ORDER BY IssueDate DESC, AddDate DESC 
)NewestFarmOut

LEFT JOIN Orders O on O.ID=BTD.orderid
LEFT JOIN Bundle_Detail BD on BD.BundleNo=BTD.BundleNo
LEFT JOIN Bundle B on B.ID =BD.Id
INNER join localSupp ls on ls.id=NewestFarmOut.endsite

ORDER BY BTD.BundleNo

DROP TABLE #MaxOutDateList, #summary

", filte.JoinToString("\r\n"));

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
