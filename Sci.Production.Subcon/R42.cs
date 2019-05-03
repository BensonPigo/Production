﻿using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class R42 : Win.Tems.PrintForm
    {
        DataTable printData;
        StringBuilder sqlCmd;
        string SubProcess, SP, M, Factory, CutRef1, CutRef2;
        string processLocation;
        DateTime? dateBundle1, dateBundle2, dateBundleTransDate1, dateBundleTransDate2;
        public R42(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            comboload();
            this.comboFactory.setDataSource();
            this.comboRFIDProcessLocation.setDataSource();
        }

        //string date = "";
        private void comboload()
        {
            //DataTable dtSubprocessID;
            DualResult Result;
            //if (Result = DBProxy.Current.Select(null, "select 'ALL' as id,1 union select id,2 from Subprocess WITH (NOLOCK) where Junk = 0 ",
            //    out dtSubprocessID))
            //{
            //    this.comboSubProcess.DataSource = dtSubprocessID;
            //    this.comboSubProcess.DisplayMember = "ID";
            //}
            //else { ShowErr(Result); }

            DataTable dtfactory;
            if (Result = DBProxy.Current.Select(null, "select '' as id union select MDivisionID from factory WITH (NOLOCK) ", out dtfactory))
            {
                this.comboM.DataSource = dtfactory;
                this.comboM.DisplayMember = "ID";
            }
            else { ShowErr(Result); }
        }

        #region ToExcel3步驟
        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateBundleCDate.Value1) && MyUtility.Check.Empty(dateBundleCDate.Value2) &&
                MyUtility.Check.Empty(dateBundleTransDate.Value1) && MyUtility.Check.Empty(dateBundleTransDate.Value2))
            {
                MyUtility.Msg.WarningBox("Bundel CDate or Bundle Trans date can't empty!!");
                return false;
            }
            SubProcess = this.txtsubprocess.Text;
            SP = this.txtSPNo.Text;
            M = this.comboM.Text;
            Factory = this.comboFactory.Text;
            CutRef1 = this.txtCutRefStart.Text;
            CutRef2 = this.txtCutRefEnd.Text;
            dateBundle1 = this.dateBundleCDate.Value1;
            dateBundle2 = this.dateBundleCDate.Value2;
            dateBundleTransDate1 = this.dateBundleTransDate.Value1;
            dateBundleTransDate2 = this.dateBundleTransDate.Value2;
            this.processLocation = this.comboRFIDProcessLocation.Text;
            return base.ValidateInput();
        }

        //非同步讀取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region sqlcmd
            this.sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
