using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Transactions;
using System.Data.SqlClient;
using Sci.Win;
using System.Reflection;

namespace Sci.Production.Warehouse
{
    public partial class P61 : Sci.Win.Tems.Input6
    {
        public P61(ToolStripMenuItem menuitem)
            :base(menuitem)
        {
            InitializeComponent();
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string ID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"
select  LID.ID
        , LID.OrderID
        , LID.Refno
        , LID.ThreadColorID
        , [Desc] = Litem.Description
        , [unit] = Linv.UnitID
        , LID.Qty
        , LID.ukey
from LocalIssue LI
join LocalIssue_Detail LID on LI.ID = LID.ID
join LocalInventory Linv on LID.OrderID = Linv.OrderID and LID.Refno = Linv.Refno and LID.ThreadColorID = Linv.ThreadColorID
left join LocalItem Litem on LID.Refno = Litem.Refno
where LI.ID = '{0}' and LI.MDivisionID = '{1}'", ID, Sci.Env.User.Keyword);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            #region Set Grid
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderID", header: "SP#", iseditingreadonly: false)
                .Text("Refno", header: "Refno", iseditingreadonly: false)
                .Text("ThreadColorID", header: "ThreadColor", iseditingreadonly: false)
                .EditText("desc", header: "Description", iseditingreadonly: false)
                .Text("unit", header: "Unit", iseditingreadonly: false)
                .Numeric("Qty", header: "Issue Qty", iseditingreadonly: false, minimum: -100);
            #endregion 
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].EqualString("confirmed"))
            {
                MyUtility.Msg.InfoBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }
            return base.ClickEditBefore();
        }

        protected override bool ClickSaveBefore()
        {
            this.detailgrid.EndEdit();
            #region Check 必輸條件
            /*--- IssueQty != 0 ---*/
            List<string> listErr = new List<string>();
            DataTable dataTable = this.detailgrid.GetTable();
            foreach (DataRow dr in dataTable.Rows)
            {
                if (dr.RowState != DataRowState.Deleted && Convert.ToDecimal(dr["Qty"]) == 0)
                {
                    listErr.Add(string.Format(@"< SP# > : {0}, < Refno > : {1}, < ThreadColor > : {2}", dr["OrderID"], dr["Refno"], dr["ThreadColorID"]));
                }
            }

            if (listErr.Count > 0)
            {
                MyUtility.Msg.InfoBox(listErr.JoinToString("\n\r"), "Issue Qty can not be Zero!!");
                return false;
            }
            #endregion
            #region 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Factory + "IO", "LocalIssue", (DateTime)CurrentMaintain["Issuedate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }
                CurrentMaintain["id"] = tmpId;
            }
            #endregion 
            return base.ClickSaveBefore();
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            DualResult result;
            List<SqlParameter> listPar = new List<SqlParameter>();
            listPar.Add(new SqlParameter("@ID", CurrentMaintain["ID"]));
            listPar.Add(new SqlParameter("@UserID", Env.User.UserID));
            #region Check 庫存
            DataTable dataTable;
            string checkStockQty = @"
select *
from (
	select	Linv.OrderID
			, Linv.Refno
			, Linv.ThreadColorID 
			, StockQty = Linv.InQty - Linv.OutQty + Linv.AdjustQty - LID.Qty 
	from LocalInventory Linv
	inner join LocalIssue_Detail LID on Linv.OrderID = LID.OrderID 
		and Linv.Refno = LID.Refno and Linv.ThreadColorID = LID.ThreadColorID
	where LID.ID = @ID
) s
where s.StockQty < 0";
            if (!(result = DBProxy.Current.Select(null, checkStockQty, listPar, out dataTable)))
            {
                ShowErr(checkStockQty, result);
                return;
            } else
            {
                if(dataTable != null && dataTable.Rows.Count > 0){
                    List<string> listErr = new List<string>();
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        listErr.Add(string.Format("< SP# > : {0}, < Refno > : {1}, < ThreadColor > : {2}"
                                                  , dr["OrderID"], dr["Refno"], dr["ThreadColorID"]));
                    }
                    MyUtility.Msg.InfoBox(listErr.JoinToString("/n/r"), "Local Stock Quantity can not less then zero!!");
                    return;
                }
            }
            #endregion
            #region SQL Command : 更新表頭
            string strUpdateLocalIssue = @"
Update LocalIssue
Set Status = 'Confirmed'
	, EditName = @UserID
	, EditDate = GETDATE()
from LocalIssue 
where ID = @ID";
            #endregion
            #region SQL Command : Update 庫存
            string strUpdateLocalInv = @"
UPDATE Linv
Set Linv.OutQty = Linv.OutQty + LID.Qty
from LocalInventory Linv
inner join LocalIssue_Detail LID on Linv.OrderID = LID.OrderID 
	and Linv.Refno = LID.Refno and Linv.ThreadColorID = LID.ThreadColorID
