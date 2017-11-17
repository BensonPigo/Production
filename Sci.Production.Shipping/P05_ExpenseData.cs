using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci;

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
        public P05_ExpenseData(string iD, string columnName)
        {
            this.InitializeComponent();
            this.id = iD;
            this.columnName = columnName;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            switch (this.columnName)
            {
                case "InvNo":
                    this.sqlCmd = string.Format(
                        @"select isnull(a.Name,'') as Type,se.CurrencyID,se.Amount,se.ShippingAPID
from ShareExpense se WITH (NOLOCK) 
LEFT JOIN FinanceEN.DBO.AccountNo a on se.AccountID = a.ID
where se.InvNo = '{0}'", this.id);
                    break;
                case "WKNo":
                    this.sqlCmd = string.Format(
                        @"select isnull(a.Name,'') as Type,se.CurrencyID,se.Amount,se.ShippingAPID
from ShareExpense se WITH (NOLOCK) 
LEFT JOIN FinanceEN.DBO.AccountNo a on se.AccountID = a.ID
where se.WKNo = '{0}'", this.id);
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
            }

            // Grid設定
            this.gridExpenseData.IsEditingReadOnly = true;
            this.gridExpenseData.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridExpenseData)
                .Text("Type", header: "Type", width: Widths.AnsiChars(33))
                .Text("CurrencyID", header: "Currency", width: Widths.AnsiChars(3))
                .Numeric("Amount", header: "Expense", decimal_places: 2)
                .Text("ShippingAPID", header: "A/P No.", width: Widths.AnsiChars(15));
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
