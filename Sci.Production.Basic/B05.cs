using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using Sci.Production.Class;
using Sci.Data;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B05
    /// </summary>
    public partial class B05 : Sci.Win.Tems.QueryForm
    {
        private int useAPS;

        /// <summary>
        /// B05
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DoubleBuffered = true;
            PropertyInfo info = this.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            info.SetValue(this.tableLayoutPanel1, true, null);
            this.useAPS = MyUtility.GetValue.Lookup(string.Format("select UseAPS from Factory WITH (NOLOCK) where ID = '{0}'", Sci.Env.User.Factory)).ToUpper() == "TRUE" ? 1 : 0;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DateTime firstDate = new DateTime(DateTime.Today.Year - 2, 12, 1);

            // Combox 的月份選項
            for (int i = 0; i < 36; i++)
            {
                firstDate = firstDate.AddMonths(1);
                this.comboYearMonth.Items.Add(firstDate.ToString("yyyy/MM"));
            }

            // 委任日期上的 Click 事件
            for (int i = 1; i <= 35; i++)
            {
                Control[] ctlarray = this.Controls.Find("Holiday" + i.ToString(), true);
                Sci.Production.Class.Holiday holiday = ctlarray[0] as Holiday;
                holiday.label1.Click += new System.EventHandler(this.Lable1_Click);
            }

            this.comboYearMonth.Text = DateTime.Today.ToString("yyyy/MM");
        }

        /// <summary>
        /// 日期上的 Click 事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Lable1_Click(object sender, EventArgs e)
        {
            if (this.useAPS == 0)
            {
                Holiday holiday = ((Label)sender).Parent.Parent as Holiday;
                B05_SetHoliday f = new B05_SetHoliday(holiday.Today);
                if (f.ShowDialog() == DialogResult.OK)
                {
                    this.btnRefersh.PerformClick();
                }
            }
        }

        /// <summary>
        /// 更新整個月的行事曆
        /// </summary>
        /// <param name="yyyymm">yyyymm</param>
        private void ShowDate(string yyyymm)
        {
            for (int i = 1; i <= 35; i++)
            {
                Control[] ctlarray = this.Controls.Find("Holiday" + i.ToString(), true);
                Sci.Production.Class.Holiday holiday = ctlarray[0] as Holiday;
                holiday.Visible = false;
            }

            if (string.IsNullOrWhiteSpace(yyyymm))
            {
                return;
            }

            int yyyy = int.Parse(yyyymm.Substring(0, 4));
            int mm = int.Parse(yyyymm.Substring(yyyymm.Length - 2));
            DateTime firstDate = new DateTime(yyyy, mm, 1);
            int dayofweek = (int)firstDate.DayOfWeek;
            for (int i = 0; i < DateTime.DaysInMonth(yyyy, mm); i++)
            {
                DateTime date = firstDate.AddDays(i);
                int cells = i + dayofweek + 1;
                if (cells > 35)
                {
                    cells = cells - 35;
                }

                Control[] ctlarray = this.Controls.Find("Holiday" + cells.ToString(), true);
                Sci.Production.Class.Holiday holiday = ctlarray[0] as Holiday;
                holiday.Visible = true;
                holiday.label1.Text = date.Day.ToString();
                holiday.Today = date;
                DataTable findData;
                DBProxy.Current.Select(null, string.Format("select * from holiday WITH (NOLOCK) where HolidayDate='{0}' and FactoryID = '{1}'", date.ToString("d"), Sci.Env.User.Factory), out findData);
                if (findData == null || findData.Rows.Count <= 0)
                {
                    holiday.label2.Text = string.Empty;
                }
                else
                {
                    holiday.label2.Text = findData.Rows[0]["Name"].ToString();
                    holiday.label2.ForeColor = Color.Red;
                }

                if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    holiday.label1.ForeColor = Color.Red;
                }

                if (date.DayOfWeek == DayOfWeek.Saturday)
                {
                    holiday.label1.ForeColor = Color.Green;
                }
            }
        }

        private void ComboYearMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnRefersh.PerformClick();
        }

        private void BtnRefersh_Click(object sender, EventArgs e)
        {
            this.ShowDate(this.comboYearMonth.Text);
        }
    }
}
