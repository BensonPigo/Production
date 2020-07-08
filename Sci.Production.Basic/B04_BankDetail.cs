using System;
using System.Collections.Generic;
using System.Data;
using Ict;
using System.Windows.Forms;
using Sci.Data;
using Ict.Win;
using System.Linq;

namespace Sci.Production.Basic
{
    public partial class B04_BankDetail : Win.Tems.Input6
    {
        private bool _canconfirm;
        private bool _canedit;
        private string LocalSupp_Bank_ID;

        // private ToolStripMenuItem newitem;
        // private ToolStripMenuItem edit;
        public class LocalSupp_Bank_Detail
        {
            public string AccountNo { get; set; }

            public string AccountName { get; set; }

            public string BankName { get; set; }

            public string BranchCode { get; set; }

            public string BranchName { get; set; }

            public string CountryID { get; set; }

            public string Alias { get; set; }

            public string City { get; set; }

            public string SWIFTCode { get; set; }

            public string MidSWIFTCode { get; set; }

            public string MidBankName { get; set; }

            public string Remark { get; set; }

            public bool IsDefault { get; set; }
        }

        public B04_BankDetail(bool canedit, string ID, string keyvalue2, string keyvalue3, bool cancomfirmed, ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            // 按右鍵開啟選單，這一定要加！
            // this.detailgrid.ContextMenuStrip = this.detailgridmenus;

            // 表身點兩下事件
            this.detailgrid.MouseDoubleClick += (s, e) =>
            {
                if (this.EditMode)
                {
                    // 新增
                    if (this.detailgrid.CurrentRow == null)
                    {
                        this.AddNewItem();
                    }
                    else
                    {
                        // DataRow currenRow = this.detailgrid.GetDataRow<DataRow>(this.detailgrid.CurrentRow.Index);
                        // int currentIndex = this.detailgrid.CurrentRow.Index;

                        // if (currenRow["IsDefault"] == DBNull.Value)
                        // {
                        //    // 新增
                        //    this.AddNewItem();
                        // }
                        // else
                        // {
                        //    // 修改
                        //    this.EditItem(currenRow);
                        // }

                        // 修改
                        this.EditItem();
                    }
                }
            };

            // 關閉表身點兩下，自動Insert新Row的功能
            this.InsertDetailGridOnDoubleClick = false;

            this._canconfirm = cancomfirmed;
            this._canedit = canedit;
            this.DefaultFilter = "ID = '" + ID.Trim() + "'";
            this.LocalSupp_Bank_ID = ID;
            if (this.CurrentMaintain == null)
            {
                this.labelStatus.Text = string.Empty;
            }
        }

        private void AddNewItem()
        {
            LocalSupp_Bank_Detail detail = new LocalSupp_Bank_Detail();
            B04_BankData_DetailInput form = new B04_BankData_DetailInput(detail);
            form.ShowDialog();

            if (detail.AccountNo != null)
            {
                DataTable dt = (DataTable)this.detailgridbs.DataSource;
                DataRow nRow = dt.NewRow();

                nRow["IsDefault"] = detail.IsDefault;
                nRow["AccountNo"] = detail.AccountNo;
                nRow["AccountName"] = detail.AccountName;
                nRow["BankName"] = detail.BankName;
                nRow["BranchCode"] = detail.BranchCode;
                nRow["BranchName"] = detail.BranchName;
                nRow["CountryID"] = detail.CountryID;
                nRow["Alias"] = detail.Alias;
                nRow["City"] = detail.City;
                nRow["SWIFTCode"] = detail.SWIFTCode;
                nRow["MidSWIFTCode"] = detail.MidSWIFTCode;
                nRow["MidBankName"] = detail.MidBankName;
                nRow["Remark"] = detail.Remark;
                dt.Rows.Add(nRow);
            }
        }

