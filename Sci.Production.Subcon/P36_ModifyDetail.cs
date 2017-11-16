using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class P36_ModifyDetail : Sci.Win.Subs.Input6A
    {
        public P36_ModifyDetail()
        {
            InitializeComponent();
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (EditMode)
            {
                if (CurrentData["TaipeiDBC"].ToString() == "True")
                {
                    txtSPNo.ReadOnly = true;
                    numClaimAmt.ReadOnly = true;
                    numAffectQty.ReadOnly = true;
                    numAdditionCharge.ReadOnly = true;
                    txtUnit.TextBox1.ReadOnly = true;
                }
                else
                {
                    txtSPNo.ReadOnly = false;
                    numClaimAmt.ReadOnly = false;
                    numAffectQty.ReadOnly = false;
                    numAdditionCharge.ReadOnly = false;
                    txtUnit.TextBox1.ReadOnly = false;
                }
            }            
        }

        private void txtSPNo_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(txtSPNo.Text))
                return;

            #region -- sql parameters declare --

            System.Data.SqlClient.SqlParameter sp_id = new System.Data.SqlClient.SqlParameter();
            sp_id.ParameterName = "@id";

            List<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            sp_id.Value = txtSPNo.Text;
            cmds.Add(sp_id);
            #endregion
            if (!MyUtility.Check.Seek(string.Format(@"select orders.id from dbo.orders WITH (NOLOCK) 
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

        private void numAdditionChargeClaimAmt_Validated(object sender, EventArgs e)
        {
            CurrentData["total"] = decimal.Parse(CurrentData["amount"].ToString()) + decimal.Parse(CurrentData["addition"].ToString());
        }
    }
}
