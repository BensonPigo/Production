using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    public partial class B02 : Sci.Win.Tems.Input1
    {
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DefaultFilter = string.Format("MDivisionID = '{0}'",Sci.Env.User.Keyword);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(comboType, 2, 1, "LBR,Line Balancing (%),LLER,Lean Line Eff. (%),EFF.,Efficiency,COPT,Changeover Process Time,COT,Changeover Time");
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if ((CurrentMaintain["Type"].ToString().Trim() == "COPT" || CurrentMaintain["Type"].ToString().Trim() == "COT"))
            {
                this.labelTarget.Text = "Target (min)";
            }
            else
            {
                this.labelTarget.Text = "Target (%)";
            }
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["EffectiveDate"] = DateTime.Today;
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.dateDate.ReadOnly = true;
            this.comboType.ReadOnly = true;
        }

        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            CurrentMaintain["ID"] = DBNull.Value;
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["EffectiveDate"]))
            {
                MyUtility.Msg.WarningBox("< Date > can not be empty!");
                this.dateDate.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Type"]))
            {
                MyUtility.Msg.WarningBox("< Type > can not be empty!");
                this.comboType.Focus();
                return false;
            }
            return base.ClickSaveBefore();
        }

        private void comboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboType.SelectedIndex != -1)
            {
                switch (comboType.SelectedValue.ToString())
                {
                    case "COPT":
                        labelTarget.Text = "Target (min)";
                        break;
                    case "COT":
                        labelTarget.Text = "Target (min)";
                        break;
                    default:
                        labelTarget.Text = "Target (%)";
                        break;
                }
            }
            else
            {
                labelTarget.Text = "Target";
            }
        }   
    }
}
