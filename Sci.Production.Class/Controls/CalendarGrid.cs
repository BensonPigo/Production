using Ict;
using Sci.Production.Class.Command;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Sci.Production.Class.Controls
{
    /// <inheritdoc/>
    public partial class CalendarGrid : Win.UI._UserControl
    {
        /// <inheritdoc/>
        public Color ColorCrossDay { get; set; } = Color.Pink;

        /// <inheritdoc/>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Custom Properties")]
        [Description("Enable or disable the header buttons.")]
        public bool EnableHearder
        {
            get => this.enableHearder;
            set
            {
                if (this.enableHearder != value)
                {
                    this.enableHearder = value;
                    this.UpdateButtonStates(); // 在屬性變更時更新按鈕狀態
                }
            }
        }

        private bool enableHearder = false;

        /// <inheritdoc/>
        public CalendarGrid()
        {
            this.InitializeComponent();
            this.GridCalendar_SizeChanged(null, null);

            // 初始設定按鈕啟用狀態
            this.UpdateButtonStates();
        }

        /// <summary>
        /// 更新所有按鈕的啟用狀態
        /// </summary>
        private void UpdateButtonStates()
        {
            this.btnday1.Enabled =
            this.btnday2.Enabled =
            this.btnday3.Enabled =
            this.btnday4.Enabled =
            this.btnday5.Enabled =
            this.btnday6.Enabled =
            this.btnday7.Enabled = this.EnableHearder;
        }

        private void GridCalendar_SizeChanged(object sender, EventArgs e)
        {
            int widthStart = this.btnday1.Width / 2;
            int widthEnd = this.btnday1.Width - widthStart;
            for (int i = 0; i <= 5; i++)
            {
                this.gridCalendar.Columns[i * 2].Width = widthStart;
                this.gridCalendar.Columns[(i * 2) + 1].Width = widthEnd;
            }

            int widthStart6 = (this.gridCalendar.Width - (this.btnday1.Width * 6)) / 2;
            int widthEnd6 = this.btnday7.Width - widthStart6;
            this.gridCalendar.Columns[12].Width = widthStart6 - 1;
            this.gridCalendar.Columns[13].Width = widthEnd6 - 1;
        }

        private void GridCalendar_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataRow dr = ((Sci.Win.UI.Grid)sender).GetDataRow(e.RowIndex);

            // e.ColumnIndex = 0 ~ 13
            int weekDay = (e.ColumnIndex / 2) + 1;
            if (MyUtility.Convert.GetBool(dr[$"IsCrossDate{weekDay}"]))
            {
                e.CellStyle.BackColor = this.ColorCrossDay;
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }
        }

        /// <summary>
        /// 有結構但是空的
        /// </summary>
        public void SetDataSource()
        {
            this.listControlBindingSource1.DataSource = new MachineCalendar().SetWeekDayDataTable();
        }

        /// <summary>
        /// 傳入的 DataTable 直接綁上
        /// </summary>
        /// <param name="dt">已經轉為WeekDayDataTable</param>
        /// <inheritdoc/>
        public void SetDataSource(DataTable dt, bool tranfer = false)
        {
            this.listControlBindingSource1.DataSource = tranfer ? new MachineCalendar().ProcessMachineToWeekDayDataTable(dt) : dt;
        }

        /// <inheritdoc/>
        public void SetDataSource(long machineIoT_CalendareUkey)
        {
            this.listControlBindingSource1.DataSource = new MachineCalendar().GetWeekDayDataTable(machineIoT_CalendareUkey);
        }
    }
}