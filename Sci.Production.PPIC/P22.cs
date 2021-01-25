using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;
using System.Runtime.InteropServices;
using System.Transactions;
using Sci.Win.Tools;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class P22 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P22(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Env.User.Keyword + "' and FabricType = 'F'";
            this.txtuserApprove.TextBox1.ReadOnly = true;
            this.txtuserApprove.TextBox1.IsSupportEditMode = false;
            this.InsertDetailGridOnDoubleClick = false;
        }

        /// <inheritdoc/>
        public P22(ToolStripMenuItem menuitem, string id)
        {
            this.InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Env.User.Keyword + "' and FabricType = 'F' and ID = '" + id + "'";
            this.txtuserApprove.TextBox1.ReadOnly = true;
            this.txtuserApprove.TextBox1.IsSupportEditMode = false;
            this.InsertDetailGridOnDoubleClick = false;
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = $@"
SELECT rd.Refno
	,rd.RequestQty
	,rd.ReplacementLocalItemReasonID
	,rr.Description
	,rd.Remark
FROM ReplacementLocalItem r
INNER JOIN ReplacementLocalItem_Detail rd ON r.ID = rd.ID
LEFT JOIN ReplacementLocalItemReason rr ON rd.ReplacementLocalItemReasonID = rr.ID

where r.ID = '{masterID}'
";

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.lbStatus.Text = this.CurrentMaintain["status"].ToString().Trim();

            if (this.CurrentMaintain["Shift"].Equals("O") && this.EditMode)
            {
                this.txtLocalSupp1.TextBox1.ReadOnly = false;
            }
            else
            {
                this.txtLocalSupp1.TextBox1.ReadOnly = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorTextColumnSettings refno = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings reason = new DataGridViewGeneratorTextColumnSettings();

            #region RefNo事件
            refno.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (e.RowIndex != -1)
                    {
                        string sqlCmd = $@"
select RefNo,Description, LocalSuppid, UnitID ,Price 
from LocalItem 
where Junk = 0 AND Category = 'Carton'
";

                        SelectItem sel = new SelectItem(sqlCmd, "RefNo,Description,LocalSuppid,UnitID,Price", this.CurrentDetailData["RefNo"].ToString(), null);
                        DialogResult res = sel.ShowDialog();
                        if (res == DialogResult.Cancel)
                        {
                            return;
                        }

                        this.CurrentDetailData["RefNo"] = sel.GetSelecteds()[0]["RefNo"];
                        this.CurrentDetailData.EndEdit();
                    }
                }
            };

            refno.CellMouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (e.RowIndex != -1)
                    {
                        string sqlCmd = $@"
select RefNo,Description, LocalSuppid, UnitID ,Price 
from LocalItem 
where Junk = 0 AND Category = 'Carton'
";

                        SelectItem sel = new SelectItem(sqlCmd, "RefNo,Description,LocalSuppid,UnitID,Price", this.CurrentDetailData["RefNo"].ToString(), null);
                        DialogResult res = sel.ShowDialog();
                        if (res == DialogResult.Cancel)
                        {
                            return;
                        }

                        this.CurrentDetailData["RefNo"] = sel.GetSelecteds()[0]["RefNo"];
                        this.CurrentDetailData.EndEdit();
                    }
                }
            };

            refno.CellValidating += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                if (this.EditMode && e.FormattedValue.ToString() != string.Empty)
                {

                    List<SqlParameter> sqlpara = new List<SqlParameter>
                    {
                        new SqlParameter("@RefNo", e.FormattedValue),
                    };

                    string sqlCmd = $@"
select RefNo,Description, LocalSuppid, UnitID ,Price 
from LocalItem 
where Junk = 0 AND Category = 'Carton'
AND RefNo = @RefNo
";

                    if (MyUtility.Check.Seek(sqlCmd, sqlpara, out DataRow dr))
                    {
                        this.CurrentDetailData["RefNo"] = e.FormattedValue;
                        this.CurrentDetailData.EndEdit();
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("Data not found!");
                        this.CurrentDetailData["RefNo"] = string.Empty;
                        this.CurrentDetailData.EndEdit();
                        return;
                    }
                }
                else if (this.EditMode && e.FormattedValue.ToString() == string.Empty)
                {
                    this.CurrentDetailData["RefNo"] = string.Empty;
                    this.CurrentDetailData.EndEdit();
                }
            };
            #endregion

            #region ReplacementLocalItemReasonID事件
            reason.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (e.RowIndex != -1)
                    {
                        List<SqlParameter> paras = new List<SqlParameter>();
                        string sqlCmd = $@"
select ID , Description
from ReplacementLocalItemReason  
where Junk = 0 AND Type='R'
AND ID  = @ID 
";
                        paras.Add(new SqlParameter("@ID", this.CurrentDetailData["ReplacementLocalItemReasonID"].ToString()));

                        SelectItem sel = new SelectItem(sqlCmd, paras, "ID,Description", this.CurrentDetailData["ReplacementLocalItemReasonID"].ToString(), null);
                        DialogResult res = sel.ShowDialog();
                        if (res == DialogResult.Cancel)
                        {
                            return;
                        }

                        this.CurrentDetailData["ReplacementLocalItemReasonID"] = sel.GetSelecteds()[0]["ID"];
                        this.CurrentDetailData.EndEdit();
                    }
                }
            };

            reason.CellMouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (e.RowIndex != -1)
                    {
                        List<SqlParameter> paras = new List<SqlParameter>();
                        string sqlCmd = $@"
select ID , Description
from ReplacementLocalItemReason  
where Junk = 0 AND Type='R'
AND ID  = @ID 
";
                        paras.Add(new SqlParameter("@ID", this.CurrentDetailData["ReplacementLocalItemReasonID"].ToString()));

                        SelectItem sel = new SelectItem(sqlCmd, paras, "ID,Description", this.CurrentDetailData["ReplacementLocalItemReasonID"].ToString(), null);
                        DialogResult res = sel.ShowDialog();
                        if (res == DialogResult.Cancel)
                        {
                            return;
                        }

                        this.CurrentDetailData["ReplacementLocalItemReasonID"] = sel.GetSelecteds()[0]["ID"];
                        this.CurrentDetailData.EndEdit();
                    }
                }
            };

            reason.CellValidating += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                if (this.EditMode && e.FormattedValue.ToString() != string.Empty)
                {

                    List<SqlParameter> sqlpara = new List<SqlParameter>
                    {
                        new SqlParameter("@ID ", e.FormattedValue),
                    };

                    string sqlCmd = $@"
select ID , Description
from ReplacementLocalItemReason  
where Junk = 0 AND Type='R'
AND ID  = @ID 
";

                    if (MyUtility.Check.Seek(sqlCmd, sqlpara, out DataRow dr))
                    {
                        this.CurrentDetailData["ReplacementLocalItemReasonID"] = e.FormattedValue;
                        this.CurrentDetailData["Description"] = MyUtility.Convert.GetString(dr["Description"]);
                        this.CurrentDetailData.EndEdit();
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("Data not found!");
                        this.CurrentDetailData["ReplacementLocalItemReasonID"] = string.Empty;
                        this.CurrentDetailData["Description"] = string.Empty;
                        this.CurrentDetailData.EndEdit();
                        return;
                    }
                }
                else if (this.EditMode && e.FormattedValue.ToString() == string.Empty)
                {
                    this.CurrentDetailData["ReplacementLocalItemReasonID"] = string.Empty;
                    this.CurrentDetailData["Description"] = string.Empty;
                    this.CurrentDetailData.EndEdit();
                }
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("RefNo", header: "Refer#", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: refno)
                .Numeric("RequestQty", header: "Request Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, maximum: 99999999.99M, minimum: 0)
                .Text("ReplacementLocalItemReasonID", header: "Reason Id", width: Widths.AnsiChars(5), iseditingreadonly: false, settings: reason)
                .EditText("Description", header: "Reason", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: false);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(this.comboType, 2, 1, "L,Lacking,R,Replacement");
            MyUtility.Tool.SetupCombox(this.comboShift, 2, 1, "D,Day,N,Night,O,Subcon-Out");
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = string.Empty;
                        break;
                    default:
                        this.DefaultWhere = string.Format("FactoryID = '{0}'", this.queryfors.SelectedValue);
                        break;
                }

                this.ReloadDatas();
            };
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["IssueDate"] = DateTime.Today;
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["ApplyName"] = Env.User.UserID;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "R";
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) != "New")
            {
                MyUtility.Msg.WarningBox("This record was approved, can't modify.");
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            if (this.CurrentMaintain["Status"].ToString() != "New")
            {
                this.txtDept.ReadOnly = true;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) != "New")
            {
                MyUtility.Msg.WarningBox("This record was approved, can't delete.");
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["OrderID"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["ApplyName"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["SewingLineID"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["SubconName"]))
            {
                MyUtility.Msg.WarningBox("<SP#>,<Handle>,<Sewing Line>,<Subcon Name>, can't empty");
                return false;
            }
            #endregion

            List<string> errorRefno = new List<string>();
            foreach (DataRow dr in this.DetailDatas)
            {
                // RequestQty 都須 > 0
                if (MyUtility.Check.Empty(dr["RequestQty"]) || MyUtility.Convert.GetDecimal(dr["RequestQty"]) <= 0)
                {
                    MyUtility.Msg.WarningBox("< Request Qty >  can't equal or less 0!");
                    return false;
                }

                // 表頭選的 SubConName, 必須和表身 Refno對應 LocalItem. LocalSuppid一致
                string cmd = $@"
SELECT 1
FROM LocalItem
WHERE LocalSuppid = '{this.CurrentMaintain["SubConName"]}'
AND Refno = '{dr["Refno"]}'
";
                if (!MyUtility.Check.Seek(cmd))
                {
                    errorRefno.Add(MyUtility.Convert.GetString(dr["Refno"]));
                }
            }

            if (errorRefno.Count > 0)
            {
                MyUtility.Msg.WarningBox($"Refno# : {errorRefno.Distinct().JoinToString(",")} not belongs to {this.CurrentMaintain["SubConName"]}");
                return false;
            }

            // GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Env.User.Factory + "RC", "ReplacementLocalItem", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = id;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            DualResult result;

            string updateCmd = $@"
update ReplacementLocalItem set 
Status = 'Confirmed'
,ApvName = '{Env.User.UserID}'
,ApvDate = GetDate()
,EditName = '{Env.User.UserID}'
,EditDate = GetDate() 
where ID = '{this.CurrentMaintain["ID"]}'
";
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm fail!\r\n" + result.ToString());
                return;
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DualResult result;

            string updateCmd = $@"
update ReplacementLocalItem set 
Status = 'New'
,ApvName = ''
,ApvDate = NULL
,EditName = '{Env.User.UserID}'
,EditDate = GetDate() 
where ID = '{this.CurrentMaintain["ID"]}'
";
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Unconfirm fail!\r\n" + result.ToString());
                return;
            }
        }

        // SP#
        private void TxtSP_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode)
            {
                if (this.txtSP.OldValue != this.txtSP.Text)
                {
                    if (!MyUtility.Check.Empty(this.txtSP.Text))
                    {
                        // 根據 this.txtSP.Text 取得POID
                        string orderid = this.txtSP.Text;
                        string poid = MyUtility.GetValue.Lookup("poid", orderid, "orders", "id");
                        string sqlCmd = string.Empty;
                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

                        #region 驗證1：使用者登入 Factory 必須是 Factory.IsProduceFty =1
                        DataTable ftyGroupData;

                        sqlCmd = $@"select FTYGroup from Factory where id='{Env.User.Factory}' and IsProduceFty = 1";
                        DBProxy.Current.Select(null, sqlCmd, out ftyGroupData);
                        if (ftyGroupData.Rows.Count == 0)
                        {
                            MyUtility.Msg.WarningBox("SP No. not found!!");
                            this.CurrentMaintain["OrderID"] = string.Empty;
                            this.CurrentMaintain["POID"] = string.Empty;
                            this.CurrentMaintain["FactoryID"] = string.Empty;
                            e.Cancel = true;
                            return;
                        }

                        // Get FTYGroup
                        string ftyGroup = ftyGroupData.Rows[0]["FTYGroup"].ToString();
                        #endregion

                        #region 驗證2：Orders.FtyGroup 必須是 與登入者的FTYGroup相同
                        cmds.Add(new System.Data.SqlClient.SqlParameter("@OrderID", orderid));

                        DataTable orders;
                        sqlCmd = "select FtyGroup from Orders WITH (NOLOCK) where ID = @OrderID";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out orders);

                        if (result && orders.Rows.Count > 0)
                        {
                            if (MyUtility.Convert.GetString(orders.Rows[0]["FtyGroup"]) != ftyGroup)
                            {
                                MyUtility.Msg.WarningBox($"Current login factory is {ftyGroup} , it is different factory group with SP# factory {MyUtility.Convert.GetString(orders.Rows[0]["FtyGroup"])}");
                                this.CurrentMaintain["OrderID"] = string.Empty;
                                this.CurrentMaintain["POID"] = string.Empty;
                                this.CurrentMaintain["FactoryID"] = string.Empty;
                                e.Cancel = true;
                                return;
                            }

                            // 通過驗證
                            this.CurrentMaintain["OrderID"] = this.txtSP.Text;
                            this.CurrentMaintain["POID"] = poid;
                            this.CurrentMaintain["FactoryID"] = ftyGroup;
                        }
                        else
                        {
                            this.CurrentMaintain["OrderID"] = string.Empty;
                            this.CurrentMaintain["POID"] = string.Empty;
                            this.CurrentMaintain["FactoryID"] = string.Empty;
                            this.DeleteAllGridData();
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("SP# not exist!!");
                            return;
                        }
                        #endregion
                    }
                    else
                    {
                        this.CurrentMaintain["OrderID"] = string.Empty;
                        this.CurrentMaintain["POID"] = string.Empty;
                        this.CurrentMaintain["FactoryID"] = string.Empty;
                    }

                    this.DeleteAllGridData();
                }
            }
        }

        // 刪除表身Grid資料
        private void DeleteAllGridData()
        {
            foreach (DataRow dr in this.DetailDatas)
            {
                dr.Delete();
            }
        }

        private void P22_FormLoaded(object sender, EventArgs e)
        {
            DataTable queryDT;
            string querySql = string.Format(
                @"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out queryDT);
            MyUtility.Tool.SetupCombox(this.queryfors, 1, queryDT);
            this.queryfors.SelectedIndex = 0;
        }
    }
}