        private void EditItem()
        {
            if (this.detailgrid.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("There is no selected item !!");
                return;
            }

            DataRow currenRow = this.detailgrid.GetDataRow<DataRow>(this.detailgrid.CurrentRow.Index);
            int currentIndex = this.detailgrid.CurrentRow.Index;

            LocalSupp_Bank_Detail detail = new LocalSupp_Bank_Detail()
            {
                IsDefault = MyUtility.Check.Empty(currenRow["IsDefault"]) ? false : Convert.ToBoolean(currenRow["IsDefault"]),
                AccountNo = currenRow["AccountNo"].ToString(),
                AccountName = currenRow["AccountName"].ToString(),
                BankName = currenRow["BankName"].ToString(),
                BranchCode = currenRow["BranchCode"].ToString(),
                BranchName = currenRow["BranchName"].ToString(),
                CountryID = currenRow["CountryID"].ToString(),
                Alias = currenRow["Alias"].ToString(),
                City = currenRow["City"].ToString(),
                SWIFTCode = currenRow["SWIFTCode"].ToString(),
                MidSWIFTCode = currenRow["MidSWIFTCode"].ToString(),
                MidBankName = currenRow["MidBankName"].ToString(),
                Remark = currenRow["Remark"].ToString(),
            };

            B04_BankData_DetailInput form = new B04_BankData_DetailInput(detail);
            form.ShowDialog();

            if (detail.AccountNo != null)
            {
                currenRow["IsDefault"] = detail.IsDefault;
                currenRow["AccountNo"] = detail.AccountNo;
                currenRow["AccountName"] = detail.AccountName;
                currenRow["BankName"] = detail.BankName;
                currenRow["BranchCode"] = detail.BranchCode;
                currenRow["BranchName"] = detail.BranchName;
                currenRow["CountryID"] = detail.CountryID;
                currenRow["Alias"] = detail.Alias;
                currenRow["City"] = detail.City;
                currenRow["SWIFTCode"] = detail.SWIFTCode;
                currenRow["MidSWIFTCode"] = detail.MidSWIFTCode;
                currenRow["MidBankName"] = detail.MidBankName;
                currenRow["Remark"] = detail.Remark;
            }
        }

        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            this.toolbar.cmdEdit.Enabled = this._canedit && !this.EditMode;
            this.toolbar.cmdNew.Enabled = this._canedit && !this.EditMode;
            this.toolbar.cmdConfirm.Visible = true;
            this.toolbar.cmdCopy.Visible = true;

            if (this.CurrentMaintain != null)
            {
                if (this.tabs.SelectedIndex == 0)
                {
                    this.toolbar.cmdConfirm.Enabled = false;
                    this.toolbar.cmdSave.Enabled = false;
                }
                else
                {
                    this.toolbar.cmdConfirm.Enabled = this._canconfirm && !this.EditMode && this.CurrentMaintain["Status"].ToString().EqualString("New");
                    this.toolbar.cmdSave.Enabled = this._canedit && this.EditMode;
                }
            }
            else
            {
                this.toolbar.cmdConfirm.Enabled = false;
                this.toolbar.cmdSave.Enabled = false;
            }

            bool hasNotConfirm = false;

            hasNotConfirm = MyUtility.Check.Seek($"SELECT 1 FROM LocalSupp_Bank WITH (NOLOCK) WHERE ID = '{this.LocalSupp_Bank_ID}' AND Status <> 'Confirmed'");

            if (!hasNotConfirm && !this.EditMode)
            {
                this.toolbar.cmdCopy.Enabled = true;
            }
            else
            {
                this.toolbar.cmdCopy.Enabled = false;
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // 按右鍵開啟選單
            #region 按右鍵開啟選單

            // this.detailgrid.CellToolTipTextNeeded += (s, e) =>
            // {
            //    e.ToolTipText = "You can show the function form to press the right key under inquiring the state.";

            // };
            // this.detailgridmenus.Items.Clear(); // 清空原有的Menu Item
            // this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Add new Item", onclick: (s, e) => this.AddNewItem()).Get(out this.newitem);
            // this.Helper.Controls.ContextMenu.Generator(this.detailgridmenus).Menu("Edit this Item ", onclick: (s, e) => this.EditItem()).Get(out this.edit);

            // this.SetContextMenuStatus(false); // 預設先將Context ment設定為disable
            #endregion

        }

