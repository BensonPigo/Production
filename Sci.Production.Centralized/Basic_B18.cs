using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Centralized
{
    public partial class Basic_B18 : Sci.Win.Tems.Input1
    {
        private string oldID = string.Empty;

        public Basic_B18(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override void OnDetailEntered()
        {
            this.oldID = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);
            base.OnDetailEntered();
            string sqlCmd = $"SELECT Name FROM FinanceTW.dbo.AccountNo WHERE ID='{this.CurrentMaintain["ID"]}' ";
            this.disAccountNoname.Text = MyUtility.GetValue.Lookup(sqlCmd, "ProductionTPE");
        }

        private void TxtAccountNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string cmd = "SELECT ID ,Name FROM FinanceTW.dbo.AccountNo WHERE Junk = 0 ORDER BY ID";
            DataTable dt;
            DBProxy.Current.Select("ProductionTPE", cmd, out dt);

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(dt, "ID,Name", "8,30", this.txtAccountNo.Text);

            DialogResult result = item.ShowDialog();

            if (result == DialogResult.Cancel)
            {
                return;
            }

            List<DataRow> dr = item.GetSelecteds().ToList();

            this.CurrentMaintain["ID"] = MyUtility.Convert.GetString(dr[0]["ID"]);
            this.disAccountNoname.Text = MyUtility.Convert.GetString(dr[0]["Name"]);

        }

        private void TxtAccountNo_Validating(object sender, CancelEventArgs e)
        {
            if (this.oldID != this.txtAccountNo.Text)
            {

                string newID = this.txtAccountNo.Text;

                if (MyUtility.Check.Empty(newID))
                {
                    this.CurrentMaintain["ID"] = newID;
                    this.oldID = newID;
                    this.disAccountNoname.Text = string.Empty;
                    return;
                }

                List<SqlParameter> paras = new List<SqlParameter>() { new SqlParameter("@ID", newID) };
                bool exists = MyUtility.Check.Seek($@"SELECT 1 FROM FinanceTW.dbo.AccountNo WHERE Junk=0 AND ID = @ID", paras, "ProductionTPE");

                if (!exists)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                    this.CurrentMaintain["ID"] = this.oldID;
                    return;
                }

                this.CurrentMaintain["ID"] = newID;
                this.oldID = newID;

                string sqlCmd = $"SELECT Name FROM FinanceTW.dbo.AccountNo WHERE ID='{this.CurrentMaintain["ID"]}' ";
                this.disAccountNoname.Text = MyUtility.GetValue.Lookup(sqlCmd, "ProductionTPE");
            }
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            string sqlcmd = @"
SELECT b.ID ,b.Name ,[Unselectable]=
	IIF(b.ID IN  (
			SELECT ID 
			FROM FinanceTW.dbo.AccountNo
			WHERE Junk = 0
			AND (ID  IN (
					SELECT ID
					FROM AccountNoSetting 
					WHERE  LEN(ID) > 4 AND UnselectableShipB03 = 1
				)
			OR SUBSTRING(ID,1,4)  IN (	
				SELECT ID
				FROM AccountNoSetting 
				WHERE  LEN(ID)=4 AND UnselectableShipB03 = 1
			))
		)
	,'Y','')
FROM FinanceTW.dbo.AccountNo b
WHERE b.Junk=0 
";
            DataTable dt;
            DualResult result = DBProxy.Current.Select("ProductionTPE", sqlcmd, out dt);

            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            this.ToExcel(dt);
            return base.ClickPrint();
        }

        private bool ToExcel(DataTable excelDt)
        {
            string strXltName = "Basic_B18";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{strXltName}.xltx");

            MyUtility.Excel.CopyToXls(excelDt, string.Empty, $"{strXltName}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[1], DisplayAlerts_ForSaveFile: false);

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(strXltName);
            excelApp.ActiveWorkbook.SaveAs(strExcelName);
            excelApp.Quit();
            Marshal.ReleaseComObject(excelApp);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

    }
}
