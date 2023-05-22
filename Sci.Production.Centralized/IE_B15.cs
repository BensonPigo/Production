using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// IE_B15
    /// </summary>
    public partial class IE_B15 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// IE_B15
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public IE_B15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.ConnectionName = "ProductionTPE";
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCentralizedmulitM.ReadOnly = true;
            this.txtCentralizedmulitFactory.ReadOnly = true;
            this.comboFunction.ReadOnly = true;
            this.comboVertify.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.CheckHistoryExists();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["MDivisionID"]))
            {
                MyUtility.Msg.WarningBox("[M] cannot be empty.");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
            {
                MyUtility.Msg.WarningBox("[Factory] cannot be empty.");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["FunctionIE"]))
            {
                MyUtility.Msg.WarningBox("[Function] cannot be empty.");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Verify"]))
            {
                MyUtility.Msg.WarningBox("[Verify] cannot be empty.");
                return false;
            }

            string sqlCheckExists = $@"
select 1 from AutomatedLineMappingConditionSetting with (nolock)
where   MDivisionID = '{this.CurrentMaintain["MDivisionID"]}' and
        FactoryID = '{this.CurrentMaintain["FactoryID"]}' and
        FunctionIE = '{this.CurrentMaintain["FunctionIE"]}' and
        Verify = '{this.CurrentMaintain["Verify"]}' 
";

            if (MyUtility.Check.Seek(sqlCheckExists, "ProductionTPE"))
            {
                MyUtility.Msg.WarningBox($"<M>, <Factory>, <Function>, <Verify> already exists");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            if (this.CurrentMaintain.RowState == DataRowState.Modified)
            {
                Dictionary<string, string> needHistoryCol = new Dictionary<string, string>();
                needHistoryCol.Add("Condition1", "1");
                needHistoryCol.Add("Condition2", "2");
                needHistoryCol.Add("Condition3", "3");
                needHistoryCol.Add("Junk", "Junk");

                string sqlInsertAutomatedLineMappingConditionSetting_History = string.Empty;
                foreach (KeyValuePair<string, string> colName in needHistoryCol)
                {
                    if (this.CurrentMaintain[colName.Key, DataRowVersion.Original].Equals(this.CurrentMaintain[colName.Key, DataRowVersion.Current]))
                    {
                        continue;
                    }

                    sqlInsertAutomatedLineMappingConditionSetting_History += $@"
insert into AutomatedLineMappingConditionSetting_History(
                AutomatedLineMappingConditionSettingUkey,
                HisType,
                OldValue,
                NewValue,
                AddName,
                AddDate)
        values( {this.CurrentMaintain["Ukey"]},
                '{colName.Value}',
                '{this.CurrentMaintain[colName.Key, DataRowVersion.Original]}',
                '{this.CurrentMaintain[colName.Key, DataRowVersion.Current]}',
                '{Env.User.UserID}',
                getdate()
                )
";
                }

                if (!MyUtility.Check.Empty(sqlInsertAutomatedLineMappingConditionSetting_History))
                {
                    DualResult result = DBProxy.Current.Execute("ProductionTPE", sqlInsertAutomatedLineMappingConditionSetting_History);
                    if (!result)
                    {
                        return result;
                    }
                }
            }

            return base.ClickSave();
        }

        private void ConditionChange()
        {
            string conditionType = $"{this.comboFunction.SelectedValue}_{this.comboVertify.SelectedValue}";
            this.flowLayoutCondition1.Visible = false;
            this.flowLayoutCondition2.Visible = false;
            this.flowLayoutCondition3.Visible = false;
            this.numericBoxCondition1.Value = 0;
            this.numericBoxCondition2.Value = 0;
            this.numericBoxCondition3.Value = 0;

            switch (conditionType)
            {
                case "IE_P05_LBRByGSD":
                    this.flowLayoutCondition1.Visible = true;
                    this.labelCondition1Desc.Text = "1. LBR By GSD Time <";
                    break;
                case "IE_P05_TargetReason":
                case "IE_P06_TargetReason":
                    this.flowLayoutCondition1.Visible = true;
                    this.flowLayoutCondition2.Visible = true;
                    this.flowLayoutCondition3.Visible = true;
                    this.labelCondition1Desc.Text = "1. [Operator Loading (Final)] >";
                    this.labelCondition2Desc.Text = "2. [Operator Loading (Auto)] >";
                    this.labelCondition3Desc.Text = "3. [Operator Loading (Final)] > [Operator Loading (Auto)] and [Operator Loading (Final)]>";
                    break;
                case "IE_P06_LBRByCycle":
                    this.flowLayoutCondition1.Visible = true;
                    this.labelCondition1Desc.Text = "1. LBR By Cycle Time <";
                    break;
                default:
                    break;
            }

            // 讓label自動調整寬度，調回false是為了讓label可以垂直置中
            this.labelCondition1Desc.AutoSize = true;
            this.labelCondition1Desc.AutoSize = false;
            this.labelCondition2Desc.AutoSize = true;
            this.labelCondition2Desc.AutoSize = false;
            this.labelCondition3Desc.AutoSize = true;
            this.labelCondition3Desc.AutoSize = false;
        }

        private void ComboFunction_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.comboFunction.SelectedIndex < 0)
            {
                return;
            }

            this.ConditionChange();
        }

        private void ComboVertify_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.comboVertify.SelectedIndex < 0)
            {
                return;
            }

            this.ConditionChange();
        }

        private void BtnHistory_Click(object sender, EventArgs e)
        {
            if (!this.CheckHistoryExists())
            {
                return;
            }

            new IE_B15_History(MyUtility.Convert.GetLong(this.CurrentMaintain["Ukey"])).ShowDialog();
        }

        private bool CheckHistoryExists()
        {
            if (MyUtility.Check.Seek($"select 1 from AutomatedLineMappingConditionSetting_History with (nolock) where AutomatedLineMappingConditionSettingUkey = '{this.CurrentMaintain["Ukey"]}'", "ProductionTPE"))
            {
                this.btnHistory.ForeColor = Color.Blue;
                this.btnHistory.Font = new Font(this.btnHistory.Font, FontStyle.Bold);
            }
            else
            {
                this.btnHistory.ForeColor = Color.Black;
                this.btnHistory.Font = new Font(this.btnHistory.Font, FontStyle.Regular);
                return false;
            }

            return true;
        }
    }
}
