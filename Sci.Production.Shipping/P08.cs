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

namespace Sci.Production.Shipping
{
    public partial class P08 : Sci.Win.Tems.Input6
    {
        //Dictionary<String, String> comboBox2_RowSource1 = new Dictionary<string, string>();
        //Dictionary<String, String> comboBox2_RowSource2 = new Dictionary<string, string>();
        DataTable subType_1 = new DataTable();
        DataTable subType_2 = new DataTable();
        BindingSource comboxbs1;
        //, comboxbs2_1, comboxbs2_2;
        Ict.Win.DataGridViewGeneratorTextColumnSettings code = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorNumericColumnSettings qty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        Ict.Win.DataGridViewGeneratorNumericColumnSettings rate = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        Ict.Win.DataGridViewGeneratorNumericColumnSettings price = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        Ict.Win.UI.DataGridViewTextBoxColumn col_code;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_qty;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_rate;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_price;
        Ict.Win.UI.DataGridViewTextBoxColumn col_remark;
        private bool haveEditShareFee;
        public P08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            detailgrid.AllowUserToOrderColumns = true;
            InsertDetailGridOnDoubleClick = false;
            txtSserAccountant.TextBox1.IsSupportEditMode = false;
            txtSserAccountant.TextBox1.ReadOnly = true;
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboFactory1, 1, factory);
            comboFactory1.SelectedIndex = 0;
            
