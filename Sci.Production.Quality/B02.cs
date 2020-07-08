using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class B02 : Sci.Win.Tems.Input1
    {
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            #region 必輸檢查
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.txtCode.Focus();
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Description"]))
            {
                this.editDescription.Focus();
                MyUtility.Msg.WarningBox("< Description > can not be empty!");
                return false;
            }

            #endregion
            return base.ClickSaveBefore();
        }
    }
}
