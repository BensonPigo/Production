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

            return base.ClickSaveBefore();
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
    }
}
