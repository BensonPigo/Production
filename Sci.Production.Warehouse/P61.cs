using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Transactions;
using System.Data.SqlClient;
using Sci.Win;
using System.Reflection;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P61 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P61(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Env.User.Keyword);
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
            this.InsertDetailGridOnDoubleClick = false;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string iD = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select  LID.ID
        , LID.OrderID
        , LID.Refno
        , LID.ThreadColorID
        , [Desc] = Litem.Description
        , [unit] = Linv.UnitID
        , LID.Qty
        , [Location] = Linv.ALocation
        , LID.ukey
from LocalIssue LI
inner join LocalIssue_Detail LID on LI.ID = LID.ID
left join LocalInventory Linv on LID.OrderID = Linv.OrderID 
                                 and LID.Refno = Linv.Refno 
                                 and LID.ThreadColorID = Linv.ThreadColorID
left join LocalItem Litem on LID.Refno = Litem.Refno
where   LI.ID = '{0}' 
        and LI.MDivisionID = '{1}'", iD, Env.User.Keyword);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region RefNo Setting
            DataGridViewGeneratorTextColumnSettings setRefno = new DataGridViewGeneratorTextColumnSettings();

            setRefno.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable selectDt;
                    string strSelectSqlCmd = string.Format(
                        @"
select  distinct refno
from LocalInventory
where OrderID = '{0}'", this.CurrentDetailData["OrderID"]);
                    DBProxy.Current.Select(null, strSelectSqlCmd, out selectDt);

                    Win.Tools.SelectItem selectItem = new Win.Tools.SelectItem(selectDt, "refno", "20", this.CurrentDetailData["Refno"].ToString());
                    DialogResult result = selectItem.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["Refno"] = selectItem.GetSelectedString();
                    this.CurrentDetailData.EndEdit();
                }
            };

            setRefno.CellValidating += (s, e) =>
            {
                string strNewRefno = e.FormattedValue.ToString();
                this.CurrentDetailData["Refno"] = strNewRefno;

                #region check Refno
                string strCheckRefno = string.Format(
                    @"
select  distinct refno
from LocalInventory
where   orderID = '{0}'
        and refno = '{1}'", this.CurrentDetailData["OrderID"],
                    strNewRefno);

                if (!strNewRefno.Empty() && !MyUtility.Check.Seek(strCheckRefno))
                {
                    MyUtility.Msg.WarningBox("Data not found.");
                    e.Cancel = true;
                    return;
                }
                #endregion
                #region set Desc
                if (strNewRefno.Empty())
                {
                    this.CurrentDetailData["desc"] = string.Empty;
                }
                else
                {
                    string strDescValue = string.Format(
                        @"
select Description
from LocalItem
where refno = '{0}'", strNewRefno);
                    this.CurrentDetailData["desc"] = MyUtility.GetValue.Lookup(strDescValue);
                }
                #endregion
                #region set Location
                string strLocationValue = string.Format(
                    @"
select ALocation
from LocalInventory
where   OrderID = '{0}'
        and refno = '{1}'
        and ThreadColorID = '{2}'", this.CurrentDetailData["OrderID"],
                    strNewRefno,
                    this.CurrentDetailData["ThreadColorID"]);
                this.CurrentDetailData["Location"] = MyUtility.GetValue.Lookup(strLocationValue);
                #endregion

                this.CurrentDetailData.EndEdit();
            };
            #endregion
            #region ThreadColor Setting
            DataGridViewGeneratorTextColumnSettings setThreadColor = new DataGridViewGeneratorTextColumnSettings();

            setThreadColor.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable selectDt;
                    string strSelectSqlCmd = string.Format(
                        @"
select  distinct ThreadColorID
from LocalInventory
where OrderID = '{0}'", this.CurrentDetailData["OrderID"]);
                    DBProxy.Current.Select(null, strSelectSqlCmd, out selectDt);

                    Win.Tools.SelectItem selectItem = new Win.Tools.SelectItem(selectDt, "ThreadColorID", "20", this.CurrentDetailData["ThreadColorID"].ToString());
                    DialogResult result = selectItem.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["ThreadColorID"] = selectItem.GetSelectedString();
                    this.CurrentDetailData.EndEdit();
                }
            };

            setThreadColor.CellValidating += (s, e) =>
            {
                string strNewThreadColor = e.FormattedValue.ToString();
                this.CurrentDetailData["ThreadColorID"] = strNewThreadColor;

                #region check ThreadColor
                string strCheckThreadColor = string.Format(
                    @"
select  distinct ThreadColorID
from LocalInventory
where   orderID = '{0}'
        and ThreadColorID = '{1}'", this.CurrentDetailData["OrderID"],
                    strNewThreadColor);

                if (!strNewThreadColor.Empty() && !MyUtility.Check.Seek(strCheckThreadColor))
                {
                    MyUtility.Msg.WarningBox("Data not found.");
                    e.Cancel = true;
                    return;
                }
                #endregion
                #region set Location
                string strLocationValue = string.Format(
                    @"
select ALocation
from LocalInventory
where   OrderID = '{0}'
        and refno = '{1}'
        and ThreadColorID = '{2}'", this.CurrentDetailData["OrderID"],
                    this.CurrentDetailData["Refno"],
                    strNewThreadColor);
                this.CurrentDetailData["Location"] = MyUtility.GetValue.Lookup(strLocationValue);
                #endregion

                this.CurrentDetailData.EndEdit();
            };
            #endregion
            #region Unit
            DataGridViewGeneratorTextColumnSettings setUnit = new DataGridViewGeneratorTextColumnSettings();

            setUnit.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable selectDt;
                    string strSelectSqlCmd = string.Format(
                        @"
select  distinct unit = UnitID
from LocalInventory
where   OrderID = '{0}'
        and refno = '{1}'", this.CurrentDetailData["OrderID"],
                        this.CurrentDetailData["Refno"]);
                    DBProxy.Current.Select(null, strSelectSqlCmd, out selectDt);

                    Win.Tools.SelectItem selectItem = new Win.Tools.SelectItem(selectDt, "Unit", "20", this.CurrentDetailData["unit"].ToString());
                    DialogResult result = selectItem.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["unit"] = selectItem.GetSelectedString();
                    this.CurrentDetailData.EndEdit();
                }
            };

            setUnit.CellValidating += (s, e) =>
            {
                string strNewUnit = e.FormattedValue.ToString();
                this.CurrentDetailData["unit"] = strNewUnit;

                #region check ThreadColor
                string strCheckUnit = string.Format(
                    @"
select  distinct UnitID
from LocalInventory
where   orderID = '{0}'
        and Refno = '{1}'
        and UnitID = '{2}'", this.CurrentDetailData["OrderID"],
                    this.CurrentDetailData["Refno"],
                    strNewUnit);

                if (!strNewUnit.Empty() && !MyUtility.Check.Seek(strCheckUnit))
                {
                    MyUtility.Msg.WarningBox("Data not found.");
                    e.Cancel = true;
                    return;
                }
                #endregion

                this.CurrentDetailData.EndEdit();
            };
            #endregion
            #region Issue Qty Setting
            DataGridViewGeneratorNumericColumnSettings setIssueQty = new DataGridViewGeneratorNumericColumnSettings();
            setIssueQty.IsSupportNegative = true;
            setIssueQty.CellValidating += (s, e) =>
            {
                decimal validateValue = Convert.ToDecimal(e.FormattedValue);
                if (validateValue > 99999999)
                {
                    validateValue = 99999999;
                }

                this.CurrentDetailData["Qty"] = validateValue;
                this.CurrentDetailData.EndEdit();
            };
            #endregion

            #region Set Grid
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(10), iseditingreadonly: false, settings: setRefno)
                .Text("ThreadColorID", header: "ThreadColor", width: Widths.AnsiChars(4), iseditingreadonly: false, settings: setThreadColor)
                .EditText("desc", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("unit", header: "Unit", width: Widths.AnsiChars(6), iseditingreadonly: false, settings: setUnit)
                .Numeric("Qty", header: "Issue Qty", iseditingreadonly: false, settings: setIssueQty)
                .EditText("Location", header: "Bulk Location", width: Widths.AnsiChars(10), iseditingreadonly: true);
            #endregion

            for (int i = 0; i < this.detailgrid.Columns.Count; i++)
            {
                this.detailgrid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("confirmed"))
            {
                MyUtility.Msg.InfoBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
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
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "IO", "LocalIssue", (DateTime)this.CurrentMaintain["Issuedate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;
            }
            #endregion
            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            DualResult result;
            List<SqlParameter> listPar = new List<SqlParameter>();
            listPar.Add(new SqlParameter("@ID", this.CurrentMaintain["ID"]));
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
                this.ShowErr(checkStockQty, result);
                return;
            }
            else
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    List<string> listErr = new List<string>();
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        listErr.Add(string.Format(
                            "<SP#>:{0}, <Refno>:{1}, <ThreadColor>:{2}",
                            dr["OrderID"].ToString().Trim(),
                            dr["Refno"].ToString().Trim(),
                            dr["ThreadColorID"].ToString().Trim()));
                    }

                    MyUtility.Msg.InfoBox(listErr.JoinToString("\n\r") + "\n\r Local Stock Quantity can not less then zero!!", "Local Stock Quantity can not less then zero!!");
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
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, strUpdateLocalIssue, listPar)))
                    {
                        transactionScope.Dispose();
                        this.ShowErr(strUpdateLocalIssue, result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, strUpdateLocalInv, listPar)))
                    {
                        transactionScope.Dispose();
                        this.ShowErr(strUpdateLocalInv, result);
                        return;
                    }

                    transactionScope.Complete();
                    transactionScope.Dispose();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception e)
                {
                    transactionScope.Dispose();
                    this.ShowErr("Commit transcation error.", e);
                    return;
                }
            }
            #endregion

        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DualResult result;
            List<SqlParameter> listPar = new List<SqlParameter>();
            listPar.Add(new SqlParameter("@ID", this.CurrentMaintain["ID"]));
            listPar.Add(new SqlParameter("@UserID", Env.User.UserID));
            #region Check 庫存
            DataTable dataTable;
            string checkStockQty = @"
