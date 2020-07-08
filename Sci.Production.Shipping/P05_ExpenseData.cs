using System;
using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P05_ExpenseData
    /// </summary>
    public partial class P05_ExpenseData : Sci.Win.Subs.Base
    {
        private string id;
        private string columnName;
        private string sqlCmd;
        private DualResult result;
        private DataTable gridData;

        /// <summary>
        /// P05_ExpenseData
        /// </summary>
        /// <param name="iD">iD</param>
        /// <param name="columnName">columnName</param>
        /// <param name="ed">ed</param>
        public P05_ExpenseData(string iD, string columnName, bool ed)
        {
            this.InitializeComponent();
            this.id = iD;
            this.columnName = columnName;
            this.btnEdit.Visible = ed;
            this.EditMode = false;
            this.gridExpenseData.IsEditingReadOnly = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // Grid設定
            this.gridExpenseData.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridExpenseData)
            .Text("Type", header: "Type", width: Widths.AnsiChars(33), iseditingreadonly: true)
            .MaskedText("AccountID", "0000-0000", header: "Account No", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("CurrencyID", header: "Currency", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Numeric("Amount", header: "Expense", decimal_places: 2, iseditingreadonly: true)
            .Text("ShippingAPID", header: "A/P No.", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("DebitID", header: "SD/ICR/DB #", width: Widths.AnsiChars(15))
            ;
            this.Query();
        }

        private void Query()
        {
            switch (this.columnName)
            {
                case "InvNo":
                    this.sqlCmd = string.Format(
                        @"select isnull(a.Name,'') as Type,se.CurrencyID,se.Amount,se.DebitID,se.ShippingAPID,se.BLNo,se.WKNo,se.InvNo,se.AccountID
from ShareExpense se WITH (NOLOCK) 
LEFT JOIN dbo.SciFMS_AccountNo a on se.AccountID = a.ID
where se.InvNo = '{0}' and se.junk=0", this.id);
                    break;
                case "WKNo":
                    this.sqlCmd = string.Format(
                        @"select isnull(a.Name,'') as Type,se.CurrencyID,se.Amount,se.DebitID,se.ShippingAPID,se.BLNo,se.WKNo,se.InvNo,se.AccountID
from ShareExpense se WITH (NOLOCK) 
LEFT JOIN dbo.SciFMS_AccountNo a on se.AccountID = a.ID
where se.WKNo = '{0}' and se.junk=0", this.id);
                    break;
                default:
                    this.sqlCmd = "select Type,CurrencyID,Amount,ShippingAPID from ShareExpense WITH (NOLOCK) where 1=2";
                    break;
            }

            if (this.result = DBProxy.Current.Select(null, this.sqlCmd, out this.gridData))
            {
                this.listControlBindingSource1.DataSource = this.gridData;
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query data fail!!");
                this.btnEdit.Enabled = false;
            }

            if (this.gridData.Rows.Count == 0)
            {
                this.btnEdit.Enabled = false;
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                if (this.gridExpenseData.DataSource == null)
                {
                    return;
                }

                // SD開頭, 且13碼, 才能SAVE成功，不過空白要放行
                bool checkResult = true;
                for (int i = 0; i <= this.gridExpenseData.Rows.Count - 1; i++)
                {
                    string DebitNote = this.gridExpenseData.Rows[i].Cells[5].Value.ToString();
                    if (MyUtility.Check.Empty(DebitNote))
                    {
                        continue;
                    }

                    if (DebitNote.Length != 13 || !(DebitNote.StartsWith("SD") || DebitNote.StartsWith("IC") || DebitNote.StartsWith("DB")))
                    {
                        checkResult = false;
                    }
                }

                if (!checkResult)
                {
                    MyUtility.Msg.WarningBox("Debit Note is not correct, please check again!");
                    return;
                }

                string sqlMerge = $@"
merge ShareExpense t
using(select * from #tmp)s
on s.ShippingAPID = t.ShippingAPID and s.BLNo =t.BLNo and s.WKNo =t.WKNo and s.InvNo = t.InvNo and s.AccountID = t.AccountID
when matched then update set
	t.DebitID = s.DebitID,
    t.EditName='{Sci.Env.User.UserID}',
    t.EditDate=GETDATE()
;
";
                DataTable dt;
                DualResult result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.listControlBindingSource1.DataSource, string.Empty, sqlMerge, out dt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                this.Query();
            }

            this.EditMode = !this.EditMode;
            this.btnEdit.Text = this.EditMode ? "Save" : "Edit";
            this.gridExpenseData.IsEditingReadOnly = !this.EditMode;
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            // this.Close();
        }
    }
}
