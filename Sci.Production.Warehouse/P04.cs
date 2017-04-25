using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using System.Data.SqlClient;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class P04 : Sci.Win.Tems.QueryForm
    {
        DataTable dataTable;

        public P04(ToolStripMenuItem menuitem)
            :base(menuitem)
        {
            this.EditMode = true;
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region Balance 開窗 P04_LocalTransaction
            Ict.Win.DataGridViewGeneratorNumericColumnSettings setBalance = new DataGridViewGeneratorNumericColumnSettings();
            setBalance.CellMouseDoubleClick += (s, e) =>
            {
                var dataRow = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (dataRow != null)
                {
                    var form = new Sci.Production.Warehouse.P04_LocalTransaction(dataRow);
                    form.Show(this);
                }
            };
            #endregion 
            #region Set Grid
            Helper.Controls.Grid.Generator(this.gridMaterialStatus)
                .Text("sp", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("unit", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("refno", header: "Refno", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .EditText("desc", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(30))
                .Text("supp", header: "Supplier", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("threadColor", header: "Thread Color", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Numeric("inQty", header: "InQty", decimal_places: 2, integer_places: 10, iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("outQty", header: "OutQty", decimal_places: 2, integer_places: 10, iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Numeric("balance", header: "Balance", decimal_places: 2, integer_places: 10, iseditingreadonly: true, width: Widths.AnsiChars(6), settings: setBalance);
            #endregion
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            event_Query();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (txtSPNo.Focus())
            {
                switch (keyData)
                {
                    case Keys.Enter:
                        event_Query();
                        break;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void event_Query()
        {
            #region check SP#
            if (txtSPNo.Text.Empty())
            {
                MyUtility.Msg.WarningBox("SP# can't be empty. Please fill SP# first!");
                txtSPNo.Focus();
                return;
            }
            #endregion 
            #region SQL Command & SqlParameter 
            string sql = string.Format(@"
select  [sp] = l.OrderID
        , [unit] = l.UnitID
        , [refno] = l.Refno
        , [desc] = b.Description
        , [supp] = c.ID + '-' + c.Abb
        , [threadColor] = l.ThreadColorID
        , [inQty] = l.InQty
        , [outQty] = l.OutQty
        , [Balance] = InQty - OutQty + AdjustQty
from LocalInventory l
left join LocalItem b on l.Refno=b.RefNo
left join LocalSupp c on b.LocalSuppid=c.ID
where l.OrderID like @spno
");

            string spno = txtSPNo.Text.TrimEnd() + "%";
            List<SqlParameter> sqlPar = new List<SqlParameter>();
            sqlPar.Add(new SqlParameter("@spno", spno));
            #endregion
            
            this.ShowWaitMessage("Data Loading....");
            #region SQL Data Loading....

            Ict.DualResult result;

            if (result = DBProxy.Current.Select(null, sql, sqlPar, out dataTable))
            {
                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found!!");
                }
                listControlBindingSource1.DataSource = dataTable;  
            }
            else
            {
                ShowErr(sql, result);
            }
            #endregion 
            this.HideWaitMessage();
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                this.ShowWaitMessage("Excel Processing...");

                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_P04.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(dataTable, "", "Warehouse_P04.xltx", 1, showExcel: false, showSaveMsg: true, excelApp: objApp);                
                Excel.Worksheet worksheet = objApp.Sheets[1];
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();
                objApp.Visible = true;

                if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
                if (worksheet != null) Marshal.FinalReleaseComObject(worksheet);    //釋放worksheet
                this.HideWaitMessage();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
