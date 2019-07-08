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
using System.Runtime.InteropServices;

namespace SendSewingDailyOutputList
{
    public partial class Main : Sci.Win.Tems.Input7
    {
        bool isAuto = false;
        DataRow mailTo;
        DataTable dt;
        string excelFile;
        TransferPms transferPMS = new TransferPms();
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
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

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
            sqlCmd = "Select * From dbo.MailTo Where ID = '020'";

            DualResult result = DBProxy.Current.Select("Production", sqlCmd, out _mailTo);

            if (!result) { ShowErr(result); return; }

            mailTo = _mailTo.Rows[0];

            this.tpeMisMail = MyUtility.GetValue.Lookup("Select ToAddress From dbo.MailTo Where ID = '101'");
        }

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

            if (!isFail)
            {
                toAddress += MyUtility.Check.Empty(toAddress) ? this.tpeMisMail : ";" + this.tpeMisMail;
            }

            if (String.IsNullOrEmpty(subject))
            {
                subject = mailTo["Subject"].ToString() + DateTime.Now.ToString("yyyy/MM/dd");
            }
            if (String.IsNullOrEmpty(desc))
            {
                desc = mailTo["Description"].ToString();
            }

            if (!MyUtility.Check.Empty(toAddress))
            {
                if (MyUtility.Check.Empty(excelFile))
                {
                    Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(sendFrom, toAddress, ccAddress, subject, "", desc, true, true);
                    mail.ShowDialog();
                }
                else
                {
                    Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(sendFrom, toAddress, ccAddress, subject, excelFile, desc, true, true);
                    mail.ShowDialog();
                }

            }
        }
        #endregion

        #region 執行SendMail
        private void ClickExport()
        {
            DualResult result = execSewingDailyOutput();
            mymailTo(result.ToSimpleString());

            if (!result)
            {
                ShowErr(result);
            }

            issucess = true;
        }

        private void mymailTo(string msg)
        {
            String subject = "";
            String desc = "";

            #region 組合 Desc
            string res = "Success";
            if (!MyUtility.Check.Empty(msg))
            {
                res = "Error";
                issucess = false;
            }

            desc = $@"
Hi all,
This mail is system autimatically generate from Sewing R04(Sewing daily output list). This message is automatically sent, please do not reply directly!
Output date: {DateTime.Now.ToString("yyyy/MM/dd")}
Region:{this.CurrentData["RgCode"]}

";
            #endregion

            subject = $"{mailTo["Subject"]} {DateTime.Now.ToString("yyyy/MM/dd")} ({this.CurrentData["RgCode"]})";
            if (issucess)
            {
                SendMail(subject, desc, issucess);
            }
            else
            {
                desc += $@"
Result : {res}

{ msg}
                ";
                SendMail(subject, desc, issucess);
            }
            
        }
        #endregion
        bool issucess = true;
        #region 執行Send_SewingDailyOutput撈資料
        private DualResult execSewingDailyOutput()
        {
            try
            {
                string sqlcmd = $"exec [dbo].[Send_SewingDailyOutput] ";
                DualResult result;
                result = DBProxy.Current.Select("", sqlcmd, out dt);
                if (!result)
                {
                    issucess = false;
                }

                #region 產生Excel
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Sewing_R04_SewingDailyOutputList.xltx"); //預先開啟excel app
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得
                objSheets.get_Range("AM:AN").EntireColumn.Delete();
                for (int i = 40; i < this.dt.Columns.Count; i++)
                {
                    objSheets.Cells[1, i + 1] = this.dt.Columns[i].ColumnName;
                }

                string r = MyUtility.Excel.ConvertNumericToExcelColumn(this.dt.Columns.Count);
                objSheets.get_Range("A1", r + "1").Cells.Interior.Color = Color.LightGreen;
                objSheets.get_Range("A1", r + "1").AutoFilter(1);
                objApp.Visible = false;

                if (dt.Rows.Count != 0)
                {
                    MyUtility.Excel.CopyToXls(dt, "", "Sewing_R04_SewingDailyOutputList.xltx", 1, false, null, objApp);
                }
                excelFile = GetName("Sewing daily output list -");
                objApp.ActiveWorkbook.SaveAs(excelFile);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);

                #endregion
            }
            catch (SqlException se)
            {
                issucess = false;
                return Ict.Result.F(se);
            }
            return Ict.Result.True;
        }
        #endregion        

        private void btnTestMail_Click(object sender, EventArgs e)
        {
            SendMail($"{mailTo["Subject"]} {DateTime.Now.ToString("yyyy/MM/dd")} Test", $"{mailTo["Description"]}");
        }

        /// <summary>
        /// Get Microsoft File Name
        /// </summary>
        /// <param name="ProcessName">主檔名</param>
        /// <param name="NameExtension">副檔名，預設 xlsx</param>
        /// <returns>路徑+檔名+副檔名</returns>
        private string GetName(string ProcessName, string NameExtension = ExcelFileNameExtension.Xlsx)
        {
            string fileName = ProcessName.Trim()
                               + DateTime.Now.ToString("_yyMMdd_HHmmssfff")
                               + NameExtension;
            return Path.Combine(Sci.Env.Cfg.ReportTempDir, fileName);
        }

        private static class ExcelFileNameExtension
        {
            public const string Xlsm = ".xlsm", Xlsx = ".xlsx";
        }
    }
}

