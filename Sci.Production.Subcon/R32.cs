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
        string subProcess;
        string factory;
        DataTable printData;

        public R32(ToolStripMenuItem menuitem)
            :base(menuitem)
        {
            InitializeComponent();
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
            if (!this.dateFarmOutDate.HasValue &&
                !this.dateBundleCdate.HasValue &&
                !this.dateBundleScan.HasValue)
            {
                MyUtility.Msg.InfoBox("[Farm Out Date],[Bundle CDate],[Bundle Scan Date] please input at least one");
                return false;
            }
            #endregion

            this.factory = this.comboFactory.Text;
            this.subProcess = this.txtsubprocess.Text;
            
            return true;
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            #region SQL first where
            List<string> FirstWhere = new List<string>();

            if (this.dateFarmOutDate.HasValue)
            {
                FirstWhere.Add($@"  and b.Id LIKE 'TB%' 
		                            and b.IssueDate >= @dateFarmOutDateFrom 
		                            and b.IssueDate <= @dateFarmOutDateTo ");
                listSqlPar.Add(new SqlParameter("@dateFarmOutDateFrom", this.dateFarmOutDate.DateBox1.Value));
                listSqlPar.Add(new SqlParameter("@dateFarmOutDateTo", this.dateFarmOutDate.DateBox2.Value));
            }

            if (this.dateBundleCdate.HasValue)
            {
                FirstWhere.Add($@" and exists (select 1 from Bundle bud with (nolock)
							                    inner join Bundle_Detail budd with (nolock) on bud.ID = budd.Id
							                    where budd.BundleNo = bd.BundleNo and bud.Cdate >= @dateBundleCdateFrom and bud.Cdate <= @dateBundleCdateTo) ");
                listSqlPar.Add(new SqlParameter("@dateBundleCdateFrom", this.dateBundleCdate.DateBox1.Value));
                listSqlPar.Add(new SqlParameter("@dateBundleCdateTo", this.dateBundleCdate.DateBox2.Value));
            }

            if (this.dateBundleScan.HasValue)
            {
                FirstWhere.Add($@"  and (	(b.Id LIKE 'TB%' and b.IssueDate >= @dateBundleScanFrom and b.IssueDate <= @dateBundleScanTo) or
				                            (b.Id LIKE 'TC%' and bd.ReceiveDate >= @dateBundleScanFrom and bd.ReceiveDate <= @dateBundleScanTo)) ");
                listSqlPar.Add(new SqlParameter("@dateBundleScanFrom", this.dateBundleScan.DateBox1.Value));
                listSqlPar.Add(new SqlParameter("@dateBundleScanTo", this.dateBundleScan.DateBox2.Value));
            }
            
            #endregion 
            #region SQL final where
            List<string> finalWhere = new List<string>();
            if (!factory.Empty())
            {
                finalWhere.Add("	and O.FTYGroup = @Factory   --Factory");
                listSqlPar.Add(new SqlParameter("@Factory", factory));
            }

            if (!MyUtility.Check.Empty(this.subProcess))
            {
                finalWhere.Add($@" and (s.id in ('{subProcess.Replace(",", "','")}') or '{subProcess}'='')");
            }

            #endregion
            #region SQL CMD
            string sqlCmd = $@"

--筆記：Farm Out=BundleTrack.IssueDate   、   Farm In = BundleTrack_Detail.ReceiveDate

--先找出該時間區段內所有的Farm Out，BundleNo、StartProcess
SELECT DISTINCT bd.BundleNo ,b.StartProcess 
INTO #Base
FROM BundleTrack b
INNER JOIN BundleTrack_detail bd ON b.ID = bd.id
WHERE   1 = 1 {FirstWhere.JoinToString("\r\n")}  

--再回頭，找全DB裡面相同BundleNo、StartProcess的Out和In資料，連同相關欄位一起找出來，lastEditDate 用於排序

SELECT b.EndSite ,bd.BundleNo ,b.StartProcess ,b.IssueDate, lastEditDate = iif (b.EditDate is null or b.AddDate > b.EditDate, b.AddDate, b.EditDate)
INTO #FarmOutList
FROM BundleTrack b
INNER JOIN BundleTrack_detail bd ON b.ID = bd.id
WHERE   b.Id LIKE 'TB%' 
		AND bd.BundleNo IN (SELECT BundleNo FROM #Base)
		AND b.StartProcess IN (SELECT StartProcess FROM #Base)


SELECT b.EndSite ,bd.BundleNo ,b.StartProcess ,bd.ReceiveDate ,lastEditDate = iif (b.EditDate is null or b.AddDate > b.EditDate, b.AddDate, b.EditDate)
INTO #FarmInList
FROM BundleTrack b
INNER JOIN BundleTrack_detail bd ON b.ID = bd.id
WHERE   b.Id LIKE 'TC%' 
		AND bd.BundleNo IN (SELECT BundleNo FROM #Base)
		AND b.StartProcess IN (SELECT StartProcess FROM #Base)


--取最大IssueDate和ReceiveDate用Outer apply排序做
SELECT  base.BundleNo
		,[EXCESS] = iif(b.IsEXCESS = 0,'','Y')
		,b.CutRef
		,b.Orderid
		,b.POID
		,b.MDivisionid
		,o.FtyGroup
		,o.Category
		,o.ProgramID
		,o.StyleID
		,o.SeasonID
		,o.BrandID
		,b.PatternPanel
		,b.Cutno
		,b.FabricPanelCode
		,b.Article
		,b.Colorid
		,b.Sewinglineid
		,b.SewingCell
		,bd.Patterncode
		,bd.PatternDesc
		,bd.BundleGroup
		,bd.SizeCode
		,[Artwork] = sub.sub
		,bd.Qty
		,[SubProcess] =  s.Id
		,b.Cdate
		,[FarmOutDate]=FarmOut.IssueDate
		,[FarmInDate]=FarmIn.ReceiveDate
		,[AvgTime] = case  when s.InOutRule = 1 then iif(FarmIn.ReceiveDate is null, null, round(Datediff(Hour,isnull(b.Cdate,''),isnull(FarmIn.ReceiveDate,''))/24.0,2))
					when s.InOutRule = 2 then iif(FarmOut.IssueDate is null, null, round(Datediff(Hour,isnull(b.Cdate,''),isnull(FarmOut.IssueDate,''))/24.0,2))
					when s.InOutRule in (3,4) and FarmIn.ReceiveDate is null and FarmOut.IssueDate is null then null
					when s.InOutRule = 3 then iif(FarmOut.IssueDate is null or FarmIn.ReceiveDate is null, null, round(Datediff(Hour,isnull(FarmIn.ReceiveDate,''),isnull(FarmOut.IssueDate,''))/24.0,2))
					when s.InOutRule = 4 then iif(FarmOut.IssueDate is null or FarmIn.ReceiveDate is null, null, round(Datediff(Hour,isnull(FarmOut.IssueDate,''),isnull(FarmIn.ReceiveDate,''))/24.0,2))
					end
		,[TimeRangeFail] = case	when s.InOutRule = 1 and FarmIn.ReceiveDate is null then 'No Scan'
						when s.InOutRule = 2 and FarmOut.IssueDate is null then 'No Scan'
						when s.InOutRule in (3,4) and FarmOut.IssueDate is null and FarmIn.ReceiveDate is null then 'No Scan'
						when s.InOutRule = 3 and (FarmOut.IssueDate is null or FarmIn.ReceiveDate is null) then 'Not Valid'
						when s.InOutRule = 4 and (FarmOut.IssueDate is null or FarmIn.ReceiveDate is null) then 'Not Valid'
						else '' end
		,EstCut.EstCutDate
		,EstCut.CuttingOutputDate
		,[Subcon] = FarmOut.EndSite + '-' + ls.Abb 
		, '' remark 
into #result
FROM #Base base
LEFT JOIN Bundle_Detail bd ON bd.BundleNo=base.BundleNo
LEFT JOIN Bundle b ON b.ID =bd.Id
LEFT JOIN Orders o ON o.ID=b.Orderid
OUTER APPLY(
	SELECT	[EstCutDate] = MAX(w.EstCutDate),
			[CuttingOutputDate] = MAX(co.cDate)
	from WorkOrder w WITH (NOLOCK)
	LEFT JOIN CuttingOutput_Detail cod WITH (NOLOCK) ON w.Ukey = cod.WorkOrderUkey
	LEFT JOIN CuttingOutput co WITH (NOLOCK) ON cod.ID = co.ID
	where w.CutRef = b.CutRef and w.MDivisionId = b.MDivisionid
)  EstCut
outer apply(
    select s.ID,s.InOutRule
    from SubProcess s
        where exists (
                        select 1 from Bundle_Detail_Art bda
                                where   bda.BundleNo = bd.BundleNo    and
                                        bda.ID = b.ID   and
                                        bda.SubProcessID = s.ID
                        ) or s.IsRFIDDefault = 1
) s
outer apply(
	    select sub= stuff((
		    Select distinct concat('+', bda.SubprocessId)
		    from Bundle_Detail_Art bda WITH (NOLOCK) 
		    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
		    for xml path('')
	    ),1,1,'')
) as sub
OUTER APPLY(
   SELECT TOP 1 EndSite,BundleNo,StartProcess,IssueDate
   FROm #FarmOutList  
   WHERE BundleNo=base.BundleNo AND StartProcess=base.StartProcess
   ORDER BY lastEditDate DESC
)FarmOut
OUTER APPLY(
   SELECT TOP 1 EndSite,BundleNo,StartProcess,ReceiveDate
   FROm  #FarmInList    
   WHERE BundleNo=base.BundleNo AND StartProcess=base.StartProcess
   ORDER BY lastEditDate DESC
)FarmIn
INNER join LocalSupp ls on ls.id=FarmOut.EndSite
WHERE 1=1 {finalWhere.JoinToString("\r\n")}


select
		BundleNo
		,EXCESS
		,CutRef
		,Orderid
		,POID
		,MDivisionid
		,FtyGroup
		,Category
		,ProgramID
		,StyleID
		,SeasonID
		,BrandID
		,PatternPanel
		,Cutno
		,FabricPanelCode
		,Article
		,Colorid
		,Sewinglineid
		,SewingCell
		,Patterncode
		,PatternDesc
		,BundleGroup
		,SizeCode
		,Artwork
		,Qty
		,SubProcess
		,Cdate
		,FarmOutDate
		,FarmInDate
		,AvgTime
		,[TimeRange] = case	when TimeRangeFail <> '' then TimeRangeFail
                        when AvgTime < 0 then 'Not Valid'
						when AvgTime >= 0 and AvgTime < 1 then '<1'
						when AvgTime >= 1 and AvgTime < 2 then '1-2'
						when AvgTime >= 2 and AvgTime < 3 then '2-3'
						when AvgTime >= 3 and AvgTime < 4 then '3-4'
						when AvgTime >= 4 and AvgTime < 5 then '4-5'
						when AvgTime >= 5 and AvgTime < 10 then '5-10'
						when AvgTime >= 10 and AvgTime < 20 then '10-20'
						when AvgTime >= 20 and AvgTime < 30 then '20-30'
						when AvgTime >= 30 and AvgTime < 40 then '30-40'
						when AvgTime >= 40 and AvgTime < 50 then '40-50'
						when AvgTime >= 50 and AvgTime < 60 then '50-60'
						else '>60' end
		,EstCutDate
		,CuttingOutputDate
		,Subcon
		,remark 
from #result


Drop Table #FarmOutList,#FarmInList,#Base,#result

";

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
