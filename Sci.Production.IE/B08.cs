using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Drawing.Printing;
using Ict.Win;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_B08
    /// </summary>
    public partial class B08 : Win.Tems.Input1
    {
        private DataTable dtOperatorDetail;
        private DataTable dtDetail;
        private int itemCount;
        /// <summary>
        /// B08
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.Helper.Controls.Grid.Generator(this.gridDetail)
            .Text("ST_MC_Type", header: "ST/MC Type", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Motion", header: "Motion", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Effi_90_day", header: "90D Effi %", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Effi_180_day", header: "180D Effi %", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Effi_270_day", header: "270D Effi %", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Effi_360_day", header: "360D Effi %", width: Widths.AnsiChars(10), iseditingreadonly: true)
            ;
        }

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                string hasJunk = MyUtility.Check.Empty(this.queryfors.SelectedValue) ? string.Empty : this.queryfors.SelectedValue.ToString();
                switch (hasJunk)
                {
                    case "0":
                        this.DefaultWhere = $"JUNK = 0 {this.PAMS_Where()}";
                        break;
                    default:
                        this.DefaultWhere = string.Empty;
                        break;
                }

                this.ReloadDatas();
            };
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            string sqlcmd = this.GetOperationHistoryEff(MyUtility.Convert.GetString(this.CurrentMaintain["FactoryID"]), MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.dtOperatorDetail);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return;
            }

            this.btnOperationHistory.ForeColor = this.dtOperatorDetail.Rows.Count > 0 ? Color.Blue : Color.Black;

            string sqlcmdDetail = this.GetOperationStageEffSQL(MyUtility.Convert.GetString(this.CurrentMaintain["FactoryID"]), MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DualResult result1 = DBProxy.Current.Select(null, sqlcmdDetail, out this.dtDetail);

            if (!result1)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return;
            }

            this.gridDetail.DataSource = this.dtDetail;

            this.chP03.Checked = MyUtility.Convert.GetString(this.CurrentMaintain["IE_P03"]) == "Y" ? true : false;
            this.chP06.Checked = MyUtility.Convert.GetString(this.CurrentMaintain["IE_P06"]) == "Y" ? true : false;
        }

        /// <summary>
        /// ClickSaveBefore()
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            if (this.txtSkill.TextLength > 200)
            {
                MyUtility.Msg.WarningBox($@"
Can only check up to 40 Skills!
You have checked <{this.itemCount}> Skills!!");
                return false;
            }

            this.txtSkill.BackColor = this.displayM.BackColor;
            return base.ClickSaveBefore();
        }

        /// <summary>
        /// ClickNewAfter()
        /// </summary>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.txtSkill.BackColor = Color.White;
        }

        /// <summary>
        /// ClickEditAfter()
        /// </summary>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.displayM.ReadOnly = true;
            this.txtFactory.ReadOnly = true;
            this.txtID.ReadOnly = true;
            this.txtLastName.ReadOnly = true;
            this.txtFirstName.ReadOnly = true;
            this.txtDept.ReadOnly = true;
            this.txtPosition.ReadOnly = true;
            this.txtSection.ReadOnly = true;
            this.dateHiredOn.ReadOnly = true;
            this.dateResigned.ReadOnly = true;
            this.chkJunk.ReadOnly = true;
            this.txtSkill.BackColor = Color.White;
        }

        /// <summary>
        /// ClickPrint()
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickPrint()
        {
            DataTable browseData = (DataTable)this.gridbs.DataSource;
            if (browseData == null || browseData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return false;
            }

            string strXltName = Env.Cfg.XltPathDir + "\\IE_P08.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            object[,] objArray = new object[1, 11];
            int intRowsStart = 2;
            int rownum = 0;
            for (int i = 0; i < browseData.Rows.Count; i++)
            {
                DataRow dr = browseData.Rows[i];
                rownum = intRowsStart + i;
                objArray[0, 0] = dr["MDivisionID"];
                objArray[0, 1] = dr["FactoryID"];
                objArray[0, 2] = dr["ID"];
                objArray[0, 3] = dr["LastName1"];
                objArray[0, 4] = dr["FirstName1"];
                objArray[0, 5] = dr["Dept"];
                objArray[0, 6] = dr["Position"];
                objArray[0, 7] = dr["Section1"];
                objArray[0, 8] = dr["Skill"];
                objArray[0, 9] = dr["OnBoardDate"];
                objArray[0, 10] = dr["ResignationDate"];

                worksheet.Range[string.Format("A{0}:K{0}", rownum)].Value2 = objArray;
            }

            string strExcelName = Class.MicrosoftFile.GetName("IE_P08");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            return base.ClickPrint();
        }

        /// <summary>
        /// ClickUndo()
        /// </summary>
        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.txtSkill.BackColor = this.displayM.BackColor;
        }

        private void TxtSkill_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                DataTable machineGroup;
                DualResult returnResule = DBProxy.Current.Select("Machine", "select [ID] = MasterGroupID + ID ,Description from MachineGroup WITH (NOLOCK)	where Junk = 0 order by MasterGroupID,ID", out machineGroup);
                Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(machineGroup, "ID,Description", "Group ID,Description", "2,35", this.txtSkill.Text);

                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }
                this.itemCount = item.GetSelecteds().Count;
                string returnData = string.Empty;
                IList<DataRow> gridData = item.GetSelecteds();
                foreach (DataRow currentRecord in gridData)
                {
                    returnData = returnData + currentRecord["ID"].ToString() + ",";
                }

                this.CurrentMaintain["Skill"] = returnData.ToString();
            }
        }

        private string PAMS_Where()
        {
            string strDept = string.Empty;
            string strPosition = string.Empty;
            string strWhere = string.Empty;
            //switch (Env.User.Factory)
            //{
            //    case "MAI":
            //    case "MA2":
            //    case "MA3":
            //    case "MW2":
            //    case "FIT":
            //    case "MWI":
            //    case "FAC":
            //    case "FA2":
            //    case "PSR":
            //    case "VT1":
            //    case "VT2":
            //    case "GMM":
            //    case "GM2":
            //    case "GMI":
            //    case "PS2":
            //    case "ALA":
            //        strDept = $"'PRO'";
            //        strPosition = $"'PCK','PRS','SEW','FSPR','LOP','STL','LL','SLS','SSLT'";
            //        strWhere = $@" and Dept in({strDept})  and Position in({strPosition})";
            //        break;
            //    case "ESP":
            //    case "ES2":
            //    case "ES3":
            //    case "VSP":
            //        strDept = $"'PRO'";
            //        strPosition = $"'PAC','PRS','SEW','LL'";
            //        strWhere = $@" and Dept in({strDept})  and Position in({strPosition})";
            //        break;
            //    case "SPT":
            //        strDept = $"'PRO'";
            //        strPosition = $"'PAC','PRS','SEW','LL','SUP','PE','PIT','TL'";
            //        strWhere = $@" and Dept in({strDept})  and Position in({strPosition})";
            //        break;
            //    case "SNP":
            //        strDept = $"'PRO'";
            //        strPosition = $"'SEW','LL','PIT'";
            //        strWhere = $@" and Dept in({strDept})  and Position in({strPosition})";
            //        break;
            //    case "SPS":
            //    case "SPR":
            //        strDept = $"'SEW'";
            //        strPosition = $"'SWR','TRNEE','Lneldr','LINSUP','PRSSR','PCKR'";
            //        strWhere = $@" and Dept in({strDept})  and Position in({strPosition})";
            //        break;
            //}

            return strWhere;
        }

        private void B08_FormLoaded(object sender, EventArgs e)
        {
            MyUtility.Tool.SetupCombox(this.queryfors, 2, 1, "0,Exclude Junk,1,Include Junk");

            // 預設查詢為 Exclude Junk
            this.queryfors.SelectedIndex = 0;
            this.DefaultWhere = $"JUNK = 0 {this.PAMS_Where()}";
            this.ReloadDatas();
        }

        /// <inheritdoc/>
        private void BtnOperationHistory_Click(object sender, EventArgs e)
        {
            B08_Operation b08_Operation = new B08_Operation(this.dtOperatorDetail);
            b08_Operation.ShowDialog(this);
        }

        /// <summary>
        /// GetOperationHistoryEff
        /// </summary>
        /// <param name="factoryID">factoryID</param>
        /// <param name="employeeID">employeeID</param>
        /// <returns>sql</returns>
        public string GetOperationHistoryEff(string factoryID, string employeeID)
        {
            string sql = $@"
-- 整合查詢
DECLARE @goDate dateTime = getdate(); -- '2024-08-12'

SELECT
[ST_MC_Type] = ISNULL(lmd.MachineTypeID,''),
[Motion] = ISNULL(Operation_P03.val,''),
[Group_Header] =  ISNULL(IIF(REPLACE(tsd.[location], '--', '') = '', REPLACE(OP.OperationID, '--', '') ,REPLACE(tsd.[location], '--', '')),''),
[Part] = PartID.val, --ISNULL(lmd.SewingMachineAttachmentID,''),
[Attachment] = ISNULL(lmd.Attachment,'') + ' ' + ISNULL(lmd.Template,'')
,lmd.GSD
,[Cycle] = lmd.Cycle *1.0
INTO #P03
FROM Employee e WITH(NOLOCK)
INNER JOIN LineMapping_Detail lmd WITH(NOLOCK) ON lmd.EmployeeID = e.ID
INNER JOIN LineMapping lm_Day WITH(NOLOCK) ON lm_Day.id = lmd.ID  AND ((lm_Day.EditDate >= DATEADD(YEAR, -3, @goDate) AND lm_Day.EditDate <= @goDate) OR (lm_Day.AddDate >= DATEADD(YEAR, -3, @goDate) AND lm_Day.AddDate <= @goDate))
OUTER APPLY (
    SELECT val = STUFF((
        SELECT DISTINCT CONCAT(',', Name)
        FROM OperationRef a WITH(NOLOCK)
        INNER JOIN IESelectCode b WITH(NOLOCK) ON a.CodeID = b.ID AND a.CodeType = b.Type
        WHERE a.CodeType = '00007' AND a.id = lmd.OperationID  
        FOR XML PATH('')), 1, 1, '')
) Operation_P03
OUTER APPLY(
	select TOP 1 tsd.Location,tsd.ID
	from TimeStudy TS
	inner join TimeStudy_Detail tsd WITH(NOLOCK) ON tsd.id = ts.id
	where TS.StyleID = lm_Day.StyleID AND TS.SeasonID = lm_Day.SeasonID AND TS.ComboType = lm_Day.ComboType AND TS.BrandID = lm_Day.BrandID
	and lmd.OperationID = tsd.OperationID

)tsd
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
	WHERE ID =  tsd.ID AND SEQ <= (SELECT TOP 1 Seq FROM TimeStudy_Detail WHERE id = tsd.ID AND OperationID = LMD.OperationID ORDER BY Seq DESC)
	ORDER BY SEQ DESC
)OP
OUTER APPLY
(
	SELECT val = R.[NAME]
	FROM Operation O WITH(NOLOCK)
	LEFT JOIN Reason R WITH(NOLOCK) ON R.ReasonTypeID = 'IE_Component' AND R.ID = SUBSTRING(O.ID,6,2)
	WHERE O.ID = lmd.OperationID 
)PartID
WHERE e.FactoryID = '{factoryID}' AND e.ID = '{employeeID}' AND (ISNULL(lmd.MachineTypeID,'') != '')

