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

namespace Sci.Production.PPIC
{
    public partial class P01_FactoryCMT : Sci.Win.Subs.Base
    {
        private DataRow orderData;

        public P01_FactoryCMT(DataRow OrderData)
        {
            InitializeComponent();
            orderData = OrderData;
            this.Text = "Factory CMT (" + orderData["ID"].ToString() + ")";
            label3.Text = "Sub Process\r\nStd. Cost";
            label4.Text = "Local Purchase\r\nStd. Cost";
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable GridData;
            string sqlCmd = string.Format(@"select ot.Seq,ot.ArtworkTypeID,ot.Qty,ot.ArtworkUnit,ot.TMS,ot.Price,iif(a.IsTtlTMS = 1,'Y','N') as ttlTMS,a.Classify
from Order_TmsCost ot
left join ArtworkType a on ot.ArtworkTypeID = a.ID
where ot.ID = '{0}' 
and (a.Classify = 'I' or a.Classify = 'A' or a.Classify = 'P')
order by ot.Seq", orderData["ID"].ToString());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out GridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query order tmscost data fail!!" + result.ToString());
            }
            listControlBindingSource1.DataSource = GridData;
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("Seq", header: "Seq#", width: Widths.AnsiChars(4))
                .Text("ArtworkTypeID", header: "Artwork Type", width: Widths.AnsiChars(20))
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6))
                .Text("ArtworkUnit", header: "Unit", width: Widths.AnsiChars(10))
                .Numeric("TMS", header: "Tms", width: Widths.AnsiChars(6))
                .Numeric("Price", header: "Price", decimal_places: 3, width: Widths.AnsiChars(6))
                .Text("ttlTMS", header: "Ttl TMS", width: Widths.AnsiChars(1));

            numericBox1.Value = Convert.ToDecimal(orderData["CPU"]);
            numericBox2.Value = 0;
            numericBox3.Value = Convert.ToDecimal(GridData.Compute("sum(Price)", "Classify = 'I' and ttlTMS = 'N'")) + Convert.ToDecimal(GridData.Compute("sum(Price)", "Classify = 'A'"));
            if (MyUtility.GetValue.Lookup(string.Format("select LocalCMT from Factory where ID = '{0}'", orderData["FactoryID"].ToString())).ToUpper() == "TRUE")
            {
                numericBox4.Value = Convert.ToDecimal(GridData.Compute("sum(Price)", "Classify = 'P'"));
            }
            else
            {
                numericBox4.Value = 0;
            }
            numericBox5.Value = MyUtility.Math.Round(Convert.ToDecimal(numericBox1.Value + numericBox2.Value + numericBox3.Value + numericBox4.Value), 2);
        }
    }
}
