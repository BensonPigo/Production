using Ict;
using Ict.Win;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Win;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;

namespace Sci.Production.Cutting
{
    public partial class P04_FabricIssueList : Sci.Win.Subs.Base
    {
        DataTable gridTb;
        string cutplanid;
        public P04_FabricIssueList()
        {
            InitializeComponent();
        }

        public P04_FabricIssueList(string str)
        {
            InitializeComponent();
            cutplanid = str;
            DBProxy.Current.Select(null, string.Format("Select id,issuedate from Issue Where Cutplanid ='{0}'", str), out gridTb);
            grid1.DataSource = gridTb;
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Helper.Controls.Grid.Generator(this.grid1)
            .Text("id", header: "Issue ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("IssueDate", header: "Issue Date", width: Widths.AnsiChars(10), iseditingreadonly: true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