select *
from (
	select	Linv.OrderID
			, Linv.Refno
			, Linv.ThreadColorID 
			, StockQty = Linv.InQty - Linv.OutQty + Linv.AdjustQty + LID.Qty 
	from LocalInventory Linv
	inner join LocalIssue_Detail LID on Linv.OrderID = LID.OrderID 
		and Linv.Refno = LID.Refno and Linv.ThreadColorID = LID.ThreadColorID
	where LID.ID = @ID
) s
where s.StockQty < 0";
            if (!(result = DBProxy.Current.Select(null, checkStockQty, listPar, out dataTable)))
            {
                this.ShowErr(checkStockQty, result);
                return;
            }
            else
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    List<string> listErr = new List<string>();
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        listErr.Add(string.Format(
                            "< SP# > : {0}, < Refno > : {1}, < ThreadColor > : {2}",
                            dr["OrderID"], dr["Refno"], dr["ThreadColorID"]));
                    }

                    MyUtility.Msg.InfoBox(listErr.JoinToString("/n/r"), "Local Stock Quantity can not less then zero!!");
                    return;
                }
            }
            #endregion
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
                        this.ShowErr(strUpdateLocalIssue, result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, strUpdateLocalInv, listPar)))
                    {
                        transactionScope.Dispose();
                        this.ShowErr(strUpdateLocalInv, result);
                        return;
                    }

                    transactionScope.Complete();
                    transactionScope.Dispose();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception e)
                {
                    transactionScope.Dispose();
                    this.ShowErr("Commit transcation error.", e);
                    return;
                }
            }
            #endregion

        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index = this.detailgridbs.Find("OrderID", this.txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            {
                MyUtility.Msg.InfoBox("Data was not found!!");
            }
            else
            {
                this.detailgridbs.Position = index;
            }
        }

        private void BtnClearEmpty_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();
            ((DataTable)this.detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var frm = new P61_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            #region Check Status
            if (this.CurrentMaintain["Status"].EqualString("Confirmed"))
            {
                MyUtility.Msg.InfoBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }
            #endregion
            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            #region Check Status
            if (!this.CurrentMaintain["Status"].EqualString("Confirmed"))
            {
                MyUtility.Msg.InfoBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }
            #endregion
            ReportDefinition report = new ReportDefinition();
            string mDivisonName = MyUtility.GetValue.Lookup(string.Format(
                @"
select NameEN
from factory
where id = '{0}'", Env.User.Keyword));
            string confirmTime = this.CurrentMaintain["Status"].EqualString("CONFIRMED") ? MyUtility.Convert.GetDate(this.CurrentMaintain["EditDate"]).Value.ToString("yyyy/MM/dd HH:mm:ss") : string.Empty;

            #region Set RDLC_Title Data
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("MDivision", mDivisonName));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", this.CurrentMaintain["ID"].ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("FactoryID", this.CurrentMaintain["FactoryID"].ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", this.CurrentMaintain["Remark"].ToString()));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuetime", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("confirmTime", confirmTime));
            #endregion
            #region Set RDLC_Detail Data
            List<P61_PrintData> data = this.detailgrid.GetTable().AsEnumerable()
                .Select(row => new P61_PrintData()
                {
                    POID = row["OrderID"].ToString().Trim(),
                    Refno = row["Refno"].ToString().Trim(),
                    DESCRIPTION = row["Desc"].ToString().Trim(),
                    ThreadColorID = row["ThreadColorID"].ToString().Trim(),
                    QTY = row["Qty"].ToString().Trim(),
                    Location = row["Location"].ToString().Trim(),
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

            var form = new Win.Subs.ReportView(report);
            form.MdiParent = this.MdiParent;
            form.Show();
            #endregion
            return true;
        }
    }
}