where LID.ID = @ID";
            #endregion
            #region SQL Updating...
            TransactionScope transactionScope = new TransactionScope();
            using (transactionScope)
            {
                try {
                    if (!(result = DBProxy.Current.Execute(null, strUpdateLocalIssue, listPar)))
                    {
                        transactionScope.Dispose();
                        ShowErr(strUpdateLocalIssue, result);
                        return;
                    }
                    if (!(result = DBProxy.Current.Execute(null, strUpdateLocalInv, listPar)))
                    {
                        transactionScope.Dispose();
                        ShowErr(strUpdateLocalInv, result);
                        return;
                    }
                    transactionScope.Complete();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                } catch (Exception e) {
                    transactionScope.Dispose();
                    ShowErr("Commit transcation error.", e);
                    return;
                }
            }
            #endregion 
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DualResult result;
            List<SqlParameter> listPar = new List<SqlParameter>();
            listPar.Add(new SqlParameter("@ID", CurrentMaintain["ID"]));
            listPar.Add(new SqlParameter("@UserID", Env.User.UserID));
            #region SQL Command : 更新表頭
            string strUpdateLocalIssue = @"
Update LocalIssue
Set Status = 'New'
	, EditName = @UserID
	, EditDate = GETDATE()
from LocalIssue 
where ID = @ID";
            #endregion
            #region SQL Command : Update 庫存
            string strUpdateLocalInv = @"
UPDATE Linv
Set Linv.OutQty = Linv.OutQty - LID.Qty
from LocalInventory Linv
inner join LocalIssue_Detail LID on Linv.OrderID = LID.OrderID 
	and Linv.Refno = LID.Refno and Linv.ThreadColorID = LID.ThreadColorID
where LID.ID = @ID";
            #endregion
            #region SQL Updating...
            TransactionScope transactionScope = new TransactionScope();
            using (transactionScope)
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, strUpdateLocalIssue, listPar)))
                    {
                        transactionScope.Dispose();
                        ShowErr(strUpdateLocalIssue, result);
                        return;
                    }
                    if (!(result = DBProxy.Current.Execute(null, strUpdateLocalInv, listPar)))
                    {
                        transactionScope.Dispose();
                        ShowErr(strUpdateLocalInv, result);
                        return;
                    }
                    transactionScope.Complete();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception e)
                {
                    transactionScope.Dispose();
                    ShowErr("Commit transcation error.", e);
                    return;
                }
            }
            #endregion 
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = detailgridbs.Find("OrderID", txtSP.Text.TrimEnd());
            if (index == -1)
            { 
                MyUtility.Msg.InfoBox("Data was not found!!"); 
            }
            else
            { 
                detailgridbs.Position = index; 
            }
        }

        private void btnClearEmpty_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P61_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        protected override bool ClickDeleteBefore()
        {
            #region Check Status
            if (CurrentMaintain["Status"].EqualString("Confirmed"))
            {
                MyUtility.Msg.InfoBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }
            #endregion 
            return base.ClickDeleteBefore();
        }

        protected override bool ClickPrint()
        {
            #region Check Status
            if (!CurrentMaintain["Status"].EqualString("Confirmed"))
            {
                MyUtility.Msg.InfoBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }
            #endregion 
            ReportDefinition report = new ReportDefinition();
            #region Set RDLC_Title Data
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", CurrentMaintain["ID"].ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("FactoryID", CurrentMaintain["FactoryID"].ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("CDate", string.Format("{0:yyyy-MM-dd}", CurrentMaintain["IssueDate"])));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", CurrentMaintain["Remark"].ToString()));
            #endregion 
            #region Set RDLC_Detail Data
            List<P61_PrintData> data = detailgrid.GetTable().AsEnumerable()
                .Select(row => new P61_PrintData(){
                    POID = row["OrderID"].ToString().Trim(),
                    Refno = row["Refno"].ToString().Trim(),
                    DESCRIPTION = row["Desc"].ToString().Trim(),
                    ThreadColorID = row["ThreadColorID"].ToString().Trim(),
                    QTY = row["Qty"].ToString().Trim()
                }).ToList();

            report.ReportDataSource = data;
            #endregion
            #region Open RDLC
            DualResult result;
            Type nameSpace = typeof(P61_PrintData);
            Assembly assembly = nameSpace.Assembly;
            string name = "P61_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(assembly, nameSpace, name, out reportresource)))
            {
                return false;
            }
            report.ReportResource = reportresource;

            var form = new Sci.Win.Subs.ReportView(report);
            form.MdiParent = MdiParent;
            form.Show();
            #endregion 
            return true;
        }
    }
}
