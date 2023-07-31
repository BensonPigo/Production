using Ict;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P74 : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P74(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            MyUtility.Tool.SetupCombox(this.cbPreparation, 2, 1, @"W,Warehouse Finished Preparation,F,Factory Received");
        }

        private void CbPreparation_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lblFinishedandReceoved.Text = this.cbPreparation.SelectedIndex == 0 ? "Finished By" : "Received By";
        }

        private void TxtTransaction_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtTransaction.Text))
            {
                return;
            }

            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            listSqlPar.Add(new SqlParameter("@TransactionID", this.txtTransaction.Text));

            string sqlcmd = $@"
            SELECT
            [FinishedBy] = ll.WHFinishedBy,
            [ReceivedBy] = ll.FtyReceivedBy,
            [Requset] = ll.RequestID,
            [Type] = case ll.Type
                        when 'L' then 'Lacking'
                        when 'R' then 'Replacement'
                        else ll.Type
                    end,
            [IssueDate] = ll.IssueDate,
            [ApvDate] = l.ApvDate,
            [Status] = ll.[Status],
            [Department] = l.Dept,
            [FinishedDate] = ll.WHFinishedDate,
            [ReceivedDate] = ll.FtyReceivedDate
            FROM IssueLack ll WITH(NOLOCK)
            inner join Lack l WITH(NOLOCK) on ll.RequestID = l.ID
            WHERE ll.ID = @TransactionID"; 
            DualResult dualResult = DBProxy.Current.Select(null, sqlcmd, listSqlPar, out DataTable dt);

            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
            }

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox($"Transaction ID : <{this.txtTransaction.Text}> cannot found.");
                this.Allempty();
                e.Cancel = true;
                return;
            }

            this.txtRequset.Text = dt.Rows[0]["Requset"].ToString();
            this.txtType.Text = dt.Rows[0]["Type"].ToString();
            this.dtIssueDate.Value = MyUtility.Convert.GetDate(dt.Rows[0]["IssueDate"]);
            this.txtApvDate.Text = dt.Rows[0]["ApvDate"].ToString();
            this.txtStatus.Text = dt.Rows[0]["Status"].ToString();
            this.txtDepartment.Text = dt.Rows[0]["Department"].ToString();
            this.txtFinishedBy.Text = dt.Rows[0]["FinishedBy"].ToString();
            this.txtFinishedDate.Text = dt.Rows[0]["FinishedDate"].ToString();
            this.txtReceivedBy.Text = dt.Rows[0]["ReceivedBy"].ToString();
            this.txtReceivedDate.Text = dt.Rows[0]["ReceivedDate"].ToString();
            this.txtFinished.Focus();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string str = this.cbPreparation.SelectedIndex == 0 ? "Finished" : "Received";
            if (MyUtility.Check.Empty(this.txtTransaction.Text))
            {
                MyUtility.Msg.WarningBox("Transaction ID cannot be empty.");
                this.txtTransaction.Focus();
                return;
            }

            if (MyUtility.Check.Empty(this.txtFinished.Text))
            {
                MyUtility.Msg.WarningBox($" {str} by cannot be empty");
                this.txtFinished.Focus();
                return;
            }

            string strMessage = string.Empty;

            if (this.cbPreparation.SelectedIndex == 0)
            {
                if (!MyUtility.Check.Empty(this.txtFinishedBy.Text) || !MyUtility.Check.Empty(this.txtFinishedDate.Text))
                {
                    strMessage = "[Warehouse Finished Preparation] have been scanned, please check if want to update.";
                }
            }
            else
            {
                if (!MyUtility.Check.Empty(this.txtReceivedBy.Text) || !MyUtility.Check.Empty(this.txtReceivedDate.Text))
                {
                    strMessage = "[Factory Received] have been scanned, please check if want to update.";
                }
            }

            if (strMessage != string.Empty)
            {
                DialogResult confirmResult = MessageBoxEX.Show($@"{strMessage}", "Save", MessageBoxButtons.YesNo, new string[] { "Continue", "Cancel" }, MessageBoxDefaultButton.Button2);
                if (confirmResult.EqualString("No"))
                {
                    return;
                }
            }

            string sqlcmd = string.Empty;
            if (this.cbPreparation.SelectedIndex == 0)
            {
                sqlcmd = $@"UPdate IssueLack set WHFinishedDate = GetDate(), WHFinishedBy = '{this.txtFinished.Text}' where id = '{this.txtTransaction.Text}'";
            }
            else
            {
                sqlcmd = $@"UPdate IssueLack set FtyReceivedDate = GetDate(), FtyReceivedBy = '{this.txtFinished.Text}' where id = '{this.txtTransaction.Text}'";
            }

            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.Allempty();
            this.txtTransaction.Focus();
        }

        private void Allempty()
        {
            this.txtTransaction.Text = string.Empty;
            this.txtRequset.Text = string.Empty;
            this.txtType.Text = string.Empty;
            this.dtIssueDate.Text = string.Empty;
            this.txtApvDate.Text = string.Empty;
            this.txtStatus.Text = string.Empty;
            this.txtDepartment.Text = string.Empty;
            this.txtFinishedBy.Text = string.Empty;
            this.txtFinishedDate.Text = string.Empty;
            this.txtReceivedBy.Text = string.Empty;
            this.txtReceivedDate.Text = string.Empty;
            this.txtFinished.Text = string.Empty;
        }
    }
}
