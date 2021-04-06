using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Production.Class;
using System.Linq;
using Ict.Win;
using System.Linq.Dynamic;
using Sci.Win.Tools;
using System.Transactions;
using System.Threading.Tasks;
using Sci.Production.Automation;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class P18_PackingError : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P18_PackingError()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.EditMode = true;
            string sqlcmd = $@"
select ID = '', Description = ''
union all

select ID, Description from PackingReason where Type = 'ER' and junk = 0 order by ID";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Tool.SetupCombox(this.cbmPackingError, 2, dt);
        }

        private void TxtAuditQC_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtAuditQC.Text.Length > 30)
            {
                MyUtility.Msg.WarningBox("Audit QC Name cannot exceed 30 characters!!");
                this.txtAuditQC.Text = string.Empty;
                return;
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (this.cbmPackingError.Items.Count == 1)
            {
                MyUtility.Msg.WarningBox("Please go to <Packing_B02. Packing Error Type Reason> to add PackingError Type data");
                return;
            }

            if (MyUtility.Check.Empty(this.cbmPackingError.SelectedValue))
            {
                MyUtility.Msg.WarningBox("<Packing Error> cannot be empty!!");
                return;
            }

            P18.P18_PackingError = this.cbmPackingError.SelectedValue.ToString();
            P18.P18_PackingErrorQty = MyUtility.Convert.GetInt(this.numErrorQty.Value);
            P18.P18_PackingAuditQC = this.txtAuditQC.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}
