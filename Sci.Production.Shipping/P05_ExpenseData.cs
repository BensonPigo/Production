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
    public partial class P05_ExpenseData : Sci.Win.Subs.Base
    {
        private string id,columnName,sqlCmd;
        private DualResult result;
        private DataTable gridData;
        public P05_ExpenseData(string ID, string ColumnName)
        {
            InitializeComponent();
            id = ID;
            columnName = ColumnName;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            switch (columnName)
            {
                case "InvNo":
                    sqlCmd = string.Format(@"select Type,CurrencyID,Amount,ShippingAPID
from ShareExpense
where InvNo = '{0}'", id);
                    break;
                case "WKNo":
                    sqlCmd = string.Format(@"select Type,CurrencyID,Amount,ShippingAPID
from ShareExpense
where WKNo = '{0}'", id);
                    break;
                default:
                    sqlCmd = "select Type,CurrencyID,Amount,ShippingAPID from ShareExpense where 1=2";
                    break;
            }
            if (result = DBProxy.Current.Select(null, sqlCmd, out gridData))
            {
                listControlBindingSource1.DataSource = gridData;
            }
            else
            {
                MyUtility.Msg.ErrorBox("Query data fail!!");
            }

            //Grid設定
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("Type", header: "Type", width: Widths.AnsiChars(13))
                .Text("CurrencyID", header: "Currency", width: Widths.AnsiChars(3))
                .Numeric("Amount",header: "Expense",decimal_places: 2)
                .Text("ShippingAPID", header: "A/P No.", width: Widths.AnsiChars(13));

        }

        //Close
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
