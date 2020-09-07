using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Sci.Win;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Subcon
{
    public partial class R45 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private StringBuilder sqlWhere = new StringBuilder();
        string SubProcess;
        string strExcelName;

        public R45(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            #region combo box預設值

            DataTable dt;
            DBProxy.Current.Select(null, @"
SELECT [Text]=ID,[Value]=ID FROM SubProcess WITH(NOLOCK) WHERE Junk=0 AND IsRFIDProcess=1
", out dt);

            this.comboSubPorcess.DataSource = dt;
            this.comboSubPorcess.ValueMember = "Value";
            this.comboSubPorcess.DisplayMember = "Text";
            this.comboSubPorcess.SelectedIndex = 0;
            #endregion

            this.txtMdivision.Text = Env.User.Keyword;

            // 排除非生產工廠
            this.txtFactory.IsProduceFty = false;
            this.txtFactory.FilteMDivision = true;
            this.txtFactory.Text = Env.User.Factory;
        }

        protected override bool ValidateInput()
        {
            this.sqlWhere.Clear();
            if (string.IsNullOrEmpty(this.txtMdivision.Text) || string.IsNullOrEmpty(this.txtFactory.Text))
            {
                MyUtility.Msg.InfoBox("M and Factory cannnot be empty");
                return false;
            }
            else
            {
                if (string.IsNullOrEmpty(this.txtSPNo.Text) && string.IsNullOrEmpty(this.txtCutRef.Text))
                {
                    MyUtility.Msg.InfoBox("SP# and CutRef# cannot all be empty.");
                    return false;
                }
                else
                {
                    #region WHERE條件

                    if (!string.IsNullOrEmpty(this.txtMdivision.Text))
                    {
                        this.sqlWhere.Append($"AND o.MDivisionID='{this.txtMdivision.Text}'" + Environment.NewLine);
                    }

                    if (!string.IsNullOrEmpty(this.txtFactory.Text))
                    {
                        this.sqlWhere.Append($"AND o.FtyGroup='{this.txtFactory.Text}'" + Environment.NewLine);
                    }

                    if (!string.IsNullOrEmpty(this.txtSPNo.Text))
                    {
                        this.sqlWhere.Append($"AND b.Orderid='{this.txtSPNo.Text}'" + Environment.NewLine);
                    }

                    if (!string.IsNullOrEmpty(this.txtCutRef.Text))
                    {
                        this.sqlWhere.Append($"AND w.CutRef='{this.txtCutRef.Text}'" + Environment.NewLine);
                    }

                    if (!string.IsNullOrEmpty(this.comboSubPorcess.Text))
                    {
                        this.sqlWhere.Append($"AND ( DefaultSubProcess.SubProcessID LIKE '%{this.comboSubPorcess.Text}%' OR HasSubProcess.Value > 0  )" + Environment.NewLine);
                    }

                    if (!MyUtility.Check.Empty(this.txtSize.Text))
                    {
                        this.sqlWhere.Append($"AND bd.SizeCode='{this.txtSize.Text}'" + Environment.NewLine);
                    }

                    this.sqlWhere.Append($"ORDER BY b.Colorid,bd.SizeCode,b.PatternPanel,bd.BundleNo");

                    // 「Extend All Parts」勾選則true
                    // IsExtendAllParts = this.chkExtendAllParts.Checked ? "true" : "false";
                    this.SubProcess = this.comboSubPorcess.Text;
                    #endregion

                    return base.ValidateInput();
                }
            }
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result;
            StringBuilder sqlCmd = new StringBuilder();

            // StringBuilder sqlWhere = new StringBuilder();
            List<SqlParameter> parameterList = new List<SqlParameter>();

            sqlCmd.Append($@"
SELECT
bd.BundleGroup
,b.Colorid
,bd.SizeCode
,b.PatternPanel
,[Cutpart]= bd.Patterncode
,[CutpartName]= {(this.chkExtendAllParts.Checked ?
                    "CASE WHEN bd.Patterncode = 'ALLPARTS' THEN bdap.PatternDesc ELSE bd.PatternDesc END --basic from 「Extend All Parts」 is checked or not"
                    :
                    "bd.PatternDesc")}

,[SubProcessID]= SubProcess.SubProcessID
,w.CutCellID
,bd.Parts
,bd.Qty
,bd.BundleNo
,[ActualAcc.ReceivedQty]=''
,[IsEXCESS] = case when b.IsEXCESS=1 then 'V' else '' end
,[Remartks]=''
,[WaterbeetleConfirmation]=''

-----Excel上方資訊
,[ExportedDate]=GETDATE()
,o.FactoryID
,b.Orderid
,o.StyleID
,b.CutRef
,b.Sewinglineid
,[SubProcess]='{this.SubProcess}'

FROM Bundle b
INNER JOIN Bundle_Detail bd ON bd.ID=b.Id
{(this.chkExtendAllParts.Checked ? "LEFT JOIN Bundle_Detail_AllPart bdap ON bdap.ID=b.ID AND bd.Patterncode ='ALLPARTS'" : string.Empty)}
INNER JOIN Orders O ON o.ID=b.Orderid
inner join factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
LEFT JOIN Workorder w ON W.CutRef=b.CutRef AND w.ID=b.POID
LEFT JOIN BundleInOut bio ON bio.BundleNo=bd.BundleNo AND bio.RFIDProcessLocationID ='' AND bio.SubProcessId='{this.SubProcess}'
OUTER APPLY(
	--用來判斷，該Bundle ID、Bundle No，是否包含User選定的SubProcess
	SELECT [Value]=IIF( COUNT(bda.SubProcessID) > 0 , 1 ,0 )
	FROM Bundle_Detail_Art bda
	WHERE  bda.BundleNo = bd.BundleNo 
	AND bda.ID = b.ID   
	AND bda.SubProcessID ='{this.SubProcess}'
)HasSubProcess

OUTER APPLY(
    --顯示該Bundle ID、Bundle No，所有的SubProcess
	SELECT [SubProcessID]=LEFT(SubProcessID,LEN(SubProcessID)-1)  
	FROM
	(
		SELECT [SubProcessID]=
		(
			SELECT ID+ ' + '
			FROM SubProcess s
			WHERE EXISTS
			(
				SELECT 1 FROM Bundle_Detail_Art bda
				WHERE  bda.BundleNo = bd.BundleNo 
				AND bda.ID = b.ID   
				AND bda.SubProcessID = s.ID
			)
			FOR XML PATH('')
		)
	)M
)SubProcess
OUTER APPLY(
--每個Bundle都會有的SubProcess
	SELECT [SubProcessID]=LEFT(SubProcessID,LEN(SubProcessID)-1)  
	FROM
	(
		SELECT [SubProcessID]=
		(
			SELECT ID+ ' + '
			FROM SubProcess s
			WHERE 
			s.IsRFIDDefault = 1
			FOR XML PATH('')
		)
	)M
)DefaultSubProcess
WHERE 1=1
");

            result = DBProxy.Current.Select(null, sqlCmd.Append(this.sqlWhere).ToString(), out this.printData);

            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.SetCount(this.printData.Rows.Count);

            this.ShowWaitMessage("Excel Processing...");

            #region 尋出的資料依照SP# + CutRef# 進行分組

            List<DataTable> DataList = new List<DataTable>();

            DataTable GroupByList = new DataTable();
            GroupByList = this.printData.DefaultView.ToTable(true, new string[] { "Orderid", "CutRef" });

            foreach (DataRow dr in GroupByList.Rows)
            {
                DataTable tmp = this.printData.Select($"Orderid='{dr["Orderid"].ToString()}' AND CutRef='{dr["CutRef"].ToString()}'").CopyToDataTable();

                DataList.Add(tmp);
            }

            #endregion

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Subcon_R45.xltx"); // 預先開啟excel app

            for (int i = 0; i < DataList.Count; i++)
            {
                // 第二筆資料才需要複製Sheet
                if (i > 0)
                {
                    // 複製第一頁Sheet
                    Excel.Worksheet worksheet1 = (Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1];
                    Excel.Worksheet worksheetn = (Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[i + 1];
                    worksheet1.Copy(worksheetn);
                }
            }

            // 每個工作表個別處理
            for (int i = 0; i < DataList.Count; i++)
            {
                if (DataList[i].Rows.Count == 0)
                {
                    continue;
                }

                // 取得工作表
                Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[i + 1];

                // 將datatable copy to excel
                MyUtility.Excel.CopyToXls(DataList[i], null, "Subcon_R45.xltx", headerRow: 8, excelApp: objApp, wSheet: objSheets, showExcel: false, showSaveMsg: false);

                objSheets.Name = DataList[i].Rows[0]["Orderid"].ToString() + "-" + DataList[i].Rows[0]["CutRef"].ToString();

                // 上方欄位 固定值
                List<string> Lines = new List<string>();

                for (int x = 0;  x <= DataList[i].DefaultView.ToTable(true, new string[] { "Sewinglineid" }).Rows.Count - 1;  x++)
                {
                    string Line = DataList[i].DefaultView.ToTable(true, new string[] { "Sewinglineid" }).Select()[x]["Sewinglineid"].ToString();
                    if (!Lines.Contains(Line))
                    {
                        Lines.Add(Line);
                    }
                }

                // Exported Date
                objSheets.Cells[2, 11] = Convert.ToDateTime(DataList[i].Rows[0]["ExportedDate"]).ToShortDateString();

                // Factory
                objSheets.Cells[3, 2] = DataList[i].Rows[0]["FactoryID"].ToString();

                // SP#
                objSheets.Cells[4, 2] = DataList[i].Rows[0]["Orderid"].ToString();

                // Inline Line#
                objSheets.Cells[4, 7] = string.Join(",", Lines); // DataList[i].Rows[0]["Sewinglineid"].ToString();

                // Style
                objSheets.Cells[5, 2] = DataList[i].Rows[0]["StyleID"].ToString();

                // SubProcess
                objSheets.Cells[5, 7] = DataList[i].Rows[0]["SubProcess"].ToString();

                // CutRef
                objSheets.Cells[6, 2] = DataList[i].Rows[0]["CutRef"].ToString();

                if (this.comboSubPorcess.Text.ToUpper() != "LOADING")
                {
                    // 解除合併儲存格
                    Excel.Range range = objSheets.get_Range((Excel.Range)objSheets.Cells[1, 1], (Excel.Range)objSheets.Cells[1, 14]);
                    range.UnMerge();

                    // 如果是LOADING，這一欄不顯示
                    objSheets.Columns["O"].Clear();

                    Excel.Range range2 = objSheets.get_Range((Excel.Range)objSheets.Cells[1, 1], (Excel.Range)objSheets.Cells[1, 12]);
                    range.Merge();
                }

                // 增加寬度
                Excel.Range range3 = objSheets.get_Range("B8");
                Excel.Range range4 = objSheets.get_Range("C8");
                range3.Columns.ColumnWidth = 8;
                range4.Columns.ColumnWidth = 8;

                // 多於資訊清除
                objSheets.Columns["P"].Clear();
                objSheets.Columns["Q"].Clear();
                objSheets.Columns["R"].Clear();
                objSheets.Columns["S"].Clear();
                objSheets.Columns["T"].Clear();
                objSheets.Columns["U"].Clear();
                objSheets.Columns["V"].Clear();

                Marshal.ReleaseComObject(objSheets); // 釋放sheet
            }

            #region Save Excel
            this.strExcelName = Class.MicrosoftFile.GetName("Subcon_R45");
            Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(this.strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(workbook);
            #endregion

            this.strExcelName.OpenFile();

            this.HideWaitMessage();

            return true;
        }
    }
}
