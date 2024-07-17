using Newtonsoft.Json.Converters;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// IE_B02
    /// </summary>
    public partial class IE_B01 : Win.Tems.Input1
    {
        /// <summary>
        /// B02
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public IE_B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        /// <summary>
        /// OnDetailEntered()
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
        }

        /// <summary>
        /// ClickSaveBefore()
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            #region 檢查必填
            if (MyUtility.Check.Empty(this.CurrentMaintain["No"]) || MyUtility.Check.Empty(this.CurrentMaintain["CheckList"]))
            {
                MyUtility.Msg.WarningBox("Please fill in both <No> and <Check List>!!", "Warning");
                return false;
            }
            #endregion
            #region 檢查重複
            DataTable[] dt;
            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@No", Value = this.CurrentMaintain["No"].ToString() });
            cmds.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@CheckList", Value = this.CurrentMaintain["CheckList"].ToString() });
            cmds.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@ID", Value = this.CurrentMaintain["ID"].ToString() });
            string sql = @"
            select ID From ChgOverCheckListBase where No = @No and ID != @ID 
            select ID From ChgOverCheckListBase where CheckList = @CheckList and ID != @ID
            ";
            DBProxy.Current.Select("ProductionTPE", sql, cmds, out dt);
            string message = string.Empty;
            if (dt[0].Rows.Count > 0)
            {
                message += "This No: <No> already exists!" + Environment.NewLine;
            }

            if (dt[1].Rows.Count > 0)
            {
                message += "This CheckList: <CheckList> already exists!" + Environment.NewLine;
            }

            if (!message.Empty())
            {
                MyUtility.Msg.WarningBox(message, "Warning");
                return false;
            }

            #endregion

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["No"] = DBNull.Value;
        }
    }
}
