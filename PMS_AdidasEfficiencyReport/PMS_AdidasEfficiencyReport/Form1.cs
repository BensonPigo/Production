using Ict;
using Sci;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;

namespace AdidasEfficiencyReport
{
    public partial class Form1 : Sci.Win.Tems.Input7
    {
        private string mailFrom = "foxpro@sportscity.com.tw";
        private string mailServer = "172.17.2.8";
        private string eMailID = "foxpro";
        private string eMailPwd = "orpxof";
        private string[] mailTO = ConfigurationManager.AppSettings["MailList"].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        private string[] mailTOCC = ConfigurationManager.AppSettings["MailListCC"].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

        bool isAuto = false;
        private List<string> factorys = new List<string>();
        private DateTime OutputDateS, OutputDateE;
        private DataTable printData;
        public Form1()
        {
            InitializeComponent();
            isAuto = false;
        }

        public Form1(string _isAuto)
        {
            InitializeComponent();
            isAuto = !_isAuto.Empty();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.EditMode = true;
            Ict.Logs.APP.LogInfo("AdidasEfficiencyReport Start IsAuto : " + isAuto.ToString());

            /*
             * 每週五：產生本週Output Date週一~Output Date周三的資料
             * 每週二：產生上週Output Date週四~Output Date周六的資料
             */
            if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
            {
                this.OutputDateS = DateTime.Now.AddDays(-5);
                this.OutputDateE = DateTime.Now.AddDays(-3);
            }
            else if(DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                this.OutputDateS = DateTime.Now.AddDays(-4);
                this.OutputDateE = DateTime.Now.AddDays(-2);
            }

            Ict.Logs.APP.LogInfo("OutputDateS : " + OutputDateS.ToString("yyyy/MM/dd") + ", OutputDateE : " + OutputDateE.ToString("yyyy/MM/dd"));

            this.GetMailSetting();

            // PMSDB所有工廠當天還原都有成功在執行(PMS&MES都要檢查)，若沒有則不執行
            if (isAuto)
            {
                DualResult result = QueryPMSDB();
                if (!result)
                {
                    Ict.Logs.APP.LogInfo("Query PMSDB Error : " + result.ToString());
                    this.Close();
                }

                result = Query();
                if (!result)
                {
                    Ict.Logs.APP.LogInfo("Query Error : " + result.ToString());
                    this.Close();
                }

                // SendMail & Create Excel
                this.CreateXLTandSend();

                this.Close();
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

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            this.GetFactoryList();

            DualResult result = Query();
            if (!result)
            {
                Ict.Logs.APP.LogInfo("Query Error : " + result.ToString());

                this.ShowErr(result);
            }

            // SendMail & Create Excel
            this.CreateXLTandSend();
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

            sql = string.Format(@"
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
	 group by s.Region, s.FilePath
)s
group by s.Region
having count(*) = 2");

            result = DBProxy.Current.Select("MIS", sql, out dt);
            if (!result)
            {
                return result;
            }

            result = new DualResult(false);
            if (dt.Rows.Count == 10)
            {
                result = new DualResult(true);
            }

            return result;
        }

        private DualResult Query()
        {
            DualResult result = new DualResult(false);
            string sqlCmd;
            if (this.factorys.Count == 0)
            {
                return result;
            }

            DBProxy.Current.DefaultTimeout = 1800;
            foreach (string conn in this.factorys)
            {
                sqlCmd = string.Format(
                    "exec dbo.GetAdidasEfficiencyReport '{0}', '{1}', '{2}', '{3}', '{4}', '{5}'",
                        this.OutputDateS.ToString("yyyy/MM/dd"),
                        this.OutputDateE.ToString("yyyy/MM/dd"),
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty);
                result = DBProxy.Current.Select(conn, sqlCmd.ToString(), null, out DataTable dt);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

                if (this.printData == null)
                {
                    this.printData = dt;
                }
                else
                {
                    this.printData.Merge(dt);
                }
            }

            if (this.printData != null && this.printData.Rows.Count > 0)
            {
                sqlCmd = @"
select t.OutputDate
	, t.FactoryID
	, t.SewingLineID
	, t.Shift
	, t.Category
	, t.StyleID
	, t.Manpower
	, t.ManHour
	, t.TotalOutput
	, t.CD
	, t.SeasonID
	, t.BrandID
	, t.Fabrication
	, t.ProductGroup
	, t.ProductFabrication
	, [GSD] = iif(isnull(sl.Rate, 0) = 0 or isnull(sq.TMS, 0) = 0, 0, (sq.TMS / 60) * (sl.Rate / 100))
	, t.Earnedhours
	, t.TotalWorkingHours
	, t.CumulateDaysofDaysinProduction
	, t.EfficiencyLine
	, t.NoofInlineDefects
	, t.NoofEndlineDefectiveGarments
	, t.WFT
from #tmp t
left join Style s on t.StyleID = s.Id and t.BrandID = s.BrandID and t.SeasonID = s.SeasonID
left join Style_Quotation sq on s.Ukey = sq.StyleUkey and sq.ArtworkTypeID = 'SEWING' and sq.Article = ''
left join Style_Location sl on s.Ukey = sl.StyleUkey and RIGHT(t.CD, 1) = sl.Location";

                DBProxy.Current.OpenConnection("Trade", out SqlConnection sqlConnection);
                result = MyUtility.Tool.ProcessWithDatatable(this.printData, null, sqlCmd, out this.printData, conn: sqlConnection);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }
            }

            DBProxy.Current.DefaultTimeout = 300;
            return result;
        }

