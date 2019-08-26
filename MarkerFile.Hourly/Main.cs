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

namespace MarkerFile.Hourly
{
    public partial class Main : Sci.Win.Tems.Input7
    {
        bool isAuto = false;
        DataRow mailTo;
        TransferPms transferPMS = new TransferPms();
        StringBuilder sqlmsg = new StringBuilder();
        bool isTestJobLog = true;
        string tpeMisMail = string.Empty;
        private string MarkerInputPath;
        private string MarkerOutputPath;
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Run();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            OnRequery();

            transferPMS.fromSystem = "Production";

            if (isAuto)
            {
                Run();
                this.Close();
            }
        }

        private void OnRequery()
        {
            DataTable _mailTo;
            String sqlCmd;
            sqlCmd = "Select * From dbo.MailTo Where ID = '101'";

            DualResult result = DBProxy.Current.Select("Production", sqlCmd, out _mailTo);

            if (!result) { ShowErr(result); return; }

            mailTo = _mailTo.Rows[0];

            this.tpeMisMail = MyUtility.GetValue.Lookup("Select ToAddress From dbo.MailTo Where ID = '101'");

            #region File Path
            DataTable Path;
            result = DBProxy.Current.Select("Production", "select MarkerInputPath,MarkerOutputPath from system", out Path);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.MarkerInputPath = MyUtility.Convert.GetString(Path.Rows[0]["MarkerInputPath"]);
            this.MarkerOutputPath = MyUtility.Convert.GetString(Path.Rows[0]["MarkerOutputPath"]);
            this.MarkerInputPath = @"C:\test\Pattern\URI MARKER\KURIS\BULK";
            this.MarkerOutputPath = @"C:\test\Pattern\MarkerFileTPETest";

            if (MyUtility.Check.Empty(MarkerInputPath) || MyUtility.Check.Empty(MarkerOutputPath))
            {
                MyUtility.Msg.WarningBox("Please set MarkerInputPath and MarkerOutputPath");
                this.Close();
            }
            #endregion
        }

        private void Run()
        {
            string msg = string.Empty;
            MarkerSwitch(out msg);
            mymailTo(msg);
        }

