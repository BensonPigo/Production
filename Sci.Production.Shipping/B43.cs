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
    public partial class B43 : Sci.Win.Tems.Input6
    {
        public B43(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(@"select *,0 as WrongUnit from VNContract_Detail where ID = '{0}' order by CONVERT(int,SUBSTRING(NLCode,3,3))", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("HSCode", header: "HS Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("NLCode", header: "NL Code", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Numeric("Qty", header: "Stock Qty", decimal_places: 3, width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Waste", header: "Waste", decimal_places: 3)
                .Numeric("Price", header: "Unit Price", decimal_places: 3, iseditingreadonly: true)
                .CheckBox("LocalPurchase", header: "Buy in VN", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .CheckBox("NecessaryItem", header: "Cons. Necessary item", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (!EditMode)
            {
                if (MyUtility.Convert.GetString(CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
                {
                    button2.Enabled = true;
                    this.lab_status.Text = "Confirmed";
                }
                else
                {
                    button2.Enabled = false;
                    this.lab_status.Text = "New";
                }
            }
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Status"] = "New";
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();

            dateBox1.ReadOnly = true;
            dateBox2.ReadOnly = true;
            textBox1.ReadOnly = true;
            numericBox1.ReadOnly = true;
        }

        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This contract already confirmed, can't edit!!");
                return false;
            }
            return base.ClickEditBefore();
        }

        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This contract already confirmed, can't delete!!");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("Contract No. can't empty!!");
                textBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["StartDate"]))
            {
                MyUtility.Msg.WarningBox("Start Date can't empty!!");
                dateBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["EndDate"]))
            {
                MyUtility.Msg.WarningBox("End Date can't empty!!");
                dateBox2.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["TotalQty"]))
            {
                MyUtility.Msg.WarningBox("Grand Total Q'ty can't empty!!");
                numericBox1.Focus();
                return false;
            }
            #endregion

            #region 檢查日期正確性
            //Start Date：輸入的日期年份一定要跟建檔當天同一年
            if (Convert.ToDateTime(dateBox1.Value).Year != DateTime.Today.Year)
            {
                MyUtility.Msg.WarningBox("Pls double check the start date!!");
                dateBox1.Focus();
                return false;
            }

            //End Date：輸入的日期年份一定要是建檔當天的隔年
            if (Convert.ToDateTime(dateBox2.Value).Year != DateTime.Today.Year + 1)
            {
                MyUtility.Msg.WarningBox("Pls double check the end date!!");
                dateBox2.Focus();
                return false;
            }
            #endregion

            #region 檢查表身Unit是否有輸入錯誤
            StringBuilder wrongUnit = new StringBuilder();
            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted && MyUtility.Convert.GetString(dr["WrongUnit"]) == "1")
                {
                    wrongUnit.Append(string.Format("NL Code: {0}, Unit: {1}\r\n", MyUtility.Convert.GetString(dr["NLCode"]), MyUtility.Convert.GetString(dr["UnitID"])));
                }
            }
            if (!MyUtility.Check.Empty(wrongUnit.ToString()))
            {
                MyUtility.Msg.WarningBox(string.Format("Below data is 'Unit' not correct.\r\n{0}", wrongUnit.ToString()));
                return false;
            }
            #endregion

            return base.ClickSaveBefore();
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            string updateCmds = string.Format("update VNContract set EditDate = GETDATE(), EditName = '{0}', Status = 'Confirmed' where ID = '{1}'",
                Sci.Env.User.UserID,MyUtility.Convert.GetString(CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Confirm fail!!\r\n" + result.ToString());
                return;
            }
            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Unconfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            string updateCmds = string.Format("update VNContract set EditDate = GETDATE(), EditName = '{0}', Status = 'New' where ID = '{1}'",
                            Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Unconfirm fail!!\r\n" + result.ToString());
                return;
            }

            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Import Data
        private void button1_Click(object sender, EventArgs e)
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

            UpdateData = ((DataTable)gridbs.DataSource).Clone();

            MyUtility.Msg.WaitWindows("Starting EXCEL...");
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
                newRow["AddName"] = Sci.Env.User.UserID;
                newRow["AddDate"] = DateTime.Now;
                ExcelDataTable.Rows.Add(newRow);
            }

            excel.Workbooks.Close();
            excel.Quit();
            excel = null;

            //刪除表身Grid資料
            foreach (DataRow dr in DetailDatas)
            {
                dr.Delete();
            }
            
            //將Excel寫入表身Grid
            foreach (DataRow dr in ExcelDataTable.Rows)
            {
                ((DataTable)detailgridbs.DataSource).ImportRow(dr);
            }
            MyUtility.Msg.WaitClear();
            MyUtility.Msg.InfoBox("Import Complete!!");
        }

        //Add New NL Code
        private void button2_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.B43_AddNLCode callNextForm = new Sci.Production.Shipping.B43_AddNLCode(CurrentMaintain);
            DialogResult result = callNextForm.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                RenewData();
            }
        }
    }
}
