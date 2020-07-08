using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class B03 : Sci.Win.Tems.Input1
    {
        public B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
        }

        protected override bool ClickSaveBefore()
        {
            #region 必輸檢查
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.txtScaleCode.Focus();
                MyUtility.Msg.WarningBox("< Scale Code > can not be empty!");
                return false;
            }

            #endregion
            return base.ClickSaveBefore();
        }
    }
}
