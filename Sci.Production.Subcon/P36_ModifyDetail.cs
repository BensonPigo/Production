using System.Collections.Generic;
using System.ComponentModel;

namespace Sci.Production.Subcon
{
    public partial class P36_ModifyDetail : Sci.Win.Subs.Input6A
    {
        public P36_ModifyDetail()
        {
            this.InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (this.EditMode)
            {
                if (this.CurrentData["TaipeiDBC"].ToString() == "True")
                {
                    this.txtSPNo.ReadOnly = true;
                    this.numClaimAmt.ReadOnly = true;
                    this.numAffectQty.ReadOnly = true;
                    this.numAdditionCharge.ReadOnly = true;
                    this.txtUnit.TextBox1.ReadOnly = true;
                }
                else
                {
                    this.txtSPNo.ReadOnly = false;
                    this.numClaimAmt.ReadOnly = false;
                    this.numAffectQty.ReadOnly = false;
                    this.numAdditionCharge.ReadOnly = false;
                    this.txtUnit.TextBox1.ReadOnly = false;
                }
            }
        }

        private void txtSPNo_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                return;
            }

            #region -- sql parameters declare --

            System.Data.SqlClient.SqlParameter sp_id = new System.Data.SqlClient.SqlParameter();
            sp_id.ParameterName = "@id";

            List<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            sp_id.Value = this.txtSPNo.Text;
            cmds.Add(sp_id);
            #endregion
            if (!MyUtility.Check.Seek(
                string.Format(
                @"select orders.id from dbo.orders WITH (NOLOCK) 
inner join factory WITH (NOLOCK) on orders.FactoryID = factory.id
where orders.FtyGroup='{0}' 
and factory.IsProduceFty = 1 
and orders.id = @id", Sci.Env.User.Factory), cmds))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("SP# is not found, Please check value is right and belong to login factory!!");
                return;
            }
        }
    }
}
