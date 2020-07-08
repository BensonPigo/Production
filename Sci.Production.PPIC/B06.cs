using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// B06
    /// </summary>
    public partial class B06 : Sci.Win.Tems.Input1
    {
        private string useAPS;

        /// <summary>
        /// B06
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            string sqlCommand = "select UseAPS from factory WITH (NOLOCK) where ID = '" + Sci.Env.User.Factory + "'";
            this.useAPS = MyUtility.GetValue.Lookup(sqlCommand, null);

            // string sqlCommand2 = "select IsSampleRoom from factory where ID = '" + Sci.Env.User.Factory + "'";
            // string IsSampleRoom = MyUtility.GetValue.Lookup(sqlCommand2, null);
            // if (IsSampleRoom == "False")
            // {
            //    IsSupportCopy = false;
            //    IsSupportDelete = false;
            //    IsSupportEdit = false;
            //    IsSupportNew = false;
            // }
            this.InitializeComponent();
            if (this.useAPS.ToUpper() == "TRUE")
            {
                this.IsSupportCopy = false;
                this.IsSupportDelete = false;
                this.IsSupportNew = false;
            }

            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "'";
            this.txtCellNo.MDivisionID = Sci.Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtLine.ReadOnly = true;

            if (this.useAPS.ToUpper() == "TRUE")
            {
                this.txtDescription.ReadOnly = true;
                this.txtCellNo.ReadOnly = true;
                this.numNoOfSewers.ReadOnly = true;
                this.checkJunk.ReadOnly = true;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["ID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Line# > can not be empty!");
                this.txtLine.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["Description"].ToString()) && this.useAPS.ToUpper() != "TRUE")
            {
                MyUtility.Msg.WarningBox("< Description > can not be empty!");
                this.txtDescription.Focus();
                return false;
            }

            return base.ClickSaveBefore();
        }

        private void TxtLine_Validating(object sender, CancelEventArgs e)
        {
            // 當輸入的值只有一個位元且介於0~9時，自動在此值前面補數字’0’
            if (!string.IsNullOrWhiteSpace(this.txtLine.Text) && this.txtLine.Text.Trim().Length == 1)
            {
                char idValue = Convert.ToChar(this.txtLine.Text.Trim().Substring(0, 1));
                if (Convert.ToInt32(idValue) >= 48 && Convert.ToInt32(idValue) <= 57)
                {
                    this.CurrentMaintain["ID"] = "0" + this.txtLine.Text.Trim();
                }
            }
        }
    }
}
