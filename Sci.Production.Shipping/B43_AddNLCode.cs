using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B43_AddNLCode
    /// </summary>
    public partial class B43_AddNLCode : Win.Subs.Base
    {
        private DataRow masterData;
        private DataGridViewGeneratorTextColumnSettings unit = new DataGridViewGeneratorTextColumnSettings();

        /// <summary>
        /// B43_AddNLCode
        /// </summary>
        /// <param name="masterData">MasterData</param>
        public B43_AddNLCode(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region Unit的Validating
            this.unit.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.gridAddNewNLCode.GetDataRow<DataRow>(e.RowIndex);
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
            this.gridAddNewNLCode.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridAddNewNLCode)
                .Text("HSCode", header: "HS Code", width: Widths.AnsiChars(10))
                .Text("NLCode", header: "Customs Code", width: Widths.AnsiChars(12))
                .Numeric("Qty", header: "Stock Qty", decimal_places: 3, width: Widths.AnsiChars(15))
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(8), settings: this.unit)
                .Numeric("WasteLower", header: "Waste Lower", decimal_places: 3)
                .Numeric("WasteUpper", header: "Waste Upper", decimal_places: 3)
                .Numeric("Price", header: "Unit Price", decimal_places: 3)
                .CheckBox("LocalPurchase", header: "Buy in VN", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .CheckBox("NecessaryItem", header: "Cons. Necessary item", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0);

            DataTable gridData;
            string sqlCmd = "select *,0 as WrongUnit from VNContract_Detail WITH (NOLOCK) where 1=0";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            this.listControlBindingSource1.DataSource = gridData;
        }

        // Append
        private void BtnAppend_Click(object sender, EventArgs e)
        {
            DataTable datas = (DataTable)this.listControlBindingSource1.DataSource;
            var data = datas.NewRow();
            datas.Rows.Add(data);
            this.listControlBindingSource1.MoveLast();
        }

        // Delete
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1.Position != -1)
            {
                this.listControlBindingSource1.RemoveCurrent();
            }
        }

        // Import from excel
        private void BtnImportFromExcel_Click(object sender, EventArgs e)
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

            DataTable excelDataTable, updateData;
            string sqlCmd = "select *,0 as WrongUnit from VNContract_Detail WITH (NOLOCK) where 1 = 0";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out excelDataTable);

            updateData = ((DataTable)this.listControlBindingSource1.DataSource).Clone();

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

                range = worksheet.Range[string.Format("A{0}:I{0}", intRowsRead)];
                objCellArray = range.Value;

                DataRow newRow = excelDataTable.NewRow();
                newRow["HSCode"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");
                newRow["NLCode"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C");
                newRow["Qty"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "N");
                newRow["UnitID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C");
                newRow["WasteLower"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "N");
                newRow["WasteUpper"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "N");
                newRow["Price"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 7], "N");
                newRow["LocalPurchase"] = MyUtility.Check.Empty(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 8], "C")) ? 0 : 1;
                newRow["NecessaryItem"] = MyUtility.Check.Empty(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 9], "C")) ? 0 : 1;
                if (MyUtility.Check.Seek(MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C")), "Unit", "ID"))
                {
                    newRow["WrongUnit"] = 0;
                }
                else
                {
                    newRow["WrongUnit"] = 1;
                }

                excelDataTable.Rows.Add(newRow);
            }

            excel.Workbooks.Close();
            excel.Quit();
            excel = null;

            // 刪除Grid資料
            ((DataTable)this.listControlBindingSource1.DataSource).Clear();

            // 將Excel寫入表身Grid
            foreach (DataRow dr in excelDataTable.Rows)
            {
                ((DataTable)this.listControlBindingSource1.DataSource).ImportRow(dr);
            }

            this.HideWaitMessage();
            MyUtility.Msg.InfoBox("Import Complete!!");
        }

        // Save
        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.gridAddNewNLCode.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            IList<string> insertCmds = new List<string>();
            StringBuilder dupNLCode = new StringBuilder();
            StringBuilder wrongUnit = new StringBuilder();
            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
            {
                insertCmds.Add(string.Format(
                    @"insert into VNContract_Detail (      ID,HSCode,NLCode,Qty,UnitID,WasteLower,WasteUpper,Price,LocalPurchase,NecessaryItem,AddName,AddDate) 
values('{0}','{1}','{2}',{3},'{4}','{5}','{6}','{7}','{8}','{9}','{10}',GETDATE());",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    MyUtility.Convert.GetString(dr["HSCode"]),
                    MyUtility.Convert.GetString(dr["NLCode"]),
                    MyUtility.Convert.GetString(dr["Qty"]),
                    MyUtility.Convert.GetString(dr["UnitID"]),
                    MyUtility.Convert.GetString(dr["WasteLower"]),
                    MyUtility.Convert.GetString(dr["WasteUpper"]),
                    MyUtility.Convert.GetString(dr["Price"]),
                    MyUtility.Convert.GetString(dr["LocalPurchase"]).ToUpper() == "TRUE" ? "1" : "0",
                    MyUtility.Convert.GetString(dr["NecessaryItem"]).ToUpper() == "TRUE" ? "1" : "0",
                    Sci.Env.User.UserID));

                if (MyUtility.Check.Seek(string.Format("select ID from VNContract_Detail WITH (NOLOCK) where ID = '{0}' and NLCode = '{1}'", MyUtility.Convert.GetString(this.masterData["ID"]), MyUtility.Convert.GetString(dr["NLCode"]))))
                {
                    dupNLCode.Append(string.Format("Customs Code: {0}\r\n", MyUtility.Convert.GetString(dr["NLCode"])));
                }

                if (MyUtility.Convert.GetString(dr["WrongUnit"]) == "1")
                {
                    wrongUnit.Append(string.Format("Customs Code: {0}, Unit: {1}\r\n", MyUtility.Convert.GetString(dr["NLCode"]), MyUtility.Convert.GetString(dr["UnitID"])));
                }
            }

            if (!MyUtility.Check.Empty(dupNLCode.ToString()) || !MyUtility.Check.Empty(wrongUnit.ToString()))
            {
                MyUtility.Msg.WarningBox(string.Format(
                    "{0}{1}",
                    !MyUtility.Check.Empty(dupNLCode.ToString()) ? "Below 'Customs Code' already exist, please delete below 'Customs Code' then save again.\r\n" + dupNLCode.ToString() + "\r\n\r\n" : string.Empty,
                    !MyUtility.Check.Empty(wrongUnit.ToString()) ? "Below data is 'Unit' not correct.\r\n" + wrongUnit.ToString() : string.Empty));
                return;
            }

            DualResult result = DBProxy.Current.Executes(null, insertCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Save Fail, please try again.\r\n" + result.ToString());
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
