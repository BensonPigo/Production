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
            .Text("ST_MC_Type", header: "ST/MC Type", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Motion", header: "Motion", width: Widths.AnsiChars(35), iseditingreadonly: true)
            .Text("Effi_90_day", header: "90D Effi %", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Effi_180_day", header: "180D Effi %", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Effi_270_day", header: "270D Effi %", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Effi_360_day", header: "360D Effi %", width: Widths.AnsiChars(15), iseditingreadonly: true)
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

            string sqlcmd = $@"
            SELECT
            [ST_MC_Type]
            ,[Motion]
            ,[Group_Header]
            ,[Part]
            ,[Attachment]
            ,[Effi_3_year] =FORMAT(AVG(CAST([Effi_3_year] AS DECIMAL(10, 2))), '0.00')
            From
            (
                SELECT 
                [ST_MC_Type] =lmd.MachineTypeID
                ,[Motion] = Operation_P03.val
                ,[Group_Header] = tsd.[location] 
                ,[Part] = lmd.SewingMachineAttachmentID
                ,[Attachment] = lmd.Attachment
                ,Effi_3_year = Effi_3_year.VAL
                from Employee e
                left JOIN LineMapping_Detail lmd WITH(NOLOCK) on lmd.EmployeeID = e.ID　
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
	                SELECT VAL = FORMAT(CAST(iif(oplmd.Cycle = 0,0,ROUND(oplmd.GSD/ oplmd.Cycle,2)*100) AS DECIMAL(10, 2)), '0.00')
	                FROM LineMapping oplm 
	                inner join LineMapping_Detail oplmd on oplm.ID = oplmd.ID
	                WHERE OPLMD.EmployeeID = E.ID
	                AND ((oplm.EditDate >= DATEADD(YEAR, -3, GETDATE()) and oplm.EditDate <= GETDATE()) or (oplm.AddDate >= DATEADD(YEAR, -3, GETDATE()) and oplm.AddDate <= GETDATE()))
                )Effi_3_year
	            WHERE 
	            e.FactoryID = '{this.CurrentMaintain["FactoryID"]}' and e.ID = '{this.CurrentMaintain["ID"]}' AND
				((lm.EditDate >= DATEADD(YEAR, -3, GETDATE()) and lm.EditDate <= GETDATE()) or (lm.AddDate >= DATEADD(YEAR, -3, GETDATE()) and lm.AddDate <= GETDATE()))
            )a
            GROUP BY [ST_MC_Type],[Motion], [Group_Header], [Part], [Attachment]

            UNION ALL

            SELECT
            [ST_MC_Type]
            ,[Motion]
            ,[Group_Header]
            ,[Part]
            ,[Attachment]
            ,[Effi_3_year] =FORMAT(AVG(CAST([Effi_3_year] AS DECIMAL(10, 2))), '0.00')
            From
            (
	            SELECT 
	            [ST_MC_Type] = lmbd.MachineTypeID
	            ,[Motion] =  Operation_P06.val
	            ,[Group_Header] = lmbd.[location]
	            ,[Part] = lmbd.SewingMachineAttachmentID
	            ,[Attachment] =  lmbd.Attachment
	            ,Effi_3_year = Effi_3_year.VAL
	            from Employee e
	            INNER JOIN LineMappingBalancing_Detail lmbd on lmbd.EmployeeID = e.ID
	            INNER JOIN LineMappingBalancing lmb on lmb.id = lmbd.ID
	            OUTER APPLY
	            (
	            select val = stuff((select distinct concat(',',Name)
		            from OperationRef a
		            inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
		            where a.CodeType = '00007' and a.id = lmbd.OperationID  for xml path('') ),1,1,'')
	            )Operation_P06
	            OUTER APPLY 
	            (
		            SELECT VAL = FORMAT(CAST(iif(oplmbd.Cycle = 0,0,ROUND(oplmbd.GSD/ oplmbd.Cycle,2)*100) AS DECIMAL(10, 2)), '0.00')
		            FROM LineMappingBalancing oplmb 
		            inner join LineMappingBalancing_Detail oplmbd on oplmb.ID = oplmbd.ID
		            WHERE oplmbd.EmployeeID = E.ID
		            AND ((oplmb.EditDate >= DATEADD(YEAR, -3, GETDATE()) and oplmb.EditDate <= GETDATE()) or (oplmb.AddDate >= DATEADD(YEAR, -3, GETDATE()) and oplmb.AddDate <= GETDATE()))
	            )Effi_3_year
	            where 
	            e.FactoryID = '{this.CurrentMaintain["FactoryID"]}' and e.ID = '{this.CurrentMaintain["ID"]}' AND
	            ((lmb.EditDate >= DATEADD(YEAR, -3, GETDATE()) and lmb.EditDate <= GETDATE()) or (lmb.AddDate >= DATEADD(YEAR, -3, GETDATE()) and lmb.AddDate <= GETDATE()))
            )b
            GROUP BY [ST_MC_Type],[Motion], [Group_Header], [Part], [Attachment]";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.dtOperatorDetail);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return;
            }

            this.btnOperationHistory.ForeColor = this.dtOperatorDetail.Rows.Count > 0 ? Color.Blue : Color.Black;

            string sqlcmdDetail = $@"
            SELECT
            [ST_MC_Type]
            ,[Motion]
            ,[Effi_90_day] =FORMAT(AVG(CAST([Effi_90_day] AS DECIMAL(10, 2))), '0.00')
            ,[Effi_180_day] =FORMAT(AVG(CAST([Effi_180_day] AS DECIMAL(10, 2))), '0.00')
            ,[Effi_270_day] =FORMAT(AVG(CAST([Effi_270_day] AS DECIMAL(10, 2))), '0.00')
            ,[Effi_360_day] =FORMAT(AVG(CAST([Effi_360_day] AS DECIMAL(10, 2))), '0.00')
            From
            (
                SELECT 
                [ST_MC_Type] =lmd.MachineTypeID
                ,[Motion] = Operation_P03.val
                ,Effi_90_day = Effi_90_day.VAL
                ,Effi_180_day = Effi_180_day.VAL
                ,Effi_270_day = Effi_270_day.VAL
                ,Effi_360_day = Effi_360_day.VAL
                from Employee e
                left JOIN LineMapping_Detail lmd WITH(NOLOCK) on lmd.EmployeeID = e.ID　
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
	                SELECT VAL = FORMAT(CAST(iif(oplmd.Cycle = 0,0,ROUND(oplmd.GSD/ oplmd.Cycle,2)*100) AS DECIMAL(10, 2)), '0.00')
	                FROM LineMapping oplm 
	                inner join LineMapping_Detail oplmd on oplm.ID = oplmd.ID
	                WHERE OPLMD.EmployeeID = E.ID
	                AND ((oplm.EditDate >= DATEADD(DAY, -90, GETDATE()) and oplm.EditDate <= GETDATE()) or (oplm.AddDate >= DATEADD(DAY, -90, GETDATE()) and oplm.AddDate <= GETDATE()))
                )Effi_90_day
                OUTER APPLY 
                (
	                SELECT VAL = FORMAT(CAST(iif(oplmd.Cycle = 0,0,ROUND(oplmd.GSD/ oplmd.Cycle,2)*100) AS DECIMAL(10, 2)), '0.00')
	                FROM LineMapping oplm 
	                inner join LineMapping_Detail oplmd on oplm.ID = oplmd.ID
	                WHERE OPLMD.EmployeeID = E.ID
	                AND ((oplm.EditDate >= DATEADD(DAY, -180, GETDATE()) and oplm.EditDate <= GETDATE()) or (oplm.AddDate >= DATEADD(DAY, -180, GETDATE()) and oplm.AddDate <= GETDATE()))
                )Effi_180_day
                OUTER APPLY 
                (
	                SELECT VAL = FORMAT(CAST(iif(oplmd.Cycle = 0,0,ROUND(oplmd.GSD/ oplmd.Cycle,2)*100) AS DECIMAL(10, 2)), '0.00')
	                FROM LineMapping oplm 
	                inner join LineMapping_Detail oplmd on oplm.ID = oplmd.ID
	                WHERE OPLMD.EmployeeID = E.ID
	                AND ((oplm.EditDate >= DATEADD(DAY, -270, GETDATE()) and oplm.EditDate <= GETDATE()) or (oplm.AddDate >= DATEADD(DAY, -270, GETDATE()) and oplm.AddDate <= GETDATE()))
                )Effi_270_day
                OUTER APPLY 
                (
	                SELECT VAL = FORMAT(CAST(iif(oplmd.Cycle = 0,0,ROUND(oplmd.GSD/ oplmd.Cycle,2)*100) AS DECIMAL(10, 2)), '0.00')
	                FROM LineMapping oplm 
	                inner join LineMapping_Detail oplmd on oplm.ID = oplmd.ID
	                WHERE OPLMD.EmployeeID = E.ID
	                AND ((oplm.EditDate >= DATEADD(DAY, -360, GETDATE()) and oplm.EditDate <= GETDATE()) or (oplm.AddDate >= DATEADD(DAY, -360, GETDATE()) and oplm.AddDate <= GETDATE()))
                )Effi_360_day
	            WHERE 
	            e.FactoryID = '{this.CurrentMaintain["FactoryID"]}' and e.ID = '{this.CurrentMaintain["ID"]}' AND
				((lm.EditDate >= DATEADD(YEAR, -3, GETDATE()) and lm.EditDate <= GETDATE()) or (lm.AddDate >= DATEADD(YEAR, -3, GETDATE()) and lm.AddDate <= GETDATE()))
            )a
            GROUP BY [ST_MC_Type],[Motion]

            UNION ALL

            SELECT
            [ST_MC_Type]
            ,[Motion]
            ,[Effi_90_day] =FORMAT(AVG(CAST([Effi_90_day] AS DECIMAL(10, 2))), '0.00')
            ,[Effi_180_day] =FORMAT(AVG(CAST([Effi_180_day] AS DECIMAL(10, 2))), '0.00')
            ,[Effi_270_day] =FORMAT(AVG(CAST([Effi_270_day] AS DECIMAL(10, 2))), '0.00')
            ,[Effi_360_day] =FORMAT(AVG(CAST([Effi_360_day] AS DECIMAL(10, 2))), '0.00')
            From
            (
	            SELECT 
	            [ST_MC_Type] = lmbd.MachineTypeID
	            ,[Motion] =  Operation_P06.val
	            ,Effi_90_day = Effi_90_day.VAL
	            ,Effi_180_day = Effi_180_day.VAL
	            ,Effi_270_day = Effi_270_day.VAL
	            ,Effi_360_day = Effi_360_day.VAL
	            from Employee e
	            INNER JOIN LineMappingBalancing_Detail lmbd on lmbd.EmployeeID = e.ID
	            INNER JOIN LineMappingBalancing lmb on lmb.id = lmbd.ID
	            OUTER APPLY
	            (
	            select val = stuff((select distinct concat(',',Name)
		            from OperationRef a
		            inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
		            where a.CodeType = '00007' and a.id = lmbd.OperationID  for xml path('') ),1,1,'')
	            )Operation_P06
	            OUTER APPLY 
	            (
		            SELECT VAL = FORMAT(CAST(iif(oplmbd.Cycle = 0,0,ROUND(oplmbd.GSD/ oplmbd.Cycle,2)*100) AS DECIMAL(10, 2)), '0.00')
		            FROM LineMappingBalancing oplmb 
		            inner join LineMappingBalancing_Detail oplmbd on oplmb.ID = oplmbd.ID
		            WHERE oplmbd.EmployeeID = E.ID
		            AND ((oplmb.EditDate >= DATEADD(DAY, -90, GETDATE()) and oplmb.EditDate <= GETDATE()) or (oplmb.AddDate >= DATEADD(DAY, -90, GETDATE()) and oplmb.AddDate <= GETDATE()))
	            )Effi_90_day
	            OUTER APPLY 
	            (
		            SELECT VAL = FORMAT(CAST(iif(oplmbd.Cycle = 0,0,ROUND(oplmbd.GSD/ oplmbd.Cycle,2)*100) AS DECIMAL(10, 2)), '0.00')
		            FROM LineMappingBalancing oplmb 
		            inner join LineMappingBalancing_Detail oplmbd on oplmb.ID = oplmbd.ID
		            WHERE oplmbd.EmployeeID = E.ID
		            AND ((oplmb.EditDate >= DATEADD(DAY, -180, GETDATE()) and oplmb.EditDate <= GETDATE()) or (oplmb.AddDate >= DATEADD(DAY, -180, GETDATE()) and oplmb.AddDate <= GETDATE()))
	            )Effi_180_day
	            OUTER APPLY 
	            (
		            SELECT VAL = FORMAT(CAST(iif(oplmbd.Cycle = 0,0,ROUND(oplmbd.GSD/ oplmbd.Cycle,2)*100) AS DECIMAL(10, 2)), '0.00')
		            FROM LineMappingBalancing oplmb 
		            inner join LineMappingBalancing_Detail oplmbd on oplmb.ID = oplmbd.ID
		            WHERE oplmbd.EmployeeID = E.ID
		            AND ((oplmb.EditDate >= DATEADD(DAY, -270, GETDATE()) and oplmb.EditDate <= GETDATE()) or (oplmb.AddDate >= DATEADD(DAY, -270, GETDATE()) and oplmb.AddDate <= GETDATE()))
	            )Effi_270_day
	            OUTER APPLY 
	            (
		            SELECT VAL = FORMAT(CAST(iif(oplmbd.Cycle = 0,0,ROUND(oplmbd.GSD/ oplmbd.Cycle,2)*100) AS DECIMAL(10, 2)), '0.00')
		            FROM LineMappingBalancing oplmb 
		            inner join LineMappingBalancing_Detail oplmbd on oplmb.ID = oplmbd.ID
		            WHERE oplmbd.EmployeeID = E.ID
		            AND ((oplmb.EditDate >= DATEADD(DAY, -360, GETDATE()) and oplmb.EditDate <= GETDATE()) or (oplmb.AddDate >= DATEADD(DAY, -360, GETDATE()) and oplmb.AddDate <= GETDATE()))
	            )Effi_360_day
	            where 
	            e.FactoryID = '{this.CurrentMaintain["FactoryID"]}' and e.ID = '{this.CurrentMaintain["ID"]}' AND
	            ((lmb.EditDate >= DATEADD(YEAR, -3, GETDATE()) and lmb.EditDate <= GETDATE()) or (lmb.AddDate >= DATEADD(YEAR, -3, GETDATE()) and lmb.AddDate <= GETDATE()))
            )b
            GROUP BY [ST_MC_Type],[Motion]";

            DualResult result1 = DBProxy.Current.Select(null, sqlcmdDetail, out this.dtDetail);

            if (!result1)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return;
            }

            this.gridDetail.DataSource = this.dtDetail;

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
    }
}
