using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class R43 : Win.Tems.PrintForm
    {
        /// <summary>
        /// R43
        /// </summary>
        public R43(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }
        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dateIssue.HasValue)
            {
                MyUtility.Msg.WarningBox("Issue Date cannot be empty.");
                return false;
            }
            return base.ValidateInput();
        }
        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            string sqlQuery = string.Empty;
            string sqlWhere = string.Empty;
            string sqlHaving = string.Empty;
            List<SqlParameter> listPar = new List<SqlParameter>();

            if (!MyUtility.Check.Empty(this.dateIssue.Value1))
            {
                sqlWhere += $@" and ir.IssueDate >= @IssueDate_S";
                listPar.Add(new SqlParameter("@IssueDate_S", this.dateIssue.Value1));
            }

            if (!MyUtility.Check.Empty(this.dateIssue.Value2))
            {
                sqlWhere += $@" and ir.IssueDate <= @IssueDate_E";
                listPar.Add(new SqlParameter("@IssueDate_E", this.dateIssue.Value2));
            }

            if (!MyUtility.Check.Empty(this.txtTransfer.Text)) 
            {
                sqlWhere += $@" and ir.IssueID = @IssueID";
                listPar.Add(new SqlParameter("@IssueID",this.txtTransfer.Text));
            }

            return null;
        }
    }
}
