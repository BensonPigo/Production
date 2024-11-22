using Sci.Data;
using Sci.Production.Class;
using System;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <summary>
    /// B12
    /// </summary>
    public partial class B12 : Win.Tems.QueryForm
    {
        /// <summary>
        /// B12
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DoubleBuffered = true;
            PropertyInfo info = this.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            info.SetValue(this.tableLayoutPanel1, true, null);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.SetComboYearMonth();
        }

        private void SetComboYearMonth()
        {
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
                Holiday holiday = ctlarray[0] as Holiday;
                holiday.label1.Click += new EventHandler(this.Lable1_Click);
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
            Holiday holiday = ((Label)sender).Parent.Parent as Holiday;
            B12_SetHoliday f = new B12_SetHoliday(holiday.Today, this.comboDropDownList1.SelectedValue.ToString());
            if (f.ShowDialog() == DialogResult.OK)
            {
                this.btnRefersh.PerformClick();
            }
        }

        /// <summary>
        /// 更新整個月的行事曆
        /// </summary>
        private void ShowDate()
        {
            string yyyymm = this.comboYearMonth.Text;
            for (int i = 1; i <= 35; i++)
            {
                Control[] ctlarray = this.Controls.Find("Holiday" + i.ToString(), true);
                Holiday holiday = ctlarray[0] as Holiday;
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
                    cells -= 35;
                }

                Control[] ctlarray = this.Controls.Find("Holiday" + cells.ToString(), true);
                Holiday holiday = ctlarray[0] as Holiday;
                holiday.Visible = true;
                holiday.label1.Text = date.Day.ToString();
                holiday.Today = date;
                string sqlcmd = $@"
select *
from MachineIoTHoliday WITH (NOLOCK)
where HolidayDate ='{date:yyyy/MM/dd}'
AND MachineIoTType = '{this.comboDropDownList1.SelectedValue}'
AND FactoryID = '{Sci.Env.User.Factory}'
";
                DBProxy.Current.Select("ManufacturingExecution", sqlcmd, out DataTable findData);
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
            this.ShowDate();
        }

        private void ComboDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ShowDate();
        }

        private void BtnImportFromPMS_Click(object sender, EventArgs e)
        {
            new B12_ImportFromPMS(this.comboDropDownList1.SelectedValue.ToString()).ShowDialog();
            this.btnRefersh.PerformClick();
        }
    }
}