Select distinct
            [Bundle#] = bt.BundleNo,
            [RFIDProcessLocationID] = bt.RFIDProcessLocationID,
            [Cut Ref#] = b.CutRef,
            [SP#] = b.Orderid,
            [Master SP#] = b.POID,
            [M] = b.MDivisionid,
            [Factory] = o.FtyGroup,
            [Style] = o.StyleID,
            [Season] = o.SeasonID,
            [Brand] = o.BrandID,
            [Comb] = b.PatternPanel,
            [Cutno] = b.Cutno,
            [Article] = b.Article,
            [Color] = b.ColorId,
            [Line] = b.SewinglineId,
            [Cell] = b.SewingCell,
            [Pattern] = bd.PatternCode,
            [PtnDesc] = bd.PatternDesc,
            [Group] = bd.BundleGroup,
            [Size] = bd.SizeCode,
            [Artwork] = stuff(sub.sub,1,1,''),
            [Qty] = bd.Qty,
            [RFID Reader] = bt.RFIDReaderId,
            [Sub-process] = bt.SubprocessId,
            [Type] = case when bt.Type = '1' then 'IN'
			              when bt.Type = '2' then 'Out'
			              when bt.Type = '3' then 'In/Out' end,
            [TagId] = bt.TagId,
            [TransferDate] = CAST(TransferDate AS DATE),
            [TransferTime] = TransferDate,
            bt.LocationID
            ,b.item
			,bt.PanelNo
			,CutCellID
            --CAST ( bt.TransferDate AS DATE) AS TransferDate
            from BundleTransfer  bt WITH (NOLOCK)
            left join Bundle_Detail bd WITH (NOLOCK) on bt.BundleNo = bd.BundleNo
            left join Bundle b WITH (NOLOCK) on bd.Id = b.Id
            left join orders o WITH (NOLOCK) on o.Id = b.OrderId
            outer apply(
	             select sub= (
		             Select distinct concat('+', bda.SubprocessId)
		             from Bundle_Detail_Art bda WITH (NOLOCK) 
		             where bda.Bundleno = bd.Bundleno
		             for xml path('')
	             )
            ) as sub
            where 1=1
            ");
            #endregion
            #region Append畫面上的條件
            if (!MyUtility.Check.Empty(SubProcess))
            {//();
                sqlCmd.Append($@" and (bt.SubprocessId in ('{SubProcess.Replace(",", "','")}') or '{SubProcess}'='')");
            }
            if (!MyUtility.Check.Empty(CutRef1) && (!MyUtility.Check.Empty(CutRef1)))
            {
                sqlCmd.Append(string.Format(@" and b.CutRef between '{0}' and '{1}'", CutRef1, CutRef2));
            }
            if (!MyUtility.Check.Empty(SP))
            {
                sqlCmd.Append(string.Format(@" and b.Orderid = '{0}'", SP));
            }
            if (!MyUtility.Check.Empty(dateBundle1))
            {
                sqlCmd.Append(string.Format(@" and b.Cdate >= '{0}'", Convert.ToDateTime(dateBundle1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateBundle2))
            {
                sqlCmd.Append(string.Format(@" and b.Cdate <= '{0}'", Convert.ToDateTime(dateBundle2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateBundleTransDate1))
            {
                sqlCmd.Append(string.Format(@" and bt.TransferDate >= '{0}'", Convert.ToDateTime(dateBundleTransDate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateBundleTransDate2))
            {
                // TransferDate 是 datetime, 直接用日期做判斷的話要加一天才不會漏掉最後一天的資料
                sqlCmd.Append(string.Format(@" and bt.TransferDate <= '{0}'", Convert.ToDateTime(((DateTime)dateBundleTransDate2).AddDays(1)).ToString("d")));
            }
            if (!MyUtility.Check.Empty(M))
            {
                sqlCmd.Append(string.Format(@" and b.MDivisionid = '{0}'", M));
            }
            if (!MyUtility.Check.Empty(Factory))
            {
                sqlCmd.Append(string.Format(@" and o.FtyGroup = '{0}'", Factory));
            }
            if (!MyUtility.Check.Empty(this.processLocation))
            {
                sqlCmd.Append(string.Format(@" and bt.RFIDProcessLocationID = '{0}'", this.processLocation));
            }
            #endregion

            string result_cnt_cmd = "select [resultcnt] = count(*) from  (" + sqlCmd.ToString() + ") resultCnt";

            DBProxy.Current.DefaultTimeout = 1200;  //加長時間為30分鐘，避免timeout
            DualResult result = DBProxy.Current.Select(null, result_cnt_cmd, out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            decimal data_cnt = MyUtility.Convert.GetInt(printData.Rows[0]["resultcnt"]);
            SetCount((long)data_cnt);
            if (data_cnt <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                printData.Clear();
                printData.Dispose();
                return false;
            }
            
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R42_Bundle Transaction detail (RFID).xltx"); //預先開啟excel app
            decimal excelMaxrow = 1010000;
            int ttlSheet = (int)Math.Ceiling(data_cnt / excelMaxrow);
            if (ttlSheet > 1)
            {
                for (int i = 0; i < ttlSheet; i++)
                {
                    if (i > 0)
                    {
                        Microsoft.Office.Interop.Excel.Worksheet worksheet1 = ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1]);
                        Microsoft.Office.Interop.Excel.Worksheet worksheetn = ((Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[i + 1]);
                        worksheet1.Copy(worksheetn);
                    }
                }
            }
            int sheet = 1;
            //因為一次載入太多筆資料到DataTable 會造成程式佔用大量記憶體，改為每1萬筆載入一次並貼在excel上
            #region 分段抓取資料填入excel
            this.ShowLoadingText($"Data Loading , please wait …");
            DataTable tmpDatas = new DataTable();
            SqlConnection conn = null;
            DBProxy.Current.OpenConnection(this.ConnectionName, out conn);
            var cmd = new SqlCommand(sqlCmd.ToString(), conn);
            cmd.CommandTimeout = 300;
            var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
            int loadCounts = 0;
            int loadCounts2 = 0;
            using (conn)
            using (reader)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    tmpDatas.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                }

                while (reader.Read())
                {
                    object[] items = new object[reader.FieldCount];
                    reader.GetValues(items);
                    tmpDatas.LoadDataRow(items, true);
                    loadCounts++;
                    loadCounts2++;
                    if (loadCounts % 10000 == 0)
                    {
                        if (sheet != 1 + loadCounts / (int)excelMaxrow)
                        {
                            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[sheet];   // 取得工作表
                            Marshal.ReleaseComObject(objSheets);    //釋放sheet

                            string strExcelName1 = Sci.Production.Class.MicrosoftFile.GetName("Subcon_R42_BundleTransactiondetail(RFID)");
                            Microsoft.Office.Interop.Excel.Workbook workbook1 = objApp.ActiveWorkbook;
                            workbook1.SaveAs(strExcelName1);
                            workbook1.Close();
                            objApp.Quit();
                            Marshal.ReleaseComObject(objApp);
                            Marshal.ReleaseComObject(workbook1);
                            objApp = MyUtility.Excel.ConnectExcel(strExcelName1);
                        }
                        sheet = 1 + loadCounts / (int)excelMaxrow;
                        if (loadCounts - ((sheet - 1) * (int)excelMaxrow) <= 0)
                        {
                            loadCounts2 = loadCounts - ((sheet - 1) * (int)excelMaxrow) + 10000;
                        }
                        else
                        {
                            loadCounts2 = loadCounts - ((sheet - 1) * (int)excelMaxrow);
                        }
                        this.ShowLoadingText($"Data Loading – {loadCounts} , please wait …");
                        MyUtility.Excel.CopyToXls(tmpDatas, "", "Subcon_R42_Bundle Transaction detail (RFID).xltx", loadCounts2 - 9999, false, null, objApp, wSheet: objApp.Sheets[sheet]);// 將datatable copy to excel
                        
                        this.DataTableClearAll(tmpDatas);
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            tmpDatas.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                        }
                    }
                }

                MyUtility.Excel.CopyToXls(tmpDatas, "", "Subcon_R42_Bundle Transaction detail (RFID).xltx", loadCounts2 - (loadCounts2 % 10000) + 1, false, null, objApp, wSheet: objApp.Sheets[sheet]);// 將datatable copy to excel
                this.DataTableClearAll(tmpDatas);
            }
            this.HideLoadingText();
            #endregion

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Subcon_R42_BundleTransactiondetail(RFID)");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);          //釋放objApp
            Marshal.ReleaseComObject(workbook);
            printData.Clear();
            printData.Dispose();
            strExcelName.OpenFile();
            #endregion             
            return true;
        }
        #endregion
        private void DataTableClearAll(DataTable target)
        {
            target.Rows.Clear();
            target.Constraints.Clear();
            target.Columns.Clear();
            target.ExtendedProperties.Clear();
            target.ChildRelations.Clear();
            target.ParentRelations.Clear();
        }
    }
}
