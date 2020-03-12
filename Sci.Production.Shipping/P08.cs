using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;
using Ict.Data;
using Sci.Win;
using System.Reflection;
using System.Linq;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P08
    /// </summary>
    public partial class P08 : Sci.Win.Tems.Input6
    {
        // Dictionary<String, String> comboBox2_RowSource1 = new Dictionary<string, string>();
        // Dictionary<String, String> comboBox2_RowSource2 = new Dictionary<string, string>();
        private DataTable subType_1 = new DataTable();
        private DataTable subType_2 = new DataTable();
        private BindingSource comboxbs1;

        // , comboxbs2_1, comboxbs2_2;
        private Ict.Win.DataGridViewGeneratorTextColumnSettings code = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings qty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings rate = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings price = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.UI.DataGridViewTextBoxColumn col_code;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_qty;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_rate;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_remark;
        private bool haveEditShareFee;

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
where p1.ID='{Sci.Env.User.UserID}'
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
select sd.*,isnull(se.Description,'') as Description, (isnull(se.AccountID,'') + '-' + isnull(a.Name,'')) as Account,se.UnitID
    ,a.IsAPP ,a.IsShippingVAT,a2.AdvancePaymentTPE
from ShippingAP_Detail sd WITH (NOLOCK) 
left join ShipExpense se WITH (NOLOCK) on se.ID = sd.ShipExpenseID
left join SciFMS_AccountNO a on a.ID = se.AccountID
left join SciFMS_AccountNO a2 on a2.ID = substring(se.AccountID,1,4)
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
                    new System.Data.SqlClient.SqlParameter("@Reason", MyUtility.Convert.GetString(this.CurrentMaintain["Reason"]))
                };

            bool status = MyUtility.Check.Empty(this.CurrentMaintain["Accountant"]);
            this.btnAcctApprove.Enabled = status ? !this.EditMode && Prgs.GetAuthority(Sci.Env.User.UserID, "P08. Account Payment - Shipping", "CanConfirm") : MyUtility.Check.Empty(this.CurrentMaintain["VoucherID"]) && Prgs.GetAuthority(this.CurrentMaintain["Accountant"].ToString(), "P08. Account Payment - Shipping", "CanUnConfirm");
            this.btnAcctApprove.Text = status ? "Acct. Approve" : "Acct. Unapprove";
            this.btnAcctApprove.ForeColor = status ? Color.Blue : Color.Black;
            this.comboType2.SelectedValue = this.CurrentMaintain["SubType"].ToString();
            this.disExVoucherID.Text = this.CurrentMaintain["ExVoucherID"].ToString();
            // Reason description
            this.txtReasonDesc.Text = MyUtility.GetValue.Lookup(
                                $@"select Description from ShippingReason where id=@Reason 
                                    and type='AP' and junk=0",
                                listPara);
            string sql = "select top 1 Vessel from Export where blno = @blno";
            this.disVesselName.Text = this.CurrentMaintain["Type"].ToString().Equals("IMPORT") && !MyUtility.Check.Empty(this.CurrentMaintain["BLNo"]) ? MyUtility.GetValue.Lookup(sql, listPara) : string.Empty;
            this.chkIncludeFoundry.Checked = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup(string.Format(" select top 1 'True' from GMTBooking where BLNo = '{0}' or BL2No = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["BLNo"]))));
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            #region Code的按右鍵與Validating、Qty與Rate的Validating
            this.code.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            if (MyUtility.Check.Empty(this.CurrentMaintain["Accountant"]))
                            {
                                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                                string localSuppID = MyUtility.Convert.GetString(this.CurrentMaintain["LocalSuppID"]);
                                string sqlCmd = string.Format("select ID,Description,[Brand]=BrandID,CurrencyID, Price,[Unit]=UnitID from ShipExpense WITH (NOLOCK) where Junk = 0 and LocalSuppID = '{0}' and AccountID != ''", localSuppID);
                                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "20,50,6,3,11,8", MyUtility.Convert.GetString(dr["ShipExpenseID"]), columndecimals: "0,0,0,0,4");
                                DialogResult returnResult = item.ShowDialog();
                                if (returnResult == DialogResult.Cancel)
                                {
                                    return;
                                }

                                e.EditingControl.Text = item.GetSelectedString();
                            }
                        }
                    }
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
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@localsuppid", MyUtility.Convert.GetString(this.CurrentMaintain["LocalSuppID"]));
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@shipexpenseid", MyUtility.Convert.GetString(MyUtility.Convert.GetString(e.FormattedValue)));

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        DataTable expenseData;
                        string sqlCmd = "select ID,Description,LocalSuppID,CurrencyID,Price,BrandID,UnitID from ShipExpense WITH (NOLOCK) where Junk = 0 and LocalSuppID = @localsuppid and ID = @shipexpenseid  and AccountID != ''";
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
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["CDate"] = DateTime.Today;
            this.CurrentMaintain["Handle"] = Sci.Env.User.UserID;
            this.CurrentMaintain["VATRate"] = 0;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "IMPORT";
            this.comboType2.DataSource = this.subType_1;
            this.CurrentMaintain["SubType"] = "MATERIAL";
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
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

        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            this.CurrentMaintain["Handle"] = Sci.Env.User.UserID;
            this.CurrentMaintain["InvNo"] = string.Empty;
            this.CurrentMaintain["ID"] = string.Empty;
            this.CurrentMaintain["VoucherID"] = string.Empty;
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

            if (MyUtility.Check.Empty(this.CurrentMaintain["PayTermID"]))
            {
                this.txtpayterm_ftyTerms.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Terms can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["BLNo"]))
            {
                this.txtBLNo.Focus();
                MyUtility.Msg.WarningBox("B/L No. can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Handle"]))
            {
                this.txtUserHandle.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Handle can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CurrencyID"]))
            {
                this.displayCurrency.Focus();
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
                        if (buttonResult == System.Windows.Forms.DialogResult.No)
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
                        if (buttonResult == System.Windows.Forms.DialogResult.No)
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
                string newID = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "SA", "ShippingAP", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(newID))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = newID;
            }

            DataTable tmpdt;
            sqlcmd = $@"
