using Ict;
using Sci.Production.Prg.PowerBI.Model;
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
        private List<ExecutedList> executedList = new List<ExecutedList>();

        /// <inheritdoc/>
        public Form1()
        {
            this.InitializeComponent();

            this.SetFormHeightAndWidth();
            this.executedList = new Logic.Base().GetExecuteList();
            this.AddControl();
        }

        private void SetFormHeightAndWidth()
        {
            Screen screen = Screen.PrimaryScreen;
            this.Height = (int)Math.Round(screen.Bounds.Height * 0.8, 0);
            this.Width = (int)Math.Round(screen.Bounds.Width * 0.9, 0);
        }

        /// <summary>
        /// 畫面顯示參數
        /// </summary>
        /// <returns>FactoryTaskJobInfo Class</returns>
        public FactoryTaskJobInfo GetJobInfo()
        {
            return new FactoryTaskJobInfo("Power BI", "1.2.1", "工廠端BI執行, 增加群組機制, 調整排序");
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
            this.AutoExecute();
        }

        private void AddControl()
        {
            foreach (ExecutedList item in this.executedList.Where(x => !string.IsNullOrEmpty(x.ClassName)))
            {
                UserControl_Detail detail = new UserControl_Detail
                {
                    Execute = item,
                };
                detail.Init();
                this.flowLayoutPanel1.Controls.Add(detail);
            }
        }

        private void AutoExecute()
        {
            Logic.Base biBase = new Logic.Base();
            if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
            {
                this.executedList.RemoveAll(x => x.RunOnSunday);
            }

            // 執行尚未跑的
            this.executedList = this.executedList
                .Where(x => !x.TransferDate.HasValue
                            || x.TransferDate.Value < DateTime.Today)
                .ToList();

            biBase.ExecuteAll(this.executedList);
        }

        /// <inheritdoc/>
        private void BtnSubmit_Click(object sender, System.EventArgs e)
        {
            List<ExecutedList> executedList = new List<ExecutedList>();
            foreach (Control control in this.flowLayoutPanel1.Controls)
            {
                UserControl_Detail detailFrom = (UserControl_Detail)control;
                Control detailFromControl = detailFrom.Controls["panelBase"].Controls["flowLayoutPanel1"];

                string className = ((TextBox)detailFromControl.Controls["panel_BIname"].Controls["txtBIname"]).Text.ToString();
                var query = this.executedList.Where(x => x.ClassName == className).Select(x => x);
                if (!query.Any())
                {
                    continue;
                }

                ExecutedList item = new ExecutedList()
                {
                    ClassName = className,
                };

                bool isExecute = false;
                foreach (Control detailControl in detailFromControl.Controls)
                {
                    switch (detailControl.Name)
                    {
                        case "panel_DateRange1":
                            Win.UI.DateRange dateRange = (Win.UI.DateRange)detailControl.Controls["dateRange1"];
                            item.SDate = dateRange.Value1;
                            item.EDate = dateRange.Value2;
                            break;
                        case "panel_DateRange2":
                            Win.UI.DateRange dateRange2 = (Win.UI.DateRange)detailControl.Controls["dateRange2"];
                            item.SDate2 = dateRange2.Value1;
                            item.EDate2 = dateRange2.Value2;
                            break;
                        case "panel_Date":
                            Win.UI.DateBox dateBox1 = (Win.UI.DateBox)detailControl.Controls["dateBox1"];
                            item.SDate = dateBox1.Value;
                            break;
                        case "tableLayoutPanel1":
                            CheckBox checkbox = (CheckBox)detailControl.Controls["chkRun"];
                            isExecute = checkbox.Checked;
                            break;
                    }
                }

                if (isExecute)
                {
                    ExecutedList itemBase = query.FirstOrDefault();
                    item.ProcedureName = itemBase.ProcedureName;
                    item.DBName = itemBase.DBName;
                    item.RunOnSunday = itemBase.RunOnSunday;
                    item.Group = itemBase.Group;
                    item.SEQ = itemBase.SEQ;
                    if (MyUtility.Check.Empty(item.SDate) && itemBase.Source.ToUpper() == "SP")
                    {
                        item.SDate = MyUtility.Convert.GetDate("1911/01/01");
                    }

                    if (MyUtility.Check.Empty(item.EDate) && itemBase.Source.ToUpper() == "SP")
                    {
                        item.EDate = MyUtility.Convert.GetDate("2999/12/31");
                    }

                    executedList.Add(item);
                }
            }

            if (executedList.Count() > 0)
            {
                Logic.Base biBase = new Logic.Base();
                biBase.ExecuteAll(executedList);
            }
        }

        /// <inheritdoc/>
        private void BtnReSet_Click(object sender, EventArgs e)
        {
            this.chkAllCheck.Checked = false;
            foreach (Control control in this.flowLayoutPanel1.Controls)
            {
                UserControl_Detail detailFrom = (UserControl_Detail)control;
                Control detailFromControl = detailFrom.Controls["panelBase"].Controls["flowLayoutPanel1"];
                string className = ((TextBox)detailFromControl.Controls["panel_BIname"].Controls["txtBIname"]).Text.ToString();
                var query = this.executedList.Where(x => x.ClassName == className).Select(x => x);
                if (!query.Any())
                {
                    continue;
                }

                ExecutedList item = query.FirstOrDefault();
                foreach (Control detailControl in detailFromControl.Controls)
                {
                    switch (detailControl.Name)
                    {
                        case "panel_DateRange1":
                            Win.UI.DateRange dateRange = (Win.UI.DateRange)detailControl.Controls["dateRange1"];
                            dateRange.Value1 = item.SDate.Value;
                            dateRange.Value2 = item.EDate.Value;
                            break;
                        case "panel_DateRange2":
                            Win.UI.DateRange dateRange2 = (Win.UI.DateRange)detailControl.Controls["dateRange2"];
                            dateRange2.Value1 = item.SDate2.Value;
                            dateRange2.Value2 = item.EDate2.Value;
                            break;
                        case "panel_Date":
                            Win.UI.DateBox dateBox1 = (Win.UI.DateBox)detailControl.Controls["dateBox1"];
                            dateBox1.Value = item.SDate.Value;
                            break;
                        case "tableLayoutPanel1":
                            CheckBox checkbox = (CheckBox)detailControl.Controls["chkRun"];
                            checkbox.Checked = false;
                            break;
                    }
                }
            }
        }

        /// <inheritdoc/>
        private void ChkAllCheck_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control control in this.flowLayoutPanel1.Controls)
            {
                UserControl_Detail detailFrom = (UserControl_Detail)control;
                CheckBox checkbox = (CheckBox)detailFrom.Controls["panelBase"].Controls["flowLayoutPanel1"].Controls["tableLayoutPanel1"].Controls["chkRun"];
                checkbox.Checked = this.chkAllCheck.Checked;
            }
        }
    }
}
