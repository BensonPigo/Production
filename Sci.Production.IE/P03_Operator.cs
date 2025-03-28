using Ict;
using Ict.Win;
using Microsoft.Office.Interop.Excel;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;
using DataTable = System.Data.DataTable;

namespace Sci.Production.IE
{
    /// <inheritdoc/>
    public partial class P03_Operator : Win.Subs.Base
    {
        private DataRow _SelectOperator;
        private DataTable dtDeful;
        private DataTable dt;
        private string strSewingLineID;
        private string strTeam;

        /// <inheritdoc/>
        public P03_Operator(DataTable dataTable, string sewingLineID, string team)
        {
            this.InitializeComponent();
            this.dtDeful = dataTable.Copy();
            this.strSewingLineID = sewingLineID;
            this.strTeam = team;
            bool isEmptySewingLine = MyUtility.Check.Empty(sewingLineID);
            var strSewingWhere = isEmptySewingLine ? string.Empty : $"(Section = '{this.strSewingLineID + this.strTeam}')";
            this.dt = dataTable.Select(strSewingWhere).TryCopyToDataTable(dataTable);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridDetail.IsEditingReadOnly = false;
            this.gridDetail.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
            .Text("ID", header: "ID", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("FirstName", header: "FirstName", width: Widths.AnsiChars(35), iseditingreadonly: true)
            .Text("LastName", header: "Last Name", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Section", header: "Section", width: Widths.AnsiChars(15), iseditingreadonly: true)
            ;
            this.listControlBindingSource1.DataSource = this.dt;
        }

        /// <inheritdoc/>
        public DataRow SelectOperator
        {
            get => this._SelectOperator;
            set => this._SelectOperator = value;
        }

        private void ChkDisplay_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkDisplay.Checked)
            {
                this.listControlBindingSource1.DataSource = this.dtDeful;
            }
            else
            {
                bool isEmptySewingLine = MyUtility.Check.Empty(this.strSewingLineID);
                var strSewingWhere = isEmptySewingLine ? string.Empty : $"(Section = '{this.strSewingLineID + this.strTeam}')";
                this.dt = this.dtDeful.Select(strSewingWhere).TryCopyToDataTable(this.dtDeful);
                this.listControlBindingSource1.DataSource = this.dt;
            }
        }

        // Find
        private void BtnFind_Click(object sender, EventArgs e)
        {
            StringBuilder filterCondition = new StringBuilder();
            List<SqlParameter> paras = new List<SqlParameter>();

            List<string> descList = new List<string>();
            if (!string.IsNullOrEmpty(this.txtDescription.Text))
            {
                string tmp = this.txtDescription.Text.Replace("'", "''");
                descList = tmp.Split(';').Where(o => !string.IsNullOrEmpty(o)).ToList();
            }

            if (descList.Any())
            {
                for (int i = 0; i < descList.Count; i++)
                {
                    if(i > 0)
                    {
                        filterCondition.Append(" OR ");
                    }
                    filterCondition.Append(" (");
                    foreach (var columnName in new[] { "ID", "FIRSTNAME", "LASTNAME", "SECTION" })
                    {
                        filterCondition.Append($"{columnName} LIKE '%{descList[i]}%'");
                        if (columnName != "SECTION")
                        {
                            filterCondition.Append(" OR ");
                        }
                    }

                    filterCondition.Append(")");
                }
            }

            if (filterCondition.Length > 0)
            {
                string filter = filterCondition.ToString();
                (this.listControlBindingSource1.DataSource as DataTable).DefaultView.RowFilter = "1=1 AND " + filter;
            }
            else
            {
                (this.listControlBindingSource1.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
            }

            // this.gridDetail.AutoResizeColumns();
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectRows = this.gridDetail.SelectedRows;

            foreach (DataGridViewRow datarow in selectRows)
            {
                this.SelectOperator = ((DataRowView)datarow.DataBoundItem).Row;
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}
