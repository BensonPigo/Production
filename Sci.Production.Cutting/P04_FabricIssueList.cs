using Ict.Win;
using System;
using System.Data;
using Sci.Data;

namespace Sci.Production.Cutting
{
    public partial class P04_FabricIssueList : Win.Subs.Base
    {
        DataTable gridTb;
        string cutplanid;

        public P04_FabricIssueList()
        {
            this.InitializeComponent();
        }

        public P04_FabricIssueList(string str)
        {
            this.InitializeComponent();
            this.cutplanid = str;
            DBProxy.Current.Select(null, string.Format("Select id,issuedate,Status from Issue WITH (NOLOCK) Where Cutplanid ='{0}'", str), out this.gridTb);
            this.gridFabricIssueList.DataSource = this.gridTb;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Helper.Controls.Grid.Generator(this.gridFabricIssueList)
            .Text("id", header: "Issue ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("IssueDate", header: "Issue Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Status", header: "Status", width: Widths.AnsiChars(10), iseditingreadonly: true);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
