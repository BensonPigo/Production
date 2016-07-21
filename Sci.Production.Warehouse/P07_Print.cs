using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P07_Print : Sci.Win.Tems.PrintForm
    {
        public P07_Print()
        {
            InitializeComponent();
        }
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
          
            
            
            return Result.True;
        }
    }
}
