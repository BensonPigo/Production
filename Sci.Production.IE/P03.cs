﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;
using Sci.Production.Class;
using System.Data.SqlClient;
using System.Reflection;
using Sci.Production.Prg;
using Sci.Win.Tools;
using Microsoft.SqlServer.Management.Smo.Agent;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P03
    /// </summary>
    public partial class P03 : Win.Tems.Input6
    {
        private string selectDataTable_DefaultView_Sort = string.Empty;
        private object totalGSD;
        private object totalCycleTime;
        private DataTable EmployeeData;
        private DataTable distdt;
        private List<GridList> ConfirmLists;
        private bool ConfirmColor = false;

        /// <summary>
        /// P03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.detailgrid.AllowUserToOrderColumns = true;
            this.InsertDetailGridOnDoubleClick = false;
            this.comboSewingTeam1.SetDataSource();
            this.splitContainer1.Panel1.Controls.Add(this.detailpanel);
            this.detailpanel.Dock = DockStyle.Fill;
            MyUtility.Tool.SetupCombox(this.comboPhase, 1, 1, ",Initial,Prelim,Final");
            this.gridicon.Append.Image = Image.FromFile(Application.StartupPath + "\\data\\Copy.png");
            this.gridicon.Append.ImageAlign = ContentAlignment.MiddleCenter;
            this.gridicon.Append.TextImageRelation = TextImageRelation.ImageBeforeText;
        }

        /// <summary>
        /// P03
        /// </summary>
        /// <param name="styleID"></param>
        /// <param name="brandID"></param>
        /// <param name="seasonID"></param>
        /// <param name="isReadOnly"></param>
        public P03(string styleID, string brandID, string seasonID, bool isReadOnly = false)
        {
            this.InitializeComponent();
            StringBuilder df = new StringBuilder();
            df.Append(string.Format("StyleID = '{0}' ", styleID));
            if (!MyUtility.Check.Empty(brandID))
            {
                df.Append(string.Format(" and BrandID ='{0}' ", brandID));
            }

            if (!MyUtility.Check.Empty(seasonID))
            {
                df.Append(string.Format(" and SeasonID ='{0}' ", seasonID));
            }

            if (isReadOnly)
            {
                this.IsSupportEdit = false;
                this.IsSupportNew = false;
                this.IsSupportConfirm = false;
                this.IsSupportCopy = false;
                this.IsSupportDelete = false;
                this.IsDeleteOnBrowse = false;
            }
            else
            {
                this.IsSupportEdit = true;
                this.IsSupportNew = true;
                this.IsSupportConfirm = true;
                this.IsSupportCopy = true;
                this.IsSupportDelete = true;
                this.IsDeleteOnBrowse = true;
            }

            this.DefaultFilter = df.ToString();
            this.detailgrid.AllowUserToOrderColumns = true;
            this.InsertDetailGridOnDoubleClick = false;
            this.comboSewingTeam1.SetDataSource();
            this.splitContainer1.Panel1.Controls.Add(this.detailpanel);
            this.detailpanel.Dock = DockStyle.Fill;
            MyUtility.Tool.SetupCombox(this.comboPhase, 1, 1, ",Initial,Prelim,Final");
            this.gridicon.Append.Image = Image.FromFile(Application.StartupPath + "\\data\\Copy.png");
            this.gridicon.Append.ImageAlign = ContentAlignment.MiddleCenter;
            this.gridicon.Append.TextImageRelation = TextImageRelation.ImageBeforeText;
        }
        /// <summary>
        /// ClickLocate
        /// </summary>
        protected override void ClickLocate()
        {
            base.ClickLocate();
            this.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            MyUtility.Tool.SetupCombox(this.queryfors, 1, 1, "last 2 years modify,All");
            this.queryfors.SelectedIndex = 0;
            if (MyUtility.Check.Empty(this.DefaultWhere) && MyUtility.Check.Empty(this.DefaultFilter))
            {
                this.DefaultWhere = " AddDate >= DATEADD(YY,-2,GETDATE()) OR EditDate >= DATEADD(YY,-2,GETDATE())";
            }
            else
            {
                this.queryfors.SelectedIndex = 1;
            }

            base.OnFormLoaded();

            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = " AddDate >= DATEADD(YY,-2,GETDATE()) OR EditDate >= DATEADD(YY,-2,GETDATE())";
                        break;
                    case 1:
                    default:
                        this.DefaultWhere = string.Empty;
                        break;
                }

                // 請參考IE P01註解
                if (this.QBCommand != null && this.QBCommand.Conditions.Count() == 0)
                {
                    this.QueryExpress = string.Empty;
                }

                this.ReloadDatas();

                GC.Collect();
            };

            this.grid.Columns["ID"].Visible = false;
        }

        /// <summary>
        /// OnDetailSelectCommandPrepare
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            string factoryID = (e.Master == null) ? string.Empty : e.Master["FactoryID"].ToString();
            this.DetailSelectCommand =
                $@"
select *
    , [sortNO] = case when ld.IsHide = 1 then 1 
                      when ld.No = '' and ld.IsHide = 0 and ld.IsPPA = 0 then 2
                      when left(ld.No, 1) = 'P' then 3
                      else 4 end
    ,[EstCycleTime] = iif(ld.OperatorEffi = '0.00','0.00',ld.GSD / ld.OperatorEffi * 100)
	,[EstTotalCycleTime] = IIF((AVG(CAST(ld.OperatorEffi AS FLOAT)) OVER (PARTITION BY ld.No)) = 0, 0, (SUM(CAST(ld.GSD AS FLOAT)) OVER (PARTITION BY ld.No)) / (AVG(CAST(ld.OperatorEffi AS FLOAT)) OVER (PARTITION BY ld.No)) * 100)
	,[EstOutputHr] = iif(CAST(ld.OperatorEffi AS FLOAT) = 0,0, 3600 / IIF((AVG(CAST(ld.OperatorEffi AS FLOAT)) OVER (PARTITION BY ld.No)) = 0, 0, (SUM(CAST(ld.GSD AS FLOAT)) OVER (PARTITION BY ld.No)) / (AVG(CAST(ld.OperatorEffi AS FLOAT)) OVER (PARTITION BY ld.No)) * 100))

	--,[EstTotalCycleTime] = CAST(IIF(
    --                                   AVG(CAST(ld.OperatorEffi AS DECIMAL)) OVER (PARTITION BY ld.EmployeeID, ld.No) = 0, 
    --                                   0.0, 
    --                                   ld.TotalGSDNO / AVG(CAST(ld.OperatorEffi AS DECIMAL)) OVER (PARTITION BY ld.EmployeeID, ld.No) * 100
    --                               ) AS DECIMAL(20, 5))
    --
    --   ,[EstOutputHr] = CAST(IIF(
    --                               AVG(CAST(ld.OperatorEffi AS DECIMAL)) OVER (PARTITION BY ld.EmployeeID, ld.No) = 0, 
    --                               0.0, 
    --                               3600 / (ld.TotalGSDNO / AVG(CAST(ld.OperatorEffi AS DECIMAL)) OVER (PARTITION BY ld.EmployeeID, ld.No) * 100)
    --                           ) AS DECIMAL(20, 5)) 
