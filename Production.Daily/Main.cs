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

namespace Production.Daily
{
    public partial class Main : Sci.Win.Tems.Input7
    {
        DataRow mailTo;
        TransferPms export = new TransferPms();

        public Main()
        {
            InitializeComponent();

        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            OnRequery();

        }

        private void OnRequery()
        {
            DataTable _mailTo;
            String sqlCmd;
            sqlCmd = "Select * From dbo.MailTo Where ID = '001'";

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out _mailTo);

            if (!result) { ShowErr(result); return; }

            mailTo = _mailTo.Rows[0];

            editToAddress.Text = mailTo["ToAddress"].ToString();
            editCcAddress.Text = mailTo["CcAddress"].ToString();
            editContent.Text = mailTo["Content"].ToString();
        }

        protected override DualResult ClickSavePost()
        {
            DualResult result;
            String sqlCmd;
            List<SqlParameter> paras = new List<SqlParameter>();

            sqlCmd = "Update dbo.MailTo Set ToAddress = @ToAddress, CcAddress = @CcAddress, Content = @Content Where ID = '001'";
            paras.Add(new SqlParameter("@ToAddress", editToAddress.Text));
            paras.Add(new SqlParameter("@CcAddress", editCcAddress.Text));
            paras.Add(new SqlParameter("@Content", editContent.Text));
            result = DBProxy.Current.Execute(null, sqlCmd, paras);

            if (!result) { return result; }

            return base.ClickSavePost();
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            OnRequery();
        }

        protected override void ClickUndo()
        {
            base.ClickUndo();
            OnRequery();
        }