        private void mymailTo(string msg)
        {
            string subject = "";
            string desc = "";
            bool issucess = true;

            #region 組合 Desc
            string res = "Success";
            if (!MyUtility.Check.Empty(msg))
            {
                res = "Error";
                issucess = false;
            }

            desc = $@"
Transfer DateTime： {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}
Result : {res}

{msg}
Please do not reply this mail.
";
            #endregion

            subject = "Auto-generate Marker File named by Cutref";
            if (issucess) subject += " Success";
            else subject += " Error!";

            if (!issucess) SendMail(subject, desc, !issucess); // 錯誤才發信

            this.CallJobLogApi(subject, desc, DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), isTestJobLog, issucess);
        }

        private void SendMail(String subject = "", String desc = "", bool isFail = true)
        {
            String mailServer = this.CurrentData["MailServer"].ToString();
            String eMailID = this.CurrentData["EMailID"].ToString();
            String eMailPwd = this.CurrentData["EMailPwd"].ToString();
            transferPMS.SetSMTP(mailServer, 25, eMailID, eMailPwd);

            String sendFrom = this.CurrentData["SendFrom"].ToString();
            String toAddress = "pmshelp@sportscity.com.tw";
            String ccAddress = "";

            if (isFail)
            {
                toAddress += MyUtility.Check.Empty(toAddress) ? this.tpeMisMail : ";" + this.tpeMisMail;
            }

            if (String.IsNullOrEmpty(subject)) subject = mailTo["Subject"].ToString();
            if (String.IsNullOrEmpty(desc)) desc = mailTo["Content"].ToString();

            if (!MyUtility.Check.Empty(toAddress))
            {
                Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(sendFrom, toAddress, ccAddress, subject, "", desc, true, true);

                mail.ShowDialog();
            }
        }

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

        private bool MarkerSwitch(out string msg)
        {
            if (!DeleteFile(out msg)) return false;
            if (!CheckUpdateFile(out msg)) return false;
            if (!CheckUpdateSwitchRecord(out msg)) return false;
            if (!CreateFile(out msg)) return false;
            msg = "";
            return true;
        }

        // 1.	刪除舊檔案及資料夾
        private bool DeleteFile(out string msg)
        {
            #region 三日內PMS Cutting［P20.Cutting Daily Output］中已有完整報產出(Lacking Layer = 0)的［CutRef#］及SP#清單
            string sqlcmd = $@"
select
	w.CutRef,CuttingID=w.ID
from CuttingOutput_Detail cod with(nolock)
inner join CuttingOutput co with(nolock) on cod.id = co.id
inner join WorkOrder w WITH (NOLOCK) on cod.WorkOrderUkey = w.Ukey
outer apply(select AccuCuttingLayer = sum(aa.Layer) from cuttingoutput_Detail aa where aa.WorkOrderUkey = w.Ukey and id <> co.ID)acc
where co.cDate between cast(Dateadd(day,-3,getdate())as date) and cast(getdate()as date)
and w.Layer -isnull(acc.AccuCuttingLayer,0)- cod.layer = 0
";
            DataTable deleteFileData;
            DualResult result = DBProxy.Current.Select("Production", sqlcmd, out deleteFileData);
            if (!result)
            {
                msg = result.ToSimpleString();
                return false;
            }

            if (!DeleteFileFromList(deleteFileData, out msg))
            {
                return false;
            }

            string sqlDelete = $@"
Delete NS
from CuttingOutput_Detail cod with(nolock)
inner join CuttingOutput co with(nolock) on cod.id = co.id
inner join WorkOrder w WITH (NOLOCK) on cod.WorkOrderUkey = w.Ukey
outer apply(select AccuCuttingLayer = sum(aa.Layer) from cuttingoutput_Detail aa where aa.WorkOrderUkey = w.Ukey and id <> co.ID)acc
inner join MarkerFileNameSwitchRecord NS WITH (NOLOCK) on NS.CutRef = w.CutRef and NS.CuttingID = w.ID
where co.cDate between cast(Dateadd(day,-3,getdate())as date) and cast(getdate()as date)
and w.Layer -isnull(acc.AccuCuttingLayer,0)- cod.layer = 0
";
            result = DBProxy.Current.Execute("Production", sqlDelete);
            if (!result)
            {
                msg = result.ToSimpleString();
                return false;
            }
            #endregion

            #region MarkerFileNameSwitchRecord 超過一個月的清單
            sqlcmd = $@"
select NS.CutRef,NS.CuttingID
from MarkerFileNameSwitchRecord NS WITH (NOLOCK)
where NS.EstCutDate < cast(Dateadd(day,-30,getdate())as date)
";
            result = DBProxy.Current.Select("Production", sqlcmd, out deleteFileData);
            if (!result)
            {
                msg = result.ToSimpleString();
                return false;
            }

            if (!DeleteFileFromList(deleteFileData, out msg))
            {
                return false;
            }

            sqlDelete = $@"
Delete MarkerFileNameSwitchRecord where EstCutDate < cast(Dateadd(day,-30,getdate())as date)
";
            result = DBProxy.Current.Execute("Production", sqlDelete);
            if (!result)
            {
                msg = result.ToSimpleString();
                return false;
            }
            #endregion

            msg = "";
            return true;
        }

        private bool DeleteFileFromList(DataTable deleteFileData,out string msg)
        {
            foreach (DataRow item in deleteFileData.Rows)
            {
                // 移除檔案
                string destFilePath = System.IO.Path.Combine(this.MarkerOutputPath, MyUtility.Convert.GetString(item["CuttingID"]), MyUtility.Convert.GetString(item["CutRef"]) + ".gbr");
                if (File.Exists(destFilePath))
                {
                    try
                    {
                        System.IO.File.Delete(destFilePath);
                    }
                    catch (IOException e)
                    {
                        msg = e.Message;
                        return false;
                    }
                }

                // 相同SP#名稱的資料夾下已經沒有檔案，則移除該資料夾
                string destPath = Path.Combine(this.MarkerOutputPath, MyUtility.Convert.GetString(item["CuttingID"]));
                DirectoryInfo di = new DirectoryInfo(destPath);
                if (di.Exists && di.GetFiles().Count() == 0)
                {
                    try
                    {
                        di.Delete();
                    }
                    catch (IOException e)
                    {
                        msg = e.Message;
                        return false;
                    }
                }
            }

            msg = "";
            return true;
        }

        // 2.	檢查及[更新]檔案
        private bool CheckUpdateFile(out string msg)
        {
            string sqlcmd = $@"select * from MarkerFileNameSwitchRecord with(nolock)";
            DataTable markerFileNameSwitchRecord;
            DualResult result = DBProxy.Current.Select("Production", sqlcmd, out markerFileNameSwitchRecord);
            if (!result)
            {
                msg = result.ToSimpleString();
                return false;
            }

            DirectoryInfo di = new DirectoryInfo(this.MarkerInputPath);
            foreach (var fi in di.GetFiles())
            {
                // 找有檔案名稱一樣,但最後編輯時間不一樣
                var chkExists = markerFileNameSwitchRecord.AsEnumerable().
                    Where(w => MyUtility.Convert.GetString(w["OriFileName"]) == fi.Name && ((DateTime)w["OriFileLastMDate"]).ToString("yyyy/MM/dd HH:mm:ss") != fi.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss")).
                    ToList();
                if (chkExists.Count >= 1)
                {
                    foreach (var item in chkExists)
                    {
                        // 以對應的[MarkerFileNameSwitchRecord].[CutRef]為檔名，複製該input資料夾路徑馬克檔，到output資料夾路徑以取代舊馬克檔
                        try
                        {
                            string sourceFile = System.IO.Path.Combine(this.MarkerInputPath, fi.Name);
                            string destPath = System.IO.Path.Combine(this.MarkerOutputPath, MyUtility.Convert.GetString(item["CuttingID"]));
                            string destFile = System.IO.Path.Combine(destPath, MyUtility.Convert.GetString(item["CutRef"]) + ".gbr");
                            Directory.CreateDirectory(destPath);
                            File.Copy(sourceFile, destFile, true);
                        }
                        catch (IOException e)
                        {
                            msg = e.Message;
                            return false;
                        }

                        // 將檔案最後修改日寫入OriFileLastMDate,並更新UpdateDate
                        string sqlupdate = $@"
update MarkerFileNameSwitchRecord set 
    OriFileLastMDate='{fi.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss")}',
    UpdateDate=GetDate() 
where Cutref='{item["Cutref"]}'";
                        result = DBProxy.Current.Execute("Production", sqlupdate);
                        if (!result)
                        {
                            msg = result.ToSimpleString();
                            return false;
                        }
                    }
                }
            }

            msg = "";
            return true;
        }

        // 3.	檢查及更新[Production].[MarkerFileNameSwitchRecord].[EstCutDate]
        private bool CheckUpdateSwitchRecord(out string msg)
        {
            string sqlupdate = $@"
update NS set NS.EstCutDate = w.EstCutDate,NS.UpdateDate=GETDATE()
from WorkOrder w with(nolock)
inner join MarkerFileNameSwitchRecord NS with(nolock) on NS.CutRef = w.CutRef and NS.CuttingID = W.ID
where w.EstCutDate <> NS.EstCutDate or NS.EstCutDate is null";
            DualResult result = DBProxy.Current.Execute("Production", sqlupdate);
            if (!result)
            {
                msg = result.ToSimpleString();
                return false;
            }

            msg = "";
            return true;
        }

        // 4.	建立資料夾及檔案
        private bool CreateFile(out string msg)
        {
            string sqlcmd = $@"
select
	w.CutRef,w.id,filename=concat(w.MarkerNo,w.Markername,'.gbr'),w.EstCutDate,acc.AccuCuttingLayer
from WorkOrder w with(nolock)
outer apply(select AccuCuttingLayer = sum(cd.Layer) from cuttingoutput_Detail cd where cd.WorkOrderUkey = w.Ukey)acc
where w.EstCutDate between cast(getdate()as date) and  cast(Dateadd(day,7,getdate())as date) 
and w.Layer -isnull(acc.AccuCuttingLayer,0) <> 0
and not exists(select 1 from [MarkerFileNameSwitchRecord] where cutref = w.cutref)
";
            DataTable copyFileData;
            DualResult result = DBProxy.Current.Select("Production", sqlcmd, out copyFileData);
            if (!result)
            {
                msg = result.ToSimpleString();
                return false;
            }

            foreach (DataRow item in copyFileData.Rows)
            {
                string sourceFile = Path.Combine(this.MarkerInputPath, @"\", MyUtility.Convert.GetString(item["filename"]));
                string targetPath = Path.Combine(this.MarkerOutputPath, MyUtility.Convert.GetString(item["id"]));
                string destFile = Path.Combine(this.MarkerOutputPath, MyUtility.Convert.GetString(item["id"]), MyUtility.Convert.GetString(item["CutRef"]) + ".gbr");
                string lastWriteTime = File.GetLastWriteTime(sourceFile).ToString("yyyy/MM/dd HH:mm:ss");
                if (File.Exists(sourceFile))
                {
                    try
                    {
                        Directory.CreateDirectory(targetPath);
                        File.Copy(sourceFile, destFile, true);
                    }
                    catch (IOException e)
                    {
                        msg = e.Message;
                        return false;
                    }

                    string sqlinsert = $@"
INSERT INTO [dbo].[MarkerFileNameSwitchRecord]
    ([CutRef]
    ,[EstCutDate]
    ,[CuttingID]
    ,[OriFileName]
    ,[OriFileLastMDate]
    ,[TransInDate])
VALUES
    ('{item["CutRef"]}'
    ,'{((DateTime)item["EstCutDate"]).ToString("yyyy/MM/dd HH:mm:ss")}'
    ,'{item["id"]}'
    ,'{item["filename"]}'
    ,'{lastWriteTime}'
    ,GETDATE())
";
                    result = DBProxy.Current.Execute("Production", sqlinsert);
                    if (!result)
                    {
                        msg = result.ToSimpleString();
                        return false;
                    }
                }
            }

            msg = "";
            return true;
        }
    }
}