SELECT
[ST_MC_Type] = ISNULL(lmbd.MachineTypeID,''),
[Motion] = ISNULL(Operation_P06.val,''),
[Group_Header] =  ISNULL(IIF(REPLACE(tsd.[location], '--', '') = '', REPLACE(OP.OperationID, '--', '') ,REPLACE(tsd.[location], '--', '')),''),
[Part] = PartID.val, --ISNULL(lmd.SewingMachineAttachmentID,''),
[Attachment] = ISNULL(lmbd.Attachment,'') + ' ' + ISNULL(lmbd.Template,'')
,lmbd.GSD
,[Cycle] = lmbd.Cycle * lmbd.SewerDiffPercentage
INTO #P06
FROM Employee e WITH(NOLOCK)
INNER JOIN LineMappingBalancing_Detail lmbd WITH(NOLOCK) ON lmbd.EmployeeID = e.ID
INNER JOIN LineMappingBalancing lmb_Day WITH(NOLOCK) ON lmb_Day.id = lmbd.ID  AND ((lmb_Day.EditDate >= DATEADD(YEAR, -3, @goDate) AND lmb_Day.EditDate <= @goDate) OR (lmb_Day.AddDate >= DATEADD(YEAR, -3, @goDate) AND lmb_Day.AddDate <= @goDate))
OUTER APPLY (
    SELECT val = STUFF((
        SELECT DISTINCT CONCAT(',', Name)
        FROM OperationRef a WITH(NOLOCK)
        INNER JOIN IESELECTCODE b WITH(NOLOCK) ON a.CodeID = b.ID AND a.CodeType = b.Type
        WHERE a.CodeType = '00007' AND a.id = lmbd.OperationID  
        FOR XML PATH('')), 1, 1, '')
) Operation_P06
OUTER APPLY(
	select TOP 1 tsd.Location,tsd.ID
	from TimeStudy TS
	inner join TimeStudy_Detail tsd WITH(NOLOCK) ON tsd.id = ts.id
	where TS.StyleID = lmb_Day.StyleID AND TS.SeasonID = lmb_Day.SeasonID AND TS.ComboType = lmb_Day.ComboType AND TS.BrandID = lmb_Day.BrandID
	and lmbd.OperationID = tsd.OperationID
)tsd
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
	WHERE ID =  tsd.ID AND SEQ <= (SELECT TOP 1 Seq FROM TimeStudy_Detail WHERE id = tsd.ID AND OperationID = lmbd.OperationID ORDER BY Seq DESC)
	ORDER BY SEQ DESC
)OP
OUTER APPLY
(
	SELECT val = R.[NAME]
	FROM Operation O WITH(NOLOCK)
	LEFT JOIN Reason R WITH(NOLOCK) ON R.ReasonTypeID = 'IE_Component' AND R.ID = SUBSTRING(O.ID,6,2)
	WHERE O.ID = lmbd.OperationID 
)PartID
WHERE e.FactoryID = '{factoryID}' AND e.ID = '{employeeID}' AND (ISNULL(lmbd.MachineTypeID,'') != '')
;
SELECT a.[ST_MC_Type]
,a.[Motion]
,a.[Group_Header]
,a.[Part]
,a.[Attachment]	
,[Effi_3_year] = ISNULL(FORMAT(((SUM(a.GSD) / SUM(a.Cycle))*100), '0.00'), '0.00')
FROM (
	select *
	from #P03
	UNION ALL
	select *
	from #P06
)a
GROUP BY [ST_MC_Type]
,[Motion]
,[Group_Header]
,[Part]
,[Attachment]


