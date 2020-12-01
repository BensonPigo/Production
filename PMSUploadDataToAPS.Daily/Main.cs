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
using System.Threading;

namespace PMSUploadDataToAPS.Daily
{
    public partial class Main : Sci.Win.Tems.Input7
    {
        bool isAuto = false;
        DataRow mailTo;
        TransferPms transferPMS = new TransferPms();
        StringBuilder sqlmsg = new StringBuilder();
        bool isTestJobLog = false;
        string tpeMisMail = string.Empty;

        public Main()
        {
            InitializeComponent();
            isAuto = false;
        }

        public Main(String _isAuto)
        {
            InitializeComponent();
            if (String.IsNullOrEmpty(_isAuto))
            {
                isAuto = true;
                this.OnFormLoaded();
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            OnRequery();

            //transferPMS.fromSystem = "Production";

            if (isAuto)
            {
                ClickExport();
                this.Close();
            }
        }

        private void OnRequery()
        {
            DataTable _mailTo;
            String sqlCmd;
            sqlCmd = "Select * From dbo.MailTo Where ID = '012'";

            DualResult result = DBProxy.Current.Select("Production", sqlCmd, out _mailTo);

            if (!result)
            {
                if (this.isAuto)
                {
                    throw result.GetException();
                }
                else
                {
                    ShowErr(result);
                    return;
                }
            }

            mailTo = _mailTo.Rows[0];

            this.tpeMisMail = MyUtility.GetValue.Lookup("Select ToAddress From dbo.MailTo Where ID = '101'");
        }

        #region 接Sql Server 進度訊息用
        private void InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            BeginInvoke(() => { this.labelProgress.Text = e.Message; });
        }
        #endregion

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ClickExport();
        }

        #region Send Mail
        private void SendMail(String subject = "", String desc = "", bool isFail = true)
        {
            String mailServer = this.CurrentData["MailServer"].ToString();
            String eMailID = this.CurrentData["EMailID"].ToString();
            String eMailPwd = this.CurrentData["EMailPwd"].ToString();
            transferPMS.SetSMTP(mailServer, 25, eMailID, eMailPwd);

            String sendFrom = this.CurrentData["SendFrom"].ToString();
            String toAddress = mailTo["ToAddress"].ToString();
            String ccAddress = mailTo["CcAddress"].ToString();

            if (isFail)
            {
                toAddress += MyUtility.Check.Empty(toAddress) ? this.tpeMisMail : ";" + this.tpeMisMail;
            }

            if (String.IsNullOrEmpty(subject))
            {
                subject = mailTo["Subject"].ToString();
            }
            if (String.IsNullOrEmpty(desc))
            {
                desc = mailTo["Content"].ToString();
            }

            if (!MyUtility.Check.Empty(toAddress))
            {
                Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(sendFrom, toAddress, ccAddress, subject, "", desc, true, true);

                DualResult mailResult = mail.Send();
                if (!mailResult)
                {
                    if (this.isAuto)
                    {
                        throw mailResult.GetException();
                    }
                    else
                    {
                        this.ShowErr(mailResult);
                    }
                }
            }
        }
        #endregion

        #region Update/Update動作
        private void ClickExport()
        {
            SqlConnection conn;

            if (!Sci.SQL.GetConnection(out conn)) { return; }

            conn.InfoMessage += new SqlInfoMessageEventHandler(InfoMessage);

            DualResult result;
            if (isAuto)
            {
                result = AsyncUpdateExport(conn);
            }
            else
            {
                result = AsyncHelper.Current.DataProcess(this, () =>
                {
                    return AsyncUpdateExport(conn);
                });
            }

            mymailTo(result.ToSimpleString());

            if (!result)
            {
                if (this.isAuto)
                {
                    throw result.GetException();
                }
                else
                {
                    ShowErr(result);
                    return;
                }
            }

            conn.Close();
            issucess = true;
        }

        private void mymailTo(string msg)
        {
            String subject = "";
            String desc = "";

            #region 完成後發送Mail
            #region 組合 Desc
            desc = "UPDATE usp_PMSUploadDataToAPS " + Environment.NewLine;
            desc += Environment.NewLine + "Dear All:" + Environment.NewLine + Environment.NewLine +
                   "             (**Please don't reply this mail. **)" + Environment.NewLine + Environment.NewLine +
                   "PMS system already uploaded data to APS." + Environment.NewLine +
                   "Pls confirm and take notes." + Environment.NewLine +
                   "================================" +
                    Environment.NewLine + mailTo["Content"].ToString() + Environment.NewLine;
            if (!MyUtility.Check.Empty(msg))
            {
                desc += "Sql msg:" + Environment.NewLine +
                    msg + Environment.NewLine;
                issucess = false;
            }
            #endregion

            subject = mailTo["Subject"].ToString().TrimEnd() + " - [" + this.CurrentData["RgCode"].ToString() + "]";
            if (issucess)
            {
                subject += " Success";
            }
            else
            {
                subject += " Error!";
            }
            SendMail(subject, desc, !issucess);
            this.CallJobLogApi(subject, desc, DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), isTestJobLog, issucess);
            #endregion
        }
        #endregion
        bool issucess = true;
        #region Export/Update (非同步)
        private DualResult AsyncUpdateExport(SqlConnection conn)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_PMSUploadDataToAPS", conn);
                cmd.CommandType = CommandType.StoredProcedure;
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

        #region Call JobLog web api回傳執行結果
        private void CallJobLogApi(string subject, string desc, string startDate, string endDate, bool isTest, bool succeeded)
        {
            JobLog jobLog = new JobLog()
            {
                GroupID = "P",
                SystemID = "PMS",
                Region = this.CurrentData["RgCode"].ToString(),
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

        private void btnTestWebAPI_Click(object sender, EventArgs e)
        {
            this.CallJobLogApi("APS upload Test", "APS upload Test", DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), true, true);
        }

        private void btnTestMail_Click(object sender, EventArgs e)
        {
            SendMail("APS Test", "APS Test");
        }
    }
}
