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
        string spNo;
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
                !this.dateBundleScan.HasValue &&
                string.IsNullOrEmpty(this.txtSPNo.Text))
            {
                MyUtility.Msg.InfoBox("[Farm Out Date],[Bundle CDate],[Bundle Scan Date],[SP#] please input at least one");
                return false;
            }
            #endregion

            this.factory = this.comboFactory.Text;
            this.subProcess = this.txtsubprocess.Text;
            this.spNo = this.txtSPNo.Text;
            
            return true;
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            #region SQL first where
            List<string> FirstWhere = new List<string>();
            List<string> finalWhere = new List<string>();
            string joinTmpBase = "LEFT JOIN #Base base ON bd.BundleNo=base.BundleNo";

            if (this.dateFarmOutDate.HasValue)
            {
                FirstWhere.Add($@"  and b.Id LIKE 'TB%'");
                if (this.dateFarmOutDate.Value1.Empty() == false)
                {
                    FirstWhere.Add(" and b.IssueDate >= @dateFarmOutDateFrom ");
                    finalWhere.Add(" and FarmOut.IssueDate >= @dateFarmOutDateFrom");
                    listSqlPar.Add(new SqlParameter("@dateFarmOutDateFrom", Convert.ToDateTime(this.dateFarmOutDate.DateBox1.Value)));
                }

                if (this.dateFarmOutDate.Value2.Empty() == false)
                {
                    FirstWhere.Add(" and b.IssueDate <= @dateFarmOutDateTo ");
                    finalWhere.Add(" and FarmOut.IssueDate <= @dateFarmOutDateTo");
                    listSqlPar.Add(new SqlParameter("@dateFarmOutDateTo", Convert.ToDateTime(this.dateFarmOutDate.DateBox2.Value)));
                }

                joinTmpBase = joinTmpBase.Replace("LEFT", "inner");
            }

            if (this.dateBundleCdate.HasValue)
            {
                FirstWhere.Add($@" and exists (select 1 from Bundle bud with (nolock)
							                    inner join Bundle_Detail budd with (nolock) on bud.ID = budd.Id
                                                where budd.BundleNo = bd.BundleNo ");
                if (this.dateBundleCdate.Value1.Empty() == false)
                {
                    FirstWhere.Add(" and bud.Cdate >= @dateBundleCdateFrom");
                    finalWhere.Add(" and b.Cdate >= @dateBundleCdateFrom");
                    listSqlPar.Add(new SqlParameter("@dateBundleCdateFrom", Convert.ToDateTime(this.dateBundleCdate.DateBox1.Value)));
                }

                if (this.dateBundleCdate.Value2.Empty() == false)
                {
                    FirstWhere.Add(" and bud.Cdate <= @dateBundleCdateTo");
                    finalWhere.Add(" and b.Cdate <= @dateBundleCdateTo");
                    listSqlPar.Add(new SqlParameter("@dateBundleCdateTo", Convert.ToDateTime(this.dateBundleCdate.DateBox2.Value)));
                }

                FirstWhere.Add(")");
            }

            if (this.dateBundleScan.HasValue)
            {
                if (dateBundleScan.Value1.Empty() == false)
                {
                    FirstWhere.Add(" and (b.Id LIKE 'TB%' and b.IssueDate >= @dateBundleScanFrom or b.Id LIKE 'TC%' and bd.ReceiveDate >= @dateBundleScanFrom)");
                    finalWhere.Add(" and (FarmOut.IssueDate >= @dateBundleScanFrom or FarmIn.ReceiveDate >= @dateBundleScanFrom)");
                    listSqlPar.Add(new SqlParameter("@dateBundleScanFrom", (this.dateBundleScan.DateBox1.Value)));
                }
            
                if (dateBundleScan.Value2.Empty() == false)
                {
                    FirstWhere.Add(" and (b.Id LIKE 'TB%' and b.IssueDate <= @dateBundleScanTo or b.Id LIKE 'TC%' and bd.ReceiveDate <= @dateBundleScanTo)");
                    finalWhere.Add(" and (FarmOut.IssueDate <= @dateBundleScanTo or FarmIn.ReceiveDate <= @dateBundleScanTo)");
                    listSqlPar.Add(new SqlParameter("@dateBundleScanTo", (this.dateBundleScan.DateBox2.Value.Value.AddDays(1).AddSeconds(-1))));
                }

                joinTmpBase = joinTmpBase.Replace("LEFT", "inner");
            }
            
            #endregion 
            #region SQL final where
            if (!factory.Empty())
            {
                finalWhere.Add("	and O.FTYGroup = @Factory   --Factory");
                listSqlPar.Add(new SqlParameter("@Factory", factory));
            }

            if (!MyUtility.Check.Empty(this.subProcess))
            {
                finalWhere.Add($@" and (s.id in ('{subProcess.Replace(",", "','")}') or '{subProcess}'='')");
            }

            if (!MyUtility.Check.Empty(this.spNo))
            {
                FirstWhere.Add($@" and bd.orderid = '{spNo}'");
                finalWhere.Add($@" and o.ID = '{spNo}'");
            }

            #endregion
            #region SQL CMD
            string sqlCmd = $@"
set arithabort on

--筆記：Farm Out=BundleTrack.AddDate   、   Farm In = BundleTrack_Detail.ReceiveDate

--先找出該時間區段內所有的Farm Out，BundleNo、StartProcess
SELECT DISTINCT bd.BundleNo ,b.StartProcess 
INTO #Base
FROM BundleTrack b
INNER JOIN BundleTrack_detail bd ON b.ID = bd.id
WHERE   1 = 1 {FirstWhere.JoinToString("\r\n")}  

--再回頭，找全DB裡面相同BundleNo、StartProcess的Out和In資料，連同相關欄位一起找出來，lastEditDate 用於排序

SELECT distinct bd.BundleNo ,b.StartProcess ,
		[IssueDate] = FIRST_VALUE(b.IssueDate) over (partition by bd.BundleNo ,b.StartProcess ORDER BY b.IssueDate desc),
		[AddDate] = FIRST_VALUE(b.AddDate) over (partition by bd.BundleNo ,b.StartProcess ORDER BY b.IssueDate desc),
		[EndSite] = FIRST_VALUE(b.EndSite) over (partition by bd.BundleNo ,b.StartProcess ORDER BY b.IssueDate desc)
INTO #FarmOutList
FROM BundleTrack b
INNER JOIN BundleTrack_detail bd ON b.ID = bd.id
WHERE   b.Id LIKE 'TB%' 
		AND exists (select 1 from #Base where BundleNo = bd.BundleNo and StartProcess=b.StartProcess)


SELECT distinct bd.BundleNo ,b.StartProcess  ,
		[ReceiveDate] = FIRST_VALUE(bd.ReceiveDate) over (partition by bd.BundleNo ,b.StartProcess ORDER BY bd.ReceiveDate desc)
INTO #FarmInList
FROM BundleTrack b
INNER JOIN BundleTrack_detail bd ON b.ID = bd.id
WHERE   b.Id LIKE 'TC%' 
		AND exists (SELECT 1 FROM #Base where BundleNo = bd.BundleNo and StartProcess=b.StartProcess)
		

--取最大IssueDate和ReceiveDate用Outer apply排序做
SELECT  isnull(base.BundleNo,bd.BundleNo) BundleNo
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
		,[FarmOutDate]=FarmOut.AddDate
		,[FarmInDate]=FarmIn.ReceiveDate
		,EstCut.EstCutDate
		,EstCut.CuttingOutputDate
		,[Subcon] = FarmOut.EndSite + '-' + ls.Abb 
		, '' remark 
from Bundle b
LEFT JOIN Orders o ON o.ID=b.Orderid
inner JOIN Bundle_Detail bd ON b.ID = bd.Id
{joinTmpBase}
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
left join #FarmOutList  FarmOut on FarmOut.BundleNo=isnull(base.BundleNo,bd.BundleNo) AND FarmOut.StartProcess= s.Id 
left join #FarmInList  FarmIn on FarmIn.BundleNo=isnull(base.BundleNo,bd.BundleNo) AND FarmIn.StartProcess= s.Id
left join LocalSupp ls on ls.id=FarmOut.EndSite
WHERE 1=1 {finalWhere.JoinToString("\r\n")}

 
order by [Bundleno],[CutRef],[Orderid],[StyleID],[SeasonID],[BrandID],[Article],[ColorID],[Sewinglineid],[SewingCell]
        ,[Patterncode],[PatternDesc],[BundleGroup],[SizeCode],[FarmOutDate] desc,[FarmInDate] desc
		
Drop Table #FarmOutList,#FarmInList,#Base
";

            #endregion
            #region Get Data
            DBProxy.Current.DefaultTimeout = 900;  //加長時間為15分鐘，避免timeout
            DualResult result;
            result = DBProxy.Current.Select(null, sqlCmd, listSqlPar, out printData);
            if (!result)
            {
                return Result.F(result.ToString());
            }            
            #endregion
            return Result.True;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region check printData
            if (printData.Rows.Count == 0)
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
