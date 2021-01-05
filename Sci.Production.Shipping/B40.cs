using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class B40 : Win.Tems.Input1
    {
        private string editName;
        private DateTime? editDate;

        /// <inheritdoc/>
        public B40(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboType, 2, 1, "F,Fabric,A,Accessory");
            this.labelDescription2.Text = "Description\r\n(Detail)";
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            string sqlcmd = $@"
select top 1 HsCode
from Fabric_HsCode h
where h.SCIRefno = '{this.CurrentMaintain["SCIRefno"]}'
order by h.Year desc, h.ukey desc";
            this.displayT2HSCode.Text = MyUtility.GetValue.Lookup(sqlcmd);
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            this.editName = MyUtility.Convert.GetString(this.CurrentMaintain["EditName"]);
            this.editDate = MyUtility.Convert.GetDate(this.CurrentMaintain["EditDate"]);
            this.txtNLCode.MainDataRow = this.CurrentMaintain;
            this.txtNLCode2.MainDataRow = this.CurrentMaintain;
            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.editDescription2.ReadOnly = true;
            this.comboType.ReadOnly = true;
            this.txtUnitUsageUnit.TextBox1.ReadOnly = true;
            this.numUsableWidth.ReadOnly = true;
            this.checkJunk.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            this.CurrentMaintain["NLCodeEditName"] = Env.User.UserID;
            this.CurrentMaintain["NLCodeEditDate"] = DateTime.Now;

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult ClickSavePost()
        {
            string updateCmd;
            if (MyUtility.Check.Empty(this.editDate))
            {
                updateCmd = string.Format("update Fabric Set EditName = '{0}', EditDate = null where SCIRefNo = '{1}';", this.editName, MyUtility.Convert.GetString(this.CurrentMaintain["SCIRefNo"]));
            }
            else
            {
                updateCmd = string.Format("update Fabric Set EditName = '{0}', EditDate = '{1}' where SCIRefNo = '{2}';", this.editName, Convert.ToDateTime(this.editDate).ToString("yyyy/MM/dd HH:mm:ss"), MyUtility.Convert.GetString(this.CurrentMaintain["SCIRefNo"]));
            }

            return DBProxy.Current.Execute(null, updateCmd);
        }
    }
}