        // 設定Context Menu的Enable/Disable
        private void SetContextMenuStatus(bool status)
        {
            // this.newitem.Enabled = status;
            // this.edit.Enabled = status;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string ID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            string PKey = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["PKey"]);
            this.DetailSelectCommand = $@"
SELECT  lbd.IsDefault
	,lbd.AccountNo
	,lbd.SWIFTCode
	,lbd.AccountName
	,lbd.BankName
	,lbd.BranchCode
	,lbd.BranchName
	,lbd.CountryID
	,c.Alias
	,lbd.City
	,lbd.MidSWIFTCode
	,lbd.MidBankName
	,lbd.Remark
	,lbd.ID
	,lb.PKey
	,lbd.Ukey
FROM LocalSupp_Bank lb WITH (NOLOCK)  
INNER JOIN LocalSupp_Bank_Detail lbd ON lb.ID=lbd.ID AND  lb.PKey=lbd.PKey 
LEFT JOIN Country c WITH (NOLOCK)  ON c.ID=lbd.CountryID
WHERE lb.ID='{ID}' AND lb.PKey='{PKey}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorTextColumnSettings CountryID = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings AccountNo = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings SWIFTCode = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings AccountName = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings BankName = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings BranchCode = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings BranchName = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings City = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings MidSWIFTCode = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings MidBankName = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings Default = new DataGridViewGeneratorCheckBoxColumnSettings();

            CountryID.MaxLength = 2;
            AccountNo.MaxLength = 30;
            SWIFTCode.MaxLength = 11;
            AccountName.MaxLength = 60;
            BankName.MaxLength = 70;
            BranchCode.MaxLength = 30;
            BranchName.MaxLength = 60;
            CountryID.MaxLength = 2;
            City.MaxLength = 20;
            MidSWIFTCode.MaxLength = 11;
            MidBankName.MaxLength = 70;

            CountryID.MaxLength = 2;
            CountryID.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["CountryID"] = string.Empty;
                        dr["Alias"] = string.Empty;
                        dr.EndEdit();
                        return;
                    }

                    dr["CountryID"] = e.FormattedValue;
                    dr["Alias"] = MyUtility.GetValue.Lookup($"SELECT Alias FROM Country WHERE ID='{e.FormattedValue}'");
                    dr.EndEdit();
                }
            };

            CountryID.CellMouseClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem item;
                    string sqlcmd;
                    sqlcmd = @"
SELECT ID,Alias
FROM Country
order by ID
";

                    item = new Win.Tools.SelectItem(sqlcmd, "5,10", null);

                    item.Size = new System.Drawing.Size(400, 600);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    var selectedData = item.GetSelecteds();
                    dr["CountryID"] = selectedData[0]["id"].ToString();
                    dr["Alias"] = selectedData[0]["Alias"].ToString();
                }
            };

            CountryID.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem item;
                    string sqlcmd;
                    sqlcmd = @"
