using System.Windows.Forms;
using Ict;

namespace Sci.Production.SubProcess
{
    /// <summary>
    /// SubProcess_B01
    /// </summary>
    public partial class B01 : Win.Tems.Input1
    {
        /// <summary>
        /// B01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// ClickNewAfter
        /// </summary>
        protected override void ClickNewAfter()
        {
            this.txtType.ReadOnly = false;
            this.txtFeature.ReadOnly = false;
            base.ClickNewAfter();
        }

        /// <summary>
        /// ClickEditBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickEditBefore()
        {
            this.txtType.ReadOnly = true;
            this.txtFeature.ReadOnly = true;
            return base.ClickEditBefore();
        }

        /// <summary>
        /// ClickSaveBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.txtType.Text))
            {
                this.txtType.Focus();
                MyUtility.Msg.WarningBox("Type cannot be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtFeature.Text))
            {
                this.txtFeature.Focus();
                MyUtility.Msg.WarningBox("Feature cannot be empty!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <summary>
        /// ClickUndo
        /// </summary>
        protected override void ClickUndo()
        {
            this.txtType.ReadOnly = true;
            this.txtFeature.ReadOnly = true;
            base.ClickUndo();
        }

        /// <summary>
        /// ClickSave
        /// </summary>
        /// <returns>DualResult</returns>
        protected override DualResult ClickSave()
        {
            this.txtFeature.ReadOnly = true;
            this.txtType.ReadOnly = true;
            return base.ClickSave();
        }

        // Type 右鍵開窗
        private void TxtType_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                @"Select  Id,ArtworkTypeId  from  subprocess  where isselection=1 and Junk=0  order by Id ",
                "10,20",
                this.txtType.Text);
            DialogResult rtnResl = item.ShowDialog();
            if (rtnResl == DialogResult.Cancel)
            {
                return;
            }

            this.txtType.Text = item.GetSelectedString();
        }

        // Type 檢核
        private void TxtType_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string txtValue = this.txtType.Text;
            if (MyUtility.Check.Empty(this.txtType.Text) || txtValue != this.txtType.OldValue)
            {
                if (!MyUtility.Check.Seek(string.Format(
                    @"Select  Id from  subprocess  
where isselection=1 and Junk=0 and id='{0}'", txtValue)))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Type: {0}> not found !", txtValue));
                    this.txtType.Text = string.Empty;
                    return;
                }
            }
        }
    }
}
