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
                [ST_MC_Type] =ISNULL(lmd.MachineTypeID,'')
                ,[Motion] = ISNULL(Operation_P03.val,'')
                ,[Group_Header] = ISNULL(tsd.[location] ,'')
                ,[Part] = ISNULL(lmd.SewingMachineAttachmentID,'')
                ,[Attachment] = ISNULL(lmd.Attachment,'')
                ,Effi_3_year = FORMAT(CAST(iif(lmd.Cycle = 0,0,ROUND(lmd.GSD/ lmd.Cycle * 100,2)) AS DECIMAL(10, 2)), '0.00')
                from Employee e
                LEFT JOIN LineMapping_Detail lmd WITH(NOLOCK) on lmd.EmployeeID = e.ID　
                LEFT JOIN LineMapping lm WITH(NOLOCK) on lm.id = lmd.ID AND ((lm.EditDate >= DATEADD(YEAR, -3, GETDATE()) and lm.EditDate <= GETDATE()) or (lm.AddDate >= DATEADD(YEAR, -3, GETDATE()) and lm.AddDate <= GETDATE()))
                LEFT JOIN TimeStudy_Detail tsd WITH(NOLOCK) on lmd.OperationID = tsd.OperationID
                OUTER APPLY
                (
                select val = stuff((select distinct concat(',',Name)
		                from OperationRef a
		                inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
		                where a.CodeType = '00007' and a.id = lmd.OperationID  for xml path('') ),1,1,'')
                )Operation_P03
	            WHERE 
	            e.FactoryID = '{this.CurrentMaintain["FactoryID"]}' and e.ID = '{this.CurrentMaintain["ID"]}' AND lmd.MachineTypeID IS NOT NULL
            )a
            WHERE a.ST_MC_Type != ''
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
	            ,Effi_3_year = FORMAT(CAST(iif(lmbd.Cycle = 0,0,ROUND(lmbd.GSD/ lmbd.Cycle*100,2)) AS DECIMAL(10, 2)), '0.00')
	            from Employee e
	            LEFT JOIN LineMappingBalancing_Detail lmbd on lmbd.EmployeeID = e.ID
	            LEFT JOIN LineMappingBalancing lmb on lmb.id = lmbd.ID AND ((lmb.EditDate >= DATEADD(YEAR, -3, GETDATE()) and lmb.EditDate <= GETDATE()) or (lmb.AddDate >= DATEADD(YEAR, -3, GETDATE()) and lmb.AddDate <= GETDATE()))
	            OUTER APPLY
	            (
	            select val = stuff((select distinct concat(',',Name)
		            from OperationRef a
		            inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
		            where a.CodeType = '00007' and a.id = lmbd.OperationID  for xml path('') ),1,1,'')
	            )Operation_P06
	            where 
	            e.FactoryID = '{this.CurrentMaintain["FactoryID"]}' and e.ID = '{this.CurrentMaintain["ID"]}' AND lmbd.MachineTypeID IS NOT NULL
            )b
            WHERE b.ST_MC_Type != ''
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
                [ST_MC_Type] = ISNULL(lmd_360_Day.MachineTypeID,'')
                ,[Motion] = ISNULL(Operation_P03.val,'')
                ,Effi_90_day = FORMAT(CAST(iif(lmd_90_Day.Cycle = 0,0,ROUND(lmd_90_Day.GSD/ lmd_90_Day.Cycle * 100,2)) AS DECIMAL(10, 2)), '0.00')
                ,Effi_180_day = FORMAT(CAST(iif(lmd_180_Day.Cycle = 0,0,ROUND(lmd_180_Day.GSD/ lmd_180_Day.Cycle * 100,2)) AS DECIMAL(10, 2)), '0.00')
                ,Effi_270_day = FORMAT(CAST(iif(lmd_270_Day.Cycle = 0,0,ROUND(lmd_270_Day.GSD/ lmd_270_Day.Cycle * 100,2)) AS DECIMAL(10, 2)), '0.00')
                ,Effi_360_day = FORMAT(CAST(iif(lmd_360_Day.Cycle = 0,0,ROUND(lmd_360_Day.GSD/ lmd_360_Day.Cycle * 100,2)) AS DECIMAL(10, 2)), '0.00')
                from Employee e
				left JOIN LineMapping_Detail lmd_360_Day WITH(NOLOCK) on lmd_360_Day.EmployeeID = e.ID
                left JOIN LineMapping lm_360_Day WITH(NOLOCK) on lm_360_Day.id = lmd_360_Day.ID AND ((lm_360_Day.EditDate >= DATEADD(DAY, -360, GETDATE()) and lm_360_Day.EditDate <= GETDATE()) or (lm_360_Day.AddDate >= DATEADD(DAY, -360, GETDATE()) and lm_360_Day.AddDate <= GETDATE()))

				left JOIN LineMapping_Detail lmd_270_Day WITH(NOLOCK) on lmd_270_Day.EmployeeID = e.ID AND lmd_270_Day.MachineTypeID = lmd_360_Day.MachineTypeID
                left JOIN LineMapping lm_270_Day WITH(NOLOCK) on lm_270_Day.id = lmd_270_Day.ID AND ((lm_270_Day.EditDate >= DATEADD(DAY, -270, GETDATE()) and lm_270_Day.EditDate <= GETDATE()) or (lm_270_Day.AddDate >= DATEADD(DAY, -270, GETDATE()) and lm_270_Day.AddDate <= GETDATE()))

				left JOIN LineMapping_Detail lmd_180_Day WITH(NOLOCK) on lmd_180_Day.EmployeeID = e.ID  AND lmd_180_Day.MachineTypeID = lmd_360_Day.MachineTypeID
                left JOIN LineMapping lm_180_Day WITH(NOLOCK) on lm_180_Day.id = lmd_180_Day.ID AND ((lm_180_Day.EditDate >= DATEADD(DAY, -180, GETDATE()) and lm_180_Day.EditDate <= GETDATE()) or (lm_180_Day.AddDate >= DATEADD(DAY, -180, GETDATE()) and lm_180_Day.AddDate <= GETDATE()))


                left JOIN LineMapping_Detail lmd_90_Day WITH(NOLOCK) on lmd_90_Day.EmployeeID = e.ID　AND lmd_90_Day.MachineTypeID = lmd_360_Day.MachineTypeID
                left JOIN LineMapping lm_90_Day WITH(NOLOCK) on lm_90_Day.id = lmd_90_Day.ID AND ((lm_90_Day.EditDate >= DATEADD(DAY, -90, GETDATE()) and lm_90_Day.EditDate <= GETDATE()) or (lm_90_Day.AddDate >= DATEADD(DAY, -90, GETDATE()) and lm_90_Day.AddDate <= GETDATE()))
                OUTER APPLY
                (
                select val = stuff((select distinct concat(',',Name)
		                from OperationRef a
		                inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
		                where a.CodeType = '00007' and a.id = lmd_360_Day.OperationID  for xml path('') ),1,1,'')
                )Operation_P03
	            WHERE 
	            e.FactoryID = '{this.CurrentMaintain["FactoryID"]}' and e.ID = '{this.CurrentMaintain["ID"]}'
            )a
            WHERE a.ST_MC_Type != ''
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
                [ST_MC_Type] = lmbd_360_Day.MachineTypeID
                ,[Motion] =  Operation_P06.val
                ,Effi_90_day = FORMAT(CAST(iif(lmbd_90_Day.Cycle = 0,0,ROUND(lmbd_90_Day.GSD/ lmbd_90_Day.Cycle *100 ,2)) AS DECIMAL(10, 2)), '0.00')
                ,Effi_180_day = FORMAT(CAST(iif(lmbd_180_Day.Cycle = 0,0,ROUND(lmbd_180_Day.GSD/ lmbd_180_Day.Cycle *100 ,2)) AS DECIMAL(10, 2)), '0.00')
                ,Effi_270_day = FORMAT(CAST(iif(lmbd_270_Day.Cycle = 0,0,ROUND(lmbd_270_Day.GSD/ lmbd_270_Day.Cycle *100 ,2)) AS DECIMAL(10, 2)), '0.00')
                ,Effi_360_day = FORMAT(CAST(iif(lmbd_360_Day.Cycle = 0,0,ROUND(lmbd_360_Day.GSD/ lmbd_360_Day.Cycle *100 ,2)) AS DECIMAL(10, 2)), '0.00')
                from Employee e
                INNER JOIN LineMappingBalancing_Detail lmbd_360_Day on lmbd_360_Day.EmployeeID = e.ID
                INNER JOIN LineMappingBalancing lmb_360_Day on lmb_360_Day.id = lmbd_360_Day.ID AND ((lmb_360_Day.EditDate >= DATEADD(DAY, -360, GETDATE()) and lmb_360_Day.EditDate <= GETDATE()) or (lmb_360_Day.AddDate >= DATEADD(DAY, -360, GETDATE()) and lmb_360_Day.AddDate <= GETDATE()))

                INNER JOIN LineMappingBalancing_Detail lmbd_270_Day on lmbd_270_Day.EmployeeID = e.ID AND lmbd_270_Day.MachineTypeID = lmbd_360_Day.MachineTypeID
                INNER JOIN LineMappingBalancing lmb_270_Day on lmb_270_Day.id = lmbd_270_Day.ID AND ((lmb_270_Day.EditDate >= DATEADD(DAY, -270, GETDATE()) and lmb_270_Day.EditDate <= GETDATE()) or (lmb_270_Day.AddDate >= DATEADD(DAY, -270, GETDATE()) and lmb_270_Day.AddDate <= GETDATE()))

                INNER JOIN LineMappingBalancing_Detail lmbd_180_Day on lmbd_180_Day.EmployeeID = e.ID AND lmbd_180_Day.MachineTypeID = lmbd_360_Day.MachineTypeID
                INNER JOIN LineMappingBalancing lmb_180_Day on lmb_180_Day.id = lmbd_180_Day.ID AND ((lmb_180_Day.EditDate >= DATEADD(DAY, -180, GETDATE()) and lmb_180_Day.EditDate <= GETDATE()) or (lmb_180_Day.AddDate >= DATEADD(DAY, -180, GETDATE()) and lmb_180_Day.AddDate <= GETDATE()))

                INNER JOIN LineMappingBalancing_Detail lmbd_90_Day on lmbd_90_Day.EmployeeID = e.ID AND lmbd_90_Day.MachineTypeID = lmbd_360_Day.MachineTypeID
                INNER JOIN LineMappingBalancing lmb_90_Day on lmb_90_Day.id = lmbd_90_Day.ID AND ((lmb_90_Day.EditDate >= DATEADD(DAY, -90, GETDATE()) and lmb_90_Day.EditDate <= GETDATE()) or (lmb_90_Day.AddDate >= DATEADD(DAY, -90, GETDATE()) and lmb_90_Day.AddDate <= GETDATE()))
                OUTER APPLY
                (
                select val = stuff((select distinct concat(',',Name)
	                from OperationRef a
	                inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
	                where a.CodeType = '00007' and a.id = lmbd_360_Day.OperationID  for xml path('') ),1,1,'')
                )Operation_P06
	            where 
	            e.FactoryID = '{this.CurrentMaintain["FactoryID"]}' and e.ID = '{this.CurrentMaintain["ID"]}'
            )b
            WHERE b.ST_MC_Type != ''
            GROUP BY [ST_MC_Type],[Motion]";

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
    }
}
