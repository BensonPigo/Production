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
            if (!MyUtility.Check.Seek(string.Format(@"select id from dbo.orders WITH (NOLOCK) 
                    where factoryid='{0}' and id = @id", Sci.Env.User.Factory), cmds))
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