drop table #P03,#P06
            ";
            return sql;
        }

        /// <summary>
        /// 取得效率表，B08表身
        /// </summary>
        /// <param name="factoryID">工廠</param>
        /// <param name="employeeID">emp</param>
        /// <returns>sql</returns>
        public string GetOperationStageEffSQL(string factoryID, string employeeID)
        {
            string sql = $@"

DECLARE @goDate date = getdate(); -- '2024-08-12'

DECLARE @DataRangeTable as table(
	Title int,
	minDays int,
	maxDays int
)
insert @DataRangeTable (Title,minDays,maxDays)
values (90,0,90)
	,(180,0,180)
	,(270,0,270)
	,(360,0,360)

SELECT
	  [ST_MC_Type] = ISNULL(lmd.MachineTypeID,'')
	, [Motion] = ISNULL(Operation_P03.val,'')
	, [DiffDays] = DATEDIFF(DAY,lm_Day.EditDate,@goDate)
	, lmd.GSD 
	, [Cycle] = lmd.Cycle * 1.0
INTO #DetailP03
FROM Employee e WITH(NOLOCK)
INNER JOIN LineMapping_Detail lmd WITH(NOLOCK) ON lmd.EmployeeID = e.ID
INNER JOIN LineMapping lm_Day WITH(NOLOCK) ON lm_Day.id = lmd.ID
OUTER APPLY (
	SELECT val = STUFF((
	SELECT DISTINCT CONCAT(',', Name)
	FROM OperationRef a WITH(NOLOCK)
	INNER JOIN IESELECTCODE b WITH(NOLOCK) ON a.CodeID = b.ID AND a.CodeType = b.Type
	WHERE a.CodeType = '00007' AND a.id = lmd.OperationID  
	FOR XML PATH('')), 1, 1, '')
) Operation_P03
WHERE e.FactoryID = '{factoryID}' AND e.ID = '{employeeID}' AND (ISNULL(lmd.MachineTypeID,'') != '')
ORDER BY lmd.MachineTypeID,Operation_P03.val;

--------------------------------------------
SELECT
[ST_MC_Type] = ISNULL(lmbd.MachineTypeID,'')
,[Motion] = ISNULL(Operation_P06.val,'')
	,[DiffDays] = DATEDIFF(DAY,lmb_Day.EditDate,@goDate)
,lmbd.GSD 
,[Cycle] = lmbd.Cycle * lmbd.SewerDiffPercentage
INTO #DetailP06
FROM Employee e WITH(NOLOCK)
INNER JOIN LineMappingBalancing_Detail lmbd WITH(NOLOCK) ON lmbd.EmployeeID = e.ID
INNER JOIN LineMappingBalancing lmb_Day WITH(NOLOCK) ON lmb_Day.id = lmbd.ID
OUTER APPLY (
	SELECT val = STUFF((
	SELECT DISTINCT CONCAT(',', Name)
	FROM OperationRef a WITH(NOLOCK)
	INNER JOIN IESELECTCODE b WITH(NOLOCK) ON a.CodeID = b.ID AND a.CodeType = b.Type
	WHERE a.CodeType = '00007' AND a.id = lmbd.OperationID  
	FOR XML PATH('')), 1, 1, '')
) Operation_P06
WHERE e.FactoryID = '{factoryID}' AND e.ID = '{employeeID}' AND (ISNULL(lmbd.MachineTypeID,'') != '')
ORDER BY lmbd.MachineTypeID,Operation_P06.val;


select a.ST_MC_Type
	,a.Motion
	,a.Title
	,[Eff] = CAST(  SUM(GSD)/SUM(Cycle) as numeric(7,4))
INTO #FinalP03
from (
	select d.ST_MC_Type,d.Motion
	,[Title] = 'Effi_' + cast( rt.Title as varchar) +'_day'
	,d.GSD,d.Cycle
	from #DetailP03 d
	inner join @DataRangeTable rt on d.DiffDays <= rt.maxDays and rt.Title=90
	UNION ALL
	select d.ST_MC_Type,d.Motion
	,[Title] = 'Effi_' + cast( rt.Title as varchar) +'_day'
	,d.GSD,d.Cycle
	from #DetailP03 d
	inner join @DataRangeTable rt on d.DiffDays <= rt.maxDays and rt.Title=180
	UNION ALL
	select d.ST_MC_Type,d.Motion
	,[Title] = 'Effi_' + cast( rt.Title as varchar) +'_day'
	,d.GSD,d.Cycle
	from #DetailP03 d
	inner join @DataRangeTable rt on d.DiffDays <= rt.maxDays and rt.Title=270
	UNION ALL
	select d.ST_MC_Type,d.Motion
	,[Title] = 'Effi_' + cast( rt.Title as varchar) +'_day'
	,d.GSD,d.Cycle
	from #DetailP03 d
	inner join @DataRangeTable rt on d.DiffDays <= rt.maxDays and rt.Title=360
	UNION ALL
	select d.ST_MC_Type,d.Motion
	,[Title] = 'Effi_' + cast( rt.Title as varchar) +'_day'
	,d.GSD,d.Cycle
	from #DetailP06 d
	inner join @DataRangeTable rt on d.DiffDays <= rt.maxDays and rt.Title=90
	UNION ALL
	select d.ST_MC_Type,d.Motion
	,[Title] = 'Effi_' + cast( rt.Title as varchar) +'_day'
	,d.GSD,d.Cycle
	from #DetailP06 d
	inner join @DataRangeTable rt on d.DiffDays <= rt.maxDays and rt.Title=180
	UNION ALL
	select d.ST_MC_Type,d.Motion
	,[Title] = 'Effi_' + cast( rt.Title as varchar) +'_day'
	,d.GSD,d.Cycle
	from #DetailP06 d
	inner join @DataRangeTable rt on d.DiffDays <= rt.maxDays and rt.Title=270
	UNION ALL
	select d.ST_MC_Type,d.Motion
	,[Title] = 'Effi_' + cast( rt.Title as varchar) +'_day'
	,d.GSD,d.Cycle
	from #DetailP06 d
	inner join @DataRangeTable rt on d.DiffDays <= rt.maxDays and rt.Title=360
)a
GROUP BY a.ST_MC_Type,a.Motion,a.Title;

SELECT 
    ST_MC_Type,
    Motion,
    ISNULL([Effi_90_day], 0) AS Effi_90_day,
    ISNULL([Effi_180_day], 0) AS Effi_180_day,
    ISNULL([Effi_270_day], 0) AS Effi_270_day,
    ISNULL([Effi_360_day], 0) AS Effi_360_day
---INTO #PivotTableP03
FROM 
    (SELECT ST_MC_Type, Motion, Title, CAST( Eff * 100 AS NUMERIC(5,2)) AS Eff -- 效率轉換為百分比
     FROM #FinalP03) AS SourceTable
PIVOT 
    (MAX(Eff) ---- 已經確定會是唯一值，所以放心取max
     FOR Title IN ([Effi_90_day], [Effi_180_day], [Effi_270_day], [Effi_360_day])
    ) AS PivotTable
ORDER BY  ST_MC_Type, Motion;
-----------------

drop table #DetailP03,#FinalP03,#DetailP06";
            return sql;
        }
    }
}
