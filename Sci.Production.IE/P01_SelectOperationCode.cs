using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.IE
{
    /// <summary>
    /// P01_SelectOperationCode
    /// </summary>
    public partial class P01_SelectOperationCode : Win.Subs.Base
    {
        private DataTable gridData;

        private DataRow _P01SelectOperationCode;

        /// <summary>
        /// p01SelectOperationCode
        /// </summary>
        public DataRow P01SelectOperationCode
        {
            get
            {
                return this._P01SelectOperationCode;
            }

            set
            {
                this._P01SelectOperationCode = value;
            }
        }

        /// <summary>
        /// P01_SelectOperationCode
        /// </summary>
        public P01_SelectOperationCode()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            DataGridViewGeneratorTextColumnSettings s1 = new DataGridViewGeneratorTextColumnSettings();
            base.OnFormLoaded();
            this.gridDetail.IsEditingReadOnly = true;
            this.gridDetail.DataSource = this.listControlBindingSource1;

            s1.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    DataGridViewSelectedRowCollection selectRows = this.gridDetail.SelectedRows;
                    foreach (DataGridViewRow datarow in selectRows)
                    {
                        this._P01SelectOperationCode = ((DataRowView)datarow.DataBoundItem).Row;
                        this.DialogResult = DialogResult.Cancel;
                    }
                }
            };

            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("ID", header: "ID", width: Widths.AnsiChars(20), iseditingreadonly: true, settings: s1)
                 .Text("DescEN", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Numeric("SMV", header: "S.M.V", decimal_places: 4, iseditingreadonly: true)
                 .Text("MachineTypeID", header: "ST/MC Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("MasterPlusGroup", header: "Machine Group", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Numeric("SeamLength", header: "Seam Length", decimal_places: 2, iseditingreadonly: true);

            string sqlCmd = "select ID,DescEN,SMV,MachineTypeID,SeamLength,MoldID,MtlFactorID,Annotation,MasterPlusGroup from Operation WITH (NOLOCK) where CalibratedCode = 1";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Operation fail\r\n" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = this.gridData;
            this.numCount.Value = this.gridData.Rows.Count;
        }

        // Find
        private void BtnFind_Click(object sender, EventArgs e)
        {
            StringBuilder filterCondition = new StringBuilder();
            if (!MyUtility.Check.Empty(this.txtID.Text))
            {
                filterCondition.Append(string.Format(" ID like '%{0}%' and", this.txtID.Text.Trim()));
            }

            if (this.txtID.Text == string.Empty)
            {
                filterCondition.Append(string.Format("   "));
            }

            if (!MyUtility.Check.Empty(this.numSMV.Value))
            {
                filterCondition.Append(string.Format(" SMV >= {0} and", this.numSMV.Value.ToString().Trim()));
            }

            if (!MyUtility.Check.Empty(this.txtMachineCode.Text))
            {
                filterCondition.Append(string.Format(" MachineTypeID like '%{0}%' and", this.txtMachineCode.Text.Trim()));
            }

            if (!MyUtility.Check.Empty(this.numSeamLength.Value))
            {
                filterCondition.Append(string.Format(" SeamLength >= {0} and", this.numSeamLength.Value.ToString().Trim()));
            }

            if (!MyUtility.Check.Empty(this.txtDescription.Text))
            {
                filterCondition.Append(string.Format(" DescEN like'%{0}%' and", this.txtDescription.Text.Trim()));
            }

            if (filterCondition.Length > 0)
            {
                string filter = filterCondition.ToString().Substring(0, filterCondition.Length - 3);
                this.gridData.DefaultView.RowFilter = filter;
                this.numCount.Value = this.gridData.DefaultView.Count;
            }

            this.gridDetail.AutoResizeColumns();
        }

        // Select
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (this.gridDetail.SelectedRows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Must Select one!!!");
                return;
            }

            DataGridViewSelectedRowCollection selectRows = this.gridDetail.SelectedRows;
            foreach (DataGridViewRow datarow in selectRows)
            {
                this._P01SelectOperationCode = ((DataRowView)datarow.DataBoundItem).Row;
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
