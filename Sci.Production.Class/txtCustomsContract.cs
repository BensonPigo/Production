using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    public partial class txtCustomsContract : Win.UI.TextBox
    {
        private bool _CheckDate = false;
        private bool _CheckStatus = true;

        public txtCustomsContract()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }

        [Description("篩選出今天（GetDate()）可以使用的合約")]
        public bool CheckDate
        {
            get { return this._CheckDate; }
            set { this._CheckDate = value; }
        }

        [Description("判斷是否需要判斷合約的狀態（Confirmed）")]
        public bool CheckStatus
        {
            get { return this._CheckStatus; }
            set { this._CheckStatus = value; }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            string sqlcmd = "select ID,StartDate,EndDate from VNContract WITH (NOLOCK) where 1=1";
            if (this.CheckDate)
            {
                sqlcmd = sqlcmd + " and GETDATE() between StartDate and EndDate ";
            }

            if (this.CheckStatus)
            {
                sqlcmd = sqlcmd + " and Status = 'Confirmed' ";
            }

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "15,10,10", this.Text, headercaptions: "Contract No.,Start Date, End Date");
            item.Size = new System.Drawing.Size(550, 375);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
            this.ValidateText();
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string str = this.Text;
            string sqlcmd = "select ID from VNContract WITH (NOLOCK) where ID = '{0}'";
            if (this.CheckDate)
            {
                sqlcmd = sqlcmd + " and GETDATE() between StartDate and EndDate ";
            }

            if (this.CheckStatus)
            {
                sqlcmd = sqlcmd + " and Status = 'Confirmed' ";
            }

            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (!MyUtility.Check.Seek(string.Format("select ID from VNContract WITH (NOLOCK) where ID = '{0}'", str)))
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Contract no. not found!!");
                    return;
                }
                else if (!MyUtility.Check.Seek(string.Format(sqlcmd, str)))
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("This Contract can't use.");
                    return;
                }
            }
        }
    }
}
