using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Tools
{
    public partial class Query_Function_Permissions : Win.Tems.QueryForm
    {
        private DataTable dtQuery;

        public Query_Function_Permissions(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.grid.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("UserID", header: "User ID", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("Name", header: "Name", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .Text("Position", header: "Position", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("Email", header: "Email", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("ExtNo", header: "Ext.", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("Factory", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("Module", header: "Module", iseditingreadonly: true, width: Widths.AnsiChars(12))
                .Text("Function", header: "Function", iseditingreadonly: true, width: Widths.AnsiChars(25))
                .Text("New", header: "New", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("Edit", header: "Edit", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("Delete", header: "Delete", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("Print", header: "Print", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("Confirm", header: "Confirm", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("UnConfirm", header: "UnConfirm", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("Send", header: "Send", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("Recall", header: "Recall", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("UnCheck", header: "UnCheck", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("Check", header: "Check", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("Close", header: "Close", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("UnClose", header: "UnClose", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("Receive", header: "Receive", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("Return", header: "Return", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("Junk", header: "Junk", iseditingreadonly: true, width: Widths.AnsiChars(5));
            this.grid.AutoGenerateColumns = true;
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            string strModule = this.txtModule.Text;
            string strUserID = this.txtUserID.TextBox1.Text;
            string strFunction = this.txtFunction.Text;
            string strPosition = this.txtPosition.Text;

            #region SQL Filter
            List<string> listSQLFilter = new List<string>();
            if (!MyUtility.Check.Empty(strModule))
            {
                listSQLFilter.Add($@" and ISNULL(M2.MenuName,M.MenuName) = '{strModule}' ");
            }

            if (!MyUtility.Check.Empty(strUserID))
            {
                string strUserid = string.Empty;
                string[] getUserId = strUserID.Split(',').Distinct().ToArray();
                foreach (var userID in getUserId)
                {
                    strUserid += ",'" + userID + "'";
                }

                listSQLFilter.Add($@" and p1.id in ({strUserid.Substring(1)})");
            }

            if (!MyUtility.Check.Empty(strFunction))
            {
                listSQLFilter.Add($@" and IIF(M.MenuName IS NOT NULL,MD.BarPrompt,MD2.BarPrompt) ='{strFunction}'");
            }

            if (!MyUtility.Check.Empty(strPosition))
            {
                listSQLFilter.Add($@" and p1.Position = '{strPosition}'");
            }
            #endregion

            #region Sql Filter Checked
            List<string> listSQLPerm = new List<string>();
            string strSQLPerm = string.Empty;

            if (this.chkNew.Checked)
            {
                listSQLPerm.Add($@" (MD.CanNew=1 and p2.CanNew=1) or");
            }

            if (this.chkEdit.Checked)
            {
                listSQLPerm.Add($@" (MD.CanEdit=1 and p2.CanEdit=1) or");
            }

            if (this.chkDelete.Checked)
            {
                listSQLPerm.Add($@" (MD.CanDelete=1 and p2.CanDelete=1) or");
            }

            if (this.chkPrint.Checked)
            {
                listSQLPerm.Add($@" (MD.CanPrint=1 and p2.CanPrint=1) or");
            }

            if (this.chkConfirmed.Checked)
            {
                listSQLPerm.Add($@" (MD.CanConfirm=1 and p2.CanConfirm=1) or");
            }

            if (this.chkUnConfirmed.Checked)
            {
                listSQLPerm.Add($@" (MD.CanUnConfirm=1 and p2.CanUnConfirm=1) or");
            }

            if (this.chkSend.Checked)
            {
                listSQLPerm.Add($@" (MD.CanSend=1 and p2.CanSend=1) or");
            }

            if (this.chkRecall.Checked)
            {
                listSQLPerm.Add($@" (MD.CanRecall=1 and p2.CanRecall=1) or");
            }

            if (this.chkUnCheck.Checked)
            {
                listSQLPerm.Add($@" (MD.CanUnCheck=1 and p2.CanUnCheck=1) or");
            }

            if (this.chkCheck.Checked)
            {
                listSQLPerm.Add($@" (MD.CanCheck=1 and p2.CanCheck=1) or");
            }

            if (this.chkClose.Checked)
            {
                listSQLPerm.Add($@" (MD.CanClose=1 and p2.CanClose=1) or");
            }

            if (this.chkUnClose.Checked)
            {
                listSQLPerm.Add($@" (MD.CanUnClose=1 and p2.CanUnClose=1) or");
            }

            if (this.chkReceive.Checked)
            {
                listSQLPerm.Add($@" (MD.CanReceive=1 and p2.CanReceive=1) or");
            }

            if (this.chkReturn.Checked)
            {
                listSQLPerm.Add($@" (MD.CanReturn=1 and p2.CanReturn=1) or");
            }

            if (this.chkJunk.Checked)
            {
                listSQLPerm.Add($@" (MD.CanJunk=1 and p2.CanJunk=1) or");
            }

            if (listSQLPerm.Count > 0)
            {
                strSQLPerm = "and (" + listSQLPerm.JoinToString(Environment.NewLine) + " 1=0 )";
            }
            #endregion

            this.ShowWaitMessage("Data Loading....");

            #region Sql Command

            string strCmd = $@"
select [UserID] = p1.ID
,p1.Name
,p1.Position
,p1.EMail
,p1.ExtNo
,p1.Factory
,[Module] = ISNULL(M2.MenuName,M.MenuName)
,[Function] = IIF(M.MenuName IS NOT NULL,MD.BarPrompt,MD2.BarPrompt) 
,[New] =   case when MD.CanNew=0 then ''
			when MD.CanNew=1 and p2.CanNew=1 then 'Y' else 'N' end
,[Edit] =  case when MD.CanEdit=0 then ''
			when MD.CanEdit=1 and p2.CanEdit=1 then 'Y' else 'N' end
,[Delete] = case when MD.CanDelete=0 then ''
			when MD.CanDelete=1 and p2.CanDelete=1 then 'Y' else 'N' end
,[Print] = case when MD.CanPrint=0 then ''
			when MD.CanPrint=1 and p2.CanPrint=1 then 'Y' else 'N' end
,[Confirm] = case when MD.CanConfirm=0 then ''
			when MD.CanConfirm=1 and p2.CanConfirm=1 then 'Y' else 'N' end
,[UnConfirm] = case when MD.CanUnConfirm=0 then ''
			when MD.CanUnConfirm=1 and p2.CanUnConfirm=1 then 'Y' else 'N' end
,[Send] = case when MD.CanSend=0 then ''
			when MD.CanSend=1 and p2.CanSend=1 then 'Y' else 'N' end
,[Recall] = case when MD.CanRecall=0 then ''
			when MD.CanRecall=1 and p2.CanRecall=1 then 'Y' else 'N' end
,[UnCheck] = case when MD.CanUnCheck=0 then ''
			when MD.CanUnCheck=1 and p2.CanUnCheck=1 then 'Y' else 'N' end
,[Check] = case when MD.CanCheck=0 then ''
			when MD.CanCheck=1 and p2.CanCheck=1 then 'Y' else 'N' end
,[Close] = case when MD.CanClose=0 then ''
			when MD.CanClose=1 and p2.CanClose=1 then 'Y' else 'N' end
,[UnClose] = case when MD.CanUnClose=0 then ''
			when MD.CanUnClose=1 and p2.CanUnClose=1 then 'Y' else 'N' end
,[Receive] = case when MD.CanReceive=0 then ''
			when MD.CanReceive=1 and p2.CanReceive=1 then 'Y' else 'N' end
,[Return] = case when MD.CanReturn=0 then ''
			when MD.CanReturn=1 and p2.CanReturn=1 then 'Y' else 'N' end
,[Junk] = case when MD.CanJunk=0 then ''
			when MD.CanJunk=1 and p2.CanJunk=1 then 'Y' else 'N' end
from Pass1 p1
inner join Pass2 p2 on p1.FKPass0=p2.FKPass0
LEFT JOIN MenuDetail MD ON MD.PKey = P2.FKMenu
LEFT JOIN Menu M ON MD.UKey = M.PKey
LEFT JOIN MenuDetail MD2 ON M.MenuName = MD2.BarPrompt
LEFT JOIN Menu M2 ON MD2.UKey = M2.PKey
where p2.Used ='Y' and( p1.Resign >= GETDATE() or p1.Resign  is null)
and m.MenuNo not in ('1400','1500')
{listSQLFilter.JoinToString($"{Environment.NewLine} ")}
{strSQLPerm}
ORDER BY p1.id,[Module], [Function]

";
            #endregion

            DualResult result = DBProxy.Current.Select(string.Empty, strCmd, out this.dtQuery);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
            }
            else if (this.dtQuery.Rows.Count < 1)
            {
                this.listControlBindingSource1.DataSource = null;
                MyUtility.Msg.InfoBox("Data not found !");
            }
            else
            {
                this.listControlBindingSource1.DataSource = this.dtQuery;
            }

            this.HideWaitMessage();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            this.txtModule.Text = string.Empty;
            this.txtUserID.TextBox1.Text = string.Empty;
            this.txtUserID.DisplayBox1.Text = string.Empty;
            this.txtFunction.Text = string.Empty;
            this.txtPosition.Text = string.Empty;
            this.chkNew.Checked = false;
            this.chkEdit.Checked = false;
            this.chkDelete.Checked = false;
            this.chkPrint.Checked = false;
            this.chkConfirmed.Checked = false;
            this.chkUnConfirmed.Checked = false;
            this.chkSend.Checked = false;
            this.chkRecall.Checked = false;
            this.chkCheck.Checked = false;
            this.chkUnCheck.Checked = false;
            this.chkClose.Checked = false;
            this.chkUnClose.Checked = false;
            this.chkReceive.Checked = false;
            this.chkReturn.Checked = false;
            this.chkJunk.Checked = false;
        }

        private void TxtModule_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlcmd = string.Format(@"
select MenuName from Menu
where IsSubMenu = 0 and MenuNo not in ('1400','1500')
order by PKey
");
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd,  "15", this.txtModule.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtModule.Text = item.GetSelectedString();
        }

        private void TxtModule_Validating(object sender, CancelEventArgs e)
        {
            string strModul = this.txtModule.Text;
            if (!string.IsNullOrWhiteSpace(strModul) && strModul != this.txtModule.OldValue)
            {
                if (MyUtility.Check.Seek($@"
select MenuName from Menu WITH (NOLOCK)
where IsSubMenu = 0 and MenuNo not in ('1400','1500') 
and MenuName = '{strModul}'") == false)
                {
                    this.txtModule.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Module : {0} > not found!!!", strModul));
                    return;
                }
            }
        }

        private void TxtFunction_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlcmd = string.Empty;
            sqlcmd = string.Format(@"
select BarPrompt as [Function], R_MenuName as Module
from (
	Select ISNULL(p.RootNo,m.MenuNo) as R_MenuNo
		, ISNULL(p.RootName,m.MenuName) as R_MenuName
		, md.BarPrompt
	from dbo.Menu as m
	outer apply(
		select p.MenuName as RootName 
			,p.MenuNo as RootNo
			,pd.BarNo
		from dbo.MenuDetail as pd 
		left join dbo.Menu as p on p.PKey = pd.UKey
		where pd.BarPrompt = m.MenuName
	) as p
	left join MenuDetail md on m.PKey=md.UKey
	where md.ObjectCode=0
) as s
where s.R_MenuNo not in ('1400','1500')
order by s.R_MenuNo,s.BarPrompt
");
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "40,15", this.txtFunction.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtFunction.Text = item.GetSelectedString();
        }

        private void TxtFunction_Validating(object sender, CancelEventArgs e)
        {
            string strFunction = this.txtFunction.Text;
            if (!string.IsNullOrWhiteSpace(strFunction) && strFunction != this.txtModule.OldValue)
            {
                if (MyUtility.Check.Seek($@"
select BarPrompt,R_MenuName
from (
	Select ISNULL(p.RootNo,m.MenuNo) as R_MenuNo
		, ISNULL(p.RootName,m.MenuName) as R_MenuName
		, md.BarPrompt
	from dbo.Menu as m
	outer apply(
		select p.MenuName as RootName 
			,p.MenuNo as RootNo
			,pd.BarNo
		from dbo.MenuDetail as pd 
		left join dbo.Menu as p on p.PKey = pd.UKey
		where pd.BarPrompt = m.MenuName
	) as p
	left join MenuDetail md on m.PKey=md.UKey
	where md.ObjectCode=0
) as s
where s.R_MenuNo not in ('1400','1500')
and BarPrompt = '{strFunction}'
order by s.R_MenuNo,s.BarPrompt
") == false)
                {
                    this.txtFunction.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Function : {0} > not found!!!", strFunction));
                    return;
                }
            }
        }

        private void TxtPosition_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlcmd = string.Format(@"
select ID,Description from pass0
");
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "20,25", this.txtPosition.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtPosition.Text = item.GetSelectedString();
        }

        private void TxtPosition_Validating(object sender, CancelEventArgs e)
        {
            string strPosition = this.txtPosition.Text;
            if (!string.IsNullOrWhiteSpace(strPosition) && strPosition != this.txtModule.OldValue)
            {
                if (MyUtility.Check.Seek($@"
select 1
 from pass0
where id= '{strPosition}'") == false)
                {
                    this.txtPosition.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Position : {0} > not found!!!", strPosition));
                    return;
                }
            }
        }
    }
}
