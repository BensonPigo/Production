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

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(textBox1.Text))
                return;

            #region -- sql parameters declare --

            System.Data.SqlClient.SqlParameter sp_id = new System.Data.SqlClient.SqlParameter();
            sp_id.ParameterName = "@id";

            List<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            sp_id.Value = textBox1.Text;
            cmds.Add(sp_id);
            #endregion
            if (!MyUtility.Check.Seek(string.Format(@"select id from dbo.orders
                    where factoryid='{0}' and id = @id", Sci.Env.User.Factory), cmds))
            {
                MyUtility.Msg.WarningBox("SP# is not found, Please check value is right and belong to login factory!!");
                e.Cancel = true;
                return;
            }
        }

        private void numericBox2_Validated(object sender, EventArgs e)
        {
            CurrentData["total"] = decimal.Parse(CurrentData["amount"].ToString()) + decimal.Parse(CurrentData["addition"].ToString());
        }
    }
}