from (
    select  ld.OriNO
	    , ld.No
	    , ld.IsPPA
        , PPAText = ISNULL(d.Name,'')
	    , ld.PPA
        , [IsHide] = cast(iif(ld.IsHide is not null, ld.IsHide ,iif(SUBSTRING(ld.OperationID, 1, 2) = '--', 1, iif(show.IsDesignatedArea = 1, 1, 0))) as bit)	    
	    , ld.MachineTypeID
	    , ld.MasterPlusGroup	
        , [Description]= IIF( o.DescEN = '' OR  o.DescEN IS NULL , ld.OperationID,o.DescEN)
	    , ld.Annotation
	    , ld.Template
	    , ld.Attachment
	    , ld.ThreadColor
	    , ld.Notice
	    , ld.EmployeeID
	    , e.Name as EmployeeName
        , e.Skill as EmployeeSkill
	    , iif(ld.Cycle = 0,0,ROUND(ld.GSD/ld.Cycle,2)*100) as Efficiency
        , [IsGroupHeader] = cast(iif(SUBSTRING(ld.OperationID, 1, 2) = '--', 1, 0) as bit)
        , [IsShow] = cast(iif( ld.OperationID like '--%' , 1, isnull(show.IsShowinIEP03, 1)) as bit)
	    , ld.ID
	    , ld.GroupKey
	    , ld.Ukey
	    , ld.ActCycle
	    , ld.TotalGSD
	    , ld.TotalCycle
	    , ld.GSD
	    , ld.Cycle
        , ld.OperationID
        , ld.New
        , ld.SewingMachineAttachmentID
        , ld.MoldID
        , ld.MachineCount
        , IsMachineTypeID_MM = Cast( IIF( ld.MachineTypeID like 'MM%',1,0) as bit)
        , [ReasonName] = lbr.Name
        , [EmployeeJunk] = e.junk
        , [IsRow] = ROW_NUMBER() OVER(PARTITION BY ld.EmployeeID,ld.Ukey ORDER by e.Junk asc) 
		, [OperatorEffi] = isnull(iif(Effi.Effi_3_year = '' or Effi.Effi_3_year is null ,Effi_90_day.Effi_90_day,Effi.Effi_3_year) ,'0.00')
		, [TotalGSDNO] = sum(ld.GSD) OVER (PARTITION BY ld.No)
		, [Motion] = Motion.val
    from LineMapping_Detail ld WITH (NOLOCK) 
    left join Employee e WITH (NOLOCK) on ld.EmployeeID = e.ID
    left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID
    left join IEReasonLBRNotHit_Detail lbr WITH (NOLOCK) on ld.IEReasonLBRNotHit_DetailUkey = lbr.Ukey
    left join DropDownList d (NOLOCK) on d.ID=ld.PPA AND d.Type = 'PMS_IEPPA'
	left JOIN LineMapping lm WITH(NOLOCK) on lm.id = ld.ID
	INNER JOIN TimeStudy TS WITH(NOLOCK) ON TS.StyleID = lm.StyleID AND TS.SeasonID = lm.SeasonID AND TS.ComboType = lm.ComboType AND TS.BrandID = lm.BrandID
	OUTER APPLY
	(
		SELECT TOP 1
		val = OperatorIDss.OperationID
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
		WHERE ID =  TS.ID AND SEQ <= (SELECT TOP 1 Seq FROM TimeStudy_Detail WHERE id = TS.ID AND OperationID = LD.OperationID ORDER BY Seq DESC)
		ORDER BY SEQ DESC
	)Group_Header
    OUTER APPLY
	(
		select val = stuff((select distinct concat(',',Name)
		from OperationRef a
		inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
		where a.CodeType = '00007' and a.id = ld.OperationID  for xml path('') ),1,1,'')
	)Motion
    outer apply (
	    select IsShowinIEP03 = IIF(isnull(md.IsNotShownInP03 ,0) = 0, 1, 0)
		    , IsDesignatedArea = ISNULL(md.IsNonSewingLine,0)
	    from Operation o2 WITH (NOLOCK) 
	    inner join MachineType m WITH (NOLOCK) on o2.MachineTypeID = m.ID
        inner join MachineType_Detail md WITH (NOLOCK) on md.ID = m.ID and md.FactoryID = '{factoryID}'
	    where o.ID = o2.ID and m.junk = 0
    )show
    outer APPLY
	(
		SELECT val = SUM(ld2.GSD) 
        FROM LineMapping_Detail ld2 
        WHERE ld2.No = ld.No
	)TotlGSFD
	OUTER APPLY
    (
	    SELECT
	    [ST_MC_Type]
	    ,[Motion]
	    ,[Effi_90_day] =isnull(FORMAT(AVG(CAST([Effi_90_day] AS DECIMAL(10, 2))), '0.00'),'0')
	    From
	    (
		    SELECT 
		    [ST_MC_Type] =lmd.MachineTypeID
		    ,[Motion] = Operation_P03.val
		    ,Effi_90_day = FORMAT(CAST(iif(LM.ID is null or LMD.Cycle = 0,0,ROUND(lmd.GSD/ lmd.Cycle * 100,2)) AS DECIMAL(10, 2)), '0.00')
		    from Employee eo
		    left JOIN LineMapping_Detail lmd WITH(NOLOCK) on lmd.EmployeeID = eo.ID　
		    left JOIN LineMapping lm WITH(NOLOCK) on lm.id = lmd.ID
		    OUTER APPLY
		    (
			    select val = stuff((select distinct concat(',',Name)
			    from OperationRef a
			    inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
			    where a.CodeType = '00007' and a.id = lmd.OperationID  for xml path('') ),1,1,'')
		    )Operation_P03
		    WHERE 
		    eo.FactoryID = e.FactoryID and eo.ID = ld.EmployeeID AND
			lmd.MachineTypeID = ld.MachineTypeID and
			Operation_P03.val = Motion.val AND
		    ((lm.EditDate >= DATEADD(day, -90, GETDATE()) and lm.EditDate <= GETDATE()) or (lm.AddDate >= DATEADD(day, -90, GETDATE()) and lm.AddDate <= GETDATE()))
	    )a
	    GROUP BY [ST_MC_Type],[Motion]
    )Effi_90_day
    OUTER APPLY
    (
        SELECT
        [ST_MC_Type]
        ,[Motion]
        ,[Group_Header]
        ,[Part]
        ,[Attachment]
        ,[Effi_3_year] =isnull(FORMAT(AVG(CAST([Effi_3_year] AS DECIMAL(10, 2))), '0.00'),'0.00')
        From
        (
            SELECT 
            [ST_MC_Type] =lmd.MachineTypeID
            ,[Motion] = Operation_P03.val
            ,[Group_Header] = ISNULL(IIF(REPLACE(tsd.[location], '--', '') = '', REPLACE(OP.OperationID, '--', '') ,REPLACE(tsd.[location], '--', '')),'')
            ,[Part] = lmd.SewingMachineAttachmentID
            ,[Attachment] = lmd.Attachment
            ,Effi_3_year = FORMAT(CAST(iif(lmd.Cycle = 0,0,ROUND(lmd.GSD/ lmd.Cycle * 100,2)) AS DECIMAL(10, 2)), '0.00')
            from Employee eo
            left JOIN LineMapping_Detail lmd WITH(NOLOCK) on lmd.EmployeeID = eo.ID　
            left JOIN LineMapping lm WITH(NOLOCK) on lm.id = lmd.ID
            left JOIN TimeStudy_Detail tsd WITH(NOLOCK) on lmd.OperationID = tsd.OperationID
            OUTER APPLY
            (
            select val = stuff((select distinct concat(',',Name)
		            from OperationRef a
		            inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
		            where a.CodeType = '00007' and a.id = lmd.OperationID  for xml path('') ),1,1,'')
            )Operation_P03
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
	        WHERE 
	        eo.FactoryID = e.FactoryID and eo.ID = ld.EmployeeID AND
		    lmd.MachineTypeID = ld.MachineTypeID and
		    Operation_P03.val = Motion.val AND
		    ISNULL(lmd.Attachment,'') = ISNULL(ld.Attachment,'') AND
		    lmd.SewingMachineAttachmentID = ld.SewingMachineAttachmentID AND
			ISNULL(IIF(REPLACE(tsd.[location], '--', '') = '', REPLACE(OP.OperationID, '--', '') ,REPLACE(tsd.[location], '--', '')),'') =  ISNULL(REPLACE(Group_Header.val, '--', ''),'') AND
		    ((lm.EditDate >= DATEADD(YEAR, -3, GETDATE()) and lm.EditDate <= GETDATE()) or (lm.AddDate >= DATEADD(YEAR, -3, GETDATE()) and lm.AddDate <= GETDATE()))
        )a
        GROUP BY [ST_MC_Type],[Motion], [Group_Header], [Part], [Attachment]
    )Effi
    where ld.ID = '{masterID}' 
)ld
where ld.EmployeeJunk is null or  (ld.EmployeeID is not null and ld.IsRow = 1)
order by case when ld.No = '' then 1
	    when left(ld.No, 1) = 'P' then 2
	    else 3 
	    end, 
        ld.GroupKey";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <summary>
        /// OnDetailEntered
        /// </summary>
        protected override void OnDetailEntered()
        {
            if (this.CurrentMaintain.Empty())
            {
                this.numCPUPC.Value = null;
                this.displayDesc.Text = string.Empty;
                this.numTargetHrIdeal.Value = null;
                this.numDailydemandshiftIdeal.Value = null;
                this.numTaktTimeIdeal.Value = null;
                this.numTotalTimeDiff.Value = null;
                this.numEOLR.Value = null;
                this.numHighestTimeDiff.Value = null;
                this.numEffieiency.Value = null;
                this.numPPH.Value = null;
                this.numLBR.Value = null;
                this.numLLER.Value = null;
                this.listControlBindingSource1.DataSource = null;
                this.btnNotHitTargetReason.Enabled = false;
                return;
            }

            base.OnDetailEntered();
            string sqlCmd = string.Format("select Description,CPU from Style WITH (NOLOCK) where Ukey = {0}", this.CurrentMaintain["StyleUkey"].ToString());
            DataRow styleData;
            if (MyUtility.Check.Seek(sqlCmd, out styleData))
            {
                this.displayDesc.Value = styleData["Description"];
                this.numCPUPC.Value = Convert.ToDecimal(styleData["CPU"]);
            }
            else
            {
                this.displayDesc.Value = string.Empty;
                this.numCPUPC.Value = 0;
            }

            string styleVersion = MyUtility.GetValue.Lookup($@"
select version 
from TimeStudy
where StyleID = '{this.CurrentMaintain["StyleID"]}'
and SeasonID= '{this.CurrentMaintain["SeasonID"]}'
and ComboType = '{this.CurrentMaintain["ComboType"]}'
and BrandID = '{this.CurrentMaintain["BrandID"]}'
");
            if (styleVersion != this.CurrentMaintain["TimeStudyVersion"].ToString() && this.EditMode == false)
            {
                this.labVersionWarning.Visible = true;
            }
            else
            {
                this.labVersionWarning.Visible = false;
            }

            this.CalculateValue(0);
            this.SaveCalculateValue();
            this.btnNotHitTargetReason.Enabled = !MyUtility.Check.Empty(this.CurrentMaintain["IEReasonID"]) || !MyUtility.Check.Empty(this.CurrentMaintain["IEReasonLBRnotHit_1stUkey"]);
            this.listControlBindingSource1.DataSource = this.distdt;

            var sumData = this.DetailDatas.AsEnumerable().Where(r => r["EstCycleTime"] != DBNull.Value && r["EstCycleTime"] != null).Sum(r => Convert.ToDecimal(r["EstCycleTime"]));
            var maxData = this.DetailDatas.AsEnumerable()
            .Where(r => r["EstCycleTime"] != DBNull.Value && r["EstCycleTime"] != null)
            .Select(r => Convert.ToDecimal(r["EstCycleTime"]))
            .DefaultIfEmpty()
            .Max();

            decimal? numEstLBRValue = 0;
            if (maxData != 0 && this.numlOprtsCurrent.Value != 0)
            {
                numEstLBRValue = sumData / maxData / this.numlOprtsCurrent.Value * 100;
            }

            this.numEstLBR.Value = numEstLBRValue;

            this.Distable();

            if (!MyUtility.Check.Empty(this.CurrentMaintain["TotalGSD"]) &&
                !MyUtility.Check.Empty(this.CurrentMaintain["CurrentOperators"]) &&
                !MyUtility.Check.Empty(this.CurrentMaintain["Workhour"]))
            {

                decimal decTotalGSD = MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSD"]);
                decimal decCurrentOperators = MyUtility.Convert.GetDecimal(this.CurrentMaintain["CurrentOperators"]);
                decimal decWorkhour = MyUtility.Convert.GetDecimal(this.CurrentMaintain["Workhour"]);

                decimal decTargetHr = 3600 * decCurrentOperators / decTotalGSD;
                decimal decDailyDemand_Shift = decTargetHr * decWorkhour;
                this.CurrentMaintain["TaktTime"] = Math.Round(3600 * decWorkhour / decDailyDemand_Shift, 2);
            }
            else
            {
                this.CurrentMaintain["TaktTime"] = 0;
            }
        }

        /// <summary>
        /// OnDetailGridSetup
        /// </summary>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Color backDefaultColor = this.detailgrid.DefaultCellStyle.BackColor;
            DataGridViewGeneratorTextColumnSettings no = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings cycle = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings machine = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings operatorid = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings operatorName = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings attachment = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings template = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings threadColor = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings notice = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings ppaText = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings ppa = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings hide = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings machineCount = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorTextColumnSettings operationID = new DataGridViewGeneratorTextColumnSettings();

            TxtMachineGroup.CelltxtMachineGroup txtSubReason = (TxtMachineGroup.CelltxtMachineGroup)TxtMachineGroup.CelltxtMachineGroup.GetGridCell();

            machineCount.HeaderAction = DataGridViewGeneratorCheckBoxHeaderAction.None;
            operationID.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                if (this.EditMode)
                {
                    if (e.Button == MouseButtons.Right && MyUtility.Convert.GetBool(dr["New"]))
                    {
                        if (e.RowIndex != -1)
                        {
                            P01_SelectOperationCode callNextForm = new P01_SelectOperationCode();
                            DialogResult result = callNextForm.ShowDialog(this);
                            if (result == DialogResult.Cancel)
                            {
                                if (callNextForm.P01SelectOperationCode != null)
                                {
                                    dr["Description"] = callNextForm.P01SelectOperationCode["DescEN"].ToString();
                                    dr["MachineTypeID"] = callNextForm.P01SelectOperationCode["MachineTypeID"].ToString();
                                    dr["Template"] = MyUtility.GetValue.Lookup($"select dbo.GetParseOperationMold('{callNextForm.P01SelectOperationCode["MoldID"]}', 'Template')");
                                    dr["Annotation"] = callNextForm.P01SelectOperationCode["Annotation"].ToString();
                                    dr["MasterPlusGroup"] = callNextForm.P01SelectOperationCode["MasterPlusGroup"].ToString();
                                    dr["GSD"] = callNextForm.P01SelectOperationCode["SMVsec"].ToString();
                                    dr.EndEdit();
                                }
                            }

                            if (result == DialogResult.OK)
                            {
                                dr["Description"] = callNextForm.P01SelectOperationCode["DescEN"].ToString();
                                dr["MachineTypeID"] = callNextForm.P01SelectOperationCode["MachineTypeID"].ToString();
                                dr["Template"] = MyUtility.GetValue.Lookup($"select dbo.GetParseOperationMold('{callNextForm.P01SelectOperationCode["MoldID"]}', 'Template')");
                                dr["Annotation"] = callNextForm.P01SelectOperationCode["Annotation"].ToString();
                                dr["MasterPlusGroup"] = callNextForm.P01SelectOperationCode["MasterPlusGroup"].ToString();
                                dr["GSD"] = callNextForm.P01SelectOperationCode["SMVsec"].ToString();
                                dr.EndEdit();
                            }
                        }
                    }
                }
            };
            #region No.的Valid
            no.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow[] selectrow = ((DataTable)this.detailgridbs.DataSource).Select(string.Format("No = '{0}'", e.FormattedValue.ToString().Trim().PadLeft(4, '0')));
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (selectrow.Length == 0)
                    {
                        dr["ActCycle"] = DBNull.Value;
                    }
                    else
                    {
                        dr["ActCycle"] = selectrow[0]["ActCycle"];
                    }

                    if (MyUtility.Check.Empty(e.FormattedValue) || (e.FormattedValue.ToString() != dr["No"].ToString()))
                    {
                        string oldValue = MyUtility.Check.Empty(dr["No"]) ? string.Empty : dr["No"].ToString();
                        dr["No"] = MyUtility.Check.Empty(e.FormattedValue) ? string.Empty : e.FormattedValue.ToString().Trim().PadLeft(4, '0');
                        dr.EndEdit();

                        this.ReclculateGridGSDCycleTime(oldValue);
                        this.ReclculateGridGSDCycleTime(dr["No"].ToString());
                        this.ComputeTaktTime();
                    }
                    this.Distable();
                    this.detailgrid.Focus();
                    this.detailgrid.CurrentCell = this.detailgrid[e.ColumnIndex, e.RowIndex];
                    this.detailgrid.BeginEdit(true);
                }
            };

            no.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.EditMode)
                {
                    return;
                }

                if (MyUtility.Convert.GetBool(dr["IsHide"]))
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
            };
            #endregion
            #region Cycle的Valid
            cycle.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["Cycle"] = 0;
                        dr["Efficiency"] = 0;
                    }
                    else
                    {
                        if (e.FormattedValue.ToString() != dr["Cycle"].ToString())
                        {
                            if (MyUtility.Convert.GetDecimal(e.FormattedValue) < 0)
                            {
                                dr["Cycle"] = 0;
                                dr["Efficiency"] = 0;
                                MyUtility.Msg.WarningBox("Cycle time can't less than 0!!");
                            }
                            else
                            {
                                dr["Cycle"] = MyUtility.Convert.GetDecimal(e.FormattedValue);
                                dr["Efficiency"] = Math.Round(MyUtility.Convert.GetDecimal(dr["GSD"]) / MyUtility.Convert.GetDecimal(dr["Cycle"]), 2) * 100;
                            }
                        }
                    }

                    dr.EndEdit();
                    this.ReclculateGridGSDCycleTime(MyUtility.Check.Empty(dr["No"]) ? string.Empty : dr["No"].ToString());
                    this.ComputeTaktTime();
                    this.Distable();
                    this.detailgrid.Focus();
                    this.detailgrid.CurrentCell = this.detailgrid[e.ColumnIndex, e.RowIndex];
                    this.detailgrid.BeginEdit(true);
                }
            };
            #endregion
            #region Operator ID No.的按右鍵與Validating
            operatorid.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (MyUtility.Check.Empty(this.txtFactory.Text))
                        {
                            MyUtility.Msg.WarningBox("Please fill in [Factory] first!");
                            return;
                        }

                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                            this.GetEmployee(null);
                            P03_Operator callNextForm = new P03_Operator(this.EmployeeData, MyUtility.Convert.GetString(this.CurrentMaintain["SewingLineID"]));
                            DialogResult result = callNextForm.ShowDialog(this);
                            if (result == DialogResult.Cancel)
                            {
                                return;
                            }

                            if (callNextForm.SelectOperator == null)
                            {
                                return;
                            }

                            DataTable dt = (DataTable)this.detailgridbs.DataSource;

                            DataRow[] errorDataRow = dt.Select($"EmployeeID = '{MyUtility.Convert.GetString(callNextForm.SelectOperator["ID"])}' and NO <> '{MyUtility.Convert.GetString(dr["No"])}'");
                            if (errorDataRow.Length > 0)
                            {
                                MyUtility.Msg.WarningBox($"<{callNextForm.SelectOperator["ID"]} {callNextForm.SelectOperator["Name"]}> already been used in No.{MyUtility.Convert.GetString(errorDataRow[0]["No"])}!!");
                                dr["EmployeeName"] = string.Empty;
                                return;
                            }

                            dt.DefaultView.Sort = this.selectDataTable_DefaultView_Sort == "ASC" ? "No ASC" : string.Empty;
                            DataTable sortDataTable = dt.DefaultView.ToTable();

                            DataRow[] listDataRows = sortDataTable.Select($"No = '{MyUtility.Convert.GetString(dr["No"])}'");
                            if (listDataRows.Length > 0)
                            {
                                foreach (DataRow dataRow in listDataRows)
                                {
                                    int dataIndex = sortDataTable.Rows.IndexOf(dataRow);
                                    DataRow row = this.detailgrid.GetDataRow<DataRow>(dataIndex);
                                    row["EmployeeID"] = callNextForm.SelectOperator["ID"];
                                    row["EmployeeName"] = callNextForm.SelectOperator["Name"];
                                    row["EmployeeSkill"] = callNextForm.SelectOperator["Skill"];
                                    row.EndEdit();
                                }
                            }

                            DataTable dt1 = (DataTable)this.listControlBindingSource1.DataSource;
                            dt1.DefaultView.Sort = this.selectDataTable_DefaultView_Sort == "ASC" ? "No ASC" : string.Empty;
                            DataTable sortDataTable1 = dt1.DefaultView.ToTable();
                            DataRow[] listDataRows1 = sortDataTable1.Select($"No = '{MyUtility.Convert.GetString(dr["No"])}'");
                            if (listDataRows1.Length > 0)
                            {
                                foreach (DataRow dataRow in listDataRows1)
                                {
                                    int dataIndex = sortDataTable1.Rows.IndexOf(dataRow);
                                    DataRow row = this.grid1.GetDataRow<DataRow>(dataIndex);
                                    row["EmployeeID"] = callNextForm.SelectOperator["ID"];
                                    row["EmployeeName"] = callNextForm.SelectOperator["Name"];
                                    row["EmployeeSkill"] = callNextForm.SelectOperator["Skill"];
                                    row.EndEdit();
                                }
                            }
                        }
                    }
                }
            };
            operatorid.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.ReviseEmployeeToEmpty(dr);
                        return;
                    }

                    if (e.FormattedValue.ToString() != dr["EmployeeID"].ToString())
                    {
                        this.GetEmployee(iD: e.FormattedValue.ToString());

                        if (this.EmployeeData == null)
                        {
                            return;
                        }

                        if (this.EmployeeData.Rows.Count <= 0)
                        {
                            this.ReviseEmployeeToEmpty(dr);
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Employee ID: {0} > not found!!!", e.FormattedValue.ToString()));
                            return;
                        }
                        else
                        {
                            DataTable dt = (DataTable)this.detailgridbs.DataSource;
                            DataRow[] errorDataRow = dt.Select($"EmployeeID = '{MyUtility.Convert.GetString(this.EmployeeData.Rows[0]["ID"])}' and NO <> '{MyUtility.Convert.GetString(dr["No"])}'");
                            if (errorDataRow.Length > 0)
                            {
                                MyUtility.Msg.WarningBox($"<{this.EmployeeData.Rows[0]["ID"]} {this.EmployeeData.Rows[0]["Name"]}> already been used in No.{MyUtility.Convert.GetString(errorDataRow[0]["No"])}!!");
                                dr["EmployeeName"] = string.Empty;
                                return;
                            }

                            dt.DefaultView.Sort = this.selectDataTable_DefaultView_Sort == "ASC" ? "No ASC" : string.Empty;
                            DataTable sortDataTable = dt.DefaultView.ToTable();

                            DataRow[] listDataRows = sortDataTable.Select($"No = '{MyUtility.Convert.GetString(dr["No"])}'");
                            if (listDataRows.Length > 0)
                            {
                                foreach (DataRow dataRow in listDataRows)
                                {
                                    int dataIndex = sortDataTable.Rows.IndexOf(dataRow);
                                    DataRow row = this.detailgrid.GetDataRow<DataRow>(dataIndex);
                                    row["EmployeeID"] = this.EmployeeData.Rows[0]["ID"];
                                    row["EmployeeName"] = this.EmployeeData.Rows[0]["Name"];
                                    row["EmployeeSkill"] = this.EmployeeData.Rows[0]["Skill"];
                                    row.EndEdit();
                                }
                            }

                            DataTable dt1 = (DataTable)this.listControlBindingSource1.DataSource;
                            dt1.DefaultView.Sort = this.selectDataTable_DefaultView_Sort == "ASC" ? "No ASC" : string.Empty;
                            DataTable sortDataTable1 = dt1.DefaultView.ToTable();
                            DataRow[] listDataRows1 = sortDataTable1.Select($"No = '{MyUtility.Convert.GetString(dr["No"])}'");
                            if (listDataRows1.Length > 0)
                            {
                                foreach (DataRow dataRow in listDataRows1)
                                {
                                    int dataIndex = sortDataTable1.Rows.IndexOf(dataRow);
                                    DataRow row = this.grid1.GetDataRow<DataRow>(dataIndex);
                                    row["EmployeeID"] = this.EmployeeData.Rows[0]["ID"];
                                    row["EmployeeName"] = this.EmployeeData.Rows[0]["Name"];
                                    row["EmployeeSkill"] = this.EmployeeData.Rows[0]["Skill"];
                                    row.EndEdit();
                                }
                            }
                        }
                    }
                }
            };

            operatorName.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (MyUtility.Check.Empty(this.txtFactory.Text))
                        {
                            MyUtility.Msg.WarningBox("Please fill in [Factory] first!");
                            return;
                        }

                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);

                            this.GetEmployee(null);

                            P03_Operator callNextForm = new P03_Operator(this.EmployeeData, MyUtility.Convert.GetString(this.CurrentMaintain["SewingLineID"]));
                            DialogResult result = callNextForm.ShowDialog(this);
                            if (result == DialogResult.Cancel)
                            {
                                return;
                            }

                            if (callNextForm.SelectOperator == null)
                            {
                                return;
                            }
                            DataTable dt = (DataTable)this.detailgridbs.DataSource;

                            DataRow[] errorDataRow = dt.Select($"EmployeeID = '{MyUtility.Convert.GetString(callNextForm.SelectOperator["ID"])}' and NO <> '{MyUtility.Convert.GetString(dr["No"])}'");
                            if (errorDataRow.Length > 0)
                            {
                                MyUtility.Msg.WarningBox($"<{callNextForm.SelectOperator["ID"]} {callNextForm.SelectOperator["Name"]}> already been used in No.{MyUtility.Convert.GetString(dr["No"])}!!");
                                return;
                            }

                            dt.DefaultView.Sort = this.selectDataTable_DefaultView_Sort == "ASC" ? "No ASC" : string.Empty;
                            DataTable sortDataTable = dt.DefaultView.ToTable();

                            DataRow[] listDataRows = sortDataTable.Select($"No = '{MyUtility.Convert.GetString(dr["No"])}'");
                            if (listDataRows.Length > 0)
                            {
                                foreach (DataRow dataRow in listDataRows)
                                {
                                    int dataIndex = sortDataTable.Rows.IndexOf(dataRow);
                                    DataRow row = this.detailgrid.GetDataRow<DataRow>(dataIndex);
                                    row["EmployeeID"] = callNextForm.SelectOperator["ID"];
                                    row["EmployeeName"] = callNextForm.SelectOperator["Name"];
                                    row["EmployeeSkill"] = callNextForm.SelectOperator["Skill"];
                                    row.EndEdit();
                                }
                            }

                            DataTable dt1 = (DataTable)this.listControlBindingSource1.DataSource;
                            dt1.DefaultView.Sort = this.selectDataTable_DefaultView_Sort == "ASC" ? "No ASC" : string.Empty;
                            DataTable sortDataTable1 = dt1.DefaultView.ToTable();
                            DataRow[] listDataRows1 = sortDataTable1.Select($"No = '{MyUtility.Convert.GetString(dr["No"])}'");
                            if (listDataRows1.Length > 0)
                            {
                                foreach (DataRow dataRow in listDataRows1)
                                {
                                    int dataIndex = sortDataTable1.Rows.IndexOf(dataRow);
                                    DataRow row = this.grid1.GetDataRow<DataRow>(dataIndex);
                                    row["EmployeeID"] = callNextForm.SelectOperator["ID"];
                                    row["EmployeeName"] = callNextForm.SelectOperator["Name"];
                                    row["EmployeeSkill"] = callNextForm.SelectOperator["Skill"];
                                    row.EndEdit();
                                }
                            }

                        }
                    }
                }
            };
            operatorName.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.ReviseEmployeeToEmpty(dr);
                        return;
                    }

                    if (!e.FormattedValue.ToString().Contains(","))
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Employee Name: {0} > not found!!!", e.FormattedValue.ToString()));
                        this.ReviseEmployeeToEmpty(dr);
                        return;
                    }

                    if (e.FormattedValue.ToString() != dr["EmployeeName"].ToString())
                    {
                        this.GetEmployee(null, name: e.FormattedValue.ToString());
                        if (this.EmployeeData.Rows.Count <= 0)
                        {
                            this.ReviseEmployeeToEmpty(dr);
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Employee Name: {0} > not found!!!", e.FormattedValue.ToString()));
                            return;
                        }
                        else
                        {
                            DataTable dt = (DataTable)this.detailgridbs.DataSource;
                            DataRow[] errorDataRow = dt.Select($"EmployeeID = '{MyUtility.Convert.GetString(this.EmployeeData.Rows[0]["ID"])}' and NO <> '{MyUtility.Convert.GetString(dr["No"])}'");
                            if (errorDataRow.Length > 0)
                            {
                                MyUtility.Msg.WarningBox($"<{this.EmployeeData.Rows[0]["ID"]} {this.EmployeeData.Rows[0]["Name"]}> already been used in No.{MyUtility.Convert.GetString(errorDataRow[0]["No"])}!!");
                                return;
                            }

                            dt.DefaultView.Sort = this.selectDataTable_DefaultView_Sort == "ASC" ? "No ASC" : string.Empty;
                            DataTable sortDataTable = dt.DefaultView.ToTable();

                            DataRow[] listDataRows = sortDataTable.Select($"No = '{MyUtility.Convert.GetString(dr["No"])}'");
                            if (listDataRows.Length > 0)
                            {
                                foreach (DataRow dataRow in listDataRows)
                                {
                                    int dataIndex = sortDataTable.Rows.IndexOf(dataRow);
                                    DataRow row = this.detailgrid.GetDataRow<DataRow>(dataIndex);
                                    row["EmployeeID"] = this.EmployeeData.Rows[0]["ID"];
                                    row["EmployeeName"] = this.EmployeeData.Rows[0]["Name"];
                                    row["EmployeeSkill"] = this.EmployeeData.Rows[0]["Skill"];
                                    row.EndEdit();
                                }
                            }

                            DataTable dt1 = (DataTable)this.listControlBindingSource1.DataSource;
                            dt1.DefaultView.Sort = this.selectDataTable_DefaultView_Sort == "ASC" ? "No ASC" : string.Empty;
                            DataTable sortDataTable1 = dt1.DefaultView.ToTable();
                            DataRow[] listDataRows1 = sortDataTable1.Select($"No = '{MyUtility.Convert.GetString(dr["No"])}'");
                            if (listDataRows1.Length > 0)
                            {
                                foreach (DataRow dataRow in listDataRows1)
                                {
                                    int dataIndex = sortDataTable1.Rows.IndexOf(dataRow);
                                    DataRow row = this.grid1.GetDataRow<DataRow>(dataIndex);
                                    row["EmployeeID"] = this.EmployeeData.Rows[0]["ID"];
                                    row["EmployeeName"] = this.EmployeeData.Rows[0]["Name"];
                                    row["EmployeeSkill"] = this.EmployeeData.Rows[0]["Skill"];
                                    row.EndEdit();
                                }
                            }
                        }
                    }
                }
            };
            #endregion

            #region PPA
            ppaText.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = @"
