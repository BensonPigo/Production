﻿using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Thread
{
    public partial class P02_QtyBreakdownByColorway : Sci.Win.Subs.Base
    {
        private DataRow P02CurrentMaintain = null;

        public P02_QtyBreakdownByColorway(DataRow P02CurrentMaintain)
        {
            InitializeComponent();
            this.P02CurrentMaintain = P02CurrentMaintain;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region Grid Setting
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("Article", header: "Colorway", iseditingreadonly: true)
                .Text("ttl", header: "Total", iseditingreadonly: true);

            for (int i = 0; i < this.grid.Columns.Count; i++)
            {
                this.grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            #endregion
            #region SQL Command
            string strSQLCmd = string.Format(@"
select  oa.Article
        , ttl = isnull (sum(oq.Qty), 0)
from Order_Article oa WITH (NOLOCK) 
inner join Order_Qty oq WITH (NOLOCK) on oa.ID = oq.ID
									     and oa.Article = oq.Article
where oa.ID = '{0}'
group by oa.Article
", this.P02CurrentMaintain["OrderID"]);
            #endregion
            #region SQL Process
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, strSQLCmd, out dtResult);
            if (result)
            {
                if (dtResult == null || dtResult.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found.");
                    this.Close();
                }
            }
            else
            {
                MyUtility.Msg.WarningBox(result.Description);
                this.Close();
            }
            #endregion
            this.listControlBindingSource.DataSource = dtResult;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
