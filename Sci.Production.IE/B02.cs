using System;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_B02
    /// </summary>
    public partial class B02 : Win.Tems.Input1
    {
        /// <summary>
        /// B02
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Env.User.Keyword);
        }

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(this.comboType, 2, 1, "LBR,Line Balancing (%),LLER,Lean Line Eff. (%),EFF.,Efficiency,COPT,Changeover Process Time,COT,Changeover Time");
        }

        /// <summary>
        /// OnDetailEntered()
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (this.CurrentMaintain["Type"].ToString().Trim() == "COPT" || this.CurrentMaintain["Type"].ToString().Trim() == "COT")
            {
                this.labelTarget.Text = "Target (min)";
            }
            else
            {
                this.labelTarget.Text = "Target (%)";
            }
        }

        /// <summary>
        /// ClickNewAfter()
        /// </summary>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["EffectiveDate"] = DateTime.Today;
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
        }

        /// <summary>
        /// ClickEditAfter()
        /// </summary>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.dateDate.ReadOnly = true;
            this.comboType.ReadOnly = true;
        }

        /// <summary>
        /// ClickCopyAfter()
        /// </summary>
        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            this.CurrentMaintain["ID"] = DBNull.Value;
        }

        /// <summary>
        /// ClickSaveBefore()
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["EffectiveDate"]))
            {
                MyUtility.Msg.WarningBox("< Date > can not be empty!");
                this.dateDate.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Type"]))
            {
                MyUtility.Msg.WarningBox("< Type > can not be empty!");
                this.comboType.Focus();
                return false;
            }

            return base.ClickSaveBefore();
        }

        private void ComboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboType.SelectedIndex != -1)
            {
                switch (this.comboType.SelectedValue.ToString())
                {
                    case "COPT":
                        this.labelTarget.Text = "Target (min)";
                        break;
                    case "COT":
                        this.labelTarget.Text = "Target (min)";
                        break;
                    default:
                        this.labelTarget.Text = "Target (%)";
                        break;
                }
            }
            else
            {
                this.labelTarget.Text = "Target";
            }
        }
    }
}
