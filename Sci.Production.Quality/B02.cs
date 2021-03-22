using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class B02 : Win.Tems.Input1
    {
        /// <inheritdoc/>
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            MyUtility.Tool.SetupCombox(this.queryfors, 2, 1, "0,Exclude Junk,1,Include Junk");

            // 預設查詢為 Exclude Junk
            this.queryfors.SelectedIndex = 0;
            this.DefaultWhere = "JUNK = 0";
            this.ReloadDatas();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                string hasJunk = MyUtility.Check.Empty(this.queryfors.SelectedValue) ? string.Empty : this.queryfors.SelectedValue.ToString();
                switch (hasJunk)
                {
                    case "0":
                        this.DefaultWhere = "JUNK = 0";
                        break;
                    case "1":
                    default:
                        this.DefaultWhere = string.Empty;
                        break;
                }

                this.ReloadDatas();
            };
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;
        }

        /// <inheritdoc/>
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
