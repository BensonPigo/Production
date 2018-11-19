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
                filte.Add("	and O.FTYGroup = @Factory   --Factory");
            #endregion
            #region SQL CMD
            string sqlCmd = string.Format(@"


--筆記：Farm Out=BundleTrack.IssueDate   、   Farm In = BundleTrack_Detail.ReceiveDate

--先找出User 輸入條件的 Farm Out清單
SELECT b.EndSite ,bd.BundleNo ,b.StartProcess ,b.IssueDate,b.AddDate
INTO #FarmOutList
FROM BundleTrack b
INNER JOIN BundleTrack_detail bd ON b.ID = bd.id
WHERE   b.Id LIKE 'TB%' 
		and b.IssueDate >= @StartDate 
		and b.IssueDate <= @EndDate
        and b.StartProcess = @SubProcess  

--取得Farm In清單
--Farm In的BundleNo+StartProcess，一定只能出現在Farm Out清單裡面，因此從上面找
SELECT b.EndSite ,bd.BundleNo ,b.StartProcess ,bd.ReceiveDate ,b.AddDate
INTO #FarmInList
FROM BundleTrack b
INNER JOIN BundleTrack_detail bd ON b.ID = bd.id
WHERE   b.Id LIKE 'TC%' 
		AND bd.BundleNo IN (SELECT BundleNo FROM #FarmOutList)
		AND b.StartProcess IN (SELECT StartProcess FROM #FarmOutList)

--以FarmOutList 的為主，去掉重複
SELECT DISTINCT BundleNo ,StartProcess
INTO #Base
FROM #FarmOutList

--取最大IssueDate和ReceiveDate用Outer apply排序做
SELECT  o.FactoryID
		,o.ID
		,o.StyleID
		,bd.SizeCode
		,bd.BundleGroup
		,base.BundleNo
		,BodyCutNo=Concat(b.FabricPanelCode, ' ',b.Cutno)
		,bd.Qty
		,Color = Concat(b.Article, ' ', b.Colorid)
		,[MaxOutDate]=FarmOut.IssueDate
		,[MaxInDate]=FarmIn.ReceiveDate
		,[Subcon] = FarmOut.EndSite + '-' + ls.Abb 
		, '' remark  
FROM #Base base
LEFT JOIN Bundle_Detail bd ON bd.BundleNo=base.BundleNo
LEFT JOIN Bundle b ON b.ID =bd.Id
LEFT JOIN Orders o ON o.ID=b.Orderid
OUTER APPLY(
   SELECT TOP 1 EndSite,BundleNo,StartProcess,IssueDate
   FROm #FarmOutList  
   WHERE BundleNo=base.BundleNo AND StartProcess=base.StartProcess
   ORDER BY IssueDate DESC,AddDate DESC
)FarmOut
OUTER APPLY(
   SELECT TOP 1 EndSite,BundleNo,StartProcess,ReceiveDate
   FROm  #FarmInList    
   WHERE BundleNo=base.BundleNo AND StartProcess=base.StartProcess
   ORDER BY ReceiveDate DESC,AddDate DESC
)FarmIn
INNER join LocalSupp ls on ls.id=FarmOut.EndSite
WHERE 1=1 {0}
ORDER BY base.BundleNo

Drop Table #FarmOutList,#FarmInList,#Base

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
