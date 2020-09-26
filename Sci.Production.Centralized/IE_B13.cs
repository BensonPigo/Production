using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    public partial class IE_B13 : Win.Tems.Input1
    {
        public IE_B13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // 新增Upload File from excel 按鈕
            Win.UI.Button btn = new Win.UI.Button();
            btn.Text = "Upload File";
            btn.Click += new EventHandler(this.BtnUploadFile_Click);
            this.browsetop.Controls.Add(btn);
            btn.Size = new Size(110, 30);
        }

        private void BtnUploadFile_Click(object sender, EventArgs e)
        {
            string excelFile = MyUtility.File.GetFile("Excel files|*.xls;*.xlsx");
            if (MyUtility.Check.Empty(excelFile))
            {
                return;
            }

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(excelFile);
            if (excel == null)
            {
                return;
            }

            DataTable excelDataTable;
            DualResult result;
            string sqlCmd = @"select [ID],[DescKH],[DescVI],[DescCHS],[AddDate],[AddName] from OperationDesc WITH (NOLOCK)	where 1 = 0";
            result = DBProxy.Current.Select("ProductionTPE", sqlCmd, out excelDataTable);

            this.ShowWaitMessage("Starting Import EXCEL...");

            excel.Visible = false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            int intRowsCount = worksheet.UsedRange.Rows.Count;
            int intRowsStart = 2;
            int intRowsRead = intRowsStart - 1;

            Microsoft.Office.Interop.Excel.Range range;
            object[,] objCellArray;

            while (intRowsRead < intRowsCount)
            {
                intRowsRead++;
                range = worksheet.Range[string.Format("A{0}:G{0}", intRowsRead)];
                objCellArray = range.Value;
                if (objCellArray[1, 2] != null && objCellArray[1, 2].ToString().StartsWith("P"))
                {
                    DataRow newRow = excelDataTable.NewRow();
                    newRow["ID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C");
                    newRow["DescCHS"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C");
                    newRow["DescVI"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "C");
                    newRow["DescKH"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 7], "C");
                    excelDataTable.Rows.Add(newRow);
                }
            }

            excel.Workbooks.Close();
            excel.Quit();
            excel = null;

            this.HideWaitMessage();

            // 匯入到DB
            string sqlUpdate = $@"
merge ProductionTPE.dbo.OperationDesc as t
using (
	select * from (
		select [row] = ROW_NUMBER() over (partition by ltrim(id) order by ltrim(id))
		,* 
		from #tmp 
	) a where row= 1
) as s
on Ltrim(s.id) = t.id
when matched then update set
	t.DescKH = s.DescKH,
	t.DescVI = s.DescVI,
	t.DescCHS = s.DescCHS,
	t.EditDate = GETDATE(),
	t.EditName = '{Env.User.UserID}'	
when not matched by target then
	insert (
	   [ID]
      ,[DescKH]
      ,[DescVI]
      ,[DescCHS]
      ,[AddDate]
      ,[AddName])
	values (  
	   Ltrim(s.[ID])
      ,s.[DescKH]
      ,s.[DescVI]
      ,s.[DescCHS]
      ,GETDATE()
      ,'{Env.User.UserID}');
";
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection("ProductionTPE", out sqlConn);
            DataTable dtresult;
            if (!(result = MyUtility.Tool.ProcessWithDatatable(excelDataTable, string.Empty, sqlUpdate, out dtresult, conn: sqlConn)))
            {
                this.ShowErr(result);
                return;
            }
            else
            {
                MyUtility.Msg.InfoBox("import excel successful!");
                this.ReloadDatas();
            }
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtID.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["ID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Operation > can not be empty!");
                this.txtID.Focus();
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            string sqlCmd = $"SELECT DescEN FROM Operation WHERE ID='{this.CurrentMaintain["ID"]}' ";

            this.txtOperationtitle.Text = MyUtility.GetValue.Lookup(sqlCmd, "Production");
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            this.ShowWaitMessage("Processing...");

            DualResult result;
            DataTable printData;
            string sqlCmd = $@"
SELECT   [Part]=''
		,ID
		,[M/C]=''
		,[OperationTitle]=''
		,DescCHS
		,DescVI
		,DescKH
FROM OperationDesc WITH (NOLOCK)	
--WHERE ID='{this.CurrentMaintain["ID"]}' ";

            result = DBProxy.Current.Select("ProductionTPE", sqlCmd, out printData);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            for (int i = 0; i < printData.Rows.Count - 1; i++)
            {
                printData.Rows[i]["OperationTitle"] = MyUtility.GetValue.Lookup($"SELECT DescEN FROM Operation WITH (NOLOCK) WHERE ID='{printData.Rows[i]["ID"]}' ", "Production");
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Centralized_IE_B13.xltx"); // 預先開啟excel app

            Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\Centralized_IE_B13.xltx", objApp);

            com.ColumnsAutoFit = false;
            com.WriteTable(printData, 2);

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Centralized_IE_B13");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);

            strExcelName.OpenFile();
            #endregion

            this.HideWaitMessage();

            return true;
        }
    }
}
