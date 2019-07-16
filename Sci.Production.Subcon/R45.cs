﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using Sci.Win;
using System.Data.SqlClient;
using Sci.Utility.Excel;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Subcon
{
    public partial class R45 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private StringBuilder sqlWhere = new StringBuilder();
        string   SubProcess ,  strExcelName;

        public R45(ToolStripMenuItem menuitem) : base(menuitem)
        {
            InitializeComponent();

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


            this.txtMdivision.Text = Sci.Env.User.Keyword;

            //排除非生產工廠
            this.txtFactory.IsProduceFty = false;
            this.txtFactory.FilteMDivision = true;
            this.txtFactory.Text = Sci.Env.User.Factory;
        }

        protected override bool ValidateInput()
        {

            sqlWhere.Clear();
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
                        sqlWhere.Append($"AND o.MDivisionID='{this.txtMdivision.Text}'" + Environment.NewLine);
                    }

                    if (!string.IsNullOrEmpty(this.txtFactory.Text))
                    {
                        sqlWhere.Append($"AND o.FtyGroup='{this.txtFactory.Text}'" + Environment.NewLine);
                    }

                    if (!string.IsNullOrEmpty(this.txtSPNo.Text))
                    {
                        sqlWhere.Append($"AND b.Orderid='{this.txtSPNo.Text}'" + Environment.NewLine);
                    }


                    if (!string.IsNullOrEmpty(this.txtCutRef.Text))
                    {
                        sqlWhere.Append($"AND w.CutRef='{this.txtCutRef.Text}'" + Environment.NewLine);
                    }


                    if (!string.IsNullOrEmpty(this.comboSubPorcess.Text))
                    {
                        sqlWhere.Append($"AND ( DefaultSubProcess.SubProcessID LIKE '%{this.comboSubPorcess.Text}%' OR HasSubProcess.Value > 0  )" + Environment.NewLine);
                    }




                    sqlWhere.Append($"ORDER BY b.Colorid,bd.SizeCode,b.PatternPanel,bd.BundleNo");

                    //「Extend All Parts」勾選則true
                    //IsExtendAllParts = this.chkExtendAllParts.Checked ? "true" : "false";

                    SubProcess = this.comboSubPorcess.Text;
                    #endregion

                    return base.ValidateInput();
                }
            }



        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result;
            StringBuilder sqlCmd = new StringBuilder();
            //StringBuilder sqlWhere = new StringBuilder();
            List<SqlParameter> parameterList = new List<SqlParameter>();




            sqlCmd.Append($@"

SELECT {(this.chkExtendAllParts.Checked ? "DISTINCT" : "")}
bd.BundleGroup
,b.Colorid
,bd.SizeCode
,b.PatternPanel
,[Cutpart]= bd.Patterncode
,[CutpartName]= {(this.chkExtendAllParts.Checked ?
                    "CASE WHEN 'true'='true'  AND  bd.Patterncode ='ALLPARTS' THEN  bdap.PatternDesc ELSE bd.PatternDesc  END --basic from 「Extend All Parts」 is checked or not"
                    :
                    "bd.PatternDesc")}
,[SubProcessID]= SubProcess.SubProcessID
,w.CutCellID
,bd.Parts
,bd.Qty
,bd.BundleNo
,[ActualAcc.ReceivedQty]=''
,[Remartks]=''
,[WaterbeetleConfirmation]=''

-----Excel上方資訊
,[ExportedDate]=GETDATE()
,o.FactoryID
,b.Orderid
,o.StyleID
,b.CutRef
,b.Sewinglineid
,[SubProcess]='{SubProcess}'

FROM Bundle b
INNER JOIN Bundle_Detail bd ON bd.ID=b.Id
{(this.chkExtendAllParts.Checked ? "LEFT JOIN Bundle_Detail_AllPart bdap ON bdap.ID=b.ID" : "")}
INNER JOIN Orders O ON o.ID=b.Orderid
LEFT JOIN Workorder w ON W.CutRef=b.CutRef AND w.ID=b.POID
LEFT JOIN BundleInOut bio ON bio.BundleNo=bd.BundleNo AND bio.RFIDProcessLocationID ='' AND bio.SubProcessId='{SubProcess}'
OUTER APPLY(
	--用來判斷，該Bundle ID、Bundle No，是否包含User選定的SubProcess
	SELECT [Value]=IIF( COUNT(bda.SubProcessID) > 0 , 1 ,0 )
	FROM Bundle_Detail_Art bda
	WHERE  bda.BundleNo = bd.BundleNo 
	AND bda.ID = b.ID   
	AND bda.SubProcessID ='{SubProcess}'
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

            result = DBProxy.Current.Select(null, sqlCmd.Append(sqlWhere).ToString(), out printData);

            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }
        

        protected override bool OnToExcel(ReportDefinition report)
        {
            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.SetCount(printData.Rows.Count);

            this.ShowWaitMessage("Excel Processing...");


            #region 尋出的資料依照SP# + CutRef# 進行分組 

            List<DataTable> DataList = new List<DataTable>();

            DataTable GroupByList = new DataTable();
            GroupByList = printData.DefaultView.ToTable(true, new string[] { "Orderid", "CutRef" });

            foreach (DataRow dr in GroupByList.Rows)
            {
                DataTable tmp = printData.Select($"Orderid='{dr["Orderid"].ToString()}' AND CutRef='{dr["CutRef"].ToString()}'").CopyToDataTable();
                
                DataList.Add(tmp);
            }


            #endregion


            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R45.xltx"); //預先開啟excel app

            for (int i = 0; i < DataList.Count; i++)
            {
                //第二筆資料才需要複製Sheet
                if (i > 0)
                {
                    //複製第一頁Sheet
                    Microsoft.Office.Interop.Excel.Worksheet worksheet1 = ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1]);
                    Microsoft.Office.Interop.Excel.Worksheet worksheetn = ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[i + 1]);
                    worksheet1.Copy(worksheetn);
                }
            }


            //每個工作表個別處理
            for (int i = 0; i < DataList.Count; i++)
            {
                if (DataList[i].Rows.Count == 0)
                    continue;

                // 取得工作表
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[i + 1]; 
                //將datatable copy to excel
                MyUtility.Excel.CopyToXls(DataList[i], null, "Subcon_R45.xltx", headerRow: 8, excelApp: objApp, wSheet: objSheets, showExcel: false, showSaveMsg: false);

                objSheets.Name = DataList[i].Rows[0]["Orderid"].ToString() + "-" + DataList[i].Rows[0]["CutRef"].ToString();

                //上方欄位 固定值
                
                List<string> Lines = new List<string>();

                for (int x = 0;  x <= DataList[i].DefaultView.ToTable(true, new string[] { "Sewinglineid" }).Rows.Count-1;  x++)
                {
                    string Line = DataList[i].DefaultView.ToTable(true, new string[] { "Sewinglineid" }).Select()[x]["Sewinglineid"].ToString();
                    if (!Lines.Contains(Line))
                    {
                        Lines.Add(Line);
                    }
                }

                //Exported Date 
                objSheets.Cells[2, 11] =Convert.ToDateTime( DataList[i].Rows[0]["ExportedDate"]).ToShortDateString();
                // Factory
                objSheets.Cells[3, 2] = DataList[i].Rows[0]["FactoryID"].ToString();
                //SP# 
                objSheets.Cells[4, 2] = DataList[i].Rows[0]["Orderid"].ToString();
                //Inline Line#
                objSheets.Cells[4, 7] = string.Join(",",Lines);// DataList[i].Rows[0]["Sewinglineid"].ToString();
                //Style
                objSheets.Cells[5, 2] = DataList[i].Rows[0]["StyleID"].ToString();
                //SubProcess
                objSheets.Cells[5, 7] = DataList[i].Rows[0]["SubProcess"].ToString();
                //CutRef
                objSheets.Cells[6, 2] = DataList[i].Rows[0]["CutRef"].ToString();

                if (this.comboSubPorcess.Text.ToUpper() != "LOADING")
                {
                    //解除合併儲存格
                    Excel.Range range = objSheets.get_Range((Excel.Range)objSheets.Cells[1, 1], (Excel.Range)objSheets.Cells[1, 14]);
                    range.UnMerge();
                    // 如果是LOADING，這一欄不顯示
                    objSheets.Columns["N"].Clear();

                    Excel.Range range2 = objSheets.get_Range((Excel.Range)objSheets.Cells[1, 1], (Excel.Range)objSheets.Cells[1, 12]);
                    range.Merge();
                }

                //增加寬度
                Excel.Range range3 = objSheets.get_Range("B8");
                Excel.Range range4 = objSheets.get_Range("C8");
                range3.Columns.ColumnWidth = 8;
                range4.Columns.ColumnWidth = 8;

                //多於資訊清除
                objSheets.Columns["O"].Clear();
                objSheets.Columns["P"].Clear();
                objSheets.Columns["Q"].Clear();
                objSheets.Columns["R"].Clear();
                objSheets.Columns["S"].Clear();
                objSheets.Columns["T"].Clear();
                objSheets.Columns["U"].Clear();



                Marshal.ReleaseComObject(objSheets); //釋放sheet      
            }


            #region Save Excel
            strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Subcon_R45");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(workbook);
            #endregion

            strExcelName.OpenFile();

            this.HideWaitMessage();


            return true;
        }

    }
}
