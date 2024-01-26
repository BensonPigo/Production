using Sci.Production.Prg.PowerBI.Model;
using System.Windows.Forms;

namespace Sci.Production.Prg.PowerBI.FormPage
{
    /// <inheritdoc/>
    public partial class Form_Detail : Form
    {
        /// <inheritdoc/>
        public Form_Detail()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        public Form_Detail(ExecutedList execute)
        {
            this.InitializeComponent();

            // 加入 Control
            this.FlowLayoutPanelControlAdd(execute);

            this.SetDefaultValue(execute);

            // this.btnSubmit.Click += (sender, e) =>
            // {
            //    this.Execute(execute);
            // };
        }

        private void FlowLayoutPanelControlAdd(ExecutedList execute)
        {
            int height = 60;
            if (execute.SDate.HasValue && execute.EDate.HasValue)
            {
                this.dateRange1.Value1 = execute.SDate.Value;
                this.dateRange1.Value2 = execute.EDate.Value;
                this.flowLayoutPanel1.Controls.Add(this.panel_DateRange1);
                height += this.panel_DateRange1.Height;
            }

            if (execute.SDate2.HasValue && execute.EDate2.HasValue)
            {
                this.dateRange2.Value1 = execute.SDate2.Value;
                this.dateRange2.Value2 = execute.EDate2.Value;
                this.flowLayoutPanel1.Controls.Add(this.panel_DateRange2);
                height += this.panel_DateRange2.Height;
            }

            if (execute.SDate.HasValue && !execute.EDate.HasValue)
            {
                this.dateBox1.Value = execute.SDate.Value;
                this.flowLayoutPanel1.Controls.Add(this.panel_Date);
                height += this.panel_Date.Height;
            }

            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel1);
            height += this.tableLayoutPanel1.Height;
            this.Height = height;
        }

        private void SetDefaultValue(ExecutedList execute)
        {
            this.Text = execute.ClassName;
            this.txtRemark.Text = execute.Remark;
        }
    }
}
