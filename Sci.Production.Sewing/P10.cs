using Ict;
using Sci.Data;
using System;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    public partial class P10 : Win.Tems.QueryForm
    {
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
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            this.HideWaitMessage();

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.Messages.ToString());
                return;
            }
            else
            {
                MyUtility.Msg.InfoBox("Updated successfully.");
            }
        }
    }
}
