using Ict;
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
        private string[] settingFactoryList = ConfigurationManager.AppSettings["SettingFactoryList"].Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        private DateTime? StartTime, EndTime;

        bool isAuto = false;
        private List<string> factorys = new List<string>();
        private DateTime OutputDateS, OutputDateE;
        private DataTable printData;
        private DataTable printDataSintex;
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
            bool bolSintexEffReportCompare = false;
            Ict.Logs.APP.LogInfo("AdidasEfficiencyReport Start IsAuto : " + isAuto.ToString());
            StartTime = DateTime.Now;

            /*
             * 每週二 ~ 每周四：產生上週Output Date週四~Output Date周六的資料  + 當年度報表
             * 其他：產生本週Output Date週一~Output Date周三的資料
             */
            if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday || DateTime.Now.DayOfWeek == DayOfWeek.Wednesday || DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
            {
                this.OutputDateS = DateTime.Now.AddDays(-5);
                this.OutputDateE = DateTime.Now.AddDays(-3);
                bolSintexEffReportCompare = true;
            }
            else
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
                    this.writeJobLog(false, result.ToString());
                    return;
                }

                result = Query(false, out printData);
                if (!result)
                {
                    Ict.Logs.APP.LogInfo("Query Error : " + result.ToString());
                    this.writeJobLog(false, result.ToString());
                    return;
                }

                if (bolSintexEffReportCompare)
                {
                    result = Query(true, out printDataSintex);
                    if (!result)
                    {
                        Ict.Logs.APP.LogInfo("Query Error : " + result.ToString());
                        this.writeJobLog(false, result.ToString());
                        return;
                    }
                }

                DataTable[] dataTables = this.OrganizeData(printData, printDataSintex);

                List<string> files = this.CreateXML(dataTables);

                this.SendMail(files);
                // SendMail & Create Excel
                //this.CreateXLTandSend();
                this.writeJobLog(true, "Adidas Efficiency Report is complete");
                return;
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
            bool bolSintexEffReportCompare = this.chkSintexEfficiencyReport.Checked;

            //this.OutputDateS = DateTime.Now.AddDays(-30);
            //this.OutputDateE = DateTime.Now.AddDays(-20);

            DualResult result = Query(false, out printData);
            if (!result)
            {
                Ict.Logs.APP.LogInfo("Query Error : " + result.ToString());
                this.writeJobLog(false, result.ToString());
                return;
            }

            if (bolSintexEffReportCompare)
            {
                result = Query(true, out printDataSintex);
                if (!result)
                {
                    Ict.Logs.APP.LogInfo("Query Error : " + result.ToString());
                    this.writeJobLog(false, result.ToString());
                    return;
                }
            }

            DataTable[] dataTables = this.OrganizeData(printData, printDataSintex);

            List<string> files = this.CreateXML(dataTables);

            this.SendMail(files);
            this.writeJobLog(true, "Adidas Efficiency Report is complete");
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
     and s.Region in ('{0}')
	 group by s.Region, s.FilePath
)s
group by s.Region
having count(*) = 2", sqlwhere);

            result = DBProxy.Current.Select("MIS", sql, out dt);
            if (!result)
            {
                return result;
            }

            List<string> RestoreDB = dt.AsEnumerable().Select(x => x["Region"].ToString()).Distinct().ToList();
            string strRestorDB = string.Empty;
            if (RestoreDB.Any())
            {
                strRestorDB = string.Join(",", RestoreDB);
            }

            if (dt.Rows.Count == newfactoryList.Count)
            {
                result = new DualResult(true);
            }
            else
            {
                result = new DualResult(false, $@"PMS資料尚未還原到台北 , RestoreDB :{strRestorDB} , Checked Fty :{sqlwhere}");
            }

            return result;
        }

        private DualResult Query(bool bolSintexEffReportCompare, out DataTable dt)
        {
            dt = new DataTable();
            DualResult result = new DualResult(false);
            string sqlCmd;
            if (isAuto)
            {
                this.factorys = settingFactoryList.ToList();
            }

            if (this.factorys.Count == 0)
            {
                return result = new DualResult(false, "settingFactoryList :" + settingFactoryList.ToList().Count.ToString()); ;
            }

            string outPutDateS = bolSintexEffReportCompare ? DateTime.Now.Year + "/01/01" : this.OutputDateS.ToString("yyyy/MM/dd");
            string outPutDateE = bolSintexEffReportCompare ? DateTime.Now.Year + "/12/31" : this.OutputDateE.ToString("yyyy/MM/dd");
            int sintexEffReportCompare = bolSintexEffReportCompare ? 1 : 0;
            DBProxy.Current.DefaultTimeout = 1800;
            foreach (string conn in this.factorys)
            {
                sqlCmd = string.Format(
                    "exec dbo.GetAdidasEfficiencyReport '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', {7}",
                        outPutDateS,
                        outPutDateE,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        "ADIDAS,REEBOK",
                        sintexEffReportCompare);
                result = DBProxy.Current.Select(conn, sqlCmd.ToString(), null, out DataTable dtTmp);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    dt = dtTmp;
                }
                else
                {
                    dt.Merge(dtTmp);
                }
            }

            if (dt != null && dt.Rows.Count > 0)
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
	, [GSD] = iif(isnull(sl.Rate, 0) = 0 or isnull(sq.TMS, 0) = 0, t.[GSD], (sq.TMS / 60) * (sl.Rate / 100))
	, [Earnedhours] = iif(isnull(sl.Rate, 0) = 0 or isnull(sq.TMS, 0) = 0 or isnull(t.TotalOutput, 0) = 0, t.[Earnedhours], (sq.TMS / 60) * (sl.Rate / 100) * t.TotalOutput / 60)
	, t.TotalWorkingHours
	, t.CumulateDaysofDaysinProduction
    , t.EfficiencyLine
	, t.GSDProsmv
	, t.Earnedhours2
	, t.EfficiencyLine2
	, t.NoofInlineDefects
	, t.NoofEndlineDefectiveGarments
	, t.WFT
	, t.Country
	, t.[Month]
	, [IsGSDPro] = iif(isnull(sl.Rate, 0) = 0 or isnull(sq.TMS, 0) = 0, t.IsGSDPro, '')
	, t.Orderseq
