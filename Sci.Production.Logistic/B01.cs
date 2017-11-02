using System.Windows.Forms;

namespace Sci.Production.Logistic
{
    public partial class B01 : Sci.Win.Tems.Input1
    {
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Sci.Env.User.Keyword + "'";
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;
        }

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

        protected override bool ClickPrint()
        {
            Sci.Production.Logistic.B01_Print callNextForm = new Sci.Production.Logistic.B01_Print(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }
    }
}