            #region 輸入Supplier後自動帶出Currency & Terms，清空表身Grid資料
            txtSubconSupplier.TextBox1.Validated += (s, e) =>
                {
                    if (EditMode == true && txtSubconSupplier.TextBox1.ReadOnly == false)
                    {
                        if (MyUtility.Check.Empty(CurrentMaintain["LocalSuppID"]))
                        {
                            CurrentMaintain["CurrencyID"] = "";
                            CurrentMaintain["PayTermID"] = "";
                        }
                        else
                        {
                            DataRow dr;
                            if (MyUtility.Check.Seek(string.Format("select * from LocalSupp WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["LocalSuppID"])), out dr))
                            {
                                CurrentMaintain["CurrencyID"] = dr["CurrencyID"];
                                CurrentMaintain["PayTermID"] = dr["PayTermID"];
                            }
                            else
                            {
                                CurrentMaintain["CurrencyID"] = "";
                                CurrentMaintain["PayTermID"] = "";
                            }
                        }
                        // 清空表身Grid資料
                        foreach (DataRow dr in DetailDatas)
                        {
                            dr.Delete();
                        }
                    }
                };
            #endregion  
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            //設定ComboBox的內容值
            subType_1.ColumnsStringAdd("Key");
            subType_1.ColumnsStringAdd("Value");
            subType_1.Rows.Add("MATERIAL", "MATERIAL");
            subType_1.Rows.Add("SISTER FACTORY TRANSFER", "SISTER FACTORY TRANSFER");
            subType_1.Rows.Add("OTHER", "OTHER");

            subType_2.ColumnsStringAdd("Key");
            subType_2.ColumnsStringAdd("Value");
            subType_2.Rows.Add("GARMENT", "GARMENT");
            subType_2.Rows.Add("SISTER FACTORY TRANSFER", "SISTER FACTORY TRANSFER");
            subType_2.Rows.Add("OTHER", "OTHER");

            comboType2.ValueMember = "Key";
            comboType2.DisplayMember = "Value";

            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("IMPORT", "IMPORT");
            comboBox1_RowSource.Add("EXPORT", "EXPORT");
            comboxbs1 = new BindingSource(comboBox1_RowSource, null);
            comboType.DataSource = comboxbs1;
            comboType.ValueMember = "Key";
            comboType.DisplayMember = "Value";
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(@"select sd.*,isnull(se.Description,'') as Description, (isnull(se.AccountID,'') + '-' + isnull(a.Name,'')) as Account,se.UnitID
from ShippingAP_Detail sd WITH (NOLOCK) 
left join ShipExpense se WITH (NOLOCK) on se.ID = sd.ShipExpenseID
left join [FinanceEN].dbo.AccountNO a on a.ID = se.AccountID
where sd.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override DualResult OnRenewDataPost(Win.Tems.Input1.RenewDataPostEventArgs e)
        {
            if (e.Data != null)
            {
                string exact = MyUtility.GetValue.Lookup("Exact", MyUtility.Convert.GetString(e.Data["CurrencyID"]), "Currency", "ID");
                int decimalPlaces = MyUtility.Check.Empty(exact) ? 0 : Convert.ToInt32(exact);
                numAmount.DecimalPlaces = decimalPlaces;
                numVAT.DecimalPlaces = decimalPlaces;
                numTotal.DecimalPlaces = decimalPlaces;
                numTotal.Value = MyUtility.Convert.GetDecimal(e.Data["Amount"]) + MyUtility.Convert.GetDecimal(e.Data["VAT"]);
            }
            return base.OnRenewDataPost(e);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            //ChangeCombo2DataSource();
            #region set comboBox2 DataSource
            switch (CurrentMaintain["Type"].ToString())
            {
                case "IMPORT":
                    //comboxbs2_1.Position = 0;
                    comboType2.DataSource = subType_1;
                    //CurrentMaintain["SubType"] = temp;
                    break;
                case "EXPORT":
                    //comboxbs2_2.Position = 0;
                    comboType2.DataSource = subType_2;
                    //CurrentMaintain["SubType"] = temp;
                    break;
            }
            #endregion

            bool status = MyUtility.Check.Empty(CurrentMaintain["Accountant"]);
            btnAcctApprove.Enabled = status ? !EditMode && Prgs.GetAuthority(Sci.Env.User.UserID, "P08. Account Payment - Shipping", "CanConfirm") : MyUtility.Check.Empty(CurrentMaintain["VoucherID"]) && Prgs.GetAuthority(CurrentMaintain["Accountant"].ToString(), "P08. Account Payment - Shipping", "CanUnConfirm");
            btnAcctApprove.Text = status ? "Acct. Approve" : "Acct. Unapprove";
            btnAcctApprove.ForeColor = status ? Color.Blue : Color.Black;
            this.comboType2.SelectedValue = this.CurrentMaintain["SubType"].ToString();
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            #region Code的按右鍵與Validating、Qty與Rate的Validating
            code.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            if (MyUtility.Check.Empty(CurrentMaintain["Accountant"]))
                            {
                                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                                string localSuppID = MyUtility.Convert.GetString(CurrentMaintain["LocalSuppID"]);
                                string sqlCmd = string.Format("select ID,Description,LocalSuppID,CurrencyID, Price,BrandID,UnitID from ShipExpense WITH (NOLOCK) where Junk = 0 and LocalSuppID = '{0}' and AccountID != ''", localSuppID);
                                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "20,50,6,3,11,8", MyUtility.Convert.GetString(dr["ShipExpenseID"]), columndecimals: "0,0,0,0,4");
                                DialogResult returnResult = item.ShowDialog();
                                if (returnResult == DialogResult.Cancel) { return; }
                                e.EditingControl.Text = item.GetSelectedString();
                            }
                        }
                    }
                }
            };
            code.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["ShipExpenseID"]))
                    {
                        
                        //sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@localsuppid",MyUtility.Convert.GetString(CurrentMaintain["LocalSuppID"]));
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@shipexpenseid",MyUtility.Convert.GetString(MyUtility.Convert.GetString(e.FormattedValue)));
                        
                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        DataTable ExpenseData;
                        string sqlCmd = "select ID,Description,LocalSuppID,CurrencyID,Price,BrandID,UnitID from ShipExpense WITH (NOLOCK) where Junk = 0 and LocalSuppID = @localsuppid and ID = @shipexpenseid  and AccountID != ''";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out ExpenseData);
                        if (!result || ExpenseData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n"+result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox(string.Format("< Code: {0} > not found!!!", e.FormattedValue.ToString()));
                            }
                            dr["ShipExpenseID"] = "";
                            dr["Description"] = "";
                            dr["Price"] = 0;
                            dr["Qty"] = 0;
                            dr["CurrencyID"] = "";
                            dr["Rate"] = 0;
                            dr["Amount"] = 0;
                            dr["UnitID"] = "";
                            dr.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            dr["ShipExpenseID"] = MyUtility.Convert.GetString(e.FormattedValue).ToUpper();
                            dr["Description"] = ExpenseData.Rows[0]["Description"];
                            dr["Price"] = MyUtility.Convert.GetDecimal(ExpenseData.Rows[0]["Price"]);
                            dr["Qty"] = 1;
                            dr["CurrencyID"] = ExpenseData.Rows[0]["CurrencyID"];
                            dr["Rate"] = 1;
                            dr["Amount"] = MyUtility.Convert.GetDecimal(ExpenseData.Rows[0]["Price"]);
                            dr["UnitID"] = ExpenseData.Rows[0]["UnitID"];
                            dr.EndEdit();
                        }
                    }
                }
            };

            qty.CellValidating += (s, e) =>
                {
                    if (this.EditMode)
                    {
                        DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                        if (e.FormattedValue != null)
                        {
                            dr["Qty"] = e.FormattedValue;
                            CalculateGridAmount(dr);
                        }
                    }
                };

            rate.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (e.FormattedValue != null)
                    {
                        dr["Rate"] = e.FormattedValue;
                        CalculateGridAmount(dr);
                    }
                }
            };
            price.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (e.FormattedValue != null)
                    {
                        dr["price"] = e.FormattedValue;
                        CalculateGridAmount(dr);
                    }
                }
            };
            #endregion
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("ShipExpenseID", header: "Code", width: Widths.AnsiChars(10), settings: code).Get(out col_code)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Numeric("Qty", header: "Q'ty", width: Widths.AnsiChars(6), decimal_places: 4, settings: qty).Get(out col_qty)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CurrencyID", header: "Currency", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(9), decimal_places: 4,minimum:-9999999999, settings: price).Get(out col_price)
                .Numeric("Rate", header: "Rate", width: Widths.AnsiChars(9), decimal_places: 6, settings: rate).Get(out col_rate)
                .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(9), decimal_places: 4, iseditingreadonly: true)
                .Text("Remark", header: "WK#/Reamrk", width: Widths.AnsiChars(10)).Get(out col_remark)
                .Text("Account", header: "Account Name", width: Widths.AnsiChars(40), iseditingreadonly: true);
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["CDate"] = DateTime.Today;
            CurrentMaintain["Handle"] = Sci.Env.User.UserID;
            CurrentMaintain["VATRate"] = 0;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "IMPORT";
            comboType2.DataSource = subType_1;
            CurrentMaintain["SubType"] = "MATERIAL";
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            numTotal.Value = 0;
            gridicon.Append.Enabled = true;
            gridicon.Insert.Enabled = true;
            gridicon.Remove.Enabled = true;
            DetailGridEditing(true);
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            if (!MyUtility.Check.Empty(CurrentMaintain["Accountant"]))
            {
                dateDate.ReadOnly = true;
                comboType.ReadOnly = true;
                comboType2.ReadOnly = true;
                txtSubconSupplier.TextBox1.ReadOnly = true;
                txtpayterm_ftyTerms.TextBox1.ReadOnly = true;
                txtInvoice.ReadOnly = true;
                numVATRate.ReadOnly = true;
                txtBLNo.ReadOnly = true;
                txtUserHandle.TextBox1.ReadOnly = true;
                gridicon.Append.Enabled = false;
                gridicon.Insert.Enabled = false;
                gridicon.Remove.Enabled = false;
                DetailGridEditing(false);
            }
            else
            {
                gridicon.Append.Enabled = true;
                gridicon.Insert.Enabled = true;
                gridicon.Remove.Enabled = true;
                DetailGridEditing(true);
            }
            txtSubconSupplier.TextBox1.ReadOnly = true;
        }

        //protected override void ClickUndo()
        //{
        //    ChangeCombo2DataSource();
        //    base.ClickUndo();
        //}

        protected override bool ClickSaveBefore()
        {
            base.CurrentMaintain["SubType"] = this.comboType2.SelectedValue;
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentMaintain["CDate"]))
            {
                dateDate.Focus();
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Type"]))
            {
                comboType.Focus();
                MyUtility.Msg.WarningBox("Type can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["SubType"]))
            {
                comboType2.Focus();
                MyUtility.Msg.WarningBox("SubType can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["LocalSuppID"]))
            {
                txtSubconSupplier.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Supplier can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["PayTermID"]))
            {
                txtpayterm_ftyTerms.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Terms can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["BLNo"]))
            {
                txtBLNo.Focus();
                MyUtility.Msg.WarningBox("B/L No. can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Handle"]))
            {
                txtUserHandle.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Handle can't empty!!");
                return false;
            }
            #endregion

            //InvNo + B/L No不可以重複建立
            if (!MyUtility.Check.Empty(CurrentMaintain["InvNo"]))
            {
                DataRow dr;
                if (MyUtility.Check.Seek(string.Format("select ID from ShippingAp WITH (NOLOCK) where InvNo = '{0}' and BLNo = '{1}' and ID != '{2}'", MyUtility.Convert.GetString(CurrentMaintain["InvNo"]), MyUtility.Convert.GetString(CurrentMaintain["Handle"]), MyUtility.Convert.GetString(CurrentMaintain["ID"])), out dr))
                {
                    MyUtility.Msg.WarningBox("< Invoice# > and < B/L No. > duplicate with No."+ MyUtility.Convert.GetString(dr["ID"]));
                    return false;
                }
            }

            // 清空表身Grid資料
            int countRec = 0; //計算表身筆數
            decimal detailAmt = 0.0m; //表身Amount加總值
            foreach (DataRow dr in DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["ShipExpenseID"]))
                {
                    dr.Delete();
                    continue;
                }
                detailAmt = detailAmt + MyUtility.Convert.GetDecimal(dr["Amount"]);
                countRec = countRec + 1;
            }

            //表身資料為空
            if (countRec == 0)
            {
                MyUtility.Msg.WarningBox("< A/P# Detail> can't empty!!");
                return false;
            }
            haveEditShareFee = false;
            //當Type & SubType其中一個值異動過，且已有Share Expense資料，就要問使用者是否確定要存檔，若是，就要將Share Expense的資料給清掉
            if (!IsDetailInserting)
            {
                DataRow seekRow;
                if (MyUtility.Check.Seek(string.Format("select Type,SubType from ShippingAP WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"])), out seekRow))
                {
                    if (MyUtility.Convert.GetString(CurrentMaintain["Type"]) != MyUtility.Convert.GetString(seekRow["Type"]))
                    {
                        DialogResult buttonResult = MyUtility.Msg.WarningBox(string.Format("Already have expense data. Are you sure want to change the type from '{0}' to '{1}'", MyUtility.Convert.GetString(seekRow["Type"]), MyUtility.Convert.GetString(CurrentMaintain["Type"])), "Warning", MessageBoxButtons.YesNo);
                        if (buttonResult == System.Windows.Forms.DialogResult.No)
                        {
                            return false;
                        }
                        else
                        {
                            haveEditShareFee = true;
                        }
                    }
                    if (!haveEditShareFee && MyUtility.Convert.GetString(CurrentMaintain["SubType"]) != MyUtility.Convert.GetString(seekRow["SubType"]))
                    {
                        DialogResult buttonResult = MyUtility.Msg.WarningBox(string.Format("Already have expense data. Are you sure want to change the type from '{0}' to '{1}'", MyUtility.Convert.GetString(seekRow["SubType"]), MyUtility.Convert.GetString(CurrentMaintain["SubType"])), "Warning", MessageBoxButtons.YesNo);
                        if (buttonResult == System.Windows.Forms.DialogResult.No)
                        {
                            return false;
                        }
                        else
                        {
                            haveEditShareFee = true;
                        }
                    }
                }
                else
                {
                    MyUtility.Msg.WarningBox("Shipping AP Data not found!");
                    return false;
                }
            }

            //將表身Amount加總值回寫回表頭
            int exact = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup("Exact", MyUtility.Convert.GetString(CurrentMaintain["CurrencyID"]), "Currency", "ID"));
            CurrentMaintain["Amount"] = MyUtility.Math.Round(detailAmt, exact);
            CurrentMaintain["VAT"] = MyUtility.Convert.GetDecimal(CurrentMaintain["VATRate"]) > 0 ? MyUtility.Math.Round(MyUtility.Convert.GetDecimal(MyUtility.Math.Round(detailAmt, exact)) * MyUtility.Convert.GetDecimal(CurrentMaintain["VATRate"]) / 100, exact) : 0;

            //Get ID
            if (IsDetailInserting)
            {
                string newID = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "SA", "ShippingAP", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(newID))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = newID;
            }

            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSavePre()
        {
            if (haveEditShareFee)
            {
                DualResult result = DBProxy.Current.Execute(null, string.Format("delete from ShareExpense where ShippingAPID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"])));
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Delete ShareExpense fail\r\n" + result.ToString());
                    return failResult;
                }
            }
            return base.ClickSavePre();
        }

        protected override DualResult ClickSavePost()
        {
            bool result = Prgs.CalculateShareExpense(MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Re-calcute share expense failed!");
                return failResult;
            }
            return base.ClickSavePost();
        }

        protected override bool ClickDeleteBefore()
        {
            if (!MyUtility.Check.Empty(CurrentMaintain["Accountant"]))
            {
                MyUtility.Msg.WarningBox("This record is < Approved >, can't be deleted!");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        protected override DualResult ClickDeletePre()
        {
            string sqlCmd = string.Format("delete from ShareExpense where ShippingAPID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Delete ShareExpense false.\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        private string chk_null(string str)
        {
            if (str == "")
            {
                return " ";
            }
            else
            {
                return str;
            }
        }

        protected override bool ClickPrint()
        {
            DualResult result;
            IReportResource reportresource;
            ReportDefinition rd = new ReportDefinition();
            if (!(result = ReportResources.ByEmbeddedResource(Assembly.GetAssembly(GetType()), GetType(), "P08_Print.rdlc", out reportresource)))
            {
                MyUtility.Msg.ErrorBox(result.ToString());
            }
            else
            {
                DataTable FactoryData,LocalSpuuData,Report_ShippingAPDetail;
                string sqlCmd = string.Format("select NameEN, AddressEN,Tel from factory WITH (NOLOCK) where ID = '{0}'",
               MyUtility.Convert.GetString(CurrentMaintain["FactoryID"]));
                result = DBProxy.Current.Select(null,sqlCmd,out FactoryData);
                if(!result)
                {
                    MyUtility.Msg.WarningBox("Prepare data fail!!\r\n"+result.ToString());
                    return false;
                }
                sqlCmd = string.Format(@"select ls.Name,ls.Address,ls.Tel,isnull(lsb.AccountNo,' ') as AccountNo, 
isnull(lsb.AccountName,' ') as AccountName, isnull(lsb.BankName,' ') as BankName,
isnull(lsb.CountryID,' ') as CountryID, isnull(lsb.City,' ') as City, isnull(lsb.SWIFTCode,' ') as SWIFTCode
from LocalSupp ls WITH (NOLOCK) 
left join LocalSupp_Bank lsb WITH (NOLOCK) on ls.ID = lsb.ID and lsb.IsDefault = 1
where ls.ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["LocalSuppID"]));
                result = DBProxy.Current.Select(null, sqlCmd, out LocalSpuuData);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Prepare data fail!!\r\n" + result.ToString());
                    return false;
                }

                sqlCmd = string.Format(@"select sd.ShipExpenseID,isnull(se.Description,' ') as Description,sd.CurrencyID,sd.Price,sd.Qty,sd.Rate,sd.Amount,se.UnitID 
from ShippingAP_Detail sd WITH (NOLOCK) 
left join ShipExpense se WITH (NOLOCK) on sd.ShipExpenseID = se.ID
where sd.ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
                result = DBProxy.Current.Select(null, sqlCmd, out Report_ShippingAPDetail);

                rd.ReportResource = reportresource;
                rd.ReportDataSources.Add(new System.Collections.Generic.KeyValuePair<string, object>("Report_ShippingAPDetail", Report_ShippingAPDetail));
                if (FactoryData.Rows.Count == 0)
                {
                    rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("company", " "));
                    rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("compAddress", " "));
                    rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("compTel", " "));
                }
                else
                {
                    rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("company", MyUtility.Convert.GetString(FactoryData.Rows[0]["NameEN"])));
                    rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("compAddress", MyUtility.Convert.GetString(FactoryData.Rows[0]["AddressEN"])));
                    rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("compTel", MyUtility.Convert.GetString(FactoryData.Rows[0]["Tel"])));
                }

                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("apId", chk_null(MyUtility.Convert.GetString(CurrentMaintain["ID"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("barcodeAPId",chk_null( "*" + MyUtility.Convert.GetString(CurrentMaintain["ID"]) + "*")));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("apDate",chk_null( MyUtility.Check.Empty(CurrentMaintain["CDate"]) ? "null" : Convert.ToDateTime(CurrentMaintain["CDate"]).ToString())));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("supplier",chk_null( MyUtility.Convert.GetString(CurrentMaintain["LocalSuppID"]) + " " + MyUtility.Convert.GetString(LocalSpuuData.Rows[0]["Name"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("address",chk_null( MyUtility.Convert.GetString(LocalSpuuData.Rows[0]["Address"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("tel",chk_null( MyUtility.Convert.GetString(LocalSpuuData.Rows[0]["Tel"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("blNo",chk_null( MyUtility.Convert.GetString(CurrentMaintain["BLNo"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("payTerm",chk_null( MyUtility.GetValue.Lookup(string.Format("select Name from PayTerm WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["PayTermID"]))))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("invNo",chk_null( MyUtility.Convert.GetString(CurrentMaintain["InvNo"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("remark",chk_null( MyUtility.Convert.GetString(CurrentMaintain["Remark"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("acNo",chk_null( MyUtility.Convert.GetString(LocalSpuuData.Rows[0]["AccountNo"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("acName",chk_null( MyUtility.Convert.GetString(LocalSpuuData.Rows[0]["AccountName"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("bankName", chk_null(MyUtility.Convert.GetString(LocalSpuuData.Rows[0]["BankName"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("country",chk_null( MyUtility.Convert.GetString(LocalSpuuData.Rows[0]["CountryID"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("city",chk_null( MyUtility.Convert.GetString(LocalSpuuData.Rows[0]["City"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("swiftCode",chk_null( MyUtility.Convert.GetString(LocalSpuuData.Rows[0]["SWIFTCode"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("currency",chk_null( MyUtility.Convert.GetString(CurrentMaintain["CurrencyID"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("amount",chk_null( MyUtility.Convert.GetString(CurrentMaintain["Amount"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("vatRate",chk_null( MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(CurrentMaintain["VATRate"])/100))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("vat",chk_null( MyUtility.Convert.GetString(CurrentMaintain["VAT"]))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("total",chk_null( MyUtility.Convert.GetString(MyUtility.Convert.GetDecimal(CurrentMaintain["Amount"]) + MyUtility.Convert.GetDecimal(CurrentMaintain["VAT"])))));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("handle",chk_null( MyUtility.GetValue.Lookup(string.Format("select Name from Pass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["Handle"]))))));

                using (var frm = new Sci.Win.Subs.ReportView(rd))
                {
                    frm.ShowDialog(this);
                }
            }
            return base.ClickPrint();
        }
      
        //計算表身Grid欄位的Amount值
        private void CalculateGridAmount(DataRow dr)
        {
            decimal qty = MyUtility.Convert.GetDecimal(dr["Qty"]);
            decimal rate = MyUtility.Convert.GetDecimal(dr["Rate"]);
            dr["Amount"] = MyUtility.Math.Round(MyUtility.Math.Round(qty * MyUtility.Convert.GetDecimal(dr["Price"]), 2) * rate, 2);
            dr.EndEdit();
        }

        //控制表身Grid欄位是否可被編輯
        private void DetailGridEditing(bool isEditing)
        {
            if (isEditing)
            {
                col_code.IsEditingReadOnly = false;
                col_qty.IsEditingReadOnly = false;
                col_rate.IsEditingReadOnly = false;
                col_price.IsEditingReadOnly = false;
                col_remark.IsEditingReadOnly = false;
               
                for (int i = 0; i < detailgrid.ColumnCount; i++)
                {
                    if (i == 0 || i == 2 || i == 5|| i == 7)
                    {
                        detailgrid.Columns[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
            else
            {
                col_code.IsEditingReadOnly = true;
                col_qty.IsEditingReadOnly = true;
                col_rate.IsEditingReadOnly = true;
                col_price.IsEditingReadOnly = true;
                col_remark.IsEditingReadOnly = true;
                for (int i = 0; i < detailgrid.ColumnCount; i++)
                {
                    detailgrid.Columns[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        //Share Expense
        private void btnShareExpense_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P08_ShareExpense callNextForm = new Sci.Production.Shipping.P08_ShareExpense(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //Acct. Approve
        private void btnAcctApprove_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(MyUtility.GetValue.Lookup(string.Format("select Accountant from ShippingAP WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"])))))
            {
                //Approve
                if (MyUtility.Check.Empty(CurrentMaintain["SubType"]) || (MyUtility.Convert.GetString(CurrentMaintain["SubType"]) != "OTHER" && !MyUtility.Check.Seek(string.Format("select ShippingAPID from ShareExpense WITH (NOLOCK) where ShippingAPID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"])))))
                {
                    MyUtility.Msg.WarningBox("No share expense, can't approve!");
                    return;
                }
                string sqlCmd = string.Format("update ShippingAP set Accountant = '{0}', ApvDate = GETDATE(), Status = 'Approved' where ID = '{1}'",Sci.Env.User.UserID,MyUtility.Convert.GetString(CurrentMaintain["ID"]));
                DualResult result = DBProxy.Current.Execute(null, sqlCmd);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Confirm fail !\r\n" + result.ToString());
                    return;
                }

                RenewData();
                OnDetailEntered();
            }
            else
            {
                //Unapprove
                DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Unapprove > this data?", "Warning", MessageBoxButtons.YesNo);
                if (buttonResult == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                string sqlCmd = string.Format("update ShippingAP set Accountant = '', ApvDate = null, Status = 'New' where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));

                DualResult result = DBProxy.Current.Execute(null, sqlCmd);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Unapprove fail !\r\n" + result.ToString());
                }

                RenewData();
                OnDetailEntered();
            }
        }

        private void comboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboType.SelectedValue == null)
                return;

            if (!EditMode)
            {
                return;
            }

            //switch (CurrentMaintain["Type"].ToString())
            switch (comboType.SelectedValue.ToString())
            {
                case "IMPORT":
                    //comboxbs2_1.Position = 0;
                    comboType2.DataSource = subType_1;

                    //CurrentMaintain["SubType"] = temp;
                    break;
                case "EXPORT":
                    //comboxbs2_2.Position = 0;
                    comboType2.DataSource = subType_2;
                    //CurrentMaintain["SubType"] = temp;
                    break;
                default:
                    //comboxbs2_1.Position = 0;
                    comboType2.DataSource = subType_1;
                    //CurrentMaintain["SubType"] = temp;
                    break;
            }

            //CurrentMaintain["SubType"] = "";
            comboType2.SelectedIndex = -1;
            
            
        }

        private void masterpanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
