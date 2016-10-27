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
    public partial class P01_Operation : Sci.Win.Subs.Input8A
    {
        public P01_Operation()
        {
            InitializeComponent();
        }
        protected override bool OnGridSetup()
        {
            #region set grid
            Helper.Controls.Grid.Generator(this.grid)
           .Text("Operationid", header: "Operationid", width: Widths.AnsiChars(20), iseditingreadonly: true)
           .Text("DescEN", header: "Thread Comb Desc", width: Widths.AnsiChars(15), iseditingreadonly: true)
           .Numeric("Seamlength", header: "Seam Length", width: Widths.AnsiChars(6), integer_places: 6, decimal_places: 2, iseditingreadonly: true);
            #endregion
            return true;
        }
    }
}
