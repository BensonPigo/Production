using Sci.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Net;
using System.IO;
using Sci;
using System.Diagnostics;
using PostJobLog;
using System.Configuration;

namespace ImportSewingLineSchedule.Daily
{
    public partial class Main : Sci.Win.Tems.Base
    {
        DataRow mailTo;
        TransferPms transferPMS = new TransferPms();
        StringBuilder sqlmsg = new StringBuilder();
        bool IsAuto = Convert.ToBoolean(ConfigurationManager.AppSettings["IsAuto"]);
        bool isTestJobLog = Convert.ToBoolean(ConfigurationManager.AppSettings["IsTestJobLog"]);

        bool issucess = true;
        string tpeMisMail = string.Empty;
        string OutputDate = "GETDATE()";


        public Main()
        {
            InitializeComponent();
        }
        

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            
            transferPMS.fromSystem = "PBIReportData";

            if (IsAuto)
            {
                ClickExport();
                this.Close();
            }
        }
        


        private void ClickExport()
        {
            SqlConnection conn;

            if (!Sci.SQL.GetConnection(out conn)) { return; }

            conn.InfoMessage += new SqlInfoMessageEventHandler(InfoMessage);

            DualResult result = AsyncHelper.Current.DataProcess(this, () =>
            {
                return AsyncUpdateExport(conn);
            });

            if (!result)
                mymailTo(result.GetException().ToString());
            else
                mymailTo(result.ToSimpleString());

            if (!result)
            {
                ShowErr(result);
            }

            conn.Close();
            issucess = true;
        }


        #region 接Sql Server 進度訊息用
        private void InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            BeginInvoke(() => { this.labelProgress.Text = e.Message; });
        }
        #endregion

        #region Export/Update (非同步) 執行Procedure
        private DualResult AsyncUpdateExport(SqlConnection conn)
        {
            DataTable serverdt;
            string sqlserver = string.Empty;
            if (isTestJobLog)
            {
                sqlserver = $@" select name from sys.servers WHERE Name Like 'testing%' and Name not like '%mis' ";
            }
            else
            {
                sqlserver= $@" select name from sys.servers WHERE Name Like 'PMS\pmsdb\%'";
            }

            DualResult dualResult = DBProxy.Current.Select(null, sqlserver, out serverdt);
            if (!dualResult)
            {
                return dualResult;
            }
            string date = DateTime.Now.AddDays(-15).ToString("yyyy/MM/dd");
            try
            {
                string sqlcmd = $@"Delete P_SewingLineSchedule --先刪除全部";

                foreach (DataRow item in serverdt.Rows)
                {
                    sqlcmd += $@"

--注意，Store Procedure 產生出來的欄位順序，必須與下列INSERT的順序一致
INSERT INTO [dbo].[P_SewingLineSchedule]
           ([APSNo]
           ,[SewingLineID]
           ,[Sewer]
           ,[SewingDay]
           ,[SewingStartTime]
           ,[SewingEndTime]
           ,[MDivisionID]
           ,[FactoryID]
           ,[PO]
           ,[POCount]
           ,[SP]
           ,[SPCount]
           ,[EarliestSCIdelivery]
           ,[LatestSCIdelivery]
           ,[EarliestBuyerdelivery]
           ,[LatestBuyerdelivery]
           ,[Category]
           ,[Colorway]
           ,[ColorwayCount]
           ,[CDCode]
           ,[ProductionFamilyID]
           ,[Style]
           ,[StyleCount]
           ,[OrderQty]
           ,[AlloQty]
           ,[StardardOutputPerDay]
           ,[CPU]
           ,[SewingCPU]
           ,[WorkHourPerDay]
           ,[StardardOutputPerHour]
           ,[Efficienycy]
           ,[ScheduleEfficiency]
           ,[LineEfficiency]
           ,[LearningCurve]
           ,[SewingInline]
           ,[SewingOffline]
           ,[PFRemark]
           ,[MTLComplete]
           ,[KPILETA]
           ,[MTLETA]
           ,[ArtworkType]
           ,[InspectionDate]
           ,[Remarks]
           ,[CuttingOutput]
           ,[SewingOutput]
           ,[ScannedQty]
           ,[ClogQty])
SELECT *  FROM OPENQUERY ([{item["name"]}],'SET FMTONLY OFF; exec production.dbo.GetSewingLineScheduleData ''{date}''')
";
                }
                   
                SqlCommand cmd = new SqlCommand(sqlcmd, conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 7200;  //12分鐘
                cmd.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                issucess = false;
                return Ict.Result.F(se);
            }
            return Ict.Result.True;
        }
        #endregion


        private void mymailTo(string ErrorMsg)
        {
            String subject = "";
            String desc = "";
            string strDate = DateTime.Now.ToString("yyyy/MM/dd");

            #region 組合 Desc
            if (!MyUtility.Check.Empty(ErrorMsg))
            {
                issucess = false;
            }

            desc = $@"Please check below information.
Transfer date:{ strDate }
Stored Procedure: GetSewingLineScheduleData
";
            #endregion

            subject = "Import SewingLineSchedule";
            if (issucess)
            {
                subject += " Success";
                desc += Environment.NewLine + "Success";
            }
            else
            {
                subject += " failed";
            }
            if (!issucess)
            {
                SendMail(subject, desc, !issucess);
            }
            desc += Environment.NewLine + ErrorMsg;
            this.CallJobLogApi(subject, desc, DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), isTestJobLog, issucess);
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ClickExport();
        }


        private void btnTestWebAPI_Click(object sender, EventArgs e)
        {
            this.CallJobLogApi("Import SewingLineSchedule to Power BI", "Import SewingLineSchedule to Power BI Test", DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), true, true);
        }

        private void btnTestMail_Click(object sender, EventArgs e)
        {
            SendMail("Import SewingLineSchedule", "Import SewingLineSchedule Test mail.");
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

        
        private void SendMail(String subject = "", String desc = "", bool isFail = true)
        {
            String mailServer = ConfigurationManager.AppSettings["mailserver_ip"];//"Mail.sportscity.com.tw";
            String eMailID = ConfigurationManager.AppSettings["mailserver_account"]; //"foxpro";
            String eMailPwd = ConfigurationManager.AppSettings["mailserver_password"]; //"orpxof";
            ushort? SmtpPort = (ushort?)Convert.ToInt32(ConfigurationManager.AppSettings["mailserver_port"]); //25;
            transferPMS.SetSMTP(mailServer, SmtpPort, eMailID, eMailPwd);

            String sendFrom = ConfigurationManager.AppSettings["MailSendFrom"];

            //string toAddress = "pmshelp@sportscity.com.tw";
            string toAddress = ConfigurationManager.AppSettings["MailSendTo"]; 
            
            
            if (!MyUtility.Check.Empty(toAddress))
            {
                Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(sendFrom, toAddress, "", subject, "", desc, true, true);
                mail.ShowDialog();
            }
        }
        

    }
}