select PPAID=ID,PPA=Name 
from DropDownList 
where Type = 'PMS_IEPPA'
";

                    SelectItem item = new SelectItem(sqlcmd, "PPA ID,PPA", "10,10", this.CurrentDetailData["PPA"].ToString(), null, null, null)
                    {
                        Width = 666,
                    };
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    DataRow dr = item.GetSelecteds()[0];
                    this.CurrentDetailData["PPA"] = dr["PPAID"];
                    this.CurrentDetailData["PPAText"] = dr["PPA"];
                }
            };

            ppaText.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.CurrentDetailData["PPA"] = string.Empty;
                        this.CurrentDetailData["PPAText"] = string.Empty;
                        return;
                    }

                    string newSewingMachineAttachmentID = MyUtility.Convert.GetString(e.FormattedValue);

                    string sqlcmd = $@"
select PPAID=ID,PPA=Name 
from DropDownList 
where Type = 'PMS_IEPPA'
and Name = @PPA
";

                    DataTable dt;
                    List<SqlParameter> paras = new List<SqlParameter>()
                                        {
                                            new SqlParameter("@PPA", MyUtility.Convert.GetString(e.FormattedValue)),
                                        };
                    DualResult r = DBProxy.Current.Select(null, sqlcmd, paras, out dt);

                    if (!r)
                    {
                        e.Cancel = true;
                        this.ShowErr(r);
                        return;
                    }

                    if (dt.Rows == null || dt.Rows.Count == 0)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Data not found");
                    }
                    else
                    {
                        DataRow dr = dt.Rows[0];
                        this.CurrentDetailData["PPA"] = dr["PPAID"];
                        this.CurrentDetailData["PPAText"] = dr["PPA"];
                        if (!MyUtility.Check.Empty(this.CurrentDetailData["PPA"]))
                        {
                            this.CurrentDetailData["IsHide"] = false;
                        }
                    }
                }
            };
            #endregion
            hide.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.EditMode)
                {
                    dr["IsHide"] = dr["IsHide"];
                    return;
                }

                if (this.EditMode)
                {
                    if (!MyUtility.Check.Empty(dr["PPA"]))
                    {
                        dr["IsHide"] = 0;
                        return;
                    }

                    if (MyUtility.Convert.GetBool(e.FormattedValue))
                    {
                        string noo = dr["No"].ToString(); // 紀錄要被刪除的No
                        dr["No"] = string.Empty;
                        //dr["IsPPA"] = 0;
                        dr["IsHide"] = 1;
                        this.SumNoGSDCycleTime(dr["GroupKey"].ToString());
                        this.AssignNoGSDCycleTime(dr["GroupKey"].ToString());
                        if (noo != string.Empty)
                        {
                            this.ReclculateGridGSDCycleTime(noo); // 重算被刪除掉的No的TotalGSD & Total Cycle Time
                        }
                    }
                    else
                    {
                        dr["IsHide"] = 0;
                        this.SumNoGSDCycleTime(dr["GroupKey"].ToString());
                        this.AssignNoGSDCycleTime(dr["GroupKey"].ToString());
                    }

                    this.ComputeTaktTime();
                    this.Distable();
                    this.detailgrid.Focus();
                    this.detailgrid.CurrentCell = this.detailgrid[e.ColumnIndex, e.RowIndex];
                    this.detailgrid.BeginEdit(true);
                }
            };

            ppa.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                    if (MyUtility.Convert.GetBool(e.FormattedValue))
                    {
                        //dr["IsPPA"] = 1;
                        dr["IsHide"] = 0;
                        dr.EndEdit();
                    }
                    else
                    {
                        //dr["IsPPA"] = 0;
                        dr.EndEdit();
                    }

                    this.ComputeTaktTime();
                    this.Distable();
                    this.detailgrid.Focus();
                    this.detailgrid.CurrentCell = this.detailgrid[e.ColumnIndex, e.RowIndex];
                    this.detailgrid.BeginEdit(true);
                }
            };

            machineCount.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                    if (!MyUtility.Check.Empty(dr["IsMachineTypeID_MM"]) && MyUtility.Convert.GetBool(dr["IsMachineTypeID_MM"]) == true && MyUtility.Convert.GetBool(e.FormattedValue))
                    {
                        MyUtility.Msg.InfoBox("This operation can't be check.");
                        dr["MachineCount"] = 0;
                        dr.EndEdit();
                    }
                }
            };
            threadColor.MaxLength = 1;
            no.MaxLength = 4;
            notice.MaxLength = 600;

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("OriNo", header: "OriNo.", width: Widths.AnsiChars(4), iseditingreadonly: true)
            .Text("No", header: "No.", width: Widths.AnsiChars(4), settings: no)
            //.CheckBox("IsPPA", header: "PPA", width: Widths.AnsiChars(1), iseditable: true, trueValue: true, falseValue: false, settings: ppa)
            .Text("PPAText", header: "PPA", width: Widths.AnsiChars(5), settings: ppaText)
            .CheckBox("IsHide", header: "Hide", width: Widths.AnsiChars(1), iseditable: true, trueValue: true, falseValue: false, settings: hide)
            .CheckBox("MachineCount", header: "Machine\r\nCount", width: Widths.AnsiChars(1), iseditable: true, trueValue: true, falseValue: false, settings: machineCount)
            .CellMachineType("MachineTypeID", "ST/MC\r\ntype", this, width: Widths.AnsiChars(2))
            .Text("MasterPlusGroup", header: "Machine\r\nGroup", width: Widths.AnsiChars(1), settings: txtSubReason)
            .EditText("Description", header: "Operation", width: Widths.AnsiChars(13), iseditingreadonly: true,settings: operationID)
            .EditText("Annotation", header: "Annotation", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Numeric("GSD", header: "GSD\r\nTime", width: Widths.AnsiChars(3), decimal_places: 2, iseditingreadonly: true)
            .Numeric("Cycle", header: "Cycle\r\nTime", width: Widths.AnsiChars(3), integer_places: 4, decimal_places: 2, minimum: 0, settings: cycle)
            .CellAttachment("Attachment", "Attachment", this, width: Widths.AnsiChars(10))
            .CellPartID("SewingMachineAttachmentID", "Part ID", this, width: Widths.AnsiChars(10))
            .CellTemplate("Template", "Template", this, width: Widths.AnsiChars(10))
            .Text("ThreadColor", header: "ThreadColor", width: Widths.AnsiChars(1), settings: threadColor)
            .Text("Notice", header: "Notice", width: Widths.AnsiChars(14), settings: notice)
            .Numeric("Efficiency", header: "Eff(%)", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
            .Numeric("EstCycleTime", header: "Est.\r\nCycle Time", width: Widths.AnsiChars(6), iseditingreadonly: true, decimal_places: 2)
            ;

            this.detailgrid.Columns["OriNo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.detailgrid.Columns["No"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.detailgrid.Columns["GSD"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.detailgrid.Columns["Cycle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.detailgrid.Columns["MachineTypeID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.detailgrid.RowPrePaint += (s, e) =>
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                DataRow dr = ((DataRowView)this.detailgrid.Rows[e.RowIndex].DataBoundItem).Row;
                #region 變色規則，若該 Row 已經變色則跳過
                if (dr["New"].ToString().ToUpper() == "TRUE")
                {
                    if (this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.FromArgb(255, 186, 117))
                    {
                        this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 186, 117);
                    }
                }
                else
                {
                    if (this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor != backDefaultColor)
                    {
                        this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = backDefaultColor;
                    }
                }
                #endregion
            };

            // [No.] 特殊排序規則 [Hide]有打勾-> [No.][PPA][Hide]皆空白-> [No.]為P開頭-> [No.]為一般數字
            int rowIndex = 0;
            int columIndex = 0;
            this.detailgrid.CellClick += (s, e) =>
            {
                rowIndex = e.RowIndex;
                columIndex = e.ColumnIndex;
            };

            this.detailgrid.Sorted += (s, e) =>
            {
                this.Distable();

                if (columIndex == 1)
                {
                    DataTable dt = (DataTable)this.detailgridbs.DataSource;
                    dt.DefaultView.Sort = "sortNO, No";
                    this.detailgridbs.DataSource = dt;
                    this.selectDataTable_DefaultView_Sort = "ASC";

                    this.IsShowTable();
                }
            };

            Ict.Win.UI.DataGridViewNumericBoxColumn act;
            DataGridViewGeneratorTextColumnSettings reasonName = new DataGridViewGeneratorTextColumnSettings();
            reasonName.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode &&
                    e.Button == MouseButtons.Right &&
                    e.RowIndex != -1)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);

                    if (!this.ConfirmColor || this.ConfirmLists.Count == 0)
                    {
                        return;
                    }

                    if (!this.ConfirmLists.Select(x => x.No).Contains(dr["No"].ToString()))
                    {
                        return;
                    }

                    DataTable dt = this.GetLBRNotHitName();
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(dt, "ReasonName,Code,Type,TypeGroup,Ukey", "100,10,10,10,10", null, headercaptions: "ReasonName,Code,Type,TypeGroup,Ukey")
                    {
                        Width = 700,
                    };
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    IList<DataRow> selectedData = item.GetSelecteds();
                    dr["ReasonName"] = selectedData[0]["ReasonName"];
                    dr.EndEdit();
                }
            };

            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("No", header: "No.", width: Widths.AnsiChars(4), iseditingreadonly: true)
            .Numeric("TotalCycle", header: "Act.\r\nCycle\r\nTime", width: Widths.AnsiChars(3), integer_places: 5, decimal_places: 2, iseditingreadonly: true/*, settings: ac*/).Get(out act)
            .Numeric("TotalGSD", header: "Ttl\r\nGSD\r\nTime", width: Widths.AnsiChars(3), decimal_places: 2, iseditingreadonly: true)
            .Text("ReasonName", header: "LBR not\r\nhit target\r\nreason.", width: Widths.AnsiChars(10), iseditable: true, iseditingreadonly: true, settings: reasonName)
            .Numeric("EstOutputHr", header: "Est. Output/Hr", width: Widths.AnsiChars(6), iseditingreadonly: true,decimal_places: 0)
            .Text("EmployeeID", header: "Operator ID No.", width: Widths.AnsiChars(10), settings: operatorid)
            .Text("EmployeeName", header: "Operator Name", width: Widths.AnsiChars(20), settings: operatorName)
            .Text("EmployeeSkill", header: "Skill", width: Widths.AnsiChars(10), iseditingreadonly: true)
            ;
            this.grid1.Columns["No"].Frozen = true;
        }

        // 撈出Employee資料
        private void GetEmployee(string iD, string name = "")
        {
            string lastName = string.Empty;
            string firstName = string.Empty;
            if (!MyUtility.Check.Empty(name))
            {
                string[] nameParts = name.Split(',');
                lastName = nameParts[0];
                firstName = nameParts[1];
            }

            string sqlCmd;

            bool IsEmptySewingLine = MyUtility.Check.Empty(this.CurrentMaintain["SewingLineID"]);
            // sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@factoryid", this.CurrentMaintain["FactoryID"].ToString());
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@id",
            };
            if (iD != null)
            {
                sp2.Value = iD;
            }
            else
            {
                sp2.Value = DBNull.Value;
            }

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>
            {
                sp1,
                sp2,
                new SqlParameter("@SewingLine", this.CurrentMaintain["SewingLineID"] + this.comboSewingTeam1.Text),
                new SqlParameter("@LastName", lastName),
                new SqlParameter("@FirstName", firstName),
            };

                sqlCmd = $@"
                select 
                e.ID
                ,FirstName
                ,LastName
                ,Section
                ,Skill
                ,SewingLineID 
                , [Name] = iif(LastName+ ','+ FirstName <> ',' ,LastName+ ','+ FirstName,'')  
                ,e.FactoryID
                , eas.P03
                from Employee e WITH (NOLOCK) 
                left join EmployeeAllocationSetting eas on e.FactoryID = eas.FactoryID and e.Dept = eas.Dept and e.Position = eas.Position 
                where 
                (ResignationDate is null or ResignationDate > GETDATE()) 
                and e.Junk = 0 and eas.P03 = 1 "
                + (iD == null ? string.Empty : " and e.ID = @id ")
                + (MyUtility.Check.Empty(lastName) ? string.Empty : " and LastName = @LastName")
                + (MyUtility.Check.Empty(firstName) ? string.Empty : " and FirstName = @FirstName");

            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out this.EmployeeData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
            }
        }

        // 將Employee相關欄位值清空
        private void ReviseEmployeeToEmpty(DataRow dr)
        {
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            DataRow[] errorDataRow = dt.Select($"NO ='{MyUtility.Convert.GetString(dr["No"])}'");
            dt.DefaultView.Sort = this.selectDataTable_DefaultView_Sort == "ASC" ? "No ASC" : string.Empty;
            DataTable sortDataTable = dt.DefaultView.ToTable();

            DataRow[] listDataRows = sortDataTable.Select($"No = '{MyUtility.Convert.GetString(dr["No"])}'");
            if (listDataRows.Length > 0)
            {
                foreach (DataRow dataRow in listDataRows)
                {
                    int dataIndex = sortDataTable.Rows.IndexOf(dataRow);
                    DataRow row = this.detailgrid.GetDataRow<DataRow>(dataIndex);
                    row["EmployeeID"] = string.Empty;
                    row["EmployeeName"] = string.Empty;
                    row["EmployeeSkill"] = string.Empty;
                    row.EndEdit();
                }
            }

            DataTable dt1 = (DataTable)this.listControlBindingSource1.DataSource;

            dt1.DefaultView.Sort = this.selectDataTable_DefaultView_Sort == "ASC" ? "No ASC" : string.Empty;
            DataTable sortDataTable1 = dt1.DefaultView.ToTable();

            DataRow[] listDataRows1 = sortDataTable1.Select($"No = '{MyUtility.Convert.GetString(dr["No"])}'");
            if (listDataRows1.Length > 0)
            {
                foreach (DataRow dataRow in listDataRows1)
                {
                    int dataIndex = sortDataTable1.Rows.IndexOf(dataRow);
                    DataRow row = this.grid1.GetDataRow<DataRow>(dataIndex);
                    row["EmployeeID"] = string.Empty;
                    row["EmployeeName"] = string.Empty;
                    row["EmployeeSkill"] = string.Empty;
                    row.EndEdit();
                }
            }

        }

        private DataTable GetLBRNotHitName()
        {
            string sqlCmd = "select distinct [ReasonName] = Name,Code,Type,TypeGroup,Ukey from IEReasonLBRNotHit_Detail WITH (NOLOCK) where junk = 0";

            DualResult result = DBProxy.Current.Select(null, sqlCmd, null, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
            }

            return dt;
        }

        /// <summary>
        /// ClickNewAfter
        /// </summary>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.txtStyleComboType.BackColor = Color.White;
        }

        /// <summary>
        /// ClickCopyAfter
        /// </summary>
        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            this.CurrentMaintain["Version"] = DBNull.Value;
            this.CurrentMaintain["IEReasonID"] = string.Empty;
            this.CurrentMaintain["Status"] = "New";
            this.txtStyleComboType.BackColor = Color.White;
        }

        /// <summary>
        /// ClickEditBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record already confirmed, so can't modify this record!!");
                return false;
            }

            this.txtStyleComboType.BackColor = Color.White;
            return true;
        }

        /// <summary>
        /// ClickDeleteBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickDeleteBefore()
        {
            if (!PublicPrg.Prgs.GetAuthority(this.CurrentMaintain["AddName"].ToString()))
            {
                MyUtility.Msg.WarningBox("This record is not created by yourself, so you can't delete this record!!");
                return false;
            }

            if (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record already confirmed, so can't delete this record!!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// ClickSaveBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            var oriNo_EmptyCnt = this.DetailDatas.AsEnumerable().Where(x => MyUtility.Check.Empty(x["OriNo"]));

            if (oriNo_EmptyCnt.Any() && this.CurrentMaintain["Phase"].ToString() != "Final")
            {
                string strMsg = "The <Insert> button can only be used in the \"Final\" Phase!" + Environment.NewLine + "Please switch Phase to \"Final\", or remove rows where OriNo is empty.";
                MyUtility.Msg.WarningBox(strMsg);
                return false;
            }

            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["StyleID"]))
            {
                MyUtility.Msg.WarningBox("Style can't empty");
                this.txtStyleID.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ComboType"]))
            {
                MyUtility.Msg.WarningBox("Combo type can't empty");
                this.txtStyleComboType.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
            {
                MyUtility.Msg.WarningBox("Factory can't empty");
                this.txtFactory.Focus();
                return false;
            }

            if (MyUtility.Convert.GetDecimal(MyUtility.Convert.GetString(this.CurrentMaintain["Workhour"])) == 0)
            {
                MyUtility.Msg.WarningBox("<No .of Hours> cannot be 0!!");
                this.numNoOfHours.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Phase"]))
            {
                MyUtility.Msg.WarningBox("<Phase> cannot be empty! Please fill in <Phase> next to <Version>.");
                this.comboPhase.Focus();
                return false;
            }
            else if (this.CurrentMaintain["Phase"].ToString().EqualString("Final"))
            {
                if (MyUtility.Check.Empty(this.CurrentMaintain["SewingLineID"]))
                {
                    MyUtility.Msg.WarningBox("Please enter <Sewing Line> since phase is Final.");
                    this.txtSewingLine.Focus();
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["Team"]))
                {
                    MyUtility.Msg.WarningBox("Please enter <Team> since phase is Final.");
                    this.comboSewingTeam1.Focus();
                    return false;
                }
            }
            #endregion

            string chkfactory = $@"select 1 from factory where FTYGroup = '{this.txtFactory.Text}'";
            if (!MyUtility.Check.Seek(chkfactory))
            {
                MyUtility.Msg.WarningBox($"Factory:{this.txtFactory.Text} not found");
                return false;
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can not empty!");
                return false;
            }

            var queryIsGroupHeader = this.DetailDatas.Where(x => x.Field<bool?>("IsShow") == true && x.Field<bool?>("IsHide") == false && x.Field<bool?>("IsGroupHeader") == true);
            if (queryIsGroupHeader.Any())
            {
                string msg = "It must be selected if the operation is [Group Header]." + Environment.NewLine;
                foreach (DataRow row in queryIsGroupHeader)
                {
                    msg += "[OriNo]: " + row["OriNo"].ToString() + Environment.NewLine;
                }

                MyUtility.Msg.WarningBox(msg);
                return false;
            }

            //var queryHideAndPPA = this.DetailDatas.Where(x => x.Field<bool?>("IsShow") == true && x.Field<bool?>("IsHide") == true && x.Field<bool?>("IsPPA") == true);
            //if (queryHideAndPPA.Any())
            //{
            //    MyUtility.Msg.WarningBox("<PPA> and <Hide> cannot be selected at the same time.");
            //    return false;
            //}

            var queryIsHide = this.DetailDatas.Where(x => x.Field<bool?>("IsShow") == true && x.Field<bool?>("IsHide") == true && !x.Field<string>("No").Empty());
            if (queryIsHide.Any())
            {
                string msg = "These operations cannot be [Hide] and have [No.] in the same time." + Environment.NewLine;
                foreach (DataRow row in queryIsHide)
                {
                    msg += "[Operation]: " + row["Description"].ToString() + Environment.NewLine;
                }

                MyUtility.Msg.WarningBox(msg);
                return false;
            }

            //var queryIsPPA = this.DetailDatas
            //    .Where(x => x.Field<bool?>("IsShow") == true && x.Field<bool?>("IsHide") == false
            //                   && ((x.Field<bool?>("IsPPA") == true && (x.Field<string>("No").Empty() || !x.Field<string>("No").Substring(0, 1).Equals("P")))
            //                    || (x.Field<bool?>("IsPPA") == false && !x.Field<string>("No").Empty() && x.Field<string>("No").Substring(0, 1).Equals("P"))));
            //if (queryIsPPA.Any())
            //{
            //    MyUtility.Msg.WarningBox("The [No.] first word must be P if the [PPA] is checked.");
            //    return false;
            //}

            var queryIsPPACentralized = this.DetailDatas
                .Where(x => x.Field<bool?>("IsShow") == true && x.Field<bool?>("IsHide") == false
                               && ((x.Field<string>("PPA") == "C" && (x.Field<string>("No").Empty() || !x.Field<string>("No").Substring(0, 1).Equals("P")))
                                || (x.Field<string>("PPA") != "C" && !x.Field<string>("No").Empty() && x.Field<string>("No").Substring(0, 1).Equals("P"))));
            if (queryIsPPACentralized.Any())
            {
                MyUtility.Msg.WarningBox("The [No.] first word must be P if the [PPA] is [Centralized].");
                return false;
            }

            this.ComputeTaktTime();

            // Vision為空的話就要填值 or ID是空值(新增)也要重新計算Version
            if (MyUtility.Check.Empty(this.CurrentMaintain["Version"]) || this.CurrentMaintain["Version"].ToString() == "0" || MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                string newVersion = MyUtility.GetValue.Lookup($"select isnull(max(Version),0)+1 as Newversion from LineMapping WITH (NOLOCK) where StyleUKey =  {this.CurrentMaintain["StyleUkey"]} and FactoryID = '{this.CurrentMaintain["FactoryID"]}'");
                if (MyUtility.Check.Empty(newVersion))
                {
                    MyUtility.Msg.WarningBox("Get Version fail!!");
                    return false;
                }

                this.CurrentMaintain["Version"] = newVersion;
            }

            int version = MyUtility.Convert.GetInt(this.CurrentMaintain["Version"]) - 1;
            string chkVersionStatus = $@"select Status from LineMapping where StyleUKey = '{this.CurrentMaintain["StyleUKey"]}' and FactoryID = '{this.CurrentMaintain["FactoryID"]}' and version = '{version}'";
            if (MyUtility.Check.Seek(chkVersionStatus, out DataRow drStatus))
            {
                if (MyUtility.Convert.GetString(drStatus["Status"]) != "Confirmed")
                {
                    MyUtility.Msg.WarningBox($"Please check that the status of version {version} needs to be Confirmed");
                    return false;
                }
            }

            var depulicateData = this.DetailDatas.GroupBy(o => new { OriNo = o.Field<string>("OriNo"), No = o.Field<string>("No") })
                .Select(o => new
                {
                    OriNo = o.Key.OriNo,
                    No = o.Key.No,
                    Count = o.Count(),
                }).Where(o => o.Count > 1);

            if (depulicateData.Any())
            {
                string msg = "<OriNo.>+<No.> can not be exactly the same.";
                foreach (var item in depulicateData)
                {
                    msg += $@"{Environment.NewLine}{item.OriNo}-{item.No}";
                }

                MyUtility.Msg.WarningBox(msg);
                return false;
            }

            this.txtStyleComboType.BackColor = this.txtStyleID.BackColor;

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            if (this.grid1.DataSource != null)
            {
                DataTable detail = (DataTable)this.listControlBindingSource1.DataSource;
                string masterID = this.CurrentMaintain["ID"].ToString();
                string updCmd = string.Empty;
                foreach (DataRow item in detail.AsEnumerable().Where(x => !x.Field<string>("NO").Empty()))
                {
                    updCmd += $@"
UPDATE ld
    SET ld.IEReasonLBRNotHit_DetailUkey = (select top 1 Ukey from IEReasonLBRNotHit_Detail where Name = '{item["ReasonName"]}' and Junk = 0)
from LineMapping_Detail ld
WHERE No = '{item["No"]}'
and ID = {masterID}";
                }

                DualResult reusult = DBProxy.Current.Execute(null, updCmd);
                if (!reusult)
                {
                    this.ShowErr(reusult);
                }
            }

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();

            if (this.detailgridbs.DataSource != null)
            {
                DataTable detail = (DataTable)this.detailgridbs.DataSource;

                string updCmd = string.Empty;
                foreach (DataRow item in detail.Rows)
                {
                    updCmd += $@"
UPDATE LineMapping_Detail
SET MasterPlusGroup = '{item["MasterPlusGroup"]}'
    ,IsHide = '{item["IsHide"]}'
WHERE Ukey={item["Ukey"]}

";
                }

                DualResult reusult = DBProxy.Current.Execute(null, updCmd);
                if (!reusult)
                {
                    this.ShowErr(reusult);
                }
            }

            this.Distable();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridAppendClick()
        {
            DataRow newrow, tmp;

            // 先紀錄目前Grid所指道的那筆資料
            tmp = this.detailgrid.GetDataRow(this.detailgrid.GetSelectedRowIndex());
            if (tmp.Empty())
            {
                return;
            }

            this.SumNoGSDCycleTime(this.CurrentDetailData["GroupKey"].ToString());
            base.OnDetailGridAppendClick();
            newrow = this.detailgrid.GetDataRow(this.detailgrid.GetSelectedRowIndex());
            newrow.ItemArray = tmp.ItemArray; // 將剛剛紀錄的資料複製到新增的那筆record
            this.CurrentDetailData["New"] = true;
            this.CurrentDetailData["No"] = string.Empty;
            this.AssignNoGSDCycleTime(this.CurrentDetailData["GroupKey"].ToString());
            this.ComputeTaktTime();
        }

        /// <summary>
        /// OnDetailGridInsertClick
        /// </summary>
        protected override void OnDetailGridInsert(int index = 0)
        {
            base.OnDetailGridInsert();
            this.CurrentDetailData["OriNo"] = string.Empty;
            this.CurrentDetailData["New"] = true;
            this.CurrentDetailData["No"] = string.Empty;
            this.CurrentDetailData["IsShow"] = true;
        }

        /// <summary>
        /// OnDetailGridDelete
        /// </summary>
        protected override void OnDetailGridDelete()
        {
            if (this.CurrentMaintain == null || this.CurrentDetailData == null)
            {
                return;
            }

            if (this.detailgrid.Rows.Count != 0)
            {
                if (this.CurrentDetailData["New"].ToString().ToUpper() == "FALSE")
                {
                    if (MyUtility.Convert.GetDecimal(this.CurrentDetailData["GSD"]) != 0)
                    {
                        MyUtility.Msg.WarningBox("This record is set up by system, can't delete!!");
                        return;
                    }
                }

                string no = this.CurrentDetailData["No"].ToString(); // 紀錄要被刪除的No
                string groupkey = this.CurrentDetailData["GroupKey"].ToString();
                this.SumNoGSDCycleTime(groupkey);
                base.OnDetailGridDelete();
                this.AssignNoGSDCycleTime(groupkey);
                if (no != string.Empty)
                {
                    this.ReclculateGridGSDCycleTime(no); // 傳算被刪除掉的No的TotalGSD & Total Cycle Time
                }

                this.ComputeTaktTime();
            }
        }

        /// <summary>
        /// ClickPrint
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickPrint()
        {
            P03_Print callNextForm = new P03_Print(this.CurrentMaintain, MyUtility.Convert.GetDecimal(this.numCPUPC.Value));
            callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }

        /// <summary>
        /// ClickUndo
        /// </summary>
        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.txtStyleComboType.BackColor = this.txtStyleID.BackColor;
            this.OnDetailEntered();
        }

        // 加總傳入的GroupKey的GSD & Cycle Time
        private void SumNoGSDCycleTime(string groupKey)
        {
            this.totalGSD = ((DataTable)this.detailgridbs.DataSource).Compute("sum(GSD)", string.Format("GroupKey = {0}", groupKey));
            this.totalCycleTime = ((DataTable)this.detailgridbs.DataSource).Compute("sum(Cycle)", string.Format("GroupKey = {0}", groupKey));
        }

        // 填輸入的GroupKey的GSD & Cycle Time
        private void AssignNoGSDCycleTime(string groupKey)
        {
            object countRec = ((DataTable)this.detailgridbs.DataSource).Compute("count(GroupKey)", string.Format("GroupKey = {0}", groupKey));
            decimal avgGSD = MyUtility.Check.Empty(Convert.ToDecimal(countRec)) ? MyUtility.Convert.GetDecimal(this.totalGSD) : Math.Round(MyUtility.Convert.GetDecimal(this.totalGSD) / MyUtility.Convert.GetDecimal(countRec), 2);
            decimal avgCycleTime = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(countRec)) ? MyUtility.Convert.GetDecimal(this.totalCycleTime) : Math.Round(MyUtility.Convert.GetDecimal(this.totalCycleTime) / MyUtility.Convert.GetDecimal(countRec), 2);
            DataRow[] findRow = ((DataTable)this.detailgridbs.DataSource).Select(string.Format("GroupKey = {0}", groupKey));
            int i = 0;
            decimal sumGSD = 0, sumCycleTime = 0;

            // 平均分配Cycle Time與GSD，若有餘數就放置最後一筆
            foreach (DataRow dr in findRow)
            {
                i++;
                if (i >= MyUtility.Convert.GetInt(countRec))
                {
                    dr["GSD"] = MyUtility.Convert.GetDecimal(this.totalGSD) - sumGSD;
                    dr["Cycle"] = MyUtility.Convert.GetDecimal(this.totalCycleTime) - sumCycleTime;
                }
                else
                {
                    dr["GSD"] = avgGSD;
                    dr["Cycle"] = avgCycleTime;
                    sumGSD = sumGSD + MyUtility.Convert.GetDecimal(dr["GSD"]);
                    sumCycleTime = sumCycleTime + MyUtility.Convert.GetDecimal(dr["Cycle"]);
                }
            }

            foreach (DataRow dr in findRow)
            {
                this.ReclculateGridGSDCycleTime(dr["No"].ToString());
            }
        }

        // 計算DailyDemand,NetTime,TaktTime,LLER,Ideal Target / Hr. (100%), Ideal Daily demand/shift, Ideal Takt Time欄位值
        private void CalculateValue(int type)
        {

            if (!MyUtility.Check.Empty(this.CurrentMaintain["TotalGSD"]) &&
                !MyUtility.Check.Empty(this.CurrentMaintain["CurrentOperators"]) &&
                !MyUtility.Check.Empty(this.CurrentMaintain["Workhour"]))
            {

                decimal decTotalGSD = MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSD"]);
                decimal decCurrentOperators = MyUtility.Convert.GetDecimal(this.CurrentMaintain["CurrentOperators"]);
                decimal decWorkhour = MyUtility.Convert.GetDecimal(this.CurrentMaintain["Workhour"]);

                decimal decTargetHr = 3600 * decCurrentOperators / decTotalGSD;
                decimal decDailyDemand_Shift = decTargetHr * decWorkhour;
                this.CurrentMaintain["TaktTime"] = Math.Round(3600 * decWorkhour / decDailyDemand_Shift, 2);
            }
            else
            {
                this.CurrentMaintain["TaktTime"] = 0;
            }

            if (type == 1)
            {
                this.CurrentMaintain["DailyDemand"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["Workhour"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["StandardOutput"]), 0);
                this.CurrentMaintain["NetTime"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["Workhour"]) * 3600, 0);
                // this.CurrentMaintain["TaktTime"] = MyUtility.Check.Empty(this.CurrentMaintain["DailyDemand"]) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["NetTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["DailyDemand"]), 0);
            }

            this.numLLER.Value = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(this.CurrentMaintain["TaktTime"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["CurrentOperators"])) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCycle"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["TaktTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["CurrentOperators"]) * 100, 2);
            this.numTargetHrIdeal.Value = MyUtility.Check.Empty(this.CurrentMaintain["TotalGSD"]) ? 0 : MyUtility.Math.Round(3600 * MyUtility.Convert.GetDecimal(this.CurrentMaintain["IdealOperators"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSD"]), 0);
            this.numDailydemandshiftIdeal.Value = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["Workhour"]) * MyUtility.Convert.GetDecimal(this.numTargetHrIdeal.Value), 0);
            // this.numTaktTimeIdeal.Value = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(this.numDailydemandshiftIdeal.Value)) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["NetTime"]) / MyUtility.Convert.GetDecimal(this.numDailydemandshiftIdeal.Value), 0);
        }

        // Compute Takt Time
        private void ComputeTaktTime()
        {
            object sumGSD = ((DataTable)this.detailgridbs.DataSource).Compute("sum(GSD)", "(IsHide = 0 or  IsHide is null) and No <> ''");
            object sumCycle = ((DataTable)this.detailgridbs.DataSource).Compute("sum(Cycle)", "(IsHide = 0 or  IsHide is null) and No <> ''");
            object maxHighGSD = ((DataTable)this.detailgridbs.DataSource).Compute("max(TotalGSD)", "(IsHide = 0 or  IsHide is null) and No <> ''");
            object maxHighCycle = ((DataTable)this.detailgridbs.DataSource).Compute("max(TotalCycle)", "(IsHide = 0 or  IsHide is null) and No <> ''");

            // object countopts = ((DataTable)detailgridbs.DataSource).Compute("count(No)", "");
            int countopts = 0;

            var temptable = this.DetailDatas.CopyToDataTable();
            temptable.DefaultView.Sort = "No";
            string no = string.Empty;
            foreach (DataRow dr in temptable.DefaultView.ToTable().Select("(IsHide = 0 or  IsHide is null)"))
            {
                if (!MyUtility.Check.Empty(dr["No"]) && no != dr["No"].ToString())
                {
                    countopts += 1;
                    no = dr["No"].ToString();
                }
            }

            this.CurrentMaintain["TotalGSD"] = sumGSD;
            this.CurrentMaintain["TotalCycle"] = sumCycle;
            this.CurrentMaintain["HighestGSD"] = maxHighGSD;
            this.CurrentMaintain["HighestCycle"] = maxHighCycle;
            this.CurrentMaintain["CurrentOperators"] = countopts;
            this.CurrentMaintain["StandardOutput"] = MyUtility.Check.Empty(this.CurrentMaintain["TotalCycle"]) ? 0 : MyUtility.Math.Round(3600 * MyUtility.Convert.GetDecimal(this.CurrentMaintain["CurrentOperators"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCycle"]), 0);
            this.CurrentMaintain["DailyDemand"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["Workhour"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["StandardOutput"]), 0);
            this.CurrentMaintain["TaktTime"] = MyUtility.Check.Empty(this.CurrentMaintain["DailyDemand"]) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["NetTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["DailyDemand"]), 0);
        }

        // 計算Total % time diff,Highest % time diff,Effieiency(%),Effieiency(%),PPH,LBR欄位值
        private void SaveCalculateValue()
        {
            this.numTotalTimeDiff.Value = MyUtility.Check.Empty(this.CurrentMaintain["TotalGSD"]) ? 0 : MyUtility.Math.Round(((MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSD"]) - MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCycle"])) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSD"])) * 100, 2);
            this.numHighestTimeDiff.Value = MyUtility.Check.Empty(this.CurrentMaintain["HighestGSD"]) ? 0 : MyUtility.Math.Round(((MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestGSD"]) - MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestCycle"])) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestGSD"])) * 100, 2);
            this.numEOLR.Value = MyUtility.Check.Empty(this.CurrentMaintain["HighestCycle"]) ? 0 : MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestCycle"]), 2);
            this.numEffieiency.Value = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestCycle"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["CurrentOperators"])) ? 0 : MyUtility.Math.Round((MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSD"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestCycle"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["CurrentOperators"])) * 100, 2);
            this.numPPH.Value = MyUtility.Check.Empty(this.CurrentMaintain["CurrentOperators"]) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.numEOLR.Value) * MyUtility.Convert.GetDecimal(this.numCPUPC.Value) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["CurrentOperators"]), 4);
            this.numLBR.Value = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestCycle"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["CurrentOperators"])) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCycle"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestCycle"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["CurrentOperators"]) * 100, 2);
        }

        // 重新計算Grid的Cycle Time
        private void ReclculateGridGSDCycleTime(string no)
        {
            object gSD = ((DataTable)this.detailgridbs.DataSource).Compute("Sum(GSD)", string.Format("(IsHide = 0 or  IsHide is null)  and No = '{0}'", no));
            object cycle = ((DataTable)this.detailgridbs.DataSource).Compute("Sum(Cycle)", string.Format("(IsHide = 0 or  IsHide is null) and No = '{0}'", no));

            DataRow[] findRow = ((DataTable)this.detailgridbs.DataSource).Select(string.Format("(IsHide = 0 or  IsHide is null) and No = '{0}'", no));
            if (findRow.Length > 0)
            {
                foreach (DataRow dr in findRow)
                {
                    dr["TotalGSD"] = gSD;
                    dr["TotalCycle"] = cycle;
                    dr["ActCycle"] = cycle;
                    dr.EndEdit();
                }
            }
        }

        // No. of Hours
        private void NumNoOfHours_Validated(object sender, EventArgs e)
        {
            this.CalculateValue(1);
        }

        // Ideal No. of Oprts
        private void NumOprtsIdeal_Validated(object sender, EventArgs e)
        {
            this.CalculateValue(0);
        }

        // Style#
        private void TxtStyleID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select ID,SeasonID,BrandID,Description,CPU,Ukey from Style WITH (NOLOCK) where Junk = 0 order by ID,SeasonID";

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "15,8,10,40,5,6", this.txtStyleID.Text, "Style#,Season,Brand,Description,CPU,Key", columndecimals: "0,0,0,0,3,0")
            {
                Width = 838,
            };
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            IList<DataRow> styleData = item.GetSelecteds();
            this.CurrentMaintain["StyleID"] = styleData[0]["ID"];
            this.CurrentMaintain["SeasonID"] = styleData[0]["SeasonID"];
            this.CurrentMaintain["BrandID"] = styleData[0]["BrandID"];
            this.CurrentMaintain["StyleUKey"] = styleData[0]["Ukey"];
            this.displayDesc.Value = styleData[0]["Description"];
            this.numCPUPC.Value = MyUtility.Convert.GetDecimal(styleData[0]["CPU"]);

            DataTable comboType;
            DualResult result = DBProxy.Current.Select(null, string.Format("select Location from Style_Location WITH (NOLOCK) where StyleUkey = {0}", this.CurrentMaintain["StyleUKey"].ToString()), out comboType);
            if (result)
            {
                if (comboType.Rows.Count > 1)
                {
                    item = new Win.Tools.SelectItem(comboType, "Location", "2", string.Empty, "Combo Type");
                    returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentMaintain["ComboType"] = item.GetSelectedString();
                }
                else
                {
                    if (comboType.Rows.Count != 0)
                    {
                        this.CurrentMaintain["ComboType"] = comboType.Rows[0]["Location"];
                    }
                    else
                    {
                        this.CurrentMaintain["ComboType"] = string.Empty;
                    }
                }
            }
        }

        // Combo Type
        private void TxtStyleComboType_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(string.Format("select Location from Style_Location WITH (NOLOCK) where StyleUkey = {0}", this.CurrentMaintain["StyleUKey"].ToString()), "2", string.Empty, "Combo Type");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.CurrentMaintain["ComboType"] = item.GetSelectedString();
        }

        // 撈出ChgOverTarget資料
        private decimal FindTarget(string type)
        {
            return MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup($@"
select top 1 c.Target
from factory f
left join ChgOverTarget c on c.MDivisionID= f.MDivisionID and c.EffectiveDate < GETDATE() and c. Type ='{type}'
where f.id = '{Sci.Env.User.Factory}'
order by EffectiveDate desc
"));
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            if (!PublicPrg.Prgs.GetAuthority(this.CurrentMaintain["AddName"].ToString()))
            {
                MyUtility.Msg.WarningBox("This record is not created by yourself, so can't confirm!");
                return;
            }

            #region 檢查表身不可為空
            DataRow[] findrow = ((DataTable)this.detailgridbs.DataSource).Select("(IsHide = 0 or  IsHide is null) and (No = '' or No is null) and (IsShow = 1)");
            if (findrow.Length > 0)
            {
                MyUtility.Msg.WarningBox("< No. > can't empty!!");
                return;
            }
            #endregion

            this.ComputeTaktTime();

            decimal lBRTarget = this.FindTarget("LBR");
            decimal lLERTarget = this.FindTarget("EFF.");
            bool checkLBR = !MyUtility.Check.Empty(lBRTarget) && Convert.ToDecimal(this.numLBR.Value) < lBRTarget;
            bool checkEFF = !MyUtility.Check.Empty(lLERTarget) && Convert.ToDecimal(this.numEffieiency.Value) < lLERTarget;
            string notHitReasonID = string.Empty;
            string lbrReasion = string.Empty;
            string sqlCmd;
            StringBuilder msg = new StringBuilder();
            Win.Tools.SelectItem item;
            DialogResult returnResult;

            if (checkLBR)
            {
                if (this.CurrentMaintain["Version"].ToString().EqualString("1"))
                {
                    msg.Append("LBR is lower than target.\r\n");
                    MyUtility.Msg.WarningBox(msg.ToString() + "Please select not hit target reason.");
                    sqlCmd = "select Ukey, Name from IEReasonLBRNotHit_1st WITH (NOLOCK) where junk = 0";
                    item = new Win.Tools.SelectItem(sqlCmd, "5,30", string.Empty);
                    returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    IList<DataRow> selectedData = item.GetSelecteds();
                    lbrReasion = selectedData[0]["Ukey"].ToString();
                }
                else
                {
                    // Version != 1 檢查表身
                    this.ConfirmChangeGridColor(true);
                    if (this.ConfirmColor && this.ConfirmLists.Count > 0)
                    {
                        MyUtility.Msg.WarningBox("No of " + string.Join(",", this.ConfirmLists.Select(x => x.No)) + " need to input not hit target reason ");
                        return;
                    }
                }
            }

            msg = new StringBuilder();
            if (checkEFF)
            {
                msg.Append("Efficiency is lower than target.\r\n");
                MyUtility.Msg.WarningBox(msg.ToString() + "Please select not hit target reason.");
                sqlCmd = "select ID, Description from IEReason WITH (NOLOCK) where Type = 'LM' and Junk = 0";
                item = new Win.Tools.SelectItem(sqlCmd, "5,30", string.Empty);
                returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                notHitReasonID = item.GetSelectedString();
            }

            DualResult result;
            string updateCmd = "update LineMapping set Status = 'Confirmed', ";
            if (!notHitReasonID.Empty())
            {
                updateCmd += string.Format(" IEReasonID = '{0}', ", notHitReasonID);
            }

            if (!lbrReasion.Empty())
            {
                updateCmd += string.Format(" IEReasonLBRNotHit_1stUkey = '{0}', ", lbrReasion);
            }

            updateCmd += string.Format(" EditName = '{0}', EditDate = GETDATE() where ID = {1}", Env.User.UserID, this.CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm fail!\r\n" + result.ToString());
                return;
            }
        }

        private void ConfirmChangeGridColor(bool chkEmpty)
        {
            decimal lBRTarget = this.FindTarget("LBR");
            decimal decLBR = 100 - lBRTarget;
            DataTable detail = (DataTable)this.listControlBindingSource1.DataSource;
            decimal? avgCycle = detail.AsEnumerable().Average(x => x.Field<decimal?>("TotalCycle"));
            this.ConfirmLists = detail.AsEnumerable()
                .Select(x => new GridList()
                {
                    No = x.Field<string>("No"),
                    TotalCycle = ((avgCycle - x.Field<decimal?>("TotalCycle")) / avgCycle) * 100,
                    ReasonName = x.Field<string>("ReasonName"),
                })
                .Where(x => (x.TotalCycle > decLBR || x.TotalCycle < decLBR * -1) && (!chkEmpty || x.ReasonName.Empty()))
                .ToList();

            if (this.ConfirmLists.Count > 0)
            {
                this.ConfirmColor = true;

                for (int i = 0; i <= detail.Rows.Count - 1; i++)
                {
                    DataGridViewRow dr = this.grid1.Rows[i];
                    dr.DefaultCellStyle.BackColor = this.ConfirmLists.Select(x => x.No).Contains(dr.Cells["No"].Value) ? Color.FromArgb(255, 255, 128) : dr.DefaultCellStyle.BackColor;
                }
            }
            else
            {
                this.ConfirmColor = false;
            }
        }

        // Not hit target reason
        private void BtnNotHitTargetReason_Click(object sender, EventArgs e)
        {
            // 不使用MyUtility.Msg.InfoBox的原因為MyUtility.Msg.InfoBox都有MessageBoxIcon
            if (!MyUtility.Check.Empty(this.CurrentMaintain["IEReasonID"]))
            {
                MessageBox.Show(MyUtility.GetValue.Lookup(string.Format("select Description from IEReason WITH (NOLOCK) where Type = 'LM' and ID = '{0}'", this.CurrentMaintain["IEReasonID"].ToString())).PadRight(60), caption: "Not hit target reason");
            }
            else
            {
                MessageBox.Show(MyUtility.GetValue.Lookup(string.Format("select Name from IEReasonLBRnotHit_1st WITH (NOLOCK) where Ukey = '{0}'", this.CurrentMaintain["IEReasonLBRnotHit_1stUkey"].ToString())).PadRight(60), caption: "Not hit target reason");
            }
        }

        // Copy from other line mapping
        private void BtnCopyFromOtherLineMapping_Click(object sender, EventArgs e)
        {
            P03_CopyFromOtherStyle callNextForm = new P03_CopyFromOtherStyle();
            DialogResult result = callNextForm.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                DataTable copyLineMapDetail;
                string sqlCmd = string.Format(
                    @"
select ID = null
	   , OriNo = ld.OriNo
	   , No = ld.No
	   , ld.Annotation
	   , ld.GSD
	   , ld.TotalGSD
	   , ld.Cycle
	   , ld.TotalCycle
	   , ld.MachineTypeID
       , ld.Attachment
       , ld.Template 
	   , ld.OperationID
	   , ld.MoldID
	   , ld.GroupKey
	   , ld.New
	   , ld.EmployeeID
	   , [Description] = IIF(o.DescEN = '' OR o.DescEN IS NULL, ld.OperationID, o.DescEN)
	   , EmployeeName = e.Name
	   , EmployeeSkill = e.Skill
	   , Efficiency = iif(ld.Cycle = 0,0,ROUND(ld.GSD/ld.Cycle,2)*100)
       , ld.isppa
       , ld.Threadcolor
       , ld.ActCycle
       , ld.MasterPlusGroup
		,[IsHide] = iif(SUBSTRING(ld.OperationID, 1, 2) = '--', 1, iif(show.IsDesignatedArea = 1, 1, 0))
		,[IsGroupHeader] = iif(SUBSTRING(ld.OperationID, 1, 2) = '--', 1, 0)
        ,[IsShow] = cast(iif( ld.OperationID like '--%' , 1, isnull(show.IsShowinIEP03, 1)) as bit)
	    , [sortNO] = case when SUBSTRING(ld.OperationID, 1, 2) = '--' then 1
		when show.IsDesignatedArea = 1 then 1
		when ld.No = '' and iif(SUBSTRING(ld.OperationID, 1, 2) = '--', 1, iif(show.IsDesignatedArea = 1, 1, 0)) = 0 
                            and ld.IsPPA = 0 
                          then 2
                          when left(ld.No, 1) = 'P' then 3
                          else 4 end
       ,[EstCycleTime] = ld.EstCycleTime
       ,[EstTotalCycleTime] = ld.EstTotalCycleTime
       ,[EstOutputHr] = ld.EstOutputHr
       ,[EstLBR] = ld.EstLBR
from LineMapping_Detail ld WITH (NOLOCK)
left join Employee e WITH (NOLOCK) on ld.EmployeeID = e.ID
left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID
outer apply (
	select IsShowinIEP03 = IIF(isnull(md.IsNotShownInP03, 0) = 0, 1, 0)
		, IsDesignatedArea = ISNULL(md.IsNonSewingLine,0)
	from Operation o2 WITH (NOLOCK)
	inner join MachineType m WITH (NOLOCK) on o2.MachineTypeID = m.ID
    inner join MachineType_Detail md WITH (NOLOCK) on md.ID = m.ID and md.FactoryID = '{1}'	
	where o.ID = o2.ID and m.junk = 0
)show
where ld.ID = {0} 
order by case when ld.No = '' then 1
			when left(ld.No, 1) = 'P' then 2
			else 3
			end, 
        ld.GroupKey",
                    callNextForm.P03CopyLineMapping["ID"].ToString(),
                    callNextForm.P03CopyLineMapping["FactoryID"].ToString());
                DualResult selectResult = DBProxy.Current.Select(null, sqlCmd, out copyLineMapDetail);
                if (!selectResult)
                {
                    MyUtility.Msg.ErrorBox("Query copy linemapping detail fail!!\r\n" + selectResult.ToString());
                    return;
                }

                // 刪除現有表身資料
                foreach (DataRow dr in this.DetailDatas)
                {
                    dr.Delete();
                }

                // 將要複製的資料寫入表身Grid
                foreach (DataRow dr in copyLineMapDetail.Rows)
                {
                    if (callNextForm.P03CopyLineMapping["FactoryID"].ToString() != this.CurrentMaintain["FactoryID"].ToString())
                    {
                        dr["EmployeeID"] = string.Empty;
                        dr["EmployeeName"] = string.Empty;
                        dr["EmployeeSkill"] = string.Empty;
                        dr.EndEdit();
                    }

                    dr.AcceptChanges();
                    dr.SetAdded();
                    ((DataTable)this.detailgridbs.DataSource).ImportRow(dr);
                }

                // 填入表頭資料
                this.CurrentMaintain["IdealOperators"] = callNextForm.P03CopyLineMapping["IdealOperators"].ToString();
                this.CurrentMaintain["CurrentOperators"] = callNextForm.P03CopyLineMapping["CurrentOperators"].ToString();
                this.CurrentMaintain["StandardOutput"] = callNextForm.P03CopyLineMapping["StandardOutput"].ToString();
                this.CurrentMaintain["DailyDemand"] = callNextForm.P03CopyLineMapping["DailyDemand"].ToString();
                this.CurrentMaintain["Workhour"] = callNextForm.P03CopyLineMapping["Workhour"].ToString();
                this.CurrentMaintain["NetTime"] = callNextForm.P03CopyLineMapping["NetTime"].ToString();
                this.CurrentMaintain["TaktTime"] = callNextForm.P03CopyLineMapping["TaktTime"].ToString();
                this.CurrentMaintain["TotalGSD"] = callNextForm.P03CopyLineMapping["TotalGSD"].ToString();
                this.CurrentMaintain["OriTotalGSD"] = callNextForm.P03CopyLineMapping["TotalGSD"].ToString();
                this.CurrentMaintain["TotalCycle"] = callNextForm.P03CopyLineMapping["TotalCycle"].ToString();
                this.CurrentMaintain["HighestGSD"] = callNextForm.P03CopyLineMapping["HighestGSD"].ToString();
                this.CurrentMaintain["HighestCycle"] = callNextForm.P03CopyLineMapping["HighestCycle"].ToString();
                this.CurrentMaintain["TimeStudyPhase"] = callNextForm.P03CopyLineMapping["TimeStudyPhase"].ToString();
                this.CurrentMaintain["TimeStudyVersion"] = callNextForm.P03CopyLineMapping["TimeStudyVersion"].ToString();
                this.CalculateValue(0);
                this.ComputeTaktTime();
            }

            this.Distable();
        }

        private DataTable GetDataFromP01()
        {
            DataTable dt;

            string cmd = $@"select OperationID=ID ,MoldID  from Operation where Junk=0";

            DBProxy.Current.Select(null, cmd, out dt);

            return dt;
        }

        // Copy from GSD
        private void BtnCopyFromGSD_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
            {
                this.ShowInfo("<Factory> cannot be empty.");
                return;
            }

            // 刪除現有表身資料
            foreach (DataRow dr in this.DetailDatas)
            {
                dr.Delete();
            }

            DataRow timeStudy;
            DataTable timeStudy_Detail;
            string sqlCmd = string.Format(
                @"
select t.* 
from TimeStudy t WITH (NOLOCK) 
	 , Style s WITH (NOLOCK) 
where t.StyleID = s.ID 
	  and t.BrandID = s.BrandID 
	  and t.SeasonID = s.SeasonID 
	  and s.Ukey = {0}
	  and t.ComboType = '{1}'",
                this.CurrentMaintain["StyleUkey"].ToString(),
                this.CurrentMaintain["ComboType"].ToString());
            if (!MyUtility.Check.Seek(sqlCmd, out timeStudy))
            {
                MyUtility.Msg.WarningBox("Fty GSD data not found!!");
                return;
            }

            if (!timeStudy["Status"].ToString().ToLower().EqualString("confirmed"))
            {
                MyUtility.Msg.WarningBox("P01. Factory GSD need to confirm first before import to P03");
                return;
            }

            string ietmsUKEY = MyUtility.GetValue.Lookup($@" select i.Ukey from TimeStudy t WITH (NOLOCK) inner join IETMS i WITH (NOLOCK) on i.id = t.IETMSID and i.Version = t.IETMSVersion where t.id = '{timeStudy["ID"]}' ");
            sqlCmd = string.Format(
                @"
select *
	, [sortNO] = case when ld.IsHide = 1 then 1 
                      when ld.No = '' and ld.IsHide = 0 and ld.IsPPA = 0 then 2
                      when left(ld.No, 1) = 'P' then 3
                      else 4 end
from (
select distinct
	ID = null
	,No = ''
	,OriNo = '0'
	,Annotation = '**Cutting'
	,GSD = round(ProTMS, 4)
	,TotalGSD = round(ProTMS, 4)
	,Cycle = round(ProTMS, 4)
	,TotalCycle = round(ProTMS, 4)
	,MachineTypeID = op.MachineTypeID
    ,Attachment = null
    ,Template = null
	,OperationID = op.ID
	,MoldID = null
    ,SewingMachineAttachmentID=''
	,GroupKey = 0
	,New = 0
	,EmployeeID = ''
	,Description = '**Cutting'
	,EmployeeName = ''
	,EmployeeSkill = ''
	,Efficiency = 100
	,IsPPA = 0
    ,[MasterPlusGroup]=''
	,MachineCount = CAST( 0 as bit)
	,[IsHide] = cast(iif(ISNULL(md.IsNonSewingLine,0) = 1, 1, 0) as bit)
	,[IsGroupHeader] = 0
    ,[IsShow] = cast(IIF(isnull(md.IsNotShownInP03, 0) = 0, 1, 0) as bit)
	,PPA = ''
    ,PPAText =''
    ,[EstCycleTime] = 0
    ,[EstTotalCycleTime] = 0
    ,[EstOutputHr] = 0
    ,[EstLBR] = 0
from [IETMS_Summary] i, Operation op
left join MachineType_Detail md WITH (NOLOCK) on md.ID = op.MachineTypeID and md.FactoryID = '{2}'
where i.location = '' and i.[IETMSUkey] = '{0}' and i.ArtworkTypeID = 'Cutting' and op.ID='PROCIPF00001'

union all
select ID = null
	   , No = ''
	   , OriNo = td.Seq
	   , td.Annotation
	   , GSD = td.SMV
	   , TotalGSD = td.SMV
	   , Cycle = td.SMV
	   , TotalCycle = td.SMV
	   , td.MachineTypeID
	   , [Attachment] = STUFF((
					select concat(',' ,s.Data)
					from SplitString(td.Mold, ',') s
					where not exists (select 1 from Mold m WITH (NOLOCK) where s.Data = m.ID and (m.Junk = 1 or m.IsTemplate = 1)) 
					for xml path ('')) 
				,1,1,'')
	    , [Template] = STUFF((
					select concat(',' ,s.Data)
					from SplitString(td.Template, ',') s
					where not exists (select 1 from Mold m WITH (NOLOCK) where s.Data = m.ID and (m.Junk = 1 or m.IsAttachment = 1)) 
					for xml path ('')) 
				,1,1,'')
	   , td.OperationID
	   , MoldID = td.Mold
       , td.SewingMachineAttachmentID
	   , GroupKey = 0
	   , New = 0
	   , EmployeeID = ''
	   , Description = IIF(td.MachineTypeID IS NULL OR td.MachineTypeID = '' ,td.OperationID ,o.DescEN )
	   , EmployeeName = ''
	   , EmployeeSkill = ''
	   , Efficiency = 100
       , IsPPA = iif(CHARINDEX('--', td.OperationID) > 0, 0, iif(td.SMV > 0, 0, 1))
       ,o.MasterPlusGroup
	   ,MachineCount = CAST(  IIF(o.MasterPlusGroup <> '' and (o.MasterPlusGroup is not null and td.MachineTypeID not like 'MM%'),1,0) as bit)
	   ,[IsHide] = cast(
			   case when SUBSTRING(td.OperationID, 1, 2) = '--' then 1
			        when show.IsDesignatedArea = 1 then 1
			        when isnull(td.IsNonSewingLine,0) = 1 then 1
			   else 0 
			   end			
		as bit)
	   ,[IsGroupHeader] = cast(iif(SUBSTRING(td.OperationID, 1, 2) = '--', 1, 0) as bit)
       ,[IsShow] = cast(iif( td.OperationID like '--%' , 1, isnull(show.IsShowinIEP03, 1)) as bit)
	   ,td.PPA
       ,PPAText = ISNULL(d.Name,'')
       ,[EstCycleTime] = 0
       ,[EstTotalCycleTime] = 0
       ,[EstOutputHr] = 0
       ,[EstLBR] = 0
from TimeStudy_Detail td WITH (NOLOCK)
left join Operation o WITH (NOLOCK) on td.OperationID = o.ID
left join DropDownList d (NOLOCK) on d.ID=td.PPA AND d.Type = 'PMS_IEPPA'
outer apply (
	select IsShowinIEP03 = IIF(isnull(md.IsNotShownInP03, 0) = 0, 1, 0)
		, IsDesignatedArea = ISNULL(md.IsNonSewingLine,0)
	from MachineType m WITH (NOLOCK)
    inner join MachineType_Detail md WITH (NOLOCK) on md.ID = m.ID and md.FactoryID = '{2}'	
	where o.MachineTypeID = m.ID and m.junk = 0
)show
where td.ID = '{1}'

union all
select distinct
	ID = null
	,No = ''
	,OriNo = '9970'
	,Annotation = '**Inspection'
	,GSD = round(ProTMS, 4)
	,TotalGSD = round(ProTMS, 4)
	,Cycle = round(ProTMS, 4)
	,TotalCycle = round(ProTMS, 4)
	,MachineTypeID = op.MachineTypeID
    ,Attachment = null
    ,Template = null
	,OperationID = op.ID -- PROCIPF00002
	,MoldID = null
    ,SewingMachineAttachmentID=''
	,GroupKey = 0
	,New = 0
	,EmployeeID = ''
	,Description = '**Inspection'
	,EmployeeName = ''
	,EmployeeSkill = ''
	,Efficiency = 100
	,IsPPA = 0
    ,[MasterPlusGroup]=''
	,MachineCount = CAST( 0 as bit)
	,[IsHide] = cast(iif(ISNULL(md.IsNonSewingLine,0) = 1, 1, 0) as bit)
	,[IsGroupHeader] = 0
    ,[IsShow] = cast(IIF(isnull(md.IsNotShownInP03, 0) = 0, 1, 0) as bit)
	,PPA = ''
    ,PPAText =''
    ,[EstCycleTime] = 0
    ,[EstTotalCycleTime] = 0
    ,[EstOutputHr] = 0
    ,[EstLBR] = 0
from [IETMS_Summary] i, Operation op
left join MachineType_Detail md WITH (NOLOCK) on md.ID = op.MachineTypeID and md.FactoryID = '{2}'
where i.location = '' and i.[IETMSUkey] = '{0}' and i.ArtworkTypeID = 'Inspection' and op.ID='PROCIPF00002'

union all
select distinct
	ID = null
	,No = ''
	,OriNo = '9980'
	,Annotation = '**Pressing'
	,GSD = round(ProTMS, 4)
	,TotalGSD = round(ProTMS, 4)
	,Cycle = round(ProTMS, 4)
	,TotalCycle = round(ProTMS, 4)
	,MachineTypeID = op.MachineTypeID
    ,Attachment = null
    ,Template = null
	,OperationID = op.ID -- PROCIPF00004
	,MoldID = null
    ,SewingMachineAttachmentID=''
	,GroupKey = 0
	,New = 0
	,EmployeeID = ''
	,Description = '**Pressing'
	,EmployeeName = ''
	,EmployeeSkill = ''
	,Efficiency = 100
	,IsPPA = 0
    ,[MasterPlusGroup]=''
	,MachineCount = CAST( 0 as bit)
	,[IsHide] = cast(iif(ISNULL(md.IsNonSewingLine,0) = 1, 1, 0) as bit)
	,[IsGroupHeader] = 0
    ,[IsShow] = cast(IIF(isnull(md.IsNotShownInP03, 0) = 0, 1, 0) as bit)
	,PPA = ''
    ,PPAText =''
    ,[EstCycleTime] = 0
    ,[EstTotalCycleTime] = 0
    ,[EstOutputHr] = 0
    ,[EstLBR] = 0
from [IETMS_Summary] i, Operation op
left join MachineType_Detail md WITH (NOLOCK) on md.ID = op.MachineTypeID and md.FactoryID = '{2}'
where i.location = '' and i.[IETMSUkey] = '{0}' and i.ArtworkTypeID = 'Pressing' and op.ID='PROCIPF00004'

union all
select distinct
	ID = null
	,No = ''
	,OriNo = '9990'
	,Annotation =  '**Packing'
	,GSD = round(ProTMS, 4)
	,TotalGSD = round(ProTMS, 4)
	,Cycle = round(ProTMS, 4)
	,TotalCycle = round(ProTMS, 4)
	,MachineTypeID = op.MachineTypeID
    ,Attachment = null
    ,Template = null
	,OperationID = op.ID--'PROCIPF00003'
	,MoldID = null
    ,SewingMachineAttachmentID=''
	,GroupKey = 0
	,New = 0
	,EmployeeID = ''
	,Description = '**Packing'
	,EmployeeName = ''
	,EmployeeSkill = ''
	,Efficiency = 100
	,IsPPA = 0
    ,[MasterPlusGroup]=''
	,MachineCount = CAST( 0 as bit)
	,[IsHide] = cast(iif(ISNULL(md.IsNonSewingLine,0) = 1, 1, 0) as bit)
	,[IsGroupHeader] = 0
    ,[IsShow] = cast(IIF(isnull(md.IsNotShownInP03, 0) = 0, 1, 0) as bit)
	,PPA = ''
    ,PPAText =''
    ,[EstCycleTime] = 0
    ,[EstTotalCycleTime] = 0
    ,[EstOutputHr] = 0
    ,[EstLBR] = 0
from [IETMS_Summary] i, Operation op
left join MachineType_Detail md WITH (NOLOCK) on md.ID = op.MachineTypeID and md.FactoryID = '{2}'
where i.location = '' and i.[IETMSUkey] = '{0}' and i.ArtworkTypeID = 'Packing' and op.ID='PROCIPF00003'
)ld",
                ietmsUKEY,
                timeStudy["ID"],
                this.CurrentMaintain["FactoryID"]);

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out timeStudy_Detail);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Fty GSD detail fail!!");
                return;
            }

            // 將要複製的資料寫入表身Grid
            int i = 0;
            foreach (DataRow dr in timeStudy_Detail.Rows)
            {
                dr["GroupKey"] = ++i;
                dr.AcceptChanges();
                dr.SetAdded();
                ((DataTable)this.detailgridbs.DataSource).ImportRow(dr);
            }

            object sumSMV = timeStudy_Detail.Compute("sum(GSD)", "(IsHide = 0 or  IsHide is null) and No <> ''");
            object maxSMV = timeStudy_Detail.Compute("max(GSD)", "(IsHide = 0 or  IsHide is null) and No <> ''");

            // 填入表頭資料
            this.CurrentMaintain["IdealOperators"] = timeStudy["NumberSewer"].ToString();
            this.CurrentMaintain["CurrentOperators"] = i;
            this.CurrentMaintain["StandardOutput"] = MyUtility.Convert.GetDecimal(sumSMV) == 0 ? 0 : MyUtility.Math.Round(3600 * i / MyUtility.Convert.GetDecimal(sumSMV));
            this.CurrentMaintain["DailyDemand"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["StandardOutput"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["Workhour"]), 0);
            this.CurrentMaintain["TaktTime"] = MyUtility.Convert.GetDecimal(this.CurrentMaintain["DailyDemand"]) == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["NetTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["DailyDemand"]), 0);
            this.CurrentMaintain["TotalGSD"] = sumSMV;
            this.CurrentMaintain["TotalCycle"] = sumSMV;
            this.CurrentMaintain["HighestGSD"] = maxSMV;
            this.CurrentMaintain["HighestCycle"] = maxSMV;
            this.CurrentMaintain["TimeStudyPhase"] = timeStudy["phase"];
            this.CurrentMaintain["TimeStudyVersion"] = timeStudy["version"];
            this.CalculateValue(0);
            this.ComputeTaktTime();
            this.Distable();
        }

        private void Distable()
        {
            if (this.listControlBindingSource1.DataSource != null)
            {
                this.listControlBindingSource1.DataSource = null;
            }

            if (this.grid1.DataSource != null)
            {
                this.grid1.DataSource = null;
            }

            this.IsShowTable();

            DataRow[] drs = ((DataTable)this.detailgridbs.DataSource).Select("No <> '' and IsShow = 1");
            if (drs.Length == 0)
            {
                return;
            }

            List<GridList> gridLists = drs
            .GroupBy(x => new
            {
                No = x.Field<string>("No"),
            })
            .Select(g => new GridList()
            {
                No = g.Key.No,
                ActCycle = g.Max(x => x.Field<decimal?>("ActCycle")), // 假設ActCycle相同項取最大值
                TotalGSD = g.Max(x => x.Field<decimal?>("TotalGSD")), // 假設TotalGSD相同項取最大值
                TotalCycle = g.Max(x => x.Field<decimal?>("TotalCycle")), // 假設TotalCycle相同項取最大值
                ReasonName = g.First().Field<string>("ReasonName"),
                SortA = g.Key.No.Substring(0, 1),
                SortB = g.Key.No.Substring(1, g.Key.No.Length - 1),
                EmployeeID = g.First().Field<string>("EmployeeID"),
                EmployeeName = g.First().Field<string>("EmployeeName"),
                EmployeeSkill = g.First().Field<string>("EmployeeSkill"),
                EstTotalCycleTime = g.Max(x => x.Field<double?>("EstTotalCycleTime")),
                EstOutputHr = g.Max(x => x.Field<double?>("EstOutputHr")),
            })
            .OrderByDescending(x => x.SortA)
            .ThenBy(x => x.SortB)
            .ToList();

            this.listControlBindingSource1.DataSource = gridLists.ToDataTable<GridList>();
            this.grid1.DataSource = this.listControlBindingSource1;
            this.ConfirmChangeGridColor(false);
        }

        private void IsShowTable()
        {
            for (int i = 0; i < this.detailgrid.Rows.Count; i++)
            {
                DataRow row = this.detailgrid.GetDataRow(i);
                if (!MyUtility.Convert.GetBool(row["IsShow"]))
                {
                    CurrencyManager currencyManager = (CurrencyManager)this.BindingContext[this.detailgrid.DataSource];
                    currencyManager.SuspendBinding();
                    this.detailgrid.Rows[i].Visible = true;
                    currencyManager.ResumeBinding();
                }
                else
                {
                    CurrencyManager currencyManager = (CurrencyManager)this.BindingContext[this.detailgrid.DataSource];
                    currencyManager.SuspendBinding();
                    this.detailgrid.Rows[i].Visible = true;
                    currencyManager.ResumeBinding();
                }
            }
        }

        private void Txtsewingline_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (MyUtility.Check.Empty(this.txtFactory.Text))
                {
                    MyUtility.Msg.WarningBox("Please fill in [Factory] first!");
                }
            }
        }

        private void TxtSewingLine_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string ftyWhere = string.Empty;
            if (!this.txtFactory.Empty())
            {
                ftyWhere = $@"Where s.FactoryID in (select ID from Factory where FTYGroup = '{this.txtFactory.Text}') and s.Junk = 0";
            }

            string sql = $@"Select ID,FactoryID,Description From Production.dbo.SewingLine s WITH (NOLOCK)  {ftyWhere}";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "2,6,16", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtSewingLine.Text = item.GetSelectedString();
        }

        private void BtnPrintDetail_Click(object sender, EventArgs e)
        {
            string excelName = "IE_P03";

            DataTable dtSheet1 = (DataTable)this.detailgridbs.DataSource;

            var selectShett1 = dtSheet1.AsEnumerable()
                            .Where(row => row.Field<bool>("IsShow") == true)
                            .Select(row => new
                            {
                                OriNo = row.Field<string>("OriNo"),
                                No = row.Field<string>("No"),
                                PPA = row.Field<string>("PPAText"),
                                Hide = row.Field<bool>("IsHide") ? "Y" : "N",
                                MachineCount = row.Field<bool>("MachineCount") ? "Y" : "N",
                                MachineTypeID = row.Field<string>("MachineTypeID"),
                                MasterPlusGroup = row.Field<string>("MasterPlusGroup"),
                                Operation = row.Field<string>("Description"),
                                Annotation = row.Field<string>("Annotation"),
                                GSD = row.Field<decimal>("GSD"),
                                Cycle = row.Field<decimal>("Cycle"),
                                Attachment = row.Field<string>("Attachment"),
                                SewingMachineAttachmentID = row.Field<string>("SewingMachineAttachmentID"),
                                Template = row.Field<string>("Template"),
                                ThreadColor = row.Field<string>("ThreadColor"),
                                Notice = row.Field<string>("Notice"),
                                Efficiency = row.Field<decimal>("Efficiency"),
                                EstCycleTime = row.Field<decimal>("EstCycleTime"),
                            }).ToList();

            DataTable dtSheet2 = (DataTable)this.listControlBindingSource1.DataSource;

            var selectShett2 = dtSheet2.AsEnumerable()
                               .Select(
                                row => new
                                {
                                    No = row.Field<string>("No"),
                                    TotalCycle = row.Field<decimal>("TotalCycle"),
                                    TotalGSD = row.Field<decimal>("TotalGSD"),
                                    ReasonName = row.Field<string>("ReasonName"),
                                    EstOutputHr = row.Field<double>("EstOutputHr"),
                                    EmployeeID = row.Field<string>("EmployeeID"),
                                    EmployeeName = row.Field<string>("EmployeeName"),
                                    EmployeeSkill = row.Field<string>("EmployeeSkill"),
                                    EstTotalCycleTime = row.Field<double>("EstTotalCycleTime"),
                                }).ToList();

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{excelName}.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(selectShett1.ToDataTable(), string.Empty, showExcel: false, xltfile: $"{excelName}.xltx", headerRow: 1, excelApp: objApp, wSheet: objApp.Sheets[1]);
            MyUtility.Excel.CopyToXls(selectShett2.ToDataTable(), string.Empty, showExcel: false, xltfile: $"{excelName}.xltx", headerRow: 1, excelApp: objApp, wSheet: objApp.Sheets[2]);

            objApp.Sheets[1].Columns[1].ColumnWidth = 6;
            objApp.Sheets[1].Columns[8].ColumnWidth = 20;
            objApp.Sheets[1].Columns[9].ColumnWidth = 25;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("IE_P03");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);          // 釋放objApp
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
        }

        /// <inheritdoc/>
        private void TxtFactory_Validated(object sender, EventArgs e)
        {
            if (this.CurrentMaintain.RowState == DataRowState.Added)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID", DataRowVersion.Original]))
            {
                return;
            }

            if (MyUtility.Convert.GetString(this.CurrentMaintain["FactoryID", DataRowVersion.Original]) != MyUtility.Convert.GetString(this.CurrentMaintain["FactoryID"]))
            {
                this.txtSewingLine.Text = string.Empty;
                this.comboSewingTeam1.SelectedIndex = 0;
                this.comboSewingTeam1.Text = string.Empty;
                this.comboSewingTeam1.SelectedValue = string.Empty;
            }
        }
    }

    /// <inheritdoc/>
    public class GridList
    {
        /// <inheritdoc/>
        public string No { get; set; }

        /// <inheritdoc/>
        public decimal? ActCycle { get; set; }

        /// <inheritdoc/>
        public decimal? TotalGSD { get; set; }

        /// <inheritdoc/>
        public decimal? TotalCycle { get; set; }

        /// <inheritdoc/>
        public string ReasonName { get; set; }

        /// <inheritdoc/>
        public string SortA { get; set; }

        /// <inheritdoc/>
        public string SortB { get; set; }

        /// <inheritdoc/>
        public string EmployeeID { get; set; }

        /// <inheritdoc/>
        public string EmployeeName { get; set; }

        /// <inheritdoc/>
        public string EmployeeSkill { get; set; }

        /// <inheritdoc/>
        public double? EstTotalCycleTime { get; set; }

        /// <inheritdoc/>
        public double? EstOutputHr { get; set; }
    }
}
