using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class B22 : Win.Tems.Input1
    {
        public B22(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = $"MDivisionID ='{Env.User.Keyword}'";
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.chkJunk.ReadOnly = !this.Perm.Junk;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;
            this.chkJunk.ReadOnly = !this.Perm.Junk;
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("<Code> can not be empty!");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
