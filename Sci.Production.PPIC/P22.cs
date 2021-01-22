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
            DataGridViewGeneratorNumericColumnSettings inqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings outqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings requestqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings issueqty = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorComboBoxColumnSettings process = new DataGridViewGeneratorComboBoxColumnSettings();
            DataTable processSourceDt = new DataTable();
            Dictionary<string, string> processSourcedata = new Dictionary<string, string>();
            string sqlprocess = $@"Select FullName from Production.dbo.subprocess where IsLackingAndReplacement=1";
            DualResult dualResult = DBProxy.Current.Select(null, sqlprocess, out processSourceDt);
            foreach (DataRow item in processSourceDt.Rows)
            {
                string fullname = MyUtility.Convert.GetString(item["FullName"]);
                processSourcedata.Add(fullname, fullname);
            }

            process.DataSource = new BindingSource(processSourcedata, null);
            process.ValueMember = "Key";
            process.DisplayMember = "Value";

            inqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            outqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            requestqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            issueqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;


            #region RefNo的CoubleClick
            refno.EditingMouseDoubleClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (e.RowIndex != -1)
                    {
                        DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                        EditMemo callNextForm = new EditMemo(MyUtility.Convert.GetString(dr["Description"]), "Description", false, null);
                        callNextForm.ShowDialog(this);
                    }
                }
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("RefNo", header: "Refer#", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: refno)
                .Numeric("RequestQty", header: "Request Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, maximum: 99999999.99M, minimum: 0, settings: requestqty)
                .Text("ReplacementLocalItemReasonID", header: "Reason Id", width: Widths.AnsiChars(5), settings: reason)
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
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FabricType"] = "F";
            this.CurrentMaintain["ApplyName"] = Env.User.UserID;
            this.CurrentMaintain["Status"] = "New";
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
            if (MyUtility.Check.Empty(this.CurrentMaintain["Type"]))
            {
                MyUtility.Msg.WarningBox("Type can't empty");
                this.comboType.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Shift"]))
            {
                MyUtility.Msg.WarningBox("Shift can't empty");
                this.comboShift.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["OrderID"]))
            {
                MyUtility.Msg.WarningBox("SP# can't empty");
                this.txtSP.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ApplyName"]))
            {
                MyUtility.Msg.WarningBox("Handle can't empty");
                this.txtuserHandle.TextBox1.Focus();
                return false;
            }
            #endregion
            int i = 0; // 計算表身Grid的總筆數
            foreach (DataRow dr in this.DetailDatas)
            {
                #region 刪除表身Seq為空白的資料
                if (MyUtility.Check.Empty(dr["Seq"]))
                {
                    dr.Delete();
                    continue;
                }
                #endregion
                i++;
                #region 表身的RequestQty不可小於0、Reason不可為空 、Type='R'時RejectQty不可小(等)於0
                if (MyUtility.Check.Empty(dr["RequestQty"]) || MyUtility.Convert.GetDecimal(dr["RequestQty"]) <= 0)
                {
                    MyUtility.Msg.WarningBox("< Request Qty >  can't equal or less 0!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["PPICReasonID"]))
                {
                    MyUtility.Msg.WarningBox("< Reason Id >  can't empty!");
                    return false;
                }

                if (MyUtility.Convert.GetString(this.CurrentMaintain["Type"]) == "R" && (MyUtility.Check.Empty(dr["RejectQty"]) || MyUtility.Convert.GetInt(dr["RejectQty"]) <= 0))
                {
                    MyUtility.Msg.WarningBox("< # of pcs rejected >  can't equal or less 0!");
                    return false;
                }

                #endregion
            }

            // 表身Grid資料不可為空
            if (i == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty!!");
                return false;
            }

            // RequestQty不可以超過Warehouse的A倉庫存數量
            DataTable exceedData;
            try
            {
                string strSQLSelect = string.Format(
                    @"
select * from (
SELECT l.Seq,l.Seq1,l.Seq2,l.RequestQty,isnull(mpd.InQty-mpd.OutQty+mpd.AdjustQty-mpd.LInvQty,0) as StockQty
FROM #tmp l
left join MDivisionPoDetail mpd WITH (NOLOCK) on mpd.POID = '{0}' and mpd.SEQ1 = l.Seq1 and mpd.SEQ2 = l.Seq2) a
where a.RequestQty > a.StockQty",
                    MyUtility.Convert.GetString(this.CurrentMaintain["POID"]));

                MyUtility.Tool.ProcessWithDatatable(
                    (DataTable)this.detailgridbs.DataSource,
                    "Seq,Seq1,Seq2,RequestQty",
                    strSQLSelect,
                    out exceedData);
            }
            catch (Exception ex)
            {
                this.ShowErr("Save error.", ex);
                return false;
            }

            StringBuilder msg = new StringBuilder();
            if (this.comboType.Text != "Lacking")
            {
                foreach (DataRow dr in exceedData.Rows)
                {
                    msg.Append(string.Format("Seq#:{0}  < Request Qty >:{1} exceed stock qty:{2}\r\n", MyUtility.Convert.GetString(dr["Seq"]), MyUtility.Convert.GetString(dr["RequestQty"]), MyUtility.Convert.GetString(dr["StockQty"])));
                }
            }

            if (msg.Length != 0)
            {
                MyUtility.Msg.WarningBox(msg.ToString());
                return false;
            }

            // GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Env.User.Factory + "FR", "Lack", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = id;
            }

            return base.ClickSaveBefore();
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
                            if (MyUtility.Convert.GetString(orders.Rows[0]["FtyGroup "]) != ftyGroup)
                            {
                                MyUtility.Msg.WarningBox($"Current login factory is {ftyGroup} , it is different factory group with SP# factory {MyUtility.Convert.GetString(orders.Rows[0]["FtyGroup "])}");
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
