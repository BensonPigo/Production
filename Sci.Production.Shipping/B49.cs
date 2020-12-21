using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class B49 : Win.Tems.Input1
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="B49"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B49(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "NeedDeclare = 1";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.txtSubconSupplier.TextBox1.ReadOnly = true;
            this.txtSubconSupplier.TextBox1.IsSupportEditMode = false;
            base.OnFormLoaded();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            this.txtCustomerCode.MainDataRow = this.CurrentMaintain;
            this.txtNLCode2.MainDataRow = this.CurrentMaintain;
            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            // 修改表身資料,不寫入表頭EditName and EditDate
            ITableSchema pass1Schema;
            var ok = DBProxy.Current.GetTableSchema("Machine", "Misc", out pass1Schema);
            pass1Schema.IsSupportEditDate = false;
            pass1Schema.IsSupportEditName = false;

            decimal pcslength = MyUtility.Check.Empty(this.CurrentMaintain["PcsLength"]) ? 0 : MyUtility.Convert.GetDecimal(this.CurrentMaintain["PcsLength"]);
            decimal pcsWidth = MyUtility.Check.Empty(this.CurrentMaintain["PcsWidth"]) ? 0 : MyUtility.Convert.GetDecimal(this.CurrentMaintain["PcsWidth"]);
            decimal pcsKG = MyUtility.Check.Empty(this.CurrentMaintain["PcsKg"]) ? 0 : MyUtility.Convert.GetDecimal(this.CurrentMaintain["PcsKg"]);
            decimal miscRate = MyUtility.Check.Empty(this.CurrentMaintain["MiscRate"]) ? 0 : MyUtility.Convert.GetDecimal(this.CurrentMaintain["MiscRate"]);

            string strUpdate = $@"
update Misc
set UsageUnit = '{this.CurrentMaintain["UsageUnit"]}'
,NLCode  = '{this.CurrentMaintain["NLCode"]}'
,NLCode2  = '{this.CurrentMaintain["NLCode2"]}'
,CustomsUnit = '{this.CurrentMaintain["CustomsUnit"]}'
,HSCode = '{this.CurrentMaintain["HSCode"]}'
,PcsLength = '{pcslength}'
,PcsWidth = '{pcsWidth}'
,PcsKg = '{pcsKG}'
,MiscRate = {miscRate}
,NLCodeEditDate = '{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}'
,NLCodeEditName = '{Env.User.UserID}'
where id='{this.CurrentMaintain["ID"]}'
";
            DualResult result;
            if (!(result = DBProxy.Current.Execute("Machine", strUpdate)))
            {
                this.ShowErr(result);
            }

            return Ict.Result.True;
        }
    }
}
