﻿using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P17
    /// </summary>
    public partial class P17 : Win.Tems.QueryForm
    {
        /// <summary>
        /// P17
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P17(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        private void Btn_import_Click(object sender, EventArgs e)
        {
            if (!this.dateRangeImport.HasValue1 || !this.dateRangeImport.HasValue2)
            {
                MyUtility.Msg.WarningBox("<Update Date Range> can not empty!");
                return;
            }

            List<SqlParameter> listPar = new List<SqlParameter>()
            {
                new SqlParameter("@startDate", this.dateRangeImport.DateBox1.Value),
                new SqlParameter("@endDate", this.dateRangeImport.DateBox2.Value),
            };
            this.ShowWaitMessage("Update processing....");
            DBProxy.Current.DefaultTimeout = 1200;
            DualResult result = DBProxy.Current.Execute(null, "exec exp_SUNRISE @startDate,@endDate", listPar);
            DBProxy.Current.DefaultTimeout = 300;
            this.HideWaitMessage();
            if (!result)
            {
                this.ShowErr(result);
            }
        }
    }
}
