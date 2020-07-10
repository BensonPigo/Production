using Ict;
using System.Windows.Forms;

namespace Sci.Production.SubProcess
{
    /// <summary>
    /// SubProcess_B03
    /// </summary>
    public partial class B03 : Win.Tems.Input1
    {
        /// <summary>
        /// B03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = $"MDivisionID = '{Env.User.Keyword}'";
        }

        /// <summary>
        /// ClickNewAfter
        /// </summary>
        protected override void ClickNewAfter()
        {
            this.txtType.ReadOnly = false;
            this.txtID.ReadOnly = false;
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            base.ClickNewAfter();
        }

        /// <summary>
        /// ClickEditBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickEditBefore()
        {
            this.txtType.ReadOnly = true;
            this.txtID.ReadOnly = true;
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

            if (MyUtility.Check.Empty(this.txtID.Text))
            {
                this.txtID.Focus();
                MyUtility.Msg.WarningBox("ID cannot be empty!");
                return false;
            }

            if (MyUtility.Convert.GetDecimal(this.CurrentMaintain["Manpower"]) >= 1000)
            {
                this.numManPower.Focus();
                MyUtility.Msg.WarningBox("ManPower cannot more than 999.99");
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
            this.txtID.ReadOnly = true;
            base.ClickUndo();
        }

        /// <summary>
        /// ClickSave
        /// </summary>
        /// <returns>DualResult</returns>
        protected override DualResult ClickSave()
        {
            this.txtType.ReadOnly = true;
            this.txtID.ReadOnly = true;
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
