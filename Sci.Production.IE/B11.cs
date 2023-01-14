using Ict;
using Sci.Data;
using Sci.Win.Tools;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_B11
    /// </summary>
    public partial class B11 : Win.Tems.Input1
    {
        private string oldMoldID;
        /// <summary>
        /// B11
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// ClickEditAfter
        /// </summary>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtID.ReadOnly = true;
        }

        /// <summary>
        /// txtID_Validating
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void TxtID_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtID.Text.Contains(","))
            {
                MyUtility.Msg.WarningBox("<ID> can not have ',' !");

                this.txtID.Text = string.Empty;
            }
        }

        private void TxtMoldID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string cmd = $@"SELECT ID FROM Mold WHERE Junk=0 AND IsTemplate = 1";
            DataTable dt;
            DBProxy.Current.Select(null, cmd, out dt);
            SelectItem item = new SelectItem(cmd, "ID", "10", string.Empty, "ID");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.CurrentMaintain["MoldID"] = item.GetSelectedString();
            this.oldMoldID = this.CurrentMaintain["MoldID"].ToString();
        }

        private void TxtMoldID_Validating(object sender, CancelEventArgs e)
        {
            string newMoldID = this.txtMoldID.Text;
            if (this.oldMoldID != newMoldID)
            {
                if (MyUtility.Check.Empty(newMoldID))
                {
                    this.CurrentMaintain["MoldID"] = string.Empty;
                    this.txtMoldID.Text = string.Empty;
                    return;
                }

                string cmd = $@"SELECT ID FROM Mold WHERE ID = @MoldID AND Junk=0 AND IsTemplate = 1 ";
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@MoldID", newMoldID));

                DataTable dt;
                DualResult r = DBProxy.Current.Select(null, cmd, paras, out dt);

                if (!r)
                {
                    this.ShowErr(r);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    this.CurrentMaintain["MoldID"] = DBNull.Value;
                    this.txtMoldID.Text = string.Empty;
                    this.oldMoldID = string.Empty;
                    MyUtility.Msg.WarningBox("There is no Attachment with [Is Template] checked in B07.");
                }
                else
                {
                    this.CurrentMaintain["MoldID"] = MyUtility.Convert.GetString(dt.Rows[0]["ID"]);
                    this.txtMoldID.Text = newMoldID;
                    this.oldMoldID = this.CurrentMaintain["MoldID"].ToString();
                }
            }
        }
    }
}