from #tmp t
left join Style s on t.StyleID = s.Id and t.BrandID = s.BrandID and t.SeasonID = s.SeasonID
left join Style_Location sl on s.Ukey = sl.StyleUkey and RIGHT(t.CD, 1) = sl.Location
outer apply (
	select TMS = sum(sq.TMS)
	from Style_Quotation sq
	where s.Ukey = sq.StyleUkey and sq.Article = ''
	and sq.ArtworkTypeID in ('SEWING', 'PRESSING', 'PACKING', 'Seamseal', 'Ultrasonic')
)sq
";


                DBProxy.Current.OpenConnection("Trade", out SqlConnection sqlConnection);
                result = MyUtility.Tool.ProcessWithDatatable(dt, null, sqlCmd, out dt, conn: sqlConnection);
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

        private List<string> CreateXML(DataTable[] dataTables)
        {
            List<string> files = new List<string>();
            try
            {
                if (dataTables[0].Rows.Count > 0)
                {
                    Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Planning_R07.xltx"); //預先開啟excel app
                    Excel.Worksheet objSheets1 = objApp.ActiveWorkbook.Worksheets[1];   // Detail
                    Excel.Worksheet objSheets2 = objApp.ActiveWorkbook.Worksheets[2];   // Sintex Eff Report Compare
                    Excel.Worksheet objSheets3 = objApp.ActiveWorkbook.Worksheets[3];   // Adidas data

                    Ict.Logs.APP.LogInfo("Create XLT Start - Detail");
                    this.SetExcelSheet1(objApp, objSheets1, dataTables[0]);
                    objSheets2.Visible = Excel.XlSheetVisibility.xlSheetHidden;

                    string excelFile = GetName("AdidasEfficiencyReport");
                    Excel.Workbook workbook = objApp.ActiveWorkbook;
                    workbook.SaveAs(excelFile);
                    workbook.Close();
                    objApp.Quit();
                    Marshal.ReleaseComObject(objApp);
                    Marshal.ReleaseComObject(objSheets1);
                    Marshal.ReleaseComObject(objSheets2);
                    Marshal.ReleaseComObject(objSheets3);
                    Marshal.ReleaseComObject(workbook);

                    // Add Files
                    files.Add(excelFile);
                }

                if (dataTables[1] != null && dataTables[1].Rows.Count > 0)
                {
                    Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Planning_R07.xltx"); //預先開啟excel app
                    Excel.Worksheet objSheets1 = objApp.ActiveWorkbook.Worksheets[1];   // Detail
                    Excel.Worksheet objSheets2 = objApp.ActiveWorkbook.Worksheets[2];   // Sintex Eff Report Compare
                    Excel.Worksheet objSheets3 = objApp.ActiveWorkbook.Worksheets[3];   // Adidas data

                    Ict.Logs.APP.LogInfo("Create XLT Start - Sintex Efficiency Report");
                    this.SetExcelSheet2(objSheets2, dataTables);
                    objSheets1.Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    objSheets3.Visible = Excel.XlSheetVisibility.xlSheetHidden;

                    string excelFile = GetName("SintexEfficiencyReport");
                    Excel.Workbook workbook = objApp.ActiveWorkbook;
                    workbook.SaveAs(excelFile);
                    workbook.Close();
                    objApp.Quit();
                    Marshal.ReleaseComObject(objApp);
                    Marshal.ReleaseComObject(objSheets1);
                    Marshal.ReleaseComObject(objSheets2);
                    Marshal.ReleaseComObject(objSheets3);
                    Marshal.ReleaseComObject(workbook);

                    // Add Files
                    files.Add(excelFile);
                }

                return files;

            }
            catch (Exception ex)
            {
                Ict.Logs.APP.LogInfo("CreateXML Error : " + ex.ToString());
                return files;
            }
        }

        private void SendMail(List<string> files)
        {
            Ict.Logs.APP.LogInfo("Send Mail Start");

            #region Sned Mail
            string subject = "Adidas Efficiency Report " + this.OutputDateS.ToString("yyyyMMdd") + " ~ " + this.OutputDateE.ToString("yyyyMMdd");
            string desc = "(**Please don't reply this mail. **)";
            this.MailToHtml(subject, files, desc);
            #endregion

            Ict.Logs.APP.LogInfo("Send Mail End");
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
                this.MailToHtml(subject,new List<string> { excelFile }, desc);
                #endregion

                Ict.Logs.APP.LogInfo("Send Mail End");
            }
            catch (Exception ex)
            {
                Ict.Logs.APP.LogInfo("CreateXLTandSend Error : " + ex.ToString());
            }
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

        public static DataTable ToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        private DataTable[] OrganizeData(DataTable dt, DataTable dtSintex)
        {
            DataTable[] rtnDataTable = new DataTable[4];

            if (dt != null && dt.Rows.Count > 0)
            {
                var queryDt = dt.AsEnumerable()
                .OrderBy(x => x.Field<DateTime?>("OutputDate"))
                .ThenBy(x => x.Field<string>("FactoryID") + x.Field<string>("SewingLineID") + x.Field<string>("SeasonID") + x.Field<string>("BrandID") + x.Field<string>("StyleID"))
                .Select((x, index) => new
                {
                    OutputDate = x.Field<DateTime?>("OutputDate"),
                    FactoryID = x.Field<string>("FactoryID"),
                    SewingLineID = x.Field<string>("SewingLineID"),
                    Shift = x.Field<string>("Shift"),
                    Category = x.Field<string>("Category"),
                    StyleID = x.Field<string>("StyleID"),
                    Manpower = x.Field<decimal?>("Manpower"),
                    ManHour = x.Field<decimal?>("ManHour"),
                    TotalOutput = x.Field<int>("TotalOutput"),
                    CD = x.Field<string>("CD"),
                    SeasonID = x.Field<string>("SeasonID"),
                    BrandID = x.Field<string>("BrandID"),
                    Fabrication = string.Format("=IFERROR(VLOOKUP(LEFT(J{0}, 2),'Adidas data '!$A$2:$G$116, 4, FALSE), \"\")", index + 2),
                    ProductGroup = string.Format("=IFERROR(VLOOKUP(LEFT(J{0}, 2),'Adidas data '!$A$2:$G$116, 7, FALSE), \"\")", index + 2),
                    ProductFabrication = string.Format("=N{0}&M{0}", index + 2),
                    GSD = x.Field<decimal?>("GSD"),
                    Earnedhours = string.Format("=IF(I{0}=\"\", \"\", IFERROR((I{0}*P{0})/60, \"\"))", index + 2),
                    TotalWorkingHours = string.Format("=H{0}*G{0}", index + 2),
                    CumulateDaysofDaysinProduction = x.Field<int>("CumulateDaysofDaysinProduction"),
                    EfficiencyLine = string.Format("=Q{0}/R{0}", index + 2),
                    GSDProsmv = x.Field<decimal?>("GSDProsmv"),
                    Earnedhours2 = string.Format("=IF(I{0}=\"\", \"\", IFERROR(I{0}*U{0}/60, \"\"))", index + 2),
                    EfficiencyLine2 = string.Format("=V{0}/R{0}", index + 2),
                    NoofInlineDefects = x.Field<int>("NoofInlineDefects"),
                    NoofEndlineDefectiveGarments = x.Field<int>("NoofEndlineDefectiveGarments"),
                    WFT = string.Format("=IFERROR((X{0}+Y{0})/I{0}, \"\")", index + 2),
                    Country = x.Field<string>("Country"),
                    Month = x.Field<string>("Month"),
                    IsGSDPro = x.Field<string>("IsGSDPro"),
                    Orderseq = x.Field<int>("Orderseq"),
                })
                .ToList();

                rtnDataTable[0] = ToDataTable(queryDt);
            }


            if (dtSintex != null && dtSintex.Rows.Count > 0)
            {
                var querySintexReportMonth = dtSintex.AsEnumerable()
                                    .OrderBy(x => x.Field<int>("Orderseq"))
                                    .ThenBy(x => x.Field<DateTime?>("OutputDate"))
                                    .GroupBy(x => new { Country = x.Field<string>("Country"), Month = x.Field<string>("Month"), Orderseq = x.Field<int>("Orderseq") })
                                    .Select((x, index) => new
                                    {
                                        x.Key.Country,
                                        x.Key.Month,
                                        x.Key.Orderseq,
                                        PROEarnedHrs = x.Sum(y => y.Field<decimal>("Earnedhours2")),
                                        PROManhours = x.Sum(y => y.Field<decimal>("TotalWorkingHours")),
                                        SIOEarnedHrs = x.Sum(y => y.Field<decimal>("Earnedhours")),
                                        SIOManhours = x.Sum(y => y.Field<decimal>("TotalWorkingHours")),
                                        SamplesSIO = x.Sum(y => y.Field<decimal>("Earnedhours")) == 0 ? 0 :x.Where(y => y.Field<string>("IsGSDPro").EqualString("V")).Sum(y => y.Field<int>("TotalOutput") * y.Field<decimal>("GSD") / 60) /
                                                     x.Sum(y => y.Field<decimal>("Earnedhours")),
                                    })
                                    .ToList();

                rtnDataTable[1] = ToDataTable(querySintexReportMonth);

                var querySintexReportSeason = dtSintex.AsEnumerable()
                    .GroupBy(x => new { Country = x.Field<string>("Country"), SeasonID = x.Field<string>("SeasonID"), Orderseq = x.Field<int>("Orderseq") })
                    .Select((x, index) => new
                    {
                        x.Key.Country,
                        x.Key.SeasonID,
                        x.Key.Orderseq,
                        ProEff = x.Sum(y => y.Field<decimal>("TotalWorkingHours")) == 0 ? 0 : x.Sum(y => y.Field<decimal>("Earnedhours2")) / x.Sum(y => y.Field<decimal>("TotalWorkingHours")),
                        SIOEff = x.Sum(y => y.Field<decimal>("TotalWorkingHours")) == 0 ? 0 : x.Sum(y => y.Field<decimal>("Earnedhours")) / x.Sum(y => y.Field<decimal>("TotalWorkingHours")),
                    })
                    .OrderBy(x => x.Country)
                    .ThenBy(x => x.SeasonID)
                    .ToList();

                rtnDataTable[2] = ToDataTable(querySintexReportSeason);

                var querySintexReportMonthByYTD = dtSintex.AsEnumerable()
                        .GroupBy(x => new { Country = x.Field<string>("Country") })
                        .Select(x => new
                        {
                            x.Key.Country,
                            SamplesSIO = x.Sum(y => y.Field<decimal>("Earnedhours")) == 0 ? 0 : x.Where(y => y.Field<string>("IsGSDPro").EqualString("V")).Sum(y => y.Field<int>("TotalOutput") * y.Field<decimal>("GSD") / 60) /
                                         x.Sum(y => y.Field<decimal>("Earnedhours")),
                        })
                        .ToList();

                rtnDataTable[3] = ToDataTable(querySintexReportMonthByYTD);
            }

            return rtnDataTable;
        }

        private void SetExcelSheet1(Excel.Application objApp, Excel.Worksheet objSheets, DataTable dt)
        {
            int cMax = 100000;
            for (int i = 0; i <= dt.Rows.Count / cMax; i++)
            {
                int cSkip = cMax * i;
                int cTake = i == 0 ? i : cSkip;
                MyUtility.Excel.CopyToXls(
                    dt.AsEnumerable().Skip(cSkip).Take(cMax).CopyToDataTable(),
                    null,
                    "Planning_R07.xltx",
                    headerRow: cTake + 1,
                    excelApp: objApp,
                    showExcel: false,
                    showSaveMsg: false,
                    wSheet: objSheets);
            }

            objSheets.get_Range("AA:AD").EntireColumn.Hidden = true;
        }

        private void SetExcelSheet2(Excel.Worksheet objSheets, DataTable[] dt)
        {
            List<string> countrys = dt[2].AsEnumerable()
                            .GroupBy(x => new { Country = x.Field<string>("Country"), OrderBySeq = x.Field<int>("Orderseq") })
                            .OrderBy(x => x.Key.OrderBySeq)
                            .Select(x => x.Key.Country)
                            .ToList();

            List<string> sessions = dt[2].AsEnumerable()
                            .GroupBy(x => new { SeasonID = x.Field<string>("SeasonID") })
                            .OrderBy(x => x.Key.SeasonID)
                            .Select(x => x.Key.SeasonID)
                            .ToList();

            #region 上半部

            for (int i = 1; i < sessions.Count; i++)
            {
                Excel.Range r = objSheets.get_Range($"A3", Type.Missing).EntireRow;
                r.Copy();
                r.Insert(Excel.XlInsertShiftDirection.xlShiftDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow); // 新增Row
            }

            for (int i = 0; i <= sessions.Count - 1; i++)
            {
                objSheets.Cells[i + 3, 2] = sessions[i];
            }

            for (int i = 0; i <= countrys.Count - 1; i++)
            {
                objSheets.Cells[1, (i + 1) * 3] = countrys[i];
                objSheets.get_Range(string.Format("{0}1:{1}1", MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3), MyUtility.Excel.ConvertNumericToExcelColumn(((i + 1) * 3) + 2))).Merge();

                object[,] objArrayTop = new object[1, 3];
                objArrayTop[0, 0] = "PRO Eff.";
                objArrayTop[0, 1] = "SIO Eff.";
                objArrayTop[0, 2] = "PRO/SIO Gap";
                objSheets.Range[string.Format("{0}2:{1}2", MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3), MyUtility.Excel.ConvertNumericToExcelColumn(((i + 1) * 3) + 2))].Value2 = objArrayTop;

                objSheets.get_Range(string.Format(
                        "{0}1:{1}{2}",
                        MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3),
                        MyUtility.Excel.ConvertNumericToExcelColumn(((i + 1) * 3) + 2),
                        sessions.Count + 2)).Interior.ColorIndex = this.SetExcelColor(countrys[i]);

                objSheets.get_Range(string.Format(
                        "{0}1:{1}{2}",
                        MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3),
                        MyUtility.Excel.ConvertNumericToExcelColumn(((i + 1) * 3) + 2),
                        sessions.Count + 2)).Font.Name = "Segoe UI";

                objSheets.get_Range(string.Format(
                        "{0}1:{1}{2}",
                        MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3),
                        MyUtility.Excel.ConvertNumericToExcelColumn(((i + 1) * 3) + 2),
                        sessions.Count + 2)).Font.Bold = true;

                objSheets.get_Range(string.Format(
                        "{0}1:{1}{2}",
                        MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3),
                        MyUtility.Excel.ConvertNumericToExcelColumn(((i + 1) * 3) + 2),
                        sessions.Count + 2)).Borders.LineStyle = 1;

                for (int j = 0; j <= sessions.Count - 1; j++)
                {
                    var querySessionRow = dt[1].AsEnumerable()
                        .Where(x => x.Field<string>("Country").EqualString(countrys[i]) &&
                                    x.Field<string>("SeasonID").EqualString(sessions[j]))
                        .Select(x => new
                        {
                            ProEff = x.Field<decimal?>("ProEff"),
                            SIOEff = x.Field<decimal?>("SIOEff"),
                        })
                        .FirstOrDefault();

                    objArrayTop = new object[1, 3];
                    objArrayTop[0, 0] = querySessionRow != null && querySessionRow.ProEff.HasValue ? querySessionRow.ProEff : 0;
                    objArrayTop[0, 1] = querySessionRow != null && querySessionRow.SIOEff.HasValue ? querySessionRow.SIOEff : 0;
                    objArrayTop[0, 2] = string.Format("=IFERROR({0}{2}-{1}{2}, \"\")", MyUtility.Excel.ConvertNumericToExcelColumn(((i + 1) * 3) + 1), MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3), j + 3);
                    objSheets.Range[string.Format("{0}{2}:{1}{2}", MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3), MyUtility.Excel.ConvertNumericToExcelColumn(((i + 1) * 3) + 2), j + 3)].Value2 = objArrayTop;
                    objSheets.Range[string.Format("{0}{2}:{1}{2}", MyUtility.Excel.ConvertNumericToExcelColumn((i + 1) * 3), MyUtility.Excel.ConvertNumericToExcelColumn(((i + 1) * 3) + 2), j + 3)].NumberFormat = "##.##%";
                }
            }

            #endregion

            #region 下半部
            int initR = sessions.Count + 6;
            int initC = 2;
            int initI = 0;
            string initCountry = string.Empty;
            object[,] objArray = new object[1, 10];
            foreach (DataRow dr in dt[0].Rows)
            {
                if (initCountry.Empty())
                {
                    initCountry = dr["Country"].ToString();
                }

                objArray[0, 0] = dr["Country"].ToString();
                objArray[0, 1] = dr["Month"].ToString();
                objArray[0, 2] = dr["PROEarnedHrs"].ToString();
                objArray[0, 3] = dr["PROManhours"].ToString();
                objArray[0, 4] = string.Format("=D{0}/E{0}", initR);
                objArray[0, 5] = dr["SIOEarnedHrs"].ToString();
                objArray[0, 6] = dr["SIOManhours"].ToString();
                objArray[0, 7] = string.Format("=G{0}/H{0}", initR);
                objArray[0, 8] = string.Format("=I{0}-F{0}", initR);
                objArray[0, 9] = dr["SamplesSIO"].ToString();
                objSheets.Range[string.Format("B{0}:K{0}", initR)].Value2 = objArray;
                objSheets.get_Range(string.Format("B{0}:C{0}", initR)).Interior.ColorIndex = this.SetExcelColor(dr["Country"].ToString());
                objSheets.get_Range(string.Format("B{0}:K{0}", initR)).Borders.LineStyle = 1;
                initR++;
                initI++;

                if (initI >= dt[0].Rows.Count || !initCountry.EqualString(dt[0].Rows[initI]["Country"]))
                {
                    int rcount = dt[0].AsEnumerable().Where(x => x.Field<string>("Country").EqualString(dr["Country"].ToString())).Count();
                    DataRow drYTD = dt[2].AsEnumerable().Where(x => x.Field<string>("Country").EqualString(dr["Country"].ToString())).FirstOrDefault();

                    objArray[0, 0] = dr["Country"].ToString();
                    objArray[0, 1] = "YTD";
                    objArray[0, 2] = string.Format("=Sum({0}{1}:{0}{2})", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 2), initR - rcount, initR - 1);
                    objArray[0, 3] = string.Format("=Sum({0}{1}:{0}{2})", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 3), initR - rcount, initR - 1);
                    objArray[0, 4] = string.Format("=IFERROR(ROUND({0}{2}/{1}{2}, 2), \"\")", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 2), MyUtility.Excel.ConvertNumericToExcelColumn(initC + 3), initR);
                    objArray[0, 5] = string.Format("=Sum({0}{1}:{0}{2})", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 5), initR - rcount, initR - 1);
                    objArray[0, 6] = string.Format("=Sum({0}{1}:{0}{2})", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 6), initR - rcount, initR - 1);
                    objArray[0, 7] = string.Format("=IFERROR(ROUND({0}{2}/{1}{2}, 2), \"\")", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 5), MyUtility.Excel.ConvertNumericToExcelColumn(initC + 6), initR);
                    objArray[0, 8] = string.Format("=IFERROR({0}{2}-{1}{2}, \"\")", MyUtility.Excel.ConvertNumericToExcelColumn(initC + 7), MyUtility.Excel.ConvertNumericToExcelColumn(initC + 4), initR);
                    objArray[0, 9] = drYTD["SamplesSIO"].ToString();
                    objSheets.Range[string.Format("B{0}:K{0}", initR)].Value2 = objArray;
                    objSheets.get_Range(string.Format("B{0}:C{0}", initR)).Interior.ColorIndex = this.SetExcelColor(dr["Country"].ToString());
                    objSheets.get_Range(string.Format("B{0}:C{0}", initR)).Font.Bold = true;
                    objSheets.get_Range(string.Format("B{0}:K{0}", initR)).Borders.LineStyle = 1;
                    initCountry = initI >= dt[0].Rows.Count ? dr["Country"].ToString() : dt[0].Rows[initI]["Country"].ToString();
                    initR++;
                }
            }

            #endregion
        }

        private int SetExcelColor(string country)
        {
            int rtnVal = 1;
            switch (country.ToUpper())
            {
                case "PHILIPPINES":
                    rtnVal = 17;
                    break;
                case "VIETNAM":
                    rtnVal = 16;
                    break;
                case "CAMBODIA":
                    rtnVal = 35;
                    break;
                case "CHINA":
                    rtnVal = 45;
                    break;
            }

            return rtnVal;
        }

        private void writeJobLog(bool isSucceed, string desc)
        {
            EndTime = DateTime.Now;

            this.CallJobLogApi("PMS Adidas Efficiency Report", desc, ((DateTime)StartTime).ToString("yyyyMMdd HH:mm:ss"), ((DateTime)EndTime).ToString("yyyyMMdd HH:mm:ss"), isTest: false, isSucceed);
        }

        private void CallJobLogApi(string subject, string desc, string startDate, string endDate, bool isTest, bool succeeded)
        {
            JobLog jobLog = new JobLog()
            {
                GroupID = "P",
                SystemID = "PMS",
                Region = "TPE",
                MDivisionID = "TPE",
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
    }
}
