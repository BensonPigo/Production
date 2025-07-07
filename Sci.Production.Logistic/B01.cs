using Ict;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Class.Command;
using System.Windows.Forms;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic_B01
    /// </summary>
    public partial class B01 : Win.Tems.Input1
    {
        /// <summary>
        /// Logistic_B01
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Env.User.Keyword + "'";
        }

        /// <summary>
        /// ClickNewAfter()
        /// </summary>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
        }

        /// <summary>
        /// ClickEditAfter()
        /// </summary>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;
        }

        /// <summary>
        /// ClickSaveBefore()
        /// </summary>
        /// <returns>base.ClickSaveBefore()</returns>
        protected override bool ClickSaveBefore()
        {
            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["ID"].ToString()))
            {
                this.txtCode.Focus();
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["Description"].ToString()))
            {
                this.txtDescription.Focus();
                MyUtility.Msg.WarningBox("< Description > can not be empty!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.txtLine.Text) || string.IsNullOrWhiteSpace(this.txtHeight.Text) || string.IsNullOrWhiteSpace(this.txtPoint.Text))
            {
                this.txtDescription.Focus();
                MyUtility.Msg.WarningBox("<Line> & <Height> & <Point> can not be empty!");
                return false;
            }

            string sql = $@"Select ID from ClogLocation where MDivisionID = '{Env.User.Keyword}' and ID = '{this.txtLine.Text}-{this.txtHeight.Text}-{this.txtPoint.Text}'  AND ID <> '{this.txtCode.Text}'";
            string existCL = DBProxy.Current.LookupEx<string>(sql).ExtendedData;

            if (!existCL.IsNullOrWhiteSpace())
            {
                MyUtility.Msg.WarningBox($"Code : <{existCL}> already exists!");
                return false;
            }
            else
            {
                this.txtCode.Text = this.txtLine.Text + "-" + this.txtHeight.Text + "-" + this.txtPoint.Text;
            }

            return base.ClickSaveBefore();
        }

        protected override void ClickSaveAfter()
        {
            string sql = $@"Update ClogLocation set ID = '{this.txtLine.Text}-{this.txtHeight.Text}-{this.txtPoint.Text}'
                            Where ID = '{this.txtCode.Text}'";

            this.txtCode.Text = this.txtLine.Text + "-" + this.txtHeight.Text + "-" + this.txtPoint.Text;

            DualResult result = DBProxy.Current.Execute(null, sql);
            base.ClickSaveAfter();
        }

        /// <summary>
        /// ClickPrint()
        /// </summary>
        /// <returns>base.ClickPrint()</returns>
        protected override bool ClickPrint()
        {
            B01_Print callNextForm = new B01_Print(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }

        private void TxtPoint_TextChanged(object sender, System.EventArgs e)
        {
            if (this.txtHeight.Text.Length > 999)
            {
                this.txtHeight.Text = this.txtHeight.Text.Substring(0, 3);
                this.txtHeight.SelectionStart = this.txtHeight.Text.Length;
            }
        }

        private void TxtHeight_TextChanged(object sender, System.EventArgs e)
        {
            if (this.txtHeight.Text.Length > 99)
            {
                this.txtHeight.Text = this.txtHeight.Text.Substring(0, 2);
                this.txtHeight.SelectionStart = this.txtHeight.Text.Length;
            }
        }

        private void TxtHeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // 阻止非數字輸入
            }
        }

        private void TxtPoint_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // 阻止非數字輸入
            }
        }
    }
}