select [IsAPP] = dbo.GetAccountNoExpressType(se.AccountID, 'IsAPP')
    ,[vat] = dbo.GetAccountNoExpressType(se.AccountID, 'vat')
    ,[AdvancePaymentTPE] = dbo.GetAccountNoExpressType(se.AccountID, 'AdvancePaymentTPE')
    ,[SisFty] = dbo.GetAccountNoExpressType(se.AccountID, 'SisFty')
from #tmp sd
left join ShipExpense se WITH (NOLOCK) on se.ID = sd.ShipExpenseID
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
            if (this.haveEditShareFee)
            {
                DualResult result = DBProxy.Current.Execute(null, string.Format("delete from ShareExpense where ShippingAPID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Delete ShareExpense fail\r\n" + result.ToString());
                    return failResult;
                }
            }

            return base.ClickSavePre();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            DualResult result = DBProxy.Current.Execute(
                "Production",
                string.Format("exec CalculateShareExpense '{0}','{1}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), Sci.Env.User.UserID));
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Re-calcute share expense failed!");
                return failResult;
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

            return Result.True;
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
                rd.ReportDataSources.Add(new System.Collections.Generic.KeyValuePair<string, object>("Report_ShippingAPDetail", report_ShippingAPDetail));
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

                using (var frm = new Sci.Win.Subs.ReportView(rd))
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
            bool apflag = false;
            if (((DataTable)this.detailgridbs.DataSource).Rows.Count > 0)
            {
                var dt = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().
                    Where(w => MyUtility.Convert.GetString(w["Account"]).Substring(0, 4) == "6105" || MyUtility.Convert.GetString(w["Account"]).Substring(0, 4) == "5912").ToList();
                if (dt.Count > 0)
                {
                    apflag = true;
                }
            }

            Sci.Production.Shipping.P08_ShareExpense callNextForm = new Sci.Production.Shipping.P08_ShareExpense(this.CurrentMaintain, apflag);
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
                if (MyUtility.Check.Empty(this.CurrentMaintain["SubType"]) || (MyUtility.Convert.GetString(this.CurrentMaintain["SubType"]) != "OTHER" && !MyUtility.Check.Seek(string.Format("select ShippingAPID from ShareExpense WITH (NOLOCK) where ShippingAPID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])))))
                {
                    MyUtility.Msg.WarningBox("No share expense, can't approve!");
                    return;
                }

                string sqlCmd = string.Format("update ShippingAP set Accountant = '{0}', ApvDate = GETDATE(), Status = 'Approved' where ID = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
                DualResult result = DBProxy.Current.Execute(null, sqlCmd);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Confirm fail !\r\n" + result.ToString());
                    return;
                }

                this.RenewData();
                this.OnDetailEntered();
            }
            else
            {
                // Unapprove
                DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Unapprove > this data?", "Warning", MessageBoxButtons.YesNo);
                if (buttonResult == System.Windows.Forms.DialogResult.No)
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
        }

        private void txtReason_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string strsqlcmd = $@"select ID,Description from ShippingReason where type='AP' and junk=0";
            Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(strsqlcmd, "8,20", this.txtReason.Text);
            item.Size = new System.Drawing.Size(410, 666);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtReason.Text = item.GetSelectedString();
            this.txtReasonDesc.Text = item.GetSelecteds()[0]["Description"].ToString();

        }

        private void txtReason_Validating(object sender, CancelEventArgs e)
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

            string strsqlcmd = $@"select ID,Description from ShippingReason where type='AP' and junk=0 and id ='{this.txtReason.Text}'";

            if (!MyUtility.Check.Seek(strsqlcmd))
            {
                MyUtility.Msg.WarningBox($@"Reason: {this.txtReason.Text} is not found!");
                this.txtReason.Text = string.Empty;
                this.txtReason.Focus();
            }

            this.CurrentMaintain["Reason"] = this.txtReason.Text;
        }

        private void comboType2_SelectedIndexChanged(object sender, EventArgs e)
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
                    new System.Data.SqlClient.SqlParameter("@blno", MyUtility.Convert.GetString(sBLNo))
                };
                string sql = "select top 1 Vessel from Export where blno = @blno";
                this.disVesselName.Text = MyUtility.GetValue.Lookup(sql, listPara);
            }
        }
    }
}
