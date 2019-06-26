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

namespace ImportSewingDailyOutput.Daily
{
    public partial class Main : Sci.Win.Tems.Base
    {
        bool isAuto = false;
        DataRow mailTo;
        TransferPms transferPMS = new TransferPms();
        StringBuilder sqlmsg = new StringBuilder();
        //上正式區改成False
        bool isTestJobLog = Convert.ToBoolean(ConfigurationManager.AppSettings["IsTestJobLog"]);
        bool issucess = true;
        string tpeMisMail = string.Empty;

        bool AnotherDat = Convert.ToBoolean(ConfigurationManager.AppSettings["UseAnotherDate"]);
        string OutputDate = "GETDATE()";


        public Main()
        {
            InitializeComponent();
        }
        

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            
            transferPMS.fromSystem = "PBIReportData";

            if (isAuto)
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
            if (this.AnotherDat)
            {
                this.OutputDate ="'"+ ConfigurationManager.AppSettings["OutputDate_AnotherDate"].ToString() + "'";
            }

            try
            {
                string sqlcmd = $@"
Declare @ServerName varchar(20);
Declare  @OutputDate datetime ={OutputDate};
DECLARE ServerNameList CURSOR FOR select name from sys.servers WHERE Name Like 'PMS\pmsdb\%'
OPEN ServerNameList
FETCH NEXT FROM ServerNameList INTO @ServerName
WHILE @@FETCH_STATUS = 0
BEGIN
	--SELECT @ServerName
	EXEC ImportSewingDailyOutput @OutputDate,@ServerName
FETCH NEXT FROM ServerNameList INTO @ServerName
END
CLOSE ServerNameList
DEALLOCATE ServerNameList
                    ";
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


            if (this.AnotherDat)
            {
                this.OutputDate =  ConfigurationManager.AppSettings["OutputDate_AnotherDate"].ToString() ;
            }

            #region 組合 Desc
            if (!MyUtility.Check.Empty(ErrorMsg))
            {
                issucess = false;
            }

            desc = $@"Import SewingOutPut failed. Check there information first.
Transfer date:{this.OutputDate }
Instance: PMSDB\POWERBI
Stored Procedure: [PBIReportData].ImportSewingDailyOutput
";
            #endregion

            subject = "Import SewingDailyOutPut";
            if (issucess)
            {
                subject += " Success";
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
            this.CallJobLogApi("Import SewingDailyOutPut to Power BI", "Import SewingDailyOutPut to Power BI Test", DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), true, true);
        }

        private void btnTestMail_Click(object sender, EventArgs e)
        {
            SendMail("Import SewingDailyOutPut", "Import SewingDailyOutPut Test mail.");
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
