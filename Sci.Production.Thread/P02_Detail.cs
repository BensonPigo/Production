using Ict.Win;
using System;
using System.Data;

namespace Sci.Production.Thread
{
    /// <summary>
    /// P02_Detail
    /// </summary>
    public partial class P02_Detail : Sci.Win.Subs.Input8A
    {
        /// <summary>
        /// P02_Detail
        /// </summary>
        public P02_Detail()
        {
            this.InitializeComponent();
        }

        private void P02_Detail_Load(object sender, EventArgs e)
        {
            this.Cal();
        }

        private void Cal()
        {
            decimal a = 0;
            foreach (DataRow dr in this.CurrentSubDetailDatas.Rows)
            {
                a += Convert.ToDecimal(dr["TotalLength"]);
            }

            this.txtTotalLength.Text = a.ToString();
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            #region set grid
            // this.grid.ReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.grid)
           .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
           .Text("ThreadCombid", header: "Thread Comb.", width: Widths.AnsiChars(10), iseditingreadonly: true)
           .Text("Threadcombdesc", header: "Thread Comb Desc", width: Widths.AnsiChars(15), iseditingreadonly: true)
           .Text("Operationid", header: "Operationid", width: Widths.AnsiChars(20), iseditingreadonly: true)
           .Numeric("Seamlength", header: "TTL Seam Length", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
           .Text("SEQ", header: "Thread Location SEQ", width: Widths.AnsiChars(2), iseditingreadonly: true)
           .Text("ThreadLocationid", header: "Thread Location", width: Widths.AnsiChars(6), iseditingreadonly: true)
           .Text("UseRatio", header: "UseRatio", width: Widths.AnsiChars(15), iseditingreadonly: true)
           .Numeric("Allowance", header: "Start End Loss", width: Widths.AnsiChars(7), integer_places: 7, iseditingreadonly: true)
           .Numeric("UseLength", header: "Use Length", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 2, iseditingreadonly: true)
           .Numeric("OrderQty", header: "Order Qty", width: Widths.AnsiChars(7), integer_places: 7, iseditingreadonly: true)
           .Numeric("TotalLength", header: "Total Length", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 2, iseditingreadonly: true);
            #endregion
            return true;
        }
    }
}
