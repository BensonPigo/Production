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
    public partial class P42 : Sci.Win.Tems.Input6
    {
        Ict.Win.DataGridViewGeneratorTextColumnSettings nlcode = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        public P42(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (EditMode)
            {
                if (MyUtility.Convert.GetString(CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
                {
                    detailgrid.IsEditingReadOnly = true;
                }
                else
                {
                    detailgrid.IsEditingReadOnly = false;
                }
                detailgrid.EnsureStyle();
            }
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "1=0" : "v.ID = "+MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(@"select vd.*,c.HSCode,c.UnitID
from VNContractQtyAdjust v
inner join VNContractQtyAdjust_Detail vd on v.ID = vd.ID
left join VNContract_Detail c on c.ID = v.VNContractID and c.NLCode = vd.NLCode
where {0}
order by CONVERT(int,SUBSTRING(vd.NLCode,3,3))", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            #region NL Code的Validating
            nlcode.CellValidating += (s, e) =>
                {
                    if (this.EditMode)
                    {
                        DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                        if (!MyUtility.Check.Empty(e.FormattedValue))
                        {
                            if (MyUtility.Convert.GetString(dr["nlcode"]) != MyUtility.Convert.GetString(e.FormattedValue))
                            {
                                DataRow seekData;
                                if (!MyUtility.Check.Seek(string.Format("select HSCode,UnitID from VNContract_Detail where ID = '{0}' and NLCode = '{1}'",
                                    MyUtility.Convert.GetString(CurrentMaintain["VNContractID"]), MyUtility.Convert.GetString(e.FormattedValue)), out seekData))
                                {
                                    MyUtility.Msg.WarningBox("NL Code not found!!");
                                    dr["HSCode"] = "";
                                    dr["NLCode"] = "";
                                    dr["Qty"] = 0;
                                    dr["UnitID"] = "";
                                    e.Cancel = true;
                                    return;
                                }
                                else
                                {
                                    dr["HSCode"] = seekData["HSCode"];
                                    dr["NLCode"] = e.FormattedValue;
                                    dr["UnitID"] = seekData["UnitID"];
                                }
                            }
                        }
                        else
                        {
                            dr["HSCode"] = "";
                            dr["NLCode"] = "";
                            dr["Qty"] = 0;
                            dr["UnitID"] = "";
                        }
                    }
                };
            #endregion

            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("HSCode", header: "HS Code", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("NLCode", header: "NL Code", width: Widths.AnsiChars(7),settings:nlcode)
                .Numeric("Qty", header: "Stock Qty", decimal_places: 3, width: Widths.AnsiChars(15))
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true);
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["CDate"] = DateTime.Today;
            CurrentMaintain["VNContractID"] = MyUtility.GetValue.Lookup("select top 1 ID from VNContract where StartDate <= GETDATE() and EndDate >= GETDATE() and Status = 'Confirmed'");
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();

            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
            {
                dateBox1.ReadOnly = true;
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                button1.Enabled = false;
                gridicon.Append.Enabled = false;
                gridicon.Insert.Enabled = false;
                gridicon.Remove.Enabled = false;
            }
        }

        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record already confirmed, can't delete!!");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentMaintain["CDate"]))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                dateBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["VNContractID"]))
            {
                MyUtility.Msg.WarningBox("Contract no. can't empty!!");
                textBox1.Focus();
                return false;
            }
            #endregion

            #region 刪除表身Qty為0的資料
            int recCount = 0;
            foreach (DataRow dr in DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["Qty"]))
                {
                    dr.Delete();
                    continue;
                }
                recCount++;
            }
            if (recCount == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty!!");
                return false;
            }
            #endregion

            return base.ClickSaveBefore();
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            string updateCmds = string.Format("update VNContractQtyAdjust set EditDate = GETDATE(), EditName = '{0}', Status = 'Confirmed' where ID = {1}",
                Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));

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

            string updateCmds = string.Format("update VNContractQtyAdjust set EditDate = GETDATE(), EditName = '{0}', Status = 'New' where ID = {1}",
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

        //Import from excel
        private void button1_Click(object sender, EventArgs e)
        {
            string excelFile = MyUtility.File.GetFile("Excel files (*.xlsx)|*.xlsx");
            if (MyUtility.Check.Empty(excelFile))
            {
                return;
            }

            //刪除表身Grid資料
            foreach (DataRow dr in DetailDatas)
            {
                dr.Delete();
            }

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(excelFile);
            if (excel == null) return;


            DataRow seekData;
            StringBuilder errNLCode = new StringBuilder();

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

                range = worksheet.Range[String.Format("A{0}:D{0}", intRowsRead)];
                objCellArray = range.Value;

                if (!MyUtility.Check.Seek(string.Format("select HSCode,UnitID from VNContract_Detail where ID = '{0}' and NLCode = '{1}'",
                    MyUtility.Convert.GetString(CurrentMaintain["VNContractID"]), MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C"))), out seekData))
                {
                    errNLCode.Append(string.Format("NL Code: {0}\r\n",MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C"))));
                    continue;
                }
                else
                {
                    DataRow newRow = ((DataTable)detailgridbs.DataSource).NewRow();
                    newRow["NLCode"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");
                    newRow["Qty"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "N");
                    newRow["HSCode"] = seekData["HSCode"];
                    newRow["UnitID"] = seekData["UnitID"];
                    ((DataTable)detailgridbs.DataSource).Rows.Add(newRow);
                }
            }

            excel.Workbooks.Close();
            excel.Quit();
            excel = null;
            MyUtility.Msg.WaitClear();
            if (!MyUtility.Check.Empty(errNLCode.ToString()))
            {
                MyUtility.Msg.WarningBox(string.Format("Below NL Code is not in B43. Customs Contract - Contract No.: {0}\r\n{1}", MyUtility.Convert.GetString(CurrentMaintain["VNContractID"]), errNLCode.ToString()));
            }
            MyUtility.Msg.InfoBox("Import Complete!!");
        }
    }
}
