using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using System.Data.SqlClient;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class P04 : Win.Tems.QueryForm
    {
        private DataTable dataTable;
        private string SPNo;

        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.EditMode = true;
            this.InitializeComponent();
        }

        // Form to Form W/H.P01
        public P04(string P01SPNo, ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.SPNo = P01SPNo;
            this.txtSPNo.Text = this.SPNo.Trim();
            this.Event_Query();
        }

        // PPIC_P01 Called
        public static void Call(string PPIC_SPNo)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is P04)
                {
                    form.Activate();
                    P04 activateForm = (P04)form;
                    activateForm.SetTxtSPNo(PPIC_SPNo);
                    activateForm.Event_Query();
                    return;
                }
            }

            ToolStripMenuItem p04MenuItem = null;
            foreach (ToolStripMenuItem toolMenuItem in Env.App.MainMenuStrip.Items)
            {
                if (toolMenuItem.Text.EqualString("Warehouse"))
                {
                    foreach (var subMenuItem in toolMenuItem.DropDown.Items)
                    {
                        if (subMenuItem.GetType().Equals(typeof(ToolStripMenuItem)))
                        {
                            if (((ToolStripMenuItem)subMenuItem).Text.EqualString("P04. Material Status (Local)"))
                            {
                                p04MenuItem = (ToolStripMenuItem)subMenuItem;
                                break;
                            }
                        }
                    }
                }
            }

            P04 callform = new P04(PPIC_SPNo, p04MenuItem);
            callform.Show();
        }

        // 隨著 P01上下筆SP#切換資料
        public void P04Data(string P01SPNo)
        {
            this.EditMode = true;
            this.SPNo = P01SPNo;
            this.txtSPNo.Text = this.SPNo.Trim();
            this.Event_Query();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region Balance 開窗 P04_LocalTransaction
            DataGridViewGeneratorNumericColumnSettings setBalance = new DataGridViewGeneratorNumericColumnSettings();
            setBalance.CellMouseDoubleClick += (s, e) =>
            {
                var dataRow = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (dataRow != null)
                {
                    var form = new P04_LocalTransaction(dataRow, "P04");
                    form.Show(this);
                }
            };
            #endregion
            #region Scrap Qty 開窗 P04_ScrapQty
            DataGridViewGeneratorNumericColumnSettings setScrapQty = new DataGridViewGeneratorNumericColumnSettings();
            setScrapQty.CellMouseDoubleClick += (s, e) =>
            {
                var dataRow = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (dataRow != null)
                {
                    // var form = new Sci.Production.Warehouse.P04_ScrapQty(string Poid, string Refno, string Color);
                    var form = new P04_ScrapQty(dataRow["sp"].ToString(), dataRow["refno"].ToString(), dataRow["threadColor"].ToString());
                    form.Show(this);
                }
            };
            #endregion
            #region Set Grid
            this.Helper.Controls.Grid.Generator(this.gridMaterialStatus)
                .Text("sp", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("unit", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("refno", header: "Refno", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .EditText("desc", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(30))
                .Text("supp", header: "Supplier", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("threadColor", header: "Thread Color", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Numeric("inQty", header: "InQty", decimal_places: 2, integer_places: 10, iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("outQty", header: "OutQty", decimal_places: 2, integer_places: 10, iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("adjustQty", header: "Adjust Qty", decimal_places: 2, integer_places: 10, iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("balance", header: "Balance", decimal_places: 2, integer_places: 10, iseditingreadonly: true, width: Widths.AnsiChars(6), settings: setBalance)
                .EditText("Alocation", header: "Bulk Location", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Numeric("ScrapQty", header: "Scrap Qty", decimal_places: 2, integer_places: 10, iseditingreadonly: true, width: Widths.AnsiChars(6), settings: setScrapQty);
            #endregion
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Event_Query();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.txtSPNo.Focused)
            {
                switch (keyData)
                {
                    case Keys.Enter:
                        this.Event_Query();
                        break;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void Event_Query()
        {
            #region check SP#
            if (this.txtSPNo.Text.Empty())
            {
                MyUtility.Msg.WarningBox("SP# can't be empty. Please fill SP# first!");
                this.txtSPNo.Focus();
                return;
            }
            #endregion
            #region SQL Command & SqlParameter
            string sql = string.Format(@"
select OrderID,Refno,ThreadColorID,UnitID
into #tmp
from LocalPO_Detail with(nolock)
where OrderID like @spno
Group by OrderID,RefNo,ThreadColorID,UnitID

select [sp] = a.OrderID
        , [unit] = a.UnitID
        , [refno] = a.Refno
        , [desc] = b.Description
        , [supp] = c.ID + '-' + c.Abb
        , [threadColor] = a.ThreadColorID
        , [inQty] = iif (l.InQty = 0, '', Convert (varchar, l.InQty))
        , [outQty] = iif (l.OutQty = 0, '', Convert (varchar, l.OutQty))
        , [adjustQty] = iif (l.AdjustQty = 0, '', Convert (varchar, l.AdjustQty))
        , [Balance] = iif (InQty - OutQty + AdjustQty = 0, '', Convert (varchar, InQty - OutQty + AdjustQty))
        , [ALocation] = l.ALocation
        , [ScrapQty] = l.LobQty
from #tmp a
left join LocalInventory l on a.OrderId = l.OrderID and a.Refno = l.Refno and a.ThreadColorID = l.ThreadColorID
left join LocalItem b on a.Refno=b.RefNo
left join LocalSupp c on b.LocalSuppid=c.ID
order by a.OrderID
drop table #tmp
");

            string spno = this.txtSPNo.Text.TrimEnd() + "%";
            List<SqlParameter> sqlPar = new List<SqlParameter>();
            sqlPar.Add(new SqlParameter("@spno", spno));
            #endregion

            this.ShowWaitMessage("Data Loading....");
            #region SQL Data Loading....

            DualResult result;

            if (result = DBProxy.Current.Select(null, sql, sqlPar, out this.dataTable))
            {
                if (this.dataTable == null || this.dataTable.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = this.dataTable;
            }
            else
            {
                this.ShowErr(sql, result);
            }
            #endregion
            this.HideWaitMessage();
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            if (this.dataTable != null && this.dataTable.Rows.Count > 0)
            {
                this.ShowWaitMessage("Excel Processing...");

                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_P04.xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.dataTable, string.Empty, "Warehouse_P04.xltx", 1, showExcel: false, showSaveMsg: true, excelApp: objApp);
                Excel.Worksheet worksheet = objApp.Sheets[1];
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Warehouse_P04");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
                #endregion
                this.HideWaitMessage();
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnNewSearch_Click(object sender, EventArgs e)
        {
            this.txtSPNo.ResetText();
            this.txtSPNo.Select();
        }

        public void SetTxtSPNo(string spNo)
        {
            this.txtSPNo.Text = spNo;
        }
    }
}
