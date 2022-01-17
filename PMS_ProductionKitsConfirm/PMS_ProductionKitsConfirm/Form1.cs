﻿using Ict;
using PostJobLog;
using Sci;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace PMS_ProductionKitsConfirm
{
    public partial class Form1 : Sci.Win.Tems.Input7
    {
        private string mailFrom = "foxpro@sportscity.com.tw";
        private string mailServer = "172.17.2.8";
        private string eMailID = "foxpro";
        private string eMailPwd = "orpxof";
        private string[] mailTO;
        private string[] mailTOCC;
        private string[] MailList_ForTest = ConfigurationManager.AppSettings["MailList_ForTest"].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        private string[] settingFactoryList = ConfigurationManager.AppSettings["SettingFactoryList"].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        private string isTest = ConfigurationManager.AppSettings["IsTest"].ToString();

        bool isAuto = false;
        private List<string> factorys = new List<string>();
        private DateTime SendDateS;
        private DateTime SendDateE;

        public Form1()
        {
            InitializeComponent();
            isAuto = false;
        }

        public Form1(string _isAuto)
        {
            InitializeComponent();
            isAuto = !_isAuto.Empty();
            if (isAuto)
            {
                this.OnFormLoaded();
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.EditMode = true;
            Ict.Logs.APP.LogInfo("ProductionKitsConfirm Start IsAuto : " + isAuto.ToString());

            this.SetSendDate();


            Ict.Logs.APP.LogInfo("SendDateS : " + SendDateS.ToString("yyyy/MM/dd") + ", SendDateE : " + SendDateE.ToString("yyyy/MM/dd"));

            this.GetMailSetting();

            Dictionary<string, DataTable> FtyDataTables = this.Query();

            if (FtyDataTables == null)
            {
                return;
            }

            List<string> files = this.CreateExcel_OneSheet(FtyDataTables);

            if (files.Count > 0)
            {
                this.SendMail(files);
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            this.EditMode = true;
            this.GetFactoryList();
            Ict.Logs.APP.LogInfo("ProductionKitsConfirm Start IsAuto : " + isAuto.ToString());

            this.SetSendDate();
            Ict.Logs.APP.LogInfo("SendDateS : " + SendDateS.ToString("yyyy/MM/dd") + ", SendDateE : " + SendDateE.ToString("yyyy/MM/dd"));

            this.GetMailSetting();

            Dictionary<string, DataTable> FtyDataTables = this.Query();

            if (FtyDataTables == null)
            {
                return;
            }

            List<string> files = this.CreateExcel_OneSheet(FtyDataTables);

            if (files.Count > 0)
            {
                this.SendMail(files);
            }

        }


        private void SendMail(List<string> files)
        {
            Ict.Logs.APP.LogInfo("Send Mail Start");


            #region Sned Mail
            string subject = "Outstanding Production Kits Confirm " + this.SendDateS.ToString("yyyyMMdd") + " ~ " + this.SendDateE.ToString("yyyyMMdd");
            string desc = $@"
Hi All FTY PPIC & Planning manager
<br/><br/>
Please help to check attached file if production kits receive or not and need FTY in charge use PMS PPIC=>P03 to confirm receive date within 2 days upon receiving.
We would like to remind factory, confirm received date means confirm MR send document is correct and without lacking.
<br/><br/>
<hr/>
";
            try
            {
                if (!MyUtility.Check.Seek("select * from mailto where id = '101'", out DataRow dr))
                {
                    return;
                }

                this.mailTO = MyUtility.Convert.GetString(dr["ToAddress"]).Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                this.mailTOCC = MyUtility.Convert.GetString(dr["CcAddress"]).Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception ex)
            {
                Ict.Logs.APP.LogInfo(ex.ToString());
                throw;
            }

            this.MailToHtml(subject, files, desc);
            #endregion

            Ict.Logs.APP.LogInfo("Send Mail End");
        }

        private void MailToHtml(string subject, List<string> attachs, string desc)
        {
            //寄件者 & 收件者
            MailMessage message = new MailMessage();
            message.Subject = subject;
            message.From = new MailAddress(this.mailFrom);

            foreach (string mail in mailTO)
            {
                message.To.Add(mail);
            }

            // 若是測試信件，則寄給測試用收件人
            if (this.isTest == "true")
            {
                message.To.Clear();

                foreach (var mail in MailList_ForTest)
                {
                    message.To.Add(mail);
                }
            }

            if (message.To.Count == 0)
            {
                return;
            }

            foreach (string mailCC in mailTOCC)
            {
                message.Bcc.Add(mailCC);
            }

            message.Body = desc;
            message.IsBodyHtml = true;
            //Gmail Smtp
            SmtpClient client = new SmtpClient(mailServer);
            //寄件者 帳密
            client.Credentials = new NetworkCredential(eMailID, eMailPwd);

            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            //夾檔
            foreach (string attach in attachs)
            {
                Attachment attachFile = new Attachment(attach);
                message.Attachments.Add(attachFile);
            }

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Ict.Logs.APP.LogInfo("Send Email Error : " + ex.ToString());
            }
        }

        private void GetMailSetting()
        {
            if (MyUtility.Check.Seek("select top 1 Mailserver,Sendfrom,EmailID,EmailPwd,RgCode from system", out DataRow emailInfo, connectionName: "PMSDB_TSR"))
            {
                this.mailServer = emailInfo["Mailserver"].ToString();
                this.mailFrom = emailInfo["Sendfrom"].ToString();
                this.eMailID = emailInfo["EmailID"].ToString();
                this.eMailPwd = emailInfo["EmailPwd"].ToString();
            }
        }

        private DualResult QueryPMSDB()
        {
            DualResult result = new DualResult(false);
            string sql = string.Empty;
            DataTable dt = new DataTable();
            if (!isAuto)
            {
                return result;
            }

            List<string> newfactoryList = settingFactoryList
                                    .Where(x => !x.EqualString("PMSDB_TSR"))
                                    .Select(x => x.Replace("PMSDB_", ""))
                                    .ToList();

            if (newfactoryList.Count == 0)
            {
                return result;
            }

            string sqlwhere = string.Join("','", newfactoryList);
            sql = $@"
select s.Region
from 
(
	 select
		 s.Region
		 , [dbName] = case when CHARINDEX('Production', s.FilePath) > 0 then 'Production'					  
						  when CHARINDEX('ManufacturingExecution', s.FilePath) > 0 then 'ManufacturingExecution'
						else ''
					end
		 , [maxStartDate] = cast(max(s.StartTime) as date)
	 from SystemJobLog s
	 where CAST(s.StartTime as date) = cast(getdate() as date)
	 and s.SystemID = 'PMS_Backup'
	 and s.Succeeded = 1
	 and  OperationName like '%Complete' 
	 and (CHARINDEX('Production', s.FilePath) > 0  or CHARINDEX('ManufacturingExecution', s.FilePath) > 0)
     and s.Region in ('{sqlwhere}')
	 group by s.Region, s.FilePath
)s
group by s.Region
having count(*) = 2";

            result = DBProxy.Current.Select("MIS", sql, out dt);
            if (!result)
            {
                return result;
            }

            result = new DualResult(false);
            if (dt.Rows.Count == newfactoryList.Count)
            {
                result = new DualResult(true);
            }

            return result;
        }

        /// <summary>
        /// 取得資料： 廠區 → 對應的DataTable的Key Value pair型別
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, DataTable> Query()
        {
            DualResult result = new DualResult(false);
            List<DataTable> DataTables = new List<DataTable>();
            Dictionary<string, DataTable> FtyData = new Dictionary<string, DataTable>();

            string sqlCmd;
            if (isAuto)
            {
                this.factorys = settingFactoryList.ToList();
            }

            if (this.factorys.Count == 0)
            {
                return null;
            }

            string sendDateS = this.SendDateS.ToString("yyyy/MM/dd");
            string sendDateE = this.SendDateE.ToString("yyyy/MM/dd");
            DBProxy.Current.DefaultTimeout = 1800;
            foreach (string conn in this.factorys)
            {
                string fty = conn.Replace("PMSDB_", "");

                sqlCmd = $@"
select sp.FactoryID
	,[StyleID] = s.ID
	,s.SeasonID
	,sp.Article
	,[DOC] = (select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'ProductionKits' and ID = sp.DOC)
	,sp.SendDate
	,sp.ReceiveDate
	,sp.FtyRemark
	,sp.ProvideDate
	,sp.OrderId
	,sp.SCIDelivery
	,sp.BuyerDelivery
	,isnull((sp.MRHandle+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = sp.MRHandle)),sp.MRHandle) as MRName
	,isnull((sp.SMR+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = sp.SMR)),sp.SMR) as SMRName
	,isnull((sp.PoHandle+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = sp.PoHandle)),sp.PoHandle) as POHandleName
	,isnull((sp.POSMR+' '+(select Name+' #'+ExtNo from TPEPass1 WITH (NOLOCK) where ID = sp.POSMR)),sp.POSMR) as POSMRName
    ,[Fty]='{fty}'
    ,sp.MRLastDate
from Style_ProductionKits sp WITH (NOLOCK) 
left join Style s WITH (NOLOCK) on s.Ukey = sp.StyleUkey
where sp.ReceiveDate is null and sp.SendDate is not null and ReasonID=''
and sp.ProductionKitsGroup IN ( SELECT MDivisionID FROM Factory  )
and sp.SendDate BETWEEN '{sendDateS}' AND '{sendDateE}' 
 
 order by FactoryID, StyleID
";
                result = DBProxy.Current.Select(conn, sqlCmd.ToString(), null, out DataTable dtTmp);
                if (!result)
                {
                    this.CallJobLogApi("PMS_ProductionKitsConfirm - DB ereor", result.ToString(), DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), false, false);
                    return null;
                }

                DataTables.Add(dtTmp);
                FtyData.Add(fty, dtTmp);

                DBProxy.Current.DefaultTimeout = 300;
            }

            return FtyData;
        }

        /// <summary>
        /// 產生Excel附件
        /// </summary>
        /// <param name="dataTables"></param>
        /// <returns></returns>
        private List<string> CreateExcel(Dictionary<string, DataTable> dataTables)
        {
            List<string> files = new List<string>();
            try
            {
                Ict.Logs.APP.LogInfo("Create XLT Start - Detail");
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\PPIC_P03.xltx"); //預先開啟excel app
                objApp.Visible = false;

                // 複製Sheet，由於本來就有一個Sheet，因此少複製一次

                int idx = 0;
                foreach (var FtyData in dataTables)
                {
                    Microsoft.Office.Interop.Excel.Worksheet worksheet1 = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1];
                    Microsoft.Office.Interop.Excel.Worksheet worksheetn = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[idx + 1];                    

                    if (idx < dataTables.Count -1)
                    {
                        worksheet1.Copy(worksheet1);
                    }

                    string fty = FtyData.Key;
                    DataTable dt = FtyData.Value;

                    worksheet1.Name = fty;

                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.Remove("Fty");
                        MyUtility.Excel.CopyToXls(dt, null, "PPIC_P03.xltx", headerRow: 2, excelApp: objApp, wSheet: worksheet1, showExcel: false, showSaveMsg: false); // 將datatable copy to excel
                        worksheetn.Cells[1, 1] = $"(PMSDB_{fty}) P03. Production Kits confirm";
                    }

                    Marshal.ReleaseComObject(worksheetn); // 釋放sheet
                    idx++;
                }

                Microsoft.Office.Interop.Excel.Worksheet no1Sheet = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1];
                no1Sheet.Activate();
                Marshal.ReleaseComObject(no1Sheet); // 釋放sheet

                Ict.Logs.APP.LogInfo("Create XLT Start - Production Kits Confirm Report");

                #region Save Excel
                string excelFile = GetName("ProductionKitsConfirm");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(excelFile);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(objApp);
                #endregion

                files.Add(excelFile);

                return files;

            }
            catch (Exception ex)
            {
                Ict.Logs.APP.LogInfo("Create Excel Error : " + ex.ToString());
                this.CallJobLogApi("PMS_ProductionKitsConfirm -Create excel ereor", ex.ToString(), DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), false, false);
                return new List<string>();
            }
        }


        /// <summary>
        /// 產生Excel附件(所有工廠合併在一個Sheet)
        /// </summary>
        /// <param name="dataTables"></param>
        /// <returns></returns>
        private List<string> CreateExcel_OneSheet(Dictionary<string, DataTable> dataTables)
        {
            List<string> files = new List<string>();
            try
            {
                Ict.Logs.APP.LogInfo("Create XLT Start - Detail");
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\PPIC_P03.xltx"); //預先開啟excel app
                
                objApp.Visible = false;

                DataTable final = dataTables.FirstOrDefault().Value.Clone();
                final.Columns.Remove("Fty");
                foreach (var keyValuePair in dataTables)
                {
                    keyValuePair.Value.Columns.Remove("Fty");
                    final.Merge(keyValuePair.Value);
                }

                MyUtility.Excel.CopyToXls(final, null, "PPIC_P03.xltx", headerRow: 2, excelApp: objApp, showExcel: false, showSaveMsg: false); // 將datatable copy to excel

                #region 產生樞紐分析表

                Excel.Worksheet sheet2 = objApp.ActiveWorkbook.Worksheets[2];
                Excel.PivotCaches pCaches = objApp.ActiveWorkbook.PivotCaches();

                // 取的要樞紐分析的資料Range
                string sourceRange = "A2:" + $"Q{final.Rows.Count + 2}";
                Excel.PivotCache pCache = pCaches.Create(Excel.XlPivotTableSourceType.xlDatabase, sourceRange);
                Excel.Range rngDes = sheet2.get_Range("A1");
                Excel.PivotTable pTable = pCache.CreatePivotTable(TableDestination: rngDes, TableName: "PivotTable");

                // 行標籤 vertical (TW Send date)
                Excel.PivotField field_V = pTable.PivotFields(6);
                field_V.Orientation = Excel.XlPivotFieldOrientation.xlRowField;
                field_V.Name = "TW Send date";
                pTable.CompactLayoutRowHeader = "TW Send date";

                // 列標籤 horizontal (FactoryID)
                Excel.PivotField field_H = pTable.PivotFields(1);
                field_H.Orientation = Excel.XlPivotFieldOrientation.xlColumnField;
                field_H.Name = "Factory";
                pTable.CompactLayoutColumnHeader = "Factory";

                // 設定要用何種方式計算, 此範例是用count(FactoryID)
                Excel.PivotField field_F = pTable.PivotFields(1);//FactoryID
                field_F.Orientation = Excel.XlPivotFieldOrientation.xlDataField;
                Excel.XlConsolidationFunction function = Excel.XlConsolidationFunction.xlCount;
                field_F.Function = function;

                // 設定ttl 顯示的名稱
                pTable.GrandTotalName = "Style Count";

                //設定邊框線
                pTable.DataBodyRange.Borders.Weight = 2;
                pTable.DataLabelRange.Borders.Weight = 2;
                pTable.ColumnRange.Borders.Weight = 2;
                pTable.RowRange.Borders.Weight = 2;

                #endregion
                Ict.Logs.APP.LogInfo("Create XLT Start - Production Kits Confirm Report");

                #region Save Excel
                string excelFile = GetName("ProductionKitsConfirm");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(excelFile);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(objApp);
                #endregion

                files.Add(excelFile);

                return files;

            }
            catch (Exception ex)
            {
                Ict.Logs.APP.LogInfo("Create Excel Error : " + ex.ToString());
                this.CallJobLogApi("PMS_ProductionKitsConfirm -Create excel ereor", ex.ToString(), DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), false, false);
                return new List<string>();
            }
        }


        #region Call JobLog web api回傳執行結果
        private void CallJobLogApi(string subject, string desc, string startDate, string endDate, bool isTest, bool succeeded)
        {
            JobLog jobLog = new JobLog()
            {
                GroupID = "P",
                SystemID = "PMS",
                Region = "TSR",
                MDivisionID = string.Empty,
                OperationName = subject,
                StartTime = startDate,
                EndTime = endDate,
                Description = desc,
                FileName = new List<string>(),
                FilePath = string.Empty,
                Succeeded = succeeded
            };
            CallTPEWebAPI callTPEWebAPI = new CallTPEWebAPI(isTest);
            callTPEWebAPI.CreateJobLogAsnc(jobLog, null);
        }

        #endregion

        public static class ExcelFileNameExtension
        {
            public const string Xlsm = ".xlsm", Xlsx = ".xlsx";
        }

        public static string GetName(string ProcessName, string NameExtension = ExcelFileNameExtension.Xlsx)
        {
            string fileName = ProcessName.Trim()
                                + DateTime.Now.ToString("_yyMMdd_HHmmssfff")
                                + NameExtension;
            return Path.Combine(Sci.Env.Cfg.ReportTempDir, fileName);
        }

        private void SetSendDate()
        {
            /*
             * 每週六寄信，時間範圍：2021/01/01 ～ 上一個周六，
             * 例如：今天為2021/03/13(六)執行此程式，資料搜尋範圍為 2021/01/01 ～ 2021/03/06，若今天是
             */

            this.SendDateS = MyUtility.Convert.GetDate("2021/01/01").Value;

            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                this.SendDateE = DateTime.Now.AddDays(-7);
            }
            else
            {
                for (int i = 1; i <= 7; i++)
                {
                    if (DateTime.Now.AddDays((-1 * i)).DayOfWeek == DayOfWeek.Saturday)
                    {
                        this.SendDateE = DateTime.Now.AddDays((-1 * i));
                    }
                }
            }
        }

        private void GetFactoryList()
        {
            this.factorys.Clear();
            if (this.chkHZG.Checked) factorys.Add("PMSDB_HZG");
            if (this.chkHXG.Checked) factorys.Add("PMSDB_HXG");
            if (this.chkSPR.Checked) factorys.Add("PMSDB_SPR");
            if (this.chkSPS.Checked) factorys.Add("PMSDB_SPS");
            if (this.chkSPT.Checked) factorys.Add("PMSDB_SPT");
            if (this.chkSNP.Checked) factorys.Add("PMSDB_SNP");
            if (this.chkESP.Checked) factorys.Add("PMSDB_ESP");
            if (this.chkPH2.Checked) factorys.Add("PMSDB_PH2");
            if (this.chkPH1.Checked) factorys.Add("PMSDB_PH1");
        }
    }
}
