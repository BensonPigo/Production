﻿using Ict;
using Ict.Win;
using Newtonsoft.Json;
using Sci.Data;
using Sci.Production.CallPmsAPI;
using Sci.Production.PublicPrg;
using Sci.Win;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static Sci.Production.CallPmsAPI.PackingA2BWebAPI_Model;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P08
    /// </summary>
    public partial class P08 : Win.Tems.Input6
    {
        // Dictionary<String, String> comboBox2_RowSource1 = new Dictionary<string, string>();
        // Dictionary<String, String> comboBox2_RowSource2 = new Dictionary<string, string>();
        private DataTable subType_1 = new DataTable();
        private DataTable subType_2 = new DataTable();
        private BindingSource comboxbs1;

        // , comboxbs2_1, comboxbs2_2;
        private DataGridViewGeneratorTextColumnSettings code = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorNumericColumnSettings qty = new DataGridViewGeneratorNumericColumnSettings();
        private DataGridViewGeneratorNumericColumnSettings rate = new DataGridViewGeneratorNumericColumnSettings();
        private DataGridViewGeneratorNumericColumnSettings price = new DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.UI.DataGridViewTextBoxColumn col_code;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_qty;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_rate;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_remark;
        private bool haveEditShareFee;

        private bool IsExistsBLNo
        {
            get
            {
                string sqlCheck = $@"
select  1   from    GMTBooking with (nolock) where BLNo = '{this.CurrentMaintain["BLNo"]}' or BL2No = '{this.CurrentMaintain["BLNo"]}'
union all
select  1   from    Export with (nolock) where BLNo = '{this.CurrentMaintain["BLNo"]}'
union all
select  1   from    FtyExport with (nolock) where BLNo = '{this.CurrentMaintain["BLNo"]}'
union all
select  1   from    TransferExport with (nolock) where BLNo = '{this.CurrentMaintain["BLNo"]}'
";
                return MyUtility.Check.Seek(sqlCheck);
            }
        }

        private bool IsCustoms
        {
            get
            {
                return MyUtility.Check.Seek($"select 1 from LocalSupp with (nolock) where ID = '{this.CurrentMaintain["LocalSuppID"]}' and IsCustoms = 1");
            }
        }

        /// <summary>
        /// P08
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.detailgrid.AllowUserToOrderColumns = true;
            this.InsertDetailGridOnDoubleClick = false;
            this.txtSserAccountant.TextBox1.IsSupportEditMode = false;
            this.txtSserAccountant.TextBox1.ReadOnly = true;
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory1, 1, factory);
            this.comboFactory1.SelectedIndex = 0;

            #region 輸入Supplier後自動帶出Currency & Terms，清空表身Grid資料
            this.txtSubconSupplier.TextBox1.Validated += (s, e) =>
                {
                    if (this.EditMode == true && this.txtSubconSupplier.TextBox1.ReadOnly == false)
                    {
                        if (MyUtility.Check.Empty(this.CurrentMaintain["LocalSuppID"]))
                        {
                            this.CurrentMaintain["CurrencyID"] = string.Empty;
                            this.CurrentMaintain["PayTermID"] = string.Empty;
                        }
                        else
                        {
                            DataRow dr;
                            if (MyUtility.Check.Seek(string.Format("select * from LocalSupp WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["LocalSuppID"])), out dr))
                            {
                                this.CurrentMaintain["CurrencyID"] = dr["CurrencyID"];
                                this.CurrentMaintain["PayTermID"] = dr["PayTermID"];
                            }
                            else
                            {
                                this.CurrentMaintain["CurrencyID"] = string.Empty;
                                this.CurrentMaintain["PayTermID"] = string.Empty;
                            }
                        }

                        // 清空表身Grid資料
                        foreach (DataRow dr in this.DetailDatas)
                        {
                            dr.Delete();
                        }
                    }
                };
            #endregion

            #region 有New權限 才能copy

            string sqlcmd = $@"
select p2.CanNew,p1.IsAdmin,p1.IsMIS 
from Pass1 p1
left join Pass0 p on p1.FKPass0=p.PKey
left join Pass2 p2 on p2.FKPass0=p.PKey
where p1.ID='{Env.User.UserID}'
and FKMenu= (select PKey from MenuDetail where FormName='Sci.Production.Shipping.P08')";
            DataRow drCopy;
            if (MyUtility.Check.Seek(sqlcmd, out drCopy))
            {
                if (!MyUtility.Check.Empty(drCopy["CanNew"]) || !MyUtility.Check.Empty(drCopy["IsAdmin"])
                    || !MyUtility.Check.Empty(drCopy["IsMIS"]))
                {
                    this.IsSupportCopy = true;
                }
            }
            #endregion
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // 設定ComboBox的內容值
            this.subType_1.ColumnsStringAdd("Key");
            this.subType_1.ColumnsStringAdd("Value");
            this.subType_1.Rows.Add("MATERIAL", "MATERIAL");
            this.subType_1.Rows.Add("SISTER FACTORY TRANSFER", "SISTER FACTORY TRANSFER");
            this.subType_1.Rows.Add("OTHER", "OTHER");

            this.subType_2.ColumnsStringAdd("Key");
            this.subType_2.ColumnsStringAdd("Value");
            this.subType_2.Rows.Add("GARMENT", "GARMENT");
            this.subType_2.Rows.Add("SISTER FACTORY TRANSFER", "SISTER FACTORY TRANSFER");
            this.subType_2.Rows.Add("OTHER", "OTHER");

            this.comboType2.ValueMember = "Key";
            this.comboType2.DisplayMember = "Value";

            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("IMPORT", "IMPORT");
            comboBox1_RowSource.Add("EXPORT", "EXPORT");
            this.comboxbs1 = new BindingSource(comboBox1_RowSource, null);
            this.comboType.DataSource = this.comboxbs1;
            this.comboType.ValueMember = "Key";
            this.comboType.DisplayMember = "Value";
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(
                @"
select sd.*,isnull(se.Description,'') as Description
    , (isnull(sd.AccountID,'') + '-' + isnull(a.Name,'')) as Account
    ,se.UnitID
    ,a.IsAPP ,a.IsShippingVAT,a2.AdvancePaymentTPE
from ShippingAP_Detail sd WITH (NOLOCK) 
left join ShipExpense se WITH (NOLOCK) on se.ID = sd.ShipExpenseID
left join SciFMS_AccountNO a on a.ID = sd.AccountID
left join SciFMS_AccountNO a2 on a2.ID = substring(sd.AccountID,1,4)
where sd.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override DualResult OnRenewDataPost(Win.Tems.Input1.RenewDataPostEventArgs e)
        {
            if (e.Data != null)
            {
                string exact = MyUtility.GetValue.Lookup("Exact", MyUtility.Convert.GetString(e.Data["CurrencyID"]), "Currency", "ID");
                int decimalPlaces = MyUtility.Check.Empty(exact) ? 0 : Convert.ToInt32(exact);
                this.numAmount.DecimalPlaces = decimalPlaces;
                this.numVAT.DecimalPlaces = decimalPlaces;
                this.numTotal.DecimalPlaces = decimalPlaces;
                this.numTotal.Value = MyUtility.Convert.GetDecimal(e.Data["Amount"]) + MyUtility.Convert.GetDecimal(e.Data["VAT"]);
            }

            return base.OnRenewDataPost(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.RefreshIsFreightForwarder();

            // ChangeCombo2DataSource();
            #region set comboBox2 DataSource
            switch (this.CurrentMaintain["Type"].ToString())
            {
                case "IMPORT":
                    // comboxbs2_1.Position = 0;
                    this.comboType2.DataSource = this.subType_1;

                    // CurrentMaintain["SubType"] = temp;
                    break;
                case "EXPORT":
                    // comboxbs2_2.Position = 0;
                    this.comboType2.DataSource = this.subType_2;

                    // CurrentMaintain["SubType"] = temp;
                    break;
            }
            #endregion
            List<System.Data.SqlClient.SqlParameter> listPara = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@blno", MyUtility.Convert.GetString(this.CurrentMaintain["BLNo"])),
                    new System.Data.SqlClient.SqlParameter("@Reason", MyUtility.Convert.GetString(this.CurrentMaintain["Reason"])),
                };

            bool status = MyUtility.Check.Empty(this.CurrentMaintain["Accountant"]);
            this.btnAcctApprove.Enabled = status ? !this.EditMode && Prgs.GetAuthority(Env.User.UserID, "P08. Account Payment - Shipping", "CanConfirm") : MyUtility.Check.Empty(this.CurrentMaintain["VoucherID"]) && Prgs.GetAuthority(this.CurrentMaintain["Accountant"].ToString(), "P08. Account Payment - Shipping", "CanUnConfirm");
            this.btnAcctApprove.Text = status ? "Acct. Approve" : "Acct. Unapprove";
            this.btnAcctApprove.ForeColor = status ? Color.Blue : Color.Black;
            this.comboType2.SelectedValue = this.CurrentMaintain["SubType"].ToString();
            this.disExVoucherID.Text = this.CurrentMaintain["ExVoucherID"].ToString();
            this.btnIncludeFoundryRatio.Enabled = MyUtility.Check.Seek($@"
SELECT  1
from ShareExpense se WITH (NOLOCK) 
INNER JOIN GMTBooking g ON se.InvNo = g.ID
where   ShippingAPID = '{this.CurrentMaintain["ID"]}'
and (Junk = 0 or Junk is null)
AND g.Foundry = 1
");

            // Reason description
            this.txtReasonDesc.Text = MyUtility.GetValue.Lookup(
                                $@"select Description from ShippingReason where id=@Reason 
                                    and type='AP' and junk=0",
                                listPara);
            string sql = "select top 1 Vessel from Export where blno = @blno";
            this.disVesselName.Text = this.CurrentMaintain["Type"].ToString().Equals("IMPORT") && !MyUtility.Check.Empty(this.CurrentMaintain["BLNo"]) ? MyUtility.GetValue.Lookup(sql, listPara) : string.Empty;
            this.IncludeFoundryRefresh(MyUtility.Convert.GetString(this.CurrentMaintain["BLNo"]));

            if (this.CurrentMaintain["Status"].ToString() != "Approved" && this.EditMode)
            {
                string localSuppID = this.CurrentMaintain["LocalSuppID"].ToString();

                string isFactory = MyUtility.GetValue.Lookup($@"SELECT IsFactory FROM LocalSupp WHERE ID = '{localSuppID}'");

                if (MyUtility.Check.Empty(isFactory) ? false : Convert.ToBoolean(isFactory))
                {
                    this.txtcurrency.ReadOnly = false;
                    this.txtcurrency.IsSupportEditMode = true;
                }
                else
                {
                    this.txtcurrency.ReadOnly = true;
                    this.txtcurrency.IsSupportEditMode = false;
                }
            }
            else
            {
                this.txtcurrency.ReadOnly = true;
                this.txtcurrency.IsSupportEditMode = false;
            }
        }

        private void IncludeFoundryRefresh(string blNo)
        {
            this.chkIncludeFoundry.Checked = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup(string.Format(" select top 1 Foundry from GMTBooking where '{0}' != '' and (BLNo = '{0}' or BL2No = '{0}') and Foundry = 1", blNo)));
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            #region Code的按右鍵與Validating、Qty與Rate的Validating
            this.code.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode &&
                    e.Button == MouseButtons.Right &&
                    e.RowIndex != -1 &&
                    MyUtility.Check.Empty(this.CurrentMaintain["Accountant"]))
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    string localSuppID = MyUtility.Convert.GetString(this.CurrentMaintain["LocalSuppID"]);
                    string expressTypeID = this.comboType.SelectedValue.ToString().ToLower().EqualString("export") ? "1" : "2";
                    string sqlCmd = string.Format(
                        @"
select ID
	, Description
	, [Brand] = BrandID
	, CurrencyID
	, Price
	, [Unit] = UnitID
    , AccountID
from ShipExpense WITH (NOLOCK)
where exists (select 1 from SciFMS_AccountNo sa where sa.ID = ShipExpense.AccountID and sa.ExpressTypeID in ({1}, 3))
and Junk = 0 
and LocalSuppID = '{0}' 
and AccountID != ''",
                        localSuppID,
                        expressTypeID);
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "20,50,6,3,11,8,13", MyUtility.Convert.GetString(dr["ShipExpenseID"]), columndecimals: "0,0,0,0,4");
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    e.EditingControl.Text = item.GetSelectedString();
                }
            };
            this.code.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["ShipExpenseID"]))
                    {
                        // sql參數
                        string expressTypeID = this.comboType.SelectedValue.ToString().ToLower().EqualString("export") ? "1" : "2";
                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>
                        {
                            new SqlParameter("@localsuppid", MyUtility.Convert.GetString(this.CurrentMaintain["LocalSuppID"])),
                            new SqlParameter("@shipexpenseid", MyUtility.Convert.GetString(MyUtility.Convert.GetString(e.FormattedValue))),
                            new SqlParameter("@expressTypeID", expressTypeID),
                        };

                        DataTable expenseData;
                        string sqlCmd = @"
select ID
	, Description
	, LocalSuppID
	, CurrencyID
	, Price
	, BrandID
	, UnitID
	, AccountID 
from ShipExpense WITH (NOLOCK)
where exists (select 1 from SciFMS_AccountNo sa where sa.ID = ShipExpense.AccountID and sa.ExpressTypeID in (@expressTypeID, 3))
and Junk = 0 
and LocalSuppID = @localsuppid 
and ID = @shipexpenseid  
and AccountID != ''";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out expenseData);
                        if (!result || expenseData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("< Code: {0} > not found!!!", e.FormattedValue.ToString()));
                            }

                            dr["ShipExpenseID"] = string.Empty;
                            dr["Description"] = string.Empty;
                            dr["Price"] = 0;
                            dr["Qty"] = 0;
                            dr["CurrencyID"] = string.Empty;
                            dr["Rate"] = 0;
                            dr["Amount"] = 0;
                            dr["UnitID"] = string.Empty;
                            dr["AccountID"] = string.Empty;
                            dr.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            dr["ShipExpenseID"] = MyUtility.Convert.GetString(e.FormattedValue).ToUpper();
                            dr["Description"] = expenseData.Rows[0]["Description"];
                            dr["Price"] = MyUtility.Convert.GetDecimal(expenseData.Rows[0]["Price"]);
                            dr["Qty"] = 1;
                            dr["CurrencyID"] = expenseData.Rows[0]["CurrencyID"];
                            dr["Rate"] = 1;
                            dr["Amount"] = MyUtility.Convert.GetDecimal(expenseData.Rows[0]["Price"]);
                            dr["UnitID"] = expenseData.Rows[0]["UnitID"];
                            dr["AccountID"] = expenseData.Rows[0]["AccountID"];
                            string sqlcmd = $@"select a.Name from SciFMS_AccountNO a  where a.ID = '{dr["AccountID"]}'";
                            dr["Account"] = MyUtility.Convert.GetString(dr["AccountID"]) + "-" + MyUtility.GetValue.Lookup(sqlcmd);
                            dr.EndEdit();
                        }
                    }
                }
            };

            this.qty.CellValidating += (s, e) =>
                {
                    if (this.EditMode)
                    {
                        DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                        if (e.FormattedValue != null)
                        {
                            dr["Qty"] = e.FormattedValue;
                            this.CalculateGridAmount(dr);
                        }
                    }
                };

            this.rate.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (e.FormattedValue != null)
                    {
                        dr["Rate"] = e.FormattedValue;
                        this.CalculateGridAmount(dr);
                    }
                }
            };
            this.price.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (e.FormattedValue != null)
                    {
                        dr["price"] = e.FormattedValue;
                        this.CalculateGridAmount(dr);
                    }
                }
            };
            #endregion
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("ShipExpenseID", header: "Code", width: Widths.AnsiChars(10), settings: this.code).Get(out this.col_code)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Numeric("Qty", header: "Q'ty", width: Widths.AnsiChars(8), decimal_places: 4, maximum: 9999999, settings: this.qty).Get(out this.col_qty)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CurrencyID", header: "Currency", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(9), iseditingreadonly: true, decimal_places: 4, minimum: -9999999999, settings: this.price)
                .Numeric("Rate", header: "Rate", width: Widths.AnsiChars(9), decimal_places: 6, settings: this.rate).Get(out this.col_rate)
                .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(9), decimal_places: 4, iseditingreadonly: true)
                .Text("Remark", header: "WK#/Reamrk", width: Widths.AnsiChars(10)).Get(out this.col_remark)
                .Text("Account", header: "Account Name", width: Widths.AnsiChars(40), iseditingreadonly: true);
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["CDate"] = DateTime.Today;
            this.CurrentMaintain["Handle"] = Env.User.UserID;
            this.CurrentMaintain["VATRate"] = 0;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "IMPORT";
            this.comboType2.DataSource = this.subType_1;
            this.CurrentMaintain["SubType"] = "MATERIAL";
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.numTotal.Value = 0;
            this.gridicon.Append.Enabled = true;
            this.gridicon.Insert.Enabled = true;
            this.gridicon.Remove.Enabled = true;
            this.DetailGridEditing(true);
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            if (!MyUtility.Check.Empty(this.CurrentMaintain["Accountant"]))
            {
                this.dateDate.ReadOnly = true;
                this.comboType.ReadOnly = true;
                this.comboType2.ReadOnly = true;
                this.txtSubconSupplier.TextBox1.ReadOnly = true;
                this.txtpayterm_ftyTerms.TextBox1.ReadOnly = true;
                this.txtInvoice.ReadOnly = true;
                this.numVATRate.ReadOnly = true;
                this.txtBLNo.ReadOnly = true;
                this.txtUserHandle.TextBox1.ReadOnly = true;
                this.gridicon.Append.Enabled = false;
                this.gridicon.Insert.Enabled = false;
                this.gridicon.Remove.Enabled = false;
                this.DetailGridEditing(false);
            }
            else
            {
                this.gridicon.Append.Enabled = true;
                this.gridicon.Insert.Enabled = true;
                this.gridicon.Remove.Enabled = true;
                this.DetailGridEditing(true);
            }

            this.txtSubconSupplier.TextBox1.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            this.CurrentMaintain["ApvDate"] = DBNull.Value;
            this.CurrentMaintain["VoucherID"] = string.Empty;
            this.CurrentMaintain["VoucherDate"] = DBNull.Value;
            this.CurrentMaintain["Accountant"] = string.Empty;
            this.disExVoucherID.Text = string.Empty;
            this.CurrentMaintain["Handle"] = Env.User.UserID;
            this.CurrentMaintain["ID"] = string.Empty;
            this.CurrentMaintain["VoucherID"] = string.Empty;
            this.comboType2.SelectedValue = this.CurrentMaintain["SubType"].ToString();
        }

        // protected override void ClickUndo()
        // {
        //    ChangeCombo2DataSource();
        //    base.ClickUndo();
        // }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            string sqlcmd = string.Empty;

            this.CurrentMaintain["SubType"] = this.comboType2.SelectedValue;
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["CDate"]))
            {
                this.dateDate.Focus();
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Type"]))
            {
                this.comboType.Focus();
                MyUtility.Msg.WarningBox("Type can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["SubType"]))
            {
                this.comboType2.Focus();
                MyUtility.Msg.WarningBox("SubType can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["LocalSuppID"]))
            {
                this.txtSubconSupplier.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Supplier can't empty!!");
                return false;
            }
            else
            {
                // 如果為SCI工廠 Sis. Fty A/P# 就不可為空
                sqlcmd = string.Format(
                    @"
select IsFactory = iif(IsFactory = 1, 'True', 'False')
from LocalSupp
where ID = '{0}'
and Junk = 0",
                    this.CurrentMaintain["LocalSuppID"]);

                bool chkIsFactory = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup(sqlcmd));
                if (chkIsFactory && MyUtility.Check.Empty(this.CurrentMaintain["SisFtyAPID"]))
                {
                    this.txtSisFtyAPID.Focus();
                    MyUtility.Msg.WarningBox("Sis. Fty A/P# can't empty!!");
                    return false;
                }
            }

            if (!this.CheckNoExportChargesGMT(this.CurrentMaintain["BLNo"].ToString()))
            {
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["PayTermID"]))
            {
                this.txtpayterm_ftyTerms.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Terms can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["BLNo"]) && this.checkIsFreightForwarder.Checked)
            {
                this.txtBLNo.Focus();
                MyUtility.Msg.WarningBox("B/L No. can't empty!!");
                return false;
            }

            if ((this.checkIsFreightForwarder.Checked && this.CurrentMaintain["Reason"].ToString() != "AP007") || this.IsCustoms)
            {
                if (!this.IsExistsBLNo)
                {
                    string errMsg = this.IsCustoms ?
                        $"Since Supplier：<{this.CurrentMaintain["LocalSuppID"]}> is Customs, the B/L#<{this.CurrentMaintain["BLNo"]}> should be filled in with the existing B/L No.!!" :
                        $"Since [Is Freight Forwarder] is checked, the B/L#<{this.CurrentMaintain["BLNo"]}> should be filled in with the existing B/L No.!!";
                    MyUtility.Msg.WarningBox(errMsg);
                    return false;
                }
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Handle"]))
            {
                this.txtUserHandle.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Handle can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CurrencyID"]))
            {
                this.txtcurrency.Focus();
                MyUtility.Msg.WarningBox("Currency cannot be empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["InvNo"]))
            {
                this.txtInvoice.Focus();
                MyUtility.Msg.WarningBox("Invoice# can't be empty!!");
                return false;
            }

            #endregion

            #region 檢查LocalSupp_Bank
            DualResult resultCheckLocalSupp_BankStatus = Prgs.CheckLocalSupp_BankStatus(this.CurrentMaintain["LocalSuppID"].ToString(), Prgs.CallFormAction.Save);
            if (!resultCheckLocalSupp_BankStatus)
            {
                return false;
            }
            #endregion

            // Supplier與B/L No 如果重複才需要填寫原因, Reason 不可為空，須排除自己這張AP
            if (!MyUtility.Check.Empty(this.txtBLNo.Text))
            {
                string strSQLcmd = $@"select 1 from ShippingAP where ID <> '{this.CurrentMaintain["ID"]}' AND BLNo='{this.txtBLNo.Text}' AND LocalSuppID='{this.CurrentMaintain["LocalSuppID"].ToString()}' ";
                if (MyUtility.Check.Seek(strSQLcmd) && MyUtility.Check.Empty(this.CurrentMaintain["Reason"]))
                {
                    MyUtility.Msg.WarningBox(@"<Reason> cannot be empty becusae this <B/L No.> is already exists in other same supplier AP.");
                    this.txtReason.Focus();
                    return false;
                }
            }

            // InvNo + B/L No不可以重複建立
            if (!MyUtility.Check.Empty(this.CurrentMaintain["InvNo"]))
            {
                DataRow dr;
                if (MyUtility.Check.Seek(string.Format("select ID from ShippingAp WITH (NOLOCK) where InvNo = '{0}' and BLNo = '{1}' and ID != '{2}'", MyUtility.Convert.GetString(this.CurrentMaintain["InvNo"]), MyUtility.Convert.GetString(this.CurrentMaintain["BLNo"]), MyUtility.Convert.GetString(this.CurrentMaintain["ID"])), out dr))
                {
                    MyUtility.Msg.WarningBox("< Invoice# > and < B/L No. > duplicate with No." + MyUtility.Convert.GetString(dr["ID"]));
                    return false;
                }
            }

            // ISP20210501
            if (this.CurrentMaintain["Type"].EqualString("EXPORT") &&
                this.checkIsFreightForwarder.Checked)
            {
                string strSQLcmd = string.Format(
                    @"
select 1
from GMTBooking gb
where 1 = 1
and(BLno = '{0}' or BL2no = '{0}')
and gb.ShipModeID in ('A/P', 'E/P')",
                    this.CurrentMaintain["BLNo"]);
                if (MyUtility.Check.Seek(strSQLcmd) && MyUtility.Check.Empty(this.CurrentMaintain["APPExchageRate"]))
                {
                    MyUtility.Msg.WarningBox("Please encode <APP Exchange Rate> if <Is Freight FWD> is selected and the <Invoice No.> shipmode is A/P or E/P which is belong to this <B/L No.>");
                    this.numericBox1.Focus();
                    return false;
                }
            }

            // 清空表身Grid資料
            int countRec = 0; // 計算表身筆數
            decimal detailAmt = 0.0m; // 表身Amount加總值
            bool currencyEmpt = false;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["ShipExpenseID"]))
                {
                    dr.Delete();
                    continue;
                }

                if (MyUtility.Check.Empty(dr["CurrencyID"]))
                {
                    currencyEmpt = true;
                }

                detailAmt = detailAmt + MyUtility.Convert.GetDecimal(dr["Amount"]);
                countRec = countRec + 1;
            }

            // 表身資料為空
            if (countRec == 0)
            {
                MyUtility.Msg.WarningBox("< A/P# Detail> can't empty!!");
                return false;
            }

            // 檢查表頭與表身科目是否吻合
            string accountNO = string.Empty;
            foreach (DataRow dr in this.DetailDatas)
            {
                sqlcmd = string.Format(
                    @"select iif(e.ExpressType = 1, null, se.AccountID) AccountID
                        from ShipExpense se
                        outer apply (
	                        select dbo.GetAccountNoExpressType(se.AccountID, '{1}') ExpressType
                        ) e
                        where se.id = '{0}' ",
                    dr["ShipExpenseID"],
                    this.CurrentMaintain["Type"]);
                string result = MyUtility.GetValue.Lookup(sqlcmd);
                if (!MyUtility.Check.Empty(result))
                {
                    accountNO = MyUtility.Check.Empty(accountNO) ? result : accountNO + "," + result;
                }
            }

            if (!MyUtility.Check.Empty(accountNO))
            {
                MyUtility.Msg.WarningBox(string.Format(
                    @"Account Payment Type is {0}, but Account No ({1}) is not {0} Item," + Environment.NewLine + "Please check and correct the inconsistencies before proceeding to the next step.",
                    this.CurrentMaintain["Type"],
                    accountNO));
                return false;
            }

            // 表身Curreny為空
            if (currencyEmpt)
            {
                MyUtility.Msg.WarningBox("Currency cannot be empty!!");
                return false;
            }

            this.haveEditShareFee = false;

            // 當Type & SubType其中一個值異動過，且已有Share Expense資料，就要問使用者是否確定要存檔，若是，就要將Share Expense的資料給清掉
            if (!this.IsDetailInserting)
            {
                DataRow seekRow;
                if (MyUtility.Check.Seek(string.Format("select Type,SubType from ShippingAP WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])), out seekRow))
                {
                    if (MyUtility.Convert.GetString(this.CurrentMaintain["Type"]) != MyUtility.Convert.GetString(seekRow["Type"]))
                    {
                        DialogResult buttonResult = MyUtility.Msg.WarningBox(string.Format("Already have expense data. Are you sure want to change the type from '{0}' to '{1}'", MyUtility.Convert.GetString(seekRow["Type"]), MyUtility.Convert.GetString(this.CurrentMaintain["Type"])), "Warning", MessageBoxButtons.YesNo);
                        if (buttonResult == DialogResult.No)
                        {
                            return false;
                        }
                        else
                        {
                            this.haveEditShareFee = true;
                        }
                    }

                    if (!this.haveEditShareFee && MyUtility.Convert.GetString(this.CurrentMaintain["SubType"]) != MyUtility.Convert.GetString(seekRow["SubType"]))
                    {
                        DialogResult buttonResult = MyUtility.Msg.WarningBox(string.Format("Already have expense data. Are you sure want to change the type from '{0}' to '{1}'", MyUtility.Convert.GetString(seekRow["SubType"]), MyUtility.Convert.GetString(this.CurrentMaintain["SubType"])), "Warning", MessageBoxButtons.YesNo);
                        if (buttonResult == DialogResult.No)
                        {
                            return false;
                        }
                        else
                        {
                            this.haveEditShareFee = true;
                        }
                    }
                }
                else
                {
                    MyUtility.Msg.WarningBox("Shipping AP Data not found!");
                    return false;
                }
            }

            // 將表身Amount加總值回寫回表頭
            int exact = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup("Exact", MyUtility.Convert.GetString(this.CurrentMaintain["CurrencyID"]), "Currency", "ID"));
            this.CurrentMaintain["Amount"] = MyUtility.Math.Round(detailAmt, exact);
            this.CurrentMaintain["VAT"] = MyUtility.Convert.GetDecimal(this.CurrentMaintain["VATRate"]) > 0 ? MyUtility.Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Math.Round(detailAmt, exact)) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["VATRate"]) / 100, exact) : 0;

            // Get ID
            if (this.IsDetailInserting)
            {
                string newID = MyUtility.GetValue.GetID(Env.User.Keyword + "SA", "ShippingAP", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(newID))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = newID;
            }

            DataTable tmpdt;
            sqlcmd = $@"
select [IsAPP] = dbo.GetAccountNoExpressType(sd.AccountID, 'IsAPP')
    ,[vat] = dbo.GetAccountNoExpressType(sd.AccountID, 'vat')
    ,[AdvancePaymentTPE] = dbo.GetAccountNoExpressType(sd.AccountID, 'AdvancePaymentTPE')
    ,[SisFty] = dbo.GetAccountNoExpressType(sd.AccountID, 'SisFty')
from #tmp sd
";
            var dtldt = this.DetailDatas.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted);
            if (dtldt.Count() > 0)
            {
                DataTable dDt = this.DetailDatas.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).CopyToDataTable();
                DualResult result = MyUtility.Tool.ProcessWithDatatable(dDt, string.Empty, sqlcmd, out tmpdt);
                if (!result)
                {
                    this.ShowErr(result);
                    return false;
                }

                // 有標記IsAPP就是APP
                var hasisapp = tmpdt.AsEnumerable().Where(w => MyUtility.Convert.GetBool(w["IsAPP"]));
                if (hasisapp.Count() > 0)
                {
                    // 沒標記IsAPP,不是IsShippingVAT(稅),不是AdvancePaymentTPE(代墊台北), 則不是IsAPP
                    var notapp = tmpdt.AsEnumerable().
                        Where(w => !MyUtility.Convert.GetBool(w["IsAPP"]) &&
                                    !MyUtility.Convert.GetBool(w["vat"]) &&
                                    !MyUtility.Convert.GetBool(w["AdvancePaymentTPE"]) &&
                                    !MyUtility.Convert.GetBool(w["SisFty"]));

                    if (notapp.Count() > 0)
                    {
                        MyUtility.Msg.WarningBox(@"Air-Prepaid Account Payment cannot inculde non Air-Prepaid Item Code.

If the application is for Air - Prepaid Invoice, please ensure that all item codes are linked to the correct Account Name -
6105 - Air prepaid apparel - FTY and / or 5912 - Disburz for SCI Adm Expz, Thank You.");
                        return false;
                    }
                }
            }

            this.IncludeFoundryRefresh(this.txtBLNo.Text);

            // 系統單純提示訊息
            if (this.chkIncludeFoundry.Checked)
            {
                MyUtility.Msg.WarningBox(
                    @"This Account Payment includes both general and foundry's SP#,
- Please be sure to separate the amount of foundry's SP# Payable at account code-3113
- Please go to foundry's PMS to create an account payment, the Supplier must be the ""Shipper""");
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePre()
        {
            DualResult result;
            string oldBLNo = MyUtility.GetValue.Lookup($"select BLNo from ShippingAP with (nolock) where ID = '{this.CurrentMaintain["ID"]}'");

            if (this.haveEditShareFee ||
                oldBLNo != this.CurrentMaintain["BLNo"].ToString())
            {
                result = DBProxy.Current.Execute(null, string.Format("delete from ShareExpense where ShippingAPID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Delete ShareExpense fail\r\n" + result.ToString());
                    return failResult;
                }
            }

            result = this.UpdateAirPP();

            if (!result)
            {
                return result;
            }

            return base.ClickSavePre();
        }

        private DualResult InitialShareExpense(DataRow drCurrentMaintain, bool checkIsFreightForwarder)
        {
            DualResult result;
            if (((checkIsFreightForwarder && this.IsExistsBLNo) || this.IsCustoms) && MyUtility.Check.Empty(this.txtBLNo.Text) == false)
            {
                string strSqlCmd;
                if (drCurrentMaintain["Type"].EqualString("EXPORT") && drCurrentMaintain["SubType"].EqualString("GARMENT"))
                {
                    #region get A2B PackingInfo
                    string sqlGetA2BGMT = $@"
select  gd.PLFromRgCode
        ,gd.PackingListID
        ,[BLNo] = iif(g.BLNo is null or g.BLNo = '', g.BL2No, g.BLNo)
        ,[WKNo] = ''
        ,[InvNo] = g.id
        ,[Type] = '{drCurrentMaintain["Type"]}'
        ,[CurrencyID] = '{drCurrentMaintain["CurrencyID"]}'
        ,[ShipModeID] = g.ShipModeID
        ,[FtyWK] = 0
        ,[AccountID] = AccountID.val
        ,[Junk] = 0
        ,[ShippingAPID] = '{drCurrentMaintain["ID"]}'
from GMTBooking g WITH (NOLOCK)
outer apply(select top 1 [val] = sd.AccountID 
            from ShippingAP_Detail sd WITH(NOLOCK)
                where sd.ID = '{drCurrentMaintain["ID"]}' and sd.AccountID != ''
                and not (dbo.GetAccountNoExpressType(sd.AccountID,'Vat') = 1 
		            or dbo.GetAccountNoExpressType(sd.AccountID,'SisFty') = 1)) AccountID
inner join GMTBooking_Detail gd with (nolock) on g.ID = gd.ID
where (g.BLNo='{drCurrentMaintain["BLNO"]}' or g.BL2No='{drCurrentMaintain["BLNO"]}') and AccountID.val is not null
";
                    DataTable dtA2BResult = new DataTable();
                    DataTable dtA2BGMT;

                    result = DBProxy.Current.Select(null, sqlGetA2BGMT, out dtA2BGMT);
                    if (!result)
                    {
                        return result;
                    }

                    string sqlGetA2BPack = @"
alter table #tmp alter column InvNo varchar(25)
alter table #tmp alter column PackingListID varchar(13)

select  g.ShippingAPID
        ,g.BLNo
        ,g.WKNo
        ,g.InvNo
        ,g.Type
        ,[GW] = sum(pd.GW)
        ,[CBM] = sum(l.CBM)
        ,g.CurrencyID
        ,g.ShipModeID
        ,g.FtyWK
        ,g.AccountID
        ,g.Junk
        ,o.FactoryID
from #tmp g
inner join PackingList p with (nolock) on p.INVNo = g.InvNo and p.ID = g.PackingListID and p.Type != 'L'
inner join PackingList_Detail pd with (nolock) on  pd.ID = p.ID and pd.CTNQty > 0
inner join Orders o with (nolock) on o.ID = pd.OrderID
inner join LocalItem l with (nolock) on l.Refno = pd.Refno
group by g.ShippingAPID
        ,g.BLNo
        ,g.WKNo
        ,g.InvNo
        ,g.Type
        ,g.CurrencyID
        ,g.ShipModeID
        ,g.FtyWK
        ,g.AccountID
        ,g.Junk
        ,o.FactoryID
";

                    foreach (var groupItem in dtA2BGMT.AsEnumerable().GroupBy(s => s["PLFromRgCode"].ToString()))
                    {
                        DataBySql dataBySql = new DataBySql()
                        {
                            SqlString = sqlGetA2BPack,
                            TmpTable = JsonConvert.SerializeObject(groupItem.CopyToDataTable()),
                        };

                        DataTable dtA2BPack;
                        result = PackingA2BWebAPI.GetDataBySql(groupItem.Key, dataBySql, out dtA2BPack);

                        if (!result)
                        {
                            return result;
                        }

                        dtA2BPack.MergeTo(ref dtA2BResult);
                    }
                    #endregion

                    string sqlUnionA2B = string.Empty;
                    if (dtA2BResult.Rows.Count > 0)
                    {
                        sqlUnionA2B = @"
union all
select  ShippingAPID
        ,BLNo
        ,WKNo
        ,InvNo
        ,Type
        ,GW
        ,CBM
        ,CurrencyID
        ,ShipModeID
        ,FtyWK
        ,AccountID
        ,Junk
        ,FactoryID
from #tmp
";
                    }

                    strSqlCmd = $@"
merge ShareExpense t
using (
select  ShippingAPID
        ,BLNo
        ,WKNo
        ,InvNo
        ,Type
        ,[GW] = sum(GW)
        ,[CBM] = sum(CBM)
        ,CurrencyID
        ,ShipModeID
        ,FtyWK
        ,AccountID
        ,Junk
        ,FactoryID
from (
select 
    [ShippingAPID] = '{drCurrentMaintain["ID"]}'
    ,[BLNo] = iif(g.BLNo is null or g.BLNo = '', g.BL2No, g.BLNo)
    ,[WKNo] = ''
    ,[InvNo] = g.id
    ,[Type] = '{drCurrentMaintain["Type"]}'
    ,[GW] = sum(pd.GW)
    ,[CBM] = sum(l.CBM)
    ,[CurrencyID] = '{drCurrentMaintain["CurrencyID"]}'
    ,[ShipModeID] = g.ShipModeID
    ,[FtyWK] = 0
    ,[AccountID] = AccountID.val
    ,[Junk] = 0
    ,o.FactoryID
from GMTBooking g WITH (NOLOCK)
outer apply(select top 1 [val] = sd.AccountID 
            from ShippingAP_Detail sd WITH(NOLOCK)
                where sd.ID = '{drCurrentMaintain["ID"]}' and sd.AccountID != ''
                and not (dbo.GetAccountNoExpressType(sd.AccountID,'Vat') = 1 
		            or dbo.GetAccountNoExpressType(sd.AccountID,'SisFty') = 1)) AccountID
inner join PackingList p with (nolock) on p.INVNo = g.ID and p.Type != 'L'
inner join PackingList_Detail pd with (nolock) on  pd.ID = p.ID and pd.CTNQty > 0
inner join Orders o with (nolock) on o.ID = pd.OrderID
inner join LocalItem l with (nolock) on l.Refno = pd.Refno
where (g.BLNo='{drCurrentMaintain["BLNO"]}' or g.BL2No='{drCurrentMaintain["BLNO"]}')  and AccountID.val is not null
group by g.BLNo, g.BL2No, g.id, g.ShipModeID, AccountID.val, o.FactoryID
{sqlUnionA2B}
) a group by ShippingAPID
            ,BLNo
            ,WKNo
            ,InvNo
            ,Type
            ,CurrencyID
            ,ShipModeID
            ,FtyWK
            ,AccountID
            ,Junk
            ,FactoryID
) as s 
on	t.ShippingAPID = s.ShippingAPID 
	and t.WKNO = s.WKNO	and t.InvNo = s.InvNo and t.FactoryID = s.FactoryID
when matched then
	update set  t.junk = 0,
                t.GW = s.GW,
                t.CBM = s.CBM
when not matched by target then 
	insert (ShippingAPID, BLNo, WKNo, InvNo, Type, GW, CBM, CurrencyID, ShipModeID, FtyWK, AccountID, Junk, FactoryID)
	values (s.ShippingAPID, s.BLNo, s.WKNo, s.InvNo, s.Type, s.GW, s.CBM, s.CurrencyID, s.ShipModeID, s.FtyWK, s.AccountID, s.Junk, s.FactoryID);";

                    if (MyUtility.Check.Empty(sqlUnionA2B))
                    {
                        result = DBProxy.Current.Execute(string.Empty, strSqlCmd);
                    }
                    else
                    {
                        result = MyUtility.Tool.ProcessWithDatatable(dtA2BResult, string.Empty, strSqlCmd, out DataTable dtEmpty);
                    }
                }
                else
                {
                    strSqlCmd = $@"
merge ShareExpense t
using (
    select *
    from (
	    select 
		    [ShippingAPID] = '{drCurrentMaintain["ID"]}'
		    ,[BLNo] = e.BLNo
		    ,[WKNo] = e.id
		    ,[InvNo] = ''
		    ,[Type] = '{drCurrentMaintain["Type"]}'
		    ,[GW] = e.WeightKg 
		    ,[CBM] = e.CBM 
		    ,[CurrencyID] = '{drCurrentMaintain["CurrencyID"]}'
		    ,[ShipModeID] = e.ShipModeID
		    ,[FtyWK] = 0
		    ,[AccountID] = AccountID.val
		    ,[Junk] = 0
            ,[FactoryID] = ''
	    from Export e WITH (NOLOCK)
        outer apply (select top 1 [val] = sd.AccountID 
                        from ShippingAP_Detail sd WITH(NOLOCK)
			            where sd.ID = '{drCurrentMaintain["ID"]}' and sd.AccountID != ''
			            and not (dbo.GetAccountNoExpressType(sd.AccountID,'Vat') = 1 
				            or dbo.GetAccountNoExpressType(sd.AccountID,'SisFty') = 1)) AccountID
	    where e.BLNo = '{drCurrentMaintain["BLNO"]}'
	    union 
	    select [ShippingAPID] = '{drCurrentMaintain["ID"]}'
		        ,[BLNo] = f.BLNo
		        ,[WKNo] = f.id
		        ,[InvNo] = ''
		        ,[Type] = '{drCurrentMaintain["Type"]}'
		        ,[GW] = f.WeightKg 
		        ,[CBM] = f.CBM 
		        ,[CurrencyID] = '{drCurrentMaintain["CurrencyID"]}'
		        ,[ShipModeID] = f.ShipModeID
		        ,[FtyWK] = 0
		        ,[AccountID] = AccountID.val
		        ,[Junk] = 0
                ,[FactoryID] = ''
	    from FtyExport f WITH (NOLOCK) 
        outer apply (select top 1 [val] = sd.AccountID
                     from ShippingAP_Detail sd WITH(NOLOCK)
			         where sd.ID = '{drCurrentMaintain["ID"]}' and sd.AccountID != ''
			         and not (dbo.GetAccountNoExpressType(sd.AccountID,'Vat') = 1 
				         or dbo.GetAccountNoExpressType(sd.AccountID,'SisFty') = 1)) AccountID
	    where   BLNo = '{drCurrentMaintain["BLNO"]}'
	            and Type <> 3
        union
        select  [ShippingAPID] = '{drCurrentMaintain["ID"]}'
                ,[BLNo] = iif(BLNo is null or BLNo='', BL2No,BLNo)
                ,[WKNo] = ''
                ,[InvNo] = id
                ,[Type] = '{drCurrentMaintain["Type"]}'
                ,[GW] = TotalGW
                ,[CBM] = TotalCBM
                ,[CurrencyID] = '{drCurrentMaintain["CurrencyID"]}'
                ,[ShipModeID] = ShipModeID
                ,[FtyWK] = 0
                ,[AccountID] = (
        	        select top 1 sd.AccountID from ShippingAP_Detail sd WITH(NOLOCK)
                    where sd.ID = '{drCurrentMaintain["ID"]}' and sd.AccountID != ''
                    and not (dbo.GetAccountNoExpressType(sd.AccountID,'Vat') = 1 
        	    	    or dbo.GetAccountNoExpressType(sd.AccountID,'SisFty') = 1))
                ,[Junk] = 0
                ,[FactoryID] = ''
        from GMTBooking g WITH (NOLOCK) 
        where BLNo='{drCurrentMaintain["BLNO"]}' or BL2No='{drCurrentMaintain["BLNO"]}'

        union
        -- ISP20221136 add Shipping.P16 data
        select [ShippingAPID] = '{drCurrentMaintain["ID"]}'
                ,[BLNo] = te.BLNo
                ,[WKNo] = te.id
                ,[InvNo] = ''
                ,[Type] = '{drCurrentMaintain["Type"]}'
                ,[GW] = te.WeightKg 
                ,[CBM] = t2.CBM 
                ,[CurrencyID] = '{drCurrentMaintain["CurrencyID"]}'
                ,[ShipModeID] = te.ShipModeID
                ,[FtyWK] = 1
                ,[AccountID] = AccountID.val
                ,[Junk] = 0
                ,[FactoryID] = ''
        from TransferExport te WITH (NOLOCK) 
        outer apply(
	        select WeightKg = sum(s.WeightKg) , Cbm = sum(s.CBM)
	        from TransferExport_Detail s
	        where s.ID = te.ID
        )t2
        outer apply (
	        select top 1 [val] = sd.AccountID
            from ShippingAP_Detail sd WITH(NOLOCK)
	        where sd.ID = '{drCurrentMaintain["ID"]}' and sd.AccountID != ''
	        and not (dbo.GetAccountNoExpressType(sd.AccountID,'Vat') = 1 
		        or dbo.GetAccountNoExpressType(sd.AccountID,'SisFty') = 1)
        ) AccountID
        where   BLNo = '{drCurrentMaintain["BLNO"]}'
    ) a where AccountID is not null
) as s 
on	t.ShippingAPID = s.ShippingAPID 
	and t.WKNO = s.WKNO	and t.InvNo = s.InvNo and t.FactoryID = s.FactoryID
when matched then
	update set  t.junk = 0,
                t.GW = s.GW,
                t.CBM = s.CBM
when not matched by target then 
	insert (ShippingAPID, BLNo, WKNo, InvNo, Type, GW, CBM, CurrencyID, ShipModeID, FtyWK, AccountID, Junk, FactoryID)
	values (s.ShippingAPID, s.BLNo, s.WKNo, s.InvNo, s.Type, s.GW, s.CBM, s.CurrencyID, s.ShipModeID, s.FtyWK, s.AccountID, s.Junk, s.FactoryID);";

                    result = DBProxy.Current.Execute(string.Empty, strSqlCmd);
                }

                if (!result)
                {
                    return new DualResult(false, "Append Data failed!");
                }
            }

            return new DualResult(true);
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            int isFreightForwarder = this.checkIsFreightForwarder.Checked ? 1 : 0;
            DualResult result;

            result = this.InitialShareExpense(this.CurrentMaintain, this.checkIsFreightForwarder.Checked);

            if (!result)
            {
                return result;
            }

            result = Prgs.CalculateShareExpense(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), isFreightForwarder);

            if (!result)
            {
                return result;
            }

            string sqlInvNo = $@"select distinct InvNo from ShareExpense where  ShippingAPID = '{this.CurrentMaintain["ID"]}'";
            if (!(result = DBProxy.Current.Select(string.Empty, sqlInvNo, out DataTable dtInvNo)))
            {
                return new DualResult(false, result.ToString());
            }

            // 執行A2B跨廠區資料 & 寫入跨廠區資料到ShareExpense_APP
            foreach (DataRow item in dtInvNo.Rows)
            {
                string invNos = item["InvNo"].ToString();
                List<string> listPLFromRgCode = PackingA2BWebAPI.GetPLFromRgCodeByInvNo(invNos);
                foreach (string plFromRgCode in listPLFromRgCode)
                {
                    // 跨Server取得ShareExpense_APP所需PackingList資料
                    DataTable dtA2BPacking;
                    result = Prgs.DataTable_Packing(plFromRgCode, invNos, out dtA2BPacking);

                    if (!result)
                    {
                        return result;
                    }

                    if (dtA2BPacking.Rows.Count > 0)
                    {
                        result = Prgs.CalculateShareExpense_APP(this.CurrentMaintain["ID"].ToString(), Env.User.UserID, dtA2BPacking, plFromRgCode);

                        if (!result)
                        {
                            return result;
                        }
                    }
                }
            }

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain["Accountant"]))
            {
                MyUtility.Msg.WarningBox("This record is < Approved >, can't be deleted!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickDeletePre()
        {
            string sqlCmd = string.Format("update ShareExpense set Junk = 1 where ShippingAPID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Delete ShareExpense false.\r\n" + result.ToString());
                return failResult;
            }

            sqlCmd = $@"
update ShareExpense_APP
set Junk = 1
    , EditDate = getdate()
    , EditName = '{Env.User.UserID}'
where ShippingAPID = '{MyUtility.Convert.GetString(this.CurrentMaintain["ID"])}'";

            result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Delete ShareExpense_APP false.\r\n" + result.ToString());
                return failResult;
            }

            result = this.UpdateAirPP();

            if (!result)
            {
                DualResult failResult = new DualResult(false, "Delete ShareExpense_APP false.\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        private string Chk_null(string str)
        {
            if (str == string.Empty)
            {
                return " ";
            }
            else
            {
                return str;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            DualResult result;
            IReportResource reportresource;
            ReportDefinition rd = new ReportDefinition();
            if (!(result = ReportResources.ByEmbeddedResource(Assembly.GetAssembly(this.GetType()), this.GetType(), "P08_Print.rdlc", out reportresource)))
            {
                MyUtility.Msg.ErrorBox(result.ToString());
            }
            else
            {
                DataTable factoryData, localSpuuData, report_ShippingAPDetail;
                string sqlCmd = string.Format(
                    "select NameEN, AddressEN,Tel from factory WITH (NOLOCK) where ID = '{0}'",
                    MyUtility.Convert.GetString(this.CurrentMaintain["FactoryID"]));
                result = DBProxy.Current.Select(null, sqlCmd, out factoryData);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Prepare data fail!!\r\n" + result.ToString());
                    return false;
                }

                sqlCmd = string.Format(
                    @"
SELECT    ls.Name
		, ls.Address
		, ls.Tel
		, [AccountNo]=LocalSupBank.AccountNo
		, [AccountName]=LocalSupBank.AccountName
		, [BankName]=LocalSupBank.BankName
		, [CountryID]=LocalSupBank.CountryID
		, [City]=LocalSupBank.City
		, [SwiftCode]=LocalSupBank.SwiftCode
FROM LocalSupp ls
OUTER APPLY(
	SELECT   [AccountNo]= IIF(lb.ByCheck=1,'',lbd.Accountno )
			, [AccountName]=IIF(lb.ByCheck=1,'',lbd.AccountName )
			, [BankName]=IIF(lb.ByCheck=1,'',lbd.BankName )
			, [CountryID]=IIF(lb.ByCheck=1,'',lbd.CountryID)
			, [City]=IIF(lb.ByCheck=1,'',lbd.City)
			, [SwiftCode]=IIF(lb.ByCheck=1,'',lbd.SwiftCode )
	FROM LocalSupp_Bank lb
	LEFT JOIN LocalSupp_Bank_Detail lbd ON lb.ID=lbd.ID AND lb.PKey=lbd.Pkey
	WHERE lb.ID=ls.ID
		AND lb.ApproveDate = (SElECT MAX(ApproveDate) FROM  LocalSupp_Bank WHERE Status='Confirmed' AND ID=ls.ID)
		AND lbd.IsDefault=1
)LocalSupBank
WHERE ls.ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["LocalSuppID"]));
                result = DBProxy.Current.Select(null, sqlCmd, out localSpuuData);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Prepare data fail!!\r\n" + result.ToString());
                    return false;
                }

                sqlCmd = string.Format(
                    @"select sd.ShipExpenseID,isnull(se.Description,' ') as Description,sd.CurrencyID,sd.Price,sd.Qty,sd.Rate,sd.Amount,se.UnitID 
from ShippingAP_Detail sd WITH (NOLOCK) 
left join ShipExpense se WITH (NOLOCK) on sd.ShipExpenseID = se.ID
where sd.ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
                result = DBProxy.Current.Select(null, sqlCmd, out report_ShippingAPDetail);

                rd.ReportResource = reportresource;
                rd.ReportDataSources.Add(new KeyValuePair<string, object>("Report_ShippingAPDetail", report_ShippingAPDetail));
                if (factoryData.Rows.Count == 0)
                {
                    rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("company", " "));
                    rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("compAddress", " "));
                    rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("compTel", " "));
                }
                else
                {
                    rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("company", MyUtility.Convert.GetString(factoryData.Rows[0]["NameEN"])));
                    rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("compAddress", MyUtility.Convert.GetString(factoryData.Rows[0]["AddressEN"])));
                    rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("compTel", MyUtility.Convert.GetString(factoryData.Rows[0]["Tel"])));
                }

                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("apId", this.Chk_null(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("barcodeAPId", this.Chk_null("*" + MyUtility.Convert.GetString(this.CurrentMaintain["ID"]) + "*")));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("apDate", this.Chk_null(MyUtility.Check.Empty(this.CurrentMaintain["CDate"]) ? "null" : Convert.ToDateTime(this.CurrentMaintain["CDate"]).ToString())));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("supplier", this.Chk_null(MyUtility.Convert.GetString(this.CurrentMaintain["LocalSuppID"]) + " " + MyUtility.Convert.GetString(localSpuuData.Rows[0]["Name"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("address", this.Chk_null(MyUtility.Convert.GetString(localSpuuData.Rows[0]["Address"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("tel", this.Chk_null(MyUtility.Convert.GetString(localSpuuData.Rows[0]["Tel"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("blNo", this.Chk_null(MyUtility.Convert.GetString(this.CurrentMaintain["BLNo"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("payTerm", this.Chk_null(MyUtility.GetValue.Lookup(string.Format("select Name from PayTerm WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["PayTermID"]))))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("invNo", this.Chk_null(MyUtility.Convert.GetString(this.CurrentMaintain["InvNo"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("remark", this.Chk_null(MyUtility.Convert.GetString(this.CurrentMaintain["Remark"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("acNo", this.Chk_null(MyUtility.Convert.GetString(localSpuuData.Rows[0]["AccountNo"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("acName", this.Chk_null(MyUtility.Convert.GetString(localSpuuData.Rows[0]["AccountName"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("bankName", this.Chk_null(MyUtility.Convert.GetString(localSpuuData.Rows[0]["BankName"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("country", this.Chk_null(MyUtility.Convert.GetString(localSpuuData.Rows[0]["CountryID"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("city", this.Chk_null(MyUtility.Convert.GetString(localSpuuData.Rows[0]["City"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("swiftCode", this.Chk_null(MyUtility.Convert.GetString(localSpuuData.Rows[0]["SWIFTCode"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("currency", this.Chk_null(MyUtility.Convert.GetString(this.CurrentMaintain["CurrencyID"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("amount", this.Chk_null(MyUtility.Convert.GetString(this.CurrentMaintain["Amount"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("vatRate", this.Chk_null(MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(this.CurrentMaintain["VATRate"]) / 100))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("vat", this.Chk_null(MyUtility.Convert.GetString(this.CurrentMaintain["VAT"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("total", this.Chk_null(MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(this.CurrentMaintain["Amount"]) + MyUtility.Convert.GetDecimal(this.CurrentMaintain["VAT"])))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("handle", this.Chk_null(MyUtility.GetValue.Lookup(string.Format("select Name from Pass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["Handle"]))))));

                using (var frm = new Win.Subs.ReportView(rd))
                {
                    frm.ShowDialog(this);
                }
            }

            return base.ClickPrint();
        }

        // 計算表身Grid欄位的Amount值
        private void CalculateGridAmount(DataRow dr)
        {
            decimal qty = MyUtility.Convert.GetDecimal(dr["Qty"]);
            decimal rate = MyUtility.Convert.GetDecimal(dr["Rate"]);
            dr["Amount"] = MyUtility.Math.Round(MyUtility.Math.Round(qty * MyUtility.Convert.GetDecimal(dr["Price"]), 2) * rate, 2);
            dr.EndEdit();
        }

        // 控制表身Grid欄位是否可被編輯
        private void DetailGridEditing(bool isEditing)
        {
            if (isEditing)
            {
                this.col_code.IsEditingReadOnly = false;
                this.col_qty.IsEditingReadOnly = false;
                this.col_rate.IsEditingReadOnly = false;
                this.col_remark.IsEditingReadOnly = false;

                for (int i = 0; i < this.detailgrid.ColumnCount; i++)
                {
                    if (i == 0 || i == 2 || i == 6 || i == 8)
                    {
                        this.detailgrid.Columns[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
            else
            {
                this.col_code.IsEditingReadOnly = true;
                this.col_qty.IsEditingReadOnly = true;
                this.col_rate.IsEditingReadOnly = true;
                this.col_remark.IsEditingReadOnly = true;
                for (int i = 0; i < this.detailgrid.ColumnCount; i++)
                {
                    this.detailgrid.Columns[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        // Share Expense
        private void BtnShareExpense_Click(object sender, EventArgs e)
        {
            bool isReasonAP007 = this.CurrentMaintain["Reason"].EqualString("AP007");
            P08_ShareExpense callNextForm = new P08_ShareExpense(this.CurrentMaintain, this.checkIsFreightForwarder.Checked, isReasonAP007, this.InitialShareExpense);
            callNextForm.ShowDialog(this);
        }

        // Acct. Approve
        private void BtnAcctApprove_Click(object sender, EventArgs e)
        {
            #region 表頭及表身Currency不可空白
            bool currencyEmpty = false;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["CurrencyID"]))
                {
                    currencyEmpty = true;
                }
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CurrencyID"]) || currencyEmpty)
            {
                MyUtility.Msg.WarningBox("Currency cannot be empty!!");
                return;
            }
            #endregion

            #region 檢查LocalSupp_Bank
            DualResult resultCheckLocalSupp_BankStatus = Prgs.CheckLocalSupp_BankStatus(this.CurrentMaintain["localsuppid"].ToString(), Prgs.CallFormAction.Confirm);
            if (!resultCheckLocalSupp_BankStatus)
            {
                return;
            }
            #endregion

            if (MyUtility.Check.Empty(MyUtility.GetValue.Lookup(string.Format("select Accountant from ShippingAP WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])))))
            {
                // Approve
                if (MyUtility.Check.Empty(this.CurrentMaintain["SubType"]) || (MyUtility.Convert.GetString(this.CurrentMaintain["SubType"]) != "OTHER" && !MyUtility.Check.Seek(string.Format("select ShippingAPID from ShareExpense WITH (NOLOCK) where ShippingAPID = '{0}' and junk = 0", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])))))
                {
                    MyUtility.Msg.WarningBox("No share expense, can't approve!");
                    return;
                }

                string sqlCmd = string.Format("update ShippingAP set Accountant = '{0}', ApvDate = GETDATE(), Status = 'Approved' where ID = '{1}'", Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
                DualResult result = DBProxy.Current.Execute(null, sqlCmd);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Confirm fail !\r\n" + result.ToString());
                    return;
                }

                string apemail = MyUtility.GetValue.Lookup($"select Email from Pass1 with(nolock) where id = '{this.CurrentMaintain["Handle"]}'");
                sqlCmd = $@"
select distinct
    e.id,
    subjuct = concat('IP for', e.ID, ' ', e.Consignee, ' ', e.ShipModeID, ' B/L#', e.Blno),
    HandleEmail = (select Email from TPEPass1 with(nolock) where ID = e.Handle),
    ShipPlanHandleEmail = (select Email from TPEPass1 with(nolock) where ID = ed.ShipPlanHandle)
from Export e with(nolock)
inner join ShareExpense se with(nolock) on se.WKNo = e.ID
inner join Export_Detail ed with(nolock) on ed.ID = e.ID
where e.Payer='M'
and se.ShippingAPID = '{this.CurrentMaintain["ID"]}'
";
                DBProxy.Current.Select(null, sqlCmd, out DataTable emailDt);
                if (emailDt.Rows.Count > 0)
                {
                    string description = @"Hi,
We paid freight /clearance fee for supplier, pls issue SD (Supplier debit note) to charge it.
Thank you.";

                    // by exportID 寄不同封信件
                    foreach (string exportID in emailDt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["ID"])).Distinct().ToList())
                    {
                        DataRow[] drs = emailDt.Select($"ID = '{exportID}'");
                        string subject = MyUtility.Convert.GetString(drs[0]["subjuct"]);
                        List<string> maillist = new List<string>() { apemail };
                        drs.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["HandleEmail"])).Distinct().ToList().ForEach(em => maillist.Add(em));
                        drs.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["ShipPlanHandleEmail"])).Distinct().ToList().ForEach(em => maillist.Add(em));

                        string mailto = maillist.Distinct().JoinToString(";");
                        var email = new MailTo(Env.Cfg.MailFrom, mailto, string.Empty, subject, string.Empty, description, true, true);
                        email.ShowDialog(this);
                    }
                }

                this.RenewData();
                this.OnDetailEntered();
            }
            else
            {
                // Unapprove
                DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Unapprove > this data?", "Warning", MessageBoxButtons.YesNo);
                if (buttonResult == DialogResult.No)
                {
                    return;
                }

                string sqlCmd = string.Format("update ShippingAP set Accountant = '', ApvDate = null, Status = 'New' where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

                DualResult result = DBProxy.Current.Execute(null, sqlCmd);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Unapprove fail !\r\n" + result.ToString());
                }

                this.RenewData();
                this.OnDetailEntered();
            }
        }

        private void ComboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboType.SelectedValue == null || !this.EditMode)
            {
                return;
            }

            this.comboType2.SelectedIndexChanged -= this.ComboType2_SelectedIndexChanged;

            // switch (CurrentMaintain["Type"].ToString())
            switch (this.comboType.SelectedValue.ToString())
            {
                case "IMPORT":
                    // comboxbs2_1.Position = 0;
                    this.comboType2.DataSource = this.subType_1;

                    break;
                case "EXPORT":
                    // comboxbs2_2.Position = 0;
                    this.comboType2.DataSource = this.subType_2;
                    break;
                default:
                    // comboxbs2_1.Position = 0;
                    this.comboType2.DataSource = this.subType_1;
                    break;
            }

            this.comboType2.SelectedIndex = -1;

            this.comboType2.SelectedIndexChanged += this.ComboType2_SelectedIndexChanged;
        }

        private void TxtReason_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string strsqlcmd = $@"select ID,Description from ShippingReason where type='AP' and junk=0";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(strsqlcmd, "8,20", this.txtReason.Text)
            {
                Size = new Size(410, 666),
            };
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtReason.Text = item.GetSelectedString();
            this.txtReasonDesc.Text = item.GetSelecteds()[0]["Description"].ToString();
        }

        private void TxtReason_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.txtReason.Text))
            {
                this.txtReasonDesc.Text = string.Empty;
                return;
            }

            this.CheckNoExportChargesGMT(this.CurrentMaintain["BLNo"].ToString());

            string strsqlcmd = $@"select ID,Description from ShippingReason where type='AP' and junk=0 and id ='{this.txtReason.Text}'";

            if (!MyUtility.Check.Seek(strsqlcmd))
            {
                MyUtility.Msg.WarningBox($@"Reason: {this.txtReason.Text} is not found!");
                this.txtReason.Text = string.Empty;
                this.txtReason.Focus();
            }

            this.CurrentMaintain["Reason"] = this.txtReason.Text;
        }

        private void ComboType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboType.SelectedValue == null || this.comboType2.SelectedValue == null || !this.EditMode || !this.IsDetailInserting)
            {
                return;
            }

            switch (this.comboType.SelectedValue.ToString())
            {
                case "EXPORT":
                    if (string.Compare(this.comboType2.SelectedValue.ToString(), "Other", true) == 0)
                    {
                        MyUtility.Msg.InfoBox(@"
Please be sure this Account Payable is for the following items, otherwise, please choose corresponding Export Type and complete Share Expense, Thank You.
      Annual Fee, 
      Purchaser C/O Form, 
      Machine Export, 
      Shred/Scrap Export,
      CN Fabric for testing, 
      Non SP# Sample/Mock-up
");
                    }

                    break;
                case "IMPORT":
                    if (string.Compare(this.comboType2.SelectedValue.ToString(), "Other", true) == 0)
                    {
                        MyUtility.Msg.InfoBox(@"
Please be sure this Account Payable is for the following items, otherwise, please choose corresponding Import Type and complete Share Expense, Thank You.
Annual Fee, 
Purchase Form for Import purpose, 
Machine Import, 
Shred/Scrap Import.
Non SP# Sample/Mock-up
");
                    }

                    break;
                default:
                    this.comboType2.DataSource = this.subType_1;
                    break;
            }
        }

        // B/L NO. Change
        private void TxtBLNo_Validating(object sender, CancelEventArgs e)
        {
            if (!this.CheckNoExportChargesGMT(this.txtBLNo.Text))
            {
                return;
            }

            this.IncludeFoundryRefresh(this.txtBLNo.Text);

            if (this.txtBLNo.Text.Empty() || this.comboType2.SelectedValue.EqualString("OTHER"))
            {
                return;
            }

            #region ISP20210237 check [GB#]and[WK#]and[FTY WK#]
            if ((this.checkIsFreightForwarder.Checked && this.CurrentMaintain["Reason"].ToString() != "AP007") ||
                this.IsCustoms)
            {
                if (!MyUtility.Check.Empty(this.CurrentMaintain["SisFtyAPID"]))
                {
                    return;
                }

                string sqlBLNoCheck = $@"
if  not exists (select  1
                from GMTBooking with (nolock) where BLno = @BLno or BL2no = @BLno
                union all
                select  1
                from Export with (nolock) where BLno = @BLno
                union all
                select  1
                from FtyExport with (nolock) where BLno = @BLno
                union all
                select 1
                from TransferExport with (nolock) where BLno = @BLno
)
begin
    select  [Code] = 'P05.',
            [Manual] = 'P05. Garment Booking',
            [Abbr.] = 'GB#',
            [Field] = '[BL/MAWB No.]or[FCR/BL/HAWB]',
            [Descriptions] = 'If the [BL/MAWB No.]or[FCR/BL/HAWB] in GB# is entered incorrectly,
please go to Trade/Garment Export/P04. Garment Invoice Data Iaintain to modify it.'
    union all
    select  [Code] = 'P03.',
            [Manual] = 'P03. Import Schedule',
            [Abbr.] = 'WK#',
            [Field] = '[B/L#]',
            [Descriptions] = 'If the [B/L#] in WK# is entered incorrectly, please contact TPE Shipping to modify it.'
    union all
    select  [Code] = 'P04.',
            [Manual] = 'P04. Raw Material Shipment',
            [Abbr.] = 'FTY WK#',
            [Field] = '[B/L(AWB) No.]',
            [Descriptions] = 'If the [B/L (AWB) No.] in FTY WK# is entered incorrectly,
pleasse go to PMS/Shipping/P04. Raw Material Shipment Data Maintain to modify it.'
    union all

    select  [Code]='P16. ',
            [Manual] = 'P16. Transfer Material WK',
            [Abbr.] ='Transfer WK#',
            [Field] = 'B/L No.',
            [Descriptions] = 'If the [B/L No.] in Transfer WK# is entered incorrectly, please contact TPE Shipping to modify it.'
end         
";
                DataTable resultBLNoCheck;
                List<SqlParameter> listPar = new List<SqlParameter>();
                listPar.Add(new SqlParameter("@BLno", this.txtBLNo.Text));
                DualResult result = DBProxy.Current.Select(null, sqlBLNoCheck, listPar, out resultBLNoCheck);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                if (resultBLNoCheck.Rows.Count > 0)
                {
                    string errMsg = @"The B/L# you entered cannot be found in the below data, please first check whether the B/L# is correct,
You can complete Account Payment only after the corresponding Number is updated, Thank You.";
                    Win.UI.MsgGridForm m = new Win.UI.MsgGridForm(resultBLNoCheck, errMsg);

                    // m.TopMost = true; 這個不能加，不然電腦會爆炸
                    m.grid1.Columns[0].Width = 100;
                    m.grid1.Columns[1].Width = 200;
                    m.grid1.Columns[2].Width = 100;
                    m.grid1.Columns[3].Width = 200;
                    m.grid1.Columns[4].Width = 500;
                    m.ShowDialog();
                    this.CurrentMaintain["BLNo"] = string.Empty;
                    return;
                }
            }
            #endregion

            if (this.comboType.SelectedValue == null || !this.EditMode || !this.IsDetailInserting)
            {
                return;
            }

            this.disVesselName.Text = string.Empty;
            string sBLNo = this.txtBLNo.Text;
            if (this.comboType.SelectedValue.ToString().Equals("IMPORT") && !MyUtility.Check.Empty(sBLNo))
            {
                List<System.Data.SqlClient.SqlParameter> listPara = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter("@blno", MyUtility.Convert.GetString(sBLNo)),
                };
                string sql = "select top 1 Vessel from Export where blno = @blno";
                this.disVesselName.Text = MyUtility.GetValue.Lookup(sql, listPara);
            }
        }

        private void TxtSubconSupplier_Validating(object sender, CancelEventArgs e)
        {
            string localSuppID = this.txtSubconSupplier.TextBox1.Text;

            DataTable dt;
            DBProxy.Current.Select(null, $@"SELECT * FROM LocalSupp WHERE ID = '{localSuppID}'", out dt);

            if (dt.Rows.Count > 0)
            {
                string isFactory = dt.Rows[0]["IsFactory"].ToString();
                string currencyID = dt.Rows[0]["CurrencyID"].ToString();

                this.CurrentMaintain["LocalSuppID"] = localSuppID;
                this.CurrentMaintain["CurrencyID"] = currencyID;

                if (MyUtility.Check.Empty(isFactory) ? false : Convert.ToBoolean(isFactory))
                {
                    this.txtcurrency.ReadOnly = false;
                    this.txtcurrency.IsSupportEditMode = true;
                }
                else
                {
                    this.txtcurrency.ReadOnly = true;
                    this.txtcurrency.IsSupportEditMode = false;
                }
            }
            else
            {
                this.txtcurrency.ReadOnly = true;
                this.txtcurrency.IsSupportEditMode = false;
            }

            this.RefreshIsFreightForwarder();
        }

        private void BtnIncludeFoundryRatio_Click(object sender, EventArgs e)
        {
            P08_IncludeFoundryRatio form = new P08_IncludeFoundryRatio(this.CurrentMaintain);
            form.ShowDialog();
        }

        private void RefreshIsFreightForwarder()
        {
            bool needDoCheckNoExportChargesGMT;
            this.checkIsFreightForwarder.Checked = this.CheckIsFreightForwarder(this.CurrentMaintain["LocalSuppID"].ToString(), out needDoCheckNoExportChargesGMT);

            if (needDoCheckNoExportChargesGMT)
            {
                this.CheckNoExportChargesGMT(this.CurrentMaintain["BLNo"].ToString());
            }
        }

        private bool CheckIsFreightForwarder(string localSuppID, out bool needDoCheckNoExportChargesGMT)
        {
            DataRow drResult;
            bool isExists = MyUtility.Check.Seek($@"SELECT IsFreightForwarder FROM LocalSupp WHERE ID = '{localSuppID}'", out drResult);
            needDoCheckNoExportChargesGMT = false;
            if (isExists)
            {
                needDoCheckNoExportChargesGMT = true;
                return MyUtility.Convert.GetBool(drResult["IsFreightForwarder"]);
            }

            return false;
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (this.EditMode &&
                MyUtility.Check.Empty(this.CurrentMaintain["Accountant"]))
            {
                string localSuppID = MyUtility.Convert.GetString(this.CurrentMaintain["LocalSuppID"]);
                string expressTypeID = this.comboType.SelectedValue.ToString().ToLower().EqualString("export") ? "1" : "2";
                string sqlCmd = string.Format(
                    @"
select ID
	, Description
	, [Brand] = BrandID
	, CurrencyID
	, Price
	, [Unit] = UnitID 
    , AccountID 
from ShipExpense WITH (NOLOCK) 
where exists (select 1 from SciFMS_AccountNo sa where sa.ID = ShipExpense.AccountID and sa.ExpressTypeID in ({1}, 3))
and Junk = 0 
and LocalSuppID = '{0}' 
and AccountID != ''",
                    localSuppID,
                    expressTypeID);
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out DataTable dt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(dt, "ID,Description,Brand,CurrencyID,Price,Unit,AccountID", "ID,Description,Brand,CurrencyID,Price,Unit,AccountID", "20,50,6,3,11,8,10", string.Empty, columndecimals: "0,0,0,0,4,0");
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                DataTable detailDT = (DataTable)this.detailgridbs.DataSource;
                foreach (DataRow dr in item.GetSelecteds())
                {
                    DataRow newRow = detailDT.NewRow();
                    newRow["ShipExpenseID"] = dr["ID"];
                    newRow["Description"] = dr["Description"];
                    newRow["Price"] = MyUtility.Convert.GetDecimal(dr["Price"]);
                    newRow["Qty"] = 1;
                    newRow["CurrencyID"] = dr["CurrencyID"];
                    newRow["Rate"] = 1;
                    newRow["Amount"] = MyUtility.Convert.GetDecimal(dr["Price"]);
                    newRow["UnitID"] = dr["Unit"];
                    newRow["AccountID"] = dr["AccountID"];
                    string sqlcmd = $@"select a.Name from SciFMS_AccountNO a  where a.ID = '{dr["AccountID"]}'";
                    newRow["Account"] = MyUtility.Convert.GetString(dr["AccountID"]) + "-" + MyUtility.GetValue.Lookup(sqlcmd);
                    detailDT.Rows.Add(newRow);
                }
            }
        }

        private bool CheckNoExportChargesGMT(string blNo)
        {
            if (this.checkIsFreightForwarder.Checked &&
                !this.txtReason.Text.EqualString("AP007") &&
                !MyUtility.Check.Empty(blNo))
            {
                string sqlCheckNoExportCharges = $@"
select  ID
from GMTBooking with (nolock)
where   (BL2no = '{blNo}' or BLno = '{blNo}') and
        NoExportCharges = 1
";
                DataTable dtCheckResult;
                DualResult result = DBProxy.Current.Select(null, sqlCheckNoExportCharges, out dtCheckResult);
                if (!result)
                {
                    this.ShowErr(result);
                    return false;
                }

                if (dtCheckResult.Rows.Count > 0)
                {
                    this.txtBLNo.Text = string.Empty;
                    this.txtBLNo.ValidateControl();
                    MyUtility.Msg.WarningBox($"<{dtCheckResult.AsEnumerable().Select(s => s["ID"].ToString()).JoinToString(",")}> is ticked \"No export charge\". If shipping expense incurred, please untick \"no export charge\" on garment booking.");
                    return false;
                }
            }

            return true;
        }

        private DualResult UpdateAirPP()
        {
            #region ISP20220298 更新AirPP資料

            // local Airpp
            string updateAirPP = $@"
update a set a.ActAmt = airAmt.ActAmt, a.ExchangeRate = airAmt.ExchangeRate, a.ShareExpenseEditDate = getdate()
	from AirPP a
	inner join dbo.GetAirPPAmt('{this.CurrentMaintain["ID"]}', '') airAmt on airAmt.AirPPID = a.ID

select AirPPID, ActAmt, ExchangeRate
from dbo.GetAirPPAmt('{this.CurrentMaintain["ID"]}', '')
";
            DualResult result = DBProxy.Current.Select(null, updateAirPP, out DataTable dtAirPPAmt);

            if (!result)
            {
                return result;
            }

            string sqlInvNo = $@"select distinct InvNo from ShareExpense with (nolock) where  ShippingAPID = '{this.CurrentMaintain["ID"]}'";
            if (!(result = DBProxy.Current.Select(string.Empty, sqlInvNo, out DataTable dtInvNo)))
            {
                return result;
            }

            if (dtInvNo != null && dtInvNo.Rows.Count > 0 && dtAirPPAmt.Rows.Count > 0)
            {
                List<string> listInv = dtInvNo.AsEnumerable().Select(s => s["InvNo"].ToString()).ToList();
                List<string> listPLFromRgCode = PackingA2BWebAPI.GetPLFromRgCodeByMutiInvNo(listInv);
                foreach (string plFromRgCode in listPLFromRgCode)
                {
                    PackingA2BWebAPI_Model.DataBySql dataBySql = new PackingA2BWebAPI_Model.DataBySql()
                    {
                        SqlString = @"
alter table #tmp alter column AirPPID varchar(13)
update a set a.ActAmt = airAmt.ActAmt, a.ExchangeRate = airAmt.ExchangeRate, a.ShareExpenseEditDate = getdate()
from AirPP a
inner join #tmp airAmt on airAmt.AirPPID = a.ID
",
                        TmpTable = JsonConvert.SerializeObject(dtAirPPAmt),
                    };
                    result = PackingA2BWebAPI.ExecuteBySql(plFromRgCode, dataBySql);

                    if (!result)
                    {
                        return result;
                    }
                }
            }
            #endregion

            return new DualResult(true);
        }

        private void BtnUpdateA2BData_Click(object sender, EventArgs e)
        {
            DataTable dtShippingAP;
            string sqlGetNeedReCalculateData = @"
select * from ShippingAP with (nolock) 
where   AddDate >= DATEADD(MONTH, -6, GETDATE()) and
        Type = 'EXPORT' and
        SubType = 'GARMENT'";
            DualResult result = DBProxy.Current.Select(null, sqlGetNeedReCalculateData, out dtShippingAP);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dtShippingAP.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("No Data need Update A2B Data");
                return;
            }

            foreach (DataRow drCurrentMaintain in dtShippingAP.Rows)
            {
                bool isReasonAP007 = drCurrentMaintain["Reason"].EqualString("AP007");
                bool isFreightForwarder = this.CheckIsFreightForwarder(drCurrentMaintain["LocalSuppID"].ToString(), out bool needDoCheckNoExportChargesGMT);
                P08_ShareExpense p08_ShareExpense = new P08_ShareExpense(drCurrentMaintain, isFreightForwarder, isReasonAP007, this.InitialShareExpense, false);
                result = p08_ShareExpense.ReCalculateShareExpense();
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }
            }

            MyUtility.Msg.InfoBox("Update A2B Data complete");
        }
    }
}
