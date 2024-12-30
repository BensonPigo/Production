using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Sci.MyUtility;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.IE
{
    /// <inheritdoc/>
    public partial class R09 : Win.Tems.PrintForm
    {
        private string date1;
        private string date2;
        private string strOperator;
        private string strSM;
        private string strStyle;
        private DataTable printData;

        /// <inheritdoc/>
        public R09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dtAddEdit.HasValue1 && !this.dtAddEdit.HasValue2)
            {
                MyUtility.Msg.InfoBox("Please input <Buyer Delivery>.");
                return false;
            }

            this.date1 = this.dtAddEdit.Value1.Value.ToString("yyyy-MM-dd");
            this.date2 = this.dtAddEdit.Value2.Value.ToString("yyyy-MM-dd");
            this.strOperator = this.txtOperatorID.Text;
            this.strSM = this.txtST_MC_Type.Text;
            this.strStyle = this.txtStyle.Text;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Empty;
            string sqlWhere = string.Empty;
            string sqlwhereTmp = string.Empty;
            List<SqlParameter> listParameter = new List<SqlParameter>();

            if (!MyUtility.Check.Empty(this.date1))
            {
                listParameter.Add(new SqlParameter("@date1", this.date1));
                listParameter.Add(new SqlParameter("@date2", this.date2));
                sqlwhereTmp += $@"
                and
                (
                    (
                        convert(varchar(10), ss.Inline, 120) >= @date1 and
                        convert(varchar(10), ss.Offline, 120) <= @date2
                    )
                    or
                    (
                        @date1 >= convert(varchar(10), ss.Inline, 120) and
                        @date2 <= convert(varchar(10), ss.Offline, 120)
                    )
                )
                ";
            }

            if (!MyUtility.Check.Empty(this.strOperator))
            {
                listParameter.Add(new SqlParameter("@OperatorID", this.strOperator));
                sqlWhere += $@" and e.ID = @OperatorID";
            }

            if (!MyUtility.Check.Empty(this.strSM))
            {
                listParameter.Add(new SqlParameter("@STMC", this.strSM));
                sqlWhere += $@" and lmd.MachineTypeID = @STMC";
            }

            if (!MyUtility.Check.Empty(this.strStyle))
            {
                listParameter.Add(new SqlParameter("@Style", this.strStyle));
                sqlWhere += $@" and lm.StyleID = @Style";
            }

            if (this.chkVersion.Checked)
            {
                sqlWhere += $@"
                 and lm.Version = (select MAX(l.Version)
	                               from LineMapping l
	                               where l.StyleUKey = lm.StyleUKey
	                               and l.FactoryID = lm.FactoryID
	                               and l.Phase = lm.Phase
	                               and l.SewingLineID = lm.SewingLineID
	                               group by l.StyleUKey, l.FactoryID,l.Phase,l.SewingLineID)";
            }

            sqlCmd = $@"
select
[Factory] = lm.FactoryID
, [OperatorID] = e.ID
, [OperatorName] = iif(e.Junk = 1 , e.[Name], iif(e.LastName + ',' + e.FirstName <> ',',e.LastName + ',' + e.FirstName,''))
, [Style] = lm.StyleID
, [Season] = lm.SeasonID
, [Brand] = lm.BrandID
, [ComboType] = lm.ComboType
, [Version] = lm.[Version]
, [Phase] = lm.Phase
, [Line] = lm.SewingLineID
, [Team] = lm.Team
, [NO] = lmd.[No]
, [OperationCode] = lmd.OperationID
, [ST/CM Type] = lmd.MachineTypeID
, [MachineGroup] = lmd.MasterPlusGroup
, [Operation] = o.DescEN
, [Motion] = Motion.val
, [Group_Header] =  ISNULL(IIF(REPLACE(tsd.[location], '--', '') = '', REPLACE(OP.OperationID, '--', '') ,REPLACE(tsd.[location], '--', '')),'')
, [PartID] = PartID.val
, [Shape] = Shape.val
, [Attachment] = lmd.Attachment
, [Tmplate] = lmd.Template
, [GSDTime] = lmd.GSD
, [Cycle Time] = lmd.Cycle
into #tmpP03
from LineMapping_Detail lmd
LEFT JOIN LineMapping lm WITH(NOLOCK) on lm.ID = lmd.ID
LEFT JOIN Employee e WITH(NOLOCK) on lmd.EmployeeID = e.ID
LEFT JOIN Operation o WITH(NOLOCK) on o.ID = lmd.OperationID
LEFT JOIN TimeStudy TS WITH(NOLOCK) ON TS.StyleID = lm.StyleID AND TS.SeasonID = lm.SeasonID AND TS.ComboType = lm.ComboType AND TS.BrandID = lm.BrandID and ts.Version = lm.TimeStudyVersion
LEFT JOIN TimeStudy_Detail tsd WITH(NOLOCK) ON lmd.OperationID = tsd.OperationID and ts.ID = tsd.ID 
OUTER APPLY
(
	select val = stuff((select distinct concat(',',Name)
    from OperationRef a
    inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
    where a.CodeType = '00007' and a.id = o.id for xml path('') ),1,1,'')
)Motion
OUTER APPLY
(
	select val = stuff((select distinct concat(',',Name)
    from OperationRef a
    inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
    where a.CodeType = '00008' and a.id = o.id for xml path('') ),1,1,'')
)Shape
OUTER APPLY
(
	SELECT TOP 1
	OperatorIDss.OperationID
	FROM
	(
		SELECT 
		td.id
		,td.Seq
		,td.OperationID
		from TimeStudy_Detail td WITH(NOLOCK)
		where  td.OperationID LIKE '-%' and td.smv = 0
	)
	OperatorIDss 
	WHERE ID =  TS.ID AND SEQ <= (SELECT TOP 1 Seq FROM TimeStudy_Detail WHERE id = TS.ID AND OperationID = LMD.OperationID ORDER BY Seq DESC)
	ORDER BY SEQ DESC
)OP
OUTER APPLY
(
	SELECT val = R.[NAME]
	FROM Operation O WITH(NOLOCK)
	LEFT JOIN Reason R WITH(NOLOCK) ON R.ReasonTypeID = 'IE_Component' AND R.ID = SUBSTRING(O.ID,6,2)
	WHERE O.ID = lmd.OperationID 
)PartID
Where 1=1 and (e.ID is not null or e.id <> '')
and e.junk = 0
and lm.Status = 'Confirmed'
{sqlWhere}
ORDER by OperationCode,Style,Season,Brand,Version
            
SELECT *
INTO #P03
from #tmpP03 t
where exists
(
	select *
	from SewingSchedule ss
	join Orders o on ss.OrderID = o.ID
	where 1=1
	{sqlwhereTmp}
	and o.StyleID = t.Style and o.SeasonID = t.Season and o.BrandID = t.Brand
    )

select
[Factory] = lm.FactoryID
, [OperatorID] = e.ID
, [OperatorName] = iif(e.Junk = 1 , e.[Name], iif(e.LastName + ',' + e.FirstName <> ',',e.LastName + ',' + e.FirstName,''))
, [Style] = lm.StyleID
, [Season] = lm.SeasonID
, [Brand] = lm.BrandID
, [ComboType] = lm.ComboType
, [Version] = lm.[Version]
, [Phase] = lm.Phase
, [Line] = lm.SewingLineID
, [Team] = lm.Team
, [NO] = lmd.[No]
, [OperationCode] = lmd.OperationID
, [ST/CM Type] = lmd.MachineTypeID
, [MachineGroup] = lmd.MasterPlusGroup
, [Operation] = o.DescEN
, [Motion] = Motion.val
, [Group_Header] =  ISNULL(IIF(REPLACE(tsd.[location], '--', '') = '', REPLACE(OP.OperationID, '--', '') ,REPLACE(tsd.[location], '--', '')),'')
, [PartID] = PartID.val
, [Shape] = Shape.val
, [Attachment] = lmd.Attachment
, [Tmplate] = lmd.Template
, [GSDTime] = lmd.GSD
, [Cycle Time] = lmd.Cycle
into #tmpP06
from LineMappingBalancing_Detail lmd
LEFT JOIN LineMappingBalancing lm WITH(NOLOCK) on lm.ID = lmd.ID
LEFT JOIN Employee e WITH(NOLOCK) on lmd.EmployeeID = e.ID
LEFT JOIN Operation o WITH(NOLOCK) on o.ID = lmd.OperationID
INNER JOIN TimeStudy TS WITH(NOLOCK) ON TS.StyleID = lm.StyleID AND TS.SeasonID = lm.SeasonID AND TS.ComboType = lm.ComboType and ts.[Version] = lm.TimeStudyVersion
LEFT JOIN TimeStudy_Detail tsd WITH(NOLOCK) ON lmd.OperationID = tsd.OperationID and ts.ID = tsd.ID
OUTER APPLY
(
	select val = stuff((select distinct concat(',',Name)
    from OperationRef a
    inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
    where a.CodeType = '00007' and a.id = o.id for xml path('') ),1,1,'')
)Motion
OUTER APPLY
(
	select val = stuff((select distinct concat(',',Name)
    from OperationRef a
    inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
    where a.CodeType = '00008' and a.id = o.id for xml path('') ),1,1,'')
)Shape
OUTER APPLY
(
	SELECT TOP 1
	OperatorIDss.OperationID
	FROM
	(
		SELECT 
		td.id
		,td.Seq
		,td.OperationID
		from TimeStudy_Detail td WITH(NOLOCK)
		where  td.OperationID LIKE '-%' and td.smv = 0
	)
	OperatorIDss 
	WHERE ID =  TS.ID AND SEQ <= (SELECT TOP 1 Seq FROM TimeStudy_Detail WHERE id = TS.ID AND OperationID = LMD.OperationID ORDER BY Seq DESC)
	ORDER BY SEQ DESC
)OP
OUTER APPLY
(
	SELECT val = R.[NAME]
	FROM Operation O WITH(NOLOCK)
	LEFT JOIN Reason R WITH(NOLOCK) ON R.ReasonTypeID = 'IE_Component' AND R.ID = SUBSTRING(O.ID,6,2)
	WHERE O.ID = lmd.OperationID 
)PartID
Where 1=1 and (e.ID is not null or e.id <> '')
and e.junk = 0
and lm.Status = 'Confirmed'
{sqlWhere}
ORDER by OperationCode,Style,Season,Brand,Version
            
SELECT *
INTO #P06
from #tmpP06 t
where exists
(
	select *
	from SewingSchedule ss
	join Orders o on ss.OrderID = o.ID
	where 1=1
	{sqlwhereTmp}
	and o.StyleID = t.Style and o.SeasonID = t.Season and o.BrandID = t.Brand
    )

select *
from #P03
UNION ALL
select *
from #P06

drop table #tmpP03, #tmpp06 ,#P03 , #P06           
            ";

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), listParameter, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            this.ShowWaitMessage("Excel Processing...");

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\IE_R09.xltx"); // 預先開啟excel app

            MyUtility.Excel.CopyToXls(this.printData, null, "IE_R09.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("IE_R09");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