        #region 接Sql Server 進度訊息用
        private void InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            BeginInvoke(() => { this.labelProgress.Text = e.Message; });
        }
        #endregion

        public List<SqlParameter> GetSqlParameter()
        {
            return null;
        }

        #region Export/Update (非同步)
        private DualResult AsyncUpdateExport(SqlConnection conn)
        {
            DateTime startDate;
            DateTime endDate;

            startDate = DateTime.Now;
            //export.CreateSqlParamsAction = (Transfer.CreateSqlParams)this.GetSqlParameter;
            //export.Export_Pms_To_Trade(this.CurrentData["FtpIP"].ToString(), this.CurrentData["FtpID"].ToString(), this.CurrentData["FtpPwd"].ToString());

            //import.i
            endDate = DateTime.Now;
            #region 發送Mail
            String sqlCmd = "";
            DualResult result;
            DataTable orderComparisonList;
            DataTable dateInfo;
            #region 增列comparison list 日期
            String transferDate;
            String updateDate;

            sqlCmd = "Select Top 1 TransferDate, UpdateDate" +
                     "  From Production.dbo.OrderComparisonList" +
                     " Where UpdateDate = (Select Max(UpdateDate) From Production.dbo.OrderComparisonList as tmp)" +
                     " Order by Ukey desc";

            result = DBProxy.Current.Select(null, sqlCmd, out orderComparisonList);
            if (!result || orderComparisonList.Rows.Count == 0)
            {
                transferDate = "";
                updateDate = "";
            }
            else
            {
                transferDate = ((DateTime)orderComparisonList.Rows[0]["TransferDate"]).ToShortDateString();
                updateDate = ((DateTime)orderComparisonList.Rows[0]["UpdateDate"]).ToShortDateString();
            }
            #endregion
            
            String mailServer = this.CurrentData["MailServer"].ToString();
            String eMailID = this.CurrentData["EMailID"].ToString();
            String eMailPwd = this.CurrentData["EMailPwd"].ToString();
            export.SetSMTP(mailServer, 25, eMailID, eMailPwd);
            
            String sendFrom = this.CurrentData["SendFrom"].ToString();
            String toAddress = mailTo["ToAddress"].ToString();
            String ccAddress = mailTo["CcAddress"].ToString();
            //String toAddress = "ben.chen@sportscity.com.tw";
            //String ccAddress = "";
            String subject = "Auto Download and Updated for PMS!! from " + this.CurrentData["RgCode"].ToString();
            #region 組合 Desc
            String desc = "Dear All:" + Environment.NewLine + Environment.NewLine +
                          "             (**Please don't reply this mail. **)" + Environment.NewLine + Environment.NewLine +
                          "PMS system already Downloaded and Updated From Taipei's FTP." + Environment.NewLine +
                          "Pls confirm and take notes." + Environment.NewLine +
                          "================================" + Environment.NewLine +
                          "Factory Name: " + this.CurrentData["RgCode"].ToString().ToUpper() + Environment.NewLine +
                          "--------------------------------------------------------" + Environment.NewLine +
                          "    Start Time: " + startDate.ToString() + Environment.NewLine +
                          "    Ending Time: " + endDate.ToString() + Environment.NewLine +
                          "    Comparison List: OldDate:" + transferDate.ToString() + "  Update:" + updateDate.ToString() + Environment.NewLine +
                          "--------------------------------------------------------"
                          ;
            #region 取得各個function的起訖日期，寫入mail desc.
            sqlCmd = "Select 1 as Seq, 'Export' as TransState, Name, DateStart, DateEnd From Pms_To_Trade.dbo.DateInfo" +
                     "Union All" +
                     "Select 2 as Seq, 'Upload' as TransState, Name, DateStart, DateEnd From Trade_To_Pms.dbo.DateInfo" +
                     " Order by Seq";
            result = DBProxy.Current.Select(null, sqlCmd, out dateInfo);
            if (result && orderComparisonList.Rows.Count > 0)
            {
                foreach (DataRow row in dateInfo.Rows)
                {
                    desc += Environment.NewLine + row["TransState"].ToString() + ":" + row["Name"].ToString() + "    From :" + row["DateEnd"].ToString() + " To :" + row["Name"].ToString();
                }
            }
            #endregion
            desc += Environment.NewLine + "================================" +
                    Environment.NewLine + mailTo["Content"].ToString();
            #endregion
            subject = mailTo["Subject"].ToString().TrimEnd() + this.CurrentData["RgCode"].ToString();

            Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(sendFrom, toAddress, ccAddress, subject, "", desc, true, true);

            mail.ShowDialog();
            #endregion

            return Result.True;
        }
        #endregion

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection conn;

            if (!Sci.SQL.GetConnection(out conn)) { return; }
            conn.InfoMessage += new SqlInfoMessageEventHandler(InfoMessage);

            DualResult result = AsyncHelper.Current.DataProcess(this, () =>
            {
                return AsyncUpdateExport(conn);
            });

            conn.Close();
        }

        private void btnTestFTP_Click(object sender, EventArgs e)
        {
            DualResult result;
            result = export.Ftp_Ping(this.CurrentData["FtpIP"].ToString(), this.CurrentData["FtpID"].ToString(), this.CurrentData["FtpPwd"].ToString());

            if (!result)
            {
                ShowErr(result);
            }
        }

        private void BtnTestMail_Click(object sender, EventArgs e)
        {
            String mailServer = this.CurrentData["MailServer"].ToString();
            String eMailID = this.CurrentData["EMailID"].ToString();
            String eMailPwd = this.CurrentData["EMailPwd"].ToString();
            export.SetSMTP(mailServer, 25, eMailID, eMailPwd);
            
            String sendFrom = this.CurrentData["SendFrom"].ToString();
            String toAddress = mailTo["ToAddress"].ToString();
            String ccAddress = mailTo["CcAddress"].ToString();
            //String toAddress = "ben.chen@sportscity.com.tw";
            //String ccAddress = "";
            String subject = mailTo["Subject"].ToString();
            String desc = mailTo["Content"].ToString();
            Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(sendFrom, toAddress, ccAddress, subject, "", desc,true,true);

            mail.ShowDialog();
        }

        private void btnGetImportDataPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            if (path.ShowDialog() == DialogResult.OK)
            {
                this.displayImportDataPath.Value = path.SelectedPath;
            }
        }

        private void btnGetExportDataPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            if (path.ShowDialog() == DialogResult.OK)
            {
                this.displayExportDataPath.Value = path.SelectedPath;
            }
        }
    }
}
