using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Tools
{
    public partial class SQL_Query : Sci.Win.Tems.QueryForm
    {
        public SQL_Query(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            GridSetup();
        }

        string sqlcmd;

        private void GridSetup()
        {
            this.grid.AutoGenerateColumns = true;
        }

        private void Query_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.DataSource = null;
            
            sqlcmd = editBox.Text;
            DualResult result;
            DataTable dt;
            result = DBProxy.Current.Select("", sqlcmd, null, out dt);
            if (!result)
            {
                ShowErr(sqlcmd, result);
                return;
            }

            listControlBindingSource1.DataSource = dt;
        }
    }
}
