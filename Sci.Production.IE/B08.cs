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

            string sqlcmd = $@"
            -- 整合查詢
            DECLARE @goDate date = getdate(); -- '2024-08-12'

            SELECT
            [ST_MC_Type] = ISNULL(lmd.MachineTypeID,''),
            [Motion] = ISNULL(Operation_P03.val,''),
            [Group_Header] =  ISNULL(IIF(REPLACE(tsd.[location], '--', '') = '', REPLACE(OP.OperationID, '--', '') ,REPLACE(tsd.[location], '--', '')),''),
            [Part] = ISNULL(lmd.SewingMachineAttachmentID,''),
            [Attachment] = ISNULL(lmd.Attachment,''),
            [Effi_3_year] = ISNULL(FORMAT(AVG((lmd.GSD / lmd.Cycle)*100), '0.00'), '0.00')
            FROM Employee e WITH(NOLOCK)
            INNER JOIN LineMapping_Detail lmd WITH(NOLOCK) ON lmd.EmployeeID = e.ID
            INNER JOIN LineMapping lm_Day WITH(NOLOCK) ON lm_Day.id = lmd.ID  AND ((lm_Day.EditDate >= DATEADD(YEAR, -3, @goDate) AND lm_Day.EditDate <= @goDate) OR (lm_Day.AddDate >= DATEADD(YEAR, -3, @goDate) AND lm_Day.AddDate <= @goDate))
            INNER JOIN TimeStudy TS WITH(NOLOCK) ON TS.StyleID = lm_Day.StyleID AND TS.SeasonID = lm_Day.SeasonID AND TS.ComboType = lm_Day.ComboType AND TS.BrandID = lm_Day.BrandID
            LEFT JOIN TimeStudy_Detail tsd WITH(NOLOCK) ON lmd.OperationID = tsd.OperationID and tsd.id = ts.id
            OUTER APPLY (
                SELECT val = STUFF((
                    SELECT DISTINCT CONCAT(',', Name)
                    FROM OperationRef a WITH(NOLOCK)
                    INNER JOIN IESELECTCODE b WITH(NOLOCK) ON a.CodeID = b.ID AND a.CodeType = b.Type
                    WHERE a.CodeType = '00007' AND a.id = lmd.OperationID  
                    FOR XML PATH('')), 1, 1, '')
            ) Operation_P03
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
            WHERE e.FactoryID = '{this.CurrentMaintain["FactoryID"]}' AND e.ID = '{this.CurrentMaintain["ID"]}' AND (ISNULL(lmd.MachineTypeID,'') != '')
            GROUP BY ISNULL(lmd.Attachment,''),ISNULL(lmd.MachineTypeID,''), ISNULL(Operation_P03.val,''),ISNULL(IIF(REPLACE(tsd.[location], '--', '') = '', REPLACE(OP.OperationID, '--', '') ,REPLACE(tsd.[location], '--', '')),''), ISNULL(lmd.SewingMachineAttachmentID,'')

            UNION ALL

            SELECT
            [ST_MC_Type] = ISNULL(lmbd.MachineTypeID,''),
            [Motion] = ISNULL(Operation_P06.val,''),
            [Group_Header] =  ISNULL(IIF(REPLACE(tsd.[location], '--', '') = '', REPLACE(OP.OperationID, '--', '') ,REPLACE(tsd.[location], '--', '')),''),
            [Part] = ISNULL(lmbd.SewingMachineAttachmentID,''),
            [Attachment] = ISNULL(lmbd.Attachment,''),
            [Effi_3_year] = ISNULL(FORMAT(AVG((lmbd.GSD / lmbd.Cycle)*100), '0.00'), '0.00')
            FROM Employee e WITH(NOLOCK)
            INNER JOIN LineMappingBalancing_Detail lmbd WITH(NOLOCK) ON lmbd.EmployeeID = e.ID
            INNER JOIN LineMappingBalancing lmb_Day WITH(NOLOCK) ON lmb_Day.id = lmbd.ID  AND ((lmb_Day.EditDate >= DATEADD(YEAR, -3, @goDate) AND lmb_Day.EditDate <= @goDate) OR (lmb_Day.AddDate >= DATEADD(YEAR, -3, @goDate) AND lmb_Day.AddDate <= @goDate))
            INNER JOIN TimeStudy TS WITH(NOLOCK) ON TS.StyleID = lmb_Day.StyleID AND TS.SeasonID = lmb_Day.SeasonID AND TS.ComboType = lmb_Day.ComboType AND TS.BrandID = lmb_Day.BrandID
            LEFT JOIN TimeStudy_Detail tsd WITH(NOLOCK) ON lmbd.OperationID = tsd.OperationID AND TSD.ID = TS.ID
            OUTER APPLY (
                SELECT val = STUFF((
                    SELECT DISTINCT CONCAT(',', Name)
                    FROM OperationRef a WITH(NOLOCK)
                    INNER JOIN IESELECTCODE b WITH(NOLOCK) ON a.CodeID = b.ID AND a.CodeType = b.Type
                    WHERE a.CodeType = '00007' AND a.id = lmbd.OperationID  
                    FOR XML PATH('')), 1, 1, '')
            ) Operation_P06
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
	            WHERE ID =  TS.ID AND SEQ <= (SELECT TOP 1 Seq FROM TimeStudy_Detail WHERE id = TS.ID AND OperationID = lmbd.OperationID ORDER BY Seq DESC)
	            ORDER BY SEQ DESC
            )OP
            WHERE e.FactoryID = '{this.CurrentMaintain["FactoryID"]}' AND e.ID = '{this.CurrentMaintain["ID"]}' AND (ISNULL(lmbd.MachineTypeID,'') != '')
            GROUP BY ISNULL(lmbd.Attachment,''),ISNULL(lmbd.MachineTypeID,''), ISNULL(Operation_P06.val,''),ISNULL(IIF(REPLACE(tsd.[location], '--', '') = '', REPLACE(OP.OperationID, '--', '') ,REPLACE(tsd.[location], '--', '')),''), ISNULL(lmbd.SewingMachineAttachmentID,'');
            ";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.dtOperatorDetail);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return;
            }

            this.btnOperationHistory.ForeColor = this.dtOperatorDetail.Rows.Count > 0 ? Color.Blue : Color.Black;

            string sqlcmdDetail = $@"
            -- 整合查詢
            DECLARE @goDate date =  GETDATE()

            SELECT
            [ST_MC_Type] = ISNULL(lmd.MachineTypeID,'')
            ,[Motion] = ISNULL(Operation_P03.val,'')
            ,[Effi_90_day] =  ISNULL(FORMAT(AVG(CASE WHEN d.DayRange = 90 THEN (lmd.GSD / lmd.Cycle)*100 ELSE NULL END) , '0.00'),0)
            ,[Effi_180_day] = ISNULL(FORMAT(AVG(CASE WHEN d.DayRange = 180 THEN (lmd.GSD / lmd.Cycle)*100 ELSE NULL END), '0.00'),0)
            ,[Effi_270_day] = ISNULL(FORMAT(AVG(CASE WHEN d.DayRange = 270 THEN (lmd.GSD / lmd.Cycle)*100 ELSE NULL END), '0.00'),0)
            ,[Effi_360_day] = ISNULL(FORMAT(AVG(CASE WHEN d.DayRange = 360 THEN (lmd.GSD / lmd.Cycle)*100 ELSE NULL END), '0.00'),0)
            FROM Employee e WITH(NOLOCK)
            INNER JOIN LineMapping_Detail lmd WITH(NOLOCK) ON lmd.EmployeeID = e.ID
            LEFT JOIN (VALUES (90),(180),(270),(360)) AS d (DayRange) ON 1=1
            INNER JOIN LineMapping lm_Day WITH(NOLOCK) ON lm_Day.id = lmd.ID  AND ((lm_Day.EditDate >= DATEADD(DAY, -d.DayRange, @goDate) AND lm_Day.EditDate <= @goDate) OR (lm_Day.AddDate >= DATEADD(DAY, -d.DayRange, @goDate) AND lm_Day.AddDate <= @goDate))
            OUTER APPLY (
	            SELECT val = STUFF((
	            SELECT DISTINCT CONCAT(',', Name)
	            FROM OperationRef a WITH(NOLOCK)
	            INNER JOIN IESELECTCODE b WITH(NOLOCK) ON a.CodeID = b.ID AND a.CodeType = b.Type
	            WHERE a.CodeType = '00007' AND a.id = lmd.OperationID  
	            FOR XML PATH('')), 1, 1, '')
            ) Operation_P03
            WHERE e.FactoryID = '{this.CurrentMaintain["FactoryID"]}' AND e.ID = '{this.CurrentMaintain["ID"]}' AND (ISNULL(lmd.MachineTypeID,'') != '')
            GROUP BY ISNULL(lmd.MachineTypeID,''), ISNULL(Operation_P03.val,'')

            UNION ALL

            SELECT
            [ST_MC_Type] = ISNULL(lmbd.MachineTypeID,'')
            ,[Motion] = ISNULL(Operation_P06.val,'')
            ,[Effi_90_day] =  ISNULL(FORMAT(AVG(CASE WHEN d.DayRange = 90 THEN  (lmbd.GSD / lmbd.Cycle)*100 ELSE NULL END) , '0.00'),0)
            ,[Effi_180_day] = ISNULL(FORMAT(AVG(CASE WHEN d.DayRange = 180 THEN (lmbd.GSD / lmbd.Cycle)*100 ELSE NULL END), '0.00'),0)
            ,[Effi_270_day] = ISNULL(FORMAT(AVG(CASE WHEN d.DayRange = 270 THEN (lmbd.GSD / lmbd.Cycle)*100 ELSE NULL END), '0.00'),0)
            ,[Effi_360_day] = ISNULL(FORMAT(AVG(CASE WHEN d.DayRange = 360 THEN (lmbd.GSD / lmbd.Cycle)*100 ELSE NULL END), '0.00'),0)
            FROM Employee e WITH(NOLOCK)
            LEFT JOIN (VALUES (90),(180),(270),(360)) AS d (DayRange) ON 1=1
            INNER JOIN LineMappingBalancing_Detail lmbd WITH(NOLOCK) ON lmbd.EmployeeID = e.ID
            INNER JOIN LineMappingBalancing lmb_Day WITH(NOLOCK) ON lmb_Day.id = lmbd.ID  AND ((lmb_Day.EditDate >= DATEADD(DAY, -d.DayRange, @goDate) AND lmb_Day.EditDate <= @goDate) OR (lmb_Day.AddDate >= DATEADD(DAY, -d.DayRange, @goDate) AND lmb_Day.AddDate <= @goDate))
            OUTER APPLY (
	            SELECT val = STUFF((
	            SELECT DISTINCT CONCAT(',', Name)
	            FROM OperationRef a WITH(NOLOCK)
	            INNER JOIN IESELECTCODE b WITH(NOLOCK) ON a.CodeID = b.ID AND a.CodeType = b.Type
	            WHERE a.CodeType = '00007' AND a.id = lmbd.OperationID  
	            FOR XML PATH('')), 1, 1, '')
            ) Operation_P06
            WHERE e.FactoryID = '{this.CurrentMaintain["FactoryID"]}' AND e.ID = '{this.CurrentMaintain["ID"]}' AND (ISNULL(lmbd.MachineTypeID,'') != '')
            GROUP BY ISNULL(lmbd.MachineTypeID,''), ISNULL(Operation_P06.val,'');
            ";

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
