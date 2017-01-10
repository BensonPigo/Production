using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class B43_AddNLCode : Sci.Win.Subs.Base
    {
        DataRow masterData;
        Ict.Win.DataGridViewGeneratorTextColumnSettings unit = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        public B43_AddNLCode(DataRow MasterData)
        {
            InitializeComponent();
            masterData = MasterData;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region Unit的Validating
            unit.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetString(dr["UnitID"]) != MyUtility.Convert.GetString(e.FormattedValue))
                    {
                        if (!MyUtility.Check.Seek(MyUtility.Convert.GetString(e.FormattedValue), "Unit", "ID"))
                        {
                            dr["WrongUnit"] = 1;
                        }
                        else
                        {
                            dr["WrongUnit"] = 0;
                        }
                    }
                }
            };
            #endregion
            this.grid1.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(grid1)
                .Text("HSCode", header: "HS Code", width: Widths.AnsiChars(10))
                .Text("NLCode", header: "NL Code", width: Widths.AnsiChars(7))
                .Numeric("Qty", header: "Stock Qty", decimal_places: 3, width: Widths.AnsiChars(15))
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(8), settings: unit)
                .Numeric("Waste", header: "Waste", decimal_places: 3)
                .Numeric("Price", header: "Unit Price", decimal_places: 3)
                .CheckBox("LocalPurchase", header: "Buy in VN", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .CheckBox("NecessaryItem", header: "Cons. Necessary item", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0);

            DataTable gridData;
            string sqlCmd = "select *,0 as WrongUnit from VNContract_Detail where 1=0";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            listControlBindingSource1.DataSource = gridData;
        }

        //Append
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable datas = (DataTable)listControlBindingSource1.DataSource;
            var data = datas.NewRow();
            datas.Rows.Add(data);
            listControlBindingSource1.MoveLast();
        }

        //Delete
        private void button2_Click(object sender, EventArgs e)
        {
            if (listControlBindingSource1.Position != -1)
            {
                listControlBindingSource1.RemoveCurrent();
            }
        }

        //Import from excel
        private void button3_Click(object sender, EventArgs e)
        {
            string excelFile = MyUtility.File.GetFile("Excel files (*.xlsx)|*.xlsx");
            if (MyUtility.Check.Empty(excelFile))
            {
                return;
            }

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(excelFile);
            if (excel == null) return;

            DataTable ExcelDataTable, UpdateData;
            string sqlCmd = "select *,0 as WrongUnit from VNContract_Detail where 1 = 0";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out ExcelDataTable);

            UpdateData = ((DataTable)listControlBindingSource1.DataSource).Clone();

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

                range = worksheet.Range[String.Format("A{0}:H{0}", intRowsRead)];
                objCellArray = range.Value;

                DataRow newRow = ExcelDataTable.NewRow();
                newRow["HSCode"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");
                newRow["NLCode"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C");
                newRow["Qty"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "N");
                newRow["UnitID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C");
                newRow["Waste"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "N");
                newRow["Price"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "N");
                newRow["LocalPurchase"] = MyUtility.Check.Empty(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 7], "C")) ? 0 : 1;
                newRow["NecessaryItem"] = MyUtility.Check.Empty(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 8], "C")) ? 0 : 1;
                if (MyUtility.Check.Seek(MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C")), "Unit", "ID"))
                {
                    newRow["WrongUnit"] = 0;
                }
                else
                {
                    newRow["WrongUnit"] = 1;
                }
                ExcelDataTable.Rows.Add(newRow);
            }

            excel.Workbooks.Close();
            excel.Quit();
            excel = null;

            //刪除Grid資料
            ((DataTable)listControlBindingSource1.DataSource).Clear();

            //將Excel寫入表身Grid
            foreach (DataRow dr in ExcelDataTable.Rows)
            {
                ((DataTable)listControlBindingSource1.DataSource).ImportRow(dr);
            }
            this.HideWaitMessage();
            MyUtility.Msg.InfoBox("Import Complete!!");
        }

        //Save
        private void button4_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            listControlBindingSource1.EndEdit();
            IList<string> insertCmds = new List<string>();
            StringBuilder dupNLCode = new StringBuilder();
            StringBuilder wrongUnit = new StringBuilder();
            foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                insertCmds.Add(string.Format(@"insert into VNContract_Detail (      ID,HSCode,NLCode,Qty,UnitID,Waste,Price,LocalPurchase,NecessaryItem,AddName,AddDate) 
values('{0}','{1}','{2}',{3},'{4}','{5}','{6}','{7}','{8}','{9}',GETDATE());", 
MyUtility.Convert.GetString(masterData["ID"]), MyUtility.Convert.GetString(dr["HSCode"]),
MyUtility.Convert.GetString(dr["NLCode"]),MyUtility.Convert.GetString(dr["Qty"]), 
MyUtility.Convert.GetString(dr["UnitID"]), MyUtility.Convert.GetString(dr["Waste"]),
MyUtility.Convert.GetString(dr["Price"]), MyUtility.Convert.GetString(dr["LocalPurchase"]).ToUpper() == "TRUE" ? "1" : "0",
MyUtility.Convert.GetString(dr["NecessaryItem"]).ToUpper() == "TRUE" ? "1" : "0", Sci.Env.User.UserID));
                if (MyUtility.Check.Seek(string.Format("select ID from VNContract_Detail where ID = '{0}' and NLCode = '{1}'", MyUtility.Convert.GetString(masterData["ID"]), MyUtility.Convert.GetString(dr["NLCode"]))))
                {
                    dupNLCode.Append(string.Format("NL Code: {0}\r\n", MyUtility.Convert.GetString(dr["NLCode"])));
                }
                if (MyUtility.Convert.GetString(dr["WrongUnit"]) == "1")
                {
                    wrongUnit.Append(string.Format("NL Code: {0}, Unit: {1}\r\n", MyUtility.Convert.GetString(dr["NLCode"]), MyUtility.Convert.GetString(dr["UnitID"])));
                }
            }

            if (!MyUtility.Check.Empty(dupNLCode.ToString()) || !MyUtility.Check.Empty(wrongUnit.ToString()))
            {
                MyUtility.Msg.WarningBox(string.Format("{0}{1}",
                    !MyUtility.Check.Empty(dupNLCode.ToString()) ? "Below 'NL Code' already exist, please delete below 'NL Code' then save again.\r\n" + dupNLCode.ToString()+"\r\n\r\n" : "",
                    !MyUtility.Check.Empty(wrongUnit.ToString()) ? "Below data is 'Unit' not correct.\r\n" + wrongUnit.ToString() : ""));
                return;
            }
            DualResult result = DBProxy.Current.Executes(null, insertCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Save Fail, please try again.\r\n"+result.ToString());
                return;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
