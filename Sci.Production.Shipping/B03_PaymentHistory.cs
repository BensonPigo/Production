using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;
using Ict;
using Ict.Win;

namespace Sci.Production.Shipping
{
    public partial class B03_PaymentHistory : Sci.Win.Subs.Base
    {
        protected DataRow motherData;
        public B03_PaymentHistory(DataRow data)
        {
            InitializeComponent();
            this.motherData = data;
            this.displayBox1.Text = MyUtility.Convert.GetString(motherData["ID"]);
            this.editBox1.Text = MyUtility.Convert.GetString(motherData["Description"]);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            string selectCommand = string.Format("select a.CDate, a.ID, b.Qty, b.CurrencyID, b.Price,b.Rate, b.Amount, c.Abb from ShippingAP a, ShippingAP_Detail b, LocalSupp c where a.ID = b.ID and b.ShipExpenseID = '{0}' and a.LocalSuppID = c.ID order by a.CDate", MyUtility.Convert.GetString(motherData["ID"]));
            DataTable selectDataTable;
            DualResult selectResult = DBProxy.Current.Select(null, selectCommand, out selectDataTable);
            bindingSource1.DataSource = selectDataTable;

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Date("Cdate",header:"A/P Date")
                 .Text("ID", header: "AP#", width: Widths.AnsiChars(13))
                 .Numeric("Qty",header:"Q'ty",decimal_places:4)
                 .Text("CurrencyID", header: "Currency", width: Widths.AnsiChars(3))
                 .Numeric("Price", header: "Price", decimal_places: 4)
                 .Numeric("Rate", header: "Rate", decimal_places: 6)
                 .Numeric("Amount", header: "Amount", decimal_places: 4)
                 .Text("Abb", header: "Supplier - Abb", width: Widths.AnsiChars(3));
        }
    }
}
