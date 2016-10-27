using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Thread
{
    public partial class P02_Detail : Sci.Win.Subs.Input8A
    {
        public P02_Detail()
        {
            InitializeComponent();
        }
        
        private void P02_Detail_Load(object sender, EventArgs e)
        {
            Cal();
        }

        private void Cal()
        {
            decimal a=0;
            foreach (DataRow dr in CurrentSubDetailDatas.Rows)
            {
                a += Convert.ToDecimal(dr["TotalLength"]);
            }
            textBoxTotalLength.Text = a.ToString();        
        }

        protected override bool OnGridSetup()
        {
            #region set grid
            //this.grid.ReadOnly = true;
            Helper.Controls.Grid.Generator(this.grid)
           .Text("Article", header: "Article", width: Widths.AnsiChars(6))
           .Text("ThreadCombid", header: "Thread Comb.", width: Widths.AnsiChars(10), iseditingreadonly: true)
           .Text("Threadcombdesc", header: "Thread Comb Desc", width: Widths.AnsiChars(15), iseditingreadonly: true)
           .Text("Operationid", header: "Operationid", width: Widths.AnsiChars(20), iseditingreadonly: true)
           .Numeric("Seamlength", header: "Seam Length", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
           .Text("SEQ", header: "Thread Location SEQ", width: Widths.AnsiChars(2), iseditingreadonly: true)
           .Text("ThreadLocationid", header: "Thread Location", width: Widths.AnsiChars(6), iseditingreadonly: true)
           .Text("UseRatioNumeric", header: "UseRatio", width: Widths.AnsiChars(15), iseditingreadonly: true)
           .Numeric("Allowance", header: "Allowance", width: Widths.AnsiChars(7), integer_places: 7, iseditingreadonly: true)
           .Numeric("UseLength", header: "Use Length", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
           .Numeric("OrderQty", header: "Order Qty", width: Widths.AnsiChars(7), integer_places: 7, iseditingreadonly: true)
           .Numeric("TotalLength", header: "Total Length", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 2, iseditingreadonly: true);
            #endregion
            return true;
        }

    }
}
