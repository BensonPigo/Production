using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Data;
using Ict;
using Sci.Win;
using Sci;

namespace Sci.Production.Subcon
{
    public partial class P01_ModifyPoQty : Sci.Win.Subs.Input6A
    {
        protected int sum_order_qty=0;
        public P01_ModifyPoQty()
        {
            InitializeComponent();
        }

        protected override void OnAttaching(DataRow data)
        {
            base.OnAttaching(data);
            DataTable tmpdt;
            Sci.Data.DBProxy.Current.Select(null,string.Format( "select sum(qty) from order_qty where id = '{0}'", data["orderid"]), out tmpdt);
            if (tmpdt.Rows[0][0] == null)
            { this.displayBox8.Text = ""; }
            else
            { sum_order_qty = int.Parse(tmpdt.Rows[0][0].ToString()); this.displayBox8.Text = tmpdt.Rows[0][0].ToString(); }
        }
        
        protected override void OnAcceptChanging(DataRow data)
        {
            base.OnAcceptChanging(data);
            

        }



        private void numericBox1_Validating(object sender, CancelEventArgs e)
        {
            if (!(CurrentData["apqty"] == DBNull.Value) && ((Sci.Win.UI.NumericBox)sender).Value < int.Parse(CurrentData["apqty"].ToString()))
            {
                MessageBox.Show("PO Qty can't less than AP Qty", "Wanring");
                e.Cancel = true;
                return;
            }
        }

        private void numericBox1_Validated(object sender, EventArgs e)
        {
            if (((Sci.Win.UI.NumericBox)sender).Value > sum_order_qty)
            {
                CurrentData["exceedqty"] = ((Sci.Win.UI.NumericBox)sender).Value - sum_order_qty;
            }
            else
            {
                CurrentData["exceedqty"] = 0;
            }
        }

        private void numericBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
