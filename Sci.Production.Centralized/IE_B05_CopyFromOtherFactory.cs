using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class IE_B05_CopyFromOtherFactory : Sci.Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public IE_B05_CopyFromOtherFactory()
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtFromFty.Text) || MyUtility.Check.Empty(this.txtToFty.Text))
            {
                MyUtility.Msg.WarningBox("From Factory or To Factory cannot be empty!");
                return;
            }

            // 將From Factory 資料by factory更新 到To Fractory
            string sqlUpdate = $@"
Merge ProductionTPE.dbo.MachineType_Detail as t
using (select * from ProductionTPE.dbo.MachineType_Detail where FactoryID = '{this.txtFromFty.Text}') as s
on t.ID = s.ID and t.FactoryID = '{this.txtToFty.Text}'
when matched  then 
update set
	t.IsSubprocess = s.IsSubprocess,
	t.IsNonSewingLine = s.IsNonSewingLine,
	t.IsNotShownInP01 = s.IsNotShownInP01,
	t.IsNotShownInP03 = s.IsNotShownInP03
when not matched by target then
insert([ID]
    ,[FactoryID]
    ,[IsSubprocess]
    ,[IsNonSewingLine]
    ,[IsNotShownInP01]
    ,[IsNotShownInP03])
values(
	s.[ID]
    ,'{this.txtToFty.Text}'
    ,s.[IsSubprocess]
    ,s.[IsNonSewingLine]
    ,s.[IsNotShownInP01]
    ,s.[IsNotShownInP03]
	);	
";
            DualResult result = DBProxy.Current.Execute("Trade", sqlUpdate);
            if (result == false)
            {
                this.ShowErr(result);
                return;
            }
            else
            {
                MyUtility.Msg.WarningBox("Copy successful.");
            }
        }

        private void TxtFromFty_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtFromFty.Text))
            {
                return;
            }

            string sqlChk = $@"
select 1 from ProductionTPE.dbo.MachineType_Detail
where FactoryID = '{this.txtFromFty.Text}';
";
            if (!MyUtility.Check.Seek(sqlChk, connectionName: "Trade"))
            {
                MyUtility.Msg.WarningBox($"The <{this.txtFromFty.Text}> has not been setting.");
                e.Cancel = true;
                return;
            }
        }

        private void TxtToFty_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtToFty.Text))
            {
                return;
            }

            string sqlChk = $@"
select ID from [Trade].dbo.Factory where IsSCI = 1
and ID = '{this.txtToFty.Text}';
";
            if (!MyUtility.Check.Seek(sqlChk, connectionName: "Trade"))
            {
                MyUtility.Msg.WarningBox($"The <{this.txtToFty.Text}> not exists");
                e.Cancel = true;
                return;
            }
        }
    }
}
