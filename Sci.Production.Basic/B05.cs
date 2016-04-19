using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Sci.Production.Class;
using Sci.Data;
using Ict;

namespace Sci.Production.Basic
{
    public partial class B05 : Sci.Win.Tems.QueryForm
    {
        int useAPS;
        public B05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            PropertyInfo info = this.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            info.SetValue(tableLayoutPanel1, true, null);
            useAPS = MyUtility.GetValue.Lookup(string.Format("select UseAPS from Factory where ID = '{0}'", Sci.Env.User.Factory)).ToUpper() == "TRUE" ? 1 : 0;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DateTime firstDate = new DateTime(DateTime.Today.Year - 2, 12, 1);

            // Combox 的月份選項
            for (int i = 0; i < 36; i++)
            {
                firstDate = firstDate.AddMonths(1);
                this.comboBox1.Items.Add(firstDate.ToString("yyyy/MM"));
            }

            //委任日期上的 Click 事件
            for (int i = 1; i <= 35; i++)
            {
                Control[] ctlarray = this.Controls.Find("Holiday" + i.ToString(), true);
                Sci.Production.Class.Holiday holiday = ctlarray[0] as Holiday;
                holiday.label1.Click += new System.EventHandler(this.lable1_Click);
            }

            this.comboBox1.Text = DateTime.Today.ToString("yyyy/MM");
        }

        /// <summary>
        /// 日期上的 Click 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lable1_Click(object sender, EventArgs e)
        {
            if (useAPS == 0)
            {
                Holiday holiday = ((Label)sender).Parent.Parent as Holiday;
                B05_SetHoliday f = new B05_SetHoliday(holiday.Today);
                if (f.ShowDialog() == DialogResult.OK) this.button1.PerformClick();
            }
        }

        /// <summary>
        /// 更新整個月的行事曆
        /// </summary>
        /// <param name="yyyymm"></param>
        private void ShowDate(string yyyymm)
        {
            for (int i = 1; i <= 35; i++)
            {
                Control[] ctlarray = this.Controls.Find("Holiday" + i.ToString(), true);
                Sci.Production.Class.Holiday holiday = ctlarray[0] as Holiday;
                holiday.Visible = false;
            }

            if (string.IsNullOrWhiteSpace(yyyymm)) return;

            int yyyy = int.Parse(yyyymm.Substring(0, 4));
            int mm = int.Parse(yyyymm.Substring(yyyymm.Length - 2));
            DateTime firstDate = new DateTime(yyyy, mm, 1);
            int dayofweek = (int)firstDate.DayOfWeek;
            for (int i = 0; i < DateTime.DaysInMonth(yyyy, mm); i++)
            {
                DateTime date = firstDate.AddDays(i);
                int cells = (i + dayofweek + 1);
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
                DBProxy.Current.Select(null, string.Format("select * from holiday where HolidayDate='{0}' and FactoryID = '{1}'", date.ToString("d"), Sci.Env.User.Factory), out findData);
                if (findData == null || findData.Rows.Count <= 0)
                {
                    holiday.label2.Text = "";
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

        private void comboBox1_Validated(object sender, EventArgs e)
        {
            this.button1.PerformClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ShowDate(this.comboBox1.Text);
        }
    }
}
