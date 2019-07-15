using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Tools
{
    public partial class Query_Function_Permissions : Sci.Win.Tems.QueryForm
    {
        private DataTable dtQuery;
        public Query_Function_Permissions(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.grid.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("ID", header: "User ID")
                .Text("Name", header: "Name")
                .Text("Position", header: "Position")
                .Text("Email", header: "Email")
                .Text("ExtNo", header: "Ext.")
                .Text("Factory", header: "Factory")
                .Text("MenuName", header: "Module")
                .Text("BarPrompt", header: "Function")
                .Text("CanNew", header: "New")
                .Text("CanEdit", header: "Edit")
                .Text("CanDelete", header: "Delete")
                .Text("CanPrint", header: "Print")
                .Text("CanConfirm", header: "Confirm")
                .Text("CanUnConfirm", header: "UnConfirm")
                .Text("CanSend", header: "Send")
                .Text("CanRecall", header: "Recall")
                .Text("CanUnCheck", header: "UnCheck")
                .Text("CanCheck", header: "Check")
                .Text("CanClose", header: "Close")
                .Text("CanUnClose", header: "UnClose")
                .Text("CanReceive", header: "Receive")
                .Text("CanReturn", header: "Return")
                .Text("CanJunk", header: "Junk");

        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            string strModule = this.txtModule.Text;
            string strUserID = this.txtUserID.Text;
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
                listSQLFilter.Add($@" and p1.id = '{strUserID}'");
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

            #endregion
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


            if (listSQLPerm.Count > 0)
            {
                strSQLPerm = "and (" + listSQLPerm.JoinToString(Environment.NewLine) + " 1=1 )";
            }
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
,[New] =   case when MD.CanNew=0 then 'N/A'
			when MD.CanNew=1 and p2.CanNew=1 then 'Y' else '' end
,[Edit] =  case when MD.CanEdit=0 then 'N/A'
			when MD.CanEdit=1 and p2.CanEdit=1 then 'Y' else '' end
,[Delete] = case when MD.CanDelete=0 then 'N/A'
			when MD.CanDelete=1 and p2.CanDelete=1 then 'Y' else '' end
,[Print] = case when MD.CanPrint=0 then 'N/A'
			when MD.CanPrint=1 and p2.CanPrint=1 then 'Y' else '' end
,[Confirm] = case when MD.CanConfirm=0 then 'N/A'
			when MD.CanConfirm=1 and p2.CanConfirm=1 then 'Y' else '' end
,[UnConfirm] = case when MD.CanUnConfirm=0 then 'N/A'
			when MD.CanUnConfirm=1 and p2.CanUnConfirm=1 then 'Y' else '' end
,[Send] = case when MD.CanSend=0 then 'N/A'
			when MD.CanSend=1 and p2.CanSend=1 then 'Y' else '' end
,[Recall] = case when MD.CanRecall=0 then 'N/A'
			when MD.CanRecall=1 and p2.CanRecall=1 then 'Y' else '' end
,[UnCheck] = case when MD.CanUnCheck=0 then 'N/A'
			when MD.CanUnCheck=1 and p2.CanUnCheck=1 then 'Y' else '' end
,[Check] = case when MD.CanCheck=0 then 'N/A'
			when MD.CanCheck=1 and p2.CanCheck=1 then 'Y' else '' end
,[Close] = case when MD.CanClose=0 then 'N/A'
			when MD.CanClose=1 and p2.CanClose=1 then 'Y' else '' end
,[UnClose] = case when MD.CanUnClose=0 then 'N/A'
			when MD.CanUnClose=1 and p2.CanUnClose=1 then 'Y' else '' end
,[Receive] = case when MD.CanReceive=0 then 'N/A'
			when MD.CanReceive=1 and p2.CanReceive=1 then 'Y' else '' end
,[Return] = case when MD.CanReturn=0 then 'N/A'
			when MD.CanReturn=1 and p2.CanReturn=1 then 'Y' else '' end
,[Junk] = case when MD.CanJunk=0 then 'N/A'
			when MD.CanJunk=1 and p2.CanJunk=1 then 'Y' else '' end
from Pass1 p1
inner join Pass2 p2 on p1.FKPass0=p2.FKPass0
LEFT JOIN MenuDetail MD ON MD.PKey = P2.FKMenu
LEFT JOIN Menu M ON MD.UKey = M.PKey
LEFT JOIN MenuDetail MD2 ON M.MenuName = MD2.BarPrompt
LEFT JOIN Menu M2 ON MD2.UKey = M2.PKey
where p2.Used ='Y' and( p1.Resign >= GETDATE() or p1.Resign  is null)
{listSQLFilter.JoinToString($"{Environment.NewLine} ")}
{strSQLPerm}
ORDER BY p1.id,[Module], [Function]

";
            #endregion
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            this.txtFunction.Text = string.Empty;
            this.txtUserID.Text = string.Empty;
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
    }
}
