using PostJobLog;
using Sci.Production.Class.Command;
using Sci.Production.Prg.PowerBI.DataAccess;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TaskJobCommonLibrary;

namespace Sci.Production.Prg.PowerBI.FormPage
{
    /// <inheritdoc/>
    public partial class Form1 : Form, IFactoryTaskJob
    {
        private List<ExecutedList> executedList;
        private DateTime StratExecutedTime;

        private enum ListName
        {
            P_MonthlySewingOutputSummary,
        }

        /// <inheritdoc/>
        public Form1()
        {
            this.InitializeComponent();
            this.StratExecutedTime = DateTime.Now;
            this.lbTitle.Text = "P_MonthlySewingOutputSummary";
        }

        /// <summary>
        /// 畫面顯示參數
        /// </summary>
        /// <returns>FactoryTaskJobInfo Class</returns>
        public FactoryTaskJobInfo GetJobInfo()
        {
            return new FactoryTaskJobInfo("Power BI", "1.0.0", "工廠端BI執行");
        }

        /// <summary>
        /// 顯示畫面
        /// </summary>
        public void ShowForm()
        {
            this.Show();
        }

        /// <summary>
        /// 實際執行
        /// </summary>
        public void TaskJobRun()
        {
            this.Execute();
        }

        /// <inheritdoc/>
        private void BtnSubmit_Click(object sender, System.EventArgs e)
        {
            this.TaskJobRun();
        }

        private void Execute()
        {
            // 未來做動態產生。
            ExecutedList executed = new ExecutedList()
            {
                ClassName = "P_MonthlySewingOutputSummary",
                SDate = this.dateRange1.Value1,
                EDate = this.dateRange1.Value2,
            };
            this.executedList = new List<ExecutedList>
            {
                executed,
            };

            Logic.Base biBase = new Logic.Base();
            List<ExecutedList> executedLists = new List<ExecutedList>();
            foreach (var item in this.executedList.Where(x => !string.IsNullOrEmpty(x.ClassName)))
            {
                Base_ViewModel result = new Base_ViewModel();
                DateTime? executeSDate = DateTime.Now;
                switch (Enum.Parse(typeof(ListName), item.ClassName))
                {
                    case ListName.P_MonthlySewingOutputSummary:
                        result = new P_Import_MonthlySewingOutputSummary().P_MonthlySewingOutputSummary(item.SDate, item.EDate);
                        break;
                }

                DateTime? executeEDate = DateTime.Now;
                ExecutedList model = new ExecutedList
                {
                    ClassName = item.ClassName,
                    Sucess = result.Result,
                    ErrorMsg = !result.Result ? result.Result.Messages.ToString() : string.Empty,
                    ExecuteSDate = executeSDate,
                    ExecuteEDate = executeEDate,
                };

                executedLists.Add(model);
            }

            string region = biBase.GetRegion();
            string description = string.Join(Environment.NewLine, executedLists.Select(x => $"[{x.ClassName}] is {(x.Sucess ? "completed " : "fail ")} Time: {x.ExecuteSDate.Value.ToString("yyyy/MM/dd HH:mm:ss")} - {x.ExecuteEDate.Value.ToString("yyyy/MM/dd HH:mm:ss")}。 {(x.Sucess ? string.Empty : Environment.NewLine + x.ErrorMsg)}"));
            bool nonSucceed = executedLists.Where(x => !x.Sucess).Count() > 0;

            JobLog jobLog = new JobLog()
            {
                GroupID = "P",
                SystemID = "Power BI",
                Region = region,
                MDivisionID = string.Empty,
                OperationName = "Factory BI transfer",
                StartTime = this.StratExecutedTime.ToString("yyyy/MM/dd HH:mm:ss"),
                EndTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                Description = description,
                FileName = new List<string>(),
                FilePath = string.Empty,
                Succeeded = !nonSucceed,
            };

            string jobLogUkey = biBase.CallJobLogApi(jobLog);

            if (nonSucceed)
            {
                // Send Mail
                string mailTo = biBase.IsTest() ? "jack.hsu@sportscity.com.tw" : "pmshelp@sportscity.com.tw";
                string mailCC = string.Empty;
                string subject = "Import BI Data Error - Facotry";
                string content = $@"
Please check below information.
Transfer date: {DateTime.Now.ToString("yyyy/MM/dd")}
M: {region}
{description}
";
                var email = new MailTo(Env.Cfg.MailFrom, mailTo, mailCC, subject, string.Empty, content, false, true);
            }
        }
    }
}
