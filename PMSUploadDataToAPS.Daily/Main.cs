﻿using Sci.DB;
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

namespace PMSUploadDataToAPS.Daily
{
    public partial class Main : Sci.Win.Tems.Input7
    {
        bool isAuto = false;
        DataRow mailTo;
        TransferPms transferPMS = new TransferPms();
        StringBuilder sqlmsg = new StringBuilder();

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
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MDivisionID.ReadOnly = false;
            OnRequery();

            transferPMS.fromSystem = "Production";

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

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out _mailTo);

            if (!result) { ShowErr(result); return; }

            mailTo = _mailTo.Rows[0];
        }
        
        #region 接Sql Server 進度訊息用
        private void InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            BeginInvoke(() => { this.labelProgress.Text = e.Message;
                sqlmsg.Append(e.Message); });
        }
        #endregion
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ClickExport();
        }

        #region Send Mail
        private void SendMail(String subject = "", String desc = "")
        {
            String mailServer = this.CurrentData["MailServer"].ToString();
            String eMailID = this.CurrentData["EMailID"].ToString();
            String eMailPwd = this.CurrentData["EMailPwd"].ToString();
            transferPMS.SetSMTP(mailServer, 25, eMailID, eMailPwd);

            String sendFrom = this.CurrentData["SendFrom"].ToString();
            String toAddress = mailTo["ToAddress"].ToString();
            String ccAddress = mailTo["CcAddress"].ToString();
            //String toAddress = "ben.chen@sportscity.com.tw";
            //String ccAddress = "";
            if (String.IsNullOrEmpty(subject))
            {
                subject = mailTo["Subject"].ToString();
            }
            if (String.IsNullOrEmpty(desc))
            {
                desc = mailTo["Content"].ToString();
            }
            Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(sendFrom, toAddress, ccAddress, subject, "", desc, true, true);

            mail.ShowDialog();
        }
        #endregion

        #region Update/Update動作
        private void ClickExport()
        {
            SqlConnection conn;

            if (!Sci.SQL.GetConnection(out conn)) { return; }
            
            conn.InfoMessage += new SqlInfoMessageEventHandler(InfoMessage);

            DualResult result = AsyncHelper.Current.DataProcess(this, () =>
            {
                return AsyncUpdateExport(conn);
            });

            mymailTo();

            if (!result)
            {
                ShowErr(result);
            }

            conn.Close();
        }

        private void mymailTo()
        {
            String subject = "";
            String desc = "";
            //MyUtility.Msg.InfoBox(sqlmsg.ToString());

            #region 完成後發送Mail
            #region 組合 Desc
            desc = "Sorry Test usp_PMSUploadDataToAPS";
            desc += "Dear All:" + Environment.NewLine + Environment.NewLine +
                   "             (**Please don't reply this mail. **)" + Environment.NewLine + Environment.NewLine +
                   "PMS system already uploaded data to APS." + Environment.NewLine +
                   "Pls confirm and take notes." + Environment.NewLine +
                   "================================" +
                    Environment.NewLine + mailTo["Content"].ToString() + Environment.NewLine +
                    "Sql msg:" + Environment.NewLine +
                    sqlmsg.ToString();
            #endregion

            subject = mailTo["Subject"].ToString().TrimEnd() + this.CurrentData["RgCode"].ToString();

            SendMail(subject, desc);
            #endregion
        }
        #endregion

        #region Export/Update (非同步)
        private DualResult AsyncUpdateExport(SqlConnection conn)
        {

            string cmdid = "select id from MDivision  where SQLServerName !='' or APSDatabaseName != '' or APSLoginId != '' or APSLoginPwd !=''";
            DataTable tbid;

            DBProxy.Current.Select(null, cmdid, out tbid);

            try
            {
                //SqlCommand cmd = new SqlCommand("usp_PMSUploadDataToAPS", conn);
                SqlCommand cmd = new SqlCommand("TESTTT", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (DataRow drid in tbid.Rows)
                {
                    cmd.Parameters.AddWithValue("@M", drid[0].ToString());
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
            catch (SqlException se)
            {
                //return Result.F("執行資料庫預存程序時發生錯誤。", se);
                return Ict.Result.F(se);
            }                       
            return Ict.Result.True;
        }
        #endregion
    }
}
