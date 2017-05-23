﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.UI;
using Ict.Win;
using Ict;

namespace Sci.Production.Class
{
    public partial class txtmfactory : Sci.Win.UI.TextBox
    {
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format("select ID,NameEN from Factory WITH (NOLOCK) where Junk = 0 and MDivisionID = '{0}' order by ID", Sci.Env.User.Keyword), "8,40", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
            this.ValidateText();
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                //sql參數
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                sp1.ParameterName = "@id";
                sp1.Value = this.Text;
                sp2.ParameterName = "@mdivisionid";
                sp2.Value = Sci.Env.User.Keyword;

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);
                cmds.Add(sp2);
                string sqlCmd = "select ID from Factory WITH (NOLOCK) where ID = @id and MDivisionID = @mdivisionid";
                DataTable FactoryData;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out FactoryData);
                if (!result)
                {
                    this.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Sql connection fail!!\r\n"+result.ToString());
                    return;
                }
                else
                {
                    if (FactoryData.Rows.Count <= 0)
                    {
                        this.Text = "";
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Factory : {0} > not found!!!", str));
                        return;
                    }
                }
            }
        }

        public txtmfactory()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }
    }
}
