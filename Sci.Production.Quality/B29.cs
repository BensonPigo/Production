using Ict.Win;
using Ict;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Data;
using System.Data.SqlTypes;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class B29 : Win.Tems.QueryForm
    {
        private DataTable dataTable;
        /// <inheritdoc/>
        public B29(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;

        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string strCmdWeaveType = $@"select WeaveTypeID = ''
                                        union
                                        select distinct WeaveTypeID from FIR_Grade";
            DualResult dualResultWeaveType = DBProxy.Current.Select(null, strCmdWeaveType, out DataTable dtWreveType);
            if (!dualResultWeaveType)
            {
                MyUtility.Msg.WarningBox(dualResultWeaveType.ToString());
                return;
            }

            this.cbWeaveType.DataSource = dtWreveType;
            this.cbWeaveType.DisplayMember = "WeaveTypeID";
            this.cbWeaveType.ValueMember = "WeaveTypeID";

            string strCmdGroup = "select distinct InspectionGroup from FIR_Grade";
            DualResult dualResultGroup = DBProxy.Current.Select(null, strCmdGroup, out DataTable dtGroup);
            if (!dualResultGroup)
            {
                MyUtility.Msg.WarningBox(dualResultGroup.ToString());
                return;
            }

            this.cbGroup.DataSource = dtGroup;
            this.cbGroup.DisplayMember = "InspectionGroup";
            this.cbGroup.ValueMember = "InspectionGroup";

            string sqlCmd = @"select 
                             BrandID 
                             , WeaveTypeID
                             , InspectionGroup
                             , Percentage
                             , Grade
                             , ShowGrade
                             , Result
                             , isFormatInP01
                             , isResultNotInP01
                             , Description
                             from FIR_Grade";
            DualResult dualResult = DBProxy.Current.Select(null, sqlCmd, out dataTable);
            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            this.gridImport.AutoGenerateColumns = true;
            this.gridImport.ReadOnly = true;
            this.gridImport.DataSource = dataTable;
        }

        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            string strWhere = string.Empty;
            if (MyUtility.Check.Empty(this.txtbrand.Text) &&
                MyUtility.Check.Empty(this.cbGroup.Text) &&
                MyUtility.Check.Empty(this.cbWeaveType.Text))
            {
                this.gridImport.DataSource = this.dataTable;
                return;
            }

            if (!MyUtility.Check.Empty(this.txtbrand.Text))
            {
                strWhere += $"and BrandID = '{this.txtbrand.Text}'";
            }

            if (!MyUtility.Check.Empty(this.cbGroup.Text))
            {
                strWhere += $"and InspectionGroup = '{this.cbGroup.Text}'";
            }

            if (!MyUtility.Check.Empty(this.cbWeaveType.Text))
            {
                strWhere += $"and WeaveTypeID = '{this.cbWeaveType.Text}'";
            }

            var whereTable = this.dataTable.Select(strWhere.Substring(4, strWhere.Length - 4));
            if (whereTable.Count() == 0)
            {
                this.gridImport.DataSource = whereTable = null;
            }
            else
            {
                this.gridImport.DataSource = whereTable.CopyToDataTable();
            }
        }
    }
}