SELECT ID,Alias
FROM Country
order by ID
";

                    item = new Win.Tools.SelectItem(sqlcmd, "5,10", null);

                    item.Size = new System.Drawing.Size(400, 600);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    var selectedData = item.GetSelecteds();
                    dr["CountryID"] = selectedData[0]["id"].ToString();
                    dr["Alias"] = selectedData[0]["Alias"].ToString();
                }
            };

            Default.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                dr["IsDefault"] = e.FormattedValue;

                // 表頭[Pay by Check] 和表身的Default可同時被勾選 2019/10/30
                // if (Convert.ToBoolean(dr["IsDefault"]))
                // {
                //    this.CurrentMaintain["ByCheck"] = false;
                // }
            };

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .CheckBox("IsDefault", header: "Default", width: Widths.AnsiChars(5), settings: Default, trueValue: 1, falseValue: 0, iseditable: true)
                .Text("AccountNo", header: "Account No.", width: Widths.AnsiChars(13), settings: AccountNo)
                .Text("SWIFTCode", header: "Swift", width: Widths.AnsiChars(13), settings: SWIFTCode)
                .Text("AccountName", header: "Account Name", width: Widths.AnsiChars(13), settings: AccountName)
                .Text("BankName", header: "Bank Name", width: Widths.AnsiChars(13), settings: BankName)
                .Text("BranchCode", header: "Branch Code", width: Widths.AnsiChars(13), settings: BranchCode)
                .Text("BranchName", header: "Branch Name", width: Widths.AnsiChars(13), settings: BranchName)
                .Text("CountryID", header: "Country", width: Widths.AnsiChars(13), settings: CountryID)
                .Text("Alias", header: "Country Name", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("City", header: "City", width: Widths.AnsiChars(13), settings: City)
                .Text("MidSWIFTCode", header: "Intermediary Bank", width: Widths.AnsiChars(13), settings: MidSWIFTCode)
                .Text("MidBankName", header: "Intermediary Bank-SWIFT Code", width: Widths.AnsiChars(13), settings: MidBankName)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(13))
                ;
        }

        protected override void OnDetailEntered()
        {
            if (this.CurrentMaintain != null)
            {
                this.labelStatus.Text = this.CurrentMaintain["Status"].ToString();
                this.txtAbb.Text = MyUtility.GetValue.Lookup($"SELECT TOP 1 Abb FROM LocalSupp WHERE ID='{this.CurrentMaintain["ID"]}'");
            }

            base.OnDetailEntered();

            if (this.EditMode)
            {
                // 先將Menu狀態全打開
                this.SetContextMenuStatus(true);
            }
            else
            {
                this.SetContextMenuStatus(false);
            }
        }

        protected override bool ClickNewBefore()
        {
            // 判斷[LocalSupp_Bank].Status若有一筆為NEW, 則不能新增
            if (
                    MyUtility.Check.Seek($"SELECT * FROM LocalSupp_Bank WHERE ID='{this.LocalSupp_Bank_ID}' AND Status = 'New'"))
            {
                MyUtility.Msg.InfoBox("Still have data not yet confirm, so can't create new record!");
                return false;
            }
            else
            {
                return base.ClickNewBefore();
            }
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["ID"] = this.LocalSupp_Bank_ID;
            this.txtAbb.Text = MyUtility.GetValue.Lookup($"SELECT TOP 1 Abb FROM LocalSupp WHERE ID='{this.CurrentMaintain["ID"]}'");
            this.CurrentMaintain["Status"] = "New";
        }

        protected override bool ClickSaveBefore()
        {
            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            bool hasEmpty = false;
            bool allEmpty = false;
            int defaultCount = 0;

            // 表頭Pay by Check打勾 = 表身Default全不勾
            // 表頭Pay by Check沒打勾 = 表身Default打勾不能超過一筆
            foreach (DataRow dr in dt.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted))
            {
                if (dr["IsDefault"] == DBNull.Value)
                {
                    dr["IsDefault"] = false;
                }

                // 打勾Default的那筆, 欄位除["BranchCode"] ["BranchName"] [Intermediary Bank][Intermediary Bank-SWIFT Code][Remark][SWIFTCode], 其他欄位必填
                if (Convert.ToBoolean(dr["IsDefault"]))
                {
                    defaultCount += 1;
                    if (
                        MyUtility.Check.Empty(dr["AccountNo"]) ||
                        MyUtility.Check.Empty(dr["AccountName"]) ||
                        MyUtility.Check.Empty(dr["BankName"]) ||
                        MyUtility.Check.Empty(dr["CountryID"]) ||
                        MyUtility.Check.Empty(dr["City"]))
                    {
                        hasEmpty = true;
                    }
                }

                // 沒打勾Default的那筆,不能全空
                else
                {
                    if (
                        MyUtility.Check.Empty(dr["AccountNo"]) &&
                        MyUtility.Check.Empty(dr["AccountName"]) &&
                        MyUtility.Check.Empty(dr["BankName"]) &&
                        MyUtility.Check.Empty(dr["BranchCode"]) &&
                        MyUtility.Check.Empty(dr["BranchName"]) &&
                        MyUtility.Check.Empty(dr["CountryID"]) &&
                        MyUtility.Check.Empty(dr["City"]) &&
                        MyUtility.Check.Empty(dr["SWIFTCode"]) &&
                        MyUtility.Check.Empty(dr["MidSWIFTCode"]) &&
                        MyUtility.Check.Empty(dr["MidBankName"]) &&
                        MyUtility.Check.Empty(dr["Remark"]))
                    {
                        allEmpty = true;
                    }
                }
            }

            // 表身Deafult只可勾一筆
            if (defaultCount > 1)
            {
                MyUtility.Msg.InfoBox("The count of checked can not be more than one.");
                return false;
            }

            // Pay by Check沒打勾
            if (!Convert.ToBoolean(this.CurrentMaintain["ByCheck"]))
            {
                if (defaultCount == 0)
                {
                    MyUtility.Msg.InfoBox("There is NO data is checked.");
                    return false;
                }

                if (hasEmpty || allEmpty)
                {
                    MyUtility.Msg.InfoBox("If default is checked, column can not be empty except [BranchCode] [BranchName] [Intermediary Bank] , [Intermediary Bank-SWIFT Code] and [Remark]." + Environment.NewLine + "If default is NOT checked, column can not be all empty.");
                    return false;
                }
            }

            // Pay by Check打勾
            else
            {
                // if (allEmpty)
                // {
                //    MyUtility.Msg.InfoBox("If default is checked, column can not be empty except [Intermediary Bank] , [Intermediary Bank-SWIFT Code] and [Remark]." + Environment.NewLine + "If default is NOT checked, column can not be all empty.");
                //    return false;
                // }
            }

            return base.ClickSaveBefore();
        }

        protected override DualResult OnSaveDetail(IList<DataRow> details, ITableSchema detailtableschema)
        {
            return base.OnSaveDetail(details, detailtableschema);
        }

        private void chkPaybyCheck_CheckedChanged(object sender, EventArgs e)
        {
            // if (this.detailgridbs.DataSource != null)
            // {
            //    DataTable dt = (DataTable)this.detailgridbs.DataSource;
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        if (this.chkPaybyCheck.Checked)
            //        {
            //            dr["IsDefault"] = false;
            //        }
            //    }
            // }
        }

        protected override void ClickConfirm()
        {
            int defaultCount = 0;
            if (this.detailgridbs.DataSource != null)
            {
                DataTable dt = (DataTable)this.detailgridbs.DataSource;
                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToBoolean(dr["IsDefault"]))
                    {
                        defaultCount++;
                    }
                }
            }

            // 表頭[Pay by Check] 和表身的Default可同時被勾選 2019/10/30
            // if (this.chkPaybyCheck.Checked || defaultCount == 1)
            // {
            //    DBProxy.Current.Execute(null , $"UPDATE LocalSupp_Bank SET Status='Confirmed',ApproveName='{Sci.Env.User.UserID}' ,ApproveDate=GETDATE() WHERE ID='{this.CurrentMaintain["ID"]}' AND PKey = '{this.CurrentMaintain["PKey"]}' ");
            // }
            // else
            // {
            //    MyUtility.Msg.InfoBox("Can not Confirm because of [Pay by Check] or any detail [Default] is checked.");
            //    return;
            // }
            DBProxy.Current.Execute(null, $"UPDATE LocalSupp_Bank SET Status='Confirmed',ApproveName='{Sci.Env.User.UserID}' ,ApproveDate=GETDATE() WHERE ID='{this.CurrentMaintain["ID"]}' AND PKey = '{this.CurrentMaintain["PKey"]}' ");
            base.ClickConfirm();
        }

        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("This record is < Confirmed >, can't be modified!");
                return false;
            }

            return base.ClickEditBefore();
        }

        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();

            DataTable dt;
            DataTable dtDetail;
            DualResult result = DBProxy.Current.Select(null, $"SELECT TOP 1 * FROM LocalSupp_Bank WITH (NOLOCK) WHERE ID = '{this.CurrentMaintain["ID"]}' AND Status = 'Confirmed'ORDER BY ApproveDate DESC", out dt);

            if (result)
            {
                DataRow dr = dt.Rows[0];

                this.CurrentMaintain["ID"] = this.LocalSupp_Bank_ID;
                this.CurrentMaintain["ByCheck"] = dr["ByCheck"];
                this.CurrentMaintain["Status"] = "New";
                this.labelStatus.Text = this.CurrentMaintain["Status"].ToString();
                this.CurrentMaintain["ApproveName"] = string.Empty;
                this.CurrentMaintain["ApproveDate"] = DBNull.Value;

                this.CurrentMaintain["AddName"] = string.Empty;
                this.CurrentMaintain["AddDate"] = DBNull.Value;
                this.CurrentMaintain["EditName"] = string.Empty;
                this.CurrentMaintain["EditDate"] = DBNull.Value;

                string cmd = $@"
SELECT  lbd.IsDefault
	,lbd.AccountNo
	,lbd.SWIFTCode
	,lbd.AccountName
	,lbd.BankName
	,lbd.BranchCode
	,lbd.BranchName
	,lbd.CountryID
	,c.Alias
	,lbd.City
	,lbd.MidSWIFTCode
	,lbd.MidBankName
	,lbd.Remark
	,lbd.ID
	,lb.PKey
	,lbd.Ukey
FROM LocalSupp_Bank lb WITH (NOLOCK)  
INNER JOIN LocalSupp_Bank_Detail lbd ON lb.ID=lbd.ID AND  lb.PKey=lbd.PKey 
LEFT JOIN Country c WITH (NOLOCK)  ON c.ID=lbd.CountryID
WHERE lb.ID='{this.LocalSupp_Bank_ID}' AND lb.PKey='{dr["Pkey"]}'
";

                DBProxy.Current.Select(null, cmd, out dtDetail);
                this.detailgridbs.DataSource = dtDetail;
            }
        }
    }
}
