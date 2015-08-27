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

namespace Sci.Production.Shipping
{
    public partial class P08 : Sci.Win.Tems.Input6
    {
        Dictionary<String, String> comboBox2_RowSource1 = new Dictionary<string, string>();
        Dictionary<String, String> comboBox2_RowSource2 = new Dictionary<string, string>();
        BindingSource comboxbs1, comboxbs2_1, comboxbs2_2;
        Ict.Win.DataGridViewGeneratorTextColumnSettings code = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorNumericColumnSettings qty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        Ict.Win.DataGridViewGeneratorNumericColumnSettings rate = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        Ict.Win.UI.DataGridViewTextBoxColumn col_code;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_qty;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_rate;
        Ict.Win.UI.DataGridViewTextBoxColumn col_remark;
        private bool haveEditShareFee;
        public P08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DefaultFilter = string.Format("FactoryID='{0}'", Sci.Env.User.Factory);
            detailgrid.AllowUserToOrderColumns = true;
            InsertDetailGridOnDoubleClick = false;
            txtuser2.TextBox1.IsSupportEditMode = false;
            txtuser2.TextBox1.ReadOnly = true;
            
            #region 輸入Supplier後自動帶出Currency & Terms，清空表身Grid資料
            txtsubcon1.TextBox1.Validated += (s, e) =>
                {
                    if (EditMode == true && txtsubcon1.TextBox1.ReadOnly == false)
                    {
                        if (MyUtility.Check.Empty(CurrentMaintain["LocalSuppID"]))
                        {
                            CurrentMaintain["CurrencyID"] = "";
                            CurrentMaintain["PayTermID"] = "";
                        }
                        else
                        {
                            DataRow dr;
                            if (MyUtility.Check.Seek(string.Format("select * from LocalSupp where ID = '{0}'", CurrentMaintain["LocalSuppID"].ToString()), out dr))
                            {
                                CurrentMaintain["CurrencyID"] = MyUtility.Check.Empty(dr["CurrencyID"]) ? "" : dr["CurrencyID"].ToString();
                                CurrentMaintain["PayTermID"] = MyUtility.Check.Empty(dr["PayTermID"]) ? "" : dr["PayTermID"].ToString();
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

            //設定ComboBox的內容值
            comboBox2_RowSource1.Add("MATERIAL", "MATERIAL");
            comboBox2_RowSource1.Add("SISTER FACTORY TRANSFER", "SISTER FACTORY TRANSFER");
            comboBox2_RowSource1.Add("OTHER", "OTHER");

            comboBox2_RowSource2.Add("GARMENT", "GARMENT");
            comboBox2_RowSource2.Add("SISTER FACTORY TRANSFER", "SISTER FACTORY TRANSFER");
            comboBox2_RowSource2.Add("OTHER", "OTHER");

            comboxbs2_1 = new BindingSource(comboBox2_RowSource1, null);
            comboxbs2_2 = new BindingSource(comboBox2_RowSource2, null);
            comboBox2.DataSource = comboxbs2_1;
            comboBox2.ValueMember = "Key";
            comboBox2.DisplayMember = "Value";

            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("IMPORT","IMPORT");
            comboBox1_RowSource.Add("EXPORT","EXPORT");
            comboxbs1 = new BindingSource(comboBox1_RowSource, null);
            comboBox1.DataSource = comboxbs1;
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select sd.*,isnull(se.Description,'') as Description, (isnull(se.AccountNo,'') + '-' + isnull(a.Name,'')) as Account
from ShippingAP_Detail sd
left join ShipExpense se on se.ID = sd.ShipExpenseID
left join [Finance].dbo.AccountNo a on a.ID = se.AccountNo
where sd.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override DualResult OnRenewDataPost(Win.Tems.Input1.RenewDataPostEventArgs e)
        {
            if (e.Data != null)
            {
                int decimalPlaces = Convert.ToInt32(MyUtility.GetValue.Lookup("Exact", e.Data["CurrencyID"].ToString(), "Currency", "ID"));
                numericBox1.DecimalPlaces = decimalPlaces;
                numericBox3.DecimalPlaces = decimalPlaces;
                numericBox4.DecimalPlaces = decimalPlaces;
                numericBox4.Value = Convert.ToDecimal(e.Data["Amount"]) + Convert.ToDecimal(e.Data["VAT"]);
            }
            return base.OnRenewDataPost(e);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            ChangeCombo2DataSource();
            bool status = MyUtility.Check.Empty(CurrentMaintain["Accountant"]);
            button2.Enabled = status ? !EditMode && Prgs.GetAuthority(Sci.Env.User.UserID, this.Text, "CanConfirm") : MyUtility.Check.Empty(CurrentMaintain["VoucherNo"]) && Prgs.GetAuthority(CurrentMaintain["Accountant"].ToString(), this.Text, "CanUnConfirm");
            button2.Text = status ? "Acct. Approve" : "Acct. Unapprove";
            button2.ForeColor = status ? Color.Blue : Color.Black;
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
                                string localSuppID = MyUtility.Check.Empty(CurrentMaintain["LocalSuppID"]) ? "" : CurrentMaintain["LocalSuppID"].ToString();
                                string sqlCmd = string.Format("select ID,Description,LocalSuppID,CurrencyID,Price,BrandID from ShipExpense where Junk = 0 and LocalSuppID = '{0}'", localSuppID);
                                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "20,50,6,3,11,8", dr["ShipExpenseID"].ToString());
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
                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["ShipExpenseID"].ToString())
                    {
                        string localSuppID = MyUtility.Check.Empty(CurrentMaintain["LocalSuppID"]) ? "" : CurrentMaintain["LocalSuppID"].ToString();
                        DataRow ExpenseData;
                        if (!MyUtility.Check.Seek(string.Format("select ID,Description,LocalSuppID,CurrencyID,Price,BrandID from ShipExpense where Junk = 0 and LocalSuppID = '{0}' and ID = '{1}'", localSuppID, e.FormattedValue.ToString()), out ExpenseData))
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Code: {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["ShipExpenseID"] = "";
                            dr["Description"] = "";
                            dr["Price"] = 0;
                            dr["Qty"] = 0;
                            dr["CurrencyID"] = "";
                            dr["Rate"] = 0;
                            dr["Amount"] = 0;
                        }
                        else
                        {
                            dr["ShipExpenseID"] = e.FormattedValue.ToString().ToUpper();
                            dr["Description"] = ExpenseData["Description"].ToString();
                            dr["Price"] = Convert.ToDecimal(ExpenseData["Price"]);
                            dr["Qty"] = 1;
                            dr["CurrencyID"] = ExpenseData["CurrencyID"].ToString();
                            dr["Rate"] = 1;
                            dr["Amount"] = Convert.ToDecimal(ExpenseData["Price"]);
                        }
                        dr.EndEdit();
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
            #endregion 
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("ShipExpenseID", header: "Code", width: Widths.AnsiChars(10), settings: code).Get(out col_code)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Numeric("Qty", header: "Q'ty", width: Widths.AnsiChars(6), decimal_places: 4, settings: qty).Get(out col_qty)
                .Text("CurrencyID", header: "Currency", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("Price", header: "Price", width: Widths.AnsiChars(9), decimal_places: 4, iseditingreadonly: true)
                .Numeric("Rate", header: "Rate", width: Widths.AnsiChars(9), decimal_places: 6, settings: rate).Get(out col_rate)
                .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(9), decimal_places: 4, iseditingreadonly: true)
                .Text("Remark", header: "WK#/Reamrk", width: Widths.AnsiChars(10)).Get(out col_remark)
                .Text("Account", header: "Account Name", width: Widths.AnsiChars(30), iseditingreadonly: true);
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["CDate"] = DateTime.Today;
            CurrentMaintain["Handle"] = Sci.Env.User.UserID;
            CurrentMaintain["VATRate"] = 0;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "IMPORT";
            CurrentMaintain["SubType"] = "MATERIAL";
            numericBox4.Value = 0;
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
                dateBox1.ReadOnly = true;
                comboBox1.ReadOnly = true;
                comboBox2.ReadOnly = true;
                txtsubcon1.TextBox1.ReadOnly = true;
                txtsubcon1.TextBox1.Enabled = false;
                txtpayterm_fty1.TextBox1.ReadOnly = true;
                txtpayterm_fty1.TextBox1.Enabled = false;
                textBox2.ReadOnly = true;
                numericBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                txtuser1.TextBox1.ReadOnly = true;
                txtuser1.TextBox1.Enabled = false;
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
            txtsubcon1.TextBox1.ReadOnly = true;
            txtsubcon1.TextBox1.Enabled = false;
        }

        protected override void ClickUndo()
        {
            ChangeCombo2DataSource();
            txtsubcon1.TextBox1.Enabled = true;
            txtpayterm_fty1.TextBox1.Enabled = true;
            txtuser1.TextBox1.Enabled = true;
            base.ClickUndo();
        }

        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentMaintain["CDate"]))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                dateBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Type"]))
            {
                MyUtility.Msg.WarningBox("Type can't empty!!");
                comboBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["SubType"]))
            {
                MyUtility.Msg.WarningBox("Type can't empty!!");
                comboBox2.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["LocalSuppID"]))
            {
                MyUtility.Msg.WarningBox("Supplier can't empty!!");
                txtsubcon1.TextBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["PayTermID"]))
            {
                MyUtility.Msg.WarningBox("Terms can't empty!!");
                txtpayterm_fty1.TextBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["BLNo"]))
            {
                MyUtility.Msg.WarningBox("B/L No. can't empty!!");
                textBox3.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Handle"]))
            {
                MyUtility.Msg.WarningBox("Handle can't empty!!");
                txtuser1.TextBox1.Focus();
                return false;
            }
            #endregion

            //InvNo + B/L No不可以重複建立
            if (!MyUtility.Check.Empty(CurrentMaintain["InvNo"]))
            {
                DataRow dr;
                if (MyUtility.Check.Seek(string.Format("select ID from ShippingAp where InvNo = '{0}' and BLNo = '{1}' and ID != '{2}'", CurrentMaintain["InvNo"].ToString(), CurrentMaintain["Handle"].ToString(), IsDetailInserting ? "" : CurrentMaintain["ID"].ToString()), out dr))
                {
                    MyUtility.Msg.WarningBox("< Invoice# > and < B/L No. > duplicate with No."+ dr["ID"].ToString());
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
                detailAmt = detailAmt + Convert.ToDecimal(dr["Amount"]);
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
                if (MyUtility.Check.Seek(string.Format("select Type,SubType from ShippingAP where ID = '{0}'", CurrentMaintain["ID"].ToString()), out seekRow))
                {
                    if (CurrentMaintain["Type"].ToString() != seekRow["Type"].ToString())
                    {
                        DialogResult buttonResult = MyUtility.Msg.WarningBox(string.Format("Already have expense data. Are you sure want to change the type from '{0}' to '{1}'", seekRow["Type"].ToString(), CurrentMaintain["Type"].ToString()), "Warning", MessageBoxButtons.YesNo);
                        if (buttonResult == System.Windows.Forms.DialogResult.No)
                        {
                            return false;
                        }
                        else
                        {
                            haveEditShareFee = true;
                        }
                    }
                    if (!haveEditShareFee && CurrentMaintain["SubType"].ToString() != seekRow["SubType"].ToString())
                    {
                        DialogResult buttonResult = MyUtility.Msg.WarningBox(string.Format("Already have expense data. Are you sure want to change the type from '{0}' to '{1}'", seekRow["SubType"].ToString(), CurrentMaintain["SubType"].ToString()), "Warning", MessageBoxButtons.YesNo);
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
            int exact = Convert.ToInt32(MyUtility.GetValue.Lookup("Exact", CurrentMaintain["CurrencyID"].ToString(), "Currency", "ID"));
            CurrentMaintain["Amount"] = MyUtility.Math.Round(detailAmt, exact);
            CurrentMaintain["VAT"] = Convert.ToDecimal(CurrentMaintain["VATRate"]) > 0 ? MyUtility.Math.Round(MyUtility.Math.Round(detailAmt, exact) / Convert.ToDecimal(CurrentMaintain["VATRate"]) * 100, exact) : 0;

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

        protected override bool ClickSavePre()
        {
            if (haveEditShareFee)
            {
                DualResult result = DBProxy.Current.Execute(null,string.Format("delete form ShareExpense where ShippingAPID = '{0}'",CurrentMaintain["ID"].ToString()));
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Delete ShareExpense fail\n\r"+result.ToString());
                    return false;
                }
            }
            return base.ClickSavePre();
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            txtsubcon1.TextBox1.Enabled = true;
            txtpayterm_fty1.TextBox1.Enabled = true;
            txtuser1.TextBox1.Enabled = true;
            
            bool result = Prgs.CalculateShareExpense(CurrentMaintain["ID"].ToString());
            if (!result)
            {
                MyUtility.Msg.WarningBox("Re-calcute share expense failed, please retry 'Share expense Re-Calculate' later." );
            }
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

        protected override bool ClickDeletePre()
        {
            string sqlCmd = string.Format("delete from ShareExpense where ShippingAPID = '{0}'",CurrentMaintain["ID"].ToString());
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Delete ShareExpense false.\n\r"+result.ToString());
                return false;
            }
            return base.ClickDeletePre();
        }

        //Type
        private void comboBox1_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode == true && comboBox1.SelectedValue.ToString() != CurrentMaintain["Type"].ToString())
            {
                CurrentMaintain["Type"] = comboBox1.SelectedValue.ToString();
                if (comboBox1.SelectedIndex != -1)
                {
                    ChangeCombo2DataSource();
                }
                CurrentMaintain["SubType"] = "";
                comboBox2.SelectedIndex = -1;
            }
        }

        //設定Combo2的DataSource
        private void ChangeCombo2DataSource()
        {
            switch (CurrentMaintain["Type"].ToString())
            {
                case "IMPORT":
                    comboBox2.DataSource = comboxbs2_1;
                    break;
                case "EXPORT":
                    comboBox2.DataSource = comboxbs2_2;
                    break;
                default:
                    comboBox2.DataSource = comboxbs2_1;
                    break;
            }
        }

        //計算表身Grid欄位的Amount值
        private void CalculateGridAmount(DataRow dr)
        {
            decimal qty = MyUtility.Check.Empty(dr["Qty"]) ? 0 : Convert.ToDecimal(dr["Qty"]);
            decimal rate = MyUtility.Check.Empty(dr["Rate"]) ? 0 : Convert.ToDecimal(dr["Rate"]);
            dr["Amount"] = MyUtility.Math.Round(MyUtility.Math.Round(qty * Convert.ToDecimal(dr["Price"]), 2) * rate, 2);
        }

        //控制表身Grid欄位是否可被編輯
        private void DetailGridEditing(bool isEditing)
        {
            if (isEditing)
            {
                col_code.IsEditingReadOnly = false;
                col_qty.IsEditingReadOnly = false;
                col_rate.IsEditingReadOnly = false;
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
                col_remark.IsEditingReadOnly = true;
                for (int i = 0; i < detailgrid.ColumnCount; i++)
                {
                    detailgrid.Columns[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        //Share Expense
        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.P08_ShareExpense callNextForm = new Sci.Production.Shipping.P08_ShareExpense(CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        //Acct. Approve
        private void button2_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(MyUtility.GetValue.Lookup(string.Format("select Accountant from ShippingAP where ID = '{0}'", CurrentMaintain["ID"].ToString()))))
            {
                //Approve
                if (MyUtility.Check.Empty(CurrentMaintain["SubType"]) || (CurrentMaintain["SubType"].ToString() != "OTHER" && !MyUtility.Check.Seek(string.Format("select ShippingAPID from ShareExpense where ShippingAPID = '{0}'", CurrentMaintain["ID"].ToString()))))
                {
                    MyUtility.Msg.WarningBox("No share expense, can't approve!");
                    return;
                }
                string sqlCmd = string.Format("update ShippingAP set Accountant = '{0}', ApvDate = '{1}', Status = 'Approved' where ID = '{2}'",Sci.Env.User.UserID,DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),CurrentMaintain["ID"].ToString());
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

                string sqlCmd = string.Format("update ShippingAP set Accountant = '', ApvDate = null, Status = 'New' where ID = '{0}'", CurrentMaintain["ID"].ToString());

                DualResult result = DBProxy.Current.Execute(null, sqlCmd);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("Unapprove fail !\r\n" + result.ToString());
                }

                RenewData();
                OnDetailEntered();
            }
        }
    }
}
