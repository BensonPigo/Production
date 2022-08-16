using Ict;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    /// <inheritdoc/>
    public partial class P10 : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateBoxUpdateDate.Value))
            {
                MyUtility.Msg.WarningBox("Update date cannot be empty.");
                return;
            }

            // if (this.dateBoxUpdateDate.Value >= DateTime.Now.Date)
            // {
            //    MyUtility.Msg.WarningBox("<Update Date> must be earlier today");
            //    return;
            // }
            string sqlcmd = $"exec SNPAutoTransferToSewingOutput '{this.dateBoxUpdateDate.Text}' ";

            // 12分鐘  比照排程執行時間
            DBProxy.Current.DefaultTimeout = 7200;
            this.ShowWaitMessage("Update processing....");
            DataTable dtSewingOutput;
            DBProxy.Current.OpenConnection("Production", out SqlConnection sqlConn);
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(7200)))
            using (sqlConn)
            {
                DualResult result = DBProxy.Current.SelectByConn(sqlConn, sqlcmd, out dtSewingOutput);
                if (!result)
                {
                    transactionScope.Dispose();
                    this.HideWaitMessage();
                    this.ShowErr(result);
                    return;
                }

                foreach (DataRow dr in dtSewingOutput.Rows)
                {
                    // 減一天是為了可以跑當天資料
                    DateTime? startOutputDate = MyUtility.Convert.GetDate(dr["OutputDate"]).GetValueOrDefault(this.dateBoxUpdateDate.Value.GetValueOrDefault()).AddDays(-1);
                    result = SewingPrg.ReCheckInlineCategory(dr["SewingLineID"].ToString(), dr["Team"].ToString(), dr["FactoryID"].ToString(), startOutputDate, sqlConn);
                    if (!result)
                    {
                        transactionScope.Dispose();
                        this.HideWaitMessage();
                        this.ShowErr(result);
                        return;
                    }
                }

                transactionScope.Complete();
            }

            this.HideWaitMessage();
            MyUtility.Msg.InfoBox("Updated successfully.");
        }
    }
}
