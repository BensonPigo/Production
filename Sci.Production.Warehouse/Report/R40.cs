using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class R40 : Sci.Win.Tems.PrintForm
    {
        public R40(ToolStripMenuItem menuitem) 
            :base(menuitem)
        {
            InitializeComponent();
        }

        protected override bool ValidateInput()
        {
            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return base.OnAsyncDataLoad(e);
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            return base.OnToExcel(report);
        }
    }
}
