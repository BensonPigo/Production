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
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Configuration;

namespace MailQMS_Fabric_Inspection_record
{
    public partial class Main : Sci.Win.Tems.Input7
    {
        bool isAuto = false;
        DataRow mailTo;
        TransferPms transferPMS = new TransferPms();
        StringBuilder sqlmsg = new StringBuilder();
        private DataTable printData;
        private string pathName;
        private string MailList_ForError = ConfigurationManager.AppSettings["MailList_ForError"].ToString();

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
            sqlCmd = "Select * From dbo.MailTo Where ID = '024'";

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
        private void SendMail(string subject = "", string desc = "", bool isFail = false)
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
                toAddress = MailList_ForError;
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
                Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(sendFrom, toAddress, ccAddress, subject, pathName, desc, true, true);

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

            //result = AsyncUpdateExport();
            if (isAuto)
            {
                result = AsyncUpdateExport();
            }
            else
            {
                result = AsyncHelper.Current.DataProcess(this, () =>
                {
                    return AsyncUpdateExport();
                });
            }

            if (this.printData == null)
            {
                mymailTo(result.ToString());
                return;
            }

            List<string> files = this.CreateExcel_OneSheet();

            if (files.Count > 0)
            {

                mymailTo(result.ToString());
            }
            else
            {

                mymailTo(result.ToString());
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

            if (!MyUtility.Check.Empty(msg))
            {
                desc += "Sql msg:" + Environment.NewLine +
                    msg + Environment.NewLine;
                issucess = false;
            }
            #endregion

            subject = mailTo["Subject"].ToString().TrimEnd();
            if (issucess)
            {
                subject += " " + DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            }
            else
            {
                subject += " " + DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd") + " Error!";
            }

            SendMail(subject, desc, !issucess);
            #endregion
        }
        #endregion
        bool issucess = true;
        #region Export/Update (非同步)
        private DualResult AsyncUpdateExport()
        {
            try
            {
                string sqlcmd = string.Empty;
                sqlcmd = $@"
select
[SP#] = F.POID 
,[SEQ] =  F.Seq1+'-'+F.Seq2 
, PSD.Refno
, PSD.ColorID
, [Dyelot] = FP.Dyelot
, [Roll] = FP.Roll
, [ReceivingQty] = FP.TicketYds
, [Inspection Lacking Qty] = FP.TicketYds-FP.ActualYds
, [StockType] = IIF(isnull(RD.StockType,'') !='', case when RD.StockType ='B' then 'Bulk'
							 when rd.StockType = 'I' then 'Inventory' else '' end,
					   case when TD.StockType ='B' then 'Bulk'
							 when TD.StockType = 'I' then 'Inventory' else '' end)
, [Stock deduct Qty] = isnull(DeductQty.Qty,0)
, [Transaction#] = isnull(FP.TransactionID,'')
, [Encode] = iif(F.PhysicalEncode = 1 , 'Y','N')
from production..FIR F
inner join production..FIR_Physical FP on F.ID=FP.ID
inner join production..PO_Supp_Detail PSD on F.POID=PSD.id and F.SEQ1=PSD.SEQ1 and F.SEQ2=PSD.SEQ2
Left join production..Receiving_Detail RD on F.ReceivingID=RD.Id and F.POID=RD.PoId and F.SEQ1=RD.Seq1 and F.SEQ2=RD.Seq2 and FP.Roll=RD.Roll and FP.Dyelot=RD.Dyelot 
Left join production..TransferIn_Detail TD on F.ReceivingID=TD.Id and F.POID=TD.PoId and F.SEQ1=TD.Seq1 and F.SEQ2=TD.Seq2 and FP.Roll=TD.Roll and FP.Dyelot=TD.Dyelot 
outer apply
(
	select Qty = sum(qty) from 
	(
		select Qty = Qty from production.dbo.SplitString(FP.TransactionID,',') tsNo
		inner join production..ReturnReceipt_Detail rrd on rrd.Id = tsNo.Data
			union all
		select Qty = ad.QtyAfter from production.dbo.SplitString(FP.TransactionID,',') tsNo
		inner join production..Adjust_Detail ad on ad.Id = tsNo.Data
	) a
) DeductQty
where 1=1
and CONVERT(date, FP.AddDate) = CONVERT(date, DATEADD(DD,-1,GETDATE())) --昨天
and FP.ActualYds < FP.TicketYds --僅限短碼資料
";

                DualResult result = DBProxy.Current.Select(null, sqlcmd, null, out this.printData);
                if (!result)
                {
                    return result;
                }

            }
            catch (SqlException se)
            {
                issucess = false;
                return Ict.Result.F(se);
            }

            return Ict.Result.True;
        }
        #endregion

        private List<string> CreateExcel_OneSheet()
        {
            List<string> files = new List<string>();
            try
            {
                Ict.Logs.APP.LogInfo("Create XLT Start");
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\QMS_Fabric_inspection_record.xltx"); //預先開啟excel app
                objApp.Visible = false;

                if (printData.Rows.Count > 0)
                {
                    MyUtility.Excel.CopyToXls(this.printData, null, "QMS_Fabric_inspection_record.xltx", headerRow: 1, excelApp: objApp, showExcel: false, showSaveMsg: false); // 將datatable copy to excel
                }

                Ict.Logs.APP.LogInfo("Create XLT Start Report");

                #region Save Excel
                pathName = GetName("QMS_Fabric_inspection_record");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(pathName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(objApp);
                #endregion

                files.Add(pathName);

                return files;

            }
            catch (Exception ex)
            {
                Ict.Logs.APP.LogInfo("Create Excel Error : " + ex.ToString());
                this.CallJobLogApi("QMS_Fabric_inspection_record -Create excel ereor", ex.ToString(), DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), false, false);
                return new List<string>();
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
            this.CallJobLogApi("MailQMS_Fabric_Inspection_record Test", "MailQMS_Fabric_Inspection_record Test", DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), true, true);
        }

        private void btnTestMail_Click(object sender, EventArgs e)
        {
            SendMail("MailQMS_Fabric_Inspection_record Test", "MailQMS_Fabric_Inspection_record Test");
        }
    }
}
