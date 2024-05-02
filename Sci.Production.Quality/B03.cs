using System.Windows.Forms;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Production.Prg.PowerBI.Logic;

namespace Sci.Production.Quality
{
    public partial class B03 : Win.Tems.Input1
    {
        public B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            Sci.Production.Prg.PowerBI.FormPage.Form1 callForm = new Prg.PowerBI.FormPage.Form1();
            callForm.ShowForm();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
        }

        /// <inheritdoc/>
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
