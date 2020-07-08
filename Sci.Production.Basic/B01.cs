using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B01
    /// </summary>
    public partial class B01 : Win.Tems.Input1
    {
        /// <summary>
        /// B01
        /// </summary>
        /// <param name="menuitem"> menuitem</param>
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Sci.Env.User.Keyword);
        }

        /// <summary>
        /// OnDetailEntered
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            // 按鈕Shipping Mark變色
            if (MyUtility.Check.Seek(this.CurrentMaintain["ID"].ToString(), "Factory_TMS", "ID") ||
                MyUtility.Check.Seek(this.CurrentMaintain["ID"].ToString(), "Factory_WorkHour", "ID"))
            {
                this.btnCapacityWorkday.ForeColor = Color.Blue;
            }
            else
            {
                this.btnCapacityWorkday.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// ClickEditAfter
        /// </summary>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;
        }

        /// <summary>
        /// ClickSaveBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                this.txtCode.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["NameEN"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Name > can not be empty!");
                this.txtName.Focus();
                return false;
            }

            // MDivisionID為登入的ID
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;

            return base.ClickSaveBefore();
        }

        private void BtnCapacityWorkday_Click(object sender, EventArgs e)
        {
            B01_CapacityWorkDay callNextForm = new B01_CapacityWorkDay(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }
    }
}
