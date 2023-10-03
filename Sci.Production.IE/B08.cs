using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_B08
    /// </summary>
    public partial class B08 : Win.Tems.Input1
    {
        private int itemCount;
        /// <summary>
        /// B08
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Env.User.Factory + $"' ";
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
            switch (Env.User.Factory)
            {
                case "MAI":
                case "MA2":
                case "MA3":
                case "MW2":
                case "FIT":
                case "MWI":
                case "FAC":
                case "FA2":
                case "PSR":
                case "VT1":
                case "VT2":
                case "GMM":
                case "GM2":
                case "GMI":
                case "PS2":
                case "ALA":
                    strDept = $"'PRO'";
                    strPosition = $"'PCK','PRS','SEW','FSPR','LOP','STL','LL','SLS','SSLT'";
                    strWhere = $@" and Dept in({strDept})  and Position in({strPosition})";
                    break;
                case "ESP":
                case "ES2":
                case "ES3":
                case "VSP":
                    strDept = $"'PRO'";
                    strPosition = $"'PAC','PRS','SEW','LL'";
                    strWhere = $@" and Dept in({strDept})  and Position in({strPosition})";
                    break;
                case "SPT":
                    strDept = $"'PRO'";
                    strPosition = $"'PAC','PRS','SEW','LL','SUP','PE','PIT','TL'";
                    strWhere = $@" and Dept in({strDept})  and Position in({strPosition})";
                    break;
                case "SNP":
                    strDept = $"'PRO'";
                    strPosition = $"'SEW','LL','PIT'";
                    strWhere = $@" and Dept in({strDept})  and Position in({strPosition})";
                    break;
                case "SPS":
                case "SPR":
                    strDept = $"'SEW'";
                    strPosition = $"'SWR','TRNEE','Lneldr','LINSUP','PRSSR','PCKR'";
                    strWhere = $@" and Dept in({strDept})  and Position in({strPosition})";
                    break;
            }

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
    }
}
