using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    public partial class IE_B13 : Sci.Win.Tems.Input1
    {
        public IE_B13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // 新增Upload File from excel 按鈕
            Sci.Win.UI.Button btn = new Win.UI.Button();
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
            result = DBProxy.Current.Select(null, sqlCmd, out excelDataTable);

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

                DataRow newRow = excelDataTable.NewRow();
                newRow["ID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C");
                newRow["DescCHS"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C");
                newRow["DescVI"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "C");
                newRow["DescKH"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 7], "C");
                excelDataTable.Rows.Add(newRow);
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
	t.EditName = '{Sci.Env.User.UserID}'	
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
      ,'{Sci.Env.User.UserID}');
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
            }

        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtID.ReadOnly = true;
        }

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
    }
}
