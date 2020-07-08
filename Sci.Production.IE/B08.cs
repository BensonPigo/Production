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
    public partial class B08 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B08
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Sci.Env.User.Keyword + "'";
        }

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // 新增Import From Barcode按鈕
            Sci.Win.UI.Button btn = new Sci.Win.UI.Button();
            btn.Text = "Import From Excel";
            btn.Click += new EventHandler(this.Btn_Click);
            this.browsetop.Controls.Add(btn);
            btn.Size = new Size(165, 30); // 預設是(80,30)
        }

        // Import From Barcode按鈕的Click事件
        private void Btn_Click(object sender, EventArgs e)
        {
            string excelFile = MyUtility.File.GetFile("Excel files (*.xlsx)|*.xlsx");
            if (MyUtility.Check.Empty(excelFile))
            {
                return;
            }

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(excelFile);
            if (excel == null)
            {
                return;
            }

            DataTable excelDataTable, mFactory, updateData;
            string sqlCmd = "select MDIVISIONID,FactoryID,ID,Name,Skill,OnBoardDate,ResignationDate,SewingLineID,SPACE(250) as ErrorMsg from Employee WITH (NOLOCK)	where 1 = 0";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out excelDataTable);

            sqlCmd = string.Format("select * from Factory WITH (NOLOCK)	where MDivisionID = '{0}'", Sci.Env.User.Keyword);
            result = DBProxy.Current.Select(null, sqlCmd, out mFactory);
            mFactory.PrimaryKey = new DataColumn[] { mFactory.Columns["ID"] };

            // UpdateData = ((DataTable)gridbs.DataSource).Clone();
            sqlCmd = "select * from Employee WITH (NOLOCK)    where 1 = 0";
            result = DBProxy.Current.Select(null, sqlCmd, out updateData);

            this.ShowWaitMessage("Starting EXCEL...");
            excel.Visible = false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            int intRowsCount = worksheet.UsedRange.Rows.Count;
            int intColumnsCount = worksheet.UsedRange.Columns.Count;
            int intRowsStart = 2;
            int intRowsRead = intRowsStart - 1;

            Microsoft.Office.Interop.Excel.Range range;
            object[,] objCellArray;

            while (intRowsRead < intRowsCount)
            {
                intRowsRead++;

                range = worksheet.Range[string.Format("A{0}:G{0}", intRowsRead)];
                objCellArray = range.Value;

                DataRow newRow = excelDataTable.NewRow();
                newRow["FactoryID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");
                newRow["ID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C");
                newRow["Name"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C");
                newRow["Skill"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C");
                newRow["OnBoardDate"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "D");
                newRow["ResignationDate"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "D");
                newRow["SewingLineID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 7], "C");
                newRow["ErrorMsg"] = string.Empty;

                excelDataTable.Rows.Add(newRow);
            }

            excel.Workbooks.Close();
            excel.Quit();
            excel = null;

            int hasError = 0, hasInsert = 0;
            foreach (DataRow dr in excelDataTable.Rows)
            {
                if (!MyUtility.Check.Empty(dr["FactoryID"]) && !MyUtility.Check.Empty(dr["ID"]))
                {
                    // DataRow[] findData = MFactory.Select(string.Format("ID = {0}", MyUtility.Convert.GetString(dr["FactoryID"])));
                    DataRow findData = mFactory.Rows.Find(MyUtility.Convert.GetString(dr["FactoryID"]));
                    if (findData != null)
                    {
                        DataRow newRow = updateData.NewRow();
                        newRow["FactoryID"] = dr["FactoryID"];
                        newRow["ID"] = dr["ID"];
                        newRow["Name"] = dr["Name"];
                        newRow["Skill"] = dr["Skill"];
                        newRow["OnBoardDate"] = dr["OnBoardDate"];
                        newRow["ResignationDate"] = dr["ResignationDate"];
                        newRow["SewingLineID"] = dr["SewingLineID"];
                        newRow["MDivisionID"] = Sci.Env.User.Keyword;
                        newRow["AddName"] = Sci.Env.User.UserID;
                        newRow["AddDate"] = DateTime.Now;
                        newRow["EditName"] = string.Empty;
                        newRow["EditDate"] = DBNull.Value;
                        updateData.Rows.Add(newRow);

                        dr["ErrorMsg"] = "Job is completed!";
                        hasInsert = 1;
                    }
                    else
                    {
                        dr["ErrorMsg"] = "< Factory > not exist!";
                        hasError = 1;
                    }
                }
                else
                {
                    dr["ErrorMsg"] = "< Factory or Employee > not exist!";
                    hasError = 1;
                }
            }

            if (!MyUtility.Tool.CursorUpdateTable(updateData, "Employee", "Production"))
            {
                this.HideWaitMessage();
                MyUtility.Msg.WarningBox("Import data fail. Pls try again!");
                return;
            }
            else
            {
                if (hasInsert == 1)
                {
                    MyUtility.Msg.InfoBox("Excel data import completed.");
                }
            }

            this.HideWaitMessage();

            if (hasError == 1)
            {
                MyUtility.Msg.WarningBox("There is some error, please check result!");

                string strXltName = Sci.Env.Cfg.XltPathDir + "\\IE_P08_ImportResult.xltx";
                Microsoft.Office.Interop.Excel.Application exportExcel = MyUtility.Excel.ConnectExcel(strXltName);
                if (exportExcel == null)
                {
                    return;
                }

                Microsoft.Office.Interop.Excel.Worksheet worksheet1 = exportExcel.ActiveWorkbook.Worksheets[1];

                object[,] objArray = new object[1, 7];
                intRowsStart = 2;
                int rownum = 0;
                for (int i = 0; i < excelDataTable.Rows.Count; i++)
                {
                    DataRow dr = excelDataTable.Rows[i];
                    rownum = intRowsStart + i;
                    objArray[0, 0] = dr["FactoryID"];
                    objArray[0, 1] = dr["ID"];
                    objArray[0, 2] = dr["Name"];
                    objArray[0, 3] = dr["Skill"];
                    objArray[0, 4] = dr["OnBoardDate"];
                    objArray[0, 5] = dr["ResignationDate"];
                    objArray[0, 6] = dr["ErrorMsg"];

                    worksheet1.Range[string.Format("A{0}:G{0}", rownum)].Value2 = objArray;
                }

                exportExcel.Visible = true;
            }
        }

        /// <summary>
        /// ClickNewAfter()
        /// </summary>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            this.txtSkill.BackColor = Color.White;
        }

        /// <summary>
        /// ClickEditAfter()
        /// </summary>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtEmployee.ReadOnly = true;
            this.txtSkill.BackColor = Color.White;
        }

        /// <summary>
        /// ClickSaveBefore()
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
            {
                MyUtility.Msg.WarningBox("< Factory > can not be empty!");
                this.txtmfactory.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("< Employee# > can not be empty!");
                this.txtEmployee.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Name"]))
            {
                MyUtility.Msg.WarningBox("< Nick Name > can not be empty!");
                this.txtNickName.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["OnBoardDate"]))
            {
                MyUtility.Msg.WarningBox("< Hired on > can not be empty!");
                this.dateHiredOn.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["SewingLineID"]))
            {
                MyUtility.Msg.WarningBox("< Line > can not be empty!");
                this.txtsewingline.Focus();
                return false;
            }

            this.txtSkill.BackColor = this.displayM.BackColor;
            return base.ClickSaveBefore();
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

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\IE_P08.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            object[,] objArray = new object[1, 7];
            int intRowsStart = 2;
            int rownum = 0;
            for (int i = 0; i < browseData.Rows.Count; i++)
            {
                DataRow dr = browseData.Rows[i];
                rownum = intRowsStart + i;
                objArray[0, 0] = dr["FactoryID"];
                objArray[0, 1] = dr["ID"];
                objArray[0, 2] = dr["Name"];
                objArray[0, 3] = dr["Skill"];
                objArray[0, 4] = dr["OnBoardDate"];
                objArray[0, 5] = dr["ResignationDate"];
                objArray[0, 6] = dr["SewingLineID"];

                worksheet.Range[string.Format("A{0}:G{0}", rownum)].Value2 = objArray;
            }

            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("IE_P08");
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
                Ict.DualResult returnResule = DBProxy.Current.Select("Machine", "select ID,Description from MachineGroup WITH (NOLOCK)	where Junk = 0 order by ID", out machineGroup);
                Sci.Win.Tools.SelectItem2 item = new Sci.Win.Tools.SelectItem2(machineGroup, "ID,Description", "Group ID,Description", "2,35", this.txtSkill.Text);

                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                string returnData = string.Empty;
                IList<DataRow> gridData = item.GetSelecteds();
                foreach (DataRow currentRecord in gridData)
                {
                    returnData = returnData + currentRecord["ID"].ToString() + ",";
                }

                this.CurrentMaintain["Skill"] = returnData.ToString();
            }
        }
    }
}