        private void GetFactoryList()
        {
            if (this.chkPH1.Checked) factorys.Add("PMSDB_PH1");
            if (this.chkPH2.Checked) factorys.Add("PMSDB_PH2");
            if (this.chkESP.Checked) factorys.Add("PMSDB_ESP");
            if (this.chkSNP.Checked) factorys.Add("PMSDB_SNP");
            if (this.chkSPT.Checked) factorys.Add("PMSDB_SPT");
            if (this.chkSPS.Checked) factorys.Add("PMSDB_SPS");
            if (this.chkSPR.Checked) factorys.Add("PMSDB_SPR");
            if (this.chkHXG.Checked) factorys.Add("PMSDB_HXG");
            if (this.chkHZG.Checked) factorys.Add("PMSDB_HZG");
            if (this.chkNAI.Checked) factorys.Add("PMSDB_NAI");
            if (this.chkTSR.Checked) factorys.Add("PMSDB_TSR");
        }

        private void CreateXLTandSend()
        {
            try
            {
                if (this.printData == null || this.printData.Rows.Count == 0)
                {
                    Ict.Logs.APP.LogInfo("CreateXLTandSend Error : No Data Found");
                    return;
                }
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Planning_R07.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(this.printData, null, "Planning_R07.xltx", headerRow: 1, excelApp: objApp, showExcel: false, showSaveMsg: false);//將datatable copy to excel

                Ict.Logs.APP.LogInfo("Create XLT Start");

                #region Save Excel
                string excelFile = GetName("AdidasEfficiencyReport");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(excelFile);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                #endregion

                Ict.Logs.APP.LogInfo("Create XLT End, Send Mail Start");

                #region Sned Mail
                string subject = "Adidas Efficiency Report " + this.OutputDateS.ToString("yyyyMMdd") + " ~ " + this.OutputDateE.ToString("yyyyMMdd");
                string desc = "(**Please don't reply this mail. **)";
                this.MailToHtml(subject, excelFile, desc);
                #endregion

                Ict.Logs.APP.LogInfo("Send Mail End");
            }
            catch (Exception ex)
            {
                Ict.Logs.APP.LogInfo("CreateXLTandSend Error : " + ex.ToString());
            }
        }

        private void MailToHtml(string subject, string attach, string desc)
        {
            //寄件者 & 收件者
            MailMessage message = new MailMessage();
            message.Subject = subject;
            message.From = new MailAddress(this.mailFrom);

            foreach (string mail in mailTO)
            {
                message.To.Add(mail);
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
            string filePath = attach;
            if (!filePath.Equals(string.Empty))
            {
                Attachment attachFile = new Attachment(filePath);
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
    }
}
