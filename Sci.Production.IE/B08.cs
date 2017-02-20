using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.IE
{
    public partial class B08 : Sci.Win.Tems.Input1
    {
        public B08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Sci.Env.User.Keyword + "'";
            
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            //新增Import From Barcode按鈕
            Sci.Win.UI.Button btn = new Sci.Win.UI.Button();
            btn.Text = "Import From Excel";
            btn.Click += new EventHandler(btn_Click);
            browsetop.Controls.Add(btn);
            btn.Size = new Size(165, 30);//預設是(80,30)
        }

        //Import From Barcode按鈕的Click事件
        private void btn_Click(object sender, EventArgs e)
        {
            string excelFile = MyUtility.File.GetFile("Excel files (*.xlsx)|*.xlsx");
            if (MyUtility.Check.Empty(excelFile))
            {
                return;
            }

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(excelFile);
            if (excel == null) return;

            DataTable ExcelDataTable, MFactory, UpdateData;
            string sqlCmd = "select FactoryID,ID,Name,Skill,OnBoardDate,ResignationDate,SewingLineID,SPACE(250) as ErrorMsg from Employee WITH (NOLOCK)	where 1 = 0";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out ExcelDataTable);

            sqlCmd = string.Format("select * from Factory WITH (NOLOCK)	where MDivisionID = '{0}'", Sci.Env.User.Keyword);
            result = DBProxy.Current.Select(null, sqlCmd, out MFactory);
            MFactory.PrimaryKey = new DataColumn[] { MFactory.Columns["ID"] };

            UpdateData = ((DataTable)gridbs.DataSource).Clone();

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
              
                range = worksheet.Range[String.Format("A{0}:G{0}", intRowsRead)];
                objCellArray = range.Value;

                DataRow newRow = ExcelDataTable.NewRow();
                newRow["FactoryID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");
                newRow["ID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C");
                newRow["Name"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C");
                newRow["Skill"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C");
                newRow["OnBoardDate"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "D");
                newRow["ResignationDate"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "D");
                newRow["SewingLineID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 7], "C");
                newRow["ErrorMsg"] = "";

                ExcelDataTable.Rows.Add(newRow);
            }

            excel.Workbooks.Close();
            excel.Quit();
            excel = null;

            int hasError = 0, hasInsert = 0;
            foreach (DataRow dr in ExcelDataTable.Rows)
            {
                if (!MyUtility.Check.Empty(dr["FactoryID"]) && !MyUtility.Check.Empty(dr["ID"]))
                {
                    //DataRow[] findData = MFactory.Select(string.Format("ID = {0}", MyUtility.Convert.GetString(dr["FactoryID"])));

                    DataRow findData = MFactory.Rows.Find(MyUtility.Convert.GetString(dr["FactoryID"]));
                    if (findData != null)
                    {
                        DataRow newRow = UpdateData.NewRow();
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
                        newRow["EditName"] = "";
                        newRow["EditDate"] = DBNull.Value;
                        UpdateData.Rows.Add(newRow);

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

            if (!MyUtility.Tool.CursorUpdateTable(UpdateData, "Employee", "Production"))
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
                if (exportExcel == null) return;
                Microsoft.Office.Interop.Excel.Worksheet worksheet1 = exportExcel.ActiveWorkbook.Worksheets[1];

                object[,] objArray = new object[1, 7];
                intRowsStart = 2;
                int rownum = 0;
                for (int i = 0; i < ExcelDataTable.Rows.Count; i++)
                {
                    DataRow dr = ExcelDataTable.Rows[i];
                    rownum = intRowsStart + i;
                    objArray[0, 0] = dr["FactoryID"];
                    objArray[0, 1] = dr["ID"];
                    objArray[0, 2] = dr["Name"];
                    objArray[0, 3] = dr["Skill"];
                    objArray[0, 4] = dr["OnBoardDate"];
                    objArray[0, 5] = dr["ResignationDate"];
                    objArray[0, 6] = dr["ErrorMsg"];

                    worksheet1.Range[String.Format("A{0}:G{0}", rownum)].Value2 = objArray;
                }
                exportExcel.Visible = true;
            }
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            textBox3.BackColor = Color.White;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.textBox1.ReadOnly = true;
            textBox3.BackColor = Color.White;
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["FactoryID"]))
            {
                MyUtility.Msg.WarningBox("< Factory > can not be empty!");
                txtmfactory1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("< Employee# > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Name"]))
            {
                MyUtility.Msg.WarningBox("< Nick Name > can not be empty!");
                this.textBox2.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["OnBoardDate"]))
            {
                MyUtility.Msg.WarningBox("< Hired on > can not be empty!");
                this.dateBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["SewingLineID"]))
            {
                MyUtility.Msg.WarningBox("< Line > can not be empty!");
                this.txtsewingline1.Focus();
                return false;
            }
            textBox3.BackColor = displayBox2.BackColor;
            return base.ClickSaveBefore();
        }

        protected override bool ClickPrint()
        {
            DataTable browseData = (DataTable)gridbs.DataSource;
            if (browseData == null || browseData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return false;
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\IE_P08.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
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

                worksheet.Range[String.Format("A{0}:G{0}", rownum)].Value2 = objArray;
            }
            excel.Visible = true;
            return base.ClickPrint();
        }
        
        protected override void ClickUndo()
        {
            base.ClickUndo();
            textBox3.BackColor = displayBox2.BackColor;
        }

        private void textBox3_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                DataTable MachineGroup;
                Ict.DualResult returnResule = DBProxy.Current.Select("Machine", "select ID,Description from MachineGroup WITH (NOLOCK)	where Junk = 0 order by ID", out MachineGroup);
                Sci.Win.Tools.SelectItem2 item = new Sci.Win.Tools.SelectItem2(MachineGroup, "ID,Description", "Group ID,Description", "2,35", textBox3.Text);

                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel) { return; }

                string returnData = "";
                IList<DataRow> gridData = item.GetSelecteds();
                foreach (DataRow currentRecord in gridData)
                {
                    returnData = returnData + currentRecord["ID"].ToString() + ",";
                }
                this.textBox3.Text = returnData;
            }
        }
    }
}
