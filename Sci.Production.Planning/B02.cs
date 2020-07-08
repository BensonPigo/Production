using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Sci.Production.Planning
{
    /// <summary>
    /// B02
    /// </summary>
    public partial class B02 : Win.Tems.Input1
    {
        private bool NeedCheck = false;  // 存檔時是否要檢核。新增時要檢核。修改時，若[BEGIN][END]有異動，才要檢核。
        private bool BeginEndChange = false;  // 紀錄[BEGIN][END]是否有異動。

        /// <summary>
        /// B02
        /// </summary>
        /// <param name="menuitem">PlanningB02</param>
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// ClickSaveBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["BeginStitch"].ToString()))
            {
                this.numBeginStitch.Focus();
                MyUtility.Msg.WarningBox("< Begin Stitch > can not be empty!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["EndStitch"].ToString()))
            {
                this.numEndStitch.Focus();
                MyUtility.Msg.WarningBox("< End Stitch > can not be empty!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["Batchno"].ToString()))
            {
                this.numBatchNumber.Focus();
                MyUtility.Msg.WarningBox("< Batch Number > can not be empty!");
                return false;
            }

            #region 新增時要檢核。修改時，若[BEGIN][END]有異動，才要檢核。
            if (this.IsDetailInserting)
            {
                this.NeedCheck = true;
            }
            else if (this.IsDetailUpdating)
            {
                if (this.BeginEndChange)
                {
                    this.NeedCheck = true;
                }
            }

            if (!this.NeedCheck)
            {
                this.NeedCheck = false;
                return true;
            }
            #endregion

            string s1 = @"SELECT * FROM [Production].[dbo].[EmbBatch] WITH (NOLOCK)
                          where BeginStitch <> @OldBegin and EndStitch <> @OldEnd
                                and not((BeginStitch > @begin  and BeginStitch > @end) or (EndStitch <  @begin and EndStitch < @end))";

            #region 準備sql參數資料
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@begin";
            sp1.Value = this.CurrentMaintain["beginstitch"].ToString();

            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            sp2.ParameterName = "@end";
            sp2.Value = this.CurrentMaintain["Endstitch"].ToString();

            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
            sp3.ParameterName = "@OldBegin";
            sp3.Value = this.numBeginStitch.OldValue;

            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
            sp4.ParameterName = "@OldEnd";
            sp4.Value = this.numEndStitch.OldValue;

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);
            cmds.Add(sp4);

            #endregion

            bool flag = false;
            Sci.Data.DBProxy.Current.Exists(null, s1, cmds, out flag);
            if (flag)
            {
                this.NeedCheck = false;
                MyUtility.Msg.WarningBox("This Data ranage already cover existed data");
                return false;
            }

            this.NeedCheck = false;
            return base.ClickSaveBefore();
        }

        private void NumBeginStitch_Validating(object sender, CancelEventArgs e)
        {
            string value = this.numBeginStitch.Text;
            if (value != this.numBeginStitch.OldValue.ToString())
            {
                this.BeginEndChange = true;
            }
        }

        private void NumEndStitch_Validating(object sender, CancelEventArgs e)
        {
            string value = this.numEndStitch.Text;
            if (value != this.numEndStitch.OldValue.ToString())
            {
                this.BeginEndChange = true;
            }
        }
    }
}
