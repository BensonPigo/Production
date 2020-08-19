using Ict.Win;
using System;
using System.Data;
using Sci.Data;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P04_FabricIssueList : Win.Subs.Base
    {
        private DataTable gridTb;
        private string cutplanid;

        /// <summary>
        /// Initializes a new instance of the <see cref="P04_FabricIssueList"/> class.
        /// </summary>
        public P04_FabricIssueList()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="P04_FabricIssueList"/> class.
        /// </summary>
        /// <param name="str">Cutplan ID</param>
        public P04_FabricIssueList(string str)
        {
            this.InitializeComponent();
            this.cutplanid = str;
            DBProxy.Current.Select(null, string.Format("Select id,issuedate,Status from Issue WITH (NOLOCK) Where Cutplanid ='{0}'", str), out this.gridTb);
            this.gridFabricIssueList.DataSource = this.gridTb;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Helper.Controls.Grid.Generator(this.gridFabricIssueList)
            .Text("id", header: "Issue ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("IssueDate", header: "Issue Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Status", header: "Status", width: Widths.AnsiChars(10), iseditingreadonly: true);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
