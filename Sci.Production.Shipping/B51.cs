using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B51
    /// </summary>
    public partial class B51 : Win.Tems.Input6
    {
        private DataTable WrongUnitID;

        /// <summary>
        /// B51
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B51(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(
                @"select kd.*,kd.Qty*kd.Price as TotalPrice,kh.GoodsDescription,kh.Category
from KHContract_Detail kd WITH (NOLOCK) 
left join KHGoodsHSCode kh WITH (NOLOCK) on kd.NLCode = kh.NLCode
where kd.ID = '{0}' order by CONVERT(INT,SUBSTRING(Seq,PATINDEX('%-%',Seq)+1,len(Seq)))", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("NLCode", header: "No.", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("GoodsDescription", header: "Description of Goods", width: Widths.AnsiChars(50), iseditingreadonly: true)
                .Text("Category", header: "Category", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Waste", decimal_places: 2, iseditingreadonly: true)
                .Numeric("Price", header: "Unit Price", decimal_places: 2, iseditingreadonly: true)
                .Numeric("TotalPrice", header: "Total Price", decimal_places: 2, iseditingreadonly: true);
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This CDC already confirmed, can't edit!!");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This CDC already confirmed, can't delete!!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["StartDate"]))
            {
                this.dateStartDate.Focus();
                MyUtility.Msg.WarningBox("Start Date can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["EndDate"]))
            {
                this.dateEndDate.Focus();
                MyUtility.Msg.WarningBox("End Date can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.txtCDCNo.Focus();
                MyUtility.Msg.WarningBox("CDC No. can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
            {
                this.txtFactory.Focus();
                MyUtility.Msg.WarningBox("Factory can't empty!!");
                return false;
            }
            #endregion

            #region 檢查日期正確性

            // End Date：輸入日期一定不能小於Start Date
            if (this.dateEndDate.Value < this.dateStartDate.Value)
            {
                this.dateEndDate.Focus();
                MyUtility.Msg.WarningBox("Pls double check the end date!!");
                return false;
            }
            #endregion

            #region 如果有做Import Data，檢查匯入的的Unit是否正確
            if (this.WrongUnitID != null && this.WrongUnitID.Rows.Count > 0)
            {
                StringBuilder wrongunit = new StringBuilder();
                foreach (DataRow dr in this.WrongUnitID.Rows)
                {
                    if (wrongunit.ToString().IndexOf(MyUtility.Convert.GetString(dr["UnitID"])) <= 0)
                    {
                        wrongunit.Append(string.Format("Unit: {0}\r\n", MyUtility.Convert.GetString(dr["UnitID"])));
                    }
                }

                MyUtility.Msg.WarningBox(string.Format("Below data is 'Unit' not correct.\r\n{0}", wrongunit.ToString()));
                return false;
            }
            #endregion

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            string updateCmds = string.Format(
                "update KHContract set EditDate = GETDATE(), EditName = '{0}', Status = 'Confirmed' where ID = '{1}'",
                Sci.Env.User.UserID,
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Confirm fail!!\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            string updateCmds = string.Format(
                "update KHContract set EditDate = GETDATE(), EditName = '{0}', Status = 'New' where ID = '{1}'",
                Sci.Env.User.UserID,
                MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Unconfirm fail!!\r\n" + result.ToString());
                return;
            }
        }

        // Import Data
        private void BtnImportData_Click(object sender, EventArgs e)
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
            string sqlCmd = "select *,0.0 as TotalPrice,'' as GoodsDescription,'' as Category from KHContract_Detail WITH (NOLOCK) where 1 = 0";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out excelDataTable);

            sqlCmd = "select UnitID from KHContract_Detail WITH (NOLOCK) where 1 = 0";
            result = DBProxy.Current.Select(null, sqlCmd, out this.WrongUnitID);

            updateData = ((DataTable)this.gridbs.DataSource).Clone();

            this.ShowWaitMessage("Starting EXCEL...");
            excel.Visible = false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            int intRowsCount = worksheet.UsedRange.Rows.Count;
            int intColumnsCount = worksheet.UsedRange.Columns.Count;
            int intRowsStart = 2;
            int intRowsRead = intRowsStart - 1;

            Microsoft.Office.Interop.Excel.Range range;
            object[,] objCellArray;
            DataRow seekData;
            StringBuilder emptyNLCode = new StringBuilder();

            while (intRowsRead < intRowsCount)
            {
                intRowsRead++;

                range = worksheet.Range[string.Format("A{0}:F{0}", intRowsRead)];
                objCellArray = range.Value;
                if (MyUtility.Check.Seek(string.Format("select * from KHGoodsHSCode WITH (NOLOCK) where GoodsDescription = '{0}'", MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C")).ToUpper()), out seekData))
                {
                    DataRow newRow = excelDataTable.NewRow();
                    newRow["Seq"] = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C")) + "-" + MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C"));
                    newRow["GoodsDescription"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C");
                    newRow["NLCode"] = seekData["NLCode"];
                    newRow["Category"] = seekData["Category"];
                    newRow["UnitID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C");
                    newRow["Qty"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "N");
                    newRow["Price"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "N");
                    newRow["TotalPrice"] = MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "N")) * MyUtility.Convert.GetDecimal(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "N"));
                    if (!MyUtility.Check.Seek(MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C")), "Unit", "ID"))
                    {
                        DataRow unitRow = this.WrongUnitID.NewRow();
                        unitRow["UnitID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C");
                        this.WrongUnitID.Rows.Add(unitRow);
                    }

                    excelDataTable.Rows.Add(newRow);
                }
                else
                {
                    emptyNLCode.Append(string.Format("{0} {1}\r\n", MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C")) + "-" + MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C")), MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C"))));
                }
            }

            excel.Workbooks.Close();
            excel.Quit();
            excel = null;

            // 刪除表身Grid資料
            foreach (DataRow dr in this.DetailDatas)
            {
                dr.Delete();
            }

            // 將Excel寫入表身Grid
            foreach (DataRow dr in excelDataTable.Rows)
            {
                ((DataTable)this.detailgridbs.DataSource).ImportRow(dr);
            }

            this.HideWaitMessage();
            if (MyUtility.Check.Empty(emptyNLCode.ToString()))
            {
                MyUtility.Msg.InfoBox("Import Complete!!");
            }
            else
            {
                MyUtility.Msg.WarningBox(string.Format("Below data isn't created in 'B50. CDC Goods's Description Basic Data:\r\n{0}'", emptyNLCode.ToString()));
            }
        }
    }
}
