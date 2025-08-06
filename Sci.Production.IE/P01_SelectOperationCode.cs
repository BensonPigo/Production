using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
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
                    var selectRows = this.gridDetail.SelectedRows;
                    if (selectRows.Count > 0)
                    {
                        this._P01SelectOperationCode = ((DataRowView)selectRows[0].DataBoundItem).Row;
                        this.DialogResult = DialogResult.OK;
                    }
                }
            };

            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("ID", header: "ID", width: Widths.AnsiChars(20), iseditingreadonly: true, settings: s1)
                 .Text("DescEN", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
                 .Numeric("SMV", header: "S.M.V", decimal_places: 4, iseditingreadonly: true)
                 .Numeric("SMVsec", header: "S.M.V(sec)", decimal_places: 4, iseditingreadonly: true)
                 .Text("MachineTypeID", header: "ST/MC Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("MasterPlusGroup", header: "Machine Group", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Numeric("SeamLength", header: "Seam Length", decimal_places: 2, iseditingreadonly: true);

            string sqlCmd = $@"
            select  o.ID,
                    o.DescEN,
                    o.SMV,
                    SMVsec = o.SMV * 60,
                    o.MachineTypeID,
                    o.SeamLength,
                    o.MtlFactorID,
                    o.Annotation,
                    o.MasterPlusGroup,
                    [MachineType_IsSubprocess] = isnull(md.IsSubprocess,0),
                    o.Junk,
                    md.IsSubprocess,
                    [IsNonSewingLine] = isnull(md.IsNonSewingLine, 0),
                    o.MoldID,
                    [Motion] = Motion.val
            from Operation o WITH (NOLOCK)
            left join MachineType_Detail md WITH (NOLOCK) on md.ID = o.MachineTypeID and md.FactoryID = '{Sci.Env.User.Factory}'
            OUTER APPLY
            (
	            select val = stuff((select distinct concat(',',Name)
	            from OperationRef a
	            inner JOIN IESELECTCODE b WITH(NOLOCK) on a.CodeID = b.ID and a.CodeType = b.Type
	            where a.CodeType = '00007' and a.id = o.ID  for xml path('') ),1,1,'')
            )Motion
            where CalibratedCode = 1
";
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

            string descriptionText = this.txtDescription.Text.Trim();

            // 定義一個正則表達式，匹配所有的特殊符號，除了分號
            string pattern = @"[^a-zA-Z0-9\s;]";

            if (Regex.IsMatch(descriptionText, pattern))
            {
                string sss = string.Empty;
                this.gridData.DefaultView.RowFilter = $@"DescEN ='{descriptionText}'";
                return;
            }

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
                        if (i > 0)
                        {
                            filterCondition.Append(" OR ");
                        }

                        filterCondition.Append(" (");
                        filterCondition.Append($"DescEN LIKE '%{descList[i]}%'");
                        filterCondition.Append(")");
                    }
                }

                filterCondition.Append(" AND ");
            }

            // ISP20220757 只顯示Operation.Junk = 0
            filterCondition.Append(string.Format(" Junk = 0 and", this.txtDescription.Text.Trim()));

            if (filterCondition.Length > 0)
            {
                string filter = filterCondition.ToString().Substring(0, filterCondition.Length - 3);
                this.gridData.DefaultView.RowFilter = "1=1 AND " + filter;
                this.numCount.Value = this.gridData.DefaultView.Count;
            }

            // this.gridDetail.AutoResizeColumns();
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
