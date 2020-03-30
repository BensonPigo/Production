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
        TransferPms transferPMS = new TransferPms();
        StringBuilder sqlmsg = new StringBuilder();
        bool isTestJobLog = false;
        private string MarkerInputPath;
        private string MarkerOutputPath;
        List<FileInfo> fileInfos = new List<FileInfo>();
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

            transferPMS.fromSystem = "Production";

            if (isAuto)
            {
                Run();
                this.Close();
            }
        }

        private void Run()
        {
            string msg = string.Empty;
            #region File Path
            DataTable Path;
            DualResult result = DBProxy.Current.Select("Production", "select MarkerInputPath,MarkerOutputPath from system", out Path);
            if (!result)
            {
                msg = result.ToString();
            }
            if (MyUtility.Check.Empty(msg))
            {
                this.MarkerInputPath = MyUtility.Convert.GetString(Path.Rows[0]["MarkerInputPath"]);
                this.MarkerOutputPath = MyUtility.Convert.GetString(Path.Rows[0]["MarkerOutputPath"]);

                if (MyUtility.Check.Empty(MarkerInputPath) || MyUtility.Check.Empty(MarkerOutputPath))
                {
                    msg = "Please set MarkerInputPath and MarkerOutputPath";
                }
            }
            #endregion

            if (MyUtility.Check.Empty(msg))
            {
                MarkerSwitch(out msg);
            }
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
            String toAddress = "";
            String ccAddress = "";

            string sqlgetmail = "select * from Mailto where ID = '019'";
            DataRow drmail;
            if (MyUtility.Check.Seek(sqlgetmail, out drmail))
            {
                toAddress = MyUtility.Convert.GetString(drmail["ToAddress"]);
                ccAddress = MyUtility.Convert.GetString(drmail["ccAddress"]);
            }

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
                msg = result.ToString() + "\r\nDeleteFile() Sql Error 1";
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
                msg = result.ToString() + "\r\nDeleteFile() Sql Error 2";
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
                msg = result.ToString() + "\r\nDeleteFile() Sql Error 3";
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
                msg = result.ToString() + "\r\nDeleteFile() Sql Error 4";
                return false;
            }
            #endregion

            msg = "";
            return true;
        }

        private bool DeleteFileFromList(DataTable deleteFileData, out string msg)
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
                        msg = e.Message + $"POID:{item["CuttingID"]}, CutRef:{item["CutRef"]}" + destFilePath + "\r\nDeleteFileFromList() Error1";
                        return false;
                    }
                }

                // 相同SP#名稱的資料夾下已經沒有檔案，則移除該資料夾
                string destPath = Path.Combine(this.MarkerOutputPath, MyUtility.Convert.GetString(item["CuttingID"]));
                DirectoryInfo di = new DirectoryInfo(destPath);
                if (di.Exists && di.GetFiles().Count() == 0 && di.GetDirectories().Count() == 0)
                {
                    try
                    {
                        di.Delete();
                    }
                    catch (IOException e)
                    {
                        msg = e.Message + $"POID:{item["CuttingID"]}, CutRef:{item["CutRef"]}" + destPath + "\r\nDeleteFileFromList() Error2";
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
                msg = result.ToString() + "\r\nCheckUpdateFile() Sql Error 1";
                return false;
            }

            fileInfos.Clear();
            // 找出指定目錄(包含所有子目錄) 的檔案, 加入 fileInfos
            try
            {
                FindFile(MarkerInputPath);
            }
            catch (Exception ex)
            {
                msg = ex.Message + "\r\nFindFile() Error";
                return false;
            }

            // 取最新編輯時間的那筆檔案
            var fileList = fileInfos.AsEnumerable().OrderByDescending(s => s.LastWriteTime)
                .GroupBy(s => s.Name, (key, fileGroup) => new
                {
                    Name = key,
                    fileGroup.First().FullName,
                    fileGroup.First().LastWriteTime,
                }).ToList();

            foreach (var fi in fileList)
            {
                // 找檔案名稱在DB內有,但最後編輯時間不一樣
                var chkExists = markerFileNameSwitchRecord.AsEnumerable().
                    Where(w => MyUtility.Convert.GetString(w["OriFileName"]) == fi.Name && ((DateTime)w["OriFileLastMDate"]).ToString("yyyy/MM/dd HH:mm:ss") != fi.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss")).
                    ToList();
                if (chkExists.Count >= 1)
                {
                    foreach (var item in chkExists)
                    {
                        // 以對應的[MarkerFileNameSwitchRecord].[CutRef]為檔名，複製該input資料夾路徑馬克檔，到output資料夾路徑以取代舊馬克檔
                        string destPath = string.Empty;
                        string destFile = string.Empty;
                        try
                        {
                            destPath = System.IO.Path.Combine(this.MarkerOutputPath, MyUtility.Convert.GetString(item["CuttingID"]));
                            destFile = System.IO.Path.Combine(destPath, MyUtility.Convert.GetString(item["CutRef"]) + ".gbr");
                            Directory.CreateDirectory(destPath);
                            File.Copy(fi.FullName, destFile, true);
                        }
                        catch (IOException e)
                        {
                            msg = e.Message + $"POID:{item["CuttingID"]}, CutRef:{item["CutRef"]}\r\n" + destFile + "\r\nCheckUpdateFile() Error";
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
                            msg = result.ToString() + $"POID:{item["CuttingID"]}, CutRef:{item["CutRef"]}\r\n" + "\r\nCheckUpdateFile() Sql Error 2";
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
                msg = result.ToString() + "\r\nCheckUpdateSwitchRecord() Sql Error 1";
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
	w.CutRef,CuttingID=w.ID,filename=concat(w.MarkerNo,w.Markername,'.gbr'),w.EstCutDate
from WorkOrder w with(nolock)
outer apply(select AccuCuttingLayer = sum(cd.Layer) from cuttingoutput_Detail cd where cd.WorkOrderUkey = w.Ukey)acc
where w.EstCutDate between  cast(Dateadd(day,0,getdate())as date) and  cast(Dateadd(day,7,getdate())as date) --從今天往後7天
and not exists(select 1 from [MarkerFileNameSwitchRecord] where cutref = w.cutref)
and isnull(w.CutRef,'') <> ''
group by w.CutRef,w.id,concat(w.MarkerNo,w.Markername,'.gbr'),w.EstCutDate
having sum(w.Layer) -sum(isnull(acc.AccuCuttingLayer,0)) <> 0
";
            DataTable copyFileData;
            DualResult result = DBProxy.Current.Select("Production", sqlcmd, out copyFileData);
            if (!result)
            {
                msg = result.ToString() + "\r\nCreateFile() Sql Error 1";
                return false;
            }

            // 取最新編輯時間的那筆檔案
            var fileList = fileInfos.AsEnumerable().OrderByDescending(s => s.LastWriteTime)
                .GroupBy(s => s.Name, (key, fileGroup) => new
                {
                    Name = key,
                    fileGroup.First().FullName,
                    fileGroup.First().LastWriteTime,
                }).ToList();

            foreach (DataRow item in copyFileData.Rows)
            {
                if (fileList.Where(w => w.Name.ToLower() == MyUtility.Convert.GetString(item["filename"]).ToLower()).Count() == 0)
                {
                    continue;
                }

                string sourceFile = fileList.Where(w => w.Name.ToLower() == MyUtility.Convert.GetString(item["filename"]).ToLower()).Select(s => s.FullName).First().ToString();
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
                        msg = e.Message + $"POID:{item["CuttingID"]}, CutRef:{item["CutRef"]}\r\n" + destFile + "\r\nCreateFile() Error";
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
    ,'{item["CuttingID"]}'
    ,'{item["filename"]}'
    ,'{lastWriteTime}'
    ,GETDATE())
";
                    result = DBProxy.Current.Execute("Production", sqlinsert);
                    if (!result)
                    {
                        msg = result.ToString() + $"POID:{item["CuttingID"]}, CutRef:{item["CutRef"]}\r\n" + "\r\nCreateFile() Sql Error 2";
                        return false;
                    }
                }
            }

            msg = "";
            return true;
        }

        public void FindFile(string dirPath) //引數dirPath為指定的目錄
        {
            //在指定目錄及子目錄下查詢檔案,在listBox1中列出子目錄及檔案
            DirectoryInfo Dir = new DirectoryInfo(dirPath);
            // 不用子目錄下，先註解
            //foreach (DirectoryInfo d in Dir.GetDirectories())//查詢子目錄
            //{
            //    FindFile(Dir + @"\" + d.ToString() + @"\");
            //}
            foreach (FileInfo f in Dir.GetFiles("*.gbr")) //查詢檔案
            {
                fileInfos.Add(f);
            }
        }
    }
}
